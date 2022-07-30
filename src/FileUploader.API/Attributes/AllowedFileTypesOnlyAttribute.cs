using System.ComponentModel.DataAnnotations;
using FileUploader.API.Extensions;

namespace FileUploader.API.Attributes;

public class AllowedFileTypesOnlyAttribute : ValidationAttribute
{
    private const string FileNotAllowedMessage = "File not allowed";

    protected override ValidationResult IsValid(object? value, ValidationContext context)
    {
        try
        {
            if (value is not IFormFile file)
            {
                return new ValidationResult(FileNotAllowedMessage);
            }
            
            var filesConfig = ConfigurationExtension.GetFilesConfig();
            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            return !filesConfig.AllowedFilesExtensions.Contains(fileExtension)
                ? new ValidationResult($"{FileNotAllowedMessage}. ({fileExtension})")
                : ValidationResult.Success!;
        }
        catch (Exception e)
        {
            return new ValidationResult(FileNotAllowedMessage);
        }
    }
}