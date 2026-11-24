using System;
using System.Data;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.BUS
{
    public class KhachHangBUS
    {
        private readonly KhachHangDAL dal = new();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public bool KiemTraTonTai(int maKH)
        {
            DataTable dt = dal.GetAll();
            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt32(row["MaKH"]) == maKH)
                    return true;
            }
            return false;
        }

        public void ThemKhachHang(int maKH, string hoTen, string cccd, string sdt, string email, string diaChi)
        {
            if (maKH <= 0)
                throw new ArgumentException("Mã khách hàng phải là số nguyên dương.");

            if (string.IsNullOrWhiteSpace(hoTen))
                throw new ArgumentException("Họ tên khách hàng không được để trống.");

            if (KiemTraTonTai(maKH))
                throw new InvalidOperationException($"Mã khách hàng {maKH} đã tồn tại trong hệ thống.");

            dal.ThemKhachHang(maKH, hoTen.Trim(), cccd?.Trim() ?? "", sdt?.Trim() ?? "", email?.Trim() ?? "", diaChi?.Trim() ?? "");
        }

        public void SuaKhachHang(int maKH, string hoTen, string cccd, string sdt, string email, string diaChi)
        {
            if (maKH <= 0)
                throw new ArgumentException("Mã khách hàng không hợp lệ.");

            if (string.IsNullOrWhiteSpace(hoTen))
                throw new ArgumentException("Họ tên khách hàng không được để trống.");

            if (!KiemTraTonTai(maKH))
                throw new InvalidOperationException($"Không tìm thấy khách hàng có mã {maKH} để cập nhật.");

            dal.SuaKhachHang(maKH, hoTen.Trim(), cccd?.Trim() ?? "", sdt?.Trim() ?? "", email?.Trim() ?? "", diaChi?.Trim() ?? "");
        }

        public void XoaKhachHang(int maKH)
        {
            if (maKH <= 0)
                throw new ArgumentException("Mã khách hàng cần xóa không hợp lệ.");

            if (!KiemTraTonTai(maKH))
                throw new InvalidOperationException($"Không tìm thấy khách hàng có mã {maKH} để xóa.");

            dal.XoaKhachHang(maKH);
        }

        public DataTable TimKiemKhachHang(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return GetAll();

            return dal.TimKiemKhachHang(tuKhoa.Trim());
        }
    }
}
