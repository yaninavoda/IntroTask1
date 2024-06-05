namespace Entities.Exceptions;

public abstract class EntitiesNotConnectedException(string message)
    : Exception(message)
{
}
