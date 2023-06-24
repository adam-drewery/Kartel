FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app
EXPOSE 5559

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["services/Kartel.Locale.Bing/Kartel.Locale.Bing.csproj", "services/Kartel.Locale.Bing/"]
RUN dotnet restore "services/Kartel.Locale.Bing/Kartel.Locale.Bing.csproj"
COPY . .
WORKDIR "services//Kartel.Locale.Bing"
RUN dotnet build "Kartel.Locale.Bing.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kartel.Locale.Bing.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kartel.Locale.Bing.dll", "docker"]