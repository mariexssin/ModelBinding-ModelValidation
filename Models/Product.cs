using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsValidation.Models
{
    public class Product : IValidatableObject
    {
        public enum Category { Toy, Technique, Clothes, Transport }

        public int Id { get; set; }

        public Category Type { get; set; }

        [Required(ErrorMessage = "Name is mandatory")]
        public string Name { get; set; }

        [MinLength(3, ErrorMessage = "Description length should be longer than 2 symbols")]
        public string Description { get; set; }

        [Range(0, 100000, ErrorMessage = "Price should be within boundaries 0 and 100000")]
        public decimal Price { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(Name))
            {
                if (Description == Name)
                {
                    yield return new ValidationResult(
                        "Description should not be equal to Name",
                        new[] { nameof(Description) });
                }
                else if (!Description.StartsWith(Name))
                {
                    yield return new ValidationResult(
                        "Description should start with the Name of the product",
                        new[] { nameof(Description) });
                }
            }
        }
    }
}