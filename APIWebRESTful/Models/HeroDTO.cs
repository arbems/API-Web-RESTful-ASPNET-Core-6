using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using APIWebRESTful.Validation;

namespace APIWebRESTful.Models
{
    public class HeroDTO
    {
        // Validaciones por atributos
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(maximumLength:30, ErrorMessage = "Maximum Length {0} is 30.")]
        [MayusRequired] // Custom attribute.
        public string Name { get; set; }
        public bool IsPopulate { get; set; }

        // Validaciones por modelo
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name != Name.ToUpper())
                yield return new ValidationResult("Value must be in uppercase.", new string[] { nameof(Name) });
        }
    }
}
