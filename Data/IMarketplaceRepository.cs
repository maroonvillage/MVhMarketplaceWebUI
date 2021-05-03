using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webcoreapp.Models;

namespace webcoreapp.Data
{
    public interface IMarketplaceRepository
    {
        Marketplace GetMarketplaceByDomain(string domain);
        Marketplace GetMarketplaceById(int marketPlacdId);
        MarketplaceSetting GetMarketplaceSettingsById(int marketPlaceId);
        MarketplaceTheme GetThemeByMarketplaceId(int marketPlaceId);
        Template GetTemplateById(int templateId);
    }
}
