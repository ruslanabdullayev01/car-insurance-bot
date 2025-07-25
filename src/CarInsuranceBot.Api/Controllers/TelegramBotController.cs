using CarInsuranceBot.Application.DTOs.Document;
using CarInsuranceBot.Application.DTOs.User;
using CarInsuranceBot.Application.IServices;
using CarInsuranceBot.Infrastructure.Services.Mindee;
using Domain.Enums;
using Domain.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Web.API.Controllers;

public class TelegramBotService(ITelegramBotClient botClient, IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, cancellationToken: stoppingToken);
        return Task.CompletedTask;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var ocrService = scope.ServiceProvider.GetRequiredService<MindeeOcrService>();
        var documentService = scope.ServiceProvider.GetRequiredService<IDocumentService>();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var extractedFieldService = scope.ServiceProvider.GetRequiredService<IExtractedFieldService>();
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        var errorService = scope.ServiceProvider.GetRequiredService<IErrorService>();

        try
        {
            var chatId = update.Message!.Chat.Id;
            var fullName = update.Message.From?.FirstName ?? "User";

            if (update is { Type: UpdateType.Message, Message.Type: MessageType.Photo })
            {
                var user = await userService.GetUserAsync(chatId, ct);
                var fileId = update.Message.Photo!.Last().FileId;
                var file = await bot.GetFileAsync(fileId, ct);

                string fileName = $"{Guid.NewGuid()}.jpg";
                using var ms = new MemoryStream();
                await bot.DownloadFileAsync(file.FilePath!, ms, ct);
                ms.Position = 0;

                IFormFile formFile = new FormFile(ms, 0, ms.Length, "file", fileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg"
                };

                Dictionary<string, string> ocrFields;
                bool valid;

                switch (user.State)
                {
                    case StateType.WaitingPassportPhoto:
                        {
                            var savedPasswordPath = await documentService.SaveDocumentAsync(
                                new SaveDocumentRequest(user.Id, formFile, DocumentType.Passport, fileName), env);

                            ocrFields = await ocrService.OcrPasswordAsync(savedPasswordPath.AbsolutePath);

                            valid = ocrFields.TryGetValue("document_type", out var docType) && docType.ToLower().Contains("passport");

                            if (!valid)
                            {
                                await bot.SendTextMessageAsync(
                                    chatId,
                                    "❌ This document is *not a passport*. Please send your passport correctly.",
                                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                                    cancellationToken: ct);
                                FileExtensions.DeleteFile(savedPasswordPath.RelativePath, env);
                                return;
                            }

                            foreach (var (key, value) in ocrFields)
                                await extractedFieldService.SaveExtractedFieldAsync(user.Id, key, value);

                            await userService.UpdateUserStateAsync(chatId, StateType.WaitingRegistrationPhoto, ct);

                            await bot.SendTextMessageAsync(
                                chatId,
                                "✅ Passport photo received! Now, please send a photo of your *vehicle registration certificate*.",
                                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                                cancellationToken: ct
                            );
                            break;
                        }
                    case StateType.WaitingRegistrationPhoto:
                        {
                            var savedRegistrationPath = await documentService.SaveDocumentAsync(
                                new SaveDocumentRequest(user.Id, formFile, DocumentType.Registration, fileName), env);

                            ocrFields = await ocrService.OcrVehicleCertificateAsync(savedRegistrationPath.AbsolutePath);

                            valid = ocrFields.TryGetValue("registration_number", out var regNum)
                                    && !string.IsNullOrWhiteSpace(regNum);
                            if (!valid)
                            {
                                await bot.SendTextMessageAsync(
                                    chatId,
                                    "❌ The vehicle registration certificate could not be read. Please send a clear photo of the document.",
                                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                                    cancellationToken: ct);
                                FileExtensions.DeleteFile(savedRegistrationPath.RelativePath, env);
                                return;
                            }

                            foreach (var (key, value) in ocrFields)
                                await extractedFieldService.SaveExtractedFieldAsync(user.Id, key, value);
                            await userService.UpdateUserStateAsync(chatId, StateType.ReadyForOcr, ct);
                            await bot.SendTextMessageAsync(
                                chatId,
                                "✅ All documents received! Now you can view the OCR results using /viewocr.",
                                cancellationToken: ct
                            );
                            break;
                        }
                    default:
                        await bot.SendTextMessageAsync(
                            chatId,
                            "⚠️ You cannot send a document right now. Please follow the instructions or type /status.",
                            cancellationToken: ct
                        );
                        break;
                }
            }

            if (update is { Type: UpdateType.Message, Message.Type: MessageType.Text })
            {
                string? text = update.Message.Text?.Trim().ToLower();
                var user = await userService.GetUserAsync(chatId, ct);

                if (text == "/start")
                {
                    await userService.CreateOrUpdateUsersAsync(
                        new CreateOrUpdateUserRequest(chatId, fullName), ct);
                    await bot.SendTextMessageAsync(
                        chatId,
                        BotMessagesExtensions.Welcome(fullName),
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                        cancellationToken: ct
                    );
                    return;
                }

                if (text == "/help")
                {
                    await bot.SendTextMessageAsync(chatId, BotMessagesExtensions.Help, cancellationToken: ct);
                    return;
                }

                if (text == "/status")
                {
                    string stateMsg = user?.State switch
                    {
                        StateType.WaitingPassportPhoto => "📄 Currently: Waiting for your passport photo.",
                        StateType.WaitingRegistrationPhoto => "📄 Currently: Waiting for your vehicle registration photo.",
                        StateType.ReadyForOcr => "✅ All documents received, analysis will start soon.",
                        StateType.Completed => "🎉 All steps are completed! If you want to start again, just type /cancel to reset the process.",
                        _ => "You haven't started yet or the process was canceled. Type /start to begin."
                    };
                    await bot.SendTextMessageAsync(chatId, stateMsg, cancellationToken: ct);
                    return;
                }

                if (text == "/cancel")
                {
                    // await mediator.Send(new CancelUserProcessCommand(chatId), ct);
                    // await bot.SendTextMessageAsync(
                    //     chatId,
                    //     "The operation has been canceled and your process has been reset. You can start again by typing /start.",
                    //     cancellationToken: ct
                    // );
                    return;
                }

                if (user.State == StateType.ReadyForOcr && text == "/viewocr")
                {
                    var extractedFields = await extractedFieldService.GetAllNonEmptyByUserIdAsync(user.Id);
                    var filtered = extractedFields
                        .Where(kv => !string.IsNullOrWhiteSpace(kv.Value) && kv.Value.ToLower() != "null")
                        .ToDictionary(kv => kv.Key, kv => kv.Value);
                    var msg = "OCR Results:\n\n" +
                              string.Join("\n", filtered.Select(x => $"{x.Key}: {x.Value}")) +
                              "\n\nYour insurance payment is 100$. Do you accept?\nPlease reply Yes or No.";
                    await bot.SendTextMessageAsync(
                        chatId,
                        msg,
                        cancellationToken: ct
                    );
                }

                if (user.State == StateType.ReadyForOcr && text is "yes" or "no")
                {
                    switch (text)
                    {
                        case "yes":

                            await userService.UpdateUserStateAsync(chatId, StateType.Completed, ct);
                            break;
                        case "no":

                            await userService.UpdateUserStateAsync(chatId, StateType.WaitingPassportPhoto, ct);
                            await bot.SendTextMessageAsync(
                                chatId,
                                BotMessagesExtensions.Welcome(fullName),
                                cancellationToken: ct
                            );
                            break;
                        default:
                            await bot.SendTextMessageAsync(
                                chatId,
                                "Please reply with *Yes* or *No* to confirm your information.",
                                parseMode: ParseMode.Markdown,
                                cancellationToken: ct
                            );
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            var chatId = update.Message!.Chat.Id;
            Domain.Entities.User user = await userService.GetUserAsync(chatId, CancellationToken.None);
            string? context = update.Message?.Text
                              ?? update.CallbackQuery?.Data
                              ?? update.Type.ToString();
            await errorService.LogErrorAsync(ex.Message, ex.StackTrace, user.Id, context);
        }
    }

    private async Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken ct)
    {
        Console.WriteLine($"Hata: {exception.Message}");
        using var scope = serviceScopeFactory.CreateScope();
        var errorService = scope.ServiceProvider.GetRequiredService<IErrorService>();
        await errorService.LogErrorAsync(exception.Message, exception.StackTrace, null, "TelegramPolling");
    }
}