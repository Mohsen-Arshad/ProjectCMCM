using LibraryCMCM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryCMCM.DataAccess;

public class DocumentData
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
    public async Task<DocumentModel> PostDocument(int requestId, int userId, string fileName, string filePathUrl)
    {
        var result = await _sql.LoadData<DocumentModel, dynamic>(
            "dbo.spPostDocument",
            new { RequestId = requestId,
                  UserId = userId,
                  FileName = fileName,
                  FilePathUrl = filePathUrl},
            "Default");
        return result.FirstOrDefault();
    }

    // dbo.spUpdateDocument
    public Task UpdateDocument(int documentId, int requestId, int userId, string fileName, string filePathUrl)
    {
        return _sql.SaveData(
            "dbo.spUpdateDocument",
            new
            {
                Id = documentId,
                RequestId = requestId,
                UserId = userId,
                FileName = fileName,
                FilePathUrl = filePathUrl
            },
            "Default");
    }
}
