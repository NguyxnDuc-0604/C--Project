
using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyKhachSan.Database;

namespace QuanLyKhachSan.DAL
{
    public class KhachHangDAL
    {
        // Lấy danh sách khách hàng
        public DataTable GetAll()
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string sql = "SELECT * FROM KHACH_HANG";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                return dt;
            }
        }
        // Thêm khách hàng
        public void ThemKhachHang(
            int maKH, string hoTen, string cccd,
            string sdt, string email, string diaChi)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string sql = @"INSERT INTO KHACH_HANG
                (MaKH, HoTen, CCCD, SDT, Email, DiaChi)
                VALUES (@MaKH, @HoTen, @CCCD, @SDT, @Email, @DiaChi)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@MaKH", maKH);
                cmd.Parameters.AddWithValue("@HoTen", hoTen);
                cmd.Parameters.AddWithValue("@CCCD", (object?)cccd ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@SDT", (object?)sdt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object?)email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DiaChi", (object?)diaChi ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Sửa khách hàng
        public void SuaKhachHang(
            int maKH, string hoTen, string cccd,
            string sdt, string email, string diaChi)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string sql = @"UPDATE KHACH_HANG SET
                    HoTen = @HoTen,
                    CCCD = @CCCD,
                    SDT = @SDT,
                    Email = @Email,
                    DiaChi = @DiaChi
                    WHERE MaKH = @MaKH";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@MaKH", maKH);
                cmd.Parameters.AddWithValue("@HoTen", hoTen);
                cmd.Parameters.AddWithValue("@CCCD", (object?)cccd ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@SDT", (object?)sdt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object?)email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DiaChi", (object?)diaChi ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Xóa khách hàng
        public void XoaKhachHang(int maKH)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string sql = "DELETE FROM KHACH_HANG WHERE MaKH = @MaKH";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaKH", maKH);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Tìm kiếm khách hàng
        public DataTable TimKiemKhachHang(string tuKhoa)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string sql = @"SELECT * FROM KHACH_HANG
                    WHERE HoTen LIKE @TuKhoa
                    OR SDT LIKE @TuKhoa";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                adapter.SelectCommand.Parameters.AddWithValue(
                    "@TuKhoa", "%" + tuKhoa + "%");

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                return dt;
            }
        }

        }
    }

