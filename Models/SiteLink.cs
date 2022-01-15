using System;
namespace webui.Models
{
    public class SiteLink
    {
        public SiteLink()
        {
        }

        public int LinkId { get; set; }
        public string LinkName { get; set; }
        public string Url { get; set; }
        public string ToolTip { get; set; }
        public string Title { get; set; }
        public string Target { get; set; }
        public int MarketplaceId { get; set; }
        public BlockLink BlockLink { get; set; }
        public SiteImage SiteImage { get; set; }

    }
}
