using Dapper;
using Microsoft.Extensions.Configuration;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class CommentaryContext_New : ICommentaryContext_New
    {
        //public SqlConnection connection;

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        public SqlConnection connection { get; set; }

        /// <summary>
        /// The connection string
        /// </summary>
        private static string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="DBContext" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public CommentaryContext_New(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:Commentary_new"];
            connection = new SqlConnection(_connectionString);
        }
        public async Task<IEnumerable<T>> GetDataList<T>(string Query, DynamicParameters DynamicParameters, CommandType CommandType)
        {
            IEnumerable<T> Response = default(IEnumerable<T>);
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                Response = await connection.QueryAsync<T>(Query, DynamicParameters, commandType: CommandType);
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return Response;
        }

        public async Task<T> GetDataFirstDefault<T>(string Query, DynamicParameters DynamicParameters, CommandType CommandType)
        {
            T Response = default(T);
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                Response = await connection.QueryFirstOrDefaultAsync<T>(Query, DynamicParameters, commandType: CommandType);
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return Response;
        }

        public async Task<(int, List<object>)> ExecutableData(string Query, DynamicParameters DynamicParameters, List<string> OutputParams, CommandType CommandType)
        {
            int Response = 0;
            List<object> objOutPutData = new List<object>();
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                Response = await connection.ExecuteAsync(Query, DynamicParameters, commandType: CommandType);
                if (OutputParams != null)
                {
                    for (int i = 0; i < OutputParams.Count; i++)
                    {
                        objOutPutData.Add(DynamicParameters.Get<object>(OutputParams[i]));
                    }
                }

                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                objOutPutData.Add(ex.Message + " :: " + ex.StackTrace + " :: " + ex.InnerException);
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
                return (0, objOutPutData);
            }
            return (Response, objOutPutData);
        }

        public async Task<dynamic> GetMultipleData(string Query, DynamicParameters DynamicParameters, CommandType CommandType)
        {
            List<dynamic> Response = new List<dynamic>();
            //dynamic Response = null;
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                var multiData = await connection.QueryMultipleAsync(Query, DynamicParameters, commandType: CommandType);
                try
                {
                    while (true)
                    {
                        Response.Add(multiData?.ReadAsync<dynamic>().Result);
                    }
                }
                catch
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                    return Response;
                }
            }
            catch (Exception ex)
            {
                return (ex.Message + " :: " + ex.StackTrace + " :: " + ex.Message);
            }
        }

        public async Task<bool> ExecutableData(string Query, DynamicParameters DynamicParameters, CommandType CommandType)
        {
            bool Response = false;
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                var iExec = await connection.ExecuteAsync(Query, DynamicParameters, commandType: CommandType);
                if (iExec > 0) Response = true;

                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return Response;
        }

        public async Task<(IEnumerable<T> appResultSet, List<object> appOutPutList)> GetDataListWithOutPut<T>(string Query, DynamicParameters DynamicParameters, List<string> OutputParams, CommandType CommandType)
        {
            try
            {
                IEnumerable<T> Response = default(IEnumerable<T>);
                List<object> objOutPutData = new List<object>();

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                Response = await connection.QueryAsync<T>(Query, DynamicParameters, commandType: CommandType);

                if (OutputParams != null)
                {
                    for (int i = 0; i < OutputParams.Count; i++)
                    {
                        objOutPutData.Add(DynamicParameters.Get<object>(OutputParams[i]));
                    }
                }

                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }

                return (Response, objOutPutData);
            }
            catch (Exception ex)
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }

            }
            return (null, null);
        }

        public async Task<int> ExecuteAsync(string Query, DynamicParameters DynamicParameters, CommandType CommandType)
        {
            int Response = 0;
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                Response = await connection.ExecuteAsync(Query, DynamicParameters, commandType: CommandType);
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
                return (0);
            }
            return (Response);
        }

        public async Task<List<object>> ExecutableDataWithOutput(string Query, DynamicParameters DynamicParameters, List<string> OutputParams, CommandType CommandType)
        {
            List<object> objOutPutData = new List<object>();
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                var iExec = await connection.ExecuteAsync(Query, DynamicParameters, commandType: CommandType);
                if (OutputParams != null)
                {
                    for (int i = 0; i < OutputParams.Count; i++)
                    {
                        objOutPutData.Add(DynamicParameters.Get<object>(OutputParams[i]));
                    }
                }
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return objOutPutData;
        }

   
    }
}
