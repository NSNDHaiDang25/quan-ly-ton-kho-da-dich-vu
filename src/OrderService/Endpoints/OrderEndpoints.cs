using Grpc.Core;
using InventorySystem.OrderService.Services;

namespace InventorySystem.OrderService.Endpoints;

/// <summary>
/// Order API - các REST endpoint phía client (ASP.NET Core Minimal API).
/// </summary>
public static class OrderEndpoints
{
    public sealed record ProductDto(string Id, string Sku, string Name, int AvailableQuantity);

    public sealed record CreateOrderLine(string ProductId, int Quantity);

    public sealed record CreateOrderRequest(string CustomerName, List<CreateOrderLine> Items);

    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/products", GetProducts).WithName("GetProducts");

        // Tuần 2-3: lưu Order DB, gọi CheckStock/ReserveStock, trả về trạng thái đơn.
        app.MapPost("/orders", (CreateOrderRequest request) =>
            Results.Problem("POST /orders sẽ được cài đặt ở tuần 2-3.", statusCode: StatusCodes.Status501NotImplemented))
            .WithName("CreateOrder");

        app.MapGet("/orders/{id:guid}", (Guid id) =>
            Results.Problem("GET /orders/{id} sẽ được cài đặt ở tuần 2-3.", statusCode: StatusCodes.Status501NotImplemented))
            .WithName("GetOrderById");

        return app;
    }

    private static async Task<IResult> GetProducts(InventoryClient inventory, ILoggerFactory loggerFactory, CancellationToken ct)
    {
        var logger = loggerFactory.CreateLogger("OrderApi.Products");
        try
        {
            var products = await inventory.GetProductsAsync(ct);
            return Results.Ok(products.Select(p => new ProductDto(p.Id, p.Sku, p.Name, p.AvailableQuantity)));
        }
        catch (RpcException ex) when (ex.StatusCode is StatusCode.Unavailable or StatusCode.DeadlineExceeded)
        {
            logger.LogError(ex, "Không kết nối được Inventory Service");
            return Results.Problem("Inventory Service tạm thời không khả dụng.", statusCode: StatusCodes.Status503ServiceUnavailable);
        }
    }
}
