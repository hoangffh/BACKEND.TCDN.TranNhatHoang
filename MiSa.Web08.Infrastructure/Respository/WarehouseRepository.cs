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
    public class WarehouseRepository : BaseRepository<Warehouse>, IWarehouseRepository
    {
        public WarehouseRepository(IConfiguration configuration) : base(configuration)
        {
        }

        
       
        
        /// <summary>
        /// Phân trang dữ liệu
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        public IEnumerable<Warehouse> GetWarehousePaging(int PageSize, int PageNumber, string? TextSearch)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var fstRecord = (PageNumber - 1) * PageSize;
                DynamicParameters dynamic = new DynamicParameters();
                dynamic.Add("@textSearch", TextSearch);
                dynamic.Add("@pageSize", PageSize);
                dynamic.Add("@fstRecord", fstRecord);
                //Sql string join 2 bảng bảng lấy dữ liệu
                var sqlCommand = "select * from warehouse";

                //Nếu TextSearch tồn tại => add string tìm kiếm
                if (!string.IsNullOrEmpty(TextSearch))
                {
                    sqlCommand = "  " + sqlCommand + $" WHERE warehouseCode LIKE '%{TextSearch}%' OR warehouseName LIKE '%{TextSearch}%'   ORDER BY modifiedOfDate DESC,modifiedOfDate DESC LIMIT @fstRecord,@pageSize "; 
                    var Warehouses = sqlConnection.Query<Warehouse>(sqlCommand, param: dynamic);
                    return Warehouses;
                }
                //add string paging
                else
                {
                    sqlCommand += " ORDER BY modifiedOfDate DESC,modifiedOfDate DESC LIMIT @fstRecord,@pageSize";
                    var Warehouses = sqlConnection.Query<Warehouse>(sqlCommand, param: dynamic);
                    return Warehouses;
                }

            }

        }
       
      
    }
}
