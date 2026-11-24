using System;
using System.Data;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.BUS
{
    public class NhaCungCapBUS
    {
        private readonly NhaCungCapDAL dal = new();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public DataTable GetDanhSach()
        {
            return dal.GetDanhSach();
        }

        public bool KiemTraTonTai(int maNCC)
        {
            DataTable dt = dal.GetAll();
            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt32(row["MaNCC"]) == maNCC)
                    return true;
            }
            return false;
        }

        public void Them(string tenNCC, string sdt, string diaChi, string email)
        {
            if (string.IsNullOrWhiteSpace(tenNCC))
                throw new ArgumentException("Tên nhà cung cấp không được để trống.");

            dal.Them(tenNCC.Trim(), sdt?.Trim() ?? "", diaChi?.Trim() ?? "", email?.Trim() ?? "");
        }

        public void Sua(int maNCC, string tenNCC, string sdt, string diaChi, string email)
        {
            if (maNCC <= 0)
                throw new ArgumentException("Mã nhà cung cấp không hợp lệ.");

            if (string.IsNullOrWhiteSpace(tenNCC))
                throw new ArgumentException("Tên nhà cung cấp không được để trống.");

            if (!KiemTraTonTai(maNCC))
                throw new InvalidOperationException($"Không tìm thấy nhà cung cấp có mã {maNCC} để sửa.");

            dal.Sua(maNCC, tenNCC.Trim(), sdt?.Trim() ?? "", diaChi?.Trim() ?? "", email?.Trim() ?? "");
        }

        public void Xoa(int maNCC)
        {
            if (maNCC <= 0)
                throw new ArgumentException("Mã nhà cung cấp cần xóa không hợp lệ.");

            if (!KiemTraTonTai(maNCC))
                throw new InvalidOperationException($"Không tìm thấy nhà cung cấp có mã {maNCC} để xóa.");

            dal.Xoa(maNCC);
        }

        public DataTable TimKiem(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return GetAll();

            return dal.TimKiem(tuKhoa.Trim());
        }
    }
}
