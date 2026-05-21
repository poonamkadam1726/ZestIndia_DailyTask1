using MediatR;

namespace Day2_EntityFrameworkCore.Commands
{
    public class CreateStudentCommand : IRequest<string>
    {
        public string Name { get; set; }
        public string Department { get; set; }  
    }
}
