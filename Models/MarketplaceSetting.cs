using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webcoreapp.Models
{
    [NotMapped]
    public partial class MarketplaceSetting
    {
        public int SettingsId { get; set; }
        public int MarketplaceId { get; set; }
        public string City { get; set; }
        public Template Tempate { get; set; }
    }
}
