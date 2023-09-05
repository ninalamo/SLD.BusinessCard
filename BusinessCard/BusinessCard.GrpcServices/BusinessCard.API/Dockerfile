FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BusinessCard.GrpcServices/BusinessCard.GrpcServices/BusinessCard.GrpcServices.csproj", "BusinessCard.GrpcServices/BusinessCard.GrpcServices/"]
RUN dotnet restore "BusinessCard.GrpcServices/BusinessCard.GrpcServices/BusinessCard.GrpcServices.csproj"
COPY . .
WORKDIR "/src/BusinessCard.GrpcServices/BusinessCard.GrpcServices"
RUN dotnet build "BusinessCard.GrpcServices.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BusinessCard.GrpcServices.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BusinessCard.GrpcServices.dll"]
