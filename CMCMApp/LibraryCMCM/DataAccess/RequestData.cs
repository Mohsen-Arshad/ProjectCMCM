
using LibraryCMCM.Models;

namespace LibraryCMCM.DataAccess;

public class RequestData : IRequestData
{
    private readonly ISqlDataAccess _sql;

    public RequestData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    // dbo.spGetAllRequests
    public async Task<List<RequestModel>> GetAllRequests(int userId)
    {
        var result = await _sql.LoadData<RequestModel, dynamic>("dbo.spGetAllRequests", new { UserId = userId }, "Default");
        return result;
    }

    // dbo.spGetRequest
    public async Task<RequestModel?> GetRequest(int requestId)
    {
        var result = await _sql.LoadData<RequestModel, dynamic>("dbo.spGetRequest", new { Id = requestId }, "Default");
        return result.FirstOrDefault();
    }

    // dbo.spDeleteRequest
    public Task DeleteRequest(int requestId)
    {
        return _sql.SaveData("dbo.spDeleteRequest", new { Id = requestId }, "Default");
    }

    // dbo.spPostRequest
    public async Task<RequestModel?> PostRequest(
        int userId,
        int categoryId,
        int documentId,
        string subject,
        string requestDocument,
        string comment)
    {
        var result = await _sql.LoadData<RequestModel, dynamic>(
            "dbo.spPostRequest",
            new
            {
                UserId = userId,
                CategoryId = categoryId,
                DocumentId = documentId,
                Subject = subject,
                RequestDocument = requestDocument,
                Comment = comment
            },
            "Default");

        return result.FirstOrDefault();
    }

    // dbo.spUpdateRequest
    public Task UpdateRequest(
        int requestId,
        int categoryId,
        int documentId,
        string subject,
        string requestDocument,
        string comment)
    {
        return _sql.SaveData(
            "dbo.spPostRequest",
            new
            {
                Id = requestId,
                CategoryId = categoryId,
                DocumentId = documentId,
                Subject = subject,
                RequestDocument = requestDocument,
                Comment = comment
            },
            "Default");
    }
}
