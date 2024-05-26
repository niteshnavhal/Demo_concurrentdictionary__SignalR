using Modal;
using Modal.betfairscore;
using Modal.CommanrtyScore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Interface
{
    public interface IScoreServices
    {


        Task<ConcurrentDictionary<int, EventScore>> getallScore();
        Task<EventScore> getScore(int iEventID);
        Task AddScore(int iEventID, int iEverttypeid);
        Task<EventScore> updateScore(int iEventID, string vResponse, object vScore, bfscore vbfScore, bool isFinished);
        Task updateScoreTimeline(int iEventID, object vScore, bool isFinished);
        Task<string> clearScore(string strEventID);

        Task<EventScore> getupdatebfScore(string strEventID);
        Task getupdatebfScoreTimeline(string strEventID);

        Task<object> getScoreobject(string strEventID);
        Task getScore(string strEventID);
        Task<object> getScoreTimeline(string strEventID);
        #region ConnectionID
        Task SubscribetEventtoSocket(string sEventID, string ConnectionID);
        Task RemoveSubscribetEventtoSocket(int key);
        Task<EventScoreShort> getConnectionID(int iEventID);
        Task<ConcurrentDictionary<int, EventScoreShort>> getallConnectionID();
        #endregion
    }
}
