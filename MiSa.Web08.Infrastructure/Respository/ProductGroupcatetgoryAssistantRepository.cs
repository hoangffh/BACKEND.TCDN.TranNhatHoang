using Dapper;
using Microsoft.Extensions.Configuration;
using MiSa.Web08.Core;
using MiSa.Web08.Core.Entities;
using MiSa.Web08.Core.Interfaces.Infrastructure;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiSa.Web08.Infrastructure.Repository
{
    public class ProductGroupcatetgoryAssistantRepository: BaseRepository<Productgroupcatetgoryassistant>, IProductGroupcatetgoryAssistantRepository
    {
        public ProductGroupcatetgoryAssistantRepository(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Thêm mới bảng trun gian 
        /// </summary>
        /// <returns></returns>
        /// Created by: Thai(10/5/2022)
        public int InsertMultiVendorGroupsAssistant(List<Guid>listIds, Guid productId) {
            string bodyString = "";
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var cmd = new  MySqlCommand("", sqlConnection))
                {
                    var props = typeof(Productgroupcatetgoryassistant).GetProperties();
                    for (int i = 0; i < listIds.Count; i++)
                    {
                        bodyString += $"(@productGroupcatetgoryAssistant{i}, ";
                        cmd.Parameters.Add(new MySqlParameter($"@productGroupcatetgoryAssistant{i}", Guid.NewGuid().ToString()));
                        bodyString += $"@groupCategoryId{i},";
                        cmd.Parameters.Add(new MySqlParameter($"@groupCategoryId{i}", listIds[i].ToString()));
                        bodyString += $"@productId{i}),";
                        cmd.Parameters.Add(new MySqlParameter($"@productId{i}", productId.ToString()));
                        


                    }
                    //cmd.Parameters.AddWithValue($"@{columnIdName}", entityId.ToString());
                    var sqlCommandString = $"insert into productgroupcatetgoryassistant values {bodyString.Substring(0, bodyString.Length - 1)}";
                    cmd.CommandText = sqlCommandString;
                    var res = cmd.ExecuteNonQuery();
                    return res;
                }
            }
        }

        /// <summary>
        /// Xóa bảng trung gian theo Id vật tư hàng hóa
        /// </summary>
        /// <returns></returns>
        /// Created by: Thai(10/5/2022)
        public int DeleteMultiProductGroupAssistantByProductId(Guid productId)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                sqlConnection.Open();
                var sqlCommand = "delete from productgroupcatetgoryassistant where productId = @productId";
                using (var cmd = new MySqlCommand(sqlCommand, sqlConnection))
                {
                    cmd.Parameters.AddWithValue($"@productId", productId.ToString());
                    var result = cmd.ExecuteNonQuery();
                    return result;
                }
            }
        }




    }
}
