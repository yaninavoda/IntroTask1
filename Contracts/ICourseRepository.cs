using IntroTask.Entities;

namespace Contracts;

public interface ICourseRepository
{
    Task<IEnumerable<Course>> GetAllCoursesAsync(bool trackChanges);
    Task<Course>? GetCourseByIdAsync(int id, bool trackChanges);
    void CreateCourse(Course course);
    void DeleteCourse(Course course);
}
