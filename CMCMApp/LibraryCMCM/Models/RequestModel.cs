using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryCMCM.CustomValidationAttribute;

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


    //[Required(ErrorMessage = "Please enter your name")]
    [MinLength(3)]
    [MaxLength(50)]
    public string Subject { get; set; }

    //[Required(ErrorMessage = "Please enter your name")]
    [MinLength(0)]
    [MaxLength(200)]
    public string RequestComment { get; set; }


    public int StatusCode { get; set; }
    public bool IsComplete { get; set; }
    public bool IsArchived { get; set; }

    [NotMapped]
    [AllowedExtensions(new string[] { ".pdf", ".jpg", ".jpeg", ".png" })]
    public IFormFile DocFile { get; set; }

    public DocumentModel? DocModel { get; set; }
}
