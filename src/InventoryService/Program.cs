using InventorySystem.InventoryService.Services;

var builder = WebApplication.CreateBuilder(args);

// Cổng và giao thức (HTTP/1 cho health, HTTP/2 cho gRPC) cấu hình trong appsettings.json
// hoặc biến môi trường Kestrel__Endpoints__..., không viết cứng trong mã nguồn.
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<InventoryGrpcService>();

app.MapGet("/", () => "Inventory Service - gRPC server. Dùng gRPC client để gọi InventoryGrpc.");
app.MapGet("/health", () => Results.Ok(new { service = "inventory-service", status = "Healthy", time = DateTimeOffset.UtcNow }));

app.Logger.LogInformation("Inventory Service đang khởi động ({Environment})", app.Environment.EnvironmentName);

app.Run();
