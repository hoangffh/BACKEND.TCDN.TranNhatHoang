using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Interfaces.Service
{
    public interface IBaseService<T>
    {
        /// <summary>
        /// Xử lý nghiệp vụ thêm mới dữ liệu
        /// </summary>
        /// <param entity="entity></param>
        /// <param id="id></param>
        /// <returns>Số bản ghi thêm mới thành công</returns>
        /// Create By: Hoang(2/10/2022)
        public int? InsertService(T customerGroup);


        /// <summary>
        /// Xử lý nghiệp vụ sửa dữ liệu
        /// </summary>
        /// <param entity="entity></param>
        /// <param id="id></param>
        /// <returns>Số bản ghi thêm mới thành công</returns>
        /// Create By: Hoang(2/10/2022)
        public int? UpdateService(T entity, Guid entityId);
    }
}
