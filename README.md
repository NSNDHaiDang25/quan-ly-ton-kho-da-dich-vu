# Hệ thống quản lý tồn kho đa dịch vụ

Bài tập lớn học phần **Phát triển ứng dụng phân tán** - Đề tài 13
Sinh viên: **Chu Hải Đăng** - MSSV: dtc245200879 - Lớp K23.K1.D2.2627.CNTT_K23L

Hệ thống gồm **Order Service** và **Inventory Service** tách biệt, mỗi service có cơ sở dữ liệu riêng. Khi tạo đơn hàng, Order Service gọi Inventory Service qua **gRPC** để kiểm tra và giữ số lượng hàng.

## Kiến trúc

```
Client (Console) --REST--> Order API ─┐
                                      │  Order Service ──gRPC──> Inventory Service ──> Inventory DB (PostgreSQL)
                                      └──────> Order DB (PostgreSQL)
```

| Thành phần | Công nghệ | Cổng (máy host) |
|---|---|---|
| `src/OrderService` | ASP.NET Core Minimal API (.NET 10), gRPC client | 5020 (REST) |
| `src/InventoryService` | ASP.NET Core gRPC server (.NET 10) | 5010 (HTTP/1 - health), 5011 (gRPC/HTTP2) |
| `src/Client` | Console App (.NET 10) | - |
| `inventory-db` | PostgreSQL 17 | 5433 |
| `order-db` | PostgreSQL 17 | 5434 |
| `protos/inventory.proto` | Hợp đồng gRPC dùng chung | - |

### API

REST (Order API):

| Method | Đường dẫn | Mô tả |
|---|---|---|
| `GET` | `/products` | Danh sách sản phẩm và tồn kho (lấy từ Inventory qua gRPC) |
| `POST` | `/orders` | Tạo đơn hàng |
| `GET` | `/orders/{id}` | Tra cứu trạng thái đơn hàng |
| `GET` | `/health` | Kiểm tra service |

gRPC (`inventory.v1.InventoryGrpc`): `GetProducts`, `CheckStock`, `ReserveStock`, `ReleaseStock`.

## Chạy hệ thống

Yêu cầu: Docker Desktop (hoặc Docker Engine + Compose v2). Để chạy không dùng Docker cần thêm .NET SDK 10.

### Bằng Docker Compose

```bash
cp .env.example .env        # tuỳ chọn, để đổi cổng/mật khẩu
docker compose up --build
```

Kiểm tra:

```bash
curl http://localhost:5020/health
curl http://localhost:5020/products
```

Chạy client:

```bash
dotnet run --project src/Client
# hoặc chỉ định địa chỉ Order API
ORDER_API_URL=http://localhost:5020 dotnet run --project src/Client
```

### Chạy trực tiếp bằng .NET SDK (để debug)

```bash
dotnet run --project src/InventoryService   # http://localhost:5010, gRPC http://localhost:5011
dotnet run --project src/OrderService       # http://localhost:5020
dotnet run --project src/Client
```

## Cấu hình

Mọi cấu hình phụ thuộc môi trường nằm ngoài mã nguồn: `appsettings.json` (giá trị mặc định khi chạy local) và biến môi trường trong `docker-compose.yml` / `.env`.

| Khoá | Ý nghĩa |
|---|---|
| `Services__InventoryGrpcUrl` | Địa chỉ gRPC của Inventory Service (trong Docker: `http://inventory-service:8081`) |
| `ConnectionStrings__InventoryDb` | Chuỗi kết nối Inventory DB |
| `ConnectionStrings__OrderDb` | Chuỗi kết nối Order DB |
| `ORDER_API_URL` | Địa chỉ Order API cho Client |

## Cấu trúc thư mục

```
.
├── protos/inventory.proto          # gRPC contract
├── src/
│   ├── InventoryService/           # gRPC server + Dockerfile
│   ├── OrderService/               # REST Minimal API + gRPC client + Dockerfile
│   └── Client/                     # Console client
├── docs/KE-HOACH-THEO-TUAN.md      # Tiến độ theo tuần
├── docker-compose.yml
└── InventorySystem.slnx
```

## Tiến độ

Xem [docs/KE-HOACH-THEO-TUAN.md](docs/KE-HOACH-THEO-TUAN.md). Hiện tại: **Tuần 1** - khung hệ thống, gRPC contract, Docker, `GET /products` chạy xuyên suốt qua gRPC.
