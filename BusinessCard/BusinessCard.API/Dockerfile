#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BusinessCard.API/BusinessCard.API.csproj", "BusinessCard.API/"]
COPY ["BusinessCard.Domain/BusinessCard.Domain.csproj", "BusinessCard.Domain/"]
COPY ["BusinessCard.Infrastructure/BusinessCard.Infrastructure.csproj", "BusinessCard.Infrastructure/"]
RUN dotnet restore "BusinessCard.API/BusinessCard.API.csproj"
COPY . .
WORKDIR "/src/BusinessCard.API"
RUN dotnet build "BusinessCard.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BusinessCard.API.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BusinessCard.API.dll"]

ENV ASPNETCORE_URLS=http://+:80;https://+:443
ENV ASPNETCORE_ENVIRONMENT=Development