
using Microsoft.Data.SqlClient;

namespace QuanLyKhachSan.Database
{
    public class DBConnection
    {
        private static readonly string connectionString =
            Environment.GetEnvironmentVariable("QUANLYKHACHSAN_CONNECTION_STRING")
            ?? @"Server=localhost\SQLEXPRESS;
                 Database=QuanLyKhachSan;
                 Trusted_Connection=True;
                 TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
