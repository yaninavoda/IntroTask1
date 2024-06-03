namespace Service.Contracts;

public interface ITeacherCourseService
{
    Task AppointTeacherForCourse(int teacherId, int courseId, bool trackChanges);
}
