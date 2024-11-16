using Mview.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mview.Dapper;
using System.Data;
using Mview.Infrastructure;
using System.Data.SqlClient;
using Dapper;
namespace Mview.Domain.Queries
{
    internal class InsertLicensePlatesErrorsAndSuggestToFixQuery
    {
        private const string _storedProcedureName = "dbo.usp_MV_InsertLicenseErrorsAndFix";
               
        public InsertLicensePlatesErrorsAndSuggestToFixQuery(List<LicensePlatesErrorsModel> licensePlates)
        {
            LicensePlatesErrorsSuggestFix = licensePlates.ToDataTable();
            //ErrorMessage = string.Empty;
        }

        [Parameter("pFields", "udt_License_Plates_Errors_Fix_Suggest_table")]
        public DataTable LicensePlatesErrorsSuggestFix { get; set; }


        public async Task<bool> Query()
        {
            await using var connection = new SqlConnection(GlobalConfiguration.ConnectionString);
            var parameters = this.ToParameters();
            try
            {
                connection.Query(_storedProcedureName, parameters, commandType: CommandType.StoredProcedure);
                return true;
                //new InsertResultModel(parameters.Get<int>("pInsertRowCount"), parameters.Get<int>("pErrCode"), parameters.Get<string>("pErrMsg"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
