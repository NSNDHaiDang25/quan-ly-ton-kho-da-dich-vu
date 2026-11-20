# Kế hoạch thực hiện theo tuần

Đề tài 13 - Hệ thống quản lý tồn kho đa dịch vụ
Sinh viên: Chu Hải Đăng (dtc245200879)

| Tuần | Nội dung | Trạng thái |
|---|---|---|
| 1 | Khởi tạo repo, cấu trúc solution .NET 10; định nghĩa gRPC contract `inventory.proto`; khung Inventory Service (gRPC server), Order Service (REST Minimal API + gRPC client), Console Client; Dockerfile từng service, `docker-compose.yml` (2 PostgreSQL tách biệt); CI build trên GitHub Actions. `GET /products` chạy xuyên suốt Client → Order API → gRPC → Inventory (dữ liệu mẫu). | Đã xong |
| 2 | Inventory DB: EF Core + Npgsql, bảng `Products`, `StockReservations`, migration + seed. Cài đặt `GetProducts`, `CheckStock` đọc từ DB. | |
| 3 | Order DB: bảng `Orders`, `OrderItems`, trạng thái (`Pending`, `Confirmed`, `Rejected`, `Failed`). Cài đặt `POST /orders`, `GET /orders/{id}`. Gọi `ReserveStock` khi tạo đơn; từ chối đơn khi không đủ hàng. | |
| 4 | Giữ/trừ tồn kho an toàn khi nhiều đơn đồng thời (transaction + optimistic concurrency); `ReserveStock` idempotent theo `order_id`; `ReleaseStock` khi xử lý lỗi. | |
| 5 | Khả năng chịu lỗi: timeout/deadline, retry gRPC, xử lý khi Inventory Service tạm dừng (đơn chuyển `Failed`/thử lại); logging theo luồng xử lý. | |
| 6 | Hoàn thiện Client (tạo đơn, tra cứu đơn, xem tồn kho); kịch bản demo; kiểm thử. | |
| 7 | Mở rộng (tuỳ chọn): Correlation ID, health check, structured logging. Viết báo cáo, sơ đồ kiến trúc, hoàn thiện README. | |
