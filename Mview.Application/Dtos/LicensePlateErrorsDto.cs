using Mview.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mview.Application.Dtos
{
    public class LicensePlateErrorsDto
    {
        public int ID { get; set; }        
        public DateTime Date {  get; set; }
        public string ErorrLicensePlate { get; set; } = string.Empty;
        public string SuggestionToFixed {  get; set; } = string.Empty;
        public LicensePlateErrorsDto(LicensePlatesModel licensePlates) 
        {
            ID = licensePlates.ID;
            Date = licensePlates.Date;
            ErorrLicensePlate = licensePlates.LicensePlate;
        }
        public LicensePlateErrorsDto(int id, DateTime dateTime, string erorrLicensePlate, string suggestionToFixed) 
        {
            ID = id;
            Date = dateTime;
            ErorrLicensePlate = erorrLicensePlate;
            SuggestionToFixed = suggestionToFixed;
        }
    }
}
