using LibraryCMCM.Models;

namespace LibraryCMCM.DataAccess;

public class BlobResponseDto
{
    public BlobDto Blob { get; set; }
    public string? Status { get; set; }
    public bool Error { get; set; }

    public BlobResponseDto()
    {
        Blob = new BlobDto();
    }
}
