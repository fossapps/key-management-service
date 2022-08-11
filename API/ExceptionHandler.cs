namespace API;

public class ExceptionHandler : IErrorFilter
{
    public IError OnError(IError error)
    {
        return error.Exception == null
            ? error
            : error.WithMessage(error.Exception.Message);
    }
}
