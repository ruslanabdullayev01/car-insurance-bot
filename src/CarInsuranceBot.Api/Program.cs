using CarInsuranceBot.Application;
using Web.API.Extensions;
using Web.API.Middlewares;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//Services
builder.Services.AddMediatr();
builder.Services.ConfigureSqlConnection(builder.Configuration);
builder.Services.AddRepositoriesInjection();
builder.Services.AddServicesInjection();
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.ConfigureSwagger();
builder.Services.ConfigureCors();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

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
