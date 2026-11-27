using System;
using System.Data;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.BUS
{
    public class LoaiPhongBUS
    {
        private readonly LoaiPhongDAL dal = new();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public bool KiemTraTonTai(int maLoai)
        {
            DataTable dt = dal.GetAll();
            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt32(row["MaLoai"]) == maLoai)
                    return true;
            }
            return false;
        }

        public void Them(int ma, string ten, decimal gia, int soNguoi, string moTa, int soLuong)
        {
            if (ma <= 0)
                throw new ArgumentException("Mã loại phòng phải là số nguyên dương.");

            if (string.IsNullOrWhiteSpace(ten))
                throw new ArgumentException("Tên loại phòng không được để trống.");

            if (gia < 0)
                throw new ArgumentException("Giá phòng không được là số âm.");

            if (soNguoi <= 0)
                throw new ArgumentException("Số người tối đa phải lớn hơn 0.");

            if (soLuong < 0)
                throw new ArgumentException("Số lượng phòng không được là số âm.");

            if (KiemTraTonTai(ma))
                throw new InvalidOperationException($"Mã loại phòng {ma} đã tồn tại trong hệ thống.");

            dal.Them(ma, ten.Trim(), gia, soNguoi, moTa?.Trim() ?? "", soLuong);
        }

        public void Sua(int ma, string ten, decimal gia, int soNguoi, string moTa, int soLuong)
        {
            if (ma <= 0)
                throw new ArgumentException("Mã loại phòng không hợp lệ.");

            if (string.IsNullOrWhiteSpace(ten))
                throw new ArgumentException("Tên loại phòng không được để trống.");

            if (gia < 0)
                throw new ArgumentException("Giá phòng không được là số âm.");

            if (soNguoi <= 0)
                throw new ArgumentException("Số người tối đa phải lớn hơn 0.");

            if (soLuong < 0)
                throw new ArgumentException("Số lượng phòng không được là số âm.");

            if (!KiemTraTonTai(ma))
                throw new InvalidOperationException($"Không tìm thấy loại phòng có mã {ma} để sửa.");

            dal.Sua(ma, ten.Trim(), gia, soNguoi, moTa?.Trim() ?? "", soLuong);
        }

        public void Xoa(int ma)
        {
            if (ma <= 0)
                throw new ArgumentException("Mã loại phòng cần xóa không hợp lệ.");

            if (!KiemTraTonTai(ma))
                throw new InvalidOperationException($"Không tìm thấy loại phòng có mã {ma} để xóa.");

            dal.Xoa(ma);
        }

        public DataTable TimKiem(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return GetAll();

            return dal.TimKiem(tuKhoa.Trim());
        }
    }
}
