using LibraryCMCM.Models;

namespace LibraryCMCM.DataAccess
{
    public interface IRequestData
    {
        Task DeleteRequest(int requestId);
        Task<List<RequestModel>> GetAllRequests(int userId);
        Task<RequestModel?> GetRequest(int requestId);
        Task PostRequest(int userId, int categoryId, string subject, string requestDocument, string comment);
        Task UpdateRequest(int requestId, int categoryId, string subject, string requestDocument, string comment);
    }
}