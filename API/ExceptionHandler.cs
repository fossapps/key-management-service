using System.Text;
using Newtonsoft.Json.Serialization;

namespace API;

public class ExceptionHandler : IErrorFilter
{
    public IError OnError(IError error)
    {
        return error.Exception == null
            ? error
            : error.WithMessage(error.Exception.Message).WithCode(ToSnakeCase(error.Exception.GetType().Name).ToUpper());
    }

    private static string ToSnakeCase(string text)
    {
        return new SnakeCaseNamingStrategy().GetPropertyName(text, false);
    }
}
