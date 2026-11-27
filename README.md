# Quản lý khách sạn

Ứng dụng desktop quản lý khách sạn viết bằng C# Windows Forms, .NET 10 và SQL Server.

## Yêu cầu

- Windows 10/11 x64.
- .NET 10 SDK và Windows Desktop Runtime.
- SQL Server Database Engine; SQL Server Express phù hợp cho học tập.
- SQL Server Management Studio (SSMS) để quản lý database.

## Cấu hình kết nối

Mặc định, `Database/DBConnection.cs` dùng Windows Authentication để kết nối:

```text
Server=localhost\SQLEXPRESS;Database=QuanLyKhachSan;Trusted_Connection=True;TrustServerCertificate=True
```

Nếu instance SQL Server có tên khác, đặt biến môi trường `QUANLYKHACHSAN_CONNECTION_STRING` trước khi chạy ứng dụng. Ví dụ PowerShell:

```powershell
$env:QUANLYKHACHSAN_CONNECTION_STRING = "Server=localhost;Database=QuanLyKhachSan;Trusted_Connection=True;TrustServerCertificate=True"
dotnet run --project QuanLyKhachSan.csproj
```

## Tạo database và dữ liệu mẫu

Trong SSMS, kết nối tới SQL Server rồi chạy:

1. `Database/SetupDatabase.sql` — tạo database nếu chưa có, tạo các bảng còn thiếu và cài trigger tồn kho. Script không xóa bảng hay dữ liệu.
2. `Database/SeedSampleData.sql` — thêm dữ liệu mẫu; các mã đã tồn tại sẽ được bỏ qua.

Để xem bảng: **Object Explorer → Databases → QuanLyKhachSan → Tables**. Nhấp phải một bảng → **Select Top 1000 Rows** để xem dữ liệu.

`CreateAllTables.sql` và `CreateTables_Khoa.sql` là script cũ, schema không đồng bộ với DAL; không dùng để dựng database mới. `NhapHang.sql` chỉ cài lại trigger tồn kho và không drop bảng/dữ liệu.

## Chạy ứng dụng

Mở terminal tại thư mục chứa file `.csproj`, sau đó chạy:

```powershell
dotnet restore
dotnet run --project QuanLyKhachSan.csproj
```

Hoặc mở `QuanLyKhachSan.csproj` trong Visual Studio và nhấn **F5**. Ứng dụng Windows Forms chỉ chạy trên Windows.

## Cấu trúc

```text
Forms / GUI  →  BUS  →  DAL  →  SQL Server
                  ↕
                Models
```

- `Forms`, `GUI`: giao diện Windows Forms.
- `BUS`: xác thực dữ liệu và điều phối nghiệp vụ.
- `DAL`: truy vấn SQL Server qua `Microsoft.Data.SqlClient`.
- `Models`: các đối tượng dữ liệu.
- `Database`: cấu hình kết nối và script SQL.

## Ghi chú bảo mật

- Mật khẩu nhân viên hiện vẫn lưu dạng văn bản trong schema cũ; không dùng dữ liệu mẫu hoặc mật khẩu mẫu cho tài khoản thật.
- Smart App Control của Windows có thể chặn file build chưa ký số trên máy phát triển.
