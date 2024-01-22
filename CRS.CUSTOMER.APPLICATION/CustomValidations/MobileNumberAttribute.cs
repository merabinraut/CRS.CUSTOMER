using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CRS.CUSTOMER.APPLICATION.CustomValidations
{
    public class MobileNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (!(value is string))
                return false;

            string mobileNumber = (string)value;
            return IsValidMobileNumberFormat(mobileNumber);
        }
        private bool IsValidMobileNumberFormat(string mobileNumber)
        {
            string[] validPrefixes = { "090", "6490", "3254" };

            return validPrefixes.Any(prefix => mobileNumber.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
        }
    }
}