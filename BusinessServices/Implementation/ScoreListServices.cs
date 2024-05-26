using BusinessServices.Interface;
using Microsoft.Extensions.Configuration;
using Modal;
using Modal.onlyscore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Modal.Enums;

namespace BusinessServices.Implementation
{
    public class ScoreListServices : IScoreListServices
    {
        private IConfiguration _IConfiguration;
        private static int IGetEventListInterval = 60000;
        private static bool isActive = false;
        private static bool IsDirectEventInfoAPICall = false;
        private static string _API = "";
        private static IList<Es> _EventList;

        private BackgroundWorker bwGenral;
        public ScoreListServices(IConfiguration configuration)
        {

            _IConfiguration = configuration;
            isActive = Convert.ToBoolean(configuration["onlyscore:isActive"].ToString());
            IsDirectEventInfoAPICall = Convert.ToBoolean(configuration["onlyscore:IsDirectEventInfoAPICall"].ToString());
            IGetEventListInterval = Convert.ToInt32(configuration["onlyscore:Interval_ScoreList"].ToString());
            _API = _IConfiguration["onlyscore:API"].ToString().Trim() + _IConfiguration["onlyscore:ScoreListEndpoint"].ToString().Trim();

            if (_EventList == null)
            {
                _EventList = new List<Es>();
            }
            if (!IsDirectEventInfoAPICall && isActive && !string.IsNullOrEmpty(_API))
            {
                bwGenral = new BackgroundWorker();
                bwGenral.DoWork += new DoWorkEventHandler(bwGenral_DoWork);
                bwGenral.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwGenral_RunWorkerCompleted);
                bwGenral.WorkerReportsProgress = true;
                bwGenral.RunWorkerAsync();
            }
            //}
        }
        public static DateTime GetDateTime() => DateTime.UtcNow.AddHours(5.5);
        private async void bwGenral_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                do
                {
                    DateTime lstGetTime = GetDateTime();
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            //client.DefaultRequestHeaders.Add("Content-Type", "application/json");

                            // Make the POST request
                            HttpResponseMessage response = await client.PostAsync(_API, null);

                            // Check if the request was successful
                            if (response.IsSuccessStatusCode)
                            {
                                // Read and display the response body
                                var content = await response.Content.ReadAsStringAsync();
                                getScoreEventList vresponse = JsonConvert.DeserializeObject<getScoreEventList>(content);
                                if (vresponse != null && vresponse.success)
                                {
                                    _EventList = vresponse.result;
                                }
                            }
                            else
                            {
                                _EventList = new List<Es>();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    if (IGetEventListInterval != 0)
                    {
                        int iTotalMilliseconds = Convert.ToInt32((GetDateTime() - lstGetTime).TotalMilliseconds);
                        if (IGetEventListInterval - iTotalMilliseconds > 0)
                            System.Threading.Thread.Sleep(IGetEventListInterval - iTotalMilliseconds);
                    }
                }
                while (true);

            }
            catch (Exception ex)
            {
            }
        }

        private void bwGenral_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        public async Task<bool> isEventExists(string eventID)
        {
            bool isEventExists = false;
            if (_EventList != null && _EventList.Count > 0)
            {
                var vData = _EventList.Where(k => k.eid == eventID).Select(k=>k).FirstOrDefault();
                if (vData != null)
                {
                    isEventExists = true;
                }
            }
            return isEventExists;
        }
    }
}
