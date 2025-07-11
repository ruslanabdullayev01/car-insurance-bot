namespace CarInsuranceBot.Application.Exceptions;

public sealed class NotFoundException(string message) : Exception(message);
