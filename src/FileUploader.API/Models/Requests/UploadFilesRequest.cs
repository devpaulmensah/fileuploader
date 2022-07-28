using System.ComponentModel.DataAnnotations;

namespace FileUploader.API.Models.Requests;

public class UploadFilesRequest
{
    [Required]
    public Guid LoanRequestId { get; set; }
    public IFormFile? Passport { get; set; }
    public IFormFile? GhanaCard { get; set; }
    public IFormFile? VotersId { get; set; }
    public IFormFile? Nhis { get; set; }
    public IFormFile? BirthCertificate { get; set; }
}