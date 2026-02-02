using DapperAndRedisCache.Dto;
using DapperAndRedisCache.Repository;
using DapperAndRedisCache.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperAndRedisCache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmploeeController : ControllerBase
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly RedisCacheService _cacheService;

        public EmploeeController(EmployeeRepository employeeRepository,RedisCacheService cacheService) 
        {
            _cacheService = cacheService;
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async  Task<IActionResult> GetAllEmployee()
        {
            var cacheKey = "employees_All";
            var cachedEmployees = await _cacheService.GetAsync<IEnumerable<Employee>>(cacheKey);
            if (cachedEmployees != null)
            {
                return Ok(cachedEmployees);
            }
            var employees = await _employeeRepository.GetAll();

            await _cacheService.SetAsync(cacheKey,employees,TimeSpan.FromMinutes(10));

            return Ok(employees);
        }

        [HttpGet("Departments")]
        public async Task<IActionResult> GetAllDepartment() 
        {
            var cacheKey = "departments_all";

            var cachedDepartments = await _cacheService.GetAsync<IEnumerable<Employee>>(cacheKey);
            if (cachedDepartments != null)
            {
                return Ok(cachedDepartments); // 🚀 From Redis
            }

            var departments = await _employeeRepository.GetAllDepartment();

            await _cacheService.SetAsync(cacheKey, departments, TimeSpan.FromMinutes(30));

            return Ok(departments);
        }
       
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Employee employee) 
        {
            await _employeeRepository.AddEmployee(employee);

            await _cacheService.RemoveAsync("employee_all");
            return Ok("Employee Created");
        }
       
        [HttpPost("CreateEmployees")]
        public async Task<IActionResult> CreateEmployees(List<Employee> employees) 
        {
            await _employeeRepository.BulkInsertEmployees(employees);

            await _cacheService.RemoveAsync("employee_all");
            return Created();
        }

        [HttpGet("ranking/{mode}")]
        public async Task<IActionResult> GetEmployeesWithRanking(int mode)
        {
            var data = await _employeeRepository.GetEmployeesWithRanking(mode);
            return Ok(data);
        }

    }
}
