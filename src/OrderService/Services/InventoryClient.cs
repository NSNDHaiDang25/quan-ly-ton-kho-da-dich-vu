using InventorySystem.Contracts.Inventory;

namespace InventorySystem.OrderService.Services;

/// <summary>
/// Lớp bọc gRPC client gọi sang Inventory Service.
/// Tách riêng để các tuần sau bổ sung retry/timeout/xử lý lỗi tại một chỗ.
/// </summary>
public sealed class InventoryClient(InventoryGrpc.InventoryGrpcClient grpc, ILogger<InventoryClient> logger)
{
    public async Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken ct)
    {
        logger.LogInformation("Gọi gRPC Inventory.GetProducts");
        var reply = await grpc.GetProductsAsync(new GetProductsRequest(), cancellationToken: ct);
        logger.LogInformation("Inventory trả về {Count} sản phẩm", reply.Products.Count);
        return reply.Products;
    }
}
