using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Enum
{
    /// <summary>
    /// Giới tính
    /// Create By: Hoang(2/10/2022)
    /// </summary>
    public enum TypeProduct
    {
        /// <summary>
        /// Hàng hóa
        /// </summary>
        Goods = 1,

        /// <summary>
        /// Dịch vụ
        /// </summary>
        Service = 2,


        /// <summary>
        /// Nguyên vật liệu
        /// </summary>
        RawMaterial = 3,


        /// <summary>
        /// Thành phẩm
        /// </summary>
        FinishedProduct = 4,

        /// <summary>
        /// Công cụ dụng cụ
        /// </summary>
        Tool = 5,

    }
}
