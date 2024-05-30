using IntroTask.Entities;

namespace Contracts;

public interface IStudentRepository
{
    Task<IEnumerable<Student>> GetAllStudentsAsync(bool trackChanges);
    Task<Student>? GetStudentByIdAsync(int id, bool trackChanges);
    void CreateStudent(Student student);
    void DeleteStudent(Student student);
}
