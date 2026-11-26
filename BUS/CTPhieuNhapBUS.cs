using System;
using System.Data;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.BUS
{
    public class CTPhieuNhapBUS
    {
        private readonly CTPhieuNhapDAL dal = new();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public DataTable GetByMaPN(int maPN)
        {
            return dal.GetByMaPN(maPN);
        }

        public DataTable GetDanhSachPhieuNhap()
        {
            return dal.GetDanhSachPhieuNhap();
        }

        public DataTable GetDanhSachDichVu()
        {
            return dal.GetDanhSachDichVu();
        }

        public bool KiemTraTonTai(int maCTPN)
        {
            DataTable dt = dal.GetAll();
            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt32(row["MaCTPN"]) == maCTPN)
                    return true;
            }
            return false;
        }

        public void Them(CTPhieuNhap ct)
        {
            if (ct == null)
                throw new ArgumentNullException(nameof(ct));

            if (ct.MaPN <= 0)
                throw new ArgumentException("Vui lòng chọn phiếu nhập hợp lệ.");

            if (ct.MaDV <= 0)
                throw new ArgumentException("Vui lòng chọn dịch vụ/hàng hóa hợp lệ.");

            if (ct.SoLuong <= 0)
                throw new ArgumentException("Số lượng nhập phải lớn hơn 0.");

            if (ct.DonGiaNhap <= 0)
                throw new ArgumentException("Đơn giá nhập phải lớn hơn 0.");

            dal.Them(ct);
        }

        public void Sua(CTPhieuNhap ct)
        {
            if (ct == null)
                throw new ArgumentNullException(nameof(ct));

            if (ct.MaCTPN <= 0)
                throw new ArgumentException("Mã chi tiết phiếu nhập không hợp lệ.");

            if (ct.MaPN <= 0)
                throw new ArgumentException("Vui lòng chọn phiếu nhập hợp lệ.");

            if (ct.MaDV <= 0)
                throw new ArgumentException("Vui lòng chọn dịch vụ/hàng hóa hợp lệ.");

            if (ct.SoLuong <= 0)
                throw new ArgumentException("Số lượng nhập phải lớn hơn 0.");

            if (ct.DonGiaNhap <= 0)
                throw new ArgumentException("Đơn giá nhập phải lớn hơn 0.");

            if (!KiemTraTonTai(ct.MaCTPN))
                throw new InvalidOperationException($"Không tìm thấy chi tiết phiếu nhập có mã {ct.MaCTPN} để cập nhật.");

            dal.Sua(ct);
        }

        public void Xoa(int maCTPN)
        {
            if (maCTPN <= 0)
                throw new ArgumentException("Mã chi tiết phiếu nhập cần xóa không hợp lệ.");

            if (!KiemTraTonTai(maCTPN))
                throw new InvalidOperationException($"Không tìm thấy chi tiết phiếu nhập có mã {maCTPN} để xóa.");

            dal.Xoa(maCTPN);
        }

        public DataTable TimKiem(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return GetAll();

            return dal.TimKiem(tuKhoa.Trim());
        }

        public decimal TinhTongTien(int maPN)
        {
            if (maPN <= 0) return 0;
            return dal.TinhTongTien(maPN);
        }
    }
}
