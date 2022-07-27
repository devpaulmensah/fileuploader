using System.ComponentModel.DataAnnotations;

namespace FileUploader.API.Models.Requests;

public class UploadFilesRequest
{
    [Required]
    public Guid LoanRequestId { get; set; }
    public IFormFile[] Passports { get; set; } = Array.Empty<IFormFile>();
    public IFormFile[] GhanaCard { get; set; } = Array.Empty<IFormFile>();
    public IFormFile[] VotersId { get; set; } = Array.Empty<IFormFile>();
    public IFormFile[] Nhis { get; set; } = Array.Empty<IFormFile>();
    public IFormFile[] BirthCertificates { get; set; } = Array.Empty<IFormFile>();
}