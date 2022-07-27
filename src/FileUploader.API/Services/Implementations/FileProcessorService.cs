using FileUploader.API.Configurations;
using FileUploader.API.Extensions;
using FileUploader.API.Models.Requests;
using FileUploader.API.Models.Responses;
using FileUploader.API.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace FileUploader.API.Services.Implementations;

public class FileProcessorService : IFileProcessorService
{
    private readonly ILogger<FileProcessorService> _logger;
    private readonly IOptions<FilesConfig> _filesConfig;

    public FileProcessorService(ILogger<FileProcessorService> logger,
        IOptions<FilesConfig> filesConfig)
    {
        _logger = logger;
        _filesConfig = filesConfig;
    }

    public async Task<BaseResponse<FileUploadResponse>> UploadMultipleFilesAsync(UploadFilesRequest request)
    {
        try
        {
            var loanRequestIdString = request.LoanRequestId.ToString();

            // Process passport files
            var passportFilesLinks = await ProcessFilesAsync(loanRequestIdString,
                FileExtensions.DocumentTypes.Passport,
                request.Passports.Distinct());
            
            // Process ghana card files
            var ghanaCardFilesLinks = await ProcessFilesAsync(loanRequestIdString,
                FileExtensions.DocumentTypes.GhanaCard,
                request.GhanaCard.Distinct());
            
            // Process voters id files
            var voterIdFilesLinks = await ProcessFilesAsync(loanRequestIdString,
                FileExtensions.DocumentTypes.VoterId,
                request.VotersId.Distinct());
            
            // Process nhis files
            var nhisFilesLinks = await ProcessFilesAsync(loanRequestIdString,
                FileExtensions.DocumentTypes.Nhis,
                request.Nhis.Distinct());
            
            // Process birth certificates
            var birthCertificateFileLinks = await ProcessFilesAsync(loanRequestIdString,
                FileExtensions.DocumentTypes.BirthCertificate,
                request.BirthCertificates.Distinct());
            
            var response = new FileUploadResponse
            {
                LoanRequestId = request.LoanRequestId,
                Passport = passportFilesLinks,
                GhanaCard = ghanaCardFilesLinks,
                VotersId = voterIdFilesLinks,
                Nhis = nhisFilesLinks,
                BirthCertificates = birthCertificateFileLinks
            };

            return CommonResponses.SuccessResponse.CreatedResponse(response, "Files uploaded successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{loanRequestId}] An error occured uploading file for loan request", 
                request.LoanRequestId.ToString());

            return CommonResponses.ErrorResponse.InternalServerErrorResponse<FileUploadResponse>();
        }
    }

    private void CreateDirectoryIfNotFound(string folderName)
    {
        var fullPath = Path.Combine(_filesConfig.Value.RootPath, folderName);
        
        if (Directory.Exists(fullPath)) return;
        
        _logger.LogInformation("Creating directory - {folderName} ....", folderName);
        Directory.CreateDirectory(fullPath);
    }
    
    private async Task<string> SaveFileAsync(string loanRequestId, string documentType, IFormFile file)
    {
        try
        {
            // Check if file is not allowed to be saved
            if (!file.IsAllowed(_filesConfig.Value.AllowedFiles)) return string.Empty;
            
            var folderName = FileExtensions.FolderName.GetFolderName(documentType);
            var fileName = $"{loanRequestId}_{documentType}{file.GetFileExtension().ToLower()}";
            var pathToSave = Path.Combine(_filesConfig.Value.RootPath, folderName, fileName);
            
            // Create directory to store file if it doesn't exist
            CreateDirectoryIfNotFound(folderName);
            
            // Save file to appropriate location
            await using var stream = new FileStream(pathToSave, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"{_filesConfig.Value.BaseUrl}/{folderName}/{fileName}";
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{loanRequestId}] An error occured processing and saving file for loan request", 
                loanRequestId);

            return string.Empty;
        }
    }

    private async Task<string[]> ProcessFilesAsync(string loanRequestId, string documentType, IEnumerable<IFormFile> files)
    {
        var fileLinks = Array.Empty<string>(); 
        var fileUploadsTaskList = files
            .Select(file => 
                SaveFileAsync(loanRequestId, documentType ,file))
            .ToList();

        if (fileUploadsTaskList.Any())
        {
            fileLinks = (await Task.WhenAll(fileUploadsTaskList))
                .Where(link => !string.IsNullOrEmpty(link))
                .ToArray();
        }

        return fileLinks;
    }
}