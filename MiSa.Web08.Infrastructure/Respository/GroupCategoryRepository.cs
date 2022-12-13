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
    public class GroupCategoryRepository : BaseRepository<GroupCategory>, IGroupCategoryRepository
    {
        public GroupCategoryRepository(IConfiguration configuration) : base(configuration)
        {
        }

        
       
       
        /// <summary>
        /// Phân trang dữ liệu
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        public IEnumerable<GroupCategory> GetGroupCategoriesPaging(int PageSize, int PageNumber, string? TextSearch)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var fstRecord = (PageNumber - 1) * PageSize;
                DynamicParameters dynamic = new DynamicParameters();
                dynamic.Add("@textSearch", TextSearch);
                dynamic.Add("@pageSize", PageSize);
                dynamic.Add("@fstRecord", fstRecord);
                //Sql string join 2 bảng bảng lấy dữ liệu
                var sqlCommand = "select * from GroupCategory";

                //Nếu TextSearch tồn tại => add string tìm kiếm
                if (!string.IsNullOrEmpty(TextSearch))
                {
                    sqlCommand = "  " + sqlCommand + $" WHERE groupCategoryCode LIKE '%{TextSearch}%' OR groupCategoryName LIKE '%{TextSearch}%'   ORDER BY modifiedOfDate DESC,modifiedOfDate DESC LIMIT @fstRecord,@pageSize "; 
                    var GroupCategories = sqlConnection.Query<GroupCategory>(sqlCommand, param: dynamic);
                    return GroupCategories;
                }
                //add string paging
                else
                {
                    sqlCommand += " ORDER BY modifiedOfDate DESC,modifiedOfDate DESC LIMIT @fstRecord,@pageSize";
                    var GroupCategories = sqlConnection.Query<GroupCategory>(sqlCommand, param: dynamic);
                    return GroupCategories;
                }

            }

        }
       

      
    }
}
