using MiSa.Web08.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Interfaces.Service
{
    public interface IEmployeeService:IBaseService<Employee>
    {
        /// <summary>
        /// Xử lý nghiệp vụ thêm record
        /// Create By: Hoang(2/10/2022)
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public int? InsertEmployeeService(Employee employee);

        /// <summary>
        /// Xử lý nghiệp vụ sửa dữ liệu
        /// Create By: Hoang(2/10/2022)
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public int UpdateEmployeeService(Employee employee, Guid employeeId);

        /// <summary>
        /// Xử lý nghiệp vụ xóa record qua Id
        /// Create By: Hoang(2/10/2022)
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public int DeleteEmployeeById(Guid employeeId);

        /// <summary>
        /// Xử lý nghiệp vụ xóa nhiều record qua nhiều Id
        /// Create By: Hoang(2/10/2022)
        /// </summary>
        /// <param name="listEmployeeId"></param>
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
