# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Копируем проектные файлы
COPY ["MerosWebApi/MerosWebApi.csproj", "MerosWebApi/"]
COPY ["MerosWebApi.Application/MerosWebApi.Application.csproj", "MerosWebApi.Application/"]
COPY ["MerosWebApi.Core/MerosWebApi.Core.csproj", "MerosWebApi.Core/"]
COPY ["MerosWebApi.Persistence/MerosWebApi.Persistence.csproj", "MerosWebApi.Persistence/"]

# Выполняем восстановление зависимостей
RUN dotnet restore "./MerosWebApi/MerosWebApi.csproj"

# Копируем остальные файлы проекта
COPY . .

# Переходим в директорию приложения и собираем проект
WORKDIR "/src/MerosWebApi"
RUN dotnet build "./MerosWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MerosWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MerosWebApi.dll"]