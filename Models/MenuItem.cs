using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace webcoreapp.Models
{
    [NotMapped]
    public partial class MenuItem
    {
        public int MenuItemId { get; set; }
        public int MenuId { get; set; }
        public int? SequenceNumber { get; set; }
        public string ItemName { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Title { get; set; }
        public int? LinkId { get; set; }
        public bool? IsActive { get; set; }
    }
}
