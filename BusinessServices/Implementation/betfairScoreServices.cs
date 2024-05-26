using BusinessServices.Cache;
using BusinessServices.Interface;
using Microsoft.Extensions.Configuration;
using Modal;
using Modal.ConcurrentDic;
using Modal.onlyscore;
using Modal.Request;
using Modal.Response;
using Modal.Response.Commentary;
using Newtonsoft.Json;
using Repository.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static Modal.Enums;

namespace BusinessServices.Implementation
{
    public class betfairScoreServices : IbetfairScoreServices
    {
        private IConfiguration _IConfiguration;
        private ICommentaryRepository _ICommentaryRepository;
        private IScoreListServices _ScoreListServices;
        private static string strAccessToken = "";
        public static int ibetfairScorekey = 9999;
        //public static int ibfrateScorekey = 9998;
        private static bool isActiveOnlyscore = false;
        private static bool IsDirectEventInfoAPICall = false;
        private static string _API = "";
        private static int iDefaultProviderID = Convert.ToInt32(Enum_ProviderType.OnlyScore);

        private static string strDifferentLinkIDWiseScore = "";
        public static ConcurrentDictionary<int, ScoreLink> _lstScore;
        public betfairScoreServices(IConfiguration IConfiguration, ICommentaryRepository ICommentaryRepository, IScoreListServices scoreListServices)
        {
            _IConfiguration = IConfiguration;
            _ICommentaryRepository = ICommentaryRepository;


            strDifferentLinkIDWiseScore = _IConfiguration["betfair:DifferentLinkIDWiseScore"];
            isActiveOnlyscore = Convert.ToBoolean(_IConfiguration["onlyscore:isActive"].ToString().Trim());
            IsDirectEventInfoAPICall = Convert.ToBoolean(_IConfiguration["onlyscore:IsDirectEventInfoAPICall"].ToString());
            _API = _IConfiguration["onlyscore:API"].ToString().Trim() + _IConfiguration["onlyscore:ScoreEndpoint"].ToString().Trim();
            iDefaultProviderID = Convert.ToInt16(_IConfiguration["DefaultScoreSourceID"].ToString().Trim());
            if (_lstScore == null)
            {
                _lstScore = new ConcurrentDictionary<int, ScoreLink>();
            }
            _ScoreListServices = scoreListServices;
        }

        #region Concurrent  Dictionary   
        public async Task<string> clearEventScore(string strEventID)
        {
            if (strEventID != "")
            {
                int iEventID = Convert.ToInt32(strEventID);
                if (_lstScore.ContainsKey(iEventID))
                {
                    var vMarkets = _lstScore[iEventID];
                    _lstScore.TryRemove(iEventID, out vMarkets);
                }
            }
            else
            {
                _lstScore = null;
                _lstScore = new ConcurrentDictionary<int, ScoreLink>();
            }
            return "Success Full clean";
        }
        public async Task<List<Scoreinfo>> getAllScorecard()
        {
            return _lstScore.Select(k => k.Value.Snap).ToList();
        }

        //public async Task<List<Scoreinfo>> getScoreEventList()
        //{
        //    return _lstScore.Select(k => k.Value).ToList();
        //}

        public async Task<string> getEventScoreUri(int iEventID, int iLinkID, int providertype)
        {
            /*
            var vinfo = new ScoreResponse();
            vinfo.EventID = iEventID;
            vinfo.ScoreUrl = "";
            vinfo.StreamingUrl = "";

            if (lstScore != null && lstScore.Count > 0)
            {
                if (lstScore.ContainsKey(iEventID))
                {
                    var vScore = lstScore[iEventID];
                    if (vScore.links.ContainsKey(iLinkID))
                    {
                        vinfo.ScoreUrl = vScore.links[iLinkID];
                    }
                }
            }
            return vinfo;
            */
            ScoreLink vEventInfo = _lstScore.Where(k => k.Key == iEventID).Select(k => k.Value).FirstOrDefault();
            if (vEventInfo != null)
            {
                var vProviderList = vEventInfo.Snap.links.Where(k => k.link == iLinkID && k.providerType == providertype).FirstOrDefault();
                if (vProviderList != null)
                {
                    //var vinfo = new ScoreResponse();
                    //vinfo.EventID = iEventID;
                    //vinfo.ScoreUrl = vProviderList.ScoreUrl;
                    //vinfo.StreamingUrl = vProviderList.StreamingUrl;

                    return vProviderList.ScoreUrl;
                }
            }
            return "";
        }
        public async void AddScoreCard(int iEventID, string strScoreUrl, string strStreamingUrl, int iLinkID, int providertype)
        {
            try
            {
                ScoreLink score = _lstScore.GetOrAdd(iEventID, id => new ScoreLink(this, id));
                await score.OnAddScoreLink(iLinkID, providertype, strScoreUrl, strStreamingUrl);
                /*
                if (lstScore.ContainsKey(iEventID))
                {
                    var vScore = lstScore[iEventID];
                    lock (vScore)
                    {
                        //  vScore.ScoreUrl = strScoreUrl;
                        // vScore.StreamingUrl = strStreamingUrl;

                        if (vScore.links.ContainsKey(iLinkID))
                        {
                            vScore.links[iLinkID] = strScoreUrl;
                        }
                        else
                        {
                            vScore.links.Add(iLinkID, strScoreUrl);
                        }

                        lstScore.TryUpdate(iEventID, vScore, vScore);
                    }
                }
                else
                {
                    Dictionary<int, string> vLinks = new Dictionary<int, string>();
                    vLinks.Add(iLinkID, strScoreUrl);

                    var vScore = new Scoreinfo();
                    vScore.EventID = iEventID;
                    //  vScore.ScoreUrl = strScoreUrl;
                    //vScore.StreamingUrl = strStreamingUrl;
                    vScore.providerType = providertype;
                    vScore.links = vLinks;
                    lstScore.TryAdd(iEventID, vScore);
                }
                */
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        /*#region bfrateScore
        public async Task getbfrateToken()
        {
            try
            {
                string _AgentCode = _IConfiguration["bfrate:Agentcode"].ToString().Trim();
                string _SecretKey = _IConfiguration["bfrate:SecretKey"].ToString().Trim();
                string _API = _IConfiguration["bfrate:API"].ToString().Trim();
                string _tokenEndpoint = _IConfiguration["bfrate:tokenEndpoint"].ToString().Trim();
                string strAPI = _API + _tokenEndpoint;

                using (HttpClient vclient = new HttpClient())
                {
                    TokenRequest _Request = new TokenRequest { agentcode = _AgentCode, secretkey = _SecretKey };
                    vclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpContent _Content = new StringContent(JsonConvert.SerializeObject(_Request), Encoding.UTF8, "application/json");

                    var vResult = vclient.PostAsync(strAPI, _Content).Result;
                    if (vResult.IsSuccessStatusCode)
                    {
                        var content = await vResult.Content.ReadAsStringAsync();
                        var vResponse = JsonConvert.DeserializeObject<getToken>(content);
                        strAccessToken = vResponse.token;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion
        */
        public async Task<object> getbetfairScore(string strEventIDs)
        {
            try
            {
                string connString = _IConfiguration["betfair:betfairScore"];
                string url = connString.Replace("{eventid}", strEventIDs);
                using (HttpClient vclient = new HttpClient())
                {
                    var vResult = await vclient.GetAsync(url);
                    if (vResult.IsSuccessStatusCode)
                    {
                        var content = await vResult.Content.ReadAsStringAsync();
                        var vResponse = JsonConvert.DeserializeObject<object>(content);
                        return vResponse;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return null;
        }
        public async Task<ScoreResponse> getScoreUri(string strEventID, int iLinkId = 0, string strcolor = "", string strfont = "", int iProviderId = 0)
        {
            bool isColorTagadd = true;
            ScoreResponse vScoreCardInfo = new ScoreResponse();
            try
            {
                Int32 iEventID = Convert.ToInt32(strEventID);
                int _ProviderID = iDefaultProviderID;
                if (iProviderId != 0)
                {
                    _ProviderID = iProviderId;
                }
                vScoreCardInfo.EventID = iEventID;
                vScoreCardInfo.ScoreUrl = await GetOrSetScoreLink(iEventID, iLinkId, _ProviderID);

                if (String.IsNullOrEmpty(vScoreCardInfo.ScoreUrl))
                {
                    isColorTagadd = true;
                    _ProviderID = Convert.ToInt32(Enum_ProviderType.BetFair);
                    int itempLinkID = ibetfairScorekey;
                    string strLinkIDForComapre = "~" + iLinkId.ToString() + "~";
                    if ((!string.IsNullOrEmpty(strDifferentLinkIDWiseScore)) && strDifferentLinkIDWiseScore.Contains(strLinkIDForComapre))
                    {
                        itempLinkID = iLinkId;
                    }
                    vScoreCardInfo.ScoreUrl = await getEventScoreUri(iEventID, itempLinkID, _ProviderID);
                    if (String.IsNullOrEmpty(vScoreCardInfo.ScoreUrl))
                    {
                        string connString = _IConfiguration["betfair:betfairScore"];
                        string url = connString.Replace("{eventid}", strEventID);
                        using (HttpClient vclient = new HttpClient())
                        {
                            var vResult = await vclient.GetAsync(url);
                            if (vResult.IsSuccessStatusCode)
                            {
                                var content = await vResult.Content.ReadAsStringAsync();
                                if (content != "[]")
                                {
                                    var vResponse = JsonConvert.DeserializeObject<object>(content);
                                    if (vResponse != null)
                                    {
                                        string strScoreUrl = _IConfiguration["betfair:bfScoreAPI"] + strEventID;
                                        if ((!string.IsNullOrEmpty(strDifferentLinkIDWiseScore)) && strDifferentLinkIDWiseScore.Contains(strLinkIDForComapre))
                                        {
                                            string strKey = "bfScoreAPI" + iLinkId.ToString();
                                            string strUrl = _IConfiguration["betfair:bfScoreAPI" + iLinkId.ToString()];

                                            if (!string.IsNullOrEmpty(strUrl))
                                            {
                                                strScoreUrl = strUrl + strEventID;
                                            }
                                        }
                                        AddScoreCard(iEventID, strScoreUrl, "", itempLinkID, Convert.ToInt32(Enum_ProviderType.BetFair));
                                        vScoreCardInfo.ScoreUrl = strScoreUrl;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            if (vScoreCardInfo != null && !String.IsNullOrEmpty(vScoreCardInfo.ScoreUrl) && isColorTagadd)
            {
                if (!String.IsNullOrEmpty(strcolor))
                    vScoreCardInfo.ScoreUrl = vScoreCardInfo.ScoreUrl + "&color=" + strcolor;
                if (!String.IsNullOrEmpty(strfont))
                    vScoreCardInfo.ScoreUrl = vScoreCardInfo.ScoreUrl + "&font=" + strfont;
            }
            return vScoreCardInfo;
        }
        public async Task<ScoreResponse> getManulScoreofevent(string strEventID, int iLinkID = 0, string strcolor = "", string strfont = "", int iProviderId = 0)
        {
            ScoreResponse vScoreCardInfo = new ScoreResponse();
            try
            {
                Int32 iEventID = Convert.ToInt32(strEventID);
                int _ProviderID = iDefaultProviderID;
                if (iProviderId != 0)
                {
                    _ProviderID = iProviderId;
                }
                vScoreCardInfo.EventID = iEventID;
                vScoreCardInfo.ScoreUrl = await GetOrSetScoreLink(iEventID, iLinkID, _ProviderID);

            }
            catch (System.Exception ex)
            {
            }
            if (vScoreCardInfo != null && !String.IsNullOrEmpty(vScoreCardInfo.ScoreUrl))
            {
                if (!String.IsNullOrEmpty(strcolor))
                    vScoreCardInfo.ScoreUrl = vScoreCardInfo.ScoreUrl + "&color=" + strcolor;
                if (!String.IsNullOrEmpty(strfont))
                    vScoreCardInfo.ScoreUrl = vScoreCardInfo.ScoreUrl + "&font=" + strfont;
            }
            return vScoreCardInfo;
        }
        public async Task<IList<ScoreResponse>> getmanualScorelist(string strEventID, int iLinkID = 0, string strcolor = "", string strfont = "", int iProviderId = 0)
        {
            IList<ScoreResponse> vScorelist = new List<ScoreResponse>();
            string strgetDbValue = "";
            string[] strEventIDS = strEventID.Split(',');
            int _ProviderID = Convert.ToInt32(Enum_ProviderType.OnlyScore);
            if (iProviderId != 0)
            {
                _ProviderID = iProviderId;
            }
            foreach (var _EventID in strEventIDS)
            {
                string sttempEventid = _EventID;
                if (sttempEventid != "")
                {
                    try
                    {
                        Int32 iEventID = Convert.ToInt32(sttempEventid);
                        ScoreResponse vScoreinfo = new ScoreResponse();
                        vScoreinfo.EventID = iEventID;
                        vScoreinfo.ScoreUrl = await getEventScoreUri(iEventID, iLinkID, _ProviderID);
                        if (String.IsNullOrEmpty(vScoreinfo.ScoreUrl))
                        {
                            strgetDbValue += sttempEventid + ",";
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(vScoreinfo.ScoreUrl))
                            {
                                if (!String.IsNullOrEmpty(strcolor))
                                    vScoreinfo.ScoreUrl = vScoreinfo.ScoreUrl + "&color=" + strcolor;
                                if (!String.IsNullOrEmpty(strfont))
                                    vScoreinfo.ScoreUrl = vScoreinfo.ScoreUrl + "&font=" + strfont;
                            }
                            vScorelist.Add(vScoreinfo);
                        }
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
            }

            strgetDbValue = strgetDbValue.TrimEnd(',').Trim();
            if (strgetDbValue != "")
            {
                IEnumerable<Scoraboradlink> _Commentary = await _ICommentaryRepository.GetScoreLinks(strgetDbValue, iLinkID);
                if (_Commentary != null)
                {
                    foreach (var vRow in _Commentary)
                    {
                        if (vRow != null)
                        {
                            AddScoreCard(Convert.ToInt32(vRow.EventID), vRow.Scorelink, "", iLinkID, Convert.ToInt32(Enum_ProviderType.OnlyScore));
                            ScoreResponse _scoreinfo = new ScoreResponse();
                            _scoreinfo.EventID = Convert.ToInt32(vRow.EventID);
                            _scoreinfo.ScoreUrl = vRow.Scorelink;
                            _scoreinfo.StreamingUrl = "";

                            if (!String.IsNullOrEmpty(strcolor))
                                _scoreinfo.ScoreUrl = _scoreinfo.ScoreUrl + "&color=" + strcolor;
                            if (!String.IsNullOrEmpty(strfont))
                                _scoreinfo.ScoreUrl = _scoreinfo.ScoreUrl + "&font=" + strfont;

                            vScorelist.Add(_scoreinfo);
                        }
                    }
                }

            }
            return vScorelist;
        }
        public async Task<object> getCommentryEventlist()
        {
            try
            {
                bool isActive = Convert.ToBoolean(_IConfiguration["commentry:isActive"].ToString());
                if (isActive)
                {
                    string strAPI = _IConfiguration["commentry:API"].ToString().Trim();
                    if (!string.IsNullOrEmpty(strAPI))
                    {
                        using (HttpClient vclient = new HttpClient())
                        {
                            vclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            HttpContent _Content = new StringContent("", Encoding.UTF8, "application/json");

                            var vResult = vclient.PostAsync(strAPI, _Content).Result;
                            if (vResult.IsSuccessStatusCode)
                            {
                                var content = await vResult.Content.ReadAsStringAsync();
                                return JsonConvert.DeserializeObject<object>(content);
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return null;
        }
        public async Task<string> GetOrSetScoreLink(int iEventID, int iLinkID, int iProviderId)
        {
            string strScoreLink = await getEventScoreUri(iEventID, iLinkID, iProviderId);
            if (String.IsNullOrEmpty(strScoreLink))
            {
                if (iProviderId != Convert.ToInt32(Enum_ProviderType.OnlyScore))
                {
                    strScoreLink = await getEventScoreUri(iEventID, iLinkID, Convert.ToInt32(Enum_ProviderType.OnlyScore));
                }
                if (string.IsNullOrEmpty(strScoreLink))
                {
                    Scoraboradlink _Commentary = await _ICommentaryRepository.GetScoreLink(iEventID.ToString(), iLinkID);
                    if (_Commentary != null)
                    {
                        AddScoreCard(iEventID, _Commentary.Scorelink, "", iLinkID, Convert.ToInt32(Enum_ProviderType.OnlyScore));
                        strScoreLink = _Commentary.Scorelink;
                    }
                }

                if (string.IsNullOrEmpty(strScoreLink) || iProviderId == Convert.ToInt32(Enum_ProviderType.NewOnlyScore))
                {
                    string strtempid = "";
                    if (iProviderId != Convert.ToInt32(Enum_ProviderType.NewOnlyScore))
                    {
                        strtempid = await getEventScoreUri(iEventID, iLinkID, Convert.ToInt32(Enum_ProviderType.NewOnlyScore));
                    }
                    if (string.IsNullOrEmpty(strtempid) && isActiveOnlyscore)
                    {
                        try
                        {
                            if (IsDirectEventInfoAPICall)
                            {
                                if (!string.IsNullOrEmpty(_API))
                                {
                                    string strAPI = _API + iEventID.ToString();

                                    using (HttpClient vclient = new HttpClient())
                                    {
                                        var vResult = vclient.GetAsync(strAPI).Result;
                                        if (vResult.IsSuccessStatusCode)
                                        {
                                            var content = await vResult.Content.ReadAsStringAsync();
                                            getScoreEventInfo vresponse = JsonConvert.DeserializeObject<getScoreEventInfo>(content);
                                            if (vresponse != null && vresponse.success)
                                            {
                                                if (vresponse.result.es.eid == iEventID.ToString())
                                                {
                                                    strScoreLink = _IConfiguration["onlyscore:OnlyScoreDesign"].ToString().Trim() + iEventID.ToString();
                                                    if ((!string.IsNullOrEmpty(strDifferentLinkIDWiseScore)) && strDifferentLinkIDWiseScore.Contains(iLinkID.ToString()))
                                                    {
                                                        string strUrl = _IConfiguration["onlyscore:OnlyScoreDesign" + iLinkID.ToString()];
                                                        if (!string.IsNullOrEmpty(strUrl))
                                                        {
                                                            strScoreLink = strUrl+ iEventID.ToString();
                                                        }
                                                    }
                                                    AddScoreCard(iEventID, strScoreLink, "", iLinkID, Convert.ToInt32(Enum_ProviderType.NewOnlyScore));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                bool isExists =await _ScoreListServices.isEventExists(iEventID.ToString());
                                if(isExists)
                                {
                                    strScoreLink = _IConfiguration["onlyscore:OnlyScoreDesign"].ToString().Trim() + iEventID.ToString();
                                    if ((!string.IsNullOrEmpty(strDifferentLinkIDWiseScore)) && strDifferentLinkIDWiseScore.Contains(iLinkID.ToString()))
                                    {
                                        string strUrl = _IConfiguration["onlyscore:OnlyScoreDesign" + iLinkID.ToString()];
                                        if (!string.IsNullOrEmpty(strUrl))
                                        {
                                            strScoreLink = strUrl + iEventID.ToString();
                                        }
                                    }
                                    AddScoreCard(iEventID, strScoreLink, "", iLinkID, Convert.ToInt32(Enum_ProviderType.NewOnlyScore));
                                }
                            }
                        }
                        catch (WebException ex)
                        {
                           
                        }
                    }
                }
            }
            return strScoreLink;
        }
    }
}
