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

    public RequestsController(IRequestData requestData, IFileService fileService)
    {
        _requestData = requestData;
        _fileService = fileService;
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
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        var result = await _requestData.GetRequest(id);

        if (int.Parse(userId) == result.UserId)
        {
            return Ok(result);
        }

        return NotFound("Record not found or you don't have access to this record!");
    }
    #endregion

    #region POST
    [HttpPost]
    public async Task<IActionResult> CreateRequest([FromForm] RequestModel model)
    {
        BlobResponseDto uploadResult;
        // Global unique ID = GUID ##### we use this guid, for naming our files that we want to have unique name
        // Everytime this method calls a new GUID creates and we can use this name for our file naming convesions.
        var guid = Guid.NewGuid();
        var filePath = Path.Combine("BlobAddress", guid+".jpg");

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
        var result = _requestData.PostRequest(int.Parse(userId), model.CategoryId, model.Subject, model.RequestDocument, model.RequestComment);

        return Ok(result);
    }
    #endregion

    #region PUT
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRequest(int id, [FromForm] RequestModel model)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var result = _requestData.UpdateRequest(id, model.CategoryId, model.Subject, model.RequestDocument, model.RequestComment);

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
}
