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

        /// <summary>
        /// Kiểm tra xem 1 mã dịch vụ đã tồn tại trong phiếu nhập (DB) chưa.
        /// </summary>
        public bool KiemTraDichVuDaTonTaiTrongPhieu(int maPN, int maDV)
        {
            if (maPN <= 0 || maDV <= 0) return false;
            return dal.KiemTraDichVuDaTonTaiTrongPhieu(maPN, maDV);
        }

        /// <summary>
        /// Nhập NHIỀU dịch vụ cho 1 phiếu nhập trong 1 lần thao tác (mô hình giỏ hàng).
        /// dtGioHang phải có các cột: MaDV (int), TenDV (string), SoLuong (int), DonGia (decimal).
        /// Tự động kiểm tra trùng mã dịch vụ với dữ liệu đã có trong DB.
        /// </summary>
        public void ThemNhieuCTPN(int maPN, System.Data.DataTable dtGioHang)
        {
            if (maPN <= 0)
                throw new ArgumentException("Mã phiếu nhập không hợp lệ.");

            if (dtGioHang == null || dtGioHang.Rows.Count == 0)
                throw new ArgumentException("Giỏ hàng đang trống, vui lòng thêm ít nhất 1 dịch vụ.");

            // Kiểm tra từng dòng trước khi lưu
            foreach (System.Data.DataRow r in dtGioHang.Rows)
            {
                int maDV = Convert.ToInt32(r["MaDV"]);
                string tenDV = r["TenDV"].ToString() ?? "";
                int sl = Convert.ToInt32(r["SoLuong"]);
                decimal dg = Convert.ToDecimal(r["DonGia"]);

                if (sl <= 0)
                    throw new ArgumentException($"Số lượng của dịch vụ '{tenDV}' phải lớn hơn 0.");
                if (dg <= 0)
                    throw new ArgumentException($"Đơn giá của dịch vụ '{tenDV}' phải lớn hơn 0.");

                // Kiểm tra trùng với DB
                if (dal.KiemTraDichVuDaTonTaiTrongPhieu(maPN, maDV))
                    throw new InvalidOperationException(
                        $"Dịch vụ '{tenDV}' đã tồn tại trong phiếu nhập #{maPN}!\n" +
                        "Vui lòng dùng chức năng 'Cập nhật' nếu muốn điều chỉnh số lượng.");
            }

            dal.ThemNhieuCTPN(maPN, dtGioHang);
        }
    }
}
