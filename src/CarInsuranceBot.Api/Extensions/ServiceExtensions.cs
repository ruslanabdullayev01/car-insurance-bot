using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Application.IServices;
using CarInsuranceBot.Application.IServices.Helper;
using CarInsuranceBot.Application.IUnitOfWork;
using CarInsuranceBot.Infrastructure.Data;
using CarInsuranceBot.Infrastructure.Repositories;
using CarInsuranceBot.Infrastructure.Services;
using CarInsuranceBot.Infrastructure.Services.Helper;
using CarInsuranceBot.Infrastructure.Services.Mindee;
using CarInsuranceBot.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Web.API.Extensions;
public static class ServiceExtensions
{
    public static void ConfigureSqlConnection(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Db")));

    public static IServiceCollection AddRepositoriesInjection(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<IExtractedFieldRepository, ExtractedFieldRepository>();
        services.AddScoped<IErrorRepository, ErrorRepository>();
        services.AddScoped<IPolicyRepository, PolicyRepository>();
        return services;
    }

    public static IServiceCollection AddServicesInjection(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IOpenAIService, OpenAIService>();
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IPdfService, PdfService>();
        services.AddSingleton<MindeeOcrService>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<IExtractedFieldService, ExtractedFieldService>();
        services.AddScoped<IErrorService, ErrorService>();
        services.AddScoped<IPolicyService, PolicyService>();

        return services;
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "WebAPI",
                Version = "v1",
                Description = ".NET 8",
            });

            s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Place to add JWT with Bearer",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
              {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Name = "Bearer",
                },
                new List<string>()
              }
                });
        });
    }

    public static void ConfigureCors(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddCors(o => o.AddPolicy("MyPolicy", builder => builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod()));
    }
}