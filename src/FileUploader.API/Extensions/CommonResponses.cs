using System.Net;
using FileUploader.API.Models.Responses;

namespace FileUploader.API.Extensions;

public static class CommonResponses
{
    public const string InternalServerErrorResponseMessage = "Something bad happened, try again later";
    private const string DefaultCreatedResponseMessage = "Created successfully";
    
    public static class ErrorResponse
    {
        public static BaseResponse<T> InternalServerErrorResponse<T>() =>
            new BaseResponse<T>
            {
                Code = (int) HttpStatusCode.InternalServerError,
                Message = InternalServerErrorResponseMessage
            };
    }

    public static class SuccessResponse
    {
        public static BaseResponse<T> CreatedResponse<T>(T data, string? message = null) =>
            new BaseResponse<T>
            {
                Code = (int) HttpStatusCode.Created,
                Message = message ?? DefaultCreatedResponseMessage,
                Data = data
            };
    }

}
