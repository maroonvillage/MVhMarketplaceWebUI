using System;
using System.ServiceModel.Syndication;

namespace webui.Models
{
    public class FeedModel : DefaultModel
    {
        public SyndicationFeed Feed { get; set; }
    }
}
