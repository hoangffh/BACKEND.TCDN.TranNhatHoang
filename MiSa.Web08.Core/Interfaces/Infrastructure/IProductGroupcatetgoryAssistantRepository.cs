using MiSa.Web08.Core;
using MiSa.Web08.Core.Entities;
using MiSa.Web08.Core.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Interfaces.Infrastructure
{
    public interface IProductGroupcatetgoryAssistantRepository:IBaseRepository<Productgroupcatetgoryassistant>
    {


        /// <summary>
        /// Thêm dữ liệu
        /// Create By: Hoang(2/10/2022)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertMultiVendorGroupsAssistant(List<Guid> listIds, Guid productId);

        /// <summary>
        /// Xóa theo id
        /// Create By: Hoang(2/10/2022)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int DeleteMultiProductGroupAssistantByProductId(Guid productId);
    }
}
