using Contracts;
using IntroTask.Entities;

namespace DataAccess.Repositories;

internal class TeacherRepository : RepositoryBase<Teacher>, ITeacherRepository
{
    public TeacherRepository(AppDbContext context) : base(context)
    {
    }
}
