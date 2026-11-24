using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyKhachSan.Database;

namespace QuanLyKhachSan.DAL
{
    public class NhaCungCapDAL
    {
        public DataTable GetAll()
        {
            using SqlConnection conn = DBConnection.GetConnection();
            using SqlDataAdapter adapter = new SqlDataAdapter("SELECT MaNCC, TenNCC, SDT, DiaChi, Email FROM NHA_CUNG_CAP ORDER BY MaNCC", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable GetDanhSach()
        {
            using SqlConnection conn = DBConnection.GetConnection();
            using SqlDataAdapter adapter = new SqlDataAdapter("SELECT MaNCC, TenNCC FROM NHA_CUNG_CAP ORDER BY TenNCC", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public void Them(string tenNCC, string sdt, string diaChi, string email)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            string sql = @"
                INSERT INTO NHA_CUNG_CAP (TenNCC, SDT, DiaChi, Email)
                VALUES (@TenNCC, @SDT, @DiaChi, @Email)";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@TenNCC", tenNCC);
            cmd.Parameters.AddWithValue("@SDT",    string.IsNullOrWhiteSpace(sdt)    ? DBNull.Value : sdt);
            cmd.Parameters.AddWithValue("@DiaChi", string.IsNullOrWhiteSpace(diaChi) ? DBNull.Value : diaChi);
            cmd.Parameters.AddWithValue("@Email",  string.IsNullOrWhiteSpace(email)  ? DBNull.Value : email);
            cmd.ExecuteNonQuery();
        }

        public void Sua(int maNCC, string tenNCC, string sdt, string diaChi, string email)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            string sql = @"
                UPDATE NHA_CUNG_CAP SET
                    TenNCC = @TenNCC,
                    SDT    = @SDT,
                    DiaChi = @DiaChi,
                    Email  = @Email
                WHERE MaNCC = @MaNCC";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@MaNCC",  maNCC);
            cmd.Parameters.AddWithValue("@TenNCC", tenNCC);
            cmd.Parameters.AddWithValue("@SDT",    string.IsNullOrWhiteSpace(sdt)    ? DBNull.Value : sdt);
            cmd.Parameters.AddWithValue("@DiaChi", string.IsNullOrWhiteSpace(diaChi) ? DBNull.Value : diaChi);
            cmd.Parameters.AddWithValue("@Email",  string.IsNullOrWhiteSpace(email)  ? DBNull.Value : email);
            cmd.ExecuteNonQuery();
        }

        public void Xoa(int maNCC)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            using SqlCommand cmd = new SqlCommand("DELETE FROM NHA_CUNG_CAP WHERE MaNCC = @MaNCC", conn);
            cmd.Parameters.AddWithValue("@MaNCC", maNCC);
            cmd.ExecuteNonQuery();
        }

        public DataTable TimKiem(string tuKhoa)
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = @"
                SELECT MaNCC, TenNCC, SDT, DiaChi, Email
                FROM NHA_CUNG_CAP
                WHERE TenNCC LIKE @q
                   OR SDT    LIKE @q
                   OR Email  LIKE @q
                   OR CAST(MaNCC AS NVARCHAR(20)) LIKE @q
                ORDER BY MaNCC";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@q", "%" + tuKhoa + "%");
            using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }
    }
}
