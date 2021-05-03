using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace webcoreapp.Models
{
    [NotMapped]
    public partial class SiteSetting
    {
        public int SiteSettingsId { get; set; }
        public int MarketplaceId { get; set; }
        public string ContentName { get; set; }
        public string ContentValue { get; set; }
    }
}
