using Modal.Response.Commentary;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Repository.Implementation
{
    public class CommentaryRepository : ICommentaryRepository
    {
        public static string strErrorHeader = "ScoreCard :: CommentaryRepository.Implementation :: ICommentaryRepository ";
        public ICommentaryContext _ICommentaryContext;
        //public ICommentaryContext_New _ICommentaryContext_New;

        public CommentaryRepository(ICommentaryContext ICommentaryContext)
        {
            _ICommentaryContext = ICommentaryContext;
            //_ICommentaryContext_New = CommentaryContext_New;
        }

        public async Task<Scoraboradlink> GetScoreLink(string strEvenID,int iLinkId)
        {
            try
            {
                DynamicParameters DynamicParameters = new DynamicParameters();
                DynamicParameters.Add("@appEventID", strEvenID);
                DynamicParameters.Add("@appLinkID", iLinkId);
                return  await this._ICommentaryContext.GetDataFirstDefault<Scoraboradlink>("[dbo].[netCore_getScoreLink]", DynamicParameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

            }
            return null;
        }
/*
        public async Task<Scoraboradlink> GetScoreLink_New(string strEvenID, int iLinkId)
        {
            try
            {
                DynamicParameters DynamicParameters = new DynamicParameters();
                DynamicParameters.Add("@appEventID", strEvenID);
                DynamicParameters.Add("@appLinkID", iLinkId);
                return await this._ICommentaryContext_New.GetDataFirstDefault<Scoraboradlink>("[dbo].[netCore_getScoreLink]", DynamicParameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

            }
            return null;
        }
*/
        public async Task<IEnumerable<Scoraboradlink>> GetScoreLinks(string strEvenID, int iLinkId)
        {
            try
            {
                DynamicParameters DynamicParameters = new DynamicParameters();
                DynamicParameters.Add("@appEventIDs", strEvenID);
                DynamicParameters.Add("@appLinkID", iLinkId);
                return await this._ICommentaryContext.GetDataList<Scoraboradlink>("[dbo].[netCore_getScoreLinks]", DynamicParameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}
