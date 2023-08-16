using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryCMCM.Models;

public class UserModel
{
    public int Id { get; set; }

    [MinLength(3)]
    [MaxLength(30)]
    //[Required(ErrorMessage ="Please enter your name")]
    public string FristName { get; set; }

    [MinLength(3)]
    [MaxLength(30)]
    //[Required(ErrorMessage = "Please enter your last name")]
    public string LastName { get; set; }

    //[Required(ErrorMessage = "Please enter your Identification Number")]
    public int IdentifitcationNumber { get; set; }

    [EmailAddress]
    //[Required(ErrorMessage = "Please enter your EmailAddress")]
    public string EmailAddress { get; set; }

    [PasswordPropertyText]
    //[Required(ErrorMessage = "Password cannot be null or empty")]
    public string Password { get; set; }

    public ICollection<RequestModel> UserRequests { get; set; }
}
