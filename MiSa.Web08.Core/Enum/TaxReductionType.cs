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
    public enum TaxReduction
    {
        /// <summary>
        /// Chưa xác định
        /// </summary>
        NotDetermined = 1,

        /// <summary>
        /// Không giảm thuế
        /// </summary>
        NotReduction = 2,

        /// <summary>
        /// Giảm thuế
        /// </summary>
        Reduction = 3,

    }
}
