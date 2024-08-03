using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Interfaces;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("GetEmployees")]
 
        public async Task<IActionResult> GetEmployees([FromQuery] string? databaseName, [FromQuery] string? pattern)
        {
            var employees = await _employeeService.GetEmployeesAsync(databaseName, pattern);
            return Ok(employees);
        }
    }
}
