using Dapper;
using DapperAndRedisCache.Dto;

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
            var query = "SELECT * from Department";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Employee>(query);
        }
        
        public async Task<IEnumerable<Employee>> GetAllDepartment()
        {
            var query = "SELECT * from Department";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Employee>(query);
        }

        

    }
}
