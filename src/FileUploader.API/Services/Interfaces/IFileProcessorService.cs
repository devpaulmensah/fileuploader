using FileUploader.API.Models.Requests;
using FileUploader.API.Models.Responses;

namespace FileUploader.API.Services.Interfaces;

public interface IFileProcessorService
{
    Task<BaseResponse<FileUploadResponse>> UploadMultipleFilesAsync(UploadFilesRequest request);
}