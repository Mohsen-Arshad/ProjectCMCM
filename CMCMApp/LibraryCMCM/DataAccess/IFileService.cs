using LibraryCMCM.Models;
using Microsoft.AspNetCore.Http;

namespace LibraryCMCM.DataAccess;

public interface IFileService
{
    Task<BlobResponseDto> DeleteAsync(string blobFileName);
    Task<BlobDto?> DownloadAsync(string blobFileName);
    Task<List<BlobDto>> ListAsync();
    Task<BlobResponseDto> UploadAsync(IFormFile blob);
}