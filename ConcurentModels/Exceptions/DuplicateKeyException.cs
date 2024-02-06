namespace ConcurentModels.Exceptions;

public class DuplicateKeyException : Exception
{
    public DuplicateKeyException(string message) : base(message)
    {
    }
}