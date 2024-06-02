using IntroTask.Entities;

namespace Contracts;

public interface ITeacherRepository
{
    Task<IEnumerable<Teacher>> GetAllTeachersAsync(bool trackChanges);
    Task<Teacher>? GetTeacherByIdAsync(int id, bool trackChanges);
    void CreateTeacher(Teacher teacher);
    void DeleteTeacher(Teacher teacher);
}
