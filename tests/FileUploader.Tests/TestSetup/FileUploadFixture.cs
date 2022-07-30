using FileUploader.API.Configurations;
using FileUploader.API.Services.Implementations;
using FileUploader.API.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FileUploader.Tests.TestSetup;

public class FileUploadFixture
{
    public ServiceProvider ServiceProvider { get; }
    
    public FileUploadFixture()
    {
        var services = new ServiceCollection();
        ConfigurationManager.SetupConfiguration();

        services.AddSingleton(sp => ConfigurationManager.Configuration);
        services.Configure<FilesConfig>(ConfigurationManager.Configuration.GetSection("FilesConfig"));

        services.AddLogging(x => x.AddConsole());
        services.AddScoped<IFileProcessorService, FileProcessorService>();
        
        ServiceProvider = services.BuildServiceProvider();
    }
}