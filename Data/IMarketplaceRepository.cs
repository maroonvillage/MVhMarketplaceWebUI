using webui.Models;

namespace webui.Data
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
