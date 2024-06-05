namespace Entities.Exceptions;

public abstract class EntitiesAlreadyConnectedException(string message)
    : Exception(message)
{
}
