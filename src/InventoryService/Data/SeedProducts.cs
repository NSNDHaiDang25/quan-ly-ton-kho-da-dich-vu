namespace InventorySystem.InventoryService.Data;

/// <summary>
/// Dữ liệu mẫu tạm thời của tuần 1, dùng để kiểm tra đường gRPC thông suốt.
/// Tuần 2 sẽ thay bằng Inventory DB (PostgreSQL + EF Core).
/// </summary>
public static class SeedProducts
{
    public sealed record ProductItem(string Id, string Sku, string Name, int AvailableQuantity, int ReservedQuantity);

    public static readonly IReadOnlyList<ProductItem> Items =
    [
        new("P001", "SKU-KB-01", "Bàn phím cơ", 50, 0),
        new("P002", "SKU-MS-01", "Chuột không dây", 120, 0),
        new("P003", "SKU-MN-24", "Màn hình 24 inch", 15, 0),
        new("P004", "SKU-HP-01", "Tai nghe", 0, 0),
    ];
}
