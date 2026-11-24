using System;
using System.Data;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.BUS
{
    public class ChiTietHoaDonBUS
    {
        private readonly ChiTietHoaDonDAL dal = new();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public DataTable GetByMaHD(int maHD)
        {
            return dal.GetByMaHD(maHD);
        }

        public DataTable GetDanhSachHoaDon()
        {
            return dal.GetDanhSachHoaDon();
        }

        public DataTable GetDanhSachDichVu()
        {
            return dal.GetDanhSachDichVu();
        }

        public bool KiemTraTonTai(int maCTHD)
        {
            DataTable dt = dal.GetAll();
            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt32(row["MaCTHD"]) == maCTHD)
                    return true;
            }
            return false;
        }

        public void Them(ChiTietHoaDon ct)
        {
            if (ct == null)
                throw new ArgumentNullException(nameof(ct));

            if (ct.MaCTHD <= 0)
                throw new ArgumentException("Mã chi tiết hóa đơn phải là số nguyên dương.");

            if (ct.MaHD <= 0)
                throw new ArgumentException("Vui lòng chọn hóa đơn hợp lệ.");

            if (ct.MaDV <= 0)
                throw new ArgumentException("Vui lòng chọn dịch vụ hợp lệ.");

            if (ct.SoLuong <= 0)
                throw new ArgumentException("Số lượng dịch vụ phải lớn hơn 0.");

            if (ct.DonGia <= 0)
                throw new ArgumentException("Đơn giá dịch vụ phải lớn hơn 0.");

            ct.ThanhTien = ct.SoLuong * ct.DonGia;

            if (KiemTraTonTai(ct.MaCTHD))
                throw new InvalidOperationException($"Mã chi tiết {ct.MaCTHD} đã tồn tại trong hệ thống.");

            dal.Them(ct);
        }

        public void Sua(ChiTietHoaDon ct)
        {
            if (ct == null)
                throw new ArgumentNullException(nameof(ct));

            if (ct.MaCTHD <= 0)
                throw new ArgumentException("Mã chi tiết hóa đơn không hợp lệ.");

            if (ct.MaHD <= 0)
                throw new ArgumentException("Vui lòng chọn hóa đơn hợp lệ.");

            if (ct.MaDV <= 0)
                throw new ArgumentException("Vui lòng chọn dịch vụ hợp lệ.");

            if (ct.SoLuong <= 0)
                throw new ArgumentException("Số lượng dịch vụ phải lớn hơn 0.");

            if (ct.DonGia <= 0)
                throw new ArgumentException("Đơn giá dịch vụ phải lớn hơn 0.");

            ct.ThanhTien = ct.SoLuong * ct.DonGia;

            if (!KiemTraTonTai(ct.MaCTHD))
                throw new InvalidOperationException($"Không tìm thấy chi tiết hóa đơn có mã {ct.MaCTHD} để sửa.");

            dal.Sua(ct);
        }

        public void Xoa(int maCTHD)
        {
            if (maCTHD <= 0)
                throw new ArgumentException("Mã chi tiết hóa đơn không hợp lệ.");

            if (!KiemTraTonTai(maCTHD))
                throw new InvalidOperationException($"Không tìm thấy chi tiết hóa đơn có mã {maCTHD} để xóa.");

            dal.Xoa(maCTHD);
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
