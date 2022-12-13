using MiSa.Web08.Core;
using MiSa.Web08.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Interfaces.Service
{
    public interface IProductService : IBaseService<Product>
    {


        

        ///// <summary>
        ///// Xử lý nghiệp vụ sửa dữ liệu
        ///// Create By: Hoang(2/10/2022)
        ///// </summary>
        ///// <param name="employee"></param>
        ///// <param name="employeeId"></param>
        ///// <returns></returns
        public int updateProductservice(Product product , Guid id);

        //// <summary>
        ///// Xử lý nghiệp vụ thêm record
        ///// Create By: Hoang(2/10/2022)
        ///// </summary>
        ///// <param name="employee"></param>
        ///// <returns></returns>
        public int InsertProductService(Product product);
        /// <summary>
        /// Xuất dữ liệu ra file Excel
        /// Create By: Hoang(2/10/2022)
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>

         //// <summary>
        ///// Xử lý nghiệp vụ xuất excel
        ///// Create By: Hoang(2/10/2022)
        ///// </summary>
        ///// <param name="employee"></param>
        ///// <returns></returns>
        public Stream ExportExcel();
    }
}
