namespace Service.Contracts;

public interface ICourseStudentService
{
    Task EnrollStudentInCourseAsync(int  studentId, int courseId, bool trackChanges);
}
