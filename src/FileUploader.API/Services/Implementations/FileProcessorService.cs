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
            var fileUploadTasks = new List<Task<string>>();
            
            // Process passport file
            if (request.Passport is not null)
            {
                fileUploadTasks.Add(SaveFileAsync(loanRequestIdString,
                    FileExtensions.DocumentTypes.Passport, 
                    request.Passport));
            }
            
            // Process ghana card file
            if (request.GhanaCard is not null)
            {
                fileUploadTasks.Add(SaveFileAsync(loanRequestIdString,
                    FileExtensions.DocumentTypes.GhanaCard, 
                    request.GhanaCard));
            }
            
            // Process voters id file
            if (request.VotersId is not null)
            {
                fileUploadTasks.Add(SaveFileAsync(loanRequestIdString,
                    FileExtensions.DocumentTypes.VoterId, 
                    request.VotersId));
            }
            
            // Process nhis files
            if (request.Nhis is not null)
            {
                fileUploadTasks.Add(SaveFileAsync(loanRequestIdString,
                    FileExtensions.DocumentTypes.Nhis, 
                    request.Nhis));
            }
            
            // Process birth certificates
            if (request.BirthCertificate is not null)
            {
                fileUploadTasks.Add(SaveFileAsync(loanRequestIdString,
                    FileExtensions.DocumentTypes.BirthCertificate, 
                    request.BirthCertificate));
            }

            var fileUploadResponseList = (await Task.WhenAll(fileUploadTasks)).ToList();
            
            var response = new FileUploadResponse
            {
                LoanRequestId = request.LoanRequestId,
                Passport = fileUploadResponseList.FirstOrDefault(x => x.Contains(FileExtensions.DocumentTypes.Passport)),
                GhanaCard = fileUploadResponseList.FirstOrDefault(x => x.Contains(FileExtensions.DocumentTypes.GhanaCard)),
                VotersId = fileUploadResponseList.FirstOrDefault(x => x.Contains(FileExtensions.DocumentTypes.VoterId)),
                Nhis = fileUploadResponseList.FirstOrDefault(x => x.Contains(FileExtensions.DocumentTypes.Nhis)),
                BirthCertificate = fileUploadResponseList.FirstOrDefault(x => x.Contains(FileExtensions.DocumentTypes.BirthCertificate))
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
            if (!file.GetFileExtension().IsAllowed(_filesConfig.Value.AllowedFiles)) return string.Empty;
            
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
}