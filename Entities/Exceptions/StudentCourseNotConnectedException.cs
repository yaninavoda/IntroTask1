namespace Entities.Exceptions;

public sealed class StudentCourseNotConnectedException(int studentId, int courseId)
    : EntitiesNotConnectedException($"The student with id '{studentId}' is not enrolled in the course with id '{courseId}' thus cannot be excluded from it.")
{
}
