using MiSa.Web08.Core;
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
    public class EmployeeService:BaseService<Employee>, IEmployeeService
    {
        #region field
        IEmployeeRepository _employeeRepository;


        IBaseRepository<Employee> _baseRepository;
        List<object> errLstMsgs = new List<object>();

        #endregion

        #region constructor
        public EmployeeService(
            IEmployeeRepository _employeeRepository,
            IBaseRepository<Employee> _baseRepository
        ) : base(_employeeRepository)
        {
            this._employeeRepository = _employeeRepository;
            this._baseRepository = _baseRepository;
        }
        #endregion

        #region method


        public int? InsertEmployeeService(Employee employee)
        {

            //validate trước khi thêm
            ValidateDuplicate(employee, false);
            ValidateObject(employee);
            return _employeeRepository.Insert(employee);
        }
        public int UpdateEmployeeService(Employee employee, Guid employeeId)
        {
            //validate trước khi sửa
            ValidateDuplicate(employee,  true);
            ValidateObject(employee);
            return _employeeRepository.Update(employee, employeeId);
        }
        public int DeleteEmployeeById(Guid employeeId)
        {
            var res = _employeeRepository.Delete(employeeId);

            return res;
        }

        //public int DeleteMultiEmployeeByIds(List<Guid> listEmployeeId)
        //{

        //    var f = _employeeRepository.DeleteMultiEmployeeByIds(listEmployeeId);

        //    return f;

        //}


        //Check trùng lặp
        /// <summary>
        /// Với trường hợp update -> không kiểm tra employeecode cũ -> phải lấy đưuocj mã cũ và select duplicate not employeecode cũ
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="notCheckOldObj">không kiểm tra mã code của object cũ</param>
        public void ValidateDuplicate(Employee employee, bool notCheckOldObj)
        {
            var codeProps = typeof(Employee).GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(NotDuplicate)));
            //biến lưu trạng thái validate: true-trùng
            bool isDuplicate = false;
            //biến lưu tên prop
            var propName = String.Empty;
            string propValue = String.Empty;
            if (codeProps is not null)
            {
                Guid employeeId = Guid.Empty;
                //Nếu là validate cho update
                if (notCheckOldObj)
                {
                    employeeId = employee.EmployeeId;
                }
                foreach (var prop in codeProps)
                {
                    propValue = prop.GetValue(employee).ToString();
                    propName = prop.Name;
                    isDuplicate = _employeeRepository.CheckDuplicate(propName, propValue, employeeId);
                }
            }
            //Nếu bị trùng code => add vào list err
            if (isDuplicate)
            {
                errLstMsgs.Add(new
                {
                    field = propName,
                    mess = "Mã nhân viên <" + propValue + "> đã tồn tại trong hệ thống, vui lòng kiểm tra lại."
                });
            }
        }
        /// <summary>
        /// Hàm validate bên back end
        /// Tràn Nhật Hoàng (10/10/2022)
        /// </summary>
        /// <param name="employee"></param>
        /// <exception cref="MISAValidateException"></exception>
        public void ValidateObject(Employee employee)
        {
            bool isValid = true;

            // Dữ liệu bắt buộc nhập
            var notEmptyProps = employee.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(NotEmpty)));
            if (notEmptyProps is not null)
            {
                foreach (var prop in notEmptyProps)
                {
                    var propValue = prop.GetValue(employee);
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
            var alphabetProps = employee.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(Alphabet)));
            if (alphabetProps is not null)
            {
                var regexAlphabet = new Regex(@"[0-9.-]");
                foreach (var prop in alphabetProps)
                {
                    var propValue = prop.GetValue(employee);
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

            // dữ liệu là Email
            var emailProps = employee.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(Email)));
            if (alphabetProps is not null)
            {
                var regexEmail = new Regex(@"^\w+([.-]?\w+)*@\w+([.-]?\w+)*(.\w{2,3})+$");
                foreach (var prop in emailProps)
                {
                    var propValue = prop.GetValue(employee);
                    //Nếu người dùng nhập Email không hợp lệ => add vào list err
                    if (propValue is not null && !regexEmail.IsMatch(propValue.ToString()))
                    {
                        errLstMsgs.Add(new
                        {
                            field = prop.Name,
                            mess = Properties.Resource.ValidateEmailMess
                        });
                    }
                }
            }


            //Dữ liệu Date không được lớn hơn ngày hiện tại
            var dateProps = employee.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(Date)));
            if (dateProps is not null)
            {
                foreach (var prop in dateProps)
                {
                    var propValue = prop.GetValue(employee);
                    if (!string.IsNullOrEmpty(propValue?.ToString()))
                    {
                        string[] arr = (propValue as String).Split("-");
                        int y = Int32.Parse(arr[0]);
                        int m = Int32.Parse(arr[1]);
                        int d = Int32.Parse(arr[2]);
                        DateTime propDate = new DateTime(y, m, d);
                        DateTime localDate = DateTime.Now;
                        if (propDate > localDate)
                        {
                            errLstMsgs.Add(new
                            {
                                field = prop.Name,
                                mess = Properties.Resource.ValidateDateMess
                            });
                        }

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

        public int DeleteMultiEmployeeFullByIds(List<Guid> listEmployeeId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        /// <returns>Export Excel</returns>
        public Stream ExportExcel()
        {
            //Lấy tất cả dữ liệu
            var data = _employeeRepository.GetEmployees().ToList<Employee>();

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
            workSheet.Column(6).Width = 20;//Chức danh
            workSheet.Column(7).Width = 25;//Tên đơn vị
            workSheet.Column(8).Width = 26;//Số tài khoản
            workSheet.Column(9).Width = 26;//Tên nngân hàng

            //set giá trị từ A1 đến I1
            using (var range = workSheet.Cells["A1:I1"])
            {
                range.Merge = true; // hợp nhất
                range.Value = "Danh Sách Nhân Viên"; //set giá trị
                range.Style.Font.Bold = true;// in đậm
                range.Style.Font.Size = 20;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //căn giữa
            }

            //header
            workSheet.Cells[3, 1].Value = Properties.Resource.Excel_col_1;
            workSheet.Cells[3, 2].Value = Properties.Resource.Excel_col_2;
            workSheet.Cells[3, 3].Value = Properties.Resource.Excel_col_3;
            workSheet.Cells[3, 4].Value = Properties.Resource.Excel_col_4;
            workSheet.Cells[3, 5].Value = Properties.Resource.Excel_col_5;
            workSheet.Cells[3, 6].Value = Properties.Resource.Excel_col_6;
            workSheet.Cells[3, 7].Value = Properties.Resource.Excel_col_7;
            workSheet.Cells[3, 8].Value = Properties.Resource.Excel_col_8;
            workSheet.Cells[3, 9].Value = Properties.Resource.Excel_col_9;

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
                workSheet.Cells[i + 4, 2].Value = data[i].EmployeeCode;
                workSheet.Cells[i + 4, 3].Value = data[i].EmployeeName;
                workSheet.Cells[i + 4, 4].Value = data[i].GenderName;

                string date;
                if (!string.IsNullOrEmpty(data[i].DateOfBirth))
                {
                    date = DateTime.ParseExact(data[i].DateOfBirth.Split(' ')[0], "MM/dd/yyyy", null).ToString("dd/MM/yyyy");
                }
                else
                {
                    date = "";
                }

                workSheet.Cells[i + 4, 5].Value = date;
                workSheet.Cells[i + 4, 6].Value = data[i].PositionName;
                workSheet.Cells[i + 4, 7].Value = data[i].DepartmentName;
                workSheet.Cells[i + 4, 8].Value = data[i].BankAccountNumber;
                workSheet.Cells[i + 4, 9].Value = data[i].BankAccountName;

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
