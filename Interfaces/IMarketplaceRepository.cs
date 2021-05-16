using webui.Models;

namespace webui.Interfaces
{
    public interface IMarketplaceRepository
    {
        Marketplace GetMarketplaceByDomain(string domain);
        Marketplace GetMarketplaceById(int marketPlacdId);
        MarketplaceSetting GetMarketplaceSettingsById(int marketPlaceId);
        MarketplaceTheme GetThemeByMarketplaceId(int marketPlaceId);
        Template GetTemplateById(int templateId);
        Menu GetMenuByName(int marketPlaceId, string menuName);
    }
}
