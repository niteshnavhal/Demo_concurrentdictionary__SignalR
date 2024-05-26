using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessServices.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modal;
using Modal.ConcurrentDic;
using Modal.Request;
using Modal.Response;

namespace betfaireScore.Controllers
{
    [Route("api")]
    [ApiController]
    public class betfairScoreController : ControllerBase
    {
        private IbetfairScoreServices _IbetfairScoreServices;
        private IHttpContextAccessor _HttpContextAccessor;
        private IScoreServices _ScoreServices;
        public betfairScoreController(IbetfairScoreServices IbetfairScoreServices, IHttpContextAccessor HttpContextAccessor, IScoreServices IScoreServices)
        {
            _IbetfairScoreServices = IbetfairScoreServices;
            _HttpContextAccessor = HttpContextAccessor;
            _ScoreServices = IScoreServices;
        }
        [HttpGet]
        [Route("Work")]
        public string API()
        {
            return "successfully call";
        }

        [HttpGet]
        [Route("ScoreEventlist")]
        public async Task<List<Scoreinfo>> getallScoreUrl()
        {
            return await _IbetfairScoreServices.getAllScorecard();
        }

        [HttpGet]
        [Route("Scorecard")]
        public async Task<ScoreResponse> scorecard(string eventId, int link = 0, string color = "", string font = "", int pid = 0)
        {
            try
            {


                if (eventId != "")
                    return await _IbetfairScoreServices.getScoreUri(eventId, link, color, font, pid);
            }
            catch (System.Exception ex)
            {
            }
            return null;
        }

        [HttpGet]
        [Route("bfrateScoreborad")]
        public async Task<ScoreResponse> bfrateScoreborad(string eventId, int link = 1, string color = "", string font = "", int pid = 0)
        {
            try
            {
                if (eventId != "")
                    return await _IbetfairScoreServices.getScoreUri(eventId, link, color, font, pid);
            }
            catch (System.Exception ex)
            {
            }
            return new ScoreResponse();
        }

        [HttpGet]
        [Route("bfScoretimeline")]
        public async Task<object> bfScoretimeline(string eventId)
        {
            try
            {
                if (eventId != "")
                {
                    return await _ScoreServices.getScoreTimeline(eventId);
                }
            }
            catch (System.Exception ex)
            {
            }
            return null;
        }

        [HttpGet]
        [Route("Scoreborad")]
        public async Task<object> getbetfairScore(string eventIds)
        {
            try
            {
                return await _IbetfairScoreServices.getbetfairScore(eventIds);
            }
            catch (System.Exception ex)
            {
            }
            return null;
        }

        [HttpGet]
        [Route("bfScore")]
        public async Task<object> bfScore(string eventId)
        {
            try
            {
                if (eventId != "")
                {
                    return await _ScoreServices.getScoreobject(eventId);
                }
            }
            catch (System.Exception ex)
            {
            }
            return null;
        }

        #region NodeAPI
        [HttpGet]
        [Route("ManulScoreborad")]
        public async Task<ScoreResponse> ManulScoreborad(string eventId, int link = 0, string color = "", string font = "", int pid = 0)
        {
            try
            {
                if (eventId != "")
                    return await _IbetfairScoreServices.getManulScoreofevent(eventId, link, color, font, pid);
            }
            catch (System.Exception ex)
            {
            }
            return new ScoreResponse();
        }

        [HttpPost]
        [Route("manualScorelist")]
        public async Task<IList<ScoreResponse>> manualScorelist(manualScoreRequest _vrequest)
        {
            try
            {
                if (_vrequest.eventIds != "")
                    return await _IbetfairScoreServices.getmanualScorelist(_vrequest.eventIds, _vrequest.link, _vrequest.color, _vrequest.font,_vrequest.provider);
            }
            catch (System.Exception ex)
            {
            }
            return null;
        }
        #endregion

        [HttpGet]
        [Route("ClearScorecard")]
        public async Task<string> ClearScorecard(string eventId = "")
        {
            return await _IbetfairScoreServices.clearEventScore(eventId);
        }

        [HttpGet]
        [Route("bfScoreCD")]
        public async Task<object> bfScoreCD(string eventId)
        {
            try
            {
                if (eventId != "")
                {
                    return await _ScoreServices.getScore(Convert.ToInt32(eventId));
                }
            }
            catch (System.Exception ex)
            {
            }
            return null;
        }

        [HttpGet]
        [Route("clearbfScore")]
        public async Task<object> clearbfScore(string eventId = "")
        {
            try
            {
                if (eventId != "")
                {
                    return await _ScoreServices.clearScore(eventId);
                }
            }
            catch (System.Exception ex)
            {
            }
            return null;
        }

        [HttpGet]
        [Route("eventscorelist")]
        public async Task<object> eventscorelist()
        {
            return await _IbetfairScoreServices.getCommentryEventlist();
        }
    }
}
