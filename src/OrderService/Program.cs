using InventorySystem.Contracts.Inventory;
using InventorySystem.OrderService.Endpoints;
using InventorySystem.OrderService.Services;

var builder = WebApplication.CreateBuilder(args);

// Địa chỉ Inventory Service lấy từ cấu hình (appsettings / biến môi trường Services__InventoryGrpcUrl).
var inventoryUrl = builder.Configuration["Services:InventoryGrpcUrl"]
    ?? throw new InvalidOperationException("Thiếu cấu hình Services:InventoryGrpcUrl");

builder.Services
    .AddGrpcClient<InventoryGrpc.InventoryGrpcClient>(o => o.Address = new Uri(inventoryUrl));
builder.Services.AddScoped<InventoryClient>();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { service = "order-service", status = "Healthy", time = DateTimeOffset.UtcNow }));
app.MapOrderEndpoints();

app.Logger.LogInformation("Order Service khởi động, Inventory gRPC = {InventoryUrl}", inventoryUrl);

app.Run();
