using System;
using System.IO;
using System.Threading.Tasks;
using FileUploader.API.Extensions;
using FileUploader.API.Services.Interfaces;
using FileUploader.Tests.TestSetup;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FileUploader.Tests;

public class FileProcessorServiceShould : IClassFixture<FileUploadFixture>
{
    private readonly IFileProcessorService _fileProcessorService;

    public FileProcessorServiceShould(FileUploadFixture fileUploadFixture)
    {
        _fileProcessorService = fileUploadFixture.ServiceProvider
            .GetRequiredService<IFileProcessorService>();
    }

    [Fact]
    public async Task Return_Empty_String_When_No_File_Is_Provided()
    {
        // Arrange
        var loanRequestId = Guid.NewGuid().ToString("D");

        // Act
        var response = await _fileProcessorService.SaveFileAsync(loanRequestId, FileExtensions.DocumentTypes.TestDocument, null);

        // Assert
        response.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Return_Url_When_An_Allowed_File_Is_Provided()
    {
        // Arrange
        const string filename = "allowed_file.png";
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", filename);
        var loanRequestId = Guid.NewGuid().ToString("D");
        var fileBytes = await File.ReadAllBytesAsync(filePath);

        // Act
        string response;
        using (var stream = new MemoryStream(fileBytes))
        {
            IFormFile file = new FormFile(stream, 0, fileBytes.Length, "allowed_file", filename);
            response = await _fileProcessorService.SaveFileAsync(loanRequestId, FileExtensions.DocumentTypes.TestDocument, file);
        };
        
        // Assert
        response.Should().NotBeNullOrEmpty();
        response.ToLower().Should().Contain(FileExtensions.DocumentTypes.TestDocument.ToLower());
        response.Should().Contain("http");
        
        // Delete created files to save space
        DeleteTestDocumentFolder();
    }
    
    [Fact]
    public async Task Return_Empty_String_When_An_Unacceptable_File_Is_Provided()
    {
        // Arrange
        const string filename = "forbidden_file.xlsx";
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", filename);
        var loanRequestId = Guid.NewGuid().ToString("D");
        var fileBytes = await File.ReadAllBytesAsync(filePath);

        // Act
        string response;
        using (var stream = new MemoryStream(fileBytes))
        {
            IFormFile file = new FormFile(stream, 0, fileBytes.Length, "forbidden_file", filename);
            response = await _fileProcessorService.SaveFileAsync(loanRequestId, FileExtensions.DocumentTypes.TestDocument, file);
        };
        
        // Assert
        response.Should().BeNullOrEmpty();
    }

    private static void DeleteTestDocumentFolder()
    {
        var folderName = FileExtensions.FolderName.GetFolderName(FileExtensions.DocumentTypes.TestDocument);
        var baseFilesPath = ConfigurationExtension.GetFilesConfig().RootFilePath;

        var fullPath = Path.Combine(baseFilesPath, folderName);
        
        foreach (var filename in Directory.EnumerateFiles(fullPath))
        {
            File.Delete(filename);
        }
        
        Directory.Delete(fullPath);
    }
}