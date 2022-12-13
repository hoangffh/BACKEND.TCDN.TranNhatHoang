using Dapper;
using Microsoft.Extensions.Configuration;
using MiSa.Web08.Core;
using MiSa.Web08.Core.Entities;
using MiSa.Web08.Core.Enum;
using MiSa.Web08.Core.Exceptions;
using MiSa.Web08.Core.Interfaces.Infrastructure;
using MySqlConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiSa.Web08.Infrastructure.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(IConfiguration configuration) : base(configuration)
        {
        }


        /// <summary>
        /// Phân trang dữ liệu
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        public object GetProductPaging(int PageSize, int PageNumber, string? TextSearch, List<MyFilter> filter)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var fstRecord = (PageNumber - 1) * PageSize;
                DynamicParameters dynamic = new DynamicParameters();
                dynamic.Add("@textSearch", TextSearch);
                dynamic.Add("@pageSize", PageSize);
                dynamic.Add("@fstRecord", fstRecord);
                //Sql string join 2 bảng bảng lấy dữ liệu
                var sqlCommand = @" SELECT GROUP_CONCAT(groupCategoryCode,'') AS groupCategoryCode, product.productId, 
                    productCode, productName, taxReduction, insurance, amount, source, describes, quantityStock, existentialValue,explainBuy, explainSell, 
                    reduceAccount, warehouseAccount, returnAccount, revenueAccount, expenseAccount, discountAccount, discountRate, fixedPrice, nearestPrice, 
                    price, vatTax, importTax, exportTax,supplyExciseTax, Product.Status, typeProduct ,warehouse.warehouseCode,unit.unitName
                     FROM product left JOIN unit  ON product.unitId = unit.unitId  left JOIN warehouse  ON product.warehouseId = warehouse.warehouseId 
                     LEFT JOIN productgroupcatetgoryassistant ON product.productId = productgroupcatetgoryassistant.productId LEFT JOIN groupcategory
                     ON productgroupcatetgoryassistant.groupCategoryId = groupcategory.groupCategoryId";

                string sqlWhere = " where 1 = 1 ";

                //Sinh câu lệnh where theo array filter
                //TypeProduct = 0
                var sqlHaving = "";
                for (int i = 0; i < filter.Count; i++)
                {
                    //Custom filter 1 số trường đằng biệt
                    if (filter[i].Key == "GroupCategoryId")
                    {
                        sqlHaving += $" HAVING GROUP_CONCAT(groupcategory.groupCategoryid,'') LIKE '%{filter[i].Value}%' ";
                        continue;
                    }
                    //
                    //Tạo kiểu tìm kiếm trong database
                    var currKey = filter[i].Key;
                    string currentWhere = "";
                    if (filter[i].TableKey != "")
                        currKey = filter[i].TableKey + "." + currKey;
                    switch (filter[i].TypeOfFilter)
                    {

                        case "=":
                             currentWhere = currKey + " = ";
                             break;
                        case "contains":
                        case "start":
                        case "end":
                        {
                            currentWhere = currKey + " like ";
                            break;
                        }
                        case "difference":
                             currentWhere = currKey + " <> ";
                             break;
                        case "null":
                            currentWhere = currKey + " IS NULL ";
                            break;
                        case "notNull":
                            currentWhere = currKey + " IS NOT NULL ";
                            break;

                        default:
                            break;
                    }
                    var x = $"@{filter[i].Key}";

                    if (filter[i].DataType == "number")
                    {
                        currentWhere = currentWhere + x;
                        dynamic.Add(x, Convert.ToInt64(filter[i].Value));
                    }
                    else if (filter[i].DataType == "string")
                    {

                        if (filter[i].TypeOfFilter == "contains")
                        {
                            currentWhere = currentWhere + x;
                            dynamic.Add(x, $"%{(string)filter[i].Value}%");
                            
                        }
                        else if (filter[i].TypeOfFilter == "start")
                        {
                            currentWhere = currentWhere + x;
                            dynamic.Add(x, $"{(string)filter[i].Value}%");
                          
                        }
                        else if (filter[i].TypeOfFilter == "end")
                        {
                            currentWhere = currentWhere + x;
                            dynamic.Add(x, $"%{(string)filter[i].Value}");
                           
                        }

                        else if (filter[i].TypeOfFilter == "null" || filter[i].TypeOfFilter == "notNull")
                            currentWhere = currentWhere;
                        else
                        {
                            currentWhere = currentWhere + x;
                            dynamic.Add(x, (string)filter[i].Value);
                            
                        }
                           
                    }

                    //Nối mỗi chuỗi where bằng and
                    sqlWhere = sqlWhere + " and " + currentWhere;

                }
                //Sau khi sinh câu lệnh where bằng vòng lặp trên sẽ có dạng `TypeProduct = @TypeProduct and WareHouseId = @WareHouseId`
                //Sau đó check tiếp TextSearch để nối vào chuỗi sqlWhere


                //Nếu TextSearch tồn tại => add string tìm kiếm
                if (!string.IsNullOrEmpty(TextSearch))
                {
                    //Nếu có textsearch thì nối vào sqlWhere
                    //sqlWhere + ........

                    sqlWhere += $" and (productCode LIKE '%{TextSearch}%' OR  productName LIKE '%{TextSearch}%')";
                    //sqlWhere = sqlWhere + sqlTextSearch;
                    //var Prodcuts = sqlConnection.Query<Product>(sqlCommand, param: dynamic);
                    //return Prodcuts;
                }
                sqlCommand += sqlWhere;

                var sqlpaging = "";
                int totalRecord = 0;

                var sqlGroupBy = @" GROUP BY(product.productId) ";
                sqlGroupBy += sqlHaving;
                sqlCommand += sqlGroupBy;

                if (filter.Count != 0 || !string.IsNullOrEmpty(TextSearch))
                {
                    totalRecord = sqlConnection.Query<Product>(sqlCommand, param: dynamic).Count();
                }
                else
                {
                    totalRecord = sqlConnection.Query<Product>("select * from product", param: dynamic).Count();
                }
                
                sqlpaging = "   ORDER BY  Product.modifiedOfDate DESC  LIMIT @fstRecord , @pageSize";
                sqlCommand = sqlCommand + sqlpaging;

               


                var res = sqlConnection.Query<Product>(sqlCommand, param: dynamic);

                double? qualityStock = 0, giatriton = 0;
                foreach (var item in res)
                {
                    qualityStock += item.QuantityStock is not null ? item.QuantityStock : 0;
                    giatriton += item.ExistentialValue is not null ? item.ExistentialValue: 0;

                }

                var result = new
                {
                    product = res,
                    totalRecord = totalRecord,
                    totalColum = new {
                        QuantityStock = qualityStock,
                        ExistentialValue = giatriton,
                    }
                };
                return result;




            }

        }
        /// <summary>
        /// Lấy mã vật tư hàng hóa mới nhất
        /// </summary>
        /// <returns></returns>
        /// Created by: Thai(10/5/2022)
        public string GetNewProductCode()
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "SELECT CONCAT(c.Prefix, LPAD((c.Value + 1), IF(LENGTH(c.Value + 1) > c.LengthOfValue, LENGTH(c.Value + 1), c.LengthOfValue), '0'), c.Suffix) AS NextValue FROM coderule c WHERE c.RefTypeCategoryCode = 'VTHH'";
                var res = sqlConnection.QueryFirstOrDefault<string>(sqlCommand);
                return res;
            }
        }
        /// <summary>
        /// Thêm mới vật tư hàng hóa
        /// </summary>
        /// <returns></returns>
        /// Created by: Thai(10/5/2022)
        public int InsertProduct(Product product)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string productId = string.Empty;
                int productRowInserted = 0;
                //tạo điểm đầu transaction
                MySqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand("", conn))
                    {
                        cmd.Transaction = transaction;
                        var props = typeof(Product).GetProperties();
                        var listCol = "";
                        var listValue = "";
                        //Lặp qua tất cả các props
                        for (int i = 0; i < props.Length; i++)
                        {
                            var prop = props[i];
                            var notMapProp = prop.GetCustomAttributes(typeof(NotMap), true);
                            //Nếu prop hiện tại cho phép map
                            if (notMapProp.Length <= 0)
                            {
                                var propName = prop.Name;
                                var propValue = prop.GetValue(product);

                                //Nếu là khoá chính => tạo value = newGuid
                                var isPrimaryKey = Attribute.IsDefined(prop, typeof(PrimaryKey));
                                if (isPrimaryKey == true && prop.PropertyType == typeof(Guid))
                                {
                                    propValue = Guid.NewGuid();
                                    productId = propValue.ToString();
                                }
                                var isAutoDate = Attribute.IsDefined(prop, typeof(autoDate));
                                if (isAutoDate == true)
                                    propValue = DateTime.Now;
                                var isUsing = Attribute.IsDefined(prop, typeof(isUsing));
                                if (isUsing == true)
                                    propValue = "1";
                                if (propValue != null)
                                {
                                    listCol += $"{propName},";
                                    listValue += $"@{propName},";
                                    cmd.Parameters.Add(new MySqlParameter($"@{propName}", propValue));
                                }

                            }
                        }
                        listCol = listCol.Substring(0, listCol.Length - 1);
                        listValue = listValue.Substring(0, listValue.Length - 1);
                        var sqlString = $"insert into product({listCol}) values({listValue})";
                        cmd.CommandText = sqlString;
                        productRowInserted = cmd.ExecuteNonQuery();
                    }

                    int productAssRowInserted = 0;
                    List<Guid> listGroupsId;
                    if (product.GroupCategoryListId is not null)
                    {
                        //Lấy danh sách các id nhóm nhà cung cấp
                        listGroupsId = new List<Guid>(product.GroupCategoryListId);
                        string bodyString = "";
                        using (var cmd = new MySqlCommand("", conn))
                        {
                            cmd.Transaction = transaction;
                            var props = typeof(Productgroupcatetgoryassistant).GetProperties();
                            //Lặp qua mảng id, add vào sql string
                            for (int i = 0; i < listGroupsId.Count; i++)
                            {
                                bodyString += $"(@productGroupcatetgoryAssistant{i}, ";
                                cmd.Parameters.AddWithValue($"@productGroupcatetgoryAssistant{i}", Guid.NewGuid().ToString());
                                bodyString += $"@groupCategoryId{i},";
                                cmd.Parameters.AddWithValue($"@groupCategoryId{i}", listGroupsId[i].ToString());
                                bodyString += $"@product{i}),";
                                cmd.Parameters.AddWithValue($"@product{i}", productId);

                            }
                            var sqlCommandString = $"insert into productgroupcatetgoryassistant values {bodyString.Substring(0, bodyString.Length - 1)}";
                            cmd.CommandText = sqlCommandString;
                            productAssRowInserted = cmd.ExecuteNonQuery();
                        }

                    }
                    //commit nếu cả quá trình thành công
                    transaction.Commit();
                    return productAssRowInserted + productRowInserted;
                }
                catch (Exception ex)
                {
                    //rollback lại tất cả nếu gặp exception
                    transaction.Rollback();
                    var res = new
                    {
                        userMsg = MiSa.Web08.Core.Properties.Resource.ExceptionMISA,
                        devMsg = ex.Message
                    };
                    throw new MISAValidateException(res);
                }

            }

            return 0;
        }

        /// <summary>
        /// Lấy thông tin chi tiết vât tư hàng hóa
        /// </summary>
        /// <returns></returns>
        /// Created by: Thai(10/5/2022)
        public object GetProductById(Guid productId)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                sqlConnection.Open();
                DynamicParameters paras = new DynamicParameters();
                paras.Add("@ProdudctId", productId);
                var sqlCommand = @"select * from product p  left join ( select p.productId as v_product_id , GROUP_CONCAT(groupCategoryId,'') as GroupCategoryListId  
                    from product p join (select productgroupcatetgoryassistant.productId, productgroupcatetgoryassistant.groupCategoryId  from productgroupcatetgoryassistant )
                     as vga on p.productId  = vga.productId where p.productId  = @ProdudctId group by p.productId )  as mr on p.productId  = mr.v_product_id WHERE 
                    p.productId = @ProdudctId";

                object res = default;

                using (var cmd = new MySqlCommand(sqlCommand, sqlConnection))
                {
                    cmd.Parameters.AddWithValue("@ProdudctId", productId.ToString());

                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            dynamic entity = new ExpandoObject();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var propName = ToPascalCase(reader.GetName(i));
                                var propValue = reader.GetValue(i);

                                ((IDictionary<string, object>)entity).Add(propName, propValue);
                            }


                            res = JsonConvert.DeserializeObject<object>(JsonConvert.SerializeObject(entity));
                        }
                    }
                }
                sqlConnection.Clone();
                return res;
            }
        }

        /// <summary>
        /// Lấy tất cả vật tư hàng hóa
        /// </summary>
        /// <returns></returns>
        /// Created by: Thai(10/5/2022)
        public IEnumerable<Product> GetAllProduct()
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var tableName = typeof(Product).Name;
                var sqlString = @"SELECT  productCode, productName, taxReduction , GROUP_CONCAT(groupCategoryCode,'') AS groupCategoryCode, insurance, amount, source, describes, quantityStock, existentialValue,explainBuy, explainSell, reduceAccount, warehouseAccount, returnAccount, revenueAccount, expenseAccount, discountAccount, discountRate, fixedPrice, nearestPrice,price, vatTax, importTax, exportTax,supplyExciseTax, Product.Status, typeProduct ,warehouse.warehouseCode,unit.unitName  FROM product left JOIN unit  ON product.unitId = unit.unitId  left JOIN warehouse  ON product.warehouseId = warehouse.warehouseId  LEFT JOIN productgroupcatetgoryassistant ON product.productId = productgroupcatetgoryassistant.productId LEFT JOIN groupcategory ON productgroupcatetgoryassistant.groupCategoryId = groupcategory.groupCategoryId GROUP BY(product.productId)";
                var entities = sqlConnection.Query<Product>(sqlString);
                return entities;
            }
        }
    }
}
