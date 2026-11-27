using Microsoft.Data.SqlClient;

namespace QuanLyKhachSan.Database
{
    /// <summary>
    /// Lớp MyConnection chuẩn theo yêu cầu Mô hình 3-Layer ver2 (Đồ án C# Tuần 7)
    /// </summary>
    public class MyConnection
    {
        public static SqlConnection GetConnection()
        {
            return DBConnection.GetConnection();
        }

        public static string GetConnectionString()
        {
            return Environment.GetEnvironmentVariable("QUANLYKHACHSAN_CONNECTION_STRING")
                ?? @"Server=localhost\SQLEXPRESS;Database=QuanLyKhachSan;Trusted_Connection=True;TrustServerCertificate=True";
        }
    }
}
