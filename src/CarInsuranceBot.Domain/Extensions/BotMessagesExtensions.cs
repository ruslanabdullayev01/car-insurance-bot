namespace Domain.Extensions;

public static class BotMessagesExtensions
{
    public static string Welcome(string fullName) =>
        $"Hello, {fullName}! 👋\n\n" +
        "This bot helps you to submit your documents for car insurance.\n\n" +
        "Please send the following documents in order:\n" +
        "1️⃣ **A photo of your passport**\n" +
        "2️⃣ **A photo of your vehicle registration certificate**\n\n" +
        "ℹ️ If you need assistance or want to see available commands, type /help at any time. \n\n" +
        "First, please send a **photo of your passport**.";

    public static string Help =
        "Help — Available commands:\n" +
        "/start — Start the process\n" +
        "/help — Show help and command list\n" +
        "/status — Show your current step/missing document\n" +
        "/cancel — Cancel the operation and reset your process\n" +
        "/chat — Chat with ChatGPT";

    public static string ChatFinish =
        "Chat finished:\n" +
        "/start — Start the process\n" +
        "/help — Show help and command list\n" +
        "/status — Show your current step/missing document\n" +
        "/cancel — Cancel the operation and reset your process\n" +
        "/chat — Chat with ChatGPT";
}