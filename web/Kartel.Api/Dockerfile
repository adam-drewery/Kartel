FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Kartel.Api/Kartel.Api.csproj", "Kartel.Api/"]
RUN dotnet restore "Kartel.Api/Kartel.Api.csproj"
COPY . .
WORKDIR "/src/Kartel.Api"
RUN dotnet build "Kartel.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kartel.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 6840
EXPOSE 6841

ENTRYPOINT ["dotnet", "Kartel.Api.dll", "docker", "--urls", "http://0.0.0.0:6840"]
