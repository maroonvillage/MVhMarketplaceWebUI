using System;
namespace webui.Models
{
    public class BlockLink
    {
        public BlockLink()
        {
        }

        public string id { get; set; }
        public int BlockLinkId { get; set; }
        public int BlockId { get; set; }
        public int LinkId { get; set; }
        public string LinkName { get; set; }
        public string Url { get; set; }
        public string Target { get; set; }
        public string Title { get; set; }
        public string ToolTip { get; set; }
        public int MarketplaceId { get; set; }

    }
}
