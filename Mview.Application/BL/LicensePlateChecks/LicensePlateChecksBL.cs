using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mview.Domain.IRepositories;
using CsvHelper;
using System.Collections;
using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Mview.Domain.Models;
using Mview.Application.Dtos;
using Mview.Application.BL.Helpers;
using AutoMapper;
using System.Reflection;
namespace Mview.Application.BL.LicensePlateChecks
{
    internal class LicensePlateChecksBL : ILicensePlateChecksBL
    {
        private readonly ILogger<LicensePlateChecksBL> _logger;
        public Dictionary<int, LicensePlateErrorsDto> LicensePlateErrorsAndSuggestionToFixed = new Dictionary<int, LicensePlateErrorsDto>();
        private readonly ILicensePlateChecksRepository _licensePlateChecksRepository;
        private readonly IMapper _mapper;

        public LicensePlateChecksBL(ILogger<LicensePlateChecksBL> logger, ILicensePlateChecksRepository licensePlateChecksRepository, IMapper mapper)
        {
            _logger = logger;
            _licensePlateChecksRepository = licensePlateChecksRepository;
            _mapper = mapper;
        }

        public string CreateCsvFileWithRandomData()
        {

            var LicensePlatesObjects = new List<LicensePlatesToCsvFileDto>();
            Random gen = new Random();

            DateTime RandomDay()
            {
                DateTime start = new DateTime(2024, 1, 1);
                int range = (DateTime.Today - start).Days;
                return start.AddDays(gen.Next(range))
                    .AddHours(gen.Next(ConstantsVariables.MINIMUM_HOUR, ConstantsVariables.MAXIMUM_HOUR))
                    .AddMinutes(gen.Next(ConstantsVariables.MINIMUM_MINUTES, ConstantsVariables.MAXIMUM_MINUTES ))
                    .AddSeconds(gen.Next(ConstantsVariables.MINIMUM_SECONDS, ConstantsVariables.MAXIMUM_SECONDS));
            }
            
            for (int i = 0; i < 100; i++)
            {
                LicensePlatesObjects.Add(new LicensePlatesToCsvFileDto(RandomDay(), gen.Next(ConstantsVariables.MINIMUM_LICENCE_PLATE_NUMBER, ConstantsVariables.MAXIMUM_LICENCE_PLATE_NUMBER).ToString()));
            }

            using (var writer = new StreamWriter(@ConstantsVariables.CSV_PATH))            
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(LicensePlatesObjects);
            }

            var path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            return path.Substring(ConstantsVariables.START_FROM_INDEX_0, path.Length - ConstantsVariables.BIN_DEDUG_NET80 ) + ConstantsVariables.CSV_PATH.Substring(ConstantsVariables.START_FROM_INDEX_3);
        }

        /*
        ******** I make several assumptions *********
        * The correct LicensePlate will be the last integer received
        * 1. If the approximation between the plates is 80% or more and the difference digits are 8,3,0 or 7,1 insert into the error table.
        * 2. else If the approximation between the plates is 80% or more and the difference digits are other digits, it does not insert into the error table.
        * 3. When a number with a missing digit and the missing digit is from the beginning or the end only, insert into the error table.
        * 4. The fix would be to always take the last integer received.
        *  For Example, on this difference case: 
        *  --------------------------------------------------------
        *  a.
        *       DateTime         LicensePlates
        * 1. 2024/11/14 15:10:05     76-468-10  The First Input
        * 2. 2024/11/14 15:10:05     16-468-10  7 replace to 1
        * 3. 2024/11/14 15:10:05     76-460-10  8 replace tp 0
        * 
        * the answer is  3. 2024/11/14 15:10:05  76-460-10  
        * 
        * --------------------------------------------------------
        *  b.
        *         DateTime         LicensePlates
        * 1. 2024/11/14 15:10:05     76-468-10  The First Input
        * 2. 2024/11/14 15:10:05     16-468-10  7 replace to 1
        * 3. 2024/11/14 15:10:06      6-460-10  the first number not captured
        *  
        *  the answer is  2. 2024/11/14 15:10:05     16-468-10 
        *  
        * --------------------------------------------------------
        * c.
        *        DateTime          LicensePlates
        * 1. 2024/11/14 15:10:05     76-468-10  The First Input
        * 2. 2024/11/14 15:10:06      6-460-10  the first number not captured
        * 3. 2024/11/14 15:10:07     76-468-1   the last  number not captured         
        * 
        *  the answer is  1. 2024/11/14 15:10:05    76-468-10
        * 
        * --------------------------------------------------------
        * d. 
        * 
        *      DateTime          LicensePlates
        * 1. 2024/11/14 15:10:05     76-468-10  The First Input
        * 2. 2024/11/14 15:10:05     16-468-10  7 replace to 1
        * 3. 2024/11/14 15:10:05     76-460-10  8 replace tp 0
        * 4. 2024/11/14 15:10:06      6-460-10  the first number not captured
        * 5. 2024/11/14 15:10:07     76-468-1   the last  number not captured
        * 6. 2024/11/14 15:10:10     76-468-18  0 replace tp 8
        * 
        *  the answer is  6. 2024/11/14 15:10:10     76-468-18
        *  On this case, The correct LicensePlate will be the last integer received. 2024/11/14 15:10:10   76-468-18
        * 
        *
        */


        public void LicensePlateFindErrors(IEnumerable<LicensePlatesModel> licensePlatesObjects)
        {
            _logger.LogInformation("LicensePlateChecksBL => LicensePlateFindErrors start");

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LicensePlateErrorsDto, LicensePlatesErrorsModel>();  // Add this mapping
            });

            IMapper mapper = configuration.CreateMapper();

            foreach (var licensePlate in licensePlatesObjects)
            {
                var licensePlatesToCheckError = licensePlatesObjects
                    .Where(x => licensePlate.Date.AddMinutes(1) >= x.Date
                                && licensePlate.Date <= x.Date);
                // Find Errors
                CheckIfErorrExist(licensePlate, licensePlatesToCheckError);
            }


            // Run Stored Procedure dbo.usp_MV_InsertLicenseErrorsAndFix
            // To Insert Error license plate and Suggeust how to fix it it's
            // the Procedure save the data on table LicensePlatesErrorsAndFixSuggest
            var destination = mapper.Map<List<LicensePlateErrorsDto>, List<LicensePlatesErrorsModel>>(new List<LicensePlateErrorsDto>(LicensePlateErrorsAndSuggestionToFixed.Values).ToList());
            _licensePlateChecksRepository.InsertLicensePlatesErrorsAndSuggestToFix(destination);

        }

        private int CheckIfErorrExist(LicensePlatesModel LicensePlateToCheck, IEnumerable<LicensePlatesModel> LicensePlatesToCheckError)
        {
            HashSet<int> errorLicenseId = new HashSet<int>();
            float ActualErrorPercentage = ConstantsVariables.INIT_ACTUAL_ERROR_PERCENTAGE;

            foreach (var LicensePlate in LicensePlatesToCheckError)
            {
                if (
                    LicensePlate.LicensePlate.Length < ConstantsVariables.MINIMUM_LICENCE_PLATE-1 ||       
                    LicensePlate.LicensePlate.Length  > ConstantsVariables.MAXIMUM_LICENCE_PLATE)
                { 
                    continue;
                }
                else
                {
                    for (int i = 0; i < LicensePlateToCheck.LicensePlate.Length; i++)
                    {
                        if (LicensePlateToCheck.LicensePlate == LicensePlate.LicensePlate)
                            break;
                        // on this case the LicensePlateToCheck License is with a less digit and LicensePlate lenght is 7 or 8
                        if (LicensePlateToCheck.LicensePlate.Length < ConstantsVariables.MINIMUM_LICENCE_PLATE-1 || LicensePlateToCheck.LicensePlate.Length > ConstantsVariables.MAXIMUM_LICENCE_PLATE)
                        { break; }

                        // check if an error license lost digit from beginning or from end
                        if (LicensePlateToCheck.LicensePlate.Length + ConstantsVariables.INDEX_ONE == LicensePlate.LicensePlate.Length)
                        {
                            string licenseLostDigitFromBeginning = GetLicenseLostDigitFromBeginning(LicensePlate.LicensePlate, LicensePlateToCheck.LicensePlate);
                            string licenseLostDigitFromEnd = GetLicenseLostDigitFromEnd(LicensePlate.LicensePlate, LicensePlateToCheck.LicensePlate);

                            if (licenseLostDigitFromBeginning != string.Empty)
                            {
                                if (AddLicensePlateErrorList(LicensePlateToCheck))
                                    errorLicenseId.Add(LicensePlateToCheck.ID);
                                if (AddLicensePlateErrorList(LicensePlate))
                                    errorLicenseId.Add(LicensePlate.ID);
                                ActualErrorPercentage = ConstantsVariables.INIT_ACTUAL_ERROR_PERCENTAGE;
                                break;
                            }
                            if (licenseLostDigitFromEnd != string.Empty)
                            {
                                if (AddLicensePlateErrorList(LicensePlateToCheck))
                                    errorLicenseId.Add(LicensePlateToCheck.ID);
                                if (AddLicensePlateErrorList(LicensePlate))
                                    errorLicenseId.Add(LicensePlate.ID);
                                ActualErrorPercentage = ConstantsVariables.INIT_ACTUAL_ERROR_PERCENTAGE;
                                break;
                            }
                        }

                        if (LicensePlateToCheck.LicensePlate.Length == LicensePlate.LicensePlate.Length + 1)
                        {
                            // the beginning digit is lost case
                            // Ex: 7646810
                            //      646810 
                            string licenseLostDigitFromBeginning = GetLicenseLostDigitFromBeginning(LicensePlateToCheck.LicensePlate, LicensePlate.LicensePlate);

                            string licenseLostDigitFromEnd = GetLicenseLostDigitFromEnd(  LicensePlateToCheck.LicensePlate, LicensePlate.LicensePlate);

                            if (licenseLostDigitFromBeginning != string.Empty)
                            {
                                if (AddLicensePlateErrorList(LicensePlate))
                                    errorLicenseId.Add(LicensePlate.ID);
                                if (AddLicensePlateErrorList(LicensePlateToCheck))
                                    errorLicenseId.Add(LicensePlateToCheck.ID);
                                ActualErrorPercentage = ConstantsVariables.INIT_ACTUAL_ERROR_PERCENTAGE;
                                break;
                            }
                            // the end digit is lost case 
                            // Ex:  646810
                            //     7646810
                            if (licenseLostDigitFromEnd != string.Empty)
                            {
                                if (AddLicensePlateErrorList(LicensePlate))
                                    errorLicenseId.Add(LicensePlate.ID);
                                if (AddLicensePlateErrorList(LicensePlateToCheck))
                                    errorLicenseId.Add(LicensePlateToCheck.ID);
                                ActualErrorPercentage = ConstantsVariables.INIT_ACTUAL_ERROR_PERCENTAGE;
                                break;
                            }


                        }
                        // on this case the LicensePlate License is with a less digit
                        //if (LicensePlateToCheck.LicensePlate.Length == LicensePlate.LicensePlate.Length+1)
                        // 
                        //
                        // check if any digit 1,7 or 0,3,8 is errors digit and <= 80%.
                        // *** 80% is a parameter we can change if necessary.

                        // When LicensePlate is less than LicensePlateToCheck, arrived to end fo LicensePlate so break.  
                        if (LicensePlate.LicensePlate.Length == i)
                                break;
                            if (LicensePlateToCheck.LicensePlate[i] != LicensePlate.LicensePlate[i])
                        {
                            if (CheckSuspiciousDigits(LicensePlateToCheck.LicensePlate[i], LicensePlate.LicensePlate[i]))
                            {
                                ActualErrorPercentage = ActualErrorPercentage - (ConstantsVariables.INIT_ACTUAL_ERROR_PERCENTAGE / LicensePlateToCheck.LicensePlate.Length);
                            }
                            else break;

                        }
                        if (i == LicensePlateToCheck.LicensePlate.Length - ConstantsVariables.INDEX_ONE
                            && ActualErrorPercentage > ConstantsVariables.ERROR_PERCENTAGE
                            && ActualErrorPercentage < ConstantsVariables.INIT_ACTUAL_ERROR_PERCENTAGE)
                        {
                            if (AddLicensePlateErrorList(LicensePlate))
                                errorLicenseId.Add(LicensePlate.ID);
                            ActualErrorPercentage = ConstantsVariables.INIT_ACTUAL_ERROR_PERCENTAGE;
                        }

                        if (ActualErrorPercentage < ConstantsVariables.ERROR_PERCENTAGE)
                        {
                            ActualErrorPercentage = ConstantsVariables.INIT_ACTUAL_ERROR_PERCENTAGE;
                            break;
                        }
                    }
                }
            }
            if (errorLicenseId.Count() > ConstantsVariables.EMPTY)
            {
                if (AddLicensePlateErrorList(LicensePlateToCheck))
                    errorLicenseId.Add(LicensePlateToCheck.ID);
                if (AddLicensePlateErrorList(LicensePlateToCheck))
                    errorLicenseId.Add(LicensePlateToCheck.ID);
            }
            AddSuggestionToFixError(errorLicenseId);
            return errorLicenseId.Count();
        }


        /// <summary>
        /// Check if the Error digit is 
        ///    from 0 to 8,3 
        /// or from 3 to 0,8
        /// or from 8 to 0,3
        /// 
        /// or from 1 to 7
        /// ot from 7 to 1
        /// </summary>
        private bool CheckSuspiciousDigits(char num1 , char num2)
        {
            int number1 = int.Parse(num1.ToString());
            int number2 = int.Parse(num2.ToString());
            return (
                            (
                                (number1 == 7 || number1 == 1) 
                                    && 
                                (number2 == 7 || number2 == 1)
                            )
                       ||
                            (
                                (number1 == 0 || number1 == 3 || number1 == 8)
                                    &&
                                (number2 == 0 || number2 == 3 || number2 == 8)
                            )
                    );
        }
        private string GetLicenseLostDigitFromBeginning(string licensePlate, string lessDigitlicensePlate)
        {
            return (
                licensePlate.Substring(1, licensePlate.Length - ConstantsVariables.INDEX_ONE) ==
                lessDigitlicensePlate.Substring(ConstantsVariables.FROM_BEGINNING, lessDigitlicensePlate.Length)) 
                ? lessDigitlicensePlate : string.Empty;
        }


        private string GetLicenseLostDigitFromEnd(string licensePlate, string lessDigitlicensePlate)
        {
            return (
                licensePlate.Substring(ConstantsVariables.FROM_BEGINNING, licensePlate.Length - ConstantsVariables.INDEX_ONE) ==
                lessDigitlicensePlate.Substring(ConstantsVariables.FROM_BEGINNING, lessDigitlicensePlate.Length ))
                ? lessDigitlicensePlate : string.Empty;
        }

        /// <summary>
        /// Add the error license plate to Dictionary AddLicensePlateErrorList if not added yet. 
        /// </summary>
        private bool AddLicensePlateErrorList(LicensePlatesModel licensePlates) 
        {
            if (!LicensePlateErrorsAndSuggestionToFixed.ContainsKey(licensePlates.ID))
            {
                LicensePlateErrorsAndSuggestionToFixed.Add(licensePlates.ID, new LicensePlateErrorsDto(licensePlates));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Return the longest plate number so that the number of characters in it is between 7 and 8
        /// If there are several of these we will return the latest received number.
        /// </summary>
        private void AddSuggestionToFixError(HashSet<int> errorLicenseId)
        {
            LicensePlatesDto correctLicensePlate = new(ConstantsVariables.FROM_BEGINNING, new DateTime(),string.Empty);

            //Find the correct license plate 
            foreach (var LicensePlate in LicensePlateErrorsAndSuggestionToFixed)
            {
                if (LicensePlate.Value.ErorrLicensePlate.Length >= ConstantsVariables.MINIMUM_LICENCE_PLATE &&
                    LicensePlate.Value.ErorrLicensePlate.Length <= ConstantsVariables.MAXIMUM_LICENCE_PLATE &&
                    correctLicensePlate.Date < LicensePlate.Value.Date &&
                    errorLicenseId.Contains(LicensePlate.Key))
                    correctLicensePlate = new(LicensePlate.Value.ID, LicensePlate.Value.Date, LicensePlate.Value.ErorrLicensePlate);                
            }

            // add SuggestionToFixed license plate 
            foreach (var LicensePlate in LicensePlateErrorsAndSuggestionToFixed)
            {
                if(errorLicenseId.Contains(LicensePlate.Key))
                    LicensePlate.Value.SuggestionToFixed = correctLicensePlate.LicensePlate;
            }
        }

    }
}
