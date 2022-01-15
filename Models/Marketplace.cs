using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace webui.Models
{
    [NotMapped]
    public partial class Marketplace
    {
        public string MarketplaceId { get; set; }
        public string Name { get; set; }
        public string HeaderLogo { get; set; }

        public string Description { get; set; }
        public string Url { get; set; }
        public MarketplaceSetting Settings { get; set; }
        public Template Template { get; set; }

    }
}
