namespace CarInsuranceBot.Application.Exceptions;

public sealed class ClientSideException(string message) : Exception(message);
