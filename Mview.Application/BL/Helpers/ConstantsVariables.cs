using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mview.Application.BL.Helpers
{
    public static class ConstantsVariables
    {
        public const string CSV_PATH = "..\\RandomlicensePlates.Csv";

        public const int BIN_DEDUG_NET80 = 32; 
        public const int START_FROM_INDEX_0 = 0;
        public const int START_FROM_INDEX_3 = 3;
        public const int MINIMUM_LICENCE_PLATE_NUMBER = 10000;
        public const int MAXIMUM_LICENCE_PLATE_NUMBER = 100000000;
        public const int MINIMUM_HOUR = 0;
        public const int MAXIMUM_HOUR = 24;

        public const int MINIMUM_MINUTES = 0;
        public const int MAXIMUM_MINUTES = 60;

        public const int MINIMUM_SECONDS = 0;
        public const int MAXIMUM_SECONDS = 60;

        public const int MINIMUM_LICENCE_PLATE = 7;
        public const int MAXIMUM_LICENCE_PLATE = 8;

        public const float ERROR_PERCENTAGE = 80;
        
        public const float INIT_ACTUAL_ERROR_PERCENTAGE = 100;

        public const int INDEX_ONE = 1;
        public const int EMPTY = 0;
        public const int FROM_BEGINNING = 0;
    }
}
