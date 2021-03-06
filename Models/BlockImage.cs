using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webui.Models
{
    [NotMapped]
    public partial class BlockImage
    {
        public int BlockImageId { get; set; }
        public int? BlockId { get; set; }
        public int? CarouselImageId { get; set; }
        public int? ImageId { get; set; }
        public bool IsLogo { get; set; }

    }
}
