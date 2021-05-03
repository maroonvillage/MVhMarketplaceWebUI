using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using webui.Models;

namespace webui.Data
{
    public class MarketplaceRepository : IMarketplaceRepository
    {

        private string _connectionString;
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;

        public MarketplaceRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public Marketplace GetMarketplaceByDomain(string domain)
        {
            Marketplace  marketPlace = null;
            MarketplaceSetting marketplaceSetting = null;
            MarketplaceTheme marketPlaceTheme = null;
            Template template = null;

            string sql = @"SELECT 
                            m.MarketplaceId,
                            m.Name,
                            m.Description,
                            m.Url,
                            mt.MarketplaceTemplateId,
                            mt.AssignmentId,
                            t.TemplateId,
                            t.TemplateMachineName,
                            t.TemplateName,
                            t.IsMobil,
                            ms.SettingsId,
                            ms.City
                            FROM Marketplaces m 
                              LEFT JOIN MarketplaceTheme mt 
                                on m.MarketplaceId = mt.MarketplaceId
                              LEFT JOIN Templates t
                                on mt.TemplateId = t.TemplateId
                              LEFT JOIN MarketplaceSettings ms
                               on m.MarketplaceId = ms.MarketplaceId
                            WHERE Url = @domain;";

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                command.Parameters.AddWithValue("@domain", domain);
                connection.Open();
               using(var reader = command.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        marketPlace = new Marketplace
                        {
                            MarketplaceId = Convert.ToInt32(reader["MarketPlaceId"]),
                            Name = reader["Name"].ToString(),
                            Description = reader["Description"].ToString(),
                            Url = reader["Url"].ToString()
                        };

                        marketplaceSetting = new MarketplaceSetting
                        {
                            SettingsId = Convert.ToInt32(reader["SettingsId"]),
                            City = reader["City"].ToString(),
                        };

                        //marketPlaceTheme = new MarketplaceTheme
                        //{
                        //};
                        template = new Template
                        {
                            TemplateId = Convert.ToInt32(reader["TemplateId"]),
                            TemplateMachineName = reader["TemplateMachineName"].ToString(),
                            TemplateName = reader["TemplateName"].ToString(),
                            IsMobile = Convert.ToBoolean(reader["IsMobil"])
                        };

                        break;


                    }
                    marketplaceSetting.Tempate = template;
                    marketPlace.Settings = marketplaceSetting;

                }
            }
            return marketPlace ?? new Marketplace();
        }

        public Marketplace GetMarketplaceById(int marketPlacdId)
        {
            Marketplace marketPlace = null;
            string sql = @"SELECT [MarketplaceId]
                                  ,[Name]
                                  ,[Description]
                                  ,[Url]
                              FROM [dbo].[Marketplaces]
                           WHERE MarketplaceId = @marketPlacdId;";

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                command.Parameters.AddWithValue("@marketPlacdId", marketPlacdId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        marketPlace = new Marketplace
                        {
                            MarketplaceId = Convert.ToInt32(reader["MarketPlaceId"]),
                            Name = reader["Name"].ToString(),
                            Description = reader["Description"].ToString(),
                            Url = reader["Url"].ToString()
                        };
                        break;

                    }
                }
            }
            return marketPlace ?? new Marketplace();
        }

        public MarketplaceSetting GetMarketplaceSettingsById(int marketPlaceId)
        {
            return new MarketplaceSetting();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketPlaceId"></param>
        /// <returns></returns>
        public MarketplaceTheme GetThemeByMarketplaceId(int marketPlaceId)
        {

            return new MarketplaceTheme();

        }

        public Template GetTemplateById(int templateId)
        {
           
            return  new Template();
        }
    }
}
