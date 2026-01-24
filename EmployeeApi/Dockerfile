# Установка DejaVu шрифтов для работы SkiaSharp в Docker
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base
RUN apt-get update && apt-get install -y \
    fonts-dejavu-core \
    && rm -rf /var/lib/apt/lists/*

FROM base AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EmployeeApi/EmployeeApi.csproj", "EmployeeApi/"]
RUN dotnet restore "EmployeeApi/EmployeeApi.csproj"
COPY . .
WORKDIR "/src/EmployeeApi"
RUN dotnet build "EmployeeApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EmployeeApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8082
EXPOSE 8083
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmployeeApi.dll"]