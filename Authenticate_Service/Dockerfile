#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Authenticate_Service/AuthenticateService.API.csproj", "Authenticate_Service/"]
COPY ["Contract/Contract.csproj", "Contract/"]
COPY ["EventBus.Message/EventBus.Message.csproj", "EventBus.Message/"]
COPY ["Infrastructures/Infrastructures.csproj", "Infrastructures/"]
RUN dotnet restore "./Authenticate_Service/./AuthenticateService.API.csproj"
COPY . .
WORKDIR "/src/Authenticate_Service"
RUN dotnet build "./AuthenticateService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AuthenticateService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthenticateService.API.dll"]