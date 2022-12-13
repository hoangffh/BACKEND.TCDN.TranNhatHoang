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
    public class UnitRepository : BaseRepository<Unit>, IUnitRepository
    {
        public UnitRepository(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// check dublicate nhân viên
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        public override bool CheckDuplicate(string propName, string propValue, Guid? unitId)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var tableName = typeof(Unit).Name;

                string sqlString = $"Select * from {tableName} where {propName} = @propValue";

                if(unitId != Guid.Empty)
                {
                    string unitNameSql = $"Select unitName from {tableName} where unitId = '{unitId}'";
                    var unitName = sqlConnection.QueryFirstOrDefault<string>(unitNameSql);
                    sqlString = $"Select * from {tableName} where {propName} = @propValue and unitName <> '{unitName}'";
                }

                DynamicParameters paras = new DynamicParameters();
                paras.Add("@propValue", propValue);

                var res = sqlConnection.QueryFirstOrDefault<object>(sqlString, paras);

                if (res != null)
                    return true;
                return false;
            }
        }
       
      
        /// <summary>
        /// Phân trang dữ liệu
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        public IEnumerable<Unit> GetUnitPaging(int PageSize, int PageNumber, string? TextSearch)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var fstRecord = (PageNumber - 1) * PageSize;
                DynamicParameters dynamic = new DynamicParameters();
                dynamic.Add("@textSearch", TextSearch);
                dynamic.Add("@pageSize", PageSize);
                dynamic.Add("@fstRecord", fstRecord);
                //Sql string join 2 bảng bảng lấy dữ liệu
                var sqlCommand = "select * from unit";

                //Nếu TextSearch tồn tại => add string tìm kiếm
                if (!string.IsNullOrEmpty(TextSearch))
                {
                    sqlCommand = "  " + sqlCommand + $" WHERE unitName LIKE '%{TextSearch}%'  ORDER BY  modifiedOfDate DESC,modifiedOfDate DESC LIMIT @fstRecord,@pageSize "; 
                    var Units = sqlConnection.Query<Unit>(sqlCommand, param: dynamic);
                    return Units;
                }
                //add string paging
                else
                {
                    sqlCommand += " ORDER BY  modifiedOfDate DESC,modifiedOfDate DESC LIMIT @fstRecord,@pageSize";
                    var Units = sqlConnection.Query<Unit>(sqlCommand, param: dynamic);
                    return Units;
                }

            }

        }
       
        
        /// <summary>
        /// Tổng số bản ghi khi tìm kiếm
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        public int GetTotalRecordSearch(string textSearch)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var tableName = typeof(Unit).Name;
                DynamicParameters paras = new DynamicParameters();
                var sqlCommand = $"Select COUNT(*) FROM unit  WHERE  unitName LIKE '%{textSearch}%'";
                paras.Add("@textSearch", textSearch);
                var total = sqlConnection.QueryFirstOrDefault<int>(sqlCommand);
                return total;

            }

        }

      
    }
}
