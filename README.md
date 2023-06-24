# Kartel

[![.NET](https://github.com/adam-drewery/Kartel/actions/workflows/dotnet.yml/badge.svg)](https://github.com/adam-drewery/Kartel/actions/workflows/dotnet.yml)

Crime simulation strategy game which uses real-world data, written using a microservices architecture using: 
- ASP.NET Core
- Blazor Webassembly
- NetMQ
- Entity Framework
- MessagePack
- SignalR

## Starting the project

### Using docker-compose

Navigate to `shared/Kartel.Config` and edit `appsettings.docker.json` to include the relevant API keys for google and bing:

```json
"BingMaps": {
    "ApiKey": ""
  },
  "Google": {
    "ApiKey": ""
  }
```

In the root of the repo, run:

```shell
docker compose build
docker compose up
```

### Using .NET SDK

Navigate to `shared/Kartel.Config`
and run the following commands to set your API keys:

```shell
dotnet user-secrets set "Google:ApiKey" "replaceme"
dotnet user-secrets set "Bing:ApiKey" "replaceme"
```

Run the following projects simultaneously:

```shell
Kartel.Web
Kartel.Api
Kartel.PropertyMarket.ZooplaWeb
Kartel.Geocoding.Bing
Kartel.Logistics.Bing
Kartel.Locale.Google
```
