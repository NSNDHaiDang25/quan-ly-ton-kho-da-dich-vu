using Grpc.Core;
using InventorySystem.Contracts.Inventory;
using InventorySystem.InventoryService.Data;

namespace InventorySystem.InventoryService.Services;

/// <summary>
/// Cài đặt gRPC server cho Inventory Service.
/// Tuần 1: GetProducts trả dữ liệu mẫu; các thao tác kiểm tra/giữ/hoàn tồn kho sẽ làm ở tuần 2-3.
/// </summary>
public sealed class InventoryGrpcService(ILogger<InventoryGrpcService> logger) : InventoryGrpc.InventoryGrpcBase
{
    public override Task<GetProductsReply> GetProducts(GetProductsRequest request, ServerCallContext context)
    {
        logger.LogInformation("gRPC GetProducts từ {Peer}", context.Peer);

        var reply = new GetProductsReply();
        reply.Products.AddRange(SeedProducts.Items.Select(p => new Product
        {
            Id = p.Id,
            Sku = p.Sku,
            Name = p.Name,
            AvailableQuantity = p.AvailableQuantity,
            ReservedQuantity = p.ReservedQuantity,
        }));

        return Task.FromResult(reply);
    }

    public override Task<CheckStockReply> CheckStock(CheckStockRequest request, ServerCallContext context)
        => throw NotYet(nameof(CheckStock));

    public override Task<ReserveStockReply> ReserveStock(ReserveStockRequest request, ServerCallContext context)
        => throw NotYet(nameof(ReserveStock));

    public override Task<ReleaseStockReply> ReleaseStock(ReleaseStockRequest request, ServerCallContext context)
        => throw NotYet(nameof(ReleaseStock));

    private RpcException NotYet(string method)
    {
        logger.LogWarning("gRPC {Method} chưa được cài đặt", method);
        return new RpcException(new Status(StatusCode.Unimplemented, $"{method} sẽ được cài đặt ở các tuần tiếp theo."));
    }
}
