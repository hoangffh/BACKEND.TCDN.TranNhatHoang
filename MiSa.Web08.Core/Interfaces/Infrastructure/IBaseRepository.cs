using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Interfaces.Infrastructure
{
    public interface IBaseRepository<T>
    {
        /// <summary>
        /// Lấy dữ liệu của 1 class
        /// Create By: Hoang(2/10/2022)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> Get();

        /// <summary>
        /// Lấy dữ liệu 1 class bằng id
        /// Create By: Hoang(2/10/2022)
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public T Get(Guid? entityId);


        /// <summary>
        /// Lấy dữ liệu phân trang
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IEnumerable<object> GetPaging(int PageSize, int PageNumber, string TextSearch);

        /// <summary>
        /// Thêm dữ liệu 1 class
        /// Create By: Hoang(2/10/2022)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert(T entity);

        /// <summary>
        /// Sửa dữ liệu 1 class
        /// Create By: Hoang(2/10/2022)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public int Update(T entity, Guid entityId);

        /// <summary>
        /// Xóa dữ liệu 1 class
        /// Create By: Hoang(2/10/2022)
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public int Delete(Guid entityId);

        /// <summary>
        /// Xóa nhiều dữ liệu theo list id
        /// Create By: Hoang(2/10/2022)
        /// </summary>
        /// <returns></returns>
        public int DeleteMulti(List<Guid> ids);
        /// <summary>
        /// Xóa toàn bộ dữ liệu của 1 class
        /// Create By: Hoang(2/10/2022)
        /// </summary>
        /// <returns></returns>


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
        /// Lấy mã phát sinh
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)

        public IEnumerable<Table> GetReference(Guid? entityId);

        /// <summary>
        /// lấy tổng số lượng tồn và giá trị tồn
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        public double GetTotalQuantityStock();

        public double GetTotalExistentialValue() ;
    }
}
