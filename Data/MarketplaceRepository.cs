using marketplacewebcore.Areas.Identity.Pages.Account.Manage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using webui.Areas.Identity.Pages.Account.Manage;
using webui.Interfaces;
using webui.Models;

namespace webui.Data
{
    public class MarketplaceRepository : IMarketplaceRepository
    {

        private string _connectionString;
        private string _contentConnectionString;
        //private readonly webuiIdentityDbContext _context;

        private readonly IConfiguration _configuration;

        public MarketplaceRepository(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:MarketplaceConnection"];
            _contentConnectionString = configuration["ConnectionStrings:ContentConnection"];
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

            try
            {

                using (var connection = new SqlConnection(_contentConnectionString))
                {
                    var command = new SqlCommand(sql, connection)
                    {
                        CommandType = System.Data.CommandType.Text
                    };
                    command.Parameters.AddWithValue("@domain", domain);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            marketPlace = new Marketplace
                            {
                                MarketplaceId = reader["MarketPlaceId"].ToString(),
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
                        marketplaceSetting.Template = template;
                        marketPlace.Settings = marketplaceSetting;

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return marketPlace ?? new Marketplace();
        }

        public Marketplace GetMarketplaceById(string marketPlacdId)
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
                            MarketplaceId = reader["MarketPlaceId"].ToString(),
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

        public MarketplaceSetting GetMarketplaceSettingsById(string marketPlaceId)
        {
            return new MarketplaceSetting();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marketPlaceId"></param>
        /// <returns></returns>
        public MarketplaceTheme GetThemeByMarketplaceId(string marketPlaceId)
        {

            return new MarketplaceTheme();

        }

        public Template GetTemplateById(int templateId)
        {
           
            return  new Template();
        }

        public Menu GetMenuByName(string marketPlaceId, string menuName)
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
                using (var connection = new SqlConnection(_contentConnectionString))
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


        public async Task<decimal> AddShop(ShopModel.InputModel shopModel)
        {
            string sql = @"INSERT INTO MarketplaceShop (ShopName, [Description], Established, OwnerId, OwnerFName, OwnerLName, Email, MobilePhone, ShopPhone, IsActive)
                            VALUES (@shop_name, @description, 
                                    @established, @owner_id, 
                                    @fname, @lname, @email, 
                                    @mobile_phone, @shop_phone, 1)
                                    SELECT SCOPE_IDENTITY()";

            var newId = 0M;

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                command.Parameters.AddWithValue("@shop_name", shopModel.ShopName);
                command.Parameters.AddWithValue("@description", shopModel.Description);
                command.Parameters.AddWithValue("@established", shopModel.Established);
                command.Parameters.AddWithValue("@owner_id", shopModel.OwnerId);
                command.Parameters.AddWithValue("@fname", shopModel.OwnerFirstName);
                command.Parameters.AddWithValue("@lname", shopModel.OwnerLastName);
                command.Parameters.AddWithValue("@email", shopModel.Email);
                command.Parameters.AddWithValue("@mobile_phone", shopModel.MobilePhone);
                command.Parameters.AddWithValue("@shop_phone", shopModel.ShopPhone);

                connection.Open();

                //rowsAffected = await command.ExecuteNonQueryAsync();
                newId = (decimal)await command.ExecuteScalarAsync();

            }
            return newId;
        }

        public async Task<int> SaveShopData(ShopModel.InputModel shopModel)
        {

            await DeleteHairStyle(shopModel.ShopId);


            string sql = @"UPDATE [dbo].[MarketplaceShop]
                            SET [ShopName] = @shop_name
                                ,[Description] = @description
                                ,[OwnerFName] = @first_name
                                ,[OwnerLName] = @last_name
                                ,[Email] = @email
                                ,[MobilePhone] = @mobile_phone
                                ,[ShopPhone] = @shop_phone

                            WHERE [OwnerId] = @owner_id;";

            var rowsAffected = 0;

            await using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                command.Parameters.AddWithValue("@shop_name", shopModel.ShopName);
                command.Parameters.AddWithValue("@description", shopModel.Description);
                command.Parameters.AddWithValue("@first_name", shopModel.OwnerFirstName);
                command.Parameters.AddWithValue("@last_name", shopModel.OwnerLastName);
                command.Parameters.AddWithValue("@email", shopModel.Email);
                command.Parameters.AddWithValue("@mobile_phone", shopModel.MobilePhone);
                command.Parameters.AddWithValue("@shop_phone", shopModel.ShopPhone);
                command.Parameters.AddWithValue("@owner_id", shopModel.OwnerId);

                connection.Open();

                rowsAffected = await command.ExecuteNonQueryAsync();

            }
            return await Task.FromResult<int>(rowsAffected);
        }

        public async Task<int> DeleteAmenity(int shopId, int locationId, int amenityId = 0)
        {
            string sql = @"DELETE FROM [dbo].[ShopAmenities]";

            var rowsAffected = 0;

            await using (var connection = new SqlConnection(_connectionString))
            {
                if (amenityId == 0)
                {
                    sql += " WHERE [ShopId] = @shop_id AND [LocationId] = @location_id";
                }
                else
                {
                    sql += @" WHERE [ShopId] = @shop_id AND [LocationId] = @location_id
                            AND AmentityId = @amenityId;";
                }

                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };

                command.Parameters.AddWithValue("@shop_id", shopId);
                command.Parameters.AddWithValue("@location_id", locationId);

                if (sql.Contains("@amenity_id"))
                    command.Parameters.AddWithValue("@amenity_id", amenityId);

                connection.Open();

                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            return await Task.FromResult<int>(rowsAffected);
        }


        public async Task<int> SaveAmenities(int shopId, int locationId, IList<Amenity> amenities)
        {
            //Delete all amenities associated with a shop and location
            var rowsAffected = await DeleteAmenity(shopId, locationId);

            string sql = @"INSERT INTO [dbo].[ShopAmenities]
                           (ShopId, LocationId, AmenityId)
                           VALUES(@shop_id, @location_id, @amenity_id);";

            await using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                command.Parameters.AddWithValue("@shop_id", shopId);
                command.Parameters.AddWithValue("@location_id", locationId);
                connection.Open();

                foreach (var item in amenities)
                {
                    command.Parameters.AddWithValue("@amenity_id", item.AmenityId);

                    rowsAffected += await command.ExecuteNonQueryAsync();

                    command.Parameters.RemoveAt(2);
                }
            }
            return await Task.FromResult<int>(rowsAffected);
        }


        public async Task<ShopModel.InputModel> GetShopModelById(string id)
        {
            var shopModel = new ShopModel.InputModel();

            string sql = @"SELECT s.[ShopId]
                            ,[ShopName]
                            ,[Description]
                            ,[Established]
                            ,[OwnerId]
                            ,[OwnerFName]
                            ,[OwnerLName]
                            ,[Email]
                            ,[MobilePhone]
                            ,[ShopPhone]
                            ,[IsActive],
                            h.ShopOwner,
                            h.YearsExperience
                    FROM [MarketplaceShop]  s
                        INNER JOIN dbo.HairPros h
                            ON s.ShopId = h.ShopId
                    WHERE [OwnerId] = @Id;";

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        shopModel.ShopId = Convert.ToInt32(reader["ShopId"]);
                        shopModel.ShopName = reader["ShopName"].ToString();
                        shopModel.Description = reader["Description"].ToString();
                        shopModel.Established = Convert.ToInt16(reader["Established"]);
                        shopModel.OwnerId = reader["OwnerId"].ToString();
                        shopModel.OwnerFirstName = reader["OwnerFName"].ToString();
                        shopModel.OwnerLastName = reader["OwnerLName"].ToString();
                        shopModel.Email = reader["Email"].ToString();
                        shopModel.ShopPhone = reader["ShopPhone"].ToString();
                        shopModel.MobilePhone = reader["MobilePhone"].ToString();
                        shopModel.HairProfessional = new HairPro
                        {
                            IsShopOwner = Convert.ToBoolean(reader["ShopOwner"].ToString()),
                            YearsOfExperience = Convert.ToInt32(reader["YearsExperience"])
                        };
                        break;
                    }

                }
            }
            return shopModel ?? new ShopModel.InputModel();
        }


        public async Task<IList<LocationsModel.InputModel>> GetLocationsByShopId(int shopId)
        {
            var locations = new List<LocationsModel.InputModel>();

            var sql = @"SELECT 
                         [LocationId]
                        ,[LocationName]
                        ,[ShopId]
                        ,[Address1]
                        ,[Address2]
                        ,[City]
                        ,[State]
                        ,[ZipCode]
                    FROM [dbo].[ShopLocation]
                    WHERE ShopId = @shop_id;";


            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                command.Parameters.AddWithValue("@shop_id", shopId);
                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var location = new LocationsModel.InputModel
                        {
                            LocationId = Convert.ToInt32(reader["LocationId"]),
                            LocationName = reader["LocationName"].ToString(),
                            ShopId = Convert.ToInt32(reader["ShopId"]),
                            Address1 = reader["Address1"].ToString(),
                            Address2 = reader["Address2"].ToString(),
                            City = reader["City"].ToString(),
                            State = reader["State"].ToString(),
                            ZipCode = reader["ZipCode"].ToString()
                        };
                        locations.Add(location);

                    }

                }
            }
            return locations;

        }

        public async Task<decimal> AddLocation(LocationsModel.InputModel locationModel)
        {
            string sql = @"INSERT INTO [dbo].[ShopLocation]
                            ([LocationName]
                            ,[ShopId]
                            ,[Address1]
                            ,[Address2]
                            ,[City]
                            ,[State]
                            ,[ZipCode])
                        VALUES
                            (@location_name
                            ,@shop_id
                            ,@address_1
                            ,@address_2
                            ,@city
                            ,@state
                            ,@zip_code)
                            SELECT SCOPE_IDENTITY()";

            var newId = 0M;

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                command.Parameters.AddWithValue("@location_name", locationModel.LocationName);
                command.Parameters.AddWithValue("@shop_id", locationModel.ShopId);
                command.Parameters.AddWithValue("@address_1", locationModel.Address1);
                command.Parameters.AddWithValue("@address_2", locationModel.Address2);
                command.Parameters.AddWithValue("@city", locationModel.City);
                command.Parameters.AddWithValue("@state", locationModel.State);
                command.Parameters.AddWithValue("@zip_code", locationModel.ZipCode);

                connection.Open();

                //rowsAffected = await command.ExecuteNonQueryAsync();
                newId = (decimal)await command.ExecuteScalarAsync();
            }
            return newId;
        }

        public Task<int> EditLocation(LocationsModel.InputModel locationModel)
        {
            return Task.FromResult<int>(0);
        }

        public async Task<IList<Amenity>> GetAmenities()
        {
            var amenities = new List<Amenity>();

            string sql = @"SELECT 
                             [AmenityId]
                            ,[AmenityName]
                            ,[AmenityDescription]
                            ,[IsFilter]
                        FROM [dbo].[Amenities];";

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var amenity = new Amenity
                        {
                            AmenityId = Convert.ToInt32(reader["AmenityId"]),
                            AmenityName = reader["AmenityName"].ToString(),
                            Description = reader["AmenityDescription"].ToString(),
                            IsFilter = Convert.ToBoolean(reader["IsFilter"])
                        };
                        amenities.Add(amenity);
                    }

                }
            }
            return amenities ?? new List<Amenity>();
        }

        public async Task<IList<HairStyle>> GetHairStyles()
        {
            var hairStyles = new List<HairStyle>();

            string sql = @"SELECT [StyleId]
                                ,[StyleName]
                                ,[TypeId]
                                ,[Description]
                                ,[IsFilter]
                            FROM [dbo].[HairStyles];";

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var style = new HairStyle
                        {
                            StyleId = Convert.ToInt32(reader["StyleId"]),
                            StyleName = reader["StyleName"].ToString(),
                            Description = reader["Description"].ToString(),
                            IsFilter = Convert.ToBoolean(reader["IsFilter"])
                        };
                        hairStyles.Add(style);
                    }

                }
            }
            return hairStyles ?? new List<HairStyle>();
        }


        public async Task<int> SaveHairStyles(int shopId, IList<HairStyle> styles)
        {

            //Delete all amenities associated with a shop and location
            var rowsAffected = 0;

            await DeleteHairStyle(shopId);

            string sql = @"INSERT INTO [dbo].[ShopHairStyles]
                           ([ShopId]
                            ,[HairStyleId])
                           VALUES(@shop_id, @style_id);";

            await using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                command.Parameters.AddWithValue("@shop_id", shopId);
                connection.Open();

                foreach (var item in styles)
                {
                    command.Parameters.AddWithValue("@style_id", item.StyleId);

                    rowsAffected += await command.ExecuteNonQueryAsync();
                    command.Parameters.RemoveAt(1);
                }
            }
            return await Task.FromResult<int>(rowsAffected);
        }

        public async Task<int> DeleteHairStyle(int shopId, int id = 0, int hairStyleId = 0)
        {
            string sql = @"DELETE FROM [dbo].[ShopHairStyles]";

            var rowsAffected = 0;

            await using (var connection = new SqlConnection(_connectionString))
            {
                if (id == 0)
                {
                    sql += " WHERE [ShopId] = @shop_id ";
                }
                else if (id > 0)
                {
                    sql += @" WHERE [ShopId] = @shop_id AND Id = @id ";
                }

                if (hairStyleId > 0)
                {
                    sql += " AND [HairStyleId] = @hair_style_id;";
                }

                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };

                command.Parameters.AddWithValue("@shop_id", shopId);

                if (sql.Contains("@id"))
                    command.Parameters.AddWithValue("@id", id);

                if (sql.Contains("@hair_style_id"))
                    command.Parameters.AddWithValue("@hair_style_id", hairStyleId);

                connection.Open();

                rowsAffected = await command.ExecuteNonQueryAsync();
            }
            return await Task.FromResult<int>(rowsAffected);
        }

        public async Task<IList<HairStyle>> GetHairStylesByShopId(int shopId)
        {
            var hairStyles = new List<HairStyle>();

            string sql = @"SELECT [Id]
                            ,sh.[ShopId]
                            ,sh.[HairStyleId]
                            ,h.StyleName
                        FROM HairStyles h
                        INNER JOIN 
                        [dbo].[ShopHairStyles] sh
                        on h.StyleId = sh.HairStyleId
                    Where sh.[ShopId] = @id;";

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                command.Parameters.AddWithValue("@id", shopId);
                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var hairStyle = new HairStyle
                        {
                            StyleId = Convert.ToInt32(reader["HairStyleId"]),
                            StyleName = reader["StyleName"].ToString(),
                        };

                        hairStyles.Add(hairStyle);
                    }

                }
            }
            return hairStyles ?? new List<HairStyle>();
        }

        public async Task<decimal> AddHairPro(int shopId, HairPro hairPro)
        {
            string sql = @"INSERT INTO HairPro (ShopId, [Name], ShopOwner, YearsExperience)
                            VALUES (@shop_id, @name, @is_shop_owner, @years_experience)
                                    SELECT SCOPE_IDENTITY()";

            var newId = 0M;

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                command.Parameters.AddWithValue("@shop_id", shopId);
                command.Parameters.AddWithValue("@name", hairPro.HairProName);
                command.Parameters.AddWithValue("@is_shop_owner", hairPro.IsShopOwner);
                command.Parameters.AddWithValue("@years_experience", hairPro.YearsOfExperience);

                connection.Open();
                newId = (decimal)await command.ExecuteScalarAsync();

            }
            return newId;
        }

        public async Task<int> SaveHairPro(int shopId, HairPro hairPro)
        {

            string sql = @"UPDATE [dbo].[HairPros]
                            SET  [Name] = @name
                                ,[ShopOwner] = @is_shop_owner
                                ,[YearsExperience] = @years_experience
                            WHERE [ShopId] = @shop_id;";

            var rowsAffected = 0;

            await using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                command.Parameters.AddWithValue("@shop_id", shopId);
                command.Parameters.AddWithValue("@name", hairPro.HairProName);
                command.Parameters.AddWithValue("@is_shop_owner", hairPro.IsShopOwner);
                command.Parameters.AddWithValue("@years_experience", hairPro.YearsOfExperience);

                connection.Open();

                rowsAffected = await command.ExecuteNonQueryAsync();

            }
            return await Task.FromResult<int>(rowsAffected);

        }


        public IEnumerable<IShopModel> GetAllMarketplaceShops()
        {
            var shopModel = new List<IShopModel>();

            string sql = @"SELECT [ShopId]
                            ,[ShopName]
                            ,[Description]
                            ,[Established]
                            ,[OwnerId]
                            ,[OwnerFName]
                            ,[OwnerLName]
                            ,[Email]
                            ,[MobilePhone]
                            ,[ShopPhone]
                            ,[IsActive]
                    FROM [MarketplaceShop]  s;";

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        yield return new ShopViewModel
                        {

                            ShopId = Convert.ToInt32(reader["ShopId"]),
                            ShopName = reader["ShopName"].ToString(),
                            Description = reader["Description"].ToString(),
                            Established = Convert.ToInt16(reader["Established"]),
                            OwnerId = reader["OwnerId"].ToString(),
                            OwnerFirstName = reader["OwnerFName"].ToString(),
                            OwnerLastName = reader["OwnerLName"].ToString(),
                            Email = reader["Email"].ToString(),
                            ShopPhone = reader["ShopPhone"].ToString(),
                            MobilePhone = reader["MobilePhone"].ToString()

                        };
                    }
                }
            }
        }

        public IEnumerable<HairPro> GetAllHairPros()
        {
            var shopModel = new List<HairPro>();

            string sql = @"SELECT [HairProId]
                            ,[ShopId]
                            ,[Name]
                            ,[ShopOwner]
                            ,[YearsExperience]
                    FROM [HairPros];";

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return new HairPro
                        {
                            HairProId = Convert.ToInt32(reader["HairProId"]),
                            //ShopId = Convert.ToInt32(reader["ShopId"]),
                            HairProName = reader["Name"].ToString(),
                            IsShopOwner = Convert.ToBoolean(reader["ShopOwner"].ToString()),
                            YearsOfExperience = Convert.ToInt16(reader["YearsExperience"])
                        };
                    }

                }
            }
        }

        public IEnumerable<ILocationsModel> GetAllLocations()
        {
            var locations = new List<ILocationsModel>();

            var sql = @"SELECT 
                         [LocationId]
                        ,[LocationName]
                        ,[ShopId]
                        ,[Address1]
                        ,[Address2]
                        ,[City]
                        ,[State]
                        ,[ZipCode]
                    FROM [dbo].[ShopLocation];";


            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return new LocationViewModel
                        {
                            LocationId = Convert.ToInt32(reader["LocationId"]),
                            LocationName = reader["LocationName"].ToString(),
                            ShopId = Convert.ToInt32(reader["ShopId"]),
                            Address1 = reader["Address1"].ToString(),
                            Address2 = reader["Address2"].ToString(),
                            City = reader["City"].ToString(),
                            State = reader["State"].ToString(),
                            ZipCode = reader["ZipCode"].ToString()
                        };

                    }

                }
            }

        }

        public IEnumerable<ShopAmenityModel> GetAllShopAmenities()
        {
            string sql = @"SELECT 
                             [ShopId]
                            ,[LocationId]
                            ,[AmenityId]
                        FROM [dbo].[ShopAmenities];";

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return new ShopAmenityModel
                        {
                            ShopId = Convert.ToInt32(reader["ShopId"]),
                            LocationId = Convert.ToInt32(reader["LocationId"]),
                            AmenityId = Convert.ToInt32(reader["AmenityId"])
                        };
                    }

                }
            }
        }

        public IEnumerable<ShopHairStyleModel> GetAllShopHairStyles()
        {
            string sql = @"SELECT 
                             [Id]
                            ,[ShopId]
                            ,[HairStyleId]
                        FROM [dbo].[ShopHairStyles];";

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(sql, connection)
                {
                    CommandType = System.Data.CommandType.Text
                };
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return new ShopHairStyleModel
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            ShopId = Convert.ToInt32(reader["ShopId"]),
                            HairStyleId = Convert.ToInt32(reader["HairStyleId"])
                        };
                    }
                }
            }
        }


    }
}
