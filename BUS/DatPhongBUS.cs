using System;
using System.Data;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.BUS
{
    public class DatPhongBUS
    {
        private readonly DatPhongDAL dal = new();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public DataTable GetKhachHang()
        {
            return dal.GetKhachHang();
        }

        public DataTable GetNhanVien()
        {
            return dal.GetNhanVien();
        }

        public int GetNextMaDP()
        {
            return dal.GetNextMaDP();
        }

        public DataTable GetDanhSachPhongTrong()
        {
            return dal.GetDanhSachPhongTrong();
        }

        public DataTable GetChiTietPhongTheoMaDP(int maDP)
        {
            if (maDP <= 0) return new DataTable();
            return dal.GetChiTietPhongTheoMaDP(maDP);
        }

        public bool KiemTraTonTai(int maDP)
        {
            DataTable dt = dal.GetAll();
            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt32(row["MaDP"]) == maDP)
                    return true;
            }
            return false;
        }

        // Lập phiếu đặt phòng kèm nhiều phòng (Mô hình Master - Detail 1-N)
        public void ThemDatPhongKemChiTiet(
            int maDP,
            int maKH,
            int maNV,
            DateTime ngayDat,
            DateTime ngayNhan,
            DateTime ngayTra,
            string trangThai,
            DataTable dtPhongChon)
        {
            if (maDP <= 0)
                throw new ArgumentException("Mã đặt phòng phải là số nguyên dương.");

            if (maKH <= 0)
                throw new ArgumentException("Vui lòng chọn khách hàng.");

            if (maNV <= 0)
                throw new ArgumentException("Vui lòng chọn nhân viên.");

            if (ngayTra.Date <= ngayNhan.Date)
                throw new ArgumentException("Ngày trả phòng phải sau ngày nhận phòng.");

            if (ngayNhan.Date < ngayDat.Date)
                throw new ArgumentException("Ngày nhận phòng không được trước ngày đặt.");

            if (dtPhongChon == null || dtPhongChon.Rows.Count == 0)
                throw new ArgumentException("Vui lòng chọn ít nhất 1 phòng vào danh sách đặt phòng.");

            if (string.IsNullOrWhiteSpace(trangThai))
                trangThai = "Đã xác nhận";

            if (KiemTraTonTai(maDP))
                throw new InvalidOperationException($"Mã đặt phòng {maDP} đã tồn tại trong hệ thống.");

            dal.ThemDatPhongKemChiTiet(maDP, maKH, maNV, ngayDat, ngayNhan, ngayTra, trangThai.Trim(), dtPhongChon);
        }

        public void ThemDatPhong(int maDP, int maKH, int maNV, DateTime ngayDat, DateTime ngayNhan, DateTime ngayTra, string trangThai)
        {
            if (maDP <= 0)
                throw new ArgumentException("Mã đặt phòng phải là số nguyên dương.");

            if (maKH <= 0)
                throw new ArgumentException("Vui lòng chọn khách hàng.");

            if (maNV <= 0)
                throw new ArgumentException("Vui lòng chọn nhân viên.");

            if (ngayTra.Date <= ngayNhan.Date)
                throw new ArgumentException("Ngày trả phòng phải sau ngày nhận phòng.");

            if (ngayNhan.Date < ngayDat.Date)
                throw new ArgumentException("Ngày nhận phòng không được trước ngày đặt.");

            if (string.IsNullOrWhiteSpace(trangThai))
                trangThai = "Chờ xác nhận";

            if (KiemTraTonTai(maDP))
                throw new InvalidOperationException($"Mã đặt phòng {maDP} đã tồn tại trong hệ thống.");

            dal.ThemDatPhong(maDP, maKH, maNV, ngayDat, ngayNhan, ngayTra, trangThai.Trim());
        }

        public void SuaDatPhong(int maDP, int maKH, int maNV, DateTime ngayDat, DateTime ngayNhan, DateTime ngayTra, string trangThai)
        {
            if (maDP <= 0)
                throw new ArgumentException("Mã đặt phòng không hợp lệ.");

            if (maKH <= 0)
                throw new ArgumentException("Vui lòng chọn khách hàng.");

            if (maNV <= 0)
                throw new ArgumentException("Vui lòng chọn nhân viên.");

            if (ngayTra.Date <= ngayNhan.Date)
                throw new ArgumentException("Ngày trả phòng phải sau ngày nhận phòng.");

            if (!KiemTraTonTai(maDP))
                throw new InvalidOperationException($"Không tìm thấy phiếu đặt phòng có mã {maDP} để cập nhật.");

            dal.SuaDatPhong(maDP, maKH, maNV, ngayDat, ngayNhan, ngayTra, trangThai.Trim());
        }

        public void XoaDatPhong(int maDP)
        {
            if (maDP <= 0)
                throw new ArgumentException("Mã đặt phòng cần xóa không hợp lệ.");

            if (!KiemTraTonTai(maDP))
                throw new InvalidOperationException($"Không tìm thấy phiếu đặt phòng có mã {maDP} để xóa.");

            dal.XoaDatPhong(maDP);
        }

        public DataTable TimKiemDatPhong(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return GetAll();

            return dal.TimKiemDatPhong(tuKhoa.Trim());
        }
    }
}
