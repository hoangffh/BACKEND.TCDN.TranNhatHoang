using MiSa.Web08.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Entities
{
    public class Warehouse
    {
        /// <summary>
        /// Id kho
        /// </summary>
        [PrimaryKey]
        public Guid WarehouseId { get; set; }


        /// <summary>
        /// Mã kho
        /// </summary>
        [NotDuplicate]
        [NotEmpty]
        public string WarehouseCode { get; set; }


        /// <summary>
        /// Tên kho
        /// </summary>
        [NotEmpty]
        public string WarehouseName { get; set; }

        /// <summary>
        /// Đại chỉ
        /// </summary>
        public string? WarehousePlace { get; set; }



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

        /// <summary>
        /// Nhánh kho
        /// </summary>
        public string? WarehouseBranch { get; set; }


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
