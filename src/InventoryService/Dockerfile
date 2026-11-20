# Build context: thư mục gốc repo (để lấy được protos/inventory.proto)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

COPY protos/ protos/
COPY src/InventoryService/InventoryService.csproj src/InventoryService/
RUN dotnet restore src/InventoryService/InventoryService.csproj

COPY src/InventoryService/ src/InventoryService/
RUN dotnet publish src/InventoryService/InventoryService.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .

EXPOSE 8080 8081
ENTRYPOINT ["dotnet", "InventoryService.dll"]
