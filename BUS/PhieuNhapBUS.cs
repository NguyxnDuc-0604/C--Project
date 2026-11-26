using System;
using System.Data;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.BUS
{
    public class PhieuNhapBUS
    {
        private readonly PhieuNhapDAL dal = new();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public DataTable GetNhaCungCap()
        {
            return dal.GetNhaCungCap();
        }

        public DataTable GetNhanVien()
        {
            return dal.GetNhanVien();
        }

        public bool KiemTraTonTai(int maPN)
        {
            DataTable dt = dal.GetAll();
            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt32(row["MaPN"]) == maPN)
                    return true;
            }
            return false;
        }

        public void Them(int maNCC, int maNV, DateTime ngayNhap)
        {
            if (maNCC <= 0)
                throw new ArgumentException("Vui lòng chọn nhà cung cấp hợp lệ.");

            if (maNV <= 0)
                throw new ArgumentException("Vui lòng chọn nhân viên lập phiếu.");

            dal.Them(maNCC, maNV, ngayNhap);
        }

        public void Sua(int maPN, int maNCC, int maNV, DateTime ngayNhap)
        {
            if (maPN <= 0)
                throw new ArgumentException("Mã phiếu nhập không hợp lệ.");

            if (maNCC <= 0)
                throw new ArgumentException("Vui lòng chọn nhà cung cấp hợp lệ.");

            if (maNV <= 0)
                throw new ArgumentException("Vui lòng chọn nhân viên lập phiếu.");

            if (!KiemTraTonTai(maPN))
                throw new InvalidOperationException($"Không tìm thấy phiếu nhập có mã {maPN} để sửa.");

            dal.Sua(maPN, maNCC, maNV, ngayNhap);
        }

        public void Xoa(int maPN)
        {
            if (maPN <= 0)
                throw new ArgumentException("Mã phiếu nhập cần xóa không hợp lệ.");

            if (!KiemTraTonTai(maPN))
                throw new InvalidOperationException($"Không tìm thấy phiếu nhập có mã {maPN} để xóa.");

            dal.Xoa(maPN);
        }

        public DataTable TimKiem(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return GetAll();

            return dal.TimKiem(tuKhoa.Trim());
        }
    }
}
