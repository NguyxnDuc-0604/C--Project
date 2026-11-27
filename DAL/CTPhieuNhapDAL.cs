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
        // Trigger cập nhật tồn kho theo các thay đổi của chi tiết phiếu nhập.
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

        // Trigger điều chỉnh tồn kho khi MaDV hoặc SoLuong thay đổi.
        public void Sua(CTPhieuNhap ct)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            const string sql = @"
                UPDATE CT_PHIEU_NHAP SET
                    MaPN = @MaPN,
                    MaDV = @MaDV,
                    SoLuong = @SoLuong,
                    DonGiaNhap = @DonGiaNhap
                WHERE MaCTPN = @MaCTPN";

            using SqlCommand cmd = new(sql, conn);
            cmd.Parameters.Add("@MaCTPN", SqlDbType.Int).Value = ct.MaCTPN;
            cmd.Parameters.Add("@MaPN", SqlDbType.Int).Value = ct.MaPN;
            cmd.Parameters.Add("@MaDV", SqlDbType.Int).Value = ct.MaDV;
            cmd.Parameters.Add("@SoLuong", SqlDbType.Int).Value = ct.SoLuong;
            cmd.Parameters.Add("@DonGiaNhap", SqlDbType.Decimal).Value = ct.DonGiaNhap;
            cmd.ExecuteNonQuery();
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

        /// <summary>
        /// Kiểm tra xem 1 mã dịch vụ đã có trong phiếu nhập chưa (tránh trùng).
        /// </summary>
        public bool KiemTraDichVuDaTonTaiTrongPhieu(int maPN, int maDV)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            const string sql = @"
                SELECT COUNT(1) FROM CT_PHIEU_NHAP
                WHERE MaPN = @MaPN AND MaDV = @MaDV";
            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaPN", SqlDbType.Int).Value = maPN;
            cmd.Parameters.Add("@MaDV", SqlDbType.Int).Value = maDV;
            conn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        /// <summary>
        /// Thêm NHIỀU chi tiết phiếu nhập trong 1 Transaction.
        /// Trigger DB sẽ tự cộng tồn kho cho từng dịch vụ sau mỗi INSERT.
        /// </summary>
        public void ThemNhieuCTPN(int maPN, System.Data.DataTable dtGioHang)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            using SqlTransaction tran = conn.BeginTransaction();
            try
            {
                const string sql = @"
                    INSERT INTO CT_PHIEU_NHAP (MaPN, MaDV, SoLuong, DonGiaNhap)
                    VALUES (@MaPN, @MaDV, @SoLuong, @DonGiaNhap)";

                foreach (System.Data.DataRow row in dtGioHang.Rows)
                {
                    using SqlCommand cmd = new SqlCommand(sql, conn, tran);
                    cmd.Parameters.Add("@MaPN",       SqlDbType.Int).Value     = maPN;
                    cmd.Parameters.Add("@MaDV",       SqlDbType.Int).Value     = Convert.ToInt32(row["MaDV"]);
                    cmd.Parameters.Add("@SoLuong",    SqlDbType.Int).Value     = Convert.ToInt32(row["SoLuong"]);
                    cmd.Parameters.Add("@DonGiaNhap", SqlDbType.Decimal).Value = Convert.ToDecimal(row["DonGia"]);
                    cmd.ExecuteNonQuery();
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
}
