FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["web/Kartel.Web/Kartel.Web.csproj", "web/Kartel.Web/"]
RUN dotnet restore "web/Kartel.Web/Kartel.Web.csproj"
COPY . .
WORKDIR "web/Kartel.Web"
RUN dotnet build "Kartel.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kartel.Web.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR "/usr/share/nginx/html"
COPY --from=publish "/app/publish/wwwroot" .
COPY "web/Kartel.Web/nginx.conf" "/etc/nginx/nginx.conf"