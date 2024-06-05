namespace Entities.Exceptions;

public sealed class TeacherCourseNotConnectedException(int teacherId, int courseId)
    : EntitiesNotConnectedException(
        $"The teacher with id '{teacherId}' is not assigned to the course with id '{courseId}' thus cannot be resigned from it.")
{
}
