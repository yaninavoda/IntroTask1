namespace Service.Contracts;

public interface IServiceManager
{
    ICourseService CourseService { get; }
    ITeacherService TeacherService { get; }
    IStudentService StudentService { get; }
}
