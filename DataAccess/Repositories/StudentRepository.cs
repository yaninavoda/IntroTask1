using Contracts;
using IntroTask.Entities;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IEnumerable<Student>> GetAllStudentsAsync(bool trackChanges)
    {
        return await FindAll(trackChanges)
            .OrderBy(x => x.Id)
            .ToListAsync();
    }

    public async Task<Student>? GetStudentByIdAsync(int id, bool trackChanges)
    {
        return await FindByCondition(x => x.Id == id, trackChanges)
            .Include(s => s.Courses)
            .SingleOrDefaultAsync();
    }
}
