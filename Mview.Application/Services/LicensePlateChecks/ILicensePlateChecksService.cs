using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mview.Application.Services.LicensePlateChecks
{
    public interface ILicensePlateChecksService
    {
        public void GenerateRandomData();
        public void LicensePlateCheckErrors();

    }
}
