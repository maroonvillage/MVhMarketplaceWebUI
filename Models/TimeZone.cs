using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace webcoreapp.Models
{
    [NotMapped]
    public partial class TimeZone
    {
        public int TimeZoneId { get; set; }
        public string TimeZoneName { get; set; }
        public string ShortName { get; set; }
        public string Utcoffset { get; set; }
        public string Description { get; set; }
    }
}
