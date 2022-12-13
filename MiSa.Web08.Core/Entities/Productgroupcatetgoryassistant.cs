using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Entities
{
    public class Productgroupcatetgoryassistant
    {
        /// <summary>
        /// Id bảng trung gian                                   
        /// </summary>
        public Guid ProductGroupcatetgoryAssistant { get; set; }
        /// <summary>
        /// Id vật tư hàng hóa                                    
        /// </summary>
        public Guid? ProductId { get; set; }
        /// <summary>
        /// Id nhóm vật tư hàng hóa                                    
        /// </summary>
        public Guid? GroupCategoryId { get; set; }

    }
}
