using DapperAndRedisCache.Dto;
using DapperAndRedisCache.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperAndRedisCache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmploeeController : ControllerBase
    {
        private readonly EmployeeRepository _employeeRepository;

        public EmploeeController(EmployeeRepository employeeRepository) 
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async  Task<IActionResult> GetAllEmployee()
        {
            var employees = await _employeeRepository.GetAll();
            return Ok(employees);
        }

        [HttpGet("Departments")]
        public async Task<IActionResult> GetAllDepartment() 
        {
            var departments = await _employeeRepository.GetAllDepartment();
            return Ok(departments);
        }
       
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Employee employee) 
        {
            var action = await _employeeRepository.AddEmployee(employee);
            return Created();
        }
       
        [HttpPost("CreateEmployees")]
        public async Task<IActionResult> CreateEmployees(List<Employee> employees) 
        {
            await _employeeRepository.BulkInsertEmployees(employees);
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
