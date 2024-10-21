using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Dtos
{
    public class SixDigitNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is int intValue)
            {
                // Check if the integer is 6 digits long
                if (intValue >= 100000 && intValue <= 999999)
                {
                    return ValidationResult.Success;
                }
            }
            return new ValidationResult("Token must be exactly 6 digits.");
        }
    }
}