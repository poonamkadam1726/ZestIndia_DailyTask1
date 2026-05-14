using Day2_EntityFrameworkCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Day2_EntityFrameworkCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly MyDbContext myDbContext;

   
        public DepartmentController(MyDbContext myDbContext)
        {
            this.myDbContext = myDbContext;
        }

        [HttpPost]
        public IActionResult AddDepartment(DepartmentDTO departmentDTO)
        {
            var existingDepartment=myDbContext.Departments.FirstOrDefault(d=>d.Name==departmentDTO.Name);
            if (existingDepartment != null)
            {
                return BadRequest($"Department {departmentDTO.Name} already exists.");
            }

            var department = new Department
            {
                Name = departmentDTO.Name
            };

            myDbContext.Departments.Add(department);

            myDbContext.SaveChanges();

            return Ok("Department added successfully");
        }
    }
}
