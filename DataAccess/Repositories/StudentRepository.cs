using Contracts;
using IntroTask.Entities;

namespace DataAccess.Repositories;

internal class StudentRepository : RepositoryBase<Student>, IStudentRepository
{
    public StudentRepository(AppDbContext context) : base(context)
    {
    }

    public void CreateStudent(Student student)
    {
        Create(student);
    }

    public void DeleteStudent(Student student)
    {
        Delete(student);
    }

    public IEnumerable<Student> GetAllStudents(bool trackChanges)
    {
        return FindAll(trackChanges)
            .OrderBy(x => x.LastName)
            .ToList();
    }

    public Student? GetStudentById(int id, bool trackChanges)
    {
        return FindByCondition(x => x.Id == id, trackChanges)
            .SingleOrDefault();
    }
}
