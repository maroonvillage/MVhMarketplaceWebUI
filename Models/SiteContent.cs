using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace webcoreapp.Models
{
    [NotMapped]
    public partial class SiteContent
    {
        public int MarketplaceId { get; set; }
        public int PageId { get; set; }
        public string ContentName { get; set; }
        public string ContentValue { get; set; }
        public bool? IsFeed { get; set; }
        public int? DynamicContentType { get; set; }
        public Block Block { get; set; }
        public SitePage SitePage { get; set; }
        public Template Template { get; set; }

        public SiteContent()
        {
            Block = new Block();
            SitePage = new SitePage();
            Template = new Template();
        }

    }

}
