using System;
using System.ServiceModel.Syndication;
using webui.Models;

namespace webui.Interfaces
{
    public interface IFeedService
    {
        SyndicationFeed GetFeed(SiteContent content);

        SyndicationFeed GetFeed(string uri);
    }
}
