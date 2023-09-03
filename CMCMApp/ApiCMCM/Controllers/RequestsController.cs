using ApiCMCM.Constants;
using Azure;
using LibraryCMCM.DataAccess;
using LibraryCMCM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiCMCM.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RequestsController : ControllerBase
{
    private readonly IRequestData _requestData;
    private readonly IFileService _fileService;
    private readonly IDocumentData _documentData;

    public RequestsController(IRequestData requestData, IFileService fileService, IDocumentData documentData)
    {
        _requestData = requestData;
        _fileService = fileService;
        _documentData = documentData;
    }

    #region GET
    [HttpGet]
    public async Task<IActionResult> GetAllRequests()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var result = await _requestData.GetAllRequests(int.Parse(userId));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRequest(int id)
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var result = await _requestData.GetRequest(id);
            if (result is not null)
            {
                if (int.Parse(userId!) == result.UserId)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest("You don't allow to access someone else's records");
                }
            }
            return NotFound("Record not found!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
    #endregion

    #region POST
    [HttpPost]
    public async Task<IActionResult> CreateRequest([FromForm] RequestModel model)
    {
        BlobResponseDto uploadResult;

        // Instead of this if condition we have to replace it with our blob storage saving method ******* NOTICE NOTICE
        if (model.DocFile != null)
        {
            uploadResult = await _fileService.UploadAsync(model.DocFile);
        }
        else
        {
            return BadRequest("Please upload your file");
        }

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var documentTableResult = await _documentData.PostDocument(int.Parse(userId), uploadResult.Blob.Name, CustomConstants.Invoices + uploadResult.Blob.Name);
        var requestResult = await _requestData.PostRequest(int.Parse(userId), model.CategoryId, documentTableResult.id, model.Subject, model.RequestComment);

        var response = PostResponse(requestResult, documentTableResult);

        return Ok(response);
    }
    #endregion

    #region PUT
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRequest(int id, [FromForm] RequestModel model)
    {
        BlobResponseDto uploadResult = new();
        RequestModel reqModel = new();
        DocumentModel docModel = new();

        // Instead of this if condition we have to replace it with our blob storage saving method ******* NOTICE NOTICE
        if (model.DocFile is not null)
        {
            reqModel = await _requestData.GetRequest(id);
            docModel = await _documentData.GetDocument(reqModel.DocumentId);
            var blobDel = await _fileService.DeleteAsync(docModel.FileName);
            uploadResult = await _fileService.UploadAsync(model.DocFile);


            docModel = await _documentData.UpdateDocument(docModel.id, reqModel.UserId, uploadResult.Blob.Name, CustomConstants.Invoices + uploadResult.Blob.Name);
        }

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var result = _requestData.UpdateRequest(id, model.CategoryId, docModel.id, model.Subject, model.RequestComment);

        return Ok("Your request updated");
    }
    #endregion

    #region DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRequest(int id)
    {
        var result = _requestData.DeleteRequest(id);
        return Ok("Your request deleted");
    }
    #endregion

    private RequestResponse PostResponse(RequestModel rModel, DocumentModel dModel)
    {
        RequestResponse response = new RequestResponse();
        string fileCorePath = "https://cmcmstorage.blob.core.windows.net/";

        response.Id = rModel.Id;
        response.UserId = rModel.UserId;
        response.CategoryId = rModel.CategoryId;
        response.DocumentId = rModel.DocumentId;
        response.FileName = dModel.FileName;
        response.Subject = rModel.Subject;
        response.RequestComment = rModel.RequestComment;
        response.StatusCode = (int)rModel.StatusCode;
        response.IsComplete = rModel.IsComplete;
        response.IsArchived = rModel.IsArchived;
        response.DocumentUrl = fileCorePath + dModel.FilePathUrl;

        return response;
    }
}
