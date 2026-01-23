namespace Registro.Web.Models;

public sealed class UiModal
{
    public string Title { get; init; } = "Mensaje";
    public string Message { get; init; } = "";
    public string Kind { get; init; } = "error"; 
}