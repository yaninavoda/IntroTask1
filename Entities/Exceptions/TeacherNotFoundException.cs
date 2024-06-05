namespace Entities.Exceptions;

public sealed class TeacherNotFoundException(int teacherId)
    : NotFoundException($"The teacher with id: {teacherId} doesn't exist in the database.")
{
}
