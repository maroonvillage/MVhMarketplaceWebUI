﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace webui.Models
{
    [NotMapped]
    public partial class SiteSettings
    {
        public int SiteSettingsId { get; set; }
        public int MarketplaceId { get; set; }

        public string ContentName { get; set; }
        public string ContentValue { get; set; }

       

    }
}
