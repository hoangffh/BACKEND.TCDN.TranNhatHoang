using MiSa.Web08.Core;
using MiSa.Web08.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Interfaces.Infrastructure
{
    public interface IUnitRepository : IBaseRepository<Unit>
    {
        public bool CheckDuplicate(string propName, string propValue, Guid? unitId);

      

        /// <summary>
        /// Lấy đơn vị theo paging
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageNumber"></param>
        /// <param name="TextSearch"></param>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        public IEnumerable<Unit> GetUnitPaging(int PageSize, int PageNumber, string? TextSearch);


        /// <summary>
        /// Lấy tổng số bản ghi
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
       
        public int GetTotalRecordSearch(string textSearch);
        /// <summary>
        /// Xóa nhân viên theo danh sách
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)

    }
}
