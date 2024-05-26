using betfaireScore.Services;
using BusinessServices.Implementation;
using BusinessServices.Interface;
using Microsoft.AspNetCore.SignalR;
using Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace betfaireScore.Hubs
{
    public class bfScore : Hub
    {
        //private readonly getScore _getScore;
        //public bfScore(getScore sendScore)
        //{
        //    _getScore = sendScore;
        //}
        private readonly getScore _gs;
        private readonly IbetfairScoreServices _ibetfairScoreServices;
        public bfScore(getScore gs, IbetfairScoreServices ibetfairScoreServices)
        {
            _gs = gs;
            _ibetfairScoreServices = ibetfairScoreServices;
        }


        #region Core Methods
        public override async Task OnConnectedAsync()
        {
            try
            {
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
            }
        }

        #endregion
        #region Extended Methods
        public async Task getscore(string strEventIDs)
        {
            try
            {
                if (strEventIDs != "")
                {


                    strEventIDs = strEventIDs.Trim(',');
                    string[] objEventIds = strEventIDs.Split(',');
                    if (objEventIds.Length > 0)
                    {
                        for (int i = 0; i < objEventIds.Length; i++)
                        {
                            if (objEventIds[i].ToString() != "")
                            {
                                string strEventID = objEventIds[i];
                                await Groups.AddToGroupAsync(Context.ConnectionId, strEventID);
                                await _ibetfairScoreServices.getScoreUri(strEventID);
                                await _gs.getLiveEventScore(Context.ConnectionId, strEventIDs);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public async Task getShortScore(string strEventIDs)
        {
            try
            {
                if (strEventIDs != "")
                {

                    strEventIDs = strEventIDs.Trim(',');
                    string[] objEventIds = strEventIDs.Split(',');
                    if (objEventIds.Length > 0)
                    {
                        for (int i = 0; i < objEventIds.Length; i++)
                        {
                            if (objEventIds[i].ToString() != "")
                            {
                                string strEventID = objEventIds[i];
                                await _ibetfairScoreServices.getScoreUri(strEventID);
                            }
                        }
                    }

                    await Groups.AddToGroupAsync(Context.ConnectionId, common.ShortScoreGroupName);
                    await _gs.getLiveEventScore(Context.ConnectionId, strEventIDs);
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }
}