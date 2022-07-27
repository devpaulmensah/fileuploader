using System.Net.Mime;
using FileUploader.API.Models.Requests;
using FileUploader.API.Models.Responses;
using FileUploader.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FileUploader.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse<NoDataResponse>))]
public class FilesController : ControllerBase
{
    private readonly IFileProcessorService _fileProcessorService;

    public FilesController(IFileProcessorService fileProcessorService)
    {
        _fileProcessorService = fileProcessorService;
    }

    /// <summary>
    /// Upload files
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BaseResponse<FileUploadResponse>))]
    [SwaggerOperation(nameof(UploadFiles), OperationId = nameof(UploadFiles))]
    public async Task<IActionResult> UploadFiles([FromForm] UploadFilesRequest request)
    {
        var response = await _fileProcessorService.UploadMultipleFilesAsync(request);
        return StatusCode(response.Code, response);
    }
}