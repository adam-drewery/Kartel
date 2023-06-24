FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app
EXPOSE 5557

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["services/Kartel.Geocoding.Bing/Kartel.Geocoding.Bing.csproj", "services/Kartel.Geocoding.Bing/"]
RUN dotnet restore "services/Kartel.Geocoding.Bing/Kartel.Geocoding.Bing.csproj"
COPY . .
WORKDIR "services/Kartel.Geocoding.Bing"
RUN dotnet build "Kartel.Geocoding.Bing.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kartel.Geocoding.Bing.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kartel.Geocoding.Bing.dll", "docker"]