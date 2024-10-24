using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using backend.Services.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace backend.Helpers
{
    public static class StringHelpers
    {
        public static string FormatPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return phone;

            var regexItem = new Regex("^[0-9+]+$");
            if (!regexItem.IsMatch(phone))
                throw new InvalidInputException($"Invalid phone number {phone}");

            if (phone.Length < 10 || phone.Length > 14)
            {
                throw new InvalidInputException($"Invalid phone number length: {phone}");
            }

            if (phone.Length == 10)
                return "234" + phone;
            else if (phone.Length == 11)
            {
                phone = phone.Remove(0, 1);
                return "234" + phone;
            }
            else if (phone.Length == 13)
            {
                return phone;
            }
            else if (phone.Length == 14)
            {
                if (phone.Contains('+'))
                {
                    if (phone.StartsWith("+"))
                    {
                        phone = phone.Replace("+", "");
                        phone = phone.Trim();
                        return phone;
                    }
                    else
                    {
                        throw new InvalidInputException($"invalid phone number format: {phone} ");
                    }

                }
                else
                {
                    throw new InvalidInputException($"unable to  format phone because of invalid length: {phone}");
                }

            }
            return phone;
            //else
            //    throw new Exception($"unable to  format phone because of invalid length: {phone}");

        }
        public static bool IsValidPhoneNumber(this string phone)
        {
            try
            {

                return !string.IsNullOrEmpty(FormatPhoneNumber(phone));
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPin(this string pin)
        {
            if (pin.Length != 6){
                return false;
            }
            return true;
        }

        public static bool IsValidExtension(this string extension)
        {
            if (extension != null && (extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) || extension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }
            return false;
        }

        public static bool IsValidGender(this string gender)
        {
            if (gender != null && (gender.Equals("Male", StringComparison.OrdinalIgnoreCase) || gender.Equals("Female", StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }
            return false;
        }


        








    }
}