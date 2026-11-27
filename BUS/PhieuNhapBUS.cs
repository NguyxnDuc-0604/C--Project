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

        public DataTable GetDanhSachDichVu()
        {
            return dal.GetDanhSachDichVu();
        }

        public DataTable GetChiTietTheoMaPN(int maPN)
        {
            if (maPN <= 0) return new DataTable();
            return dal.GetChiTietTheoMaPN(maPN);
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

        // Lập phiếu nhập kèm nhiều mặt hàng (Mô hình Master - Detail 1-N)
        public int ThemPhieuNhapKemChiTiet(int maNCC, int maNV, DateTime ngayNhap, DataTable dtHangNhap)
        {
            if (maNCC <= 0)
                throw new ArgumentException("Vui lòng chọn nhà cung cấp hợp lệ.");

            if (maNV <= 0)
                throw new ArgumentException("Vui lòng chọn nhân viên lập phiếu.");

            if (dtHangNhap == null || dtHangNhap.Rows.Count == 0)
                throw new ArgumentException("Vui lòng thêm ít nhất 1 mặt hàng vào phiếu nhập.");

            foreach (DataRow r in dtHangNhap.Rows)
            {
                int sl = Convert.ToInt32(r["SoLuong"]);
                decimal dg = Convert.ToDecimal(r["DonGiaNhap"]);
                if (sl <= 0)
                    throw new ArgumentException($"Số lượng cho mặt hàng '{r["TenDV"]}' phải lớn hơn 0.");
                if (dg < 0)
                    throw new ArgumentException($"Đơn giá nhập cho mặt hàng '{r["TenDV"]}' không được âm.");
            }

            return dal.ThemPhieuNhapKemChiTiet(maNCC, maNV, ngayNhap, dtHangNhap);
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
