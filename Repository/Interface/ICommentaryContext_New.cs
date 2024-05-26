using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ICommentaryContext_New
    {
        Task<IEnumerable<T>> GetDataList<T>(string Query, DynamicParameters DynamicParameters, CommandType CommandType);
        Task<T> GetDataFirstDefault<T>(string Query, DynamicParameters DynamicParameters, CommandType CommandType);
        Task<(int, List<object>)> ExecutableData(string Query, DynamicParameters DynamicParameters, List<string> OutputParams, CommandType CommandType);
        Task<dynamic> GetMultipleData(string Query, DynamicParameters DynamicParameters, CommandType CommandType);
        Task<bool> ExecutableData(string Query, DynamicParameters DynamicParameters, CommandType CommandType);
        Task<(IEnumerable<T> appResultSet, List<object> appOutPutList)> GetDataListWithOutPut<T>(string Query, DynamicParameters DynamicParameters, List<string> OutputParams, CommandType CommandType);
        Task<int> ExecuteAsync(string Query, DynamicParameters DynamicParameters, CommandType CommandType);
       Task<List<object>> ExecutableDataWithOutput(string Query, DynamicParameters DynamicParameters, List<string> OutputParams, CommandType CommandType);

    }
}
