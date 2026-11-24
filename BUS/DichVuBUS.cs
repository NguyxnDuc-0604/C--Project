using System;
using System.Data;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.BUS
{
    public class DichVuBUS
    {
        private readonly DichVuDAL dal = new();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public bool KiemTraTonTai(int maDV)
        {
            DataTable dt = dal.GetAll();
            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt32(row["MaDV"]) == maDV)
                    return true;
            }
            return false;
        }

        public void Them(DichVu dv)
        {
            if (dv == null)
                throw new ArgumentNullException(nameof(dv));

            if (dv.MaDV <= 0)
                throw new ArgumentException("Mã dịch vụ phải là số nguyên dương.");

            if (string.IsNullOrWhiteSpace(dv.TenDV))
                throw new ArgumentException("Tên dịch vụ không được để trống.");

            if (dv.DonGia <= 0)
                throw new ArgumentException("Đơn giá dịch vụ phải lớn hơn 0.");

            if (KiemTraTonTai(dv.MaDV))
                throw new InvalidOperationException($"Mã dịch vụ {dv.MaDV} đã tồn tại trong hệ thống.");

            dal.Them(dv);
        }

        public void Sua(DichVu dv)
        {
            if (dv == null)
                throw new ArgumentNullException(nameof(dv));

            if (dv.MaDV <= 0)
                throw new ArgumentException("Mã dịch vụ không hợp lệ.");

            if (string.IsNullOrWhiteSpace(dv.TenDV))
                throw new ArgumentException("Tên dịch vụ không được để trống.");

            if (dv.DonGia <= 0)
                throw new ArgumentException("Đơn giá dịch vụ phải lớn hơn 0.");

            if (!KiemTraTonTai(dv.MaDV))
                throw new InvalidOperationException($"Không tìm thấy dịch vụ có mã {dv.MaDV} để cập nhật.");

            dal.Sua(dv);
        }

        public void Xoa(int maDV)
        {
            if (maDV <= 0)
                throw new ArgumentException("Mã dịch vụ cần xóa không hợp lệ.");

            if (!KiemTraTonTai(maDV))
                throw new InvalidOperationException($"Không tìm thấy dịch vụ có mã {maDV} để xóa.");

            dal.Xoa(maDV);
        }

        public DataTable TimKiem(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return GetAll();

            return dal.TimKiem(tuKhoa.Trim());
        }
    }
}
