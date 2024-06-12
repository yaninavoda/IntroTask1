using Contracts;
using IntroTask.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

internal class StudentRepository : RepositoryBase<Student>, IStudentRepository
{
    public StudentRepository(AppDbContext context) : base(context)
    {
    }
}
