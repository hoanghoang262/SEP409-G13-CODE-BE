#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ModerationService.API/ModerationService.API.csproj", "ModerationService.API/"]
COPY ["Contract/Contract.csproj", "Contract/"]
COPY ["EventBus.Message/EventBus.Message.csproj", "EventBus.Message/"]
RUN dotnet restore "./ModerationService.API/./ModerationService.API.csproj"
COPY . .
WORKDIR "/src/ModerationService.API"
RUN dotnet build "./ModerationService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ModerationService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ModerationService.API.dll"]