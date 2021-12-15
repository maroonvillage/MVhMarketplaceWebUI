using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace webui.Interfaces
{
    public interface ICosmosDbWebContent
    {
        Task GetSitePagesByMarketplaceIdAsync(string marketId);
        Task etSitePagesByPageMachineNameAsync(string marketName);
        Task GetMenusByMarketplaceIdAsync(string marketId);
        Task GetMenusByNameAsync(string menuName);
        Task GetBlockImagesByBlockIdAsync(string blockId);
        Task GetBlockLinksByBlockIdAsync(string blockId);
    }
}
