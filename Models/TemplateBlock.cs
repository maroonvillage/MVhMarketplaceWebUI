using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace webcoreapp.Models
{
    [NotMapped]
    public partial class TemplateBlock
    {
        public int TemplateBlockId { get; set; }
        public int TemplateId { get; set; }
        public int BlockId { get; set; }
        public int PageId { get; set; }
    }
}
