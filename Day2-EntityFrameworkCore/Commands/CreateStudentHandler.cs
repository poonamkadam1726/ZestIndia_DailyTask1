using Day2_EntityFrameworkCore.Models;
using Day2_EntityFrameworkCore.Repository;
using MediatR;

namespace Day2_EntityFrameworkCore.Commands
{
    public class CreateStudentHandler : IRequestHandler<CreateStudentCommand, string>
    {
        public Task<string> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            // add code to save the student to a database
            var student = new Student
            {
                Id = StudentRepository.students.Count + 1,
                Name = request.Name,
                Department = request.Department
            };
            StudentRepository.students.Add(student);
            return Task.FromResult("Student created successfully");
        }
    }
}
