FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app
EXPOSE 5556

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["services/Kartel.Logistics.Bing/Kartel.Logistics.Bing.csproj", "services/Kartel.Logistics.Bing/"]
RUN dotnet restore "services/Kartel.Logistics.Bing/Kartel.Logistics.Bing.csproj"
COPY . .
WORKDIR "services/Kartel.Logistics.Bing"
RUN dotnet build "Kartel.Logistics.Bing.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kartel.Logistics.Bing.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kartel.Logistics.Bing.dll", "docker"]