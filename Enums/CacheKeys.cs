using System;
using System.ComponentModel;

namespace webui.Enums
{
    public enum CacheKeys
    {
        [Description("UsStates")]
        [DefaultValue(1440)]
        UsStates,
        [Description("Clients_{0}")]
        [DefaultValue(240)]
        Clients,
        [Description("Feed_{0}")]
        [DefaultValue(240)]
        Feed,
        [Description("BusinessHours_{0}")]
        [DefaultValue(240)]
        BusinessHours,
        [Description("Menu_{0}_{1}")]
        [DefaultValue(240)]
        Menu,
        [Description("Offers_{0}")]
        [DefaultValue(240)]
        Offers,
        [Description("Promotions_{0}")]
        [DefaultValue(240)]
        Promotions,
        [Description("Testimonials_{0}")]
        [DefaultValue(240)]
        Testimonials,
        [Description("Carouselmages_{0}")]
        [DefaultValue(60)]
        CarouselImages,
        [Description("EmailTemplates_{0}")]
        [DefaultValue(240)]
        EmailTemplates,
        [Description("NewsLinks_{0}")]
        [DefaultValue(240)]
        NewsLinks,
        [Description("SearchCriteriaByCategory")]
        [DefaultValue(60)]
        SearchCriteriaByCategory,
        [Description("SiteContentDictionary_{0}")]
        [DefaultValue(240)]
        SiteContentDictionary,
        [Description("SiteSettings_{0}")]
        [DefaultValue(240)]
        SiteSettings,
        [Description("Users_{0}")]
        [DefaultValue(240)]
        Users

    }
}
