using MiSa.Web08.Core;
using MiSa.Web08.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Interfaces.Infrastructure
{
    public interface IGroupCategoryRepository : IBaseRepository<GroupCategory>
    {
        /// <summary>
        /// Check trùng mã
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)

        public bool CheckDuplicate(string propName, string propValue, Guid? GroupCategoryId);

        /// <summary>
        /// Lấy nhóm vật tư hàng hóa theo paging
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageNumber"></param>
        /// <param name="TextSearch"></param>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        public IEnumerable<GroupCategory> GetGroupCategoriesPaging(int PageSize, int PageNumber, string? TextSearch);


  

    }
}
