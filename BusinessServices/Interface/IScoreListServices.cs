using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Interface
{
    public interface IScoreListServices
    {
        Task<bool> isEventExists(string eventID);
    }
}
