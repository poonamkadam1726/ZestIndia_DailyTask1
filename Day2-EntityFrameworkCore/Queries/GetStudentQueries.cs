using Day2_EntityFrameworkCore.Models;
using MediatR;

namespace Day2_EntityFrameworkCore.Queries
{
    public class GetStudentQueries :IRequest<List<Student>>
    {

    }
}
