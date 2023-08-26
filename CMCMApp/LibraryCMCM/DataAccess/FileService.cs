using Azure.Storage;
using Azure.Storage.Blobs;
using LibraryCMCM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace LibraryCMCM.DataAccess;

public class FileService : IFileService
{
    private readonly BlobContainerClient _filesContainer;
    private readonly IConfiguration _config;

    public FileService(IConfiguration config)
    {
        _config = config;

        var credential = new StorageSharedKeyCredential(_config.GetSection("FileService:StorageAccount").ToString(), _config.GetSection("FileService:Key").ToString());
        var blobUri = $"https://{_config.GetSection("FileService:StorageAccount")}.blob.core.windows.net";
        var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
        _filesContainer = blobServiceClient.GetBlobContainerClient("files");
    }

    public async Task<List<BlobDto>> ListAsync()
    {
        List<BlobDto> files = new List<BlobDto>();

        await foreach (var file in _filesContainer.GetBlobsAsync())
        {
            string uri = _filesContainer.Uri.ToString();
            var fileName = file.Name;
            var fileUri = $"{uri}/{fileName}";

            files.Add(new BlobDto
            {
                Uri = uri,
                Name = fileName,
                ContentType = file.Properties.ContentType
            });
        }

        return files;
    }

    public async Task<BlobResponseDto> UploadAsync(IFormFile blob)
    {
        BlobResponseDto response = new();
        BlobClient client = _filesContainer.GetBlobClient(blob.FileName);

        await using (Stream? data = blob.OpenReadStream())
        {
            await client.UploadAsync(data);
        }

        response.Status = $"File {blob.FileName} Uploaded Successfully";
        response.Error = false;
        response.Blob.Uri = client.Uri.AbsoluteUri;
        response.Blob.Name = client.Name;

        return response;
    }

    public async Task<BlobDto?> DownloadAsync(string blobFileName)
    {
        BlobClient file = _filesContainer.GetBlobClient(blobFileName);

        if (await file.ExistsAsync())
        {
            var data = await file.OpenReadAsync();
            Stream blobContenct = data;

            var content = await file.DownloadContentAsync();

            string name = blobFileName;
            string contentType = content.Value.Details.ContentType;


            return new BlobDto { Content = blobContenct, Name = name, ContentType = contentType };
        }

        return null;
    }


    public async Task<BlobResponseDto> DeleteAsync(string blobFileName)
    {
        BlobClient file = _filesContainer.GetBlobClient(blobFileName);
        await file.DeleteAsync();
        BlobResponseDto response = new BlobResponseDto { Error = false, Status = $"File: {blobFileName} has successfully deleted." };
        return response;
    }

}

