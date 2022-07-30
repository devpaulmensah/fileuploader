namespace FileUploader.API.Configurations;

public class FilesConfig
{
    public string BaseUrl { get; set; }
    public string RootFilePath { get; set; }
    public string[] AllowedFilesExtensions { get; set; }
}