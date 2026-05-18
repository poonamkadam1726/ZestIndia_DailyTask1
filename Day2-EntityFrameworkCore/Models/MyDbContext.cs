using Microsoft.EntityFrameworkCore;

namespace Day2_EntityFrameworkCore.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
