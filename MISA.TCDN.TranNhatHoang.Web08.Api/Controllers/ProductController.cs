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
    public class ProductController : MISABaseController<Product>
    {
        #region fields
        IProductRepository _productRepository;
        IProductService _productService;
        #endregion
        #region constructor
        public ProductController(IProductRepository productRepository, IProductService productService) : base(productService, productRepository)
        {
            _productRepository = productRepository;
            _productService = productService;
        }
        #endregion

        #region method



        /// <summary>
        /// Lấy mã vật tưu hàng hóa mới
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)

        [HttpGet("newcode")]
        public IActionResult GetNewProductCode()
        {
            try
            {
                var res = _productRepository.GetNewProductCode();
                return Ok(res);
            }
            catch (Exception ex)
            {
                var mes = new
                {
                    devMsg = ex.Message,
                    userMsg = MiSa.Web08.Core.Properties.Resource.ValidateErrMsg
                };
                return StatusCode(500, mes);
            }
        }


        /// <summary>
        /// Lấy toàn bộ thông tin của vật tư hàng hóa  theo trang hoặc tìm kiếm
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        [HttpPost("paging")]
        public IActionResult GetProductPaging(int pageSize, int pageNumber, string? textSearch, List<MyFilter> filter)
        {
            try
            {
                object res = _productRepository.GetProductPaging(pageSize, pageNumber, textSearch, filter);

                

                return Ok(new
                {
                    res,
                    pageSize,
                    pageNumber,
               
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
        /// Lấy toàn bộ thông tin của danh sách nhân viên theo trang hoặc tìm kiếm
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        [HttpGet("sum")]
        public IActionResult GetProductSum()
        {
            try
            {
               
                object totalQuantityStock, totalExistentialValue;
                totalQuantityStock = _productRepository.GetTotalQuantityStock();
                totalExistentialValue = _productRepository.GetTotalExistentialValue();

                return Ok(new
                {
                    totalQuantityStock,
                    totalExistentialValue
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
        /// Lấy toàn bộ thông tin vật tư hàng hóa  qua Id
        /// </summary>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var warehouse = _productRepository.GetProductById(id);
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

        // <summary>
        // Thêm 1  vật tư , hàng hóa
        // </summary>
        // <returns></returns>
        // Create By: Hoang(2/10/2022)
        [HttpPost("Insert")]
        public IActionResult Post(Product product)
        {
            try
            {
                var res = _productService.InsertProductService(product);
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
        /// Sửu một vật tư , hàng hóa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(Product product, Guid id)
        {
            try
            {
                var res = _productService.updateProductservice(product, id);
                if (res >= 1)
                {
                    return StatusCode(200, res);
                }
                return Ok(res);
            }
            catch(MISAValidateException ex)
            {
                return StatusCode(400, ex.Data);
            }
            catch (Exception ex)
            {
                var mess = new
                {
                    devMsg = ex.Message,
                    userMsg = MiSa.Web08.Core.Properties.Resource.ExceptionMISA
                };
                return StatusCode(500, mess);
            }
        }


        /// <summary>
        /// Xóa 1 vật tư , hàng hóa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        [HttpDelete("{id}")]
        public IActionResult DeleteUnit(Guid id)
        {
            try
            {
                var res = _productRepository.Delete(id);
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
        /// Xóa nhiều vật tư , hàng hóa
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        /// Create By: Hoang(2/10/2022)
        [HttpPut("multi")]
        public IActionResult DeleteMultiEmployee(List<Guid> listId)
        {
            try
            {
                var res = _productRepository.DeleteMulti(listId);
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
            var stream = _productService.ExportExcel();
            string fileName = $"{MiSa.Web08.Core.Properties.Resource.File_name_excel_product}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

        }

        #endregion
    }
}
