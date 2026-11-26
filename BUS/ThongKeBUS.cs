using System;
using System.Data;
using QuanLyKhachSan.DAL;

namespace QuanLyKhachSan.BUS
{
    /// <summary>Applies business validation before requesting report data from DAL.</summary>
    public class ThongKeBUS
    {
        private readonly ThongKeDAL _dal = new();

        public DataTable ThongKeThuChi(DateTime tuNgay, DateTime denNgay)
        {
            if (tuNgay.Date > denNgay.Date)
                throw new ArgumentException("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc.");
            return _dal.ThongKeThuChi(tuNgay.Date, denNgay.Date);
        }

        public DataTable ThongKeHoaDonTheoNhanVien(int nam)
        {
            ValidateYear(nam);
            return _dal.ThongKeHoaDonTheoNhanVien(nam);
        }

        public DataTable ThongKeHoaDonTheoKhachHang(int nam)
        {
            ValidateYear(nam);
            return _dal.ThongKeHoaDonTheoKhachHang(nam);
        }

        public DataTable ThongKeDichVuTheoQuy(int nam)
        {
            ValidateYear(nam);
            return _dal.ThongKeDichVuTheoQuy(nam);
        }

        private static void ValidateYear(int nam)
        {
            if (nam < 2000 || nam > 2100)
                throw new ArgumentOutOfRangeException(nameof(nam), "Năm thống kê phải nằm trong khoảng 2000–2100.");
        }
    }
}
