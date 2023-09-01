using LibraryCMCM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryCMCM.DataAccess;

public class DocumentData : IDocumentData
{
    private readonly ISqlDataAccess _sql;

    public DocumentData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    // dbo.spGetAllDocuments
    public async Task<List<DocumentModel>> GetAllDocuments(int userId)
    {
        var result = await _sql.LoadData<DocumentModel, dynamic>(
            "dbo.spGetAllDocuments",
            new { UserId = userId },
            "Default");

        return result.ToList();
    }

    // dbo.spGetDocument
    public async Task<DocumentModel> GetDocument(int documentId)
    {
        var result = await _sql.LoadData<DocumentModel, dynamic>(
            "dbo.spGetDocument",
            new { Id = documentId },
            "Default");

        return result.FirstOrDefault();
    }

    // dbo.spPostDocument
    public async Task<DocumentModel> PostDocument(int userId, string fileName, string filePathUrl)
    {
        var result = await _sql.LoadData<DocumentModel, dynamic>(
            "dbo.spPostDocument",
            new
            {
                UserId = userId,
                FileName = fileName,
                FilePathUrl = filePathUrl
            },
            "Default");
        return result.FirstOrDefault();
    }

    // dbo.spUpdateDocument
    public async Task<DocumentModel> UpdateDocument(int documentId, int userId, string fileName, string filePathUrl)
    {
        var result = await _sql.LoadData<DocumentModel,dynamic>(
            "dbo.spUpdateDocument",
            new
            {
                Id = documentId,
                UserId = userId,
                FileName = fileName,
                FilePathUrl = filePathUrl
            },
            "Default");

        return result.FirstOrDefault();
    }
}
