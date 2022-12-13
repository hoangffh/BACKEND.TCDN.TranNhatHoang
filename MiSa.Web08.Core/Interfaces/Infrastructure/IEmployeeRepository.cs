using MiSa.Web08.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Interfaces.Infrastructure
{
    public interface IEmployeeRepository:IBaseRepository<Employee>
    {
        public bool CheckDuplicate(string propName, string propValue, Guid? employeeId);

        /// <summary>
        /// Lấy mã nhân viên mới
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        public string GetNewEmployeeCode();

        /// <summary>
        /// Lấy toàn bộ nhân viên
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        public IEnumerable<Employee> GetEmployees();

        /// <summary>
        /// Lấy nhân viên theo pagin
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageNumber"></param>
        /// <param name="TextSearch"></param>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        public IEnumerable<Employee> GetEmployeePaging(int PageSize, int PageNumber, string? TextSearch);


        /// <summary>
        /// Lấy nhân viên theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        public object GetEmployeeById(Guid id);

        /// <summary>
        /// Lấy tổng số bản ghi
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        public int GetTotalRecord();

        /// <summary>
        /// Lấy tổng số bản ghi paging
        /// </summary>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        public int GetTotalRecordSearch(string textSearch);
        /// <summary>
        /// Xóa nhân viên theo danh sách
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)

        //public int DeleteMultiEmployeeByIds(List<Guid> listId);
    }
}
