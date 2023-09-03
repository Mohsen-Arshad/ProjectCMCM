using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryCMCM.CustomValidationAttribute;

public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _fileTypes;

    public AllowedExtensionsAttribute(string[] fileTypes)
    {
        _fileTypes = fileTypes;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!_fileTypes.Contains(fileExtension))
            {
                return new ValidationResult($"Only these file types are allowed: {string.Join(", ", _fileTypes)}");
            }
        }

        return ValidationResult.Success;
    }
}
