using Dapper;
using Microsoft.Extensions.Configuration;
using MiSa.Web08.Core;
using MiSa.Web08.Core.Interfaces.Infrastructure;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiSa.Web08.Infrastructure.Repository
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// check dublicate nhân viên
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        //public override bool CheckDuplicate(string propName, string propValue, Guid? employeeId)
        //{
        //    using (sqlConnection = new MySqlConnection(connectionString))
        //    {
        //        var tableName = typeof(Employee).Name;

        //        string sqlString = $"Select * from {tableName} where {propName} = @propValue";

        //        if(employeeId != Guid.Empty)
        //        {
        //            string employeeCodeSql = $"Select EmployeeCode from {tableName} where EmployeeId = '{employeeId}'";
        //            var employeeCode = sqlConnection.QueryFirstOrDefault<string>(employeeCodeSql);
        //            sqlString = $"Select * from {tableName} where {propName} = @propValue and EmployeeCode <> '{employeeCode}'";
        //        }

        //        DynamicParameters paras = new DynamicParameters();
        //        paras.Add("@propValue", propValue);

        //        var res = sqlConnection.QueryFirstOrDefault<object>(sqlString, paras);

        //        if (res != null)
        //            return true;
        //        return false;
        //    }
        //}
        /// <summary>
        /// Lấy ra mã nhân viên lớn nhất
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        public string GetNewEmployeeCode()
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "SELECT MAX(EmployeeCode) FROM Employee  where length(EmployeeCode) = (select max(Length(employee.EmployeeCode)) from employee)";
                var res = sqlConnection.QueryFirstOrDefault<string>(sqlCommand);

                string[] temp = res.Split("V");
                long numberCode = Int64.Parse(temp[1]);
                string nextEmployeeCode = numberCode < 9 ? "0" + (numberCode + 1) : numberCode + 1 + "";
                res = temp[0]+"V" + nextEmployeeCode;


                return res;
            }
        }
        /// <summary>
        ///Lây ra toàn bộ thông tin nhân viên
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        public IEnumerable<Employee> GetEmployees()
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "select * from employee INNER JOIN department  WHERE employee.DepartmentId = department.DepartmentId";

                var Employees = sqlConnection.Query<Employee>(sqlCommand);
                return Employees;
            }
        }
        /// <summary>
        /// Phân trang dữ liệu
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        public IEnumerable<Employee> GetEmployeePaging(int PageSize, int PageNumber, string? TextSearch)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var fstRecord = (PageNumber - 1) * PageSize;
                DynamicParameters dynamic = new DynamicParameters();
                dynamic.Add("@textSearch", TextSearch);
                dynamic.Add("@pageSize", PageSize);
                dynamic.Add("@fstRecord", fstRecord);
                //Sql string join 2 bảng bảng lấy dữ liệu
                var sqlCommand = "select *, DepartmentName from Employee  join department  on employee.DepartmentId =department.DepartmentId";

                //Nếu TextSearch tồn tại => add string tìm kiếm
                if (!string.IsNullOrEmpty(TextSearch))
                {
                    sqlCommand = "  " + sqlCommand + $" WHERE EmployeeCode LIKE '%{TextSearch}%' OR EmployeeName LIKE '%{TextSearch}%'   ORDER BY Length(EmployeeCode) DESC,EmployeeCode DESC LIMIT @fstRecord,@pageSize "; 
                    var Employees = sqlConnection.Query<Employee>(sqlCommand, param: dynamic);
                    return Employees;
                }
                //add string paging
                else
                {
                    sqlCommand += " ORDER BY Length(EmployeeCode) DESC,EmployeeCode DESC LIMIT @fstRecord,@pageSize";
                    var Employees = sqlConnection.Query<Employee>(sqlCommand, param: dynamic);
                    return Employees;
                }

            }

        }
        /// <summary>
        ///Lấy thông tin chi tiết nhân viên theo Id
        /// Trần Nhật Hoàng (10/10/2022)
        /// </summary>
        public object GetEmployeeById(Guid employeeId)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                DynamicParameters paras = new DynamicParameters();
                paras.Add("@employeeId", employeeId);
                var sqlCommand = "SELECT * FROM employee inner join department WHERE employee.DepartmentId = department.DepartmentId and employeeId = @EmployeeId";

                var Employee = sqlConnection.QueryFirstOrDefault<Employee>(sqlCommand, paras);
                return Employee;
            }
        }
      

        //public int DeleteMultiEmployeeByIds(List<Guid> listId)
        //{
        //    using (sqlConnection = new MySqlConnection(connectionString))
        //    {
        //        using (var transaction = sqlConnection.BeginTransaction())
        //        {
        //            var tableName = typeof(Employee).Name;
        //            //Khai báo chuỗi làm điều kiện where
        //            var conditionString = string.Empty;
        //            var length = listId.Count;
        //            //contro 1
        //            DynamicParameters paras = new DynamicParameters();
        //            //Loop danh sách id
        //            for (int i = 0; i < length; i++)
        //            {
        //                //add id cần xóa vào chuỗi làm điều kiện where
        //                if (i == length - 1)
        //                    conditionString += $"EmployeeId=@item{i}";
        //                else
        //                    conditionString += $"EmployeeId=@item{i} OR ";

        //                paras.Add($"@item{i}", listId[i]);
        //            }
        //            string sqlString = $"delete from {tableName} where {conditionString}";
        //            var res = sqlConnection.Execute(sqlString, paras);

        //            if (res == length)
        //                transaction.Commit();
        //            else
        //                transaction.Rollback();
        //            return res;


        //        }

        //    }
        //}
    }
}
