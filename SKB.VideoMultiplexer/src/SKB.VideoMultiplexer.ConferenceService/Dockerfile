FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SKB.VideoMultiplexer.ConferenceService/SKB.VideoMultiplexer.ConferenceService.csproj", "src/SKB.VideoMultiplexer.ConferenceService/"]
RUN dotnet restore "src/SKB.VideoMultiplexer.ConferenceService/SKB.VideoMultiplexer.ConferenceService.csproj"
COPY . .
WORKDIR "/src/src/SKB.VideoMultiplexer.ConferenceService"
RUN dotnet build "./SKB.VideoMultiplexer.ConferenceService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SKB.VideoMultiplexer.ConferenceService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SKB.App.SKB.VideoMultiplexer.ConferenceService.dll"]
