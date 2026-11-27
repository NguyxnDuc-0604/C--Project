using System;
using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyKhachSan.Database;

namespace QuanLyKhachSan.DAL
{
    public class PhieuNhapDAL
    {
        // Lấy toàn bộ danh sách phiếu nhập
        public DataTable GetAll()
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = @"
                SELECT
                    PN.MaPN,
                    PN.MaNCC,
                    NCC.TenNCC,
                    PN.MaNV,
                    NV.HoTen AS TenNhanVien,
                    PN.NgayNhap,
                    ISNULL((SELECT SUM(CT.SoLuong * CT.DonGiaNhap) FROM CT_PHIEU_NHAP CT WHERE CT.MaPN = PN.MaPN), 0) AS TongTien
                FROM PHIEU_NHAP PN
                INNER JOIN NHA_CUNG_CAP NCC ON PN.MaNCC = NCC.MaNCC
                INNER JOIN NHAN_VIEN    NV  ON PN.MaNV  = NV.MaNV
                ORDER BY PN.MaPN DESC";

            using SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        // Lấy danh sách nhà cung cấp cho ComboBox
        public DataTable GetNhaCungCap()
        {
            using SqlConnection conn = DBConnection.GetConnection();
            using SqlDataAdapter adapter = new SqlDataAdapter("SELECT MaNCC, TenNCC FROM NHA_CUNG_CAP ORDER BY TenNCC", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        // Lấy danh sách nhân viên cho ComboBox
        public DataTable GetNhanVien()
        {
            using SqlConnection conn = DBConnection.GetConnection();
            using SqlDataAdapter adapter = new SqlDataAdapter("SELECT MaNV, HoTen FROM NHAN_VIEN ORDER BY HoTen", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        // Lấy danh sách dịch vụ / mặt hàng cho ComboBox
        public DataTable GetDanhSachDichVu()
        {
            using SqlConnection conn = DBConnection.GetConnection();
            using SqlDataAdapter adapter = new SqlDataAdapter(@"
                SELECT 
                    MaDV, 
                    TenDV, 
                    ISNULL(DonGia, 0) AS DonGia, 
                    ISNULL(SoLuong, 0) AS SoLuongTon
                FROM DICH_VU 
                ORDER BY TenDV", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        // Lấy chi tiết các mặt hàng của 1 phiếu nhập (Master - Detail)
        public DataTable GetChiTietTheoMaPN(int maPN)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            string sql = @"
                SELECT
                    CT.MaCTPN,
                    CT.MaPN,
                    CT.MaDV,
                    DV.TenDV,
                    CT.SoLuong,
                    CT.DonGiaNhap,
                    (CT.SoLuong * CT.DonGiaNhap) AS ThanhTien
                FROM CT_PHIEU_NHAP CT
                INNER JOIN DICH_VU DV ON CT.MaDV = DV.MaDV
                WHERE CT.MaPN = @MaPN
                ORDER BY CT.MaCTPN";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@MaPN", maPN);

            using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        // Lập phiếu nhập kèm nhiều mặt hàng trong 1 Transaction (1-N)
        public int ThemPhieuNhapKemChiTiet(int maNCC, int maNV, DateTime ngayNhap, DataTable dtHangNhap)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                // 1. Thêm Master PHIEU_NHAP và nhận lại MaPN vừa tạo
                string sqlPN = @"
                    INSERT INTO PHIEU_NHAP (MaNCC, MaNV, NgayNhap)
                    VALUES (@MaNCC, @MaNV, @NgayNhap);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                int maPN;
                using (SqlCommand cmdPN = new SqlCommand(sqlPN, conn, tran))
                {
                    cmdPN.Parameters.AddWithValue("@MaNCC", maNCC);
                    cmdPN.Parameters.AddWithValue("@MaNV", maNV);
                    cmdPN.Parameters.AddWithValue("@NgayNhap", ngayNhap);
                    maPN = Convert.ToInt32(cmdPN.ExecuteScalar());
                }

                // 2. Thêm từng Detail trong dtHangNhap
                foreach (DataRow row in dtHangNhap.Rows)
                {
                    int maDV = Convert.ToInt32(row["MaDV"]);
                    int soLuong = Convert.ToInt32(row["SoLuong"]);
                    decimal donGiaNhap = Convert.ToDecimal(row["DonGiaNhap"]);

                    string sqlCT = @"
                        INSERT INTO CT_PHIEU_NHAP (MaPN, MaDV, SoLuong, DonGiaNhap)
                        VALUES (@MaPN, @MaDV, @SoLuong, @DonGiaNhap)";

                    using (SqlCommand cmdCT = new SqlCommand(sqlCT, conn, tran))
                    {
                        cmdCT.Parameters.AddWithValue("@MaPN", maPN);
                        cmdCT.Parameters.AddWithValue("@MaDV", maDV);
                        cmdCT.Parameters.AddWithValue("@SoLuong", soLuong);
                        cmdCT.Parameters.AddWithValue("@DonGiaNhap", donGiaNhap);
                        cmdCT.ExecuteNonQuery();
                    }
                }

                tran.Commit();
                return maPN;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        // Thêm phiếu nhập đơn (tương thích code cũ)
        public void Them(int maNCC, int maNV, DateTime ngayNhap)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            string sql = @"
                INSERT INTO PHIEU_NHAP (MaNCC, MaNV, NgayNhap)
                VALUES (@MaNCC, @MaNV, @NgayNhap)";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@MaNCC",    maNCC);
            cmd.Parameters.AddWithValue("@MaNV",     maNV);
            cmd.Parameters.AddWithValue("@NgayNhap", ngayNhap);
            cmd.ExecuteNonQuery();
        }

        // Cập nhật thông tin phiếu nhập
        public void Sua(int maPN, int maNCC, int maNV, DateTime ngayNhap)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            string sql = @"
                UPDATE PHIEU_NHAP SET
                    MaNCC    = @MaNCC,
                    MaNV     = @MaNV,
                    NgayNhap = @NgayNhap
                WHERE MaPN = @MaPN";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@MaPN",     maPN);
            cmd.Parameters.AddWithValue("@MaNCC",    maNCC);
            cmd.Parameters.AddWithValue("@MaNV",     maNV);
            cmd.Parameters.AddWithValue("@NgayNhap", ngayNhap);
            cmd.ExecuteNonQuery();
        }

        // Xóa phiếu nhập (xóa cả các chi tiết bên trong phiếu trước để đảm bảo toàn vẹn dữ liệu)
        public void Xoa(int maPN)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                using (SqlCommand cmdCT = new SqlCommand("DELETE FROM CT_PHIEU_NHAP WHERE MaPN = @MaPN", conn, tran))
                {
                    cmdCT.Parameters.AddWithValue("@MaPN", maPN);
                    cmdCT.ExecuteNonQuery();
                }

                using (SqlCommand cmdPN = new SqlCommand("DELETE FROM PHIEU_NHAP WHERE MaPN = @MaPN", conn, tran))
                {
                    cmdPN.Parameters.AddWithValue("@MaPN", maPN);
                    cmdPN.ExecuteNonQuery();
                }

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        // Tìm kiếm phiếu nhập
        public DataTable TimKiem(string tuKhoa)
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = @"
                SELECT
                    PN.MaPN,
                    PN.MaNCC,
                    NCC.TenNCC,
                    PN.MaNV,
                    NV.HoTen AS TenNhanVien,
                    PN.NgayNhap,
                    ISNULL((SELECT SUM(CT.SoLuong * CT.DonGiaNhap) FROM CT_PHIEU_NHAP CT WHERE CT.MaPN = PN.MaPN), 0) AS TongTien
                FROM PHIEU_NHAP PN
                INNER JOIN NHA_CUNG_CAP NCC ON PN.MaNCC = NCC.MaNCC
                INNER JOIN NHAN_VIEN    NV  ON PN.MaNV  = NV.MaNV
                WHERE CAST(PN.MaPN AS NVARCHAR(20)) LIKE @TuKhoa
                   OR NCC.TenNCC LIKE @TuKhoa
                   OR NV.HoTen LIKE @TuKhoa
                ORDER BY PN.MaPN DESC";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@TuKhoa", "%" + tuKhoa + "%");

            using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }
    }
}
