using Contracts;
using IntroTask.Entities;

namespace DataAccess.Repositories;

public class TeacherRepository : RepositoryBase<Teacher>, ITeacherRepository
{
    public TeacherRepository(AppDbContext context) : base(context)
    {
    }
}
