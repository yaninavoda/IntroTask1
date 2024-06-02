using Contracts;
using IntroTask.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

internal class CourseRepository : RepositoryBase<Course>, ICourseRepository
{
    public CourseRepository(AppDbContext context) : base(context)
    {
    }

    public void CreateCourse(Course course)
    {
        Create(course);
    }

    public void DeleteCourse(Course course)
    {
        Delete(course);
    }

    public async Task<IEnumerable<Course>> GetAllCoursesAsync(bool trackChanges)
    {
        return await FindAll(trackChanges)
            .OrderBy(x => x.Id)
            .ToListAsync();
    }

    public async Task<Course>? GetCourseByIdAsync(int id, bool trackChanges)
    {
        return await FindByCondition(x => x.Id == id, trackChanges)
            .SingleOrDefaultAsync();
    }
}
