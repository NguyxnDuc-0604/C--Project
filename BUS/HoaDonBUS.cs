using System;
using System.Data;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.BUS
{
    public class HoaDonBUS
    {
        private readonly HoaDonDAL dal = new();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public DataTable GetDanhSachDatPhong()
        {
            return dal.GetDanhSachDatPhong();
        }

        public DataTable GetDanhSachNhanVien()
        {
            return dal.GetDanhSachNhanVien();
        }

        public bool KiemTraTonTai(int maHD)
        {
            DataTable dt = dal.GetAll();
            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt32(row["MaHD"]) == maHD)
                    return true;
            }
            return false;
        }

        public void Them(HoaDon hd)
        {
            if (hd == null)
                throw new ArgumentNullException(nameof(hd));

            if (hd.MaHD <= 0)
                throw new ArgumentException("Mã hóa đơn phải là số nguyên dương.");

            if (hd.MaDP <= 0)
                throw new ArgumentException("Vui lòng chọn phiếu đặt phòng.");

            if (hd.MaNV <= 0)
                throw new ArgumentException("Vui lòng chọn nhân viên lập hóa đơn.");

            if (KiemTraTonTai(hd.MaHD))
                throw new InvalidOperationException($"Mã hóa đơn {hd.MaHD} đã tồn tại trong hệ thống.");

            dal.Them(hd);
        }

        public void Sua(HoaDon hd)
        {
            if (hd == null)
                throw new ArgumentNullException(nameof(hd));

            if (hd.MaHD <= 0)
                throw new ArgumentException("Mã hóa đơn không hợp lệ.");

            if (hd.MaDP <= 0)
                throw new ArgumentException("Vui lòng chọn phiếu đặt phòng.");

            if (hd.MaNV <= 0)
                throw new ArgumentException("Vui lòng chọn nhân viên lập hóa đơn.");

            if (!KiemTraTonTai(hd.MaHD))
                throw new InvalidOperationException($"Không tìm thấy hóa đơn có mã {hd.MaHD} để cập nhật.");

            dal.Sua(hd);
        }

        public void Xoa(int maHD)
        {
            if (maHD <= 0)
                throw new ArgumentException("Mã hóa đơn cần xóa không hợp lệ.");

            if (!KiemTraTonTai(maHD))
                throw new InvalidOperationException($"Không tìm thấy hóa đơn có mã {maHD} để xóa.");

            dal.Xoa(maHD);
        }

        public DataTable TimKiem(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return GetAll();

            return dal.TimKiem(tuKhoa.Trim());
        }

        public decimal TinhTongTien(int maHD)
        {
            if (maHD <= 0) return 0;
            return dal.TinhTongTien(maHD);
        }
    }
}
