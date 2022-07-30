using FileUploader.API.Configurations;

namespace FileUploader.API.Extensions;

public static class ConfigurationExtension
{
    private static IConfigurationRoot GetConfigurationRoot()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json", true, true)
            .Build();
    }

    public static FilesConfig GetFilesConfig()
    {
        var filesConfig = new FilesConfig();
        GetConfigurationRoot()
            .GetSection("FilesConfig")
            .Bind(filesConfig);
        
        return filesConfig;
    }
}