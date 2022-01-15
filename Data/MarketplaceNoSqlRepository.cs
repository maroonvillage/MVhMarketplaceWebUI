using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using webui.Interfaces;
using webui.Models;

namespace webui.Data
{
    public class MarketplaceNoSqlRepository  : NoSqlRepositoryBase, IMarketplaceNoSqlRepository
    {
        public MarketplaceNoSqlRepository(IConfiguration configuration) : base(configuration)
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
            //Console.WriteLine("Created Database: {0}\n", this.database.Id);
        }

        /// Create the container if it does not exist. 
        /// Specifiy "/LastName" as the partition key since we're storing family information, to ensure good distribution of requests and storage.
        private async Task<Container> CreateContainerAsync(string containerId)
        {
            // Create a new container
            await CreateDatabaseAsync();
            return Database.GetContainer(containerId);

        }

        public async Task<Marketplace> GetMarketplaceByDomain(string url)
        {
            //CreateContainerAsync()
            var mrkts = await QueryMarketplaceByDomainAsync(url, "Marketplaces");
            

            return new Marketplace
            {
                MarketplaceId = mrkts.MarketplaceId,
                Name = mrkts.Name,
                HeaderLogo = mrkts.HeaderLogo,
                Description = mrkts.Description,
                Url = mrkts.Url,
                Settings = new MarketplaceSetting
                {
                    City = "TODO: Add property",
                    Template = mrkts.Template
                }
            };

        }

        public async Task<Marketplace> GetMarketplaceById(string url)
        {
            var mrkts = await QueryMarketplaceByIdAsync(url, "Marketplaces");


            return new Marketplace
            {
                MarketplaceId = mrkts.MarketplaceId,
                Name = mrkts.Name,
                HeaderLogo = mrkts.HeaderLogo,
                Description = mrkts.Description,
                Url = mrkts.Url,
                Settings = new MarketplaceSetting
                {
                    City = "TODO: Add property",
                    Template = mrkts.Template
                }
            };

        }

        public async Task<Menu> GetMenuByNameAsync(string marketplaceId, string menuName)
        {

            var _menu = await QueryMenuByNameAsync(marketplaceId, menuName, "Menus");


            var menu = new Menu
            {
               MenuId = _menu.MenuId,
               MenuName = _menu.MenuName,
               MenuParentId = _menu.MenuParentId,

            };

            menu.MenuItems = new List<MenuItem>();
            foreach(var mi in _menu.MenuItems)
            {
                menu.MenuItems.Add(mi);
            }

            return menu;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public async Task<Marketplace> QueryMarketplaceByIdAsync(string marketplaceId, string containerId)
        {
            Container = await CreateContainerAsync(containerId);

            var sqlQueryText = "SELECT * FROM c WHERE c.MarketplaceId = @market_place";

            //Console.WriteLine("Running query: {0}\n", sqlQueryText);

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            queryDefinition.WithParameter("@market_place", marketplaceId);


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


        public async Task<Menu> QueryMenuByNameAsync(string marketplaceId, string menuName, string containerId)
        {
            Container = await CreateContainerAsync(containerId);

            var sqlQueryText = "SELECT * FROM c WHERE c.MarketPlace.MarketplaceId = @market_place_id AND c.MenuName = @menu_name";

            //Console.WriteLine("Running query: {0}\n", sqlQueryText);

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            queryDefinition.WithParameter("@market_place_id", marketplaceId);
            queryDefinition.WithParameter("@menu_name", menuName);



            FeedIterator<Menu> queryResultSetIterator = Container.GetItemQueryIterator<Menu>(queryDefinition);
            var menu = new Menu();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Menu> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Menu _menu in currentResultSet)
                {
                    menu = _menu;
                    break;
                }
            }

            return menu;
        }


    }
}
