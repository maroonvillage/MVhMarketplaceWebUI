using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using webui.Models;
using webui.Interfaces;
using webcoreapp.Enumerators;

namespace webui.Data
{
    public class SiteContentRepository : ISiteContentRepository
    {
        private string _connectionString;
        private readonly IConfiguration _configuration;


        public SiteContentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public IEnumerable<SiteContent> GetSiteContentDictionaryByMarketplaceId(int marketPlaceId, string templateMachineName, string pageMachineName)
        {
            IDictionary<string, SiteContent> siteContentDictionary = null;

            Marketplace marketPlace = null;
            MarketplaceSetting marketplaceSetting = null;
            Template template = null;
            var sql = @"SELECT 
                        S.MarketplaceId, 
                        TB.PageId, 
                        S.BlockId,
                        b.BlockMachineName,
                        S.ContentName, 
                        S.ContentValue,
                        b.IsDataFeed,
                        s.IsFeed,
                        s.DynamicContentType,
						T.TemplateId,
						T.TemplateMachineName,
						T.TemplateName,
						p.PageMachineName,
						p.PageTitle
                        FROM [dbo].[SiteContents] S, 
                        [dbo].[SitePages] p,
                        [dbo].[Templates] T,
                        [dbo].[TemplateBlocks] TB
                        INNER JOIN [dbo].[Blocks] b WITH (NOLOCK)
                            ON TB.BlockId = b.BlockId
                        WHERE S.MarketplaceId = @marketplace_id AND S.BlockId = TB.BlockId
                        AND p.PageMachineName = @page_machine_name AND T.TemplateMachineName = @template_machine_name;";

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                command.Parameters.AddWithValue("@marketplace_id", marketPlaceId);
                command.Parameters.AddWithValue("@page_machine_name", pageMachineName);
                command.Parameters.AddWithValue("@template_machine_name", templateMachineName);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        yield return new SiteContent
                        {
                            MarketplaceId = Convert.ToInt32(reader["MarketplaceId"]),
                            PageId = Convert.ToInt32(reader["PageId"]),
                            ContentName = reader["ContentName"].ToString(),
                            ContentValue = reader["ContentValue"].ToString(),
                            IsFeed = reader["IsFeed"] != null && Convert.ToBoolean(reader["IsFeed"]),
                            ContentType = (DynamicContentType) Convert.ToInt32(reader["DynamicContentType"]),
                            Block =
                            {
                                BlockId = Convert.ToInt32(reader["BlockId"]),
                                BlockMachineName = reader["BlockMachineName"].ToString()
                            },
                            Template =
                            {
                                TemplateId = Convert.ToInt32(reader["TemplateId"]),
                                TemplateMachineName = reader["TemplateMachineName"].ToString(),
                                TemplateName = reader["TemplateName"].ToString()
                            },
                            SitePage =
                            {
                                PageId = Convert.ToInt32(reader["PageId"]),
                                PageMachineName = reader["PageMachineName"].ToString(),
                                PageTitle = reader["PageTitle"].ToString(),
                            }
                        };

                    }
                }
            }

           // return null;
        }
        public IDictionary<string, SiteContent> GetSiteContentDictionary(Marketplace marketPlace,string pageMachineName)
        {
            IDictionary<string, SiteContent> siteContentDictionary = null;
            try
            {
                var templateMachineName = marketPlace.Settings.Tempate.TemplateMachineName;
                return GetSiteContentDictionaryByMarketplaceId(marketPlace.MarketplaceId, templateMachineName, pageMachineName).ToDictionary(x => x.Block.BlockMachineName);
            }
            catch(Exception e)
            {


            }

            return siteContentDictionary;

        }

    }
}
