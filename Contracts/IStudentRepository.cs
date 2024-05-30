using IntroTask.Entities;

namespace Contracts;

public interface IStudentRepository
{
    IEnumerable<Student> GetAllStudents(bool trackChanges);
    Student? GetStudentById(int id, bool trackChanges);
    void CreateStudent(Student student);
    void DeleteStudent(Student student);
}
