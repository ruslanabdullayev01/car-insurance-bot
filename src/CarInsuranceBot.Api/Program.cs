using CarInsuranceBot.Application;
using CarInsuranceBot.Infrastructure.Services.Helper;
using PdfSharp.Fonts;
using Telegram.Bot;
using Web.API.Controllers;
using Web.API.Extensions;
using Web.API.Middlewares;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Telegram bot
builder.Services.AddSingleton<ITelegramBotClient>(_ =>
    new TelegramBotClient(builder.Configuration["TelegramBot:Token"]!));

// Bot servis
builder.Services.AddHostedService<TelegramBotService>();

//Services
builder.Services.AddMediatr();
builder.Services.ConfigureSqlConnection(builder.Configuration);
builder.Services.AddRepositoriesInjection();
builder.Services.AddServicesInjection();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureCors();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

GlobalFontSettings.FontResolver = new WebRootFontResolver(app.Environment.WebRootPath);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCustomException();

app.UseCors("MyPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();