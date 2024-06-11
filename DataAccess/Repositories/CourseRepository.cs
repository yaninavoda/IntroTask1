using Contracts;
using IntroTask.Entities;

namespace DataAccess.Repositories;

internal class CourseRepository : RepositoryBase<Course>, ICourseRepository
{
    public CourseRepository(AppDbContext context) : base(context)
    {
    }

    //public void CreateCourse(Course course)
    //{
    //    Create(course);
    //}

    //public void DeleteCourse(Course course)
    //{
    //    Delete(course);
    //}

    //public async Task<IEnumerable<Course>> GetAllCoursesAsync(bool trackChanges)
    //{
    //    return await FindAll(trackChanges)
    //       // .Include(x => x.Teacher)
    //        .OrderBy(x => x.Id)
    //        .ToListAsync();
    //}

    //public async Task<Course>? GetCourseByIdAsync(int id, bool trackChanges)
    //{
    //    return await FindByCondition(x => x.Id == id, trackChanges)
    //        .Include(c => c.Teacher)
    //        .Include(c => c.Students)
    //        .SingleOrDefaultAsync();
    //}

    //public async Task<IEnumerable<Course>>? GetCoursesByTeacherIdAsync(int teacherId, bool trackChanges)
    //{
    //    return await FindByCondition(x => x.TeacherId == teacherId, trackChanges)
    //        .ToListAsync();
    //}
}
