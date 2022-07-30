using FileUploader.API.Extensions;
using FluentAssertions;
using Xunit;

namespace FileUploader.Tests;

public class FileExtensionShould
{
    [Theory]
    [InlineData(".pdf")]
    [InlineData(".jpeg")]
    [InlineData(".png")]
    [InlineData(".docx")]
    [InlineData(".doc")]
    [InlineData(".jpg")]
    public void Pass_IsAllowed_For_Allowed_File_Extension(string extension)
    {
        var extensionAllowed = extension.IsAllowed();
        extensionAllowed.Should().BeTrue();
    }
    
    [Theory]
    [InlineData(".exe")]
    [InlineData(".txt")]
    [InlineData(".mp4")]
    [InlineData(".json")]
    public void Fail_IsAllowed_For_File_Extensions_That_Not_Accepted(string extension)
    {
        var extensionAllowed = extension.IsAllowed();
        extensionAllowed.Should().BeFalse();
    }
}