using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryCMCM.Models;

public class UserModel
{
    public int Id { get; set; }

    [MinLength(3)]
    [MaxLength(30)]
    public string FristName { get; set; }

    [MinLength(3)]
    [MaxLength(30)]
    public string LastName { get; set; }

    public int IdentifitcationNumber { get; set; }

    [EmailAddress]
    public string EmailAddress { get; set; }

    [PasswordPropertyText]
    public string Password { get; set; }

    public ICollection<RequestModel> UserRequests { get; set; }
}
