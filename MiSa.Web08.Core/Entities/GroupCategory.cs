using MiSa.Web08.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Entities
{
    public class GroupCategory
    {
        /// <summary>
        /// Id nhóm vật tư hàng hóa                                    
        /// </summary>
        [PrimaryKey]
        public Guid GroupCategoryId { get; set; }


        /// <summary>
        /// Mã nhóm vật tư hàng hóa
        /// </summary>
        [NotDuplicate]
        [NotEmpty]
        public string GroupCategoryCode { get; set; }


        /// <summary>
        /// Tên nhóm vật  tư , hàng hóa
        /// </summary>
        [NotEmpty]
        public string GroupCategoryName { get; set; }


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


        /// <summary>
        /// Id của the cha
        /// </summary>
        public Guid? ParentId { get; set; }

    }

}
