using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace webui.Models
{
    [NotMapped]
    public partial class SiteImage
    {
        public int SiteImageId { get; set; }
        public string ImageUrl { get; set; }
        public string FileName { get; set; }
        public int? SequenceNumber { get; set; }
        public int? WebsiteId { get; set; }
        public BlockImage BlockImage { get; set; }
        public SiteLink SiteLink { get; set; }

    }
}
