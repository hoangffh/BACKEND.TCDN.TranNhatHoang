using MiSa.Web08.Core;
using MiSa.Web08.Infrastructure.Repository;
using System;
using MiSa.Web08.Core.Interfaces.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Dapper;
using MySqlConnector;
using System.Collections;
using System.Dynamic;
using Newtonsoft.Json;

namespace MiSa.Web08.Infrastructure.Respository
{
    public class DepartmentRepository : BaseRepository<department> , IDepartmentRepository
    {
        public DepartmentRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public IEnumerable<department> GetDepartment()
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var sqlCommand = "select * from department";


                var Departments = sqlConnection.Query<department>(sqlCommand);
                return Departments;
            }
        }


        public object GetDepartmentById(Guid departmentId)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                DynamicParameters paras = new DynamicParameters();
                paras.Add("@departmentId", departmentId);
                var sqlCommand = "SELECT * FROM department where DepartmentId = @DepartmentId";

                var Department = sqlConnection.QueryFirstOrDefault<department>(sqlCommand, paras);
                return Department;
            }
        }
    }
}
