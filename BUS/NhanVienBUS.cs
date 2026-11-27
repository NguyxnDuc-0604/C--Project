using System;
using System.Data;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.BUS
{
    public class NhanVienBUS
    {
        private readonly NhanVienDAL dal = new();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public bool KiemTraTonTai(int maNV)
        {
            DataTable dt = dal.GetAll();
            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt32(row["MaNV"]) == maNV)
                    return true;
            }
            return false;
        }

        public void Them(int maNV, string ten, string sdt, string chucVu, string taiKhoan, string matKhau)
        {
            if (maNV <= 0)
                throw new ArgumentException("Mã nhân viên phải là số nguyên dương.");

            if (string.IsNullOrWhiteSpace(ten))
                throw new ArgumentException("Tên nhân viên không được để trống.");

            if (KiemTraTonTai(maNV))
                throw new InvalidOperationException($"Mã nhân viên {maNV} đã tồn tại trong hệ thống.");

            dal.Them(maNV, ten.Trim(), sdt?.Trim() ?? "", chucVu?.Trim() ?? "", taiKhoan?.Trim() ?? "", matKhau?.Trim() ?? "");
        }

        public void Sua(int maNV, string ten, string sdt, string chucVu, string taiKhoan, string matKhau)
        {
            if (maNV <= 0)
                throw new ArgumentException("Mã nhân viên không hợp lệ.");

            if (string.IsNullOrWhiteSpace(ten))
                throw new ArgumentException("Tên nhân viên không được để trống.");

            if (!KiemTraTonTai(maNV))
                throw new InvalidOperationException($"Không tìm thấy nhân viên có mã {maNV} để cập nhật.");

            dal.Sua(maNV, ten.Trim(), sdt?.Trim() ?? "", chucVu?.Trim() ?? "", taiKhoan?.Trim() ?? "", matKhau?.Trim() ?? "");
        }

        public void Xoa(int maNV)
        {
            if (maNV <= 0)
                throw new ArgumentException("Mã nhân viên cần xóa không hợp lệ.");

            if (!KiemTraTonTai(maNV))
                throw new InvalidOperationException($"Không tìm thấy nhân viên có mã {maNV} để xóa.");

            dal.Xoa(maNV);
        }

        public DataTable TimKiem(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return GetAll();

            return dal.TimKiem(tuKhoa.Trim());
        }
    }
}
