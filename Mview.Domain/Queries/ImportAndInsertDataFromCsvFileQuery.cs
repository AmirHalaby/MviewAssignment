using Mview.Dapper;
using Mview.Domain.Models;
using Mview.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Mview.Domain.Queries
{
    internal class ImportAndInsertDataFromCsvFileQuery
    {
        private const string _storedProcedureName = "dbo.usp_MV_ImportAndInsertDataFromCsvFile";

        public ImportAndInsertDataFromCsvFileQuery(string csvPathFile)
        {
            CsvPathFile = csvPathFile;
            //ErrorMessage = string.Empty;
        }

        [Parameter("pCsvPathFile")]
        public string CsvPathFile { get; set; }

        //[Parameter("pErrCode", parameterDirection: ParameterDirection.Output)]
        //public int ErrorCode { get; set; }

        //[Parameter("pErrMsg", parameterDirection: ParameterDirection.Output)]
        //public string ErrorMessage { get; set; }

        public async Task<bool> Query()
        {
            await using var connection = new SqlConnection(GlobalConfiguration.ConnectionString);
            var parameters = this.ToParameters();
            try
            {
                connection.Query(_storedProcedureName, parameters, commandType: CommandType.StoredProcedure);

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
