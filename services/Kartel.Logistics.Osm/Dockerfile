﻿FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app
EXPOSE 5556

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Kartel.Logistics.Osm/Kartel.Logistics.Osm.csproj", "Kartel.Logistics.Osm/"]
RUN dotnet restore "Kartel.Logistics.Osm/Kartel.Logistics.Osm.csproj"
COPY . .
WORKDIR "/src/Kartel.Logistics.Osm"
RUN dotnet build "Kartel.Logistics.Osm.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kartel.Logistics.Osm.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kartel.Logistics.Osm.dll", "docker"]
