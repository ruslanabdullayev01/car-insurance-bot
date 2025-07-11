namespace CarInsuranceBot.Application.DTOs.Base;

public sealed class NoContentDto(string message)
{
    public string Message { get; set; } = message;
}
