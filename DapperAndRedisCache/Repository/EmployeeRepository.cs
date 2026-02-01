using Dapper;
using DapperAndRedisCache.Dto;
using System.Data;
using System.Reflection.Metadata;

namespace DapperAndRedisCache.Repository
{

    public class EmployeeRepository
    {
        private readonly DapperContext _context;

        public EmployeeRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            var query = "SELECT e.Id,e.Name,e.Email,d.Name as Department FROM Employees as e inner join Departments as d on e.DepartmentId=d.Id;";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Employee>(query);
        }

        public async Task<IEnumerable<Employee>> GetAllDepartment()
        {
            var query = "SELECT * from Departments";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Employee>(query);
        }

        public async Task<int> AddEmployee(Employee employee)
        {
            var query = "INSERT INTO Employees (Name,Email,DepartmentId) VALUES (@Name,@Email,@DepartmentId)";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(query, new { Name = employee.Name, Email = employee.Email, DepartmentId = employee.Id });
        }

        public async Task BulkInsertEmployees(List<Employee> employees)
        {
            using var connection = _context.CreateConnection();

            var table = new DataTable();
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("DepartmentId", typeof(int));
            table.Columns.Add("Email", typeof(string));

            foreach (var emp in employees)
            {
                table.Rows.Add(emp.Name, emp.Id, emp.Email);
            }

            //DynamicParameters is a Dapper helper class.

//It lets you pass multiple parameters(including complex ones) to SQL.

//Think of it as a flexible parameter bag.

            var param = new DynamicParameters();
            param.Add("@Emp", table.AsTableValuedParameter("dbo.EmployeeTVP"));

            await connection.ExecuteAsync("sp_BulkInsertEmployees", param, commandType: CommandType.StoredProcedure);
    }

        public async Task<IEnumerable<EmployeeRankingDto>> GetEmployeesWithRanking(int mode)
        {
            using var connection = _context.CreateConnection();

            var param = new DynamicParameters();
            param.Add("@Mode", mode);

            return await connection.QueryAsync<EmployeeRankingDto>(
                "sp_GetEmployeesWithDynamicRanking_CTE",
                param,
                commandType: CommandType.StoredProcedure
            );
        }
            
    }
}
