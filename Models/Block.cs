using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webui.Models
{
    [NotMapped]
    public partial class Block
    {
        public int BlockId { get; set; }
        public string BlockMachineName { get; set; }
        public string BlockTitle { get; set; }
        public int BlockTypeOfContent { get; set; }
        public bool? IsDataFeed { get; set; }
        public BlockContent Content { get; set; }
    }
}
