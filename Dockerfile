#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GreenFlux.API/GreenFlux.API.csproj", "GreenFlux.API/"]
COPY ["GreenFlux.Application/GreenFlux.Application.csproj", "GreenFlux.Application/"]
COPY ["GreenFlux.Domain/GreenFlux.Domain.csproj", "GreenFlux.Domain/"]
COPY ["GreenFlux.Infrastructure/GreenFlux.Infrastructure.csproj", "GreenFlux.Infrastructure/"]
RUN dotnet restore "./GreenFlux.API/GreenFlux.API.csproj"
COPY . .
WORKDIR "/src/GreenFlux.API"
RUN dotnet build "./GreenFlux.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GreenFlux.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GreenFlux.API.dll"]