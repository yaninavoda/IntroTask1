using Shared.Dtos.CourseDtos;

namespace Service.Contracts;

public interface ICourseService
{
    Task<IEnumerable<CourseResponseDto>> GetAllCoursesAsync(bool trackChanges);
    Task<CourseResponseDto> GetCourseByIdAsync(int id, bool trackChanges);
    Task<CourseResponseDto> CreateCourseAsync(CourseCreateDto courseCreateDto);
    Task DeleteCourseAsync(int id, bool trackChanges);
    Task UpdateCourseAsync(
        int id,
        CourseUpdateDto courseUpdateDto,
        bool trackChanges);
    Task AppointTeacherForCourse(
        int id,
        CourseAppointTeacherDto courseDto,
        bool trackChanges);
}
