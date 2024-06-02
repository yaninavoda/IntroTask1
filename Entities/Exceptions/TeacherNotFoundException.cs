namespace Entities.Exceptions;

public sealed class TeacherNotFoundException : NotFoundException
{
    public TeacherNotFoundException(int teacherId)
    : base($"The teacher with id: {teacherId} doesn't exist in the database.")
    {
    }
}
