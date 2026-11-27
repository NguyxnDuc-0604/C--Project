using System;
using System.Data;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.BUS
{
    public class ChiTietDatPhongBUS
    {
        private readonly ChiTietDatPhongDAL dal = new();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public DataTable GetDanhSachDatPhong()
        {
            return dal.GetDanhSachDatPhong();
        }

        public DataTable GetDanhSachPhong()
        {
            return dal.GetDanhSachPhong();
        }

        public bool KiemTraTonTai(int maCTDP)
        {
            DataTable dt = dal.GetAll();
            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt32(row["MaCTDP"]) == maCTDP)
                    return true;
            }
            return false;
        }

        public void Them(ChiTietDatPhong ct)
        {
            if (ct == null)
                throw new ArgumentNullException(nameof(ct));

            if (ct.MaCTDP <= 0)
                throw new ArgumentException("Mã chi tiết đặt phòng phải là số nguyên dương.");

            if (ct.MaDP <= 0)
                throw new ArgumentException("Vui lòng chọn mã đặt phòng hợp lệ.");

            if (ct.MaPhong <= 0)
                throw new ArgumentException("Vui lòng chọn phòng hợp lệ.");

            if (ct.DonGia <= 0)
                throw new ArgumentException("Đơn giá phòng phải lớn hơn 0.");

            if (KiemTraTonTai(ct.MaCTDP))
                throw new InvalidOperationException($"Mã chi tiết {ct.MaCTDP} đã tồn tại.");

            dal.Them(ct);
        }

        public void Sua(ChiTietDatPhong ct)
        {
            if (ct == null)
                throw new ArgumentNullException(nameof(ct));

            if (ct.MaCTDP <= 0)
                throw new ArgumentException("Mã chi tiết đặt phòng không hợp lệ.");

            if (ct.MaDP <= 0)
                throw new ArgumentException("Vui lòng chọn mã đặt phòng hợp lệ.");

            if (ct.MaPhong <= 0)
                throw new ArgumentException("Vui lòng chọn phòng hợp lệ.");

            if (ct.DonGia <= 0)
                throw new ArgumentException("Đơn giá phòng phải lớn hơn 0.");

            if (!KiemTraTonTai(ct.MaCTDP))
                throw new InvalidOperationException($"Không tìm thấy chi tiết đặt phòng mã {ct.MaCTDP} để sửa.");

            dal.Sua(ct);
        }

        public void Xoa(int maCTDP)
        {
            if (maCTDP <= 0)
                throw new ArgumentException("Mã chi tiết đặt phòng không hợp lệ.");

            if (!KiemTraTonTai(maCTDP))
                throw new InvalidOperationException($"Không tìm thấy chi tiết đặt phòng mã {maCTDP} để xóa.");

            dal.Xoa(maCTDP);
        }

        public DataTable TimKiem(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return GetAll();

            return dal.TimKiem(tuKhoa.Trim());
        }

        public decimal TinhTongTien(int maDP)
        {
            if (maDP <= 0) return 0;
            return dal.TinhTongTien(maDP);
        }
    }
}
