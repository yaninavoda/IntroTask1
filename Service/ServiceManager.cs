using AutoMapper;
using Contracts;
using Service.Contracts;

namespace Service;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<ICourseService> _courseService;
    private readonly Lazy<ITeacherService> _teacherService;
    private readonly Lazy<IStudentService> _studentService;

    public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _courseService = new Lazy<ICourseService>(() => new CourseService(repositoryManager, mapper));
        _teacherService = new Lazy<ITeacherService>(() => new TeacherService(repositoryManager, mapper));
        _studentService = new Lazy<IStudentService>(() => new StudentService(repositoryManager, mapper));
    }

    public ICourseService CourseService => _courseService.Value;
    public ITeacherService TeacherService => _teacherService.Value;
    public IStudentService StudentService => _studentService.Value;
}
