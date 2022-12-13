using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Entities {

    public class MyFilter
    {
        /// <summary>
        /// Key lọc                                    
        /// </summary>
        public string Key { get;set;}

        /// <summary>
        /// Giá trị lọc                                    
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// lọại lọc                                    
        /// </summary>
        public string TypeOfFilter { get; set; }
        /// <summary>
        /// Lcc theo giá trị : number hoặc string                                    
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// Tên cột  lọc                                   
        /// </summary>
        public string? TableKey { get; set; }
    }
}
