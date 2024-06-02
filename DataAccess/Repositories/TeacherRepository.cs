using Contracts;
using IntroTask.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

internal class TeacherRepository : RepositoryBase<Teacher>, ITeacherRepository
{
    public TeacherRepository(AppDbContext context) : base(context)
    {
    }

    public void CreateTeacher(Teacher teacher)
    {
        throw new NotImplementedException();
    }

    public void DeleteTeacher(Teacher teacher)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Teacher>> GetAllTeachersAsync(bool trackChanges)
    {
        return await FindAll(trackChanges)
            .OrderBy(x => x.Id)
            .ToListAsync();
    }

    public async Task<Teacher>? GetTeacherByIdAsync(int id, bool trackChanges)
    {
        return await FindByCondition(x => x.Id == id, trackChanges)
            .SingleOrDefaultAsync();
    }
}
