using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace LibraryCMCM.Models;

public class RequestModel
{
    enum RequestStatusCodes
    {
        Submitted,
        Processing,
        Pending,
        Successfull
    }

    public int Id { get; set; }
    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public int DocumentId { get; set; }

    [MinLength(3)]
    [MaxLength(50)]
    //[Required(ErrorMessage = "Please enter your name")]
    public string Subject { get; set; }

    //[Required(ErrorMessage = "Please enter your name")]
    public string RequestDocument { get; set; }

    [MinLength(0)]
    [MaxLength(200)]
    //[Required(ErrorMessage = "Please enter your name")]
    public string RequestComment { get; set; }


    public int StatusCode { get; set; }
    public bool IsComplete { get; set; }
    public bool IsArchived { get; set; }
}
