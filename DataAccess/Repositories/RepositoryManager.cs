using Contracts;

namespace DataAccess.Repositories;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly AppDbContext _context;
    private readonly Lazy<ICourseRepository> _courseRepository;
    private readonly Lazy<ITeacherRepository> _teacherRepository;
    private readonly Lazy<IStudentRepository> _studentRepository;

    public RepositoryManager(AppDbContext context)
    {
        _context = context;
        _courseRepository = new Lazy<ICourseRepository>(() => new CourseRepository(context));
        _teacherRepository = new Lazy<ITeacherRepository>(() => new TeacherRepository(context));
        _studentRepository = new Lazy<IStudentRepository>(() => new StudentRepository(context));
    }

    public ICourseRepository Course => _courseRepository.Value;
    public ITeacherRepository Teacher => _teacherRepository.Value;
    public IStudentRepository Student => _studentRepository.Value;

    public void Save() => _context.SaveChanges();
}
