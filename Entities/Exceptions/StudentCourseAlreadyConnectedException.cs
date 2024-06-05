namespace Entities.Exceptions;

public sealed class StudentCourseAlreadyConnectedException(
    int studentId,
    int courseId)
    : EntitiesAlreadyConnectedException(
        $"The student with id '{studentId}' is already enrolled in the course with id '{courseId}' thus cannot be enrolled again.")
{
}
