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
                    INNER JOIN NHAN_VIEN NV ON DP.MaNV = NV.MaNV
                    ORDER BY DP.MaDP DESC";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        // Lấy mã đặt phòng tiếp theo tự động
        public int GetNextMaDP()
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string sql = "SELECT ISNULL(MAX(MaDP), 0) + 1 FROM DAT_PHONG";
                using SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // Lấy danh sách khách hàng để đưa vào ComboBox
        public DataTable GetKhachHang()
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string sql = "SELECT MaKH, HoTen FROM KHACH_HANG ORDER BY HoTen";
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
                string sql = "SELECT MaNV, HoTen FROM NHAN_VIEN ORDER BY HoTen";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        // Lấy danh sách phòng còn trống để đặt
        public DataTable GetDanhSachPhongTrong()
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string sql = @"
                    SELECT P.MaPhong, P.TenPhong, L.TenLoai, ISNULL(P.GiaPhong, 0) AS GiaPhong
                    FROM PHONG P
                    LEFT JOIN LOAI_PHONG L ON P.MaLoai = L.MaLoai
                    WHERE P.TrangThai = N'Trống' OR P.TrangThai IS NULL
                    ORDER BY P.TenPhong";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        // Lấy chi tiết các phòng thuộc về 1 phiếu đặt phòng (Mô hình Master - Detail)
        public DataTable GetChiTietPhongTheoMaDP(int maDP)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string sql = @"
                    SELECT 
                        CT.MaCTDP, 
                        CT.MaDP, 
                        CT.MaPhong, 
                        P.TenPhong, 
                        ISNULL(L.TenLoai, N'Tiêu chuẩn') AS TenLoai, 
                        CT.DonGia
                    FROM CHI_TIET_DAT_PHONG CT
                    INNER JOIN PHONG P ON CT.MaPhong = P.MaPhong
                    LEFT JOIN LOAI_PHONG L ON P.MaLoai = L.MaLoai
                    WHERE CT.MaDP = @MaDP
                    ORDER BY CT.MaCTDP";

                using SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaDP", maDP);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        // Lập phiếu đặt phòng kèm nhiều phòng (Master - Detail trong 1 Transaction)
        public void ThemDatPhongKemChiTiet(
            int maDP,
            int maKH,
            int maNV,
            DateTime ngayDat,
            DateTime ngayNhan,
            DateTime ngayTra,
            string trangThai,
            DataTable dtPhongChon)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    // 1. Thêm Master DAT_PHONG
                    string sqlDP = @"
                        INSERT INTO DAT_PHONG
                        (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai)
                        VALUES
                        (@MaDP, @MaKH, @MaNV, @NgayDat, @NgayNhan, @NgayTra, @TrangThai)";

                    using (SqlCommand cmdDP = new SqlCommand(sqlDP, conn, tran))
                    {
                        cmdDP.Parameters.AddWithValue("@MaDP", maDP);
                        cmdDP.Parameters.AddWithValue("@MaKH", maKH);
                        cmdDP.Parameters.AddWithValue("@MaNV", maNV);
                        cmdDP.Parameters.AddWithValue("@NgayDat", ngayDat);
                        cmdDP.Parameters.AddWithValue("@NgayNhan", ngayNhan);
                        cmdDP.Parameters.AddWithValue("@NgayTra", ngayTra);
                        cmdDP.Parameters.AddWithValue("@TrangThai", trangThai);
                        cmdDP.ExecuteNonQuery();
                    }

                    // 2. Lấy Max MaCTDP hiện tại
                    int nextMaCTDP = 1;
                    using (SqlCommand cmdMax = new SqlCommand("SELECT ISNULL(MAX(MaCTDP), 0) FROM CHI_TIET_DAT_PHONG", conn, tran))
                    {
                        nextMaCTDP = Convert.ToInt32(cmdMax.ExecuteScalar()) + 1;
                    }

                    // 3. Thêm từng phòng Detail
                    foreach (DataRow row in dtPhongChon.Rows)
                    {
                        int maPhong = Convert.ToInt32(row["MaPhong"]);
                        decimal donGia = Convert.ToDecimal(row["DonGia"]);

                        string sqlCT = @"
                            INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia)
                            VALUES (@MaCTDP, @MaDP, @MaPhong, @DonGia)";

                        using (SqlCommand cmdCT = new SqlCommand(sqlCT, conn, tran))
                        {
                            cmdCT.Parameters.AddWithValue("@MaCTDP", nextMaCTDP++);
                            cmdCT.Parameters.AddWithValue("@MaDP", maDP);
                            cmdCT.Parameters.AddWithValue("@MaPhong", maPhong);
                            cmdCT.Parameters.AddWithValue("@DonGia", donGia);
                            cmdCT.ExecuteNonQuery();
                        }

                        // Cập nhật trạng thái phòng sang 'Đang sử dụng'
                        string sqlP = "UPDATE PHONG SET TrangThai = N'Đang sử dụng' WHERE MaPhong = @MaPhong";
                        using (SqlCommand cmdP = new SqlCommand(sqlP, conn, tran))
                        {
                            cmdP.Parameters.AddWithValue("@MaPhong", maPhong);
                            cmdP.ExecuteNonQuery();
                        }
                    }

                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        // Thêm đặt phòng đơn (tương thích code cũ)
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

        // Xóa đặt phòng (Kèm xóa chi tiết và trả lại trạng thái phòng Trống)
        public void XoaDatPhong(int maDP)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    // Hoàn lại trạng thái phòng 'Trống'
                    string sqlP = @"
                        UPDATE PHONG SET TrangThai = N'Trống'
                        WHERE MaPhong IN (SELECT MaPhong FROM CHI_TIET_DAT_PHONG WHERE MaDP = @MaDP)";
                    using (SqlCommand cmdP = new SqlCommand(sqlP, conn, tran))
                    {
                        cmdP.Parameters.AddWithValue("@MaDP", maDP);
                        cmdP.ExecuteNonQuery();
                    }

                    // Xóa chi tiết
                    string sqlCT = "DELETE FROM CHI_TIET_DAT_PHONG WHERE MaDP = @MaDP";
                    using (SqlCommand cmdCT = new SqlCommand(sqlCT, conn, tran))
                    {
                        cmdCT.Parameters.AddWithValue("@MaDP", maDP);
                        cmdCT.ExecuteNonQuery();
                    }

                    // Xóa master
                    string sqlDP = "DELETE FROM DAT_PHONG WHERE MaDP = @MaDP";
                    using (SqlCommand cmdDP = new SqlCommand(sqlDP, conn, tran))
                    {
                        cmdDP.Parameters.AddWithValue("@MaDP", maDP);
                        cmdDP.ExecuteNonQuery();
                    }

                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        // Tìm kiếm đặt phòng theo mã hoặc tên khách
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
                    OR KH.HoTen LIKE @TuKhoa
                    ORDER BY DP.MaDP DESC";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@TuKhoa", "%" + tuKhoa + "%");
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}
