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
    public class GroupCategoryService : BaseService<GroupCategory>, IGroupCategoryService
    {
        #region field
       IGroupCategoryRepository _GroupCategoryRepository;


        IBaseRepository<GroupCategory> _baseRepository;
        List<object> errLstMsgs = new List<object>();

        #endregion

        #region constructor
        public GroupCategoryService(
             IGroupCategoryRepository _GroupCategoryRepository,
        IBaseRepository<GroupCategory> _baseRepository
        ) : base(_GroupCategoryRepository)
        {
            this._GroupCategoryRepository = _GroupCategoryRepository;
            this._baseRepository = _baseRepository;
        }
        #endregion

        #region method


        public int? InsertGroupCategoryService(GroupCategory groupCategory)
        {
            //validate trước khi thêm
            ValidateDuplicate(groupCategory, false);
            ValidateObject(groupCategory);
            return _GroupCategoryRepository.Insert(groupCategory);
        }
        public int UpdateGroupCategoryService(GroupCategory groupCategory, Guid groupCategoryId)
        {
            //validate trước khi sửa
            ValidateDuplicate(groupCategory,  true);
            ValidateObject(groupCategory);
            return _GroupCategoryRepository.Update(groupCategory, groupCategoryId);
        }

        //Check trùng lặp
        /// <summary>
        /// Với trường hợp update -> không kiểm tra employeecode cũ -> phải lấy đưuocj mã cũ và select duplicate not employeecode cũ
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="notCheckOldObj">không kiểm tra mã code của object cũ</param>
        public void ValidateDuplicate(GroupCategory groupCategory, bool notCheckOldObj)
        {
            var codeProps = typeof(GroupCategory).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(NotDuplicate)));
            //biến lưu trạng thái validate: true-trùng
            bool isDuplicate = false;
            //biến lưu tên prop
            var propName = String.Empty;
            string propValue = String.Empty;
            if (codeProps is not null)
            {
                Guid groupCategoryId = Guid.Empty;
                //Nếu là validate cho update
                if (notCheckOldObj)
                {
                    groupCategoryId = groupCategory.GroupCategoryId;
                }
                foreach (var prop in codeProps)
                {
                    propValue = prop.GetValue(groupCategory).ToString();
                    propName = prop.Name;
                    isDuplicate = _GroupCategoryRepository.CheckDuplicate(propName, propValue, groupCategoryId);
                }
            }
            //Nếu bị trùng code => add vào list err
            if (isDuplicate)
            {
                errLstMsgs.Add(new
                {
                    field = propName,
                    mess = "Mã nhóm vật tư , hàng hóa <" + propValue + "> đã tồn tại trong hệ thống, vui lòng kiểm tra lại."
                });
            }
        }
        /// <summary>
        /// Hàm validate bên back end
        /// Tràn Nhật Hoàng (10/10/2022)
        /// </summary>
        /// <param name="employee"></param>
        /// <exception cref="MISAValidateException"></exception>
        public void ValidateObject(GroupCategory groupCategory)
        {
            bool isValid = true;

            // Dữ liệu bắt buộc nhập
            var notEmptyProps = groupCategory.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(NotEmpty)));
            if (notEmptyProps is not null)
            {
                foreach (var prop in notEmptyProps)
                {
                    var propValue = prop.GetValue(groupCategory);
                    //Nếu trường này null hoặc bỏ trống => add vào list err
                    if (propValue == null || string.IsNullOrEmpty(propValue.ToString()))
                        errLstMsgs.Add(new
                        {
                            field = prop.Name,
                            mess = Properties.Resource.ValidateNotEmptyMess
                        });
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
            var data = _GroupCategoryRepository.Get().ToList<GroupCategory>();

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


            //set giá trị từ A1 đến I1
            using (var range = workSheet.Cells["A1:D1"])
            {
                range.Merge = true; // hợp nhất
                range.Value = "Danh Sách Nhóm vật tư hàng hóa"; //set giá trị
                range.Style.Font.Bold = true;// in đậm
                range.Style.Font.Size = 20;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //căn giữa
            }

            //header
            workSheet.Cells[3, 1].Value = Properties.Resource.Excel_col_1;
            workSheet.Cells[3, 2].Value = Properties.Resource.Excel_groupCategory_col_2;
            workSheet.Cells[3, 3].Value = Properties.Resource.Excel_groupCategory_col_3;
            workSheet.Cells[3, 4].Value = Properties.Resource.Excel_groupCategory_col_Status;

            //style header
            using (var range = workSheet.Cells["A3:D3"])
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
                workSheet.Cells[i + 4, 2].Value = data[i].GroupCategoryCode;
                workSheet.Cells[i + 4, 3].Value = data[i].GroupCategoryName;
                workSheet.Cells[i + 4, 4].Value = data[i].UsingStatus;

               

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
