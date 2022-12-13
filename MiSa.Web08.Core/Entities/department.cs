using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiSa.Web08.Core
{
    public class department
    {
        [PrimaryKey]
        public Guid DepartmentId { get; set; }
        [NotDuplicate]
        [NotEmpty]
        public string DepartmentCode { get; set; }
        [NotEmpty]
        public string DepartmentName { get; set; }
        public DateTime? DepartmentCreatdDate { get; set; }
        public string? DepartmentCreatedName { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string? ModifyBy { get; set; }
    }
}
