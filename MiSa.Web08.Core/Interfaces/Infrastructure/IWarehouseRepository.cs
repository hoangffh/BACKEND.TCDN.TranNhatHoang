using MiSa.Web08.Core;
using MiSa.Web08.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Interfaces.Infrastructure
{
    public interface IWarehouseRepository : IBaseRepository<Warehouse>
    {
        public bool CheckDuplicate(string propName, string propValue, Guid? warehouseId);

        /// <summary>
        /// Lấy kho theo paging
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageNumber"></param>
        /// <param name="TextSearch"></param>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        public IEnumerable<Warehouse> GetWarehousePaging(int PageSize, int PageNumber, string? TextSearch);



    }
}
