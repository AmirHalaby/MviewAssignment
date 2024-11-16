using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mview.Domain.Models
{
    public class InsertResultModel
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public InsertResultModel(int errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}
