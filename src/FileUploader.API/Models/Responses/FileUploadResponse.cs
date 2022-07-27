namespace FileUploader.API.Models.Responses;

public class FileUploadResponse
{
    public Guid LoanRequestId { get; set; }
    public string[] Passport { get; set; } = Array.Empty<string>();
    public string[] GhanaCard { get; set; } = Array.Empty<string>();
    public string[] VotersId { get; set; } = Array.Empty<string>();
    public string[] Nhis { get; set; } = Array.Empty<string>();
    public string[] BirthCertificates { get; set; } = Array.Empty<string>();

}