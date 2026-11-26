using System;
using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyKhachSan.Database;

namespace QuanLyKhachSan.DAL
{
    /// <summary>Runs reporting queries against SQL Server.</summary>
    public class ThongKeDAL
    {
        public DataTable ThongKeThuChi(DateTime tuNgay, DateTime denNgay)
        {
            const string sql = @"
                SELECT
                    ISNULL((SELECT SUM(TongTien) FROM HOA_DON WHERE NgayLap >= @TuNgay AND NgayLap < @DenNgayExclusive), 0) AS TongThu,
                    ISNULL((SELECT SUM(CT.SoLuong * CT.DonGiaNhap)
                            FROM CT_PHIEU_NHAP CT
                            INNER JOIN PHIEU_NHAP PN ON CT.MaPN = PN.MaPN
                            WHERE PN.NgayNhap >= @TuNgay AND PN.NgayNhap < @DenNgayExclusive), 0) AS TongChi,
                    ISNULL((SELECT SUM(TongTien) FROM HOA_DON WHERE NgayLap >= @TuNgay AND NgayLap < @DenNgayExclusive), 0) -
                    ISNULL((SELECT SUM(CT.SoLuong * CT.DonGiaNhap)
                            FROM CT_PHIEU_NHAP CT
                            INNER JOIN PHIEU_NHAP PN ON CT.MaPN = PN.MaPN
                            WHERE PN.NgayNhap >= @TuNgay AND PN.NgayNhap < @DenNgayExclusive), 0) AS LoiNhuan";

            using SqlConnection conn = DBConnection.GetConnection();
            using SqlCommand cmd = new(sql, conn);
            cmd.Parameters.Add("@TuNgay", SqlDbType.DateTime).Value = tuNgay.Date;
            cmd.Parameters.Add("@DenNgayExclusive", SqlDbType.DateTime).Value = denNgay.Date.AddDays(1);
            using SqlDataAdapter adapter = new(cmd);
            DataTable result = new();
            adapter.Fill(result);
            return result;
        }

        public DataTable ThongKeHoaDonTheoNhanVien(int nam)
        {
            const string sql = @"
                SELECT NV.MaNV, NV.HoTen AS TenNhanVien,
                    ISNULL(SUM(CASE WHEN DATEPART(QUARTER, HD.NgayLap) = 1 THEN HD.TongTien ELSE 0 END), 0) AS Q1,
                    ISNULL(SUM(CASE WHEN DATEPART(QUARTER, HD.NgayLap) = 2 THEN HD.TongTien ELSE 0 END), 0) AS Q2,
                    ISNULL(SUM(CASE WHEN DATEPART(QUARTER, HD.NgayLap) = 3 THEN HD.TongTien ELSE 0 END), 0) AS Q3,
                    ISNULL(SUM(CASE WHEN DATEPART(QUARTER, HD.NgayLap) = 4 THEN HD.TongTien ELSE 0 END), 0) AS Q4,
                    ISNULL(SUM(HD.TongTien), 0) AS TongCong
                FROM NHAN_VIEN NV
                LEFT JOIN HOA_DON HD ON NV.MaNV = HD.MaNV AND HD.NgayLap >= @TuNam AND HD.NgayLap < @NamSau
                GROUP BY NV.MaNV, NV.HoTen ORDER BY NV.MaNV";
            return QueryYear(sql, nam);
        }

        public DataTable ThongKeHoaDonTheoKhachHang(int nam)
        {
            const string sql = @"
                SELECT KH.MaKH, KH.HoTen AS TenKhachHang,
                    ISNULL(SUM(CASE WHEN DATEPART(QUARTER, HD.NgayLap) = 1 THEN HD.TongTien ELSE 0 END), 0) AS Q1,
                    ISNULL(SUM(CASE WHEN DATEPART(QUARTER, HD.NgayLap) = 2 THEN HD.TongTien ELSE 0 END), 0) AS Q2,
                    ISNULL(SUM(CASE WHEN DATEPART(QUARTER, HD.NgayLap) = 3 THEN HD.TongTien ELSE 0 END), 0) AS Q3,
                    ISNULL(SUM(CASE WHEN DATEPART(QUARTER, HD.NgayLap) = 4 THEN HD.TongTien ELSE 0 END), 0) AS Q4,
                    ISNULL(SUM(HD.TongTien), 0) AS TongCong
                FROM KHACH_HANG KH
                LEFT JOIN DAT_PHONG DP ON KH.MaKH = DP.MaKH
                LEFT JOIN HOA_DON HD ON DP.MaDP = HD.MaDP AND HD.NgayLap >= @TuNam AND HD.NgayLap < @NamSau
                GROUP BY KH.MaKH, KH.HoTen ORDER BY KH.MaKH";
            return QueryYear(sql, nam);
        }

        public DataTable ThongKeDichVuTheoQuy(int nam)
        {
            const string sql = @"
                SELECT DV.MaDV, DV.TenDV,
                    ISNULL(SUM(CASE WHEN HD.NgayLap IS NOT NULL AND DATEPART(QUARTER, HD.NgayLap) = 1 THEN CTHD.ThanhTien ELSE 0 END), 0) AS Q1,
                    ISNULL(SUM(CASE WHEN HD.NgayLap IS NOT NULL AND DATEPART(QUARTER, HD.NgayLap) = 2 THEN CTHD.ThanhTien ELSE 0 END), 0) AS Q2,
                    ISNULL(SUM(CASE WHEN HD.NgayLap IS NOT NULL AND DATEPART(QUARTER, HD.NgayLap) = 3 THEN CTHD.ThanhTien ELSE 0 END), 0) AS Q3,
                    ISNULL(SUM(CASE WHEN HD.NgayLap IS NOT NULL AND DATEPART(QUARTER, HD.NgayLap) = 4 THEN CTHD.ThanhTien ELSE 0 END), 0) AS Q4,
                    ISNULL(SUM(CASE WHEN HD.NgayLap IS NOT NULL THEN CTHD.ThanhTien ELSE 0 END), 0) AS TongCong
                FROM DICH_VU DV
                LEFT JOIN CHI_TIET_HOA_DON CTHD ON DV.MaDV = CTHD.MaDV
                LEFT JOIN HOA_DON HD ON CTHD.MaHD = HD.MaHD AND HD.NgayLap >= @TuNam AND HD.NgayLap < @NamSau
                GROUP BY DV.MaDV, DV.TenDV ORDER BY DV.MaDV";
            return QueryYear(sql, nam);
        }

        private static DataTable QueryYear(string sql, int nam)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            using SqlCommand cmd = new(sql, conn);
            cmd.Parameters.Add("@TuNam", SqlDbType.DateTime).Value = new DateTime(nam, 1, 1);
            cmd.Parameters.Add("@NamSau", SqlDbType.DateTime).Value = new DateTime(nam + 1, 1, 1);
            using SqlDataAdapter adapter = new(cmd);
            DataTable result = new();
            adapter.Fill(result);
            return result;
        }
    }
}
