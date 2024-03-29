﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webui.Models
{
    [NotMapped]
    public partial class MarketplaceSetting
    {
        public int SettingsId { get; set; }
        public int MarketplaceId { get; set; }
        public string City { get; set; }
        public Template Template { get; set; }
    }
}
