using System.ComponentModel.DataAnnotations;

namespace LibraryCMCM.DataAccess;

public class RequestResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public int DocumentId { get; set; }
    public string FileName { get; set; }
    public string Subject { get; set; }
    public string RequestComment { get; set; }
    public int StatusCode { get; set; }
    public bool IsComplete { get; set; }
    public bool IsArchived { get; set; }
    public string DocumentUrl { get; set; }
}
