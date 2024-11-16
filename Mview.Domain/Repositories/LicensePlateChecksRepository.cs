using Microsoft.Extensions.Logging;
using Mview.Domain.IRepositories;
using Mview.Domain.Models;
using Mview.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mview.Domain.Repositories
{
    public class LicensePlateChecksRepository : ILicensePlateChecksRepository
    {
        private readonly ILogger<LicensePlateChecksRepository> _logger;

        public LicensePlateChecksRepository(ILogger<LicensePlateChecksRepository> logger)
        {
            _logger = logger;
        }

        public async Task<bool> ImportAndInsertDataFromCsvFile(string csvFile)
        {
            _logger.LogInformation("ImportAndInsertDataFromCsvFile, Repository Start");
            var importFile = new ImportAndInsertDataFromCsvFileQuery(csvFile);
            return await importFile.Query();

        }

        public async Task<IEnumerable<LicensePlatesModel>> GetLicensePlates()
        {
            _logger.LogInformation("GetHoursOfOperation Start");
            var licensePlates = new GetLicensePlates();
            return await licensePlates.Query();
        }

        public async Task<bool> InsertLicensePlatesErrorsAndSuggestToFix(List<LicensePlatesErrorsModel> licensePlates)
        {

            _logger.LogInformation("InsertLicensePlatesErrorsAndSuggestToFix  Start");
            var licensePlateError = new InsertLicensePlatesErrorsAndSuggestToFixQuery(licensePlates);
            return await licensePlateError.Query();
        }
    }
}
