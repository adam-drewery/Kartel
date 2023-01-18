FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app
EXPOSE 5555

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Kartel.PropertyMarket.ZooplaApi/Kartel.PropertyMarket.ZooplaApi.csproj", "Kartel.PropertyMarket.ZooplaApi/"]
RUN dotnet restore "Kartel.PropertyMarket.ZooplaApi/Kartel.PropertyMarket.ZooplaApi.csproj"
COPY . .
WORKDIR "/src/Kartel.PropertyMarket.ZooplaApi"
RUN dotnet build "Kartel.PropertyMarket.ZooplaApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kartel.PropertyMarket.ZooplaApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kartel.PropertyMarket.ZooplaApi.dll", "docker"]
