using Shared.Dtos.StudentDtos;

namespace Service.Contracts;

public interface IStudentService
{
    Task<IEnumerable<StudentShortResponseDto>> GetAllStudentsAsync();
    Task<StudentResponseDto> GetStudentByIdAsync(int id, bool trackChanges);
    Task<StudentShortResponseDto> CreateStudentAsync(StudentCreateDto studentCreateDto);
    Task DeleteStudentAsync(int id, bool trackChanges);
    Task UpdateStudentAsync(int id, StudentUpdateDto studentUpdateDto, bool trackChanges);
    Task EnrollStudentInCourseAsync(int studentId, int courseId, StudentUpdateDto studentUpdateDto, bool trackChanges);
}
