using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using webui.Interfaces;
using webui.Models;

namespace webui.Data
{
    public class SiteSettingsNoSqlRepository : NoSqlRepositoryBase, ISiteSettingsNoSqlRepository
    {
        private readonly IConfiguration _configuration;

        public SiteSettingsNoSqlRepository(IConfiguration configuration) :base(configuration)
        {
            CreateClient();
        }
        private void CreateClient()
        {
            // Create a new instance of the Cosmos Client
            if (string.IsNullOrEmpty(EndpointUrl) || string.IsNullOrEmpty(PrimaryKey))
                throw new ArgumentNullException("DocumentDB endpoint url or primary key is missing.");

            Client = new CosmosClient(EndpointUrl, PrimaryKey);

        }

        private async Task CreateDatabaseAsync()
        {
            // Create a new database
            Database = await Client.CreateDatabaseIfNotExistsAsync(DatabaseId);
        }

        /// Create the container if it does not exist. 
        /// Specifiy "/LastName" as the partition key since we're storing family information, to ensure good distribution of requests and storage.
        private async Task<Container> CreateContainerAsync(string containerId)
        {
            // Create a new container
            await CreateDatabaseAsync();

            return Database.GetContainer(containerId);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketPlaceId"></param>
        /// <returns></returns>
        public async Task<IDictionary<string, SiteSettings>> GetSiteSettingsByMarketplaceId(string marketPlaceId)
        {
            var settings = await QuerySiteSettingsByMarketplaceId(marketPlaceId, "SiteSettings");

            var siteSettings = new Dictionary<string, SiteSettings>();
            foreach (var s in settings)
            {
                var key = s.ContentName;
                siteSettings.Add(key,new SiteSettings
                {
                   SiteSettingsId = s.SiteSettingsId,
                   MarketplaceId = s.MarketplaceId,
                   ContentName = s.ContentName,
                   ContentValue = s.ContentValue

                });
            }

            return siteSettings;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketPlaceId"></param>
        /// <returns></returns>
        public async Task<IList<BlockImage>> GetSiteImagesByMarketplaceId(string marketPlaceId)
        {
            //var images = await QueryImagesByMarketplaceId(marketPlaceId, "SiteImages");
            var images = await QueryBlockImagesByMarketplaceId(marketPlaceId, "BlockImages");
            var siteImages = new List<BlockImage>();

            foreach (var img in images)
            {
                siteImages.Add(new BlockImage
                {
                   BlockId = img.BlockId,
                   CarouselImageId = img.CarouselImageId,
                   ImageId = img.ImageId,
                   IsLogo = img.IsLogo,
                   FileName = img.FileName,
                   ImageUrl = img.ImageUrl,
                   SequencdNumber = img.SequencdNumber,
                   MarketplaceId = img.MarketplaceId

                });
            }

            return siteImages;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketPlaceId"></param>
        /// <returns></returns>
        public async Task<IList<BlockLink>> GetSiteLinksByMarketplaceId(string marketPlaceId)
        {
            var links = await QueryBlockLinksByMarketplaceId(marketPlaceId, "BlockLinks");

            var siteLinks = new List<BlockLink>();
            foreach (var lnk in links)
            {
                siteLinks.Add(new BlockLink
                {
                  BlockId = lnk.BlockId,
                  BlockLinkId = lnk.BlockLinkId,
                  LinkName = lnk.LinkName,
                  Url = lnk.Url,
                  Target = lnk.Target,
                  Title = lnk.Title,
                  ToolTip = lnk.ToolTip,
                  MarketplaceId = lnk.MarketplaceId

                });
            }

            return siteLinks;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketPlaceId"></param>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public async Task<IList<SiteImage>> QueryImagesByMarketplaceId(string marketPlaceId, string containerId)
        {
            if (string.IsNullOrEmpty(marketPlaceId)) throw new ArgumentNullException("marketplaceId was null");

            Container = await CreateContainerAsync(containerId);

            var sqlQueryText = "SELECT * FROM c WHERE c.MarketplaceId = @marketplace_id";

            var id = Convert.ToInt32(marketPlaceId);

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            queryDefinition.WithParameter("@marketplace_id", id);


            FeedIterator<SiteImage> queryResultSetIterator = Container.GetItemQueryIterator<SiteImage>(queryDefinition);

            List<SiteImage> siteImages = new List<SiteImage>();


            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<SiteImage> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (SiteImage si in currentResultSet)
                {
                    siteImages.Add(si);
                }
            }

            return siteImages;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketPlaceId"></param>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public async Task<IList<SiteLink>> QueryLinksByMarketplaceId(string marketPlaceId, string containerId)
        {
            Container = await CreateContainerAsync(containerId);

            var sqlQueryText = "SELECT * FROM c WHERE c.MarketplaceId = @marketplace_id";

            //Console.WriteLine("Running query: {0}\n", sqlQueryText);

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            queryDefinition.WithParameter("@marketplace_id", marketPlaceId);



            FeedIterator<SiteLink> queryResultSetIterator = Container.GetItemQueryIterator<SiteLink>(queryDefinition);
            List<SiteLink> siteLinks = new List<SiteLink>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<SiteLink> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (SiteLink sl in currentResultSet)
                {
                    siteLinks.Add(sl);
                }
            }

            return siteLinks;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketPlaceId"></param>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public async Task<IList<SiteSettings>> QuerySiteSettingsByMarketplaceId(string marketPlaceId, string containerId)
        {
            Container = await CreateContainerAsync(containerId);

            var sqlQueryText = "SELECT * FROM c WHERE c.MarketplaceId = @marketplace_id";


            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            queryDefinition.WithParameter("@marketplace_id", marketPlaceId);



            FeedIterator<SiteSettings> queryResultSetIterator = Container.GetItemQueryIterator<SiteSettings>(queryDefinition);
            List<SiteSettings> siteSettings = new List<SiteSettings>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<SiteSettings> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (SiteSettings ss in currentResultSet)
                {
                    siteSettings.Add(ss);
                }
            }

            return siteSettings;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketPlaceId"></param>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public async Task<IList<BlockImage>> QueryBlockImagesByMarketplaceId(string marketPlaceId, string containerId)
        {
            Container = await CreateContainerAsync(containerId);

            var sqlQueryText = "SELECT * FROM c WHERE c.MarketplaceId = @marketplace_id";
            int id = Convert.ToInt32(marketPlaceId);

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            queryDefinition.WithParameter("@marketplace_id", id);


            FeedIterator<BlockImage> queryResultSetIterator = Container.GetItemQueryIterator<BlockImage>(queryDefinition);
           var blockImages = new List<BlockImage>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<BlockImage> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (BlockImage bl in currentResultSet)
                {
                    blockImages.Add(bl);
                }
            }

            return blockImages;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketPlaceId"></param>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public async Task<IList<BlockLink>> QueryBlockLinksByMarketplaceId(string marketPlaceId, string containerId)
        {
            Container = await CreateContainerAsync(containerId);

            var sqlQueryText = "SELECT * FROM c WHERE c.MarketplaceId = @marketplace_id";


            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            queryDefinition.WithParameter("@marketplace_id", marketPlaceId);


            FeedIterator<BlockLink> queryResultSetIterator = Container.GetItemQueryIterator<BlockLink>(queryDefinition);
            var blockLinks = new List<BlockLink>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<BlockLink> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (BlockLink bl in currentResultSet)
                {
                    blockLinks.Add(bl);
                }
            }

            return blockLinks;
        }

    }
}
