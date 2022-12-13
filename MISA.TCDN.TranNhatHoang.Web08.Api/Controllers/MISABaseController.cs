using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiSa.Web08.Core;
using MiSa.Web08.Core.Exceptions;
using MiSa.Web08.Core.Interfaces.Infrastructure;
using MiSa.Web08.Core.Service;
using MiSa.Web08.Core.Interfaces.Service;
using System.Linq;

namespace MISA.TCDN.TranNhatHoang.Web08.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MISABaseController<T> : ControllerBase
    {
        #region field
        IBaseService<T> _baseService;
        IBaseRepository<T> _baseRepository;
        #endregion
        #region constructor
        public MISABaseController(IBaseService<T> baseService, IBaseRepository<T> baseRepository)
        {
            _baseService = baseService;
            _baseRepository = baseRepository;
        }
        #endregion
       
    }
}
