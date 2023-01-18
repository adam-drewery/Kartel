# Kartel
Crime simulation strategy game written using a microservices architecture using: 
- ASP.NET Core
- Blazor Webassembly
- NetMQ
- Entity Framework
- MessagePack

## Starting the project

### Using docker-compose

In the root of the repo, run:

`docker compose build`

`docker compose up`

### Using .NET SDK

Run the following projects simultaneously:

`Kartel.Web`

`Kartel.Api`

`Kartel.PropertyMarket.ZooplaWeb`

`Kartel.Geocoding.Bing`

`Kartel.Logistics.Bing`