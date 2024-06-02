namespace Entities.Exceptions;

public sealed class CourseNotFoundException : NotFoundException
{
    public CourseNotFoundException(int courseId)
    : base($"The course with id: {courseId} doesn't exist in the database.")
    {
    }
}