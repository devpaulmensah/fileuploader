namespace FileUploader.API.Configurations;

public class FilesConfig
{
    public string BaseUrl { get; set; }
    public string RootPath { get; set; }
    public string[] AllowedFiles { get; set; }
}