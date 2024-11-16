using Mview.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mview.Domain.IRepositories
{
    public interface ILicensePlateChecksRepository
    {
        public Task<IEnumerable<LicensePlatesModel>> GetLicensePlates();
        public Task<bool> ImportAndInsertDataFromCsvFile(string csvFile);
        public Task<bool> InsertLicensePlatesErrorsAndSuggestToFix(List<LicensePlatesErrorsModel> licensePlates);
    }
}
