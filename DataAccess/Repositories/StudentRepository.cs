using Contracts;
using IntroTask.Entities;

namespace DataAccess.Repositories;

internal class StudentRepository : RepositoryBase<Student>, IStudentRepository
{
    public StudentRepository(AppDbContext context) : base(context)
    {
    }

    public IEnumerable<Student> GetAllStudents(bool trackChanges)
    {
        return FindAll(trackChanges)
            .OrderBy(x => x.LastName)
            .ToList();
    }
}
