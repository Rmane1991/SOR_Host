using System;
using System.Net;
using System.Text.RegularExpressions;
using AppLogger;

namespace BussinessAccessLayer
{
    public class clsCustomeRegularExpressions
    {
        #region enum Validators
        public enum Validators
        {
            AccountNumber = 1,
            Address = 2,
            AdharCard = 3,
            Date = 4,
            DrivingLisence = 5,
            Email = 6,
            Mobile = 7,
            Name = 8,
            NumberOnly = 9,
            PanCard = 10,
            Pincode = 11,
            TextWithNumbers = 12,
            TextWithNumbersCharacters = 13,
            TextWithNumbersCharacterSpace = 14,
            TextWithoutSpace = 15,
            TextWithSpace = 16,
            Time = 17,
            Website = 18,
            GSTIN = 19,
            Landline = 20,
            IpAddress = 21,
            Percentage = 22,
            IFSC = 23,
            Amount = 24,
            Passport = 25,
            NUMERIC = 26,
            ALPHANUMERIC = 27,
            CommaSeperatedMobileNumber = 28,
            SlashSeperatedMobileNumber = 29,
            date2=30,
            TextWithSpaceSC = 31,
            GeoCode=32,
        }
        #endregion

        #region CustomeRegExpValidation
        public bool CustomeRegExpValidation(Validators _Validators, string _InputValue)
        {
            bool _IsValid = false;
            string _RegularExpression = string.Empty;
            _InputValue = _InputValue.Trim();
            try
            {
                switch (_Validators.ToString())
                {
                    case "Mobile":
                        _RegularExpression = "^[1-9][0-9]{9}$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "Landline":
                        _RegularExpression = "^[0-9]{3}[-][0-9]{6,8}$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "Address":
                        _RegularExpression = @"^([a-z]|[A-Z]|[0-9]|[-\s/=$*#_?,:'.()])[a-zA-Z0-9-\s/=$*#_?,:'.()]*$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "Email":
                        _RegularExpression = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "Pincode":
                        _RegularExpression = "^[1-9][0-9]{5}$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "AccountNumber":
                        _RegularExpression = "^([0-9]){19}$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "PanCard":
                        _RegularExpression = @"^([A-Z]){5}([0-9]){4}([A-Z]){1}$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "AdharCard":
                        _RegularExpression = "^([0-9]){12}$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "DrivingLisence":
                        _RegularExpression = "";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "Date":
                        /// dd//mm/yyyy
                        _RegularExpression = @"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "Time":
                        /// HH:MM 12-hour format mandatory meridiems (AM/PM)
                        _RegularExpression = @"^((0|1)?0?[0-9]|1[0-2]):[0-5][0-9] [AaPp][Mm]$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "TextWithoutSpace":
                        _RegularExpression = @"^[a-zA-Z]+$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "TextWithSpace":
                        _RegularExpression = "^[a-zA-Z][a-zA-Z\\s]+$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "TextWithSpaceSC":
                        _RegularExpression = "[a-zA-Z]+('[a-zA-Z])?[a-zA-Z]*";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "TextWithNumbers":
                        _RegularExpression = "^[a-zA-Z0-9][a-zA-Z0-9]*$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "TextWithNumbersCharacters":
                        _RegularExpression = @"[a-zA-Z0-9\@\#\$\%\&\*\(\)\-\_\+\]\[\'\;\:\?\.\,\!]+$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "TextWithNumbersCharacterSpace":
                        _RegularExpression = @"^([\d|\w]|[-/=$*#_?.,\(\)@!|\s])+$"; // @"^[\d|\w\s]+$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "Website":
                        _RegularExpression = @"^(((ht|f)tp(s?))\://)?(www.|[a-zA-Z].)[a-zA-Z0-9\-\.]+\.(com|edu|gov|mil|net|org|biz|info|name|museum|us|ca|uk)(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\;\?\'\\\+&amp;%\$#\=~_\-]+))*$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "NumberOnly":
                        _RegularExpression = @"^\d+$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "GSTIN":
                        _RegularExpression = @"^([0][1-9]|[1-2][0-9]|[3][0-7])([a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}[1-9a-zA-Z]{1}[zZ]{1}[0-9a-zA-Z]{1})+$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "IpAddress":
                        try
                        {
                            IPAddress _IPAddress = IPAddress.Parse(_InputValue);
                            _IsValid = true;
                        }
                        catch
                        {
                            _IsValid = false;
                        }
                        break;
                    case "Percentage":
                        _RegularExpression = @"^[\d.]+$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    //case "IFSC":
                    //    _RegularExpression = @"^[^\s]{4}\d{7}$";
                    //    _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                    //    break;
                    case "Amount":
                        _RegularExpression = @"^[+-]?(\d*\.)?\d+$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "CommaSeperatedMobileNumber":
                        _RegularExpression = @"^[0-9]{10}(,|[0-9]{10})*";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "SlashSeperatedMobileNumber":
                        _RegularExpression = @"^[0-9]{10}(/|[0-9]{10})*";// //[0-9A-Za-z/\s-]*
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    //case "Passport":
                    //    _RegularExpression = @" ^ (? !^ 0 +$)[a-zA-Z0-9]{3,20}$*";
                    //    _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                    //    break;

                    case "IFSC":
                        _RegularExpression = @"^[A-Z]{4}0[A-Z0-9]{6}$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;

                    case "Passport":
                        _RegularExpression = @"^[A-PR-WYa-pr-wy][1-9]\d\s?\d{4}[1-9]$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "date2":
                        _RegularExpression = @"((((19|20)\d\d)\-(0[1-9]|1[0-2])\-(0|1)[0-9]|2[0-9]|3[0-1]))$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;
                    case "GeoCode":
                        ////_RegularExpression = @"^\d+$";^(-?\d+(\.\d+)?),\s*(-?\d+(\.\d+)?)$
                        _RegularExpression = @"^(-?\d+(\.\d+)?),\s*(-?\d+(\.\d+)?)$";
                        _IsValid = System.Text.RegularExpressions.Regex.IsMatch(_InputValue, _RegularExpression, RegexOptions.IgnoreCase);
                        break;

                }
            }
            catch (Exception Ex)
            {
                ErrorLog.CommonTrace("Page : clsCustomeRegularExpressions.cs \nFunction : CustomeRegExpValidation() \nException Occured\n" + Ex.Message);
            }
            return _IsValid;
        }
        #endregion

    }
}
