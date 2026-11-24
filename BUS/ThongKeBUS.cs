using System;
using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyKhachSan.Database;

namespace QuanLyKhachSan.BUS
{
    public class ThongKeBUS
    {
        // 1. Thống kê tổng thu, tổng chi, lợi nhuận từ ngày đến ngày
        public DataTable ThongKeThuChi(DateTime tuNgay, DateTime denNgay)
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = @"
                SELECT 
                    ISNULL((SELECT SUM(TongTien) FROM HOA_DON WHERE NgayLap >= @TuNgay AND NgayLap <= @DenNgay), 0) AS TongThu,
                    ISNULL((SELECT SUM(CT.SoLuong * CT.DonGiaNhap) 
                            FROM CT_PHIEU_NHAP CT 
                            INNER JOIN PHIEU_NHAP PN ON CT.MaPN = PN.MaPN 
                            WHERE PN.NgayNhap >= @TuNgay AND PN.NgayNhap <= @DenNgay), 0) AS TongChi,
                    ISNULL((SELECT SUM(TongTien) FROM HOA_DON WHERE NgayLap >= @TuNgay AND NgayLap <= @DenNgay), 0) -
                    ISNULL((SELECT SUM(CT.SoLuong * CT.DonGiaNhap) 
                            FROM CT_PHIEU_NHAP CT 
                            INNER JOIN PHIEU_NHAP PN ON CT.MaPN = PN.MaPN 
                            WHERE PN.NgayNhap >= @TuNgay AND PN.NgayNhap <= @DenNgay), 0) AS LoiNhuan";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@TuNgay", tuNgay.Date);
            cmd.Parameters.AddWithValue("@DenNgay", denNgay.Date.AddDays(1).AddTicks(-1));

            using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        // 2. Thống kê tổng tiền hóa đơn theo nhân viên và năm
        public DataTable ThongKeHoaDonTheoNhanVien(int nam)
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = @"
                SELECT 
                    NV.MaNV,
                    NV.HoTen AS TenNhanVien,
                    ISNULL(SUM(CASE WHEN DATEPART(QUARTER, HD.NgayLap) = 1 THEN HD.TongTien ELSE 0 END), 0) AS Q1,
                    ISNULL(SUM(CASE WHEN DATEPART(QUARTER, HD.NgayLap) = 2 THEN HD.TongTien ELSE 0 END), 0) AS Q2,
                    ISNULL(SUM(CASE WHEN DATEPART(QUARTER, HD.NgayLap) = 3 THEN HD.TongTien ELSE 0 END), 0) AS Q3,
                    ISNULL(SUM(CASE WHEN DATEPART(QUARTER, HD.NgayLap) = 4 THEN HD.TongTien ELSE 0 END), 0) AS Q4,
                    ISNULL(SUM(HD.TongTien), 0) AS TongCong
                FROM NHAN_VIEN NV
                LEFT JOIN HOA_DON HD ON NV.MaNV = HD.MaNV AND YEAR(HD.NgayLap) = @Nam
                GROUP BY NV.MaNV, NV.HoTen
                ORDER BY NV.MaNV";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Nam", nam);

            using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        // 3. Thống kê tổng tiền hóa đơn theo khách hàng và năm
        public DataTable ThongKeHoaDonTheoKhachHang(int nam)
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = @"
                SELECT 
                    KH.MaKH,
                    KH.HoTen AS TenKhachHang,
                    ISNULL(SUM(CASE WHEN DATEPART(QUARTER, HD.NgayLap) = 1 THEN HD.TongTien ELSE 0 END), 0) AS Q1,
                    ISNULL(SUM(CASE WHEN DATEPART(QUARTER, HD.NgayLap) = 2 THEN HD.TongTien ELSE 0 END), 0) AS Q2,
                    ISNULL(SUM(CASE WHEN DATEPART(QUARTER, HD.NgayLap) = 3 THEN HD.TongTien ELSE 0 END), 0) AS Q3,
                    ISNULL(SUM(CASE WHEN DATEPART(QUARTER, HD.NgayLap) = 4 THEN HD.TongTien ELSE 0 END), 0) AS Q4,
                    ISNULL(SUM(HD.TongTien), 0) AS TongCong
                FROM KHACH_HANG KH
                LEFT JOIN DAT_PHONG DP ON KH.MaKH = DP.MaKH
                LEFT JOIN HOA_DON HD ON DP.MaDP = HD.MaDP AND YEAR(HD.NgayLap) = @Nam
                GROUP BY KH.MaKH, KH.HoTen
                ORDER BY KH.MaKH";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Nam", nam);

            using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        // 4. Thống kê tiền dịch vụ theo từng quý trong năm
        public DataTable ThongKeDichVuTheoQuy(int nam)
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = @"
                SELECT 
                    DV.MaDV,
                    DV.TenDV,
                    ISNULL(SUM(CASE WHEN DATEPART(QUARTER, HD.NgayLap) = 1 THEN CTHD.ThanhTien ELSE 0 END), 0) AS Q1,
                    ISNULL(SUM(CASE WHEN DATEPART(QUARTER, HD.NgayLap) = 2 THEN CTHD.ThanhTien ELSE 0 END), 0) AS Q2,
                    ISNULL(SUM(CASE WHEN DATEPART(QUARTER, HD.NgayLap) = 3 THEN CTHD.ThanhTien ELSE 0 END), 0) AS Q3,
                    ISNULL(SUM(CASE WHEN DATEPART(QUARTER, HD.NgayLap) = 4 THEN CTHD.ThanhTien ELSE 0 END), 0) AS Q4,
                    ISNULL(SUM(CTHD.ThanhTien), 0) AS TongCong
                FROM DICH_VU DV
                LEFT JOIN CHI_TIET_HOA_DON CTHD ON DV.MaDV = CTHD.MaDV
                LEFT JOIN HOA_DON HD ON CTHD.MaHD = HD.MaHD AND YEAR(HD.NgayLap) = @Nam
                GROUP BY DV.MaDV, DV.TenDV
                ORDER BY DV.MaDV";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Nam", nam);

            using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }
    }
}
