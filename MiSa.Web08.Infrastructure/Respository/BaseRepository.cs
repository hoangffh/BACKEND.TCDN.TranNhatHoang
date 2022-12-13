using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using MiSa.Web08.Core;
using MiSa.Web08.Core.Interfaces.Infrastructure;
using MySqlConnector;
using MiSa.Web08.Core.Entities;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MiSa.Web08.Infrastructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T>
    {
        #region field and constructor
        /// <summary>
        /// Khởi tạo kết nối đến database
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        protected string connectionString = String.Empty;   
        protected MySqlConnection sqlConnection;
        public BaseRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("MISA_TCDN_WEB08");
        }
        #endregion

        #region method
        /// <summary>
        /// Lấy toàn bộ thông tin nhân viên
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        public IEnumerable<T> Get()
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
            var tableName = typeof(T).Name;
                var sqlString = $"select * from {tableName}";
                var entities = sqlConnection.Query<T>(sqlString);
                return entities;
            }
        }
        /// <summary>
        ///Lấy thông tin chi tiết nhân viên theo Id
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        public T Get(Guid? entityId)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var tableName = typeof(T).Name;
                string sqlString = $"select * from {tableName} where {tableName}Id = @entityId";

                var paras = new DynamicParameters();
                paras.Add("@entityId", entityId);

                var entity = sqlConnection.QueryFirstOrDefault<T>(sqlString, paras);

                return entity;
            }

            
        }
        public IEnumerable<Table> GetReference(Guid? entityId)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var tableName = typeof(T).Name;

                string sqlString = $"select ProductId from {tableName} inner join product on {tableName}.{tableName}Id  = product.{tableName}Id where {tableName}.{tableName}Id = @entityId";
                var paras = new DynamicParameters();
                paras.Add("@entityId", entityId);

                var entity = sqlConnection.Query<Table>(sqlString, paras);
                return entity;
            }
           


        }
        public IEnumerable<object> GetPaging(int PageSize, int PageNumber, string TextSearch)
        {
            return new List<object>();
        }
        /// <summary>
        /// Thêm mới nhân viên
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        public int Insert(T entity)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var tableName = typeof(T).Name;

                //Khai báo 2 chuỗi sẽ dùng khi thực hiện insert
                var listProperty = string.Empty;
                var listValue = string.Empty;

                var paras = new DynamicParameters();
                var props = typeof(T).GetProperties();
                foreach (var prop in props)
                {
                    

                    //Kiểm tra nếu prop hiện tại có attri [NotMap] thì bỏ qua
                    var notMapProp = prop.GetCustomAttributes(typeof(NotMap), true);

                    if(notMapProp.Length == 0)
                    {
                        var propName = prop.Name;
                        var propValue = prop.GetValue(entity);

                        var propType = prop.PropertyType; 
                        //Kiểm tra nếu property hiện tại là PK => tạo mã mới để insert
                        var isPrimaryKey = Attribute.IsDefined(prop, typeof(PrimaryKey));
                        if (isPrimaryKey == true && propType == typeof(Guid))
                            propValue = Guid.NewGuid();
                        var isAutoDate = Attribute.IsDefined(prop, typeof(autoDate));
                        if (isAutoDate == true)
                            propValue = DateTime.Now;
                        var isUsing = Attribute.IsDefined(prop, typeof(isUsing));
                        if (isUsing == true)
                            propValue = "1";
                        //add thêm giá trị vào 2 chuỗi sẽ xử dụng để insert
                        listProperty += $"{propName},";
                        listValue += $"@{propName},";

                        paras.Add($"@{propName}", propValue);
                    }
                }
                //Xóa kí tự thừa "," ở cuối 2 chuỗi
                listProperty = listProperty.Substring(0, listProperty.Length - 1);
                listValue = listValue.Substring(0, listValue.Length - 1);

                var sqlString = $"insert into {tableName}({listProperty}) values({listValue})";
                var res = sqlConnection.Execute(sqlString, paras);
                return res;
            }
        }
        /// <summary>
        /// Sửa nhân viên
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        public int Update(T entity, Guid entityId)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var tableName = typeof(T).Name;
                var listProps = typeof(T).GetProperties();

                //Khai báo 1 chuỗi option sử dụng để Update dữ liệu
                var option = string.Empty;
                var paras = new DynamicParameters();
                paras.Add("@entityId", entityId);
                foreach (var prop in listProps)
                {
                    //Kiểm tra nếu prop hiện tại chưa [NotMap] => bỏ qua
                    var inor = prop.GetCustomAttributes(typeof(NotMap), true);
                    if(inor.Length == 0)
                    {
                        var propName = prop.Name;
                        var propValue = prop.GetValue(entity);

                        var isAutoDate = Attribute.IsDefined(prop, typeof(autoDate));
                        if(isAutoDate && propName != "createdDate")
                            propValue = DateTime.Now;
                        //add thêm giá trị vào chuỗi sẽ xử dụng để update
                        option += $"{propName} = @{propName},";
                        //add dữ liệu cho các tham số sẽ dùng khi update
                        paras.Add($"@{propName}", propValue);
                    }
                    
                }
                //Xóa kí tự thừa "," ở cuối chuỗi
                option = option.Substring(0, option.Length - 1);

                var sqlString = $"update {tableName} set {option} where {tableName}Id = @entityId";

                var res = sqlConnection.Execute(sqlString, paras);
                return res;
            }
        }
        /// <summary>
        /// Xóa nhân viên theo Id
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        public int Delete(Guid entityId)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
               
                var tableName = typeof(T).Name;
                var sqlString = $"delete from {tableName} where {tableName}Id = @entityId";

                var paras = new DynamicParameters();
                paras.Add("@entityId", entityId);

                var res = sqlConnection.Execute(sqlString, paras);

                return res;
            }
        }    
        /// <summary>
        /// Xóa nhiều nhân viên
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        public int DeleteMulti(List<Guid> ids)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var transaction = sqlConnection.BeginTransaction())
                {
                    var tableName = typeof(T).Name;
                    List<string> temp = new List<string>();

                    foreach (var id in ids)
                    {
                        string idString = $" {tableName}Id = '{id}' ";
                        temp.Add(idString);
                    }
                    string condition = String.Join("OR", temp);
                    string sqlString = $"Delete from {@tableName} where {@condition}";
                    DynamicParameters paras = new DynamicParameters();
                    paras.Add("@tableName", tableName);
                    paras.Add("@condition", condition);
                    var res = sqlConnection.Execute(sqlString, null, transaction);
                    if (res == ids.Count)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                    return res;

               
                }
               
            }
        }
        public virtual bool CheckDuplicate(string propName, string propValue, Guid? id)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var tableName = typeof(T).Name;

                string sqlString = $"Select * from {tableName} where {propName} = @propValue";

                if (id != Guid.Empty)
                {
                    string tableCodeSql = $"Select {tableName}Code from {tableName} where {tableName}Id = '{id}'";
                    var tableCode = sqlConnection.QueryFirstOrDefault<string>(tableCodeSql);
                    sqlString = $"Select * from {tableName} where {propName} = @propValue and {tableName}Code <> '{tableCode}'";
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
        /// Tổng số bản  ghi
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        public int GetTotalRecord()
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var tableName = typeof(T).Name;
                var sqlCommand = $"Select COUNT(*) FROM {tableName} ";
                var total = sqlConnection.QueryFirstOrDefault<int>(sqlCommand);
                return total;
            }
        }
        /// <summary>
        /// Tổng số lượng tồn , giá trị tồn
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        public double GetTotalQuantityStock()
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var tableName = typeof(T).Name;
                var sqlCommand = $"Select SUM(quantityStock) FROM {tableName} ";
                var total = sqlConnection.QueryFirstOrDefault<double>(sqlCommand);
                return total;
            }
        }

        public double GetTotalExistentialValue()
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var tableName = typeof(T).Name;
                var sqlCommand = $"Select SUM(existentialValue) FROM {tableName} ";
                var total = sqlConnection.QueryFirstOrDefault<double>(sqlCommand);
                return total;
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
                var tableName = typeof(T).Name;
                DynamicParameters paras = new DynamicParameters();
                var sqlCommand = $"Select COUNT(*) FROM {tableName}  WHERE {tableName}Code LIKE '%{textSearch}%' OR {tableName}Name LIKE '%{textSearch}%'";
                paras.Add("@textSearch", textSearch);
                var total = sqlConnection.QueryFirstOrDefault<int>(sqlCommand);
                return total;

            }

        }
        //not use
        public static string ToPascalCase(string str)
        {
            string result = Regex.Replace(str, "_[a-z]", delegate (Match m) {
                return m.ToString().TrimStart('_').ToUpper();
            });

            result = char.ToUpper(result[0]) + result.Substring(1);

            return result;
        }
        #endregion
    }
}
