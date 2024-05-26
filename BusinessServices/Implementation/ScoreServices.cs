using BusinessServices.Interface;
using Microsoft.Extensions.Configuration;
using Modal;
using Modal.betfairscore;
using Modal.CommanrtyScore;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Modal.Enums;

namespace BusinessServices.Implementation
{
    public class ScoreServices : IScoreServices
    {
        private static string strErrorHeader = "BussinessServices :: ScoreServices ";
        private IConfiguration _IConfiguration;
        public static ConcurrentDictionary<int, EventScore> lstbetfairScore = new ConcurrentDictionary<int, EventScore>();
        public static ConcurrentDictionary<int, EventScoreShort> lstConnectionIDS = new ConcurrentDictionary<int, EventScoreShort>();
        public static int iShowTimeUpdateinterval = 1000;
        public static int iCount = 0;
        public ScoreServices(IConfiguration IConfiguration)
        {
            _IConfiguration = IConfiguration;
            iShowTimeUpdateinterval = Convert.ToInt32(_IConfiguration["betfair:bfshowTimeupdateinterval"].ToString());
            iCount = 0;
        }
        public static DateTime GetDateTime() => DateTime.UtcNow.AddHours(5.5);

        #region "ConcurrentDic funcation"
        public async Task<ConcurrentDictionary<int, EventScore>> getallScore()
        {
            return new ConcurrentDictionary<int, EventScore>(lstbetfairScore);
        }
        public async Task<EventScore> getScore(int iEventID)
        {

            if (lstbetfairScore != null && lstbetfairScore.Count > 0)
            {

                if (lstbetfairScore.ContainsKey(iEventID))
                {
                    return lstbetfairScore[iEventID];
                }
            }
            return null;
        }
        public async Task AddScore(int iEventID, int iEverttypeid)
        {
            try
            {
                if (!lstbetfairScore.ContainsKey(iEventID))
                {
                    var vinfo = new EventScore();
                    vinfo.eventid = iEventID;
                    vinfo.eventtypeid = iEverttypeid;
                    vinfo.isfinished = false;
                    vinfo.showtime = GetDateTime();
                    lstbetfairScore.TryAdd(iEventID, vinfo);
                }
            }
            catch (Exception ex)
            {
            }
        }
        public async Task<EventScore> updateScore(int iEventID, string vResponse, object vScore, bfscore vbfScore, bool isFinished)
        {
            try
            {
                if (vbfScore != null)
                {
                    ShortScore _SScore = new ShortScore();
                    _SScore.eti = vbfScore.EventTypeId;
                    _SScore.eid = vbfScore.EventId.ToString();
                    try
                    {
                        if (_SScore.eti == Convert.ToInt32(Enum_SportsType.Soccer))
                        {
                            var bfSoccer = JsonConvert.DeserializeObject<List<bfSoccerModal>>(vResponse);
                            if (bfSoccer != null && bfSoccer.Count > 0)
                            {
                                var obj = bfSoccer[0];
                                _SScore.en = obj.score.home.name.ToString() + " v " + obj.score.away.name.ToString();
                                _SScore.t1s = obj.score.home.score.ToString();
                                _SScore.t2s = obj.score.away.score.ToString();
                                _SScore.te1n = obj.score.home.name.ToString();
                                _SScore.te2n = obj.score.away.name.ToString();
                                _SScore.pt = obj.timeElapsed;
                            }

                        }
                        else if (_SScore.eti == Convert.ToInt32(Enum_SportsType.Tennis))
                        {
                            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(vScore);
                            var bfTennis = JsonConvert.DeserializeObject<List<bfTennisModal>>(jsonString);

                            if (bfTennis != null && bfTennis.Count > 0)
                            {
                                var obj = bfTennis[0];
                                _SScore.en = obj.score.home.name.ToString() + " v " + obj.score.away.name.ToString();
                                _SScore.t1s = obj.score.home.games.ToString();
                                _SScore.t2s = obj.score.away.games.ToString();
                                _SScore.te1n = obj.score.home.name.ToString();
                                _SScore.te2n = obj.score.away.name.ToString();
                                _SScore.pt = obj.currentSet;

                                _SScore.t1set = obj.score.home.sets;
                                _SScore.t2set = obj.score.away.sets;
                                _SScore.t1p = obj.score.home.score;
                                _SScore.t2p = obj.score.away.score;
                            }
                        }
                        else
                        {
                            _SScore = null;
                        }
                    }
                    catch (Exception exss)
                    {
                        _SScore = null;
                    }


                    if (lstbetfairScore.ContainsKey(iEventID))
                    {
                        var vtemp = lstbetfairScore[iEventID];
                        if (vtemp != null)
                        {
                            lock (vtemp)
                            {
                                vtemp.score = vScore;
                                vtemp.isfinished = isFinished;
                                vtemp.shortscore = _SScore;
                                vtemp.showtime = GetDateTime();
                                lstbetfairScore.TryUpdate(iEventID, vtemp, vtemp);
                            }
                            return vtemp;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        public async Task updateScoreTimeline(int iEventID, object vScore, bool isFinished)
        {
            try
            {
                if (lstbetfairScore.ContainsKey(iEventID))
                {
                    var vtemp = lstbetfairScore[iEventID];
                    lock (vtemp)
                    {
                        vtemp.score = vScore;
                        vtemp.isfinished = isFinished;
                        lstbetfairScore.TryUpdate(iEventID, vtemp, vtemp);
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }
        public async Task<string> clearScore(string strEventID)
        {
            string strMsg = "Success";
            if (strEventID != "")
            {
                int iEventID = Convert.ToInt32(strEventID);
                if (lstbetfairScore.ContainsKey(iEventID))
                {
                    var _score = lstbetfairScore[iEventID];
                    lstbetfairScore.TryRemove(iEventID, out _score);
                    strMsg += " Event :" + strEventID;
                }
            }
            else
            {
                lstbetfairScore = null;
                lstbetfairScore = new ConcurrentDictionary<int, EventScore>();
                strMsg += " Full";
            }
            return strMsg + " clean";
        }
        public async Task updateshowtime(int iEventID)
        {
            try
            {
                if (lstbetfairScore.ContainsKey(iEventID))
                {
                    var vtemp = lstbetfairScore[iEventID];
                    lock (vtemp)
                    {
                        vtemp.showtime = GetDateTime();
                        lstbetfairScore.TryUpdate(iEventID, vtemp, vtemp);
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        public async Task<EventScore> getupdatebfScore(string strEventID)
        {
            EventScore Score = null;
            try
            {
                string connString = _IConfiguration["betfair:betfairScore"];
                string url = connString.Replace("{eventid}", strEventID);
                using (HttpClient vclient = new HttpClient())
                {
                    var vResult = await vclient.GetAsync(url);
                    if (vResult.IsSuccessStatusCode)
                    {
                        var content = await vResult.Content.ReadAsStringAsync();
                        if (content != "[]" && content != "")
                        {
                            var vResponse = JsonConvert.DeserializeObject<object>(content);
                            var vScore = JsonConvert.DeserializeObject<IList<bfscore>>(content);
                            if (vScore != null && vScore.Count > 0)
                            {
                                bool isCompleted = false;
                                var vData = vScore[0];
                                if (vData.MatchStatus != null && vData.MatchStatus != "")
                                {
                                    if (vData.MatchStatus.ToLower() == "finished")
                                        isCompleted = true;
                                }
                                Score = await updateScore(vData.EventId, content, vResponse, vData, isCompleted);
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return Score;
        }
        public async Task getupdatebfScoreTimeline(string strEventID)
        {
            try
            {
                string connString = _IConfiguration["betfair:betfairScoreTimeline"];
                string url = connString.Replace("{eventid}", strEventID);
                using (HttpClient vclient = new HttpClient())
                {
                    var vResult = await vclient.GetAsync(url);
                    if (vResult.IsSuccessStatusCode)
                    {
                        var content = await vResult.Content.ReadAsStringAsync();
                        if (content != "[]" && content != "")
                        {
                            var vResponse = JsonConvert.DeserializeObject<object>(content);
                            var vScore = JsonConvert.DeserializeObject<bfscoreTimeline>(content);
                            if (vScore != null)
                            {
                                bool isCompleted = false;

                                if (vScore.InPlayMatchStatus != null && vScore.InPlayMatchStatus != "")
                                {
                                    if (vScore.InPlayMatchStatus.ToLower() == "finished")
                                        isCompleted = true;
                                }

                                await updateScoreTimeline(vScore.EventId, vResponse, isCompleted);
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {

            }
        }
        public async Task<object> getScoreobject(string strEventID)
        {
            try
            {
                int iEventID = Convert.ToInt32(strEventID);
                var vScore = await getScore(iEventID);
                if (vScore == null)
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
                                var vResponse = JsonConvert.DeserializeObject<IList<bfscore>>(content);
                                if (vResponse != null && vResponse.Count > 0)
                                {
                                    var vData = vResponse[0];
                                    await AddScore(vData.EventId, vData.EventTypeId);
                                    return JsonConvert.DeserializeObject<object>(content);
                                }
                            }
                        }
                    }
                }
                else
                {
                    TimeSpan spDate = (GetDateTime() - vScore.showtime);
                    var iMilliseconds = spDate.TotalMilliseconds;
                    if (iMilliseconds >= iShowTimeUpdateinterval)
                    {
                        await updateshowtime(vScore.eventid);
                    }

                    return vScore.score;
                }
            }
            catch (System.Exception ex)
            {

            }
            return null;
        }
        public async Task getScore(string strEventID)
        {
            try
            {
                int iEventID = Convert.ToInt32(strEventID);
                var vScore = await getScore(iEventID);
                if (vScore == null)
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
                                var vResponse = JsonConvert.DeserializeObject<IList<bfscore>>(content);
                                if (vResponse != null && vResponse.Count > 0)
                                {
                                    var vData = vResponse[0];
                                    await AddScore(vData.EventId, vData.EventTypeId);
                                }
                            }
                        }
                    }
                }
                else
                {
                    TimeSpan spDate = (GetDateTime() - vScore.showtime);
                    var iMilliseconds = spDate.TotalMilliseconds;
                    if (iMilliseconds >= iShowTimeUpdateinterval)
                    {
                        await updateshowtime(vScore.eventid);
                    }
                }
            }
            catch (System.Exception ex)
            {

            }
        }
        public async Task<object> getScoreTimeline(string strEventID)
        {
            try
            {
                string connString = _IConfiguration["betfair:betfairScoreTimeline"];
                string url = connString.Replace("{eventid}", strEventID);
                using (HttpClient vclient = new HttpClient())
                {
                    var vResult = await vclient.GetAsync(url);
                    if (vResult.IsSuccessStatusCode)
                    {
                        var content = await vResult.Content.ReadAsStringAsync();
                        if (content != "[]" && content != "")
                        {
                            return JsonConvert.DeserializeObject<object>(content);
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {

            }
            return null;
        }

        #region ConnectionID
        public async Task SubscribetEventtoSocket(string sEventID, string ConnectionID)
        {
            try
            {
                iCount++;
                if (!lstConnectionIDS.ContainsKey(iCount))
                {
                    EventScoreShort eventScoreShort = new EventScoreShort();
                    eventScoreShort.eventids = sEventID;
                    eventScoreShort.connectionID = ConnectionID;
                    lstConnectionIDS.TryAdd(iCount, eventScoreShort);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public async Task RemoveSubscribetEventtoSocket(int key)
        {
            try
            {
                if (lstConnectionIDS.ContainsKey(key))
                {
                    EventScoreShort eventScoreShort;
                    lstConnectionIDS.TryRemove(key, out eventScoreShort);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public async Task<EventScoreShort> getConnectionID(int iEventID)
        {

            if (lstConnectionIDS != null && lstConnectionIDS.Count > 0)
            {

                if (lstConnectionIDS.ContainsKey(iEventID))
                {
                    return lstConnectionIDS[iEventID];
                }
            }
            return null;
        }

        public async Task<ConcurrentDictionary<int, EventScoreShort>> getallConnectionID()
        {
            return new ConcurrentDictionary<int, EventScoreShort>(lstConnectionIDS);
        }
        #endregion

    }
}
