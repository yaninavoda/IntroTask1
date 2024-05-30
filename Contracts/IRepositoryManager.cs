namespace Contracts;

public interface IRepositoryManager
{
    ICourseRepository Course { get; }
    ITeacherRepository Teacher { get; }
    IStudentRepository Student { get; }
    Task SaveAsync();
}
