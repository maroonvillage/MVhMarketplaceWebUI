using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace webui.Data
{
    public class NoSqlRepositoryBase
    {

        public readonly IConfiguration _configuration;

        private CosmosClient _cosmosClient;

        // The database we will create
        private Database _database;

        // The container we will create.
        private Container _container;

        private string _containerId;

        private string _databaseId;


        public NoSqlRepositoryBase(IConfiguration configuration)
        {
            _configuration = configuration;
            _databaseId = _configuration["DocumentDb:Database"];
        }

        public Database Database {
            get { return _database; }
            set { _database = value; }
        }

        public CosmosClient Client
        {
            get { return _cosmosClient; }
            set { _cosmosClient = value; }
        }

        public Container Container
        {
            get { return _container; }
            set { _container = value; }
        }

        public string DatabaseId {
            get { return _databaseId;  }
            set { _databaseId = value; }
        }

        public string ContainerId
        {
            get { return _containerId; }
            set { _containerId = value; }
        }

        public string EndpointUrl
        {
            get
            {
                //TODO: add logic that looks for the url of the cosmos db account
                //      which is public by default.  
                string url = _configuration["DocumentDb:EndpointUrl"];
                return url;
            }
           
        }

        public string PrimaryKey
        {
            get
            {
                //TODO: add logic that looks for the key of the cosmos db account
                //      which is public by default.  
                string key = "t7nquHb5lXgzqoxhqTvjQNUGGxZnU1q6qxRJrtPVYalTlRK0KdFC9vU7wgztEvNuRsVogFVBIexahjKgIub40g==";
                return key;
            }
            
        }



      

    }
}
