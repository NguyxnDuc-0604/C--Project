
using Microsoft.Data.SqlClient;
using System.Data;
using QuanLyKhachSan.Database;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.DAL
{
    public class ChiTietDatPhongDAL
    {
        // Lấy danh sách chi tiết đặt phòng
        public DataTable GetAll()
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = @"
                SELECT
                    CT.MaCTDP,
                    CT.MaDP,
                    CT.MaPhong,
                    P.TenPhong,
                    CT.DonGia
                FROM CHI_TIET_DAT_PHONG CT
                INNER JOIN PHONG P ON CT.MaPhong = P.MaPhong
                ORDER BY CT.MaCTDP";

            using SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        // Lấy danh sách mã đặt phòng
        public DataTable GetDanhSachDatPhong()
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = "SELECT MaDP FROM DAT_PHONG ORDER BY MaDP";

            using SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        // Lấy danh sách phòng
        public DataTable GetDanhSachPhong()
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = @"
                SELECT MaPhong, TenPhong
                FROM PHONG
                ORDER BY MaPhong";

            using SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        // Thêm chi tiết đặt phòng
        public void Them(ChiTietDatPhong ct)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            string sql = @"
                INSERT INTO CHI_TIET_DAT_PHONG
                (MaCTDP, MaDP, MaPhong, DonGia)
                VALUES
                (@MaCTDP, @MaDP, @MaPhong, @DonGia)";

            using SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@MaCTDP", SqlDbType.Int).Value = ct.MaCTDP;
            cmd.Parameters.Add("@MaDP", SqlDbType.Int).Value = ct.MaDP;
            cmd.Parameters.Add("@MaPhong", SqlDbType.Int).Value = ct.MaPhong;
            cmd.Parameters.Add("@DonGia", SqlDbType.Decimal).Value = ct.DonGia;

            cmd.ExecuteNonQuery();
        }

        // Sửa chi tiết đặt phòng
        public void Sua(ChiTietDatPhong ct)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            string sql = @"
                UPDATE CHI_TIET_DAT_PHONG
                SET MaDP = @MaDP,
                    MaPhong = @MaPhong,
                    DonGia = @DonGia
                WHERE MaCTDP = @MaCTDP";

            using SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@MaCTDP", SqlDbType.Int).Value = ct.MaCTDP;
            cmd.Parameters.Add("@MaDP", SqlDbType.Int).Value = ct.MaDP;
            cmd.Parameters.Add("@MaPhong", SqlDbType.Int).Value = ct.MaPhong;
            cmd.Parameters.Add("@DonGia", SqlDbType.Decimal).Value = ct.DonGia;

            cmd.ExecuteNonQuery();
        }

        // Xóa chi tiết đặt phòng
        public void Xoa(int maCTDP)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            string sql = @"
                DELETE FROM CHI_TIET_DAT_PHONG
                WHERE MaCTDP = @MaCTDP";

            using SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@MaCTDP", SqlDbType.Int).Value = maCTDP;

            cmd.ExecuteNonQuery();
        }

        // Tìm kiếm chi tiết đặt phòng
        public DataTable TimKiem(string tuKhoa)
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = @"
                SELECT
                    CT.MaCTDP,
                    CT.MaDP,
                    CT.MaPhong,
                    P.TenPhong,
                    CT.DonGia
                FROM CHI_TIET_DAT_PHONG CT
                INNER JOIN PHONG P ON CT.MaPhong = P.MaPhong
                WHERE CAST(CT.MaCTDP AS NVARCHAR(20)) LIKE @TuKhoa
                   OR CAST(CT.MaDP AS NVARCHAR(20)) LIKE @TuKhoa
                   OR P.TenPhong LIKE @TuKhoa
                ORDER BY CT.MaCTDP";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@TuKhoa", SqlDbType.NVarChar, 100)
                .Value = "%" + tuKhoa + "%";

            using SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        // Tính tổng tiền theo phiếu đặt phòng
        public decimal TinhTongTien(int maDP)
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = @"
                SELECT ISNULL(SUM(DonGia), 0)
                FROM CHI_TIET_DAT_PHONG
                WHERE MaDP = @MaDP";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaDP", SqlDbType.Int).Value = maDP;

            conn.Open();

            return Convert.ToDecimal(cmd.ExecuteScalar());
        }
    }
}
