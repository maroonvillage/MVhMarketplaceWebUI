using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using webui.Interfaces;
using webui.Models;

namespace webui.Data
{
    public class SiteSettingsRepository : ISiteSettingsRepository
    {

        private string _connectionString;
        private readonly IConfiguration _configuration;
        public SiteSettingsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ContentConnection");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketPlaceId"></param>
        /// <returns></returns>
        public IDictionary<string, SiteSettings> GetSiteSettingsByMarketplaceId(int marketPlaceId)
        {
            var siteSettingsDictionary = new Dictionary<string, SiteSettings>();
            string sql = @" SELECT 
                           MarketplaceId ,
                           ContentName ,
                           ContentValue 
                         FROM (
                                SELECT 
                                 ss.MarketplaceId,
                                 ss.ContentName,
                                 ss.ContentValue
						         from SiteSettings ss					
						         UNION
						         SELECT m.MarketplaceId,
						                'MarketplaceName' as 'ContentName',
						                m.Name as 'ContentValue'
						         FROM Marketplaces m 
                                 UNION                         
                                 SELECT ms.MarketplaceId,
						                'MarketplaceCity' as 'ContentName',
						                ms.City as 'ContentValue'
						         FROM MarketplaceSettings ms 
		                    ) s 
                           WHERE s.MarketplaceId = @marketPlacdId;";

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                command.Parameters.AddWithValue("@marketPlacdId", marketPlaceId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        var key = reader["ContentName"].ToString();
                       var siteSetting = new SiteSettings
                        {
                            MarketplaceId = Convert.ToInt32(reader["MarketPlaceId"]),
                            ContentName = key,
                            ContentValue = reader["ContentValue"].ToString()
                        };

                        siteSettingsDictionary.Add(key, siteSetting);

                    }
                }
            }
            return siteSettingsDictionary ?? new Dictionary<string, SiteSettings>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketPlaceId"></param>
        /// <returns></returns>
        public IList<SiteImage> GetSiteImagesByMarketplaceId(int marketPlaceId)
        {
            var siteImages = new List<SiteImage>();
            string sql = @" SELECT 
                             bi.BlockImageId,
                             bi.BlockId,
                             bi.ImageId,
                             bi.IsLogo,
                             si.FileName,
                             si.ImageUrl,
                             si.SequenceNumber,
                             si.WebsiteId,
                             sli.LinkId,
                             sl.LinkName,
                             sl.Url,
                             sl.ToolTip,
                             sl.Title,
                             sl.Target 
                            FROM BlockImages bi
                              INNER JOIN SiteImages si
                               ON bi.ImageId = si.SiteImageId
                              LEFT JOIN SiteLinksImages sli 
                               ON si.SiteImageId = sli.ImageId 
                               LEFT JOIN SiteLinks sl 
                                ON sli.LinkId  = sl.LinkId
                            WHERE si.Websiteid = @marketPlaceId;";

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };

                command.Parameters.AddWithValue("@marketPlaceId", marketPlaceId);

               

                    connection.Open();
                    using var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var blockImage = new BlockImage
                        {
                            BlockImageId = Convert.ToInt32(reader["BlockImageId"]),
                            BlockId = Convert.ToInt32(reader["BlockId"]),
                            ImageId = Convert.ToInt32(reader["ImageId"]),
                            IsLogo = Convert.ToBoolean(reader["IsLogo"])

                        };
                        var siteImage = new SiteImage
                        {
                            FileName = Convert.ToString(reader["FileName"]),
                            ImageUrl = Convert.ToString(reader["ImageUrl"]),
                            SequenceNumber = reader["SequenceNumber"] != DBNull.Value ? Convert.ToInt32(reader["SequenceNumber"]) : null,
                            WebsiteId = Convert.ToInt32(reader["WebsiteId"])
                        };

                        var siteLink = new SiteLink
                        {
                            LinkName = reader["LinkName"] != DBNull.Value ? Convert.ToString(reader["LinkName"]) : null,
                            Url = reader["Url"] != DBNull.Value ? Convert.ToString(reader["Url"]) : null,
                            ToolTip = reader["ToolTip"] != DBNull.Value ? Convert.ToString(reader["ToolTip"]) : null,
                            Title = reader["Title"] != DBNull.Value ? Convert.ToString(reader["Title"]) : null,
                            Target = reader["Target"] != DBNull.Value ? Convert.ToString(reader["Target"]) : null,

                        };

                        siteImage.BlockImage = blockImage;
                        siteImage.SiteLink = siteLink;

                        siteImages.Add(siteImage);

                    }
                }
                catch (Exception ex)
                {
                    //TODO: Add logging here ...
                    siteImages.Clear();
                }
            }
            return siteImages ?? new List<SiteImage>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketPlaceId"></param>
        /// <returns></returns>
        public IList<SiteLink> GetSiteLinksByMarketplaceId(int marketPlaceId)
        {
            var siteLinks = new List<SiteLink>();
            string sql = @" SELECT
                            bl.BlockLinkId ,
                           bl.BlockId ,
                           bl.LinkId ,
                           sl.LinkName ,
                           sl.Target ,
                           sl.Title ,
                           sl.ToolTip ,
                           sl.WebsiteId ,
                           sli.ImageId ,
                           si.FileName ,
                           si.ImageUrl ,
                           si.SequenceNumber ,
                           si.WebsiteId 
                          FROM BlockLinks bl 
                            INNER JOIN SiteLinks sl 
                              ON bl.LinkId = sl.LinkId 
                             LEFT JOIN SiteLinksImages sli 
                              ON sl.LinkId = sli.LinkId 
                             LEFT JOIN SiteImages si 
                               ON sli.ImageId = si.SiteImageId
            WHERE sl.WebsiteId =  @marketplace_id;";

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand(sql, connection)
                    {
                        CommandType = System.Data.CommandType.Text
                    };

                    command.Parameters.AddWithValue("@marketplace_id", marketPlaceId);

                    connection.Open();
                    using var reader = command.ExecuteReader();

                    while (reader.Read())
                    {

                        var blockLink = new BlockLink
                        {
                            BlockLinkId = Convert.ToInt32(reader["BlockLinkId"]),
                            BlockId = Convert.ToInt32(reader["BlockId"]),
                            LinkId = Convert.ToInt32(reader["LinkId"])
                        };
                        var siteLink = new SiteLink
                        {
                            LinkName = reader["LinkName"] != DBNull.Value ? Convert.ToString(reader["LinkName"]) : null,
                            ToolTip = reader["ToolTip"] != DBNull.Value ? Convert.ToString(reader["ToolTip"]) : null,
                            Title = reader["Title"] != DBNull.Value ? Convert.ToString(reader["Title"]) : null,
                            Target = reader["Target"] != DBNull.Value ? Convert.ToString(reader["Target"]) : null,
                            WebsiteId = Convert.ToInt32(reader["WebsiteId"]),

                        };

                        var blockImage = new BlockImage
                        {
                            BlockId = reader["BlockId"] != DBNull.Value ? Convert.ToInt32(reader["BlockId"]) : null,
                            ImageId = reader["ImageId"] != DBNull.Value ? Convert.ToInt32(reader["ImageId"]) : null,

                        };
                        var siteImage = new SiteImage
                        {
                            FileName = Convert.ToString(reader["FileName"]),
                            ImageUrl = Convert.ToString(reader["ImageUrl"]),
                            SequenceNumber = reader["SequenceNumber"] != DBNull.Value ? Convert.ToInt32(reader["SequenceNumber"]) : null,
                            WebsiteId = reader["WebsiteId"] != DBNull.Value ? Convert.ToInt32(reader["WebsiteId"]) : null
                        };


                        siteLink.BlockLink = blockLink;
                        siteLink.SiteImage = siteImage;

                        siteLinks.Add(siteLink);

                    }
                }
                catch (Exception ex)
                {
                    //TODO: Add logging here ...
                    siteLinks.Clear();
                }
            }
            return siteLinks ?? new List<SiteLink>();
        }
    }
}
