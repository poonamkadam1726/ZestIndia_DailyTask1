using Day2_EntityFrameworkCore.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//CRUD Operation Web API with Entity Framework Core-One to Many Relationship

namespace Day2_EntityFrameworkCore.Controllers
{
    //[EnableCors("MyCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        // This allows us to use the database context in our controller actions
        private readonly MyDbContext myDbContext;

        // Constructor injection of MyDbContext
        public EmployeeController(MyDbContext myDbContext)
        {
            this.myDbContext = myDbContext;
        }

        [HttpPost]
        public IActionResult EmployeeRegistration(EmployeeDTO employeeDTO)
        {
            // Find the department by name
            var department = myDbContext.Departments.FirstOrDefault(d => d.Name == employeeDTO.DepartmentName);
            if (department == null)
            {
                return BadRequest($"Department '{employeeDTO.DepartmentName}' not found.");
            }

            var employee = new Employee
            {
                Name = employeeDTO.Name,
                Email = employeeDTO.Email,
                Phone = employeeDTO.Phone,
                City = employeeDTO.City,
                DepartmentId = department.Id, // Set the foreign key to the department

            };
            // Add the new employee to the database context and save changes
            myDbContext.Employees.Add(employee);
            myDbContext.SaveChanges();
            return Ok("Employee registered successfully.");
        }

        [HttpGet]
        public IActionResult GetEmployees()
        {
            var employees = myDbContext.Employees.Include(e => e.Department).ToList();
            var employeeResponses = employees.Select(e => new EmpoyeeResponseDto
            {
                Name = e.Name,
                Id = e.Id,
                Email = e.Email,
                Phone = e.Phone,
                City = e.City,
                DepartmentName = e.Department.Name
            }).ToList();
            return Ok(employeeResponses);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var employee = myDbContext.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound("Employee not found");
            }
            myDbContext.Employees.Remove(employee);
            myDbContext.SaveChanges();
            return Ok("Employee deleted successfully.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, EmployeeDTO employeeDTO)
        {
            var employee = myDbContext.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return BadRequest("Employee not found");
            }
            employee.Name = employeeDTO.Name;
            employee.Email = employeeDTO.Email;
            employee.Phone = employeeDTO.Phone;
            employee.City = employeeDTO.City;
            myDbContext.SaveChanges();
            return Ok("Employee updated successfully.");
        }

        [HttpGet("ITDepartmentEmp")]
        public IActionResult GetEmployeeOnDept()
        {
            var employees = myDbContext.Employees.Include(e => e.Department).Where(e => e.Department.Name == "IT").ToList();
            var employeeResponses = employees.Select(e => new EmpoyeeResponseDto
            {
                Name = e.Name,
                Id = e.Id,
                Email = e.Email,
                Phone = e.Phone,
                City = e.City,
                DepartmentName = e.Department.Name
            }).ToList();
            return Ok(employeeResponses);
        }

        [HttpGet("MiddlewareDemo")]
        public IActionResult GetEmployeeUsingMiddleware()
        {
           
            return Ok("Middleware working");
        }
    }
}