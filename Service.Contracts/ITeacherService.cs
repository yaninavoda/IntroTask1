using Shared.Dtos.TeacherDtos;

namespace Service.Contracts;

public interface ITeacherService
{
    Task<IEnumerable<TeacherShortResponseDto>> GetAllTeachersAsync(bool trackChanges);
    Task<TeacherShortResponseDto> GetTeacherByIdAsync(int id, bool trackChanges);
    Task<TeacherShortResponseDto> CreateTeacherAsync(TeacherCreateDto teacherCreateDto);
    Task DeleteTeacherAsync(int id, bool trackChanges);
    Task UpdateTeacherAsync(int id, TeacherUpdateDto teacherUpdateDto, bool trackChanges);
}
