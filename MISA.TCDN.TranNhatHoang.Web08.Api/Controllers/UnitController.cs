using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiSa.Web08.Core;
using MiSa.Web08.Core.Entities;
using MiSa.Web08.Core.Exceptions;
using MiSa.Web08.Core.Interfaces.Infrastructure;
using MiSa.Web08.Core.Interfaces.Service;
using MiSa.Web08.Core.Service;
using System.Linq;

namespace MISA.TCDN.TranNhatHoang.Web08.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UnitController : MISABaseController<Unit>
    {
        #region fields
        IUnitRepository _unitRepository;
        IUnitService _unitService;
        #endregion
        #region constructor
        public UnitController(IUnitRepository unitRepository, IUnitService unitService) : base(unitService, unitRepository)
        {
            _unitRepository = unitRepository;
            _unitService = unitService;
        }
        #endregion

        #region method

        

        /// <summary>
        /// Lấy toàn bộ danh sách đơn vị tính
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)

        [HttpGet()]
        public IActionResult GetUnit()
        {
            try
            {
                var employees = _unitRepository.Get();
                return StatusCode(200, employees);
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
        /// Lấy toàn bộ thông tin của danh sách đơn vị tính  theo trang hoặc tìm kiếm
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        [HttpGet("paging")]
        public IActionResult GetUnitPaging(int pageSize, int pageNumber, string? textSearch)
        {
            try
            {
                var warehouses = _unitRepository.GetUnitPaging(pageSize, pageNumber, textSearch);
                object total;
                if (!string.IsNullOrEmpty(textSearch))
                {
                    total = _unitRepository.GetTotalRecordSearch(textSearch);
                } else
                {
                    total = _unitRepository.GetTotalRecord();

                }
                return Ok(new
                {
                    total,
                    pageSize,
                    pageNumber,
                    warehouses
                });
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
        /// Lấy toàn bộ thông tin của đơn vị tính  qua Id
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var warehouse = _unitRepository.Get(id);
                return StatusCode(200, warehouse);
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
        /// Kiểm tra khóa ngoài 
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        [HttpGet]
        [Route("referenceId")]
        public IActionResult GetReferenceById(Guid referenceId)
        {
            try
            {
                var warehouse = _unitRepository.GetReference(referenceId);
                return StatusCode(200, warehouse);
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
        /// Thêm đơn vị tính 
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        [HttpPost]
        public IActionResult Post(Unit unit)
        {
            try
            {
                var res = _unitService.InsertUnitService(unit);
                if (res > 0)
                {
                    return StatusCode(201, res);
                }
                return Ok(res);
            }
            catch (MISAValidateException ex)
            {
                return StatusCode(400, ex.Data);
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
        /// Sửa đơn vị tính
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        [HttpPut("{id}")]
        public IActionResult Update(Unit unit, Guid id)
        {
            try
            {
                var res = _unitService.UpdateUnitService(unit,id);
                return StatusCode(200, res);
            }
            catch (MISAValidateException ex)
            {
                return StatusCode(400, ex.Data);
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
        /// Xóa 1 đơn vị tính
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        [HttpDelete("{id}")]
        public IActionResult DeleteUnit(Guid id)
        {
            try
            {
                var res = _unitRepository.Delete(id);
                return StatusCode(200, res);
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
        /// Xuất dữ liệu ra Excel
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)

        [HttpGet("export")]
        public IActionResult Export()
        {
            var stream = _unitService.ExportExcel();
            string fileName = $"{MiSa.Web08.Core.Properties.Resource.UnitList}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

        }

        #endregion
    }
}
