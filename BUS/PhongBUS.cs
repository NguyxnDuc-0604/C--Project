using System;
using System.Data;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.BUS
{
    public class PhongBUS
    {
        private readonly PhongDAL dal = new();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public DataTable GetLoaiPhong()
        {
            return dal.GetLoaiPhong();
        }

        public bool KiemTraTonTai(int maPhong)
        {
            DataTable dt = dal.GetAll();
            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt32(row["MaPhong"]) == maPhong)
                    return true;
            }
            return false;
        }

        public void Them(int maPhong, int maLoai, string tenPhong, string viTri, string trangThai)
        {
            if (maPhong <= 0)
                throw new ArgumentException("Mã phòng phải là số nguyên dương.");

            if (maLoai <= 0)
                throw new ArgumentException("Vui lòng chọn loại phòng hợp lệ.");

            if (string.IsNullOrWhiteSpace(tenPhong))
                throw new ArgumentException("Tên phòng không được để trống.");

            if (string.IsNullOrWhiteSpace(trangThai))
                trangThai = "Trống";

            if (KiemTraTonTai(maPhong))
                throw new InvalidOperationException($"Mã phòng {maPhong} đã tồn tại trong hệ thống.");

            dal.Them(maPhong, maLoai, tenPhong.Trim(), viTri?.Trim() ?? "", trangThai.Trim());
        }

        public void Sua(int maPhong, int maLoai, string tenPhong, string viTri, string trangThai)
        {
            if (maPhong <= 0)
                throw new ArgumentException("Mã phòng không hợp lệ.");

            if (maLoai <= 0)
                throw new ArgumentException("Vui lòng chọn loại phòng hợp lệ.");

            if (string.IsNullOrWhiteSpace(tenPhong))
                throw new ArgumentException("Tên phòng không được để trống.");

            if (!KiemTraTonTai(maPhong))
                throw new InvalidOperationException($"Không tìm thấy phòng có mã {maPhong} để sửa.");

            dal.Sua(maPhong, maLoai, tenPhong.Trim(), viTri?.Trim() ?? "", trangThai?.Trim() ?? "Trống");
        }

        public void Xoa(int maPhong)
        {
            if (maPhong <= 0)
                throw new ArgumentException("Mã phòng cần xóa không hợp lệ.");

            if (!KiemTraTonTai(maPhong))
                throw new InvalidOperationException($"Không tìm thấy phòng có mã {maPhong} để xóa.");

            dal.Xoa(maPhong);
        }

        public DataTable TimKiem(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return GetAll();

            return dal.TimKiem(tuKhoa.Trim());
        }
    }
}
