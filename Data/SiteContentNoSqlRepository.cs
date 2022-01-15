using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using webcoreapp.Enumerators;
using webui.Interfaces;
using webui.Models;
using webui.Services;

namespace webui.Data
{
    public class SiteContentNoSqlRepository : NoSqlRepositoryBase, ISiteContentNoSqlRepository
    {


        private readonly IConfiguration _configuration;

        public SiteContentNoSqlRepository(IConfiguration configuration) :base(configuration)
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

        public IEnumerable<SiteContent> GetSiteContentDictionaryByMarketplaceId(string marketPlaceId, string templateMachineName, string pageMachineName)
        {
            var page = QuerySitePageByPageMachineNameAsync(marketPlaceId, pageMachineName, containerId: "SitePages");

            //TODO: build the site content object using the site page
            //
            var sitePage = page.Result;
            foreach(var blk in sitePage.Blocks)
            {
                yield return new SiteContent
                {
                    MarketplaceId = sitePage.MarketPlace.MarketplaceId,
                    PageId = sitePage.PageId,
                    ContentName = blk.Content.ContentName,
                    ContentValue = blk.Content.ContentValue,
                    IsFeed = blk.IsDataFeed,
                    ContentType = EnumExtension.GetValueFromDescription<DynamicContentType>(blk.Content.DynamicContentType),
                    Block =
                            {
                                BlockId = blk.BlockId,
                                BlockMachineName = blk.BlockMachineName
                            },
                    Template =
                            {
                                TemplateId = Convert.ToInt32(sitePage.Template.TemplateId),
                                TemplateMachineName = sitePage.Template.TemplateMachineName,
                                TemplateName = sitePage.Template.TemplateName
                            },
                    SitePage =
                            {
                                PageId = sitePage.PageId,
                                PageMachineName = sitePage.PageMachineName,
                                PageTitle = sitePage.PageTitle
                            }
                };
            }

            //return page.Result;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public async Task<Marketplace> QueryMarketplaceByDomainAsync(string url, string containerId)
        {
            Container = await CreateContainerAsync(containerId);

            var sqlQueryText = "SELECT * FROM c WHERE c.Url = @url";

            //Console.WriteLine("Running query: {0}\n", sqlQueryText);

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            queryDefinition.WithParameter("@url", url);


            FeedIterator<Marketplace> queryResultSetIterator = Container.GetItemQueryIterator<Marketplace>(queryDefinition);
            var marketPlace = new Marketplace();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Marketplace> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Marketplace marketplace in currentResultSet)
                {
                    marketPlace = marketplace;
                    break;
                }
            }

            return marketPlace;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketplaceId"></param>
        /// <param name="sitePageMachineName"></param>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public async Task<SitePage> QuerySitePageByPageMachineNameAsync(string marketplaceId, string pageMachineName, string containerId)
        {

            Container =  await CreateContainerAsync(containerId);

            var sqlQueryText = "SELECT * FROM c WHERE c.MarketPlace.MarketplaceId = @marketplace_id AND c.PageMachineName = @page_machine_name";

            //Console.WriteLine("Running query: {0}\n", sqlQueryText);

            var queryDefinition = new QueryDefinition(sqlQueryText);
            queryDefinition.WithParameter("@marketplace_id", marketplaceId);
            queryDefinition.WithParameter("@page_machine_name", pageMachineName);

            FeedIterator<SitePage> queryResultSetIterator = Container.GetItemQueryIterator<SitePage>(queryDefinition);
            var page = new SitePage();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<SitePage> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (SitePage sitePage in currentResultSet)
                {
                    page = sitePage;
                    break;
                }
            }

            return page;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public async Task<List<SiteContent>> QuerySiteContentByBlockIdsAsync(string pageId, string containerId)
        {


            Container = await CreateContainerAsync(containerId);


            var sqlQueryText = @"SELECT * FROM c WHERE c.SitePageId IN = (@page_id)";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            queryDefinition.WithParameter("@page_id", pageId);


            FeedIterator<SiteContent> queryResultSetIterator = Container.GetItemQueryIterator<SiteContent>(queryDefinition);

            List<SiteContent> contents = new List<SiteContent>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<SiteContent> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (SiteContent sc in currentResultSet)
                {
                    contents.Add(sc);
                }
            }

            return contents;
        }

    }
}