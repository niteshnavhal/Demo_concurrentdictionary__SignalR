using Modal;
using Modal.ConcurrentDic;
using Modal.Response;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Interface
{
    public interface IbetfairScoreServices
    {


        Task<List<Scoreinfo>> getAllScorecard();
        //Task<List<Scoreinfo>> getScoreEventList();
        Task<object> getbetfairScore(string strEventIDs);
        Task<ScoreResponse> getScoreUri(string strEventID, int iLinkId = 0, string strcolor = "", string strfont = "", int iProviderId = 0);
        Task<ScoreResponse> getManulScoreofevent(string strEventID, int iLinkID = 0, string strcolor = "", string strfont = "", int iProviderId = 0);
        Task<IList<ScoreResponse>> getmanualScorelist(string strEventID, int iLinkID = 0, string strcolor = "", string strfont = "", int iProviderId = 0);
        Task<string> clearEventScore(string strEventID);
        Task<object> getCommentryEventlist();
    }
}
