using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Interfaces.Infrastructure
{
    public interface IDepartmentRepository:IBaseRepository<department>
    {
        /// <summary>
        /// Lấy toàn bộ phòng ban
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        public IEnumerable<department> GetDepartment();


        /// <summary>
        /// Lấy phòng ban theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        public object GetDepartmentById(Guid id);
    }
}
