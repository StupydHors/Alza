FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Alza.API/Alza.API.csproj", "Alza.API/"]
COPY ["Alza.Application/Alza.Application.csproj", "Alza.Application/"]
COPY ["Alza.Domain/Alza.Domain.csproj", "Alza.Domain/"]
COPY ["Alza.Infrastructure/Alza.Infrastructure.csproj", "Alza.Infrastructure/"]
RUN dotnet restore "Alza.API/Alza.API.csproj"
COPY . .
WORKDIR "/src/Alza.API"
RUN dotnet build "Alza.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Alza.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Alza.API.dll"]
