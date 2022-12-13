using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiSa.Web08.Core;
using MiSa.Web08.Core.Entities;
using MiSa.Web08.Core.Exceptions;
using MiSa.Web08.Core.Interfaces.Infrastructure;
using MiSa.Web08.Core.Interfaces.Service;
using MiSa.Web08.Core.Service;
using MiSa.Web08.Infrastructure.Repository;
using System.Linq;

namespace MISA.TCDN.TranNhatHoang.Web08.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WarehouseController : MISABaseController<Warehouse>
    {
        #region fields
        IWarehouseRepository _warehouseRepository;
        IWarehouseService _warehouseService;
        #endregion
        #region constructor
        public WarehouseController(IWarehouseRepository warehouseRepository, IWarehouseService warehouseService) : base(warehouseService, warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
            _warehouseService = warehouseService;
        }
        #endregion

        #region method

        

        /// <summary>
        /// Lấy toàn bộ danh sách kho
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)

        [HttpGet()]
        public IActionResult GetEmployees()
        {
            try
            {
                var warehouses = _warehouseRepository.Get();
                return StatusCode(200, warehouses);
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
        /// Lấy toàn bộ thông tin của danh sách kho theo trang hoặc tìm kiếm
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        [HttpGet("paging")]
        public IActionResult GetWarehousePaging(int pageSize, int pageNumber, string? textSearch)
        {
            try
            {
                var warehouses = _warehouseRepository.GetWarehousePaging(pageSize, pageNumber, textSearch);
                object total;
                if (!string.IsNullOrEmpty(textSearch))
                {
                    total = _warehouseRepository.GetTotalRecordSearch(textSearch);
                } else
                {
                    total = _warehouseRepository.GetTotalRecord();

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
        /// Lấy toàn bộ thông tin kho qua Id
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var warehouse = _warehouseRepository.Get(id);
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
                var warehouse = _warehouseRepository.GetReference(referenceId);
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
        /// Thêm mưới một kho
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        [HttpPost]
        public IActionResult Post(Warehouse warehouse)
        {
            try
            {
                var res = _warehouseService.InsertWarehouseService(warehouse);
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
        /// Sửa thôn tin kho
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        [HttpPut("{id}")]
        public IActionResult Update(Warehouse warehouse, Guid id)
        {
            try
            {
                var res = _warehouseService.UpdateWarehouseService(warehouse, id);
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
        /// Xóa 1 kho
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            try
            {
                var res =_warehouseRepository.Delete(id);
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
            var stream = _warehouseService.ExportExcel();
            string fileName = $"{MiSa.Web08.Core.Properties.Resource.WarehouseList}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

        }

        #endregion
    }
}
