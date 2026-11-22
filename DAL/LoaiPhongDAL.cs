using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyKhachSan.Database;

namespace QuanLyKhachSan.DAL
{
    public class LoaiPhongDAL
    {
        public DataTable GetAll() => Query("SELECT MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong FROM LOAI_PHONG");

        public DataTable TimKiem(string tuKhoa)
        {
            using var conn = DBConnection.GetConnection();
            using var cmd = new SqlCommand("SELECT MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong FROM LOAI_PHONG WHERE TenLoai LIKE @q OR CAST(MaLoai AS NVARCHAR(20)) LIKE @q", conn);
            cmd.Parameters.AddWithValue("@q", "%" + tuKhoa + "%");
            using var adapter = new SqlDataAdapter(cmd);
            var dt = new DataTable(); adapter.Fill(dt); return dt;
        }

        private static DataTable Query(string sql)
        {
            using var conn = DBConnection.GetConnection(); using var adapter = new SqlDataAdapter(sql, conn);
            var dt = new DataTable(); adapter.Fill(dt); return dt;
        }

        public void Them(int ma, string ten, decimal gia, int soNguoi, string moTa, int soLuong) => Execute(
            "INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (@ma,@ten,@gia,@nguoi,@mota,@sl)", ma, ten, gia, soNguoi, moTa, soLuong);
        public void Sua(int ma, string ten, decimal gia, int soNguoi, string moTa, int soLuong) => Execute(
            "UPDATE LOAI_PHONG SET TenLoai=@ten, GiaPhong=@gia, SoNguoiToiDa=@nguoi, MoTa=@mota, SoLuongPhong=@sl WHERE MaLoai=@ma", ma, ten, gia, soNguoi, moTa, soLuong);
        public void Xoa(int ma)
        {
            using var conn = DBConnection.GetConnection(); using var cmd = new SqlCommand("DELETE FROM LOAI_PHONG WHERE MaLoai=@ma", conn);
            cmd.Parameters.AddWithValue("@ma", ma); conn.Open(); cmd.ExecuteNonQuery();
        }
        private static void Execute(string sql, int ma, string ten, decimal gia, int nguoi, string moTa, int sl)
        {
            using var conn = DBConnection.GetConnection(); using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ma", ma); cmd.Parameters.AddWithValue("@ten", ten); cmd.Parameters.AddWithValue("@gia", gia);
            cmd.Parameters.AddWithValue("@nguoi", nguoi); cmd.Parameters.AddWithValue("@mota", string.IsNullOrWhiteSpace(moTa) ? DBNull.Value : moTa);
            cmd.Parameters.AddWithValue("@sl", sl); conn.Open(); cmd.ExecuteNonQuery();
        }
    }
}
