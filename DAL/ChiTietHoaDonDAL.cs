
using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyKhachSan.Database;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.DAL
{
    public class ChiTietHoaDonDAL
    {
        // Lấy danh sách chi tiết hóa đơn
        public DataTable GetAll()
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = @"
                SELECT
                    CT.MaCTHD,
                    CT.MaHD,
                    CT.MaDV,
                    DV.TenDV,
                    CT.SoLuong,
                    CT.DonGia,
                    CT.ThanhTien
                FROM CHI_TIET_HOA_DON CT
                INNER JOIN DICH_VU DV ON CT.MaDV = DV.MaDV
                ORDER BY CT.MaCTHD";

            using SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        // Lấy chi tiết theo mã hóa đơn
        public DataTable GetByMaHD(int maHD)
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = @"
                SELECT
                    CT.MaCTHD,
                    CT.MaHD,
                    CT.MaDV,
                    DV.TenDV,
                    CT.SoLuong,
                    CT.DonGia,
                    CT.ThanhTien
                FROM CHI_TIET_HOA_DON CT
                INNER JOIN DICH_VU DV ON CT.MaDV = DV.MaDV
                WHERE CT.MaHD = @MaHD
                ORDER BY CT.MaCTHD";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaHD", SqlDbType.Int).Value = maHD;

            using SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        // Lấy danh sách mã hóa đơn cho ComboBox
        public DataTable GetDanhSachHoaDon()
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = "SELECT MaHD FROM HOA_DON ORDER BY MaHD";

            using SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        // Lấy danh sách dịch vụ cho ComboBox
        public DataTable GetDanhSachDichVu()
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = "SELECT MaDV, TenDV, DonGia FROM DICH_VU ORDER BY MaDV";

            using SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        // Thêm chi tiết hóa đơn
        public void Them(ChiTietHoaDon ct)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            string sql = @"
                INSERT INTO CHI_TIET_HOA_DON
                (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien)
                VALUES
                (@MaCTHD, @MaHD, @MaDV, @SoLuong, @DonGia, @ThanhTien)";

            using SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@MaCTHD", SqlDbType.Int).Value = ct.MaCTHD;
            cmd.Parameters.Add("@MaHD", SqlDbType.Int).Value = ct.MaHD;
            cmd.Parameters.Add("@MaDV", SqlDbType.Int).Value = ct.MaDV;
            cmd.Parameters.Add("@SoLuong", SqlDbType.Int).Value = ct.SoLuong;
            cmd.Parameters.Add("@DonGia", SqlDbType.Decimal).Value = ct.DonGia;
            cmd.Parameters.Add("@ThanhTien", SqlDbType.Decimal).Value = ct.ThanhTien;

            cmd.ExecuteNonQuery();
        }

        // Sửa chi tiết hóa đơn
        public void Sua(ChiTietHoaDon ct)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            string sql = @"
                UPDATE CHI_TIET_HOA_DON SET
                    MaHD = @MaHD,
                    MaDV = @MaDV,
                    SoLuong = @SoLuong,
                    DonGia = @DonGia,
                    ThanhTien = @ThanhTien
                WHERE MaCTHD = @MaCTHD";

            using SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@MaCTHD", SqlDbType.Int).Value = ct.MaCTHD;
            cmd.Parameters.Add("@MaHD", SqlDbType.Int).Value = ct.MaHD;
            cmd.Parameters.Add("@MaDV", SqlDbType.Int).Value = ct.MaDV;
            cmd.Parameters.Add("@SoLuong", SqlDbType.Int).Value = ct.SoLuong;
            cmd.Parameters.Add("@DonGia", SqlDbType.Decimal).Value = ct.DonGia;
            cmd.Parameters.Add("@ThanhTien", SqlDbType.Decimal).Value = ct.ThanhTien;

            cmd.ExecuteNonQuery();
        }

        // Xóa chi tiết hóa đơn
        public void Xoa(int maCTHD)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            string sql = "DELETE FROM CHI_TIET_HOA_DON WHERE MaCTHD = @MaCTHD";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaCTHD", SqlDbType.Int).Value = maCTHD;

            cmd.ExecuteNonQuery();
        }

        // Tìm kiếm chi tiết hóa đơn
        public DataTable TimKiem(string tuKhoa)
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = @"
                SELECT
                    CT.MaCTHD,
                    CT.MaHD,
                    CT.MaDV,
                    DV.TenDV,
                    CT.SoLuong,
                    CT.DonGia,
                    CT.ThanhTien
                FROM CHI_TIET_HOA_DON CT
                INNER JOIN DICH_VU DV ON CT.MaDV = DV.MaDV
                WHERE CAST(CT.MaCTHD AS NVARCHAR(20)) LIKE @TuKhoa
                   OR CAST(CT.MaHD AS NVARCHAR(20)) LIKE @TuKhoa
                   OR DV.TenDV LIKE @TuKhoa
                ORDER BY CT.MaCTHD";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@TuKhoa", SqlDbType.NVarChar, 100)
                .Value = "%" + tuKhoa + "%";

            using SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        // Tính tổng tiền theo hóa đơn
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
