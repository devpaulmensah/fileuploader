using System.ComponentModel.DataAnnotations;
using FileUploader.API.Attributes;

namespace FileUploader.API.Models.Requests;

public class UploadFilesRequest
{
    [Required]
    public Guid LoanRequestId { get; set; }
    [Required] 
    [AllowedFileTypesOnly] 
    public IFormFile Passport { get; set; }
    [Required] 
    [AllowedFileTypesOnly] 
    public IFormFile GhanaCard { get; set; }
    [Required] 
    [AllowedFileTypesOnly] 
    public IFormFile VotersId { get; set; }
    [Required] 
    [AllowedFileTypesOnly] 
    public IFormFile Nhis { get; set; }
    [Required] 
    [AllowedFileTypesOnly] 
    public IFormFile BirthCertificate { get; set; }
}