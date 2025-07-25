FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/CarInsuranceBot.Api/CarInsuranceBot.Api.csproj", "src/CarInsuranceBot.Api/"]
COPY ["src/CarInsuranceBot.Application/CarInsuranceBot.Application.csproj", "src/CarInsuranceBot.Application/"]
COPY ["src/CarInsuranceBot.Domain/CarInsuranceBot.Domain.csproj", "src/CarInsuranceBot.Domain/"]
COPY ["src/CarInsuranceBot.Infrastructure/CarInsuranceBot.Infrastructure.csproj", "src/CarInsuranceBot.Infrastructure/"]
RUN dotnet restore "./src/CarInsuranceBot.Api/./CarInsuranceBot.Api.csproj"

COPY . .
WORKDIR "src/CarInsuranceBot.Api"
RUN dotnet build "./CarInsuranceBot.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CarInsuranceBot.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "CarInsuranceBot.Api.dll"]
