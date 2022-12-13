using MiSa.Web08.Core.Interfaces.Infrastructure;
using MiSa.Web08.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Service
{
    public class DepartmentService : BaseService<department> ,IDepartmentService
    {
        #region field
        IDepartmentRepository _departmentRepository;
        IBaseRepository<department> _baseRepository;
        List<object> errLstMsgs = new List<object>();

        #endregion

        #region constructor
        public DepartmentService(
            IDepartmentRepository _departmentRepository,
            IBaseRepository<department> _baseRepository
        ) : base(_departmentRepository)
        {
            this._departmentRepository = _departmentRepository;
            this._baseRepository = _baseRepository;
        }
        #endregion
    }
}
