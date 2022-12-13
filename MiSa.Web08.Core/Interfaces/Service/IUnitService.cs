using MiSa.Web08.Core;
using MiSa.Web08.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Interfaces.Service
{
    public interface IUnitService : IBaseService<Unit>
    {
        /// <summary>
        /// Xử lý nghiệp vụ thêm record
        /// Create By: Hoang(2/10/2022)
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public int? InsertUnitService(Unit unit);

        /// <summary>
        /// Xử lý nghiệp vụ sửa dữ liệu
        /// Create By: Hoang(2/10/2022)
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public int UpdateUnitService(Unit unit, Guid unitId);

        /// <summary>
        /// Xử lý nghiệp vụ xóa record qua Id
        /// Create By: Hoang(2/10/2022)
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>



        /// <summary>
        /// Xuất dữ liệu ra file Excel
        /// Create By: Hoang(2/10/2022)
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public Stream ExportExcel();
    }
}
