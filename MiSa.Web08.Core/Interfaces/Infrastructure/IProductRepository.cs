using MiSa.Web08.Core;
using MiSa.Web08.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Interfaces.Infrastructure
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        public bool CheckDuplicate(string propName, string propValue, Guid? productId);

        /// <summary>
        /// Lấy vật tư hàng hóa theo paging
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageNumber"></param>
        /// <param name="TextSearch"></param>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        public object GetProductPaging(int PageSize, int PageNumber, string? TextSearch, List<MyFilter> filter);

        /// <summary>
        /// Thêm mới môt vật tư , hàng hóa
        /// </summary>
        /// <returns></returns>
        /// Created by: Thai(10/5/2022)
        public int InsertProduct(Product product);
        /// <summary>
        /// Lẫy mã vật tư hàng hóa mới nhất
        /// </summary>
        /// <returns></returns>
        /// Created by: Thai(10/5/2022)
        public string GetNewProductCode();

        /// <summary>
        /// Lấy mã vật tư hàng hóa theo id
        /// </summary>
        /// <returns></returns>
        /// Created by: Thai(10/5/2022)
        public object GetProductById(Guid productId);

        /// <summary>
        /// Lấy toàn bộ vật tư hàng hóa
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        public IEnumerable<Product> GetAllProduct();
    }
}
