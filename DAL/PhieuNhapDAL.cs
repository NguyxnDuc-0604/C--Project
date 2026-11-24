using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyKhachSan.Database;

namespace QuanLyKhachSan.DAL
{
    public class PhieuNhapDAL
    {
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
                    PN.NgayNhap
                FROM PHIEU_NHAP PN
                INNER JOIN NHA_CUNG_CAP NCC ON PN.MaNCC = NCC.MaNCC
                INNER JOIN NHAN_VIEN    NV  ON PN.MaNV  = NV.MaNV
                ORDER BY PN.MaPN";

            using SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable GetNhaCungCap()
        {
            using SqlConnection conn = DBConnection.GetConnection();
            using SqlDataAdapter adapter = new SqlDataAdapter("SELECT MaNCC, TenNCC FROM NHA_CUNG_CAP ORDER BY TenNCC", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable GetNhanVien()
        {
            using SqlConnection conn = DBConnection.GetConnection();
            using SqlDataAdapter adapter = new SqlDataAdapter("SELECT MaNV, HoTen FROM NHAN_VIEN ORDER BY HoTen", conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

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

        public void Xoa(int maPN)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            using SqlCommand cmd = new SqlCommand("DELETE FROM PHIEU_NHAP WHERE MaPN = @MaPN", conn);
            cmd.Parameters.AddWithValue("@MaPN", maPN);
            cmd.ExecuteNonQuery();
        }

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
                    PN.NgayNhap
                FROM PHIEU_NHAP PN
                INNER JOIN NHA_CUNG_CAP NCC ON PN.MaNCC = NCC.MaNCC
                INNER JOIN NHAN_VIEN    NV  ON PN.MaNV  = NV.MaNV
                WHERE CAST(PN.MaPN AS NVARCHAR(20)) LIKE @q
                   OR NCC.TenNCC LIKE @q
                   OR NV.HoTen   LIKE @q
                ORDER BY PN.MaPN";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@q", "%" + tuKhoa + "%");
            using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }
    }
}
