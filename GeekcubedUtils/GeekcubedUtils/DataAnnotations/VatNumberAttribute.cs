// Copyright 2012 Ian Stapleton
// Port of original JS Version by Braemoor Software
//
//Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace GeekcubedUtils.DataAnnotations
{
    /// <summary>
    /// Validation attribute ensures confirmation to one of the expected formats for a Vat Registration number as recognised across the EU
    /// </summary>
    /// <remarks>Updated to keep in synch with original referenced JS version. July 2012</remarks>
    /// <example>
    ///     [VatNumber(ErrorMessageResourceName = "User_VATNumber", ErrorMessageResourceType = typeof(Resources.Validation))]
    ///     string VatNumber { get; set; }
    /// </example>
    /// <see cref="http://www.braemoor.co.uk/software/vat.shtml"/>
    public class VatNumberAttribute : ValidationAttribute, IClientValidatable
    {
        // Note - VAT codes without the "**" in the comment do not
        // have check digit checking.
        private static string[] vatExpressions = new string[] {
            @"^(AT)U(\d{8})$",                           //** Austria
            @"^(BE)0(\d{9})$",                         //** Belgium 
            @"^(BG)(\d{9,10})$",                         // Bulgaria 
            @"^(CY)(\d{8}[A-Z])$",                       //** Cyprus 
            @"^(CZ)(\d{8,10})(\d{3})?$",                 //** Czech Republic
            @"^(DE)(\d{9})$",                            //** Germany 
            @"^(DK)((\d{8}))$",                          //** Denmark 
            @"^(EE)(\d{9})$",                            //** Estonia 
            @"^(EL)(\d{9})$",                          //** Greece 
            @"^(ES)([A-Z]\d{8})$",                       //** Spain (1)
            @"^(ES)(\d{8}[A-Z])$",                       // Spain (2)
            @"^(ES)([A-Z]\d{7}[A-Z])$",                  //** Spain (3)
            @"^(EU)(\d{9})$",                            //** EU-type 
            @"^(FI)(\d{8})$",                            //** Finland 
            @"^(FR)(\d{11})$",                           //** France (1)
            @"^(FR)[(A-H)|(J-N)|(P-Z)]\d{10}$",          // France (2)
            @"^(FR)\d[(A-H)|(J-N)|(P-Z)]\d{9}$",         // France (3)
            @"^(FR)[(A-H)|(J-N)|(P-Z)]{2}\d{9}$",        // France (4)
            @"^(GB)?(\d{9})$",                           //** UK (standard)
            /*@"^(GB)(\d{10})$",                          //** UK (Commercial)*/
            @"^(GB)?(\d{12})$",                          //UK (IOM standard)
            /*@"^(GB)(\d{13})$",                          //UK (IOM commercial)*/
            @"^(GB)?GD\d{3})$",                         //** UK (Government)
            @"^(GB)?(HA\d{3})$",                         //** UK (Health authority)
            @"^(GR)(\d{8,9})$",                          //** Greece 
            @"^(HU)(\d{8})$",                            //** Hungary 
            @"^(IE)(\d{7}[A-W])$",                       //** Ireland (1)
            @"^(IE)([7-9][A-Z\*\+)]\d{5}[A-W])$",        //** Ireland (2)
            @"^(IT)(\d{11})$",                           //** Italy 
            @"^(LV)(\d{11})$",                           //** Latvia 
            @"^(LT)(\d{9}|\d{12})$",                     //** Lithunia
            @"^(LU)(\d{8})$",                            //** Luxembourg 
            @"^(MT)(\d{8})$",                            //** Malta
            @"^(NL)(\d{9})B\d{2}$",                      //** Netherlands
            @"^(PL)(\d{10})$",                           //** Poland
            @"^(PT)(\d{9})$",                            //** Portugal
            @"^(RO)(\d{2,10})$",                         //** Romania
            @"^(SI)(\d{8})$",                            //** Slovenia
            @"^(SK)(\d{9}|\d{10})$",                     // Slovakia Republic
            @"^(SE)(\d{10}\d[1-4])$",                    //** Sweden
        };

        public override bool IsValid(object value)
        {
            //Only validate when there is input
            //Other validators should handle any Required attributes
            if (value == null || String.IsNullOrEmpty(value.ToString()))
            {
                return true;
            }

            var isValid = false;
            foreach (string anExpression in VatNumberAttribute.vatExpressions)
            {
                var m = Regex.Match(value.ToString(), anExpression);

                if (m.Success)
                {
                    var cCountry = m.Groups[1].Value;
                    var cNumber = m.Groups[0].Value.Substring(cCountry.Length);

                    switch (cCountry)
                    {
                        case "AT":
                            isValid = this.DigitCheckAT(cNumber);
                            break;
                        case "BE":
                            isValid = this.DigitCheckBE(cNumber);
                            break;
                        case "BG":
                            // The SIMA validation rules are incorrect for Bulgarian numbers.
                            //valid = BGVATCheckDigit (cNumber)
                            isValid = true;
                            break;
                        case "CY":
                            isValid = this.DigitCheckCY(cNumber);
                            break;
                        case "CZ":
                            isValid = this.DigitCheckCZ(cNumber);
                            break;
                        case "DE":
                            isValid = this.DigitCheckDE(cNumber);
                            break;
                        case "DK":
                            isValid = this.DigitCheckDK(cNumber);
                            break;
                        case "EE":
                            isValid = this.DigitCheckEE(cNumber);
                            break;
                        case "EL":
                            isValid = this.DigitCheckEL(cNumber);
                            break;
                        case "ES":
                            isValid = this.DigitCheckES(cNumber);
                            break;
                        case "EU":
                            isValid = this.DigitCheckEU(cNumber);
                            break;
                        case "FI":
                            isValid = this.DigitCheckFI(cNumber);
                            break;
                        case "FR":
                            isValid = this.DigitCheckFR(cNumber);
                            break;
                        case "GB":
                            isValid = this.DigitCheckUK(cNumber);
                            break;
                        case "GR":
                            isValid = this.DigitCheckGR(cNumber);
                            break;
                        case "HU":
                            isValid = this.DigitCheckHU(cNumber);
                            break;
                        case "IE":
                            isValid = this.DigitCheckIE(cNumber);
                            break;
                        case "IT":
                            isValid = this.DigitCheckIT(cNumber);
                            break;
                        case "LT":
                            isValid = this.DigitCheckLT(cNumber);
                            break;
                        case "LU":
                            isValid = this.DigitCheckLU(cNumber);
                            break;
                        case "LV":
                            isValid = this.DigitCheckLV(cNumber);
                            break;
                        case "MT":
                            isValid = this.DigitCheckMT(cNumber);
                            break;
                        case "NL":
                            isValid = this.DigitCheckNL(cNumber);
                            break;
                        case "PL":
                            isValid = this.DigitCheckPL(cNumber);
                            break;
                        case "PT":
                            isValid = this.DigitCheckPT(cNumber);
                            break;
                        case "RO":
                            isValid = this.DigitCheckRO(cNumber);
                            break;
                        case "SE":
                            isValid = this.DigitCheckSE(cNumber);
                            break;
                        case "SI":
                            isValid = this.DigitCheckSI(cNumber);
                            break;
                        case "SK":
                            isValid = this.DigitCheckSK(cNumber);
                            break;
                        default:
                            isValid = false;
                            break;
                    }

                    //Matched a country
                    break;
                }
            }

            return isValid;
        }

        private bool DigitCheckSK(string cNumber)
        {
            decimal total = 0;
            int[] multipliers = new int[] { 8, 7, 6, 5, 4, 3, 2 };

            // Extract the next digit and multiply by the counter.
            for (var i = 3; i < 9; i++) {
                total += int.Parse(cNumber.Substring(i, 1)) * multipliers[i - 3];                
            }

            // Establish check digits by getting modulus 11.
            total = 11 - total % 11;
            if (total > 9) {total = total - 10;}

            // Compare it with the last character of the VAT number. If it is the same, 
            // then it's a valid check digit.
            return (total == decimal.Parse(cNumber.Substring(9, 10)));
        }

        private bool DigitCheckUK(string cNumber)
        {
            //Government departments
            if (cNumber.Substring(0, 2) == "GD")
            {
                if (int.Parse(cNumber.Substring(2, 3)) < 500)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (cNumber.Substring(0, 2) == "HA")
            {
                if (int.Parse(cNumber.Substring(2, 3)) > 499)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            //Standard / commercial numbers
            if (cNumber.Length == 9 || cNumber.Length == 10)
            {
                //Early exit validity checking
                if (cNumber.Length == 10 && cNumber.Substring(9, 1) != "3")
                {
                    return false;
                }

                //Calculate the total
                int[] multipliers = new int[] { 8, 7, 6, 5, 4, 3, 2 };
                decimal total = 0;
                for (int i = 0; i < 7; i++)
                {
                    total += int.Parse(cNumber.Substring(i, 1)) * multipliers[i];
                }

                // Old numbers use a simple 97 modulus, but new numbers use an adaptation of that (less
                // 55). Our VAT number could use either system, so we check it against both.
                decimal cDigit = total;
                while (cDigit > 0)
                {
                    cDigit = cDigit - 97;
                }
                cDigit = Math.Abs(cDigit);

                // Get the absolute value and compare it with the last two characters of the
                // VAT number. If the same, then it is a valid traditional check digit.
                if (cDigit == Decimal.Parse(cNumber.Substring(7, 2)))
                {
                    return true;
                }
                else
                {
                    // Now try the new method by subtracting 55 from the check digit if we can - else add 42
                    if (cDigit >= 55) { cDigit = cDigit - 55; }
                    else { cDigit = cDigit + 42; }

                    return (cDigit == Decimal.Parse(cNumber.Substring(7, 2)));
                }
            }
            else
            {
                // We don't check 12 and 13 digit UK numbers - not only can we not find any, 
                // but the information found on the format is contradictory.
                return false;
            }
        }

        private bool DigitCheckSI(string cNumber)
        {
            //Calculate total
            int[] multipliers = new int[] { 8, 7, 6, 5, 4, 3, 2 };
            decimal total = 0;
            for (int i = 0; i < 7; i++)
            {
                total += int.Parse(cNumber.Substring(i, 1)) * multipliers[i];
            }

            //Check digit is mod 11
            int cDigit = 11 - ((int)total % 11);
            if (cDigit > 9) { cDigit = 0; }

            //Valid?
            return (cDigit == int.Parse(cNumber.Substring(7, 1)));
        }

        private bool DigitCheckSE(string cNumber)
        {
            //Caculate the total
            int[] multipliers = new int[] { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
            decimal total = 0;

            for (int i = 0; i < 9; i++)
            {
                decimal tmp = int.Parse(cNumber.Substring(i, 1)) * multipliers[i];
                if (tmp > 9)
                {
                    total += Math.Floor(tmp / 10) + tmp % 10;
                }
                else
                {
                    total += tmp;
                }
            }

            //Caculate the check digit from the total
            var cDigit = 10 - (total % 10);
            if (cDigit == 10) { cDigit = 0; }

            //Valid?
            return (cDigit == decimal.Parse(cNumber.Substring(9, 1)));
        }

        private bool DigitCheckRO(string cNumber)
        {
            //Calculate total
            int[] multipliers = new int[] { 7, 5, 3, 2, 1, 7, 5, 3, 2, 1 };
            decimal total = 0;
            for (int i = 0; i < cNumber.Length - 1; i++)
            {
                total += int.Parse(cNumber.Substring(i, 1)) * multipliers[i];
            }

            //Check digit is mod 11
            int cDigit = ((int)total * 10) % 11;
            if (cDigit == 10) { cDigit = 0; }

            //Valid?
            return (cDigit == int.Parse(cNumber.Substring(cNumber.Length - 1, 1)));
        }

        private bool DigitCheckPT(string cNumber)
        {
            //Calculate total
            int[] multipliers = new int[] { 9, 8, 7, 6, 5, 4, 3, 2 };
            decimal total = 0;
            for (int i = 0; i < 8; i++)
            {
                total += int.Parse(cNumber.Substring(i, 1)) * multipliers[i];
            }

            //Check digit is mod 11
            int cDigit = 11 - (int)total % 11;
            if (cDigit > 9) { cDigit = 0; }

            //Valid?
            return (cDigit == int.Parse(cNumber.Substring(8, 1)));
        }

        private bool DigitCheckPL(string cNumber)
        {
            //Calculate total
            int[] multipliers = new int[] { 6, 5, 7, 2, 3, 4, 5, 6, 7 };
            decimal total = 0;
            for (int i = 0; i < 9; i++)
            {
                total += int.Parse(cNumber.Substring(i, 1)) * multipliers[i];
            }

            //Check digit is mod 11
            int cDigit = (int)total % 11;
            if (cDigit > 9) { cDigit = 0; }

            //Valid?
            return (cDigit == int.Parse(cNumber.Substring(9, 1)));
        }

        private bool DigitCheckNL(string cNumber)
        {
            //Calculate total
            int[] multipliers = new int[] { 9, 8, 7, 6, 5, 4, 3, 2 };
            decimal total = 0;
            for (int i = 0; i < 8; i++)
            {
                total += int.Parse(cNumber.Substring(i, 1)) * multipliers[i];
            }

            //Check digit is mod 11
            int cDigit = (int)total % 11;
            if (cDigit > 9) { cDigit = 0; }

            //Valid?
            return (cDigit == int.Parse(cNumber.Substring(8, 1)));
        }

        private bool DigitCheckMT(string cNumber)
        {
            //Calculate total
            int[] multipliers = new int[] { 3, 4, 6, 7, 8, 9 };
            decimal total = 0;
            for (int i = 0; i < 6; i++)
            {
                total += int.Parse(cNumber.Substring(i, 1)) * multipliers[i];
            }

            //Check digit is mod 37
            int cDigit = 37 - (int)total % 37;

            return (cDigit == int.Parse(cNumber.Substring(6, 2)));
        }

        private bool DigitCheckLV(string cNumber)
        {
            // Only check the legal bodies
            if (Regex.IsMatch(@"^[0-3]]", cNumber))
            {
                return false;
            }

            //Calculate total
            int[] multipliers = new int[] { 9, 1, 4, 8, 3, 10, 2, 5, 7, 6 };
            decimal total = 0;
            for (int i = 0; i < 10; i++)
            {
                total += int.Parse(cNumber.Substring(i, 1)) * multipliers[i];
            }

            //Determing test digit
            int cDigit = 0;
            if (total % 11 == 4 && cNumber[0] == '9')
            {
                total -= 45;
            }
            if (total % 11 == 4)
            {
                cDigit = 4 - (int)total % 11;
            }
            else if (total % 11 > 4)
            {
                cDigit = 14 - (int)total % 11;
            }
            else
            {
                cDigit = 3 - (int)total % 11;
            }

            //Valid?
            return (cDigit == int.Parse(cNumber.Substring(10, 1)));
        }

        private bool DigitCheckLU(string cNumber)
        {
            //easy peasy
            return (int.Parse(cNumber.Substring(0, 6)) % 89 == int.Parse(cNumber.Substring(6, 2)));
        }

        private bool DigitCheckLT(string cNumber)
        {
            if (cNumber.Length != 9)
            {
                //None standard VAT number
                return false;
            }

            int total = 0;
            // Extract the next digit and multiply by the counter+1.for (int i = 0; i < 8; i++)
            for (int i = 0; i < 8; i++)
            {
                total += int.Parse(cNumber.Substring(i, 1)) * (i + 1);
            }

            // Can have a double check digit calculation!
            if (total % 11 == 10)
            {
                int[] multipliers = new int[] { 3, 4, 5, 6, 7, 8, 9, 1 };
                total = 0;
                for (int i = 0; i < 8; i++)
                {
                    total += int.Parse(cNumber.Substring(i, 1)) * multipliers[i];
                }
            }

            //Check digit
            int cDigit = (int)total % 11;
            if (cDigit == 10) { cDigit = 0; }

            //Valid?
            return (cDigit == int.Parse(cNumber.Substring(8, 1)));
        }

        private bool DigitCheckIT(string cNumber)
        {
            // The last three digits are the issuing office, and cannot exceed more 201
            int office = int.Parse(cNumber.Substring(7));
            if (office < 1 || office > 201)
            {
                return false;
            }

            //Calc total
            int[] multipliers = new int[] { 1, 2, 1, 2, 1, 2, 1, 2, 1, 2 };
            decimal total = 0;
            for (int i = 0; i < 7; i++)
            {
                decimal tmp = int.Parse(cNumber.Substring(i, 1)) * multipliers[i];
                if (tmp > 9)
                {
                    total += Math.Floor(tmp / 10) + tmp % 10;
                }
                else
                {
                    total += tmp;
                }
            }

            //Check digit
            int cDigit = (int)(10 - total % 10);
            if (cDigit > 9) { cDigit = 0; }

            //Valid?
            return (cDigit == int.Parse(cNumber.Substring(10, 1)));
        }

        private bool DigitCheckIE(string cNumber)
        {
            //Convert old formats to new
            if (Regex.IsMatch(@"^\d[A-Z\*\+]", cNumber))
            {
                cNumber = "0" + cNumber.Substring(2, 7) + cNumber.Substring(0, 1) + cNumber.Substring(7, 8);
            }

            //Calc total
            int[] multipliers = new int[] { 8, 7, 6, 5, 4, 3, 2 };
            var total = 0;
            for (int i = 0; i < 7; i++)
            {
                total += int.Parse(cNumber.Substring(i, 1)) * multipliers[i];
            }

            //Check digit - Mod23, ascii(?) equivalent
            char cDigit = 'W';
            total = total % 23;
            if (total > 0)
            {
                cDigit = (char)(total + 64);
            }

            //Valid?
            return (cDigit == cNumber.Substring(7, 1)[0]);
        }

        private bool DigitCheckGR(string cNumber)
        {
            //Cannot be validated
            return false;
        }

        private bool DigitCheckHU(string cNumber)
        {
            //Calculate the total
            int[] multipliers = new int[] { 9, 7, 3, 1, 9, 7, 3 };
            var total = 0;
            for (int i = 0; i < 7; i++)
            {
                total += int.Parse(cNumber.Substring(i, 1)) * multipliers[i];
            }

            //Calc check digit, mod11
            int cDigit = 10 - total % 10;
            if (cDigit == 10) { cDigit = 0; }

            //Valid
            return (cDigit == int.Parse(cNumber.Substring(7, 1)));
        }

        private bool DigitCheckFR(string cNumber)
        {
            if (!Regex.IsMatch(cNumber, @"^\d{11}$"))
            {
                return false;
            }

            // Extract the last nine digits as an integer.
            // Establish check digit.
            int cDigit = (int.Parse(cNumber.Substring(2)) * 100 + 12) % 97;

            //Valid?
            return (cDigit == int.Parse(cNumber.Substring(0, 2)));
        }

        private bool DigitCheckFI(string cNumber)
        {
            //Calculate the total
            int[] multipliers = new int[] { 7, 9, 10, 5, 8, 4, 2 };
            var total = 0;
            for (int i = 0; i < 7; i++)
            {
                total += int.Parse(cNumber.Substring(i, 1)) * multipliers[i];
            }

            //Calc check digit, mod11
            int cDigit = 11 - total % 11;
            if (cDigit > 9) { cDigit = 0; }

            //Valid
            return (cDigit == int.Parse(cNumber.Substring(7, 1)));
        }

        private bool DigitCheckEU(string cNumber)
        {
            //No way to validate these atm
            return true;
        }

        private bool DigitCheckES(string cNumber)
        {
            int[] multipliers = new int[] { 256, 128, 64, 32, 16, 8, 4, 2 };
            decimal total = 0;

            if (Regex.IsMatch(@"^[A-H]\d{8}$", cNumber))
            {
                //With-Profit company
                //Calc total
                for (int i = 0; i < 7; i++)
                {
                    decimal temp = int.Parse(cNumber.Substring(i, 1)) * multipliers[i];
                    if (temp > 9)
                        total = total + Math.Floor(temp / 10) + temp % 10;
                    else
                        total += temp;
                }

                //Calc check digit - Mod10
                int cDigit = (int)(10 - total % 10);
                if (cDigit == 10) { cDigit = 0; }

                //Valid?
                return (cDigit == int.Parse(cNumber.Substring(8, 1)));

            }
            else if (Regex.IsMatch(@"^[N|P|Q|S]\d{7}[A-Z]$", cNumber))
            {
                //Calc total
                for (int i = 0; i < 7; i++)
                {
                    decimal temp = int.Parse(cNumber.Substring(i, 1)) * multipliers[i];
                    if (temp > 9)
                        total = total + Math.Floor(temp / 10) + temp % 10;
                    else
                        total += temp;
                }

                //Check digit - ascii(?) character based on Mod10
                char cDigit = (char)((10 - total % 10) + 64);

                //Valid?
                return (cDigit == cNumber.Substring(8, 1)[0]);
            }
            else
            {
                //Individual etc.
                return false;
            }
        }

        private bool DigitCheckEL(string cNumber)
        {
            //eight character numbers should be prefixed with an 0.
            if (cNumber.Length == 8) { cNumber = "0" + cNumber; }

            //Calculate the total
            int[] multipliers = new int[] { 256, 128, 64, 32, 16, 8, 4, 2 };
            var total = 0;
            for (int i = 0; i < 8; i++)
            {
                total += int.Parse(cNumber.Substring(i, 1)) * multipliers[i];
            }

            //Calc check digit, mod11
            int cDigit = total % 11;
            if (cDigit > 9) { cDigit = 0; }

            //Valid
            return (cDigit == int.Parse(cNumber.Substring(8, 1)));
        }

        private bool DigitCheckEE(string cNumber)
        {
            //Calculate the total
            int[] multipliers = new int[] { 2, 7, 6, 5, 4, 3, 2, 1 };
            int total = 0;
            for (int i = 0; i < 8; i++)
            {
                total += int.Parse(cNumber.Substring(i, 1)) * multipliers[i];
            }

            //Calc check digit, mod10
            int cDigit = 10 - total % 10;
            if (cDigit == 10) { cDigit = 0; }

            //Valid?
            return (cDigit == int.Parse(cNumber.Substring(8, 1)));
        }

        private bool DigitCheckDK(string cNumber)
        {
            //Calculate the total
            int[] multipliers = new int[] { 2, 7, 6, 5, 4, 3, 2, 1 };
            int total = 0;
            for (int i = 0; i < 8; i++)
            {
                total += int.Parse(cNumber.Substring(i, 1)) * multipliers[i];
            }

            //Mod11 of total should be zero to be valid
            return (total % 11 == 0);
        }

        private bool DigitCheckDE(string cNumber)
        {
            //Calculate the total via a (very) weird algorithm
            int product = 10;
            int sum = 0;
            for (int i = 0; i < 8; i++)
            {
                sum = (int.Parse(cNumber.Substring(i, 1)) + product) % 10;
                if (sum == 0) { sum = 10; }
                product = (2 * sum) % 11;
            }

            //Calculate check digit
            int cDigit = 11 - product;
            if (cDigit == 10) { cDigit = 0; }

            //Valid?
            return (cDigit == int.Parse(cNumber.Substring(8, 1)));
        }

        private bool DigitCheckCZ(string cNumber)
        {
            //Only validate standard VAT numbers
            if (cNumber.Length != 8)
            {
                return false;
            }

            //Calculate the total
            int[] multipliers = new int[] { 8, 7, 6, 5, 4, 3, 2 };
            int total = 0;
            for (int i = 0; i < 7; i++)
            {
                total += int.Parse(cNumber.Substring(i, 1)) * multipliers[i];
            }

            //Caculate the check digit
            int cDigit = 11 - total % 11;
            if (cDigit == 10) { cDigit = 0; }
            if (cDigit == 11) { cDigit = 1; }

            //Valid?
            return (cDigit == int.Parse(cNumber.Substring(7, 1)));
        }

        private bool DigitCheckCY(string cNumber)
        {
            //Parse the supplied number to generate a check-digit total
            int total = 0;
            for (int i = 0; i < 8; i++)
            {
                int tmp = int.Parse(cNumber.Substring(i, 1));
                //We do some funky stuff with even digits
                if (i % 2 == 0)
                {
                    switch (tmp)
                    {
                        case 0:
                            tmp = 1;
                            break;
                        case 1:
                            tmp = 0;
                            break;
                        case 2:
                            tmp = 5;
                            break;
                        case 3:
                            tmp = 7;
                            break;
                        case 4:
                            tmp = 9;
                            break;
                        default:
                            tmp = tmp * 2 + 3;
                            break;
                    }
                }

                total += tmp;
            }

            //Check digit is ascii(?) character of mod26 of total
            char cChar = (char)((total % 26) + 65);

            //Valid?
            return (cChar == cNumber.Substring(8, 1)[0]);
        }

        private bool DigitCheckBE(string cNumber)
        {
            //Length 10 numbers should start with a zero
            if (cNumber.Length == 10 && cNumber.Substring(0, 1) != "0")
            {
                return false;
            }

            //Pad out with a zero if only 9 chars
            if (cNumber.Length == 9)
            {
                cNumber = "0" + cNumber;
            }

            //Check digit is mod 97 of the entire preceeding string
            //Valid?
            return ((97 - int.Parse(cNumber.Substring(0, 8)) % 97) == int.Parse(cNumber.Substring(8)));
        }

        private bool DigitCheckAT(string cNumber)
        {
            //Caculate the total
            int[] multipliers = new int[] { 1, 2, 1, 2, 1, 2, 1 };
            decimal total = 0;

            for (int i = 0; i < 7; i++)
            {
                decimal tmp = int.Parse(cNumber.Substring(i, 1)) * multipliers[i];
                if (tmp > 9)
                {
                    total += Math.Floor(tmp / 10) + tmp % 10;
                }
                else
                {
                    total += tmp;
                }
            }

            //Caculate the check digit from the total
            var cDigit = 10 - (total - 4) % 10;
            if (cDigit == 10) { cDigit = 0; }

            //Valid?
            return (cDigit == decimal.Parse(cNumber.Substring(7, 1)));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = base.ErrorMessageString,
                ValidationType = "vatnumber"
            };
        }
    }
}