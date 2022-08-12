namespace Business.Exceptions;

public class OperationNotPermitted : Exception
{
    public OperationNotPermitted(string? message) : base(message)
    {
    }
}
