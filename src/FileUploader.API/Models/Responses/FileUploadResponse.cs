namespace FileUploader.API.Models.Responses;

public class FileUploadResponse
{
    public Guid LoanRequestId { get; set; }
    public string? Passport { get; set; }
    public string? GhanaCard { get; set; }
    public string? VotersId { get; set; }
    public string? Nhis { get; set; }
    public string? BirthCertificate { get; set; }
}