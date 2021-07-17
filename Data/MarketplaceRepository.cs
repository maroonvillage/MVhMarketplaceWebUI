using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using webui.Interfaces;
using webui.Models;

namespace webui.Data
{
    public class MarketplaceRepository : IMarketplaceRepository
    {

        private string _connectionString;
        //private readonly webuiIdentityDbContext _context;

        private readonly IConfiguration _configuration;

        public MarketplaceRepository(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:ContentConnection"];
        }

        public Marketplace GetMarketplaceByDomain(string domain)
        {
            Marketplace  marketPlace = null;
            MarketplaceSetting marketplaceSetting = null;
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

        public Menu GetMenuByName(int marketPlaceId, string menuName)
        {

                Menu menu = null;

            var sqlText = @"SELECT 
                            m.MenuId,
                            m.MenuParentId,
                            m.MenuName,
                            m.MarketplaceId, 
                            mi.MenuItemId,
                            mi.SequenceNumber,
                            mi.ItemName,
                            mi.Controller,
                            mi.[Action],
                            mi.Title,
                            mi.LinkId
                            FROM Menus m
                              INNER JOIN MenuItems mi
                                on m.MenuId = mi.MenuId
                            WHERE m.MarketplaceId = @id AND m.MenuName = @name AND mi.IsActive = 1
                            ORDER BY mi.SequenceNumber;
                            ";
                try
                {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var command = new SqlCommand(sqlText, connection)
                    {
                        CommandType = System.Data.CommandType.Text
                    };
                    //set parameters 
                    command.Parameters.AddWithValue("@id", marketPlaceId);
                    command.Parameters.AddWithValue("@name", menuName);


                        command.Connection.Open();

                        menu = new Menu();
                        menu.MenuItems = new List<MenuItem>();

                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        MenuItem mi = new MenuItem();

                        menu.MenuId = Convert.ToInt32(reader["MenuId"]);
                        menu.MenuParentId = reader["MenuParentId"] != DBNull.Value ? Convert.ToString(reader["MenuParentId"]) : null;
                        mi.MenuItemId = Convert.ToInt32(reader["MenuItemId"]);
                        mi.SequenceNumber = reader["SequenceNumber"] != DBNull.Value ? Convert.ToInt32(reader["SequenceNumber"]) : null;
                        mi.ItemName = Convert.ToString(reader["ItemName"]);
                        mi.Controller = Convert.ToString(reader["Controller"]);
                        mi.Action = Convert.ToString(reader["Action"]);
                        mi.Title = Convert.ToString(reader["Title"]);
                        mi.LinkId = reader["LinkId"] != DBNull.Value ? Convert.ToInt32(reader["LinkId"]) : null;
                        menu.MenuItems.Add(mi);

                    }// end while
                     // end using

                }// end using

                }
                catch (SqlException sqlex)
                {
                    throw new Exception($"GetMenuByName: Caught a SqlException: {sqlex.Message}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"GetMenuByName: Caught a general exception: {ex.StackTrace}");

                }
                finally
                {
                }

                return menu;
            }
    }
}
