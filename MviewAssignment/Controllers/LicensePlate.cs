using Microsoft.AspNetCore.Mvc;
using Mview.Application.Services.LicensePlateChecks;

namespace Mview.Api.Controllers
{
    [ApiController]
    [Route("LicensePlateChecks")]
    public class LicensePlate : ControllerBase
    {
        private readonly ILogger<LicensePlate> _logger;
        private readonly ILicensePlateChecksService _licensePlateChecksService;
        public LicensePlate(ILogger<LicensePlate> logger, ILicensePlateChecksService licensePlateChecksService)
        {
            _logger = logger;
            _licensePlateChecksService = licensePlateChecksService;
        }
        

        [HttpPost("LicensePlateCheckErrors")]
        public IActionResult LicensePlateCheckErrors()
        {
            _logger.LogInformation("LicensePlateCheckErrors Controller");
            _licensePlateChecksService.LicensePlateCheckErrors();
            return Ok();
        }
        [HttpPost("GenerateAndSaveRandomData")]
        public IActionResult GenerateRandomData()
        {
            _logger.LogInformation("LicensePlateChecks Controller");
            _licensePlateChecksService.GenerateRandomData();
            return Ok();
        }
    }
}
