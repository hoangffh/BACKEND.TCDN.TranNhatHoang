using MiSa.Web08.Core.Entities;
using MiSa.Web08.Core.Interfaces.Infrastructure;
using MiSa.Web08.Core.Interfaces.Service;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiSa.Web08.Core.Exceptions;
using System.Text.RegularExpressions;

namespace MiSa.Web08.Core.Service
{
    public class ProductService : BaseService<Product> ,IProductService
    {
        #region field
        IProductRepository _productRepository;
        IBaseRepository<Product> _baseRepository;

        IProductGroupcatetgoryAssistantRepository _productGroupcatetgoryAssistantRepository;
        List<object> errLstMsgs = new List<object>();

        #endregion

        #region constructor
        public ProductService(
            IProductRepository _productRepository,
            IBaseRepository<Product> _baseRepository,
            IProductGroupcatetgoryAssistantRepository _productGroupcatetgoryAssistantRepository
        ) : base(_productRepository)
        {
            this._productRepository = _productRepository;
            this._baseRepository = _baseRepository;
            this._productGroupcatetgoryAssistantRepository = _productGroupcatetgoryAssistantRepository;
        }


        public int InsertProductService(Product product)
        {

            //ValidateObject(vendor);
            ValidateDuplicate(product, false);
            ValidateObject(product);
            var res = _productRepository.InsertProduct(product);
            return res;
        }

        public int InsertMultiProductGroupsAssistant(List<Guid>? idProductGroups, Guid id)
        {
            return _productGroupcatetgoryAssistantRepository.InsertMultiVendorGroupsAssistant(idProductGroups, id);
        }

        public int updateProductservice(Product product, Guid productid)
        {
            ValidateDuplicate(product, true);
            ValidateObject(product);
            var f = _productGroupcatetgoryAssistantRepository.DeleteMultiProductGroupAssistantByProductId(productid);
           
            var s = _productRepository.Update(product,productid);
            var l = 0;
            if (s > 0 && product.GroupCategoryListId != null)
            {
                List<Guid> GroupCategoryListId = new List<Guid>(product.GroupCategoryListId);
                l = InsertMultiProductGroupsAssistant(GroupCategoryListId, productid);
            }
            return f + s + l;
        }
        /// <summary>
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        /// <returns>Export Excel</returns>
        /// 

        //Check trùng lặp
        /// <summary>
        /// Với trường hợp update -> không kiểm tra employeecode cũ -> phải lấy đưuocj mã cũ và select duplicate not employeecode cũ
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="notCheckOldObj">không kiểm tra mã code của object cũ</param>
        public void ValidateDuplicate(Product product, bool notCheckOldObj)
        {
            var codeProps = typeof(Product).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(NotDuplicate)));
            //biến lưu trạng thái validate: true-trùng
            bool isDuplicate = false;
            //biến lưu tên prop
            var propName = String.Empty;
            string propValue = String.Empty;
            if (codeProps is not null)
            {
                Guid productId = Guid.Empty;
                //Nếu là validate cho update
                if (notCheckOldObj)
                {
                    productId = product.ProductId;
                }
                foreach (var prop in codeProps)
                {
                    propValue = prop.GetValue(product).ToString();
                    propName = prop.Name;
                    isDuplicate = _productRepository.CheckDuplicate(propName, propValue, productId);
                }
            }
            //Nếu bị trùng code => add vào list err
            if (isDuplicate)
            {
                errLstMsgs.Add(new
                {
                    field = propName,
                    mess = "Mã hàng hóa , vật tư <" + propValue + "> đã tồn tại trong hệ thống, vui lòng kiểm tra lại."
                });
            }
        }
        /// <summary>
        /// Hàm validate bên back end
        /// Tràn Nhật Hoàng (10/10/2022)
        /// </summary>
        /// <param name="employee"></param>
        /// <exception cref="MISAValidateException"></exception>
        public void ValidateObject(Product product)
        {
            bool isValid = true;

            // Dữ liệu bắt buộc nhập
            var notEmptyProps = product.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(NotEmpty)));
            if (notEmptyProps is not null)
            {
                foreach (var prop in notEmptyProps)
                {
                    var propValue = prop.GetValue(product);
                    //Nếu trường này null hoặc bỏ trống => add vào list err
                    if (propValue == null || string.IsNullOrEmpty(propValue.ToString()))
                        errLstMsgs.Add(new
                        {
                            field = prop.Name,
                            mess = Properties.Resource.ValidateNotEmptyMess
                        });
                }
            }


            // Dữ liệu là Alphabe
            var alphabetProps = product.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(Alphabet)));
            if (alphabetProps is not null)
            {
                var regexAlphabet = new Regex(@"[0-9.-]");
                foreach (var prop in alphabetProps)
                {
                    var propValue = prop.GetValue(product);
                    //Nếu người dùng nhập kí tự khác Alphabet => add vào list err
                    if (propValue is not null && regexAlphabet.IsMatch(propValue.ToString()))
                    {
                        errLstMsgs.Add(new
                        {
                            field = prop.Name,
                            mess = $"Trường này chỉ được nhập ký tự Alphabet"
                        });
                    }
                }
            }





            //Kiểm tra nếu có lỗi thì throw danh sách lỗi
            if (errLstMsgs.Count > 0)
            {
                isValid = false;
                var res = new
                {
                    userMsg = Properties.Resource.ValidateErrMsg,
                    errlst = errLstMsgs
                };
                throw new MISAValidateException(res);
            }

        }
        public Stream ExportExcel()
        {
            //Lấy tất cả dữ liệu
            var data = _productRepository.GetAllProduct().ToList<Product>();

            //tạo bộ nhớ stream để đọc file trên RAM
            var stream = new MemoryStream();

            // ExcelPackage.LicenseContext = LicenseContext.Commercial;
            using var package = new ExcelPackage(stream);

            //tên bảng
            var workSheet = package.Workbook.Worksheets.Add(Properties.Resource.NameSheet);

            //căn chỉnh 
            workSheet.Column(1).Width = 5;//STT
            workSheet.Column(2).Width = 30;//Mã nhân viên 
            workSheet.Column(3).Width = 30;//Tên nhân viên
            workSheet.Column(4).Width = 15;//Giới tính
            workSheet.Column(5).Width = 30;//Ngày sinh
            workSheet.Column(6).Width = 15;//Chức danh
            workSheet.Column(7).Width = 25;//Tên đơn vị
            workSheet.Column(8).Width = 26;//Số tài khoản
            workSheet.Column(9).Width = 26;//Tên nngân hàng
            workSheet.Column(10).Width = 15;//Giới tính
            workSheet.Column(11).Width = 20;//Ngày sinh
            workSheet.Column(12).Width = 20;//Chức danh
            workSheet.Column(13).Width = 25;//Tên đơn vị
            workSheet.Column(14).Width = 26;//Số tài khoản
            workSheet.Column(15).Width = 26;//Tên nngân hàng
            workSheet.Column(16).Width = 15;//Mã nhân viên 
            workSheet.Column(17).Width = 30;//Tên nhân viên
            workSheet.Column(18).Width = 15;//Giới tính
            workSheet.Column(19).Width = 20;//Ngày sinh
            workSheet.Column(20).Width = 20;//Chức danh
            workSheet.Column(21).Width = 25;//Tên đơn vị
            workSheet.Column(22).Width = 26;//Số tài khoản
            workSheet.Column(23).Width = 26;//Tên nngân hàng
            workSheet.Column(24).Width = 15;//Giới tính
            workSheet.Column(25).Width = 20;//Ngày sinh
            workSheet.Column(26).Width = 20;//Chức danh
            workSheet.Column(27).Width = 25;//Tên đơn vị
            workSheet.Column(28).Width = 26;//Số tài khoản
            workSheet.Column(29).Width = 26;//Tên nngân hàng
            workSheet.Column(30).Width = 26;//Tên nngân hàng
            workSheet.Column(31).Width = 26;//Tên nngân hàng
            //set giá trị từ A1 đến I1
            using (var range = workSheet.Cells["A1:AE1"])
            {
                range.Merge = true; // hợp nhất
                range.Value = Properties.Resource.Product_list_header; //set giá trị
                range.Style.Font.Bold = true;// in đậm
                range.Style.Font.Size = 20;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //căn giữa
            }

            //header
            workSheet.Cells[3, 1].Value = Properties.Resource.Excel_col_1;
            workSheet.Cells[3, 2].Value = Properties.Resource.Excel_product_col_2;
            workSheet.Cells[3, 3].Value = Properties.Resource.Excel_product_col_3;
            workSheet.Cells[3, 4].Value = Properties.Resource.Excel_product_col_4;
            workSheet.Cells[3, 5].Value = Properties.Resource.Excel_product_col_5;
            workSheet.Cells[3, 6].Value = Properties.Resource.Excel_product_col_6;
            workSheet.Cells[3, 7].Value = Properties.Resource.Excel_product_col_7;
            workSheet.Cells[3, 8].Value = Properties.Resource.Excel_product_col_8;
            workSheet.Cells[3, 9].Value = Properties.Resource.Excel_product_col_9;
            workSheet.Cells[3, 10].Value = Properties.Resource.Excel_product_col_10;
            workSheet.Cells[3, 11].Value = Properties.Resource.Excel_product_col_11;
            workSheet.Cells[3, 12].Value = Properties.Resource.Excel_product_col_12;
            workSheet.Cells[3, 13].Value = Properties.Resource.Excel_product_col_13;
            workSheet.Cells[3, 14].Value = Properties.Resource.Excel_product_col_14;
            workSheet.Cells[3, 15].Value = Properties.Resource.Excel_product_col_15;
            workSheet.Cells[3, 16].Value = Properties.Resource.Excel_product_col_16;
            workSheet.Cells[3, 17].Value = Properties.Resource.Excel_product_col_17;
            workSheet.Cells[3, 18].Value = Properties.Resource.Excel_product_col_18;
            workSheet.Cells[3, 19].Value = Properties.Resource.Excel_product_col_19;
            workSheet.Cells[3, 20].Value = Properties.Resource.Excel_product_col_20;
            workSheet.Cells[3, 21].Value = Properties.Resource.Excel_product_col_21;
            workSheet.Cells[3, 22].Value = Properties.Resource.Excel_product_col_22;
            workSheet.Cells[3, 23].Value = Properties.Resource.Excel_product_col_23;
            workSheet.Cells[3, 24].Value = Properties.Resource.Excel_product_col_24;
            workSheet.Cells[3, 25].Value = Properties.Resource.Excel_product_col_25;
            workSheet.Cells[3, 26].Value = Properties.Resource.Excel_product_col_26;
            workSheet.Cells[3, 27].Value = Properties.Resource.Excel_product_col_27;
            workSheet.Cells[3, 28].Value = Properties.Resource.Excel_product_col_28;
            workSheet.Cells[3, 29].Value = Properties.Resource.Excel_product_col_29;
            workSheet.Cells[3, 30].Value = Properties.Resource.Excel_product_col_30;
            workSheet.Cells[3, 31].Value = Properties.Resource.Excel_product_col_31;

            //style header
            using (var range = workSheet.Cells["A3:AE3"])
            {
                range.Style.Fill.PatternType = ExcelFillStyle.Solid; // phủ kín background
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray); //set nền
                range.Style.Font.Bold = true; // in đậm text
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin); // viền xung quanh
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // căn giữa cell
            }

            // đổ dữ liệu vào
            for (int i = 0; i < data.Count(); i++)
            {


                workSheet.Cells[i + 4, 1].Value = i + 1;
                workSheet.Cells[i + 4, 2].Value = data[i].ProductCode;
                workSheet.Cells[i + 4, 3].Value = data[i].ProductName;
                workSheet.Cells[i + 4, 4].Value = data[i].TaxReductionValue;
                workSheet.Cells[i + 4, 5].Value = data[i].GroupCategoryCode;
                workSheet.Cells[i + 4, 6].Value = data[i].Insurance;
                workSheet.Cells[i + 4, 7].Value = data[i].Amount;
                workSheet.Cells[i + 4, 8].Value = data[i].Source;
                workSheet.Cells[i + 4, 9].Value = data[i].Describes;
                workSheet.Cells[i + 4, 10].Value = data[i].QuantityStock;
                workSheet.Cells[i + 4, 11].Value = data[i].ExistentialValue;
                workSheet.Cells[i + 4, 12].Value = data[i].ExplainBuy;
                workSheet.Cells[i + 4, 13].Value = data[i].ExplainSell;
                workSheet.Cells[i + 4, 14].Value = data[i].ReduceAccount;
                workSheet.Cells[i + 4, 15].Value = data[i].WarehouseAccount;
                workSheet.Cells[i + 4, 16].Value = data[i].ReturnAccount;
                workSheet.Cells[i + 4, 17].Value = data[i].RevenueAccount;
                workSheet.Cells[i + 4, 18].Value = data[i].ExpenseAccount;
                workSheet.Cells[i + 4, 19].Value = data[i].DiscountAccount;
                workSheet.Cells[i + 4, 20].Value = data[i].DiscountRate;
                workSheet.Cells[i + 4, 21].Value = data[i].FixedPrice;
                workSheet.Cells[i + 4, 22].Value = data[i].NearestPrice;
                workSheet.Cells[i + 4, 23].Value = data[i].Price;
                workSheet.Cells[i + 4, 24].Value = data[i].VatTax;
                workSheet.Cells[i + 4, 25].Value = data[i].ImportTax;
                workSheet.Cells[i + 4, 26].Value = data[i].ExportTax;
                workSheet.Cells[i + 4, 27].Value = data[i].SupplyExciseTax;
                workSheet.Cells[i + 4, 28].Value = data[i].UsingStatus;
                workSheet.Cells[i + 4, 29].Value = data[i].TypeProductValue;
                workSheet.Cells[i + 4, 30].Value = data[i].WarehouseCode;
                workSheet.Cells[i + 4, 31].Value = data[i].UnitName;



                //Căn giữa: Số thứ tự, Quản lý đào tạo, Tình trạng làm việc
                workSheet.Cells[i + 4, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // căn giữa cell
                workSheet.Cells[i + 4, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // căn giữa cell


                //viền cho tất cả 9 cột
                using (var range = workSheet.Cells[i + 4, 1, i + 4, 31])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }
            }

            // lưu
            package.Save();
            //reset stream
            stream.Position = 0;
            //trả ra kết quả
            return package.Stream;
        }
        #endregion
    }
}
