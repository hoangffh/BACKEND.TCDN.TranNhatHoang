using MiSa.Web08.Core;
using MiSa.Web08.Core.Entities;
using MiSa.Web08.Core.Exceptions;
using MiSa.Web08.Core.Interfaces.Infrastructure;
using MiSa.Web08.Core.Interfaces.Service;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MiSa.Web08.Core.Service
{
    public class WarehouseService : BaseService<Warehouse>, IWarehouseService
    {
        #region field
        IWarehouseRepository _warehouseRepository;


        IBaseRepository<Warehouse> _baseRepository;
        List<object> errLstMsgs = new List<object>();

        #endregion

        #region constructor
        public WarehouseService(
             IWarehouseRepository _warehouseRepository,
        IBaseRepository<Warehouse> _baseRepository
        ) : base(_warehouseRepository)
        {
            this._warehouseRepository = _warehouseRepository;
            this._baseRepository = _baseRepository;
        }
        #endregion

        #region method


        public int? InsertWarehouseService(Warehouse warehouse)
        {

            //validate trước khi thêm
            ValidateDuplicate(warehouse, false);
            ValidateObject(warehouse);
            return _warehouseRepository.Insert(warehouse);
        }
        public int UpdateWarehouseService(Warehouse warehouse, Guid  warehouseId)
        {
            //validate trước khi sửa
            ValidateDuplicate(warehouse,  true);
            ValidateObject(warehouse);
            return _warehouseRepository.Update(warehouse, warehouseId);
        }
        public int DeleteWarehouseById(Guid warehouseId)
        {
            var res = _warehouseRepository.Delete(warehouseId);

            return res;
        }

        //Check trùng lặp
        /// <summary>
        /// Với trường hợp update -> không kiểm tra employeecode cũ -> phải lấy đưuocj mã cũ và select duplicate not employeecode cũ
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="notCheckOldObj">không kiểm tra mã code của object cũ</param>
        public void ValidateDuplicate(Warehouse warehouse, bool notCheckOldObj)
        {
            var codeProps = typeof(Warehouse).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(NotDuplicate)));
            //biến lưu trạng thái validate: true-trùng
            bool isDuplicate = false;
            //biến lưu tên prop
            var propName = String.Empty;
            string propValue = String.Empty;
            if (codeProps is not null)
            {
                Guid warehouseId = Guid.Empty;
                //Nếu là validate cho update
                if (notCheckOldObj)
                {
                    warehouseId = warehouse.WarehouseId;
                }
                foreach (var prop in codeProps)
                {
                    propValue = prop.GetValue(warehouse).ToString();
                    propName = prop.Name;
                    isDuplicate = _warehouseRepository.CheckDuplicate(propName, propValue, warehouseId);
                }
            }
            //Nếu bị trùng code => add vào list err
            if (isDuplicate)
            {
                errLstMsgs.Add(new
                {
                    field = propName,
                    mess = "Mã kho <" + propValue + "> đã tồn tại trong hệ thống, vui lòng kiểm tra lại."
                });
            }
        }
        /// <summary>
        /// Hàm validate bên back end
        /// Tràn Nhật Hoàng (10/10/2022)
        /// </summary>
        /// <param name="employee"></param>
        /// <exception cref="MISAValidateException"></exception>
        public void ValidateObject(Warehouse warehouse)
        {
            bool isValid = true;

            // Dữ liệu bắt buộc nhập
            var notEmptyProps = warehouse.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(NotEmpty)));
            if (notEmptyProps is not null)
            {
                foreach (var prop in notEmptyProps)
                {
                    var propValue = prop.GetValue(warehouse);
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
            var alphabetProps = warehouse.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(Alphabet)));
            if (alphabetProps is not null)
            {
                var regexAlphabet = new Regex(@"[0-9.-]");
                foreach (var prop in alphabetProps)
                {
                    var propValue = prop.GetValue(warehouse);
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

       

        /// <summary>
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        /// <returns>Export Excel</returns>
        public Stream ExportExcel()
        {
            //Lấy tất cả dữ liệu
            var data = _warehouseRepository.Get().ToList<Warehouse>();

            //tạo bộ nhớ stream để đọc file trên RAM
            var stream = new MemoryStream();

            // ExcelPackage.LicenseContext = LicenseContext.Commercial;
            using var package = new ExcelPackage(stream);

            //tên bảng
            var workSheet = package.Workbook.Worksheets.Add(Properties.Resource.NameSheet);

            //căn chỉnh 
            workSheet.Column(1).Width = 5;//STT
            workSheet.Column(2).Width = 15;//Mã nhân viên 
            workSheet.Column(3).Width = 30;//Tên nhân viên
            workSheet.Column(4).Width = 15;//Giới tính
            workSheet.Column(5).Width = 20;//Ngày sinh
 

            //set giá trị từ A1 đến I1
            using (var range = workSheet.Cells["A1:I1"])
            {
                range.Merge = true; // hợp nhất
                range.Value = "Danh Sách Kho"; //set giá trị
                range.Style.Font.Bold = true;// in đậm
                range.Style.Font.Size = 20;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //căn giữa
            }

            //header
            workSheet.Cells[3, 1].Value = Properties.Resource.Excel_col_1;
            workSheet.Cells[3, 2].Value = Properties.Resource.Excel_warehouse_col_2;
            workSheet.Cells[3, 3].Value = Properties.Resource.Excel_warehouse_col_3;
            workSheet.Cells[3, 4].Value = Properties.Resource.Excel_warehouse_col_4;
            workSheet.Cells[3, 5].Value = Properties.Resource.Excel_groupCategory_col_Status;


            //style header
            using (var range = workSheet.Cells["A3:I3"])
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
                workSheet.Cells[i + 4, 2].Value = data[i].WarehouseCode;
                workSheet.Cells[i + 4, 3].Value = data[i].WarehouseName;
                workSheet.Cells[i + 4, 4].Value = data[i].WarehousePlace;
                workSheet.Cells[i + 4, 5].Value = data[i].UsingStatus;

               

                //Căn giữa: Số thứ tự, Quản lý đào tạo, Tình trạng làm việc
                workSheet.Cells[i + 4, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // căn giữa cell
                workSheet.Cells[i + 4, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // căn giữa cell


                //viền cho tất cả 9 cột
                using (var range = workSheet.Cells[i + 4, 1, i + 4, 9])
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
