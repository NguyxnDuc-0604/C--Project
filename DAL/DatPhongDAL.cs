
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyKhachSan.Database;

namespace QuanLyKhachSan.DAL
{
    public class DatPhongDAL
    {
        // Lấy danh sách đặt phòng
        public DataTable GetAll()
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string sql = @"
                    SELECT
                        DP.MaDP,
                        DP.MaKH,
                        KH.HoTen AS TenKhachHang,
                        DP.MaNV,
                        NV.HoTen AS TenNhanVien,
                        DP.NgayDat,
                        DP.NgayNhan,
                        DP.NgayTra,
                        DP.TrangThai
                    FROM DAT_PHONG DP
                    INNER JOIN KHACH_HANG KH ON DP.MaKH = KH.MaKH
                    INNER JOIN NHAN_VIEN NV ON DP.MaNV = NV.MaNV";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                return dt;
            }
        }

        // Lấy danh sách khách hàng để đưa vào ComboBox
        public DataTable GetKhachHang()
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string sql = "SELECT MaKH, HoTen FROM KHACH_HANG";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                return dt;
            }
        }

        // Lấy danh sách nhân viên để đưa vào ComboBox
        public DataTable GetNhanVien()
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string sql = "SELECT MaNV, HoTen FROM NHAN_VIEN";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                return dt;
            }
        }

        // Thêm đặt phòng
        public void ThemDatPhong(
            int maDP,
            int maKH,
            int maNV,
            DateTime ngayDat,
            DateTime ngayNhan,
            DateTime ngayTra,
            string trangThai)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string sql = @"
                    INSERT INTO DAT_PHONG
                    (MaDP, MaKH, MaNV, NgayDat,
                     NgayNhan, NgayTra, TrangThai)
                    VALUES
                    (@MaDP, @MaKH, @MaNV, @NgayDat,
                     @NgayNhan, @NgayTra, @TrangThai)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@MaDP", maDP);
                cmd.Parameters.AddWithValue("@MaKH", maKH);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@NgayDat", ngayDat);
                cmd.Parameters.AddWithValue("@NgayNhan", ngayNhan);
                cmd.Parameters.AddWithValue("@NgayTra", ngayTra);
                cmd.Parameters.AddWithValue("@TrangThai", trangThai);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Sửa đặt phòng
        public void SuaDatPhong(
            int maDP,
            int maKH,
            int maNV,
            DateTime ngayDat,
            DateTime ngayNhan,
            DateTime ngayTra,
            string trangThai)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string sql = @"
                    UPDATE DAT_PHONG SET
                        MaKH = @MaKH,
                        MaNV = @MaNV,
                        NgayDat = @NgayDat,
                        NgayNhan = @NgayNhan,
                        NgayTra = @NgayTra,
                        TrangThai = @TrangThai
                    WHERE MaDP = @MaDP";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@MaDP", maDP);
                cmd.Parameters.AddWithValue("@MaKH", maKH);
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@NgayDat", ngayDat);
                cmd.Parameters.AddWithValue("@NgayNhan", ngayNhan);
                cmd.Parameters.AddWithValue("@NgayTra", ngayTra);
                cmd.Parameters.AddWithValue("@TrangThai", trangThai);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Xóa đặt phòng
        public void XoaDatPhong(int maDP)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string sql = "DELETE FROM DAT_PHONG WHERE MaDP = @MaDP";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaDP", maDP);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Tìm kiếm đặt phòng theo mã
        public DataTable TimKiemDatPhong(string tuKhoa)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string sql = @"
                    SELECT
                        DP.MaDP,
                        DP.MaKH,
                        KH.HoTen AS TenKhachHang,
                        DP.MaNV,
                        NV.HoTen AS TenNhanVien,
                        DP.NgayDat,
                        DP.NgayNhan,
                        DP.NgayTra,
                        DP.TrangThai
                    FROM DAT_PHONG DP
                    INNER JOIN KHACH_HANG KH ON DP.MaKH = KH.MaKH
                    INNER JOIN NHAN_VIEN NV ON DP.MaNV = NV.MaNV
                    WHERE CAST(DP.MaDP AS NVARCHAR(20)) LIKE @TuKhoa
                    OR KH.HoTen LIKE @TuKhoa";

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
