using System;
using System.ServiceModel.Syndication;
using System.Xml;
using ElmahCore;
using webui.Enums;
using webui.Interfaces;
using webui.Models;

namespace webui.Services
{
    public class FeedService : IFeedService
    {
        private readonly ICacheService _cacheService;

        public FeedService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public SyndicationFeed GetFeed(SiteContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            if ((bool)!content.IsFeed)
            {
                throw new Exception(string.Format("Attempted to load a non feed SiteContent item as a feed. [{0},{1}]", content.Block.BlockMachineName, content.ContentName));
            }

            if (string.IsNullOrWhiteSpace(content.ContentValue))
            {
                throw new Exception(string.Format("SiteContent is missing ContentValue for feed. [{0},{1}]", content.Block.BlockMachineName, content.ContentName));
            }

            try
            {
                return GetCachedFeed(content.ContentValue);
            }
            catch (Exception ex)
            {
                //ElmahCore.ErrorSignal.FromCurrentContext().Raise(new Exception(string.Format("Error downloading feed: {0}, BlockId: {1}, DealerId: {2}", content.ContentValue, content.Block.Id, content.DealerId), ex));
                return CacheFeed(content.ContentValue, new SyndicationFeed()); // cache it to stop errors for a while
            }
        }

        public SyndicationFeed GetFeed(string uri)
        {
            try
            {
                return GetCachedFeed(uri);
            }
            catch (Exception ex)
            {
                //Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception("Error downloading feed: " + uri, ex));
                return CacheFeed(uri, new SyndicationFeed()); // cache it to stop errors for a while
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private SyndicationFeed GetCachedFeed(string uri)
        {
            if (_cacheService.Contains(uri))
            {
                return (SyndicationFeed)(_cacheService.Get(uri)  ?? new SyndicationFeed());
            }

            using (var reader = XmlReader.Create(uri))
            {
                return CacheFeed(uri, SyndicationFeed.Load(reader));
            }
        }

        private SyndicationFeed CacheFeed(string uri, SyndicationFeed syndicationFeed)
        {
            var cacheKey = string.Format(HelpersService.GetEnumDescription(CacheKeys.Feed), uri);

            _cacheService.Add(cacheKey, syndicationFeed);
            return syndicationFeed;
        }
    }
}
