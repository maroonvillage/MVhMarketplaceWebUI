using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webui.Models
{
    [NotMapped]
    public partial class SitePage
    {
        public int PageId { get; set; }
        public string PageMachineName { get; set; }
        public string PageTitle { get; set; }
    }
}
