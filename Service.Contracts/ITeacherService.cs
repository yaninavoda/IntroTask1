using Shared.Dtos.TeacherDtos;

namespace Service.Contracts;

public interface ITeacherService
{
    Task<IEnumerable<TeacherShortResponseDto>> GetAllTeachersAsync();
    Task<TeacherResponseDto> GetTeacherByIdAsync(int id, bool trackChanges);
    Task<TeacherShortResponseDto> CreateTeacherAsync(TeacherCreateDto teacherCreateDto);
    Task DeleteTeacherAsync(int id, bool trackChanges);
    Task UpdateTeacherAsync(int id, TeacherUpdateDto teacherUpdateDto, bool trackChanges);
    Task ResignTeacherFromCourse(int id, int courseId, TeacherUpdateDto teacherUpdateDto, bool trackChanges);
}
