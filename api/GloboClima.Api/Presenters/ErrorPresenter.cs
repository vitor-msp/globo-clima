using System.Text.Json;

namespace GloboClima.Api.Presenters;

public class ErrorPresenter
{
    public string ErrorMessage { get; private set; } = "";

    private ErrorPresenter() { }

    public static string GenerateJson(string errorMessage)
    {
        var output = new ErrorPresenter()
        {
            ErrorMessage = errorMessage
        };
        return JsonSerializer.Serialize(output);
    }
}