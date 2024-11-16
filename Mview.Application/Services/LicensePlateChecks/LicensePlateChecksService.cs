using Microsoft.Extensions.Logging;
using Mview.Application.BL.LicensePlateChecks;
using Mview.Domain.IRepositories;


namespace Mview.Application.Services.LicensePlateChecks
{
    internal class LicensePlateChecksService : ILicensePlateChecksService
    {
        private readonly ILogger<LicensePlateChecksService> _logger;
        private readonly ILicensePlateChecksBL _licensePlateChecksBL;
        private readonly ILicensePlateChecksRepository _licensePlateChecksRepository;
        public LicensePlateChecksService(ILogger<LicensePlateChecksService> logger, ILicensePlateChecksBL licensePlateChecksBL, ILicensePlateChecksRepository licensePlateChecksRepository)
        {
            _logger = logger;
            _licensePlateChecksBL = licensePlateChecksBL;
            _licensePlateChecksRepository = licensePlateChecksRepository;
        }

        //Question 1,2,3
        public void GenerateRandomData()
        {
            //Question 1 
            // Create a Csv file containing random times and license plates.
            var pathToCreateCsvFile = _licensePlateChecksBL.CreateCsvFileWithRandomData();

            //Question 2,3
            // Run Stored Procedure ImportAndInsertDataFromCsvFile
            //  1.Create a table if not exist  
            //  2.Receives data into the table from the Csv file
            _licensePlateChecksRepository.ImportAndInsertDataFromCsvFile(pathToCreateCsvFile);
        }

        public async void LicensePlateCheckErrors()
        {
            // Question 4,5
            _logger.LogInformation("LicensePlateChecksService start");
            _licensePlateChecksBL.LicensePlateFindErrors(await _licensePlateChecksRepository.GetLicensePlates());
        }
    }
}
