using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mview.Domain.Models
{
    public class LicensePlatesModel
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
    }
}
