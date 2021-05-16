using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webui.Models
{
    [NotMapped]
    public partial class Menu
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuParentId { get; set; }
        public int? SiteId { get; set; }

        public IList<MenuItem> MenuItems { get; set; }

    }
}
