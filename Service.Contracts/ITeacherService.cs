using Shared.Dtos.TeacherDtos;

namespace Service.Contracts;

public interface ITeacherService
{
    Task<IEnumerable<TeacherResponseDto>> GetAllTeachersAsync(bool trackChanges);
    Task<TeacherResponseDto> GetTeacherByIdAsync(int id, bool trackChanges);
    Task<TeacherResponseDto> CreateTeacherAsync(TeacherCreateDto teacherCreateDto);
    Task DeleteTeacherAsync(int id, bool trackChanges);
    Task UpdateTeacherAsync(int id, TeacherUpdateDto teacherUpdateDto, bool trackChanges);
}
