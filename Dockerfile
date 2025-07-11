FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Web.API/Web.API.csproj", "src/Web.API/"]
COPY ["src/BusinessLayer/BusinessLayer.csproj", "src/BusinessLayer/"]
COPY ["src/DataAccessLayer/DataAccessLayer.csproj", "src/DataAccessLayer/"]
COPY ["src/CoreLayer/CoreLayer.csproj", "src/CoreLayer/"]
RUN dotnet restore "./src/Web.API/./Web.API.csproj"

COPY . .
WORKDIR "src/Web.API"
RUN dotnet build "./Web.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Web.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Web.API.dll"]
