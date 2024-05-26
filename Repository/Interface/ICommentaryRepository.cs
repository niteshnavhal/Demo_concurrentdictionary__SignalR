using Modal.Response.Commentary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ICommentaryRepository
    {
        Task<Scoraboradlink> GetScoreLink(string strEvenID, int iLinkId);
        //Task<Scoraboradlink> GetScoreLink_New(string strEvenID, int iLinkId);
        Task<IEnumerable<Scoraboradlink>> GetScoreLinks(string strEvenID, int iLinkId);
    }
}
