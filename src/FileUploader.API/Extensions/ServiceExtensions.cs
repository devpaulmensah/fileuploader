using System.Reflection;
using FileUploader.API.Configurations;
using FileUploader.API.Services.Implementations;
using FileUploader.API.Services.Interfaces;
using Microsoft.OpenApi.Models;

namespace FileUploader.API.Extensions;

public static class ServiceExtensions
{
    public static void AddServicesAndConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FilesConfig>(configuration.GetSection(nameof(FilesConfig)));
        services.AddScoped<IFileProcessorService, FileProcessorService>();
    }
    
    public static void InitializeSwagger(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));
        
        var version = configuration["SwaggerConfig:Version"];
        var title = configuration["SwaggerConfig:Title"];

        serviceCollection.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(version, new OpenApiInfo
            {
                Contact = new OpenApiContact
                {
                    Email = "paulmensah1409@gmail.com",
                    Name = "Paul Mensah",
                    Url = new Uri("https://paulmensah.dev")
                },
                Version = version,
                Title = title
            });
            c.ResolveConflictingActions(resolver => resolver.First());
            c.EnableAnnotations();
            
            var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            c.IncludeXmlComments(xmlPath);
        });
    }
    
    public static void UseSwaggerDocumentation(this IApplicationBuilder applicationBuilder, IConfiguration configuration)
    {
        var title = configuration["SwaggerConfig:Title"];
        
        applicationBuilder.UseSwagger();
        applicationBuilder.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", title);
        });
    }
}