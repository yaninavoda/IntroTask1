using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;

namespace Service;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<ICourseService> _courseService;
    private readonly Lazy<ITeacherService> _teacherService;
    private readonly Lazy<IStudentService> _studentService;
    private readonly Lazy<IAuthenticationService> _authenticationService;

    public ServiceManager(
        IRepositoryManager repositoryManager,
        IMapper mapper,
        ILoggerManager loggerManager,
        UserManager<User> userManager,
        IConfiguration configuration)
    {
        _courseService = new Lazy<ICourseService>(() => new CourseService(repositoryManager, mapper));
        _teacherService = new Lazy<ITeacherService>(() => new TeacherService(repositoryManager, mapper));
        _studentService = new Lazy<IStudentService>(() => new StudentService(repositoryManager, mapper));
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(
            repositoryManager,
            mapper,
            loggerManager,
            userManager,
            configuration));
    }

    public ICourseService CourseService => _courseService.Value;
    public ITeacherService TeacherService => _teacherService.Value;
    public IStudentService StudentService => _studentService.Value;
    public IAuthenticationService AuthenticationService => _authenticationService.Value;
}
