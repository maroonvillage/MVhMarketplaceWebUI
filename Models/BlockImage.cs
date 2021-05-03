using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webcoreapp.Models
{
    [NotMapped]
    public partial class BlockImage
    {
        public int BlockImageId { get; set; }
        public int BlockId { get; set; }
        public int? CarouselImageId { get; set; }
    }
}
