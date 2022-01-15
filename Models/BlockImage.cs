using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webui.Models
{
    public partial class BlockImage
    {
        public string id { get; set; }
        public int BlockImageId { get; set; }
        public int? BlockId { get; set; }
        public int? CarouselImageId { get; set; }
        public int? ImageId { get; set; }
        public bool IsLogo { get; set; }
        public string FileName { get; set; }
        public string ImageUrl { get; set; }
        public int SequencdNumber { get; set; }
        public int MarketplaceId { get; set; }

    }
}
