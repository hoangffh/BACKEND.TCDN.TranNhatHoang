using MiSa.Web08.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Entities
{
    public class Unit
    {
        /// <summary>
        /// Id đơn vị tính
        /// </summary>
        [PrimaryKey]
        public Guid UnitId { get; set; }

        /// <summary>
        /// Tên đơn vị tính
        /// </summary>
        [NotDuplicate]
        [NotEmpty]
        public string UnitName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string? UnitDescribe { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        [isUsing]
        public Status Status { get; set; }

        [NotMap]
        public string? UsingStatus
        {
            get
            {
                switch (Status)
                {
                    case Enum.Status.Using:
                        return Properties.Resource.Enum_Type_Using;
                    case Enum.Status.NotUsing:
                        return Properties.Resource.Enum_Type_NotUsing;
                    default:
                        return null;
                }

            }
        }

        [autoDate]
        public DateTime? createdDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? createdBy { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        [autoDate]
        public DateTime? modifiedOfDate { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>
        public string? modifiedOfBy { get; set; }

    }

}
