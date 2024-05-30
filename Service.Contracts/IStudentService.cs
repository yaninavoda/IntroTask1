using Shared.Dtos;

namespace Service.Contracts;

public interface IStudentService
{
    IEnumerable<StudentResponseDto> GetAllStudents(bool trackChanges);
    StudentResponseDto GetStudentById(int id, bool trackChanges);
    StudentResponseDto CreateStudent(StudentCreateDto studentCreateDto);
    void DeleteStudent(int id, bool trackChanges);

}
