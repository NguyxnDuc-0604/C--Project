
using Microsoft.Data.SqlClient;

namespace QuanLyKhachSan.Database
{
    public class DBConnection
    {
        private static string connectionString =
            @"Server=localhost\SQLEXPRESS;
              Database=QuanLyKhachSan;
              Trusted_Connection=True;
              TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
