using LibraryCMCM.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiCMCM.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly IFileService _fileService;

    public FilesController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ListAllBlobs()
    {
        var result = await _fileService.ListAsync();
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var result = await _fileService.UploadAsync(file);
        return Ok(result);
    }

    [HttpGet]
    [Route("DownloadFile")]
    [AllowAnonymous]
    public async Task<IActionResult> Download([FromBody] string filename)
    {
        var result = await _fileService.DownloadAsync(filename);
        return File(result.Content, result.ContentType, result.Name);
    }

    [HttpDelete]
    [Route("filename")]
    [AllowAnonymous]
    public async Task<IActionResult> Delete(string filename)
    {
        var result = await _fileService.DeleteAsync(filename);
        return Ok(result);
    }
}
