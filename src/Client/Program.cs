using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

// Console client gọi Order API qua REST.
// Địa chỉ Order API lấy từ biến môi trường ORDER_API_URL (mặc định http://localhost:5020).
Console.OutputEncoding = Encoding.UTF8;

var baseUrl = Environment.GetEnvironmentVariable("ORDER_API_URL") ?? "http://localhost:5020";
using var http = new HttpClient { BaseAddress = new Uri(baseUrl), Timeout = TimeSpan.FromSeconds(10) };
var json = new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true };

Console.WriteLine($"=== Client - Hệ thống quản lý tồn kho đa dịch vụ ===");
Console.WriteLine($"Order API: {baseUrl}");

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1. Kiểm tra Order API (GET /health)");
    Console.WriteLine("2. Xem danh sách sản phẩm (GET /products)");
    Console.WriteLine("3. Tạo đơn hàng (POST /orders)        [tuần 2-3]");
    Console.WriteLine("4. Tra cứu đơn hàng (GET /orders/{id}) [tuần 2-3]");
    Console.WriteLine("0. Thoát");
    Console.Write("Chọn: ");

    var choice = Console.ReadLine()?.Trim();
    if (choice is null or "0") break;

    try
    {
        switch (choice)
        {
            case "1":
                await PrintAsync(await http.GetAsync("/health"));
                break;
            case "2":
                await PrintAsync(await http.GetAsync("/products"));
                break;
            case "3":
            case "4":
                Console.WriteLine("Chức năng sẽ được bổ sung ở các tuần tiếp theo.");
                break;
            default:
                Console.WriteLine("Lựa chọn không hợp lệ.");
                break;
        }
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"Không kết nối được Order API: {ex.Message}");
    }
    catch (TaskCanceledException)
    {
        Console.WriteLine("Hết thời gian chờ phản hồi từ Order API.");
    }
}

async Task PrintAsync(HttpResponseMessage response)
{
    Console.WriteLine($"HTTP {(int)response.StatusCode} {response.StatusCode}");
    var body = await response.Content.ReadAsStringAsync();
    if (string.IsNullOrWhiteSpace(body)) return;
    try
    {
        using var doc = JsonDocument.Parse(body);
        Console.WriteLine(JsonSerializer.Serialize(doc.RootElement, json));
    }
    catch (JsonException)
    {
        Console.WriteLine(body);
    }
}
