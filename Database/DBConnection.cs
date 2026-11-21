
using Microsoft.Data.SqlClient;

namespace QuanLyKhachSan.Database
{
    public class DBConnection
    {
        private static string connectionString =
            @"Server=localhost\MSSQLSERVER01;
              Database=QuanLyKhachSan;
              Trusted_Connection=True;
              TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
