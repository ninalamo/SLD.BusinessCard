FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HttpAggregator/HttpAggregator.csproj", "HttpAggregator/"]
RUN dotnet restore "HttpAggregator/HttpAggregator.csproj"
COPY . .
WORKDIR "/src/HttpAggregator"
RUN dotnet build "HttpAggregator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HttpAggregator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HttpAggregator.dll"]
