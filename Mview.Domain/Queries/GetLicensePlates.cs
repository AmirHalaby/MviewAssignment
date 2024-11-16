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
    internal class GetLicensePlates
    {
        private const string _storedProcedureName = "usp_MV_GetLicensePlates";

        public GetLicensePlates()
        {
        }

        public async Task<IEnumerable<LicensePlatesModel>> Query()
        {

            await using var connection = new SqlConnection(GlobalConfiguration.ConnectionString);
            var parameters = this.ToParameters();
            try
            {
                var x = await connection.QueryAsync<LicensePlatesModel>(_storedProcedureName, parameters, commandType: CommandType.StoredProcedure);
                return x;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
