using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace webcoreapp.Models
{
    [NotMapped]
    public class MarketplaceTheme
    {
        public int MarketplaceTemplateId { get; set; }
        public int TemplateId { get; set; }
        public int MarketplaceId { get; set; }
        public bool? IsMobile { get; set; }


    }
}
