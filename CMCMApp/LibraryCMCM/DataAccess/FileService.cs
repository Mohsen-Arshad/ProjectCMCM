using Azure.Storage;
using Azure.Storage.Blobs;
using LibraryCMCM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Reflection.Metadata;

namespace LibraryCMCM.DataAccess;

public class FileService : IFileService
{
    private readonly BlobContainerClient _filesContainer;
    private readonly IConfiguration _config;

    public FileService(IConfiguration config)
    {
        _config = config;

        //string storageAccount = "cmcmstorage";
        //string storageKey = "+lPSqADC3CgECy59+MXtZUvZr+6wha6nbOnfftQ7w+wKbdWJfP+MG5Mb9oOSseRxpymGMg5EN9F3+AStTt1lBg==";

        var credential = new StorageSharedKeyCredential(_config.GetValue<string>("FileService:StorageAccount"), _config.GetValue<string>("FileService:Key"));
        var blobUri = $"https://{_config.GetValue<string>("FileService:StorageAccount")}.blob.core.windows.net/invoicesfiles/";
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
        // Global unique ID = GUID ##### we use this guid, for naming our files that we want to have unique name
        // Everytime this method calls a new GUID creates and we can use this name for our file naming convesions.
        var guid = Guid.NewGuid();
        string documentType = DocumentType(blob.ContentType.ToString());

        BlobResponseDto response = new();
        BlobClient client = _filesContainer.GetBlobClient(guid.ToString() + documentType);

        await using (Stream? data = blob.OpenReadStream())
        {
            await client.UploadAsync(data);
        }

        response.Status = guid.ToString() + documentType;
        response.Error = false;
        response.Blob.Uri = client.Uri.AbsoluteUri;
        response.Blob.Name = client.Name;
        response.Blob.ContentType = blob.ContentType;

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
        BlobResponseDto response;
        try
        {
            BlobClient file = _filesContainer.GetBlobClient(blobFileName);
            var result = await file.DeleteAsync();
            response = new BlobResponseDto { Error = false, Status = $"File: {blobFileName} has successfully deleted." };
            return response;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private string DocumentType(string contentType)
    {
        int indexOfSlash = contentType.IndexOf('/');
        string result = "." + contentType.Substring(indexOfSlash + 1);
        return result;
    }

}

