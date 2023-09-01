using LibraryCMCM.Models;

namespace LibraryCMCM.DataAccess
{
    public interface IDocumentData
    {
        Task<List<DocumentModel>> GetAllDocuments(int userId);
        Task<DocumentModel> GetDocument(int documentId);
        Task<DocumentModel> PostDocument(int userId, string fileName, string filePathUrl);
        Task<DocumentModel> UpdateDocument(int documentId, int userId, string fileName, string filePathUrl);
    }
}