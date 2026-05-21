using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Day2_EntityFrameworkCore.Models; 
using Day2_EntityFrameworkCore.Queries;
using Day2_EntityFrameworkCore.Repository; 

namespace Day2_EntityFrameworkCore.Queries
{
    public class GetStudentHandler : IRequestHandler<GetStudentQueries, List<Student>>
    {
        public Task<List<Student>> Handle(GetStudentQueries request, CancellationToken cancellationToken)
        {
            return Task.FromResult(StudentRepository.students);
        }
    }
}
