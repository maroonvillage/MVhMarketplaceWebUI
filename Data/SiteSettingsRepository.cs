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
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
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
                             bi.BlockImageId ,
                             bi.BlockId ,
                             bi.ImageId ,
                             bi.IsLogo ,
                             si.FileName ,
                             si.ImageUrl ,
                             si.SequenceNumber ,
                             si.WebsiteId 
                            FROM BlockImages bi
                              INNER JOIN SiteImages si
                               ON bi.ImageId = si.SiteImageId
                            WHERE si.Websiteid = @marketPlaceId;";

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
                            ImageUrl = Convert.ToString(reader["FileName"]),
                            SequenceNumber = reader["SequenceNumber"] != DBNull.Value ? Convert.ToInt32(reader["SequenceNumber"]) : null,
                            WebsiteId = Convert.ToInt32(reader["WebsiteId"])
                        };

                        siteImage.BlockImage = blockImage;

                        siteImages.Add(siteImage);

                    }
                }
            }
            return siteImages ?? new List<SiteImage>();
        }
    }
}
