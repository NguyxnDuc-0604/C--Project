using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyKhachSan.Database;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.DAL
{
    public class CTPhieuNhapDAL
    {
        // Lấy tất cả chi tiết phiếu nhập — JOIN để hiển thị TenDV
        public DataTable GetAll()
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
                    CT.SoLuong * CT.DonGiaNhap AS ThanhTien
                FROM CT_PHIEU_NHAP CT
                INNER JOIN DICH_VU DV ON CT.MaDV = DV.MaDV
                ORDER BY CT.MaPN, CT.MaCTPN";

            using SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        // Lấy chi tiết theo mã phiếu nhập
        public DataTable GetByMaPN(int maPN)
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
                    CT.SoLuong * CT.DonGiaNhap AS ThanhTien
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

        // Lấy danh sách phiếu nhập cho ComboBox
        public DataTable GetDanhSachPhieuNhap()
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = "SELECT MaPN FROM PHIEU_NHAP ORDER BY MaPN";

            using SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        // Lấy danh sách dịch vụ cho ComboBox (hiển thị TenDV)
        public DataTable GetDanhSachDichVu()
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = "SELECT MaDV, TenDV, DonGia FROM DICH_VU ORDER BY MaDV";

            using SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        // Thêm chi tiết phiếu nhập
        // Trigger trg_CapNhatTonKho_Insert sẽ tự động cộng SoLuong vào DICH_VU
        public void Them(CTPhieuNhap ct)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            string sql = @"
                INSERT INTO CT_PHIEU_NHAP (MaPN, MaDV, SoLuong, DonGiaNhap)
                VALUES (@MaPN, @MaDV, @SoLuong, @DonGiaNhap)";

            using SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@MaPN",       SqlDbType.Int).Value     = ct.MaPN;
            cmd.Parameters.Add("@MaDV",       SqlDbType.Int).Value     = ct.MaDV;
            cmd.Parameters.Add("@SoLuong",    SqlDbType.Int).Value     = ct.SoLuong;
            cmd.Parameters.Add("@DonGiaNhap", SqlDbType.Decimal).Value = ct.DonGiaNhap;

            cmd.ExecuteNonQuery();
        }

        // Sửa chi tiết phiếu nhập
        // Tự điều chỉnh tồn kho: trừ số lượng cũ, cộng số lượng mới (trong transaction)
        public void Sua(CTPhieuNhap ct, int soLuongCu)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                // Cập nhật chi tiết phiếu nhập
                string sqlUpdate = @"
                    UPDATE CT_PHIEU_NHAP SET
                        MaPN       = @MaPN,
                        MaDV       = @MaDV,
                        SoLuong    = @SoLuong,
                        DonGiaNhap = @DonGiaNhap
                    WHERE MaCTPN = @MaCTPN";

                using SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, conn, tran);
                cmdUpdate.Parameters.Add("@MaCTPN",    SqlDbType.Int).Value     = ct.MaCTPN;
                cmdUpdate.Parameters.Add("@MaPN",      SqlDbType.Int).Value     = ct.MaPN;
                cmdUpdate.Parameters.Add("@MaDV",      SqlDbType.Int).Value     = ct.MaDV;
                cmdUpdate.Parameters.Add("@SoLuong",   SqlDbType.Int).Value     = ct.SoLuong;
                cmdUpdate.Parameters.Add("@DonGiaNhap",SqlDbType.Decimal).Value = ct.DonGiaNhap;
                cmdUpdate.ExecuteNonQuery();

                // Điều chỉnh tồn kho theo chênh lệch số lượng
                int chenhLech = ct.SoLuong - soLuongCu;
                if (chenhLech != 0)
                {
                    string sqlTK = "UPDATE DICH_VU SET SoLuong = SoLuong + @cl WHERE MaDV = @MaDV";
                    using SqlCommand cmdTK = new SqlCommand(sqlTK, conn, tran);
                    cmdTK.Parameters.AddWithValue("@cl",   chenhLech);
                    cmdTK.Parameters.AddWithValue("@MaDV", ct.MaDV);
                    cmdTK.ExecuteNonQuery();
                }

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        // Xoá chi tiết phiếu nhập
        // Trigger trg_CapNhatTonKho_Delete sẽ tự trừ SoLuong khỏi DICH_VU
        public void Xoa(int maCTPN)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            string sql = "DELETE FROM CT_PHIEU_NHAP WHERE MaCTPN = @MaCTPN";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaCTPN", SqlDbType.Int).Value = maCTPN;

            cmd.ExecuteNonQuery();
        }

        // Tìm kiếm chi tiết phiếu nhập
        public DataTable TimKiem(string tuKhoa)
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
                    CT.SoLuong * CT.DonGiaNhap AS ThanhTien
                FROM CT_PHIEU_NHAP CT
                INNER JOIN DICH_VU DV ON CT.MaDV = DV.MaDV
                WHERE CAST(CT.MaCTPN AS NVARCHAR(20)) LIKE @q
                   OR CAST(CT.MaPN   AS NVARCHAR(20)) LIKE @q
                   OR DV.TenDV LIKE @q
                ORDER BY CT.MaPN, CT.MaCTPN";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@q", "%" + tuKhoa + "%");

            using SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        // Tính tổng tiền của một phiếu nhập
        public decimal TinhTongTien(int maPN)
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = @"
                SELECT ISNULL(SUM(SoLuong * DonGiaNhap), 0)
                FROM CT_PHIEU_NHAP
                WHERE MaPN = @MaPN";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@MaPN", maPN);

            conn.Open();
            return Convert.ToDecimal(cmd.ExecuteScalar());
        }
    }
}
