namespace Day2_EntityFrameworkCore.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }

        public int DepartmentId { get; set; } // Foreign key property
        public Department Department { get; set; }
    }
}
