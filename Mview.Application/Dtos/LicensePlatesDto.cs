using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mview.Application.Dtos
{
    public class LicensePlatesDto
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string LicensePlate { get; set; }

        public LicensePlatesDto(int id, DateTime date, string licensePlate)
        {
            ID = id;
            Date = date;
            LicensePlate = licensePlate;
        }
    }
}
