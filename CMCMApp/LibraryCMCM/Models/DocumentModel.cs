using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryCMCM.Models;

public class DocumentModel
{
    public int id { get; set; }
    public int RequestId { get; set; }
    public int UserId { get; set; }
    public string FileName { get; set; }
    public string FilePathUrl { get; set; }
}
