using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mview.Application.Dtos
{
    internal class LicensePlatesToCsvFileDto
    {
        public DateTime DateTime { get; set; }
        public string RandomNumber { get; set; }

        public LicensePlatesToCsvFileDto(DateTime date, string licensePlate)
        {
            DateTime = date;
            RandomNumber = licensePlate;
        }
    }
}
