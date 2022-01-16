using System;
using System.ComponentModel.DataAnnotations;

namespace APIWebRESTful.Validation
{
    public class MayusRequiredAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var val = value.ToString();

            if (string.IsNullOrEmpty(val)) return ValidationResult.Success;

            if (val != val.ToUpper())
                return new ValidationResult("Value must be in uppercase.");

            return ValidationResult.Success;
        }
    }
}
