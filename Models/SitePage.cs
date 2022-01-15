using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webui.Models
{
    [NotMapped]
    public partial class SitePage
    {
        public int PageId { get; set; }
        public string PageMachineName { get; set; }
        public string PageTitle { get; set; }
        public Marketplace MarketPlace { get; set; }
        public Template Template { get; set; }
        public List<Block> Blocks { get; set; }
        public string SitePageId { get; set; }

        public List<string> GetBlockNames()
        {
            var ids = new List<string>();

            foreach (var block in Blocks)
            {

                ids.Add(block.BlockMachineName);

            }
            return ids;
        }
    }
}
