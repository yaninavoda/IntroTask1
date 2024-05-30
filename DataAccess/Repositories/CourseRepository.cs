using Contracts;
using IntroTask.Entities;

namespace DataAccess.Repositories;

internal class CourseRepository : RepositoryBase<Course>, ICourseRepository
{
    public CourseRepository(AppDbContext context) : base(context)
    {
    }
}
