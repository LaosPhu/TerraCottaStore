using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;

namespace TerraCottaStore.Repository.Validation
{
    public class FileExtensionAttribute :ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
            var extention =Path.GetExtension(file.FileName);
                string[] extentions = { "jpg", "png","jpeg" };
                bool result = extentions.Any(x=> extention.EndsWith(x));
                if (!result)
                {
                    return new ValidationResult("Allow Jpeg|| jpg || png . file only !!");
                }

            }
            return ValidationResult.Success;
           
        }
    }
}
