using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mview.Domain.Models
{
    public class LicensePlatesErrorsModel
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string ErorrLicensePlate { get; set; } = string.Empty;
        public string SuggestionToFixed { get; set; } = string.Empty;
        public object DateTime1 { get; set; }
    }
}
