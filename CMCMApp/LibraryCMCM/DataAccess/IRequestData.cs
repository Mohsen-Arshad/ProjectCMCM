using LibraryCMCM.Models;

namespace LibraryCMCM.DataAccess
{
    public interface IRequestData
    {
        Task DeleteRequest(int requestId);
        Task<List<RequestModel>> GetAllRequests(int userId);
        Task<RequestModel?> GetRequest(int requestId);
        Task<RequestModel?> PostRequest(int userId, int categoryId, int documentId, string subject, string comment);
        Task UpdateRequest(int requestId, int categoryId, int documentId, string subject, string comment);
    }
}