# Kartel
Crime simulation strategy game which uses real-world data, written using a microservices architecture using: 
- ASP.NET Core
- Blazor Webassembly
- NetMQ
- Entity Framework
- MessagePack

## Starting the project

### Set your API keys

Navigate to `shared/Kartel.Config`
and run the following commands to set dotnet secrets:

```
dotnet user-secrets set "Google:ApiKey" "replaceme"
dotnet user-secrets set "Bing:ApiKey" "replaceme"
```

### Using docker-compose

In the root of the repo, run:

```
docker compose build
docker compose up
```

### Using .NET SDK

Run the following projects simultaneously:

```
Kartel.Web
Kartel.Api
Kartel.PropertyMarket.ZooplaWeb
Kartel.Geocoding.Bing
Kartel.Logistics.Bing
Kartel.Locale.Google
```
