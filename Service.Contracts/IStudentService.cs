using Shared.Dtos;

namespace Service.Contracts;

public interface IStudentService
{
    IEnumerable<StudentDto> GetAllStudents(bool trackChanges);
}
