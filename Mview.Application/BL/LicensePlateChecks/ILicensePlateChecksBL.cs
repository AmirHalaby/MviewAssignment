using Mview.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mview.Application.BL.LicensePlateChecks
{
    public interface ILicensePlateChecksBL
    {
        public string CreateCsvFileWithRandomData();
        public void LicensePlateFindErrors(IEnumerable<LicensePlatesModel> licensePlatesModel);

    }
}
