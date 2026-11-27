
using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyKhachSan.Database;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.DAL
{
    public class HoaDonDAL
    {
        // Lấy danh sách hóa đơn (JOIN để hiện tên khách hàng, nhân viên)
        public DataTable GetAll()
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = @"
                SELECT
                    HD.MaHD,
                    HD.MaDP,
                    DP.MaKH,
                    KH.HoTen AS TenKhachHang,
                    HD.MaNV,
                    NV.HoTen AS TenNhanVien,
                    HD.NgayLap,
                    HD.TongTien,
                    HD.GhiChu
                FROM HOA_DON HD
                INNER JOIN DAT_PHONG DP ON HD.MaDP = DP.MaDP
                INNER JOIN KHACH_HANG KH ON DP.MaKH = KH.MaKH
                INNER JOIN NHAN_VIEN NV ON HD.MaNV = NV.MaNV
                ORDER BY HD.MaHD";

            using SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        // Lấy danh sách mã đặt phòng cho ComboBox
        public DataTable GetDanhSachDatPhong()
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = "SELECT MaDP FROM DAT_PHONG ORDER BY MaDP";

            using SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        // Lấy danh sách nhân viên cho ComboBox
        public DataTable GetDanhSachNhanVien()
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = "SELECT MaNV, HoTen FROM NHAN_VIEN ORDER BY MaNV";

            using SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        // Thêm hóa đơn
        public void Them(HoaDon hd)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            string sql = @"
                INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu)
                VALUES (@MaHD, @MaDP, @MaNV, @NgayLap, @TongTien, @GhiChu)";

            using SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@MaHD", SqlDbType.Int).Value = hd.MaHD;
            cmd.Parameters.Add("@MaDP", SqlDbType.Int).Value = hd.MaDP;
            cmd.Parameters.Add("@MaNV", SqlDbType.Int).Value = hd.MaNV;
            cmd.Parameters.Add("@NgayLap", SqlDbType.DateTime).Value = hd.NgayLap;
            cmd.Parameters.Add("@TongTien", SqlDbType.Decimal).Value = hd.TongTien;
            cmd.Parameters.Add("@GhiChu", SqlDbType.NVarChar, 200).Value =
                (object?)hd.GhiChu ?? DBNull.Value;

            cmd.ExecuteNonQuery();
        }

        // Sửa hóa đơn
        public void Sua(HoaDon hd)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            string sql = @"
                UPDATE HOA_DON SET
                    MaDP = @MaDP,
                    MaNV = @MaNV,
                    NgayLap = @NgayLap,
                    TongTien = @TongTien,
                    GhiChu = @GhiChu
                WHERE MaHD = @MaHD";

            using SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@MaHD", SqlDbType.Int).Value = hd.MaHD;
            cmd.Parameters.Add("@MaDP", SqlDbType.Int).Value = hd.MaDP;
            cmd.Parameters.Add("@MaNV", SqlDbType.Int).Value = hd.MaNV;
            cmd.Parameters.Add("@NgayLap", SqlDbType.DateTime).Value = hd.NgayLap;
            cmd.Parameters.Add("@TongTien", SqlDbType.Decimal).Value = hd.TongTien;
            cmd.Parameters.Add("@GhiChu", SqlDbType.NVarChar, 200).Value =
                (object?)hd.GhiChu ?? DBNull.Value;

            cmd.ExecuteNonQuery();
        }

        // Xóa hóa đơn
        public void Xoa(int maHD)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            string sql = "DELETE FROM HOA_DON WHERE MaHD = @MaHD";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaHD", SqlDbType.Int).Value = maHD;

            cmd.ExecuteNonQuery();
        }

        // Tìm kiếm hóa đơn
        public DataTable TimKiem(string tuKhoa)
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = @"
                SELECT
                    HD.MaHD,
                    HD.MaDP,
                    DP.MaKH,
                    KH.HoTen AS TenKhachHang,
                    HD.MaNV,
                    NV.HoTen AS TenNhanVien,
                    HD.NgayLap,
                    HD.TongTien,
                    HD.GhiChu
                FROM HOA_DON HD
                INNER JOIN DAT_PHONG DP ON HD.MaDP = DP.MaDP
                INNER JOIN KHACH_HANG KH ON DP.MaKH = KH.MaKH
                INNER JOIN NHAN_VIEN NV ON HD.MaNV = NV.MaNV
                WHERE CAST(HD.MaHD AS NVARCHAR(20)) LIKE @TuKhoa
                   OR KH.HoTen LIKE @TuKhoa
                   OR NV.HoTen LIKE @TuKhoa
                ORDER BY HD.MaHD";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@TuKhoa", SqlDbType.NVarChar, 100)
                .Value = "%" + tuKhoa + "%";

            using SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        // Tính tổng tiền hóa đơn từ chi tiết hóa đơn
        public decimal TinhTongTien(int maHD)
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = @"
                SELECT ISNULL(SUM(ThanhTien), 0)
                FROM CHI_TIET_HOA_DON
                WHERE MaHD = @MaHD";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaHD", SqlDbType.Int).Value = maHD;

            conn.Open();

            return Convert.ToDecimal(cmd.ExecuteScalar());
        }
    }
}
