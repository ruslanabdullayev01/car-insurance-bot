using Domain.Abstractions;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using CarInsuranceBot.Application.DTOs.Base;
using CarInsuranceBot.Application.Exceptions;

namespace Web.API.Middlewares;

public static class UseCustomExceptionHandler
{
    public static void UseCustomException(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(config => config.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                IExceptionHandlerFeature exceptionFeature = context.Features.Get<IExceptionHandlerFeature>()!;

                int statusCode = exceptionFeature!.Error switch
                {
                    ClientSideException => 400,
                    UnauthorizedException => 401,
                    NotFoundException => 404,
                    _ => 500
                };

                context.Response.StatusCode = statusCode;

                var response = Result<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message);

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));

            }));
    }
}
