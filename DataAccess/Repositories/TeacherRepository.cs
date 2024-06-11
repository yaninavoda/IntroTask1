using Contracts;
using IntroTask.Entities;

namespace DataAccess.Repositories;

public class TeacherRepository : RepositoryBase<Teacher>, ITeacherRepository
{
    public TeacherRepository(AppDbContext context) : base(context)
    {
    }

    //public void Create(Teacher teacher)
    //{
    //    Create(teacher);
    //}

    //public void Delete(Teacher teacher)
    //{
    //    Delete(teacher);
    //}

    //public async Task<IEnumerable<Teacher>> GetAllAsync(bool trackChanges)
    //{
    //    return await FindAll(trackChanges)
    //        .OrderBy(x => x.Id)
    //        .ToListAsync();
    //}

    //public async Task<Teacher>? GetTeacherByIdAsync(int id, bool trackChanges)
    //{
    //    return await FindByCondition(x => x.Id == id, trackChanges)
    //        .Include(t => t.Courses)
    //        .SingleOrDefaultAsync();
    //}
}
