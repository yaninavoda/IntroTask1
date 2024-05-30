using Shared.Dtos;

namespace Service.Contracts;

public interface IStudentService
{
    Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync(bool trackChanges);
    Task<StudentResponseDto> GetStudentByIdAsync(int id, bool trackChanges);
    Task <StudentResponseDto> CreateStudentAsync(StudentCreateDto studentCreateDto);
    Task DeleteStudentAsync(int id, bool trackChanges);
    Task UpdateStudentAsync(int id, StudentUpdateDto studentUpdateDto, bool trackChanges);

}
