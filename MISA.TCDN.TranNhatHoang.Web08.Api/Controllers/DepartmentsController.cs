using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiSa.Web08.Core;
using MiSa.Web08.Core.Exceptions;
using MiSa.Web08.Core.Interfaces.Infrastructure;
using MiSa.Web08.Core.Interfaces.Service;
using MiSa.Web08.Core.Service;
using MiSa.Web08.Infrastructure.Repository;
using MiSa.Web08.Infrastructure.Respository;
using System.Linq;


namespace MISA.TCDN.TranNhatHoang.Web08.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : MISABaseController<department>
    {

        #region fields
        IDepartmentRepository _departmentRepository;
        IDepartmentService _departmentService;
        #endregion
        #region constructor
        public DepartmentsController(IDepartmentRepository departmentRepository, IDepartmentService departmentService) : base(departmentService, departmentRepository)
        {
            _departmentRepository = departmentRepository;
            _departmentService = departmentService;
        }
        #endregion


        /// <summary>
        /// Lấy toàn bộ danh sách phòng ban
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)

        [HttpGet()]
        public IActionResult GetDepartment()
        {
            try
            {
                var department =_departmentRepository.GetDepartment();
                return StatusCode(200, department);
            }
            catch (Exception ex)
            {
                var mes = new
                {
                    devMsg = ex.Message,
                    userMsg = MiSa.Web08.Core.Properties.Resource.ExceptionMISA
                };
                return StatusCode(500, mes);
            }
        }
        /// <summary>
        /// Lấy toàn bộ thông tin của phòng ban  qua Id
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var department = _departmentRepository.GetDepartmentById(id);
                return StatusCode(200, department);
            }
            catch (Exception ex)
            {
                var mes = new
                {
                    devMsg = ex.Message,
                    userMsg = MiSa.Web08.Core.Properties.Resource.ExceptionMISA
                };
                return StatusCode(500, mes);
            }
        }

    }
}
