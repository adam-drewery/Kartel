﻿FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app
EXPOSE 5555

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["services/Kartel.PropertyMarket.ZooplaWeb/Kartel.PropertyMarket.ZooplaWeb.csproj", "services/Kartel.PropertyMarket.ZooplaWeb/"]
RUN dotnet restore "services/Kartel.PropertyMarket.ZooplaWeb/Kartel.PropertyMarket.ZooplaWeb.csproj"
COPY . .
WORKDIR "services/Kartel.PropertyMarket.ZooplaWeb"
RUN dotnet build "Kartel.PropertyMarket.ZooplaWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kartel.PropertyMarket.ZooplaWeb.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kartel.PropertyMarket.ZooplaWeb.dll", "docker"]