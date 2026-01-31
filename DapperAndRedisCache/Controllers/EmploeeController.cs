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
    }
}
