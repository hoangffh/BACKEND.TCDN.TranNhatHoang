using MiSa.Web08.Core;
using MiSa.Web08.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Interfaces.Service
{
    public interface IGroupCategoryService : IBaseService<GroupCategory>
    {
        /// <summary>
        /// Xử lý nghiệp vụ thêm record
        /// Create By: Hoang(2/10/2022)
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public int? InsertGroupCategoryService(GroupCategory groupCategory);

        /// <summary>
        /// Xử lý nghiệp vụ sửa dữ liệu
        /// Create By: Hoang(2/10/2022)
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public int UpdateGroupCategoryService(GroupCategory groupCategory , Guid groupCategoryId);


        /// <summary>
        /// Xuất dữ liệu ra file Excel
        /// Create By: Hoang(2/10/2022)
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public Stream ExportExcel();
    }
}
