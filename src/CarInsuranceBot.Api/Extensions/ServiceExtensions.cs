using System.Text;
using CarInsuranceBot.Application.IRepositories;
using CarInsuranceBot.Application.IServices;
using CarInsuranceBot.Application.IUnitOfWork;
using CarInsuranceBot.Infrastructure.Context;
using CarInsuranceBot.Infrastructure.Repositories;
using CarInsuranceBot.Infrastructure.Services;
using CarInsuranceBot.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Web.API.Extensions;
public static class ServiceExtensions
{
    public static void ConfigureSqlConnection(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));
    
    public static IServiceCollection AddRepositoriesInjection(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        return services;
    }

    public static IServiceCollection AddServicesInjection(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        return services;
    }
    
    public static IServiceCollection AddJwtConfiguration(this IServiceCollection services,
        IConfiguration config)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:secret"]!));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
               .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = key,
                   //LifetimeValidator = (notBefore, expires, securityToken, validationParameters)
                   //=> expires != null ? expires > DateTime.UtcNow : false
               });
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

    //public static void UseSeedData(this WebApplication app)
    //{
    //    var scopFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
    //    using (var scope = scopFactory.CreateScope())
    //    {
    //        var context = scope.ServiceProvider.GetService<AppDbContext>();
    //        var passwordHasher = scope.ServiceProvider.GetService<IPasswordHasher>();
    //        var dateTimeProvider = scope.ServiceProvider.GetService<IDateTimeProvider>();

    //        DbInitializer.Seed(context, passwordHasher, dateTimeProvider);
    //    }

    //}

    public static void ConfigureCors(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddCors(o => o.AddPolicy("MyPolicy", builder => builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod()));
    }
}
