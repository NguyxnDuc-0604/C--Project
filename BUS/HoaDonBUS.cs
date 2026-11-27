using System;
using System.Data;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.BUS
{
    public class HoaDonBUS
    {
        private readonly HoaDonDAL dal = new();

        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public DataTable GetDanhSachDatPhong()
        {
            return dal.GetDanhSachDatPhong();
        }

        public DataTable GetDanhSachNhanVien()
        {
            return dal.GetDanhSachNhanVien();
        }

        public bool KiemTraTonTai(int maHD)
        {
            DataTable dt = dal.GetAll();
            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt32(row["MaHD"]) == maHD)
                    return true;
            }
            return false;
        }

        public void Them(HoaDon hd)
        {
            if (hd == null)
                throw new ArgumentNullException(nameof(hd));

            if (hd.MaHD <= 0)
                throw new ArgumentException("Mã hóa đơn phải là số nguyên dương.");

            if (hd.MaDP <= 0)
                throw new ArgumentException("Vui lòng chọn phiếu đặt phòng.");

            if (hd.MaNV <= 0)
                throw new ArgumentException("Vui lòng chọn nhân viên lập hóa đơn.");

            if (KiemTraTonTai(hd.MaHD))
                throw new InvalidOperationException($"Mã hóa đơn {hd.MaHD} đã tồn tại trong hệ thống.");

            dal.Them(hd);
        }

        public void Sua(HoaDon hd)
        {
            if (hd == null)
                throw new ArgumentNullException(nameof(hd));

            if (hd.MaHD <= 0)
                throw new ArgumentException("Mã hóa đơn không hợp lệ.");

            if (hd.MaDP <= 0)
                throw new ArgumentException("Vui lòng chọn phiếu đặt phòng.");

            if (hd.MaNV <= 0)
                throw new ArgumentException("Vui lòng chọn nhân viên lập hóa đơn.");

            if (!KiemTraTonTai(hd.MaHD))
                throw new InvalidOperationException($"Không tìm thấy hóa đơn có mã {hd.MaHD} để cập nhật.");

            dal.Sua(hd);
        }

        public void Xoa(int maHD)
        {
            if (maHD <= 0)
                throw new ArgumentException("Mã hóa đơn cần xóa không hợp lệ.");

            if (!KiemTraTonTai(maHD))
                throw new InvalidOperationException($"Không tìm thấy hóa đơn có mã {maHD} để xóa.");

            dal.Xoa(maHD);
        }

        public DataTable TimKiem(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return GetAll();

            return dal.TimKiem(tuKhoa.Trim());
        }

        public decimal TinhTongTien(int maHD)
        {
            if (maHD <= 0) return 0;
            return dal.TinhTongTien(maHD);
        }

        /// <summary>
        /// Tìm kiếm nâng cao đa tiêu chí (AND, OR, >, >=, <, <=) theo yêu cầu Tuần 7 số 8
        /// </summary>
        public DataTable TimKiemNangCao(string tuKhoa, decimal? giaTu, decimal? giaDen, DateTime? tuNgay, DateTime? denNgay)
        {
            DataTable dt = GetAll();
            if (dt.Rows.Count == 0) return dt;

            DataView dv = dt.DefaultView;
            System.Collections.Generic.List<string> filters = new();

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                string kw = tuKhoa.Trim().Replace("'", "''");
                filters.Add($"(Convert(MaHD, 'System.String') LIKE '%{kw}%' OR TenKhachHang LIKE '%{kw}%' OR TenNhanVien LIKE '%{kw}%' OR GhiChu LIKE '%{kw}%')");
            }

            if (giaTu.HasValue)
                filters.Add($"TongTien >= {giaTu.Value}");

            if (giaDen.HasValue)
                filters.Add($"TongTien <= {giaDen.Value}");

            if (tuNgay.HasValue)
                filters.Add($"NgayLap >= #{tuNgay.Value:yyyy-MM-dd}#");

            if (denNgay.HasValue)
                filters.Add($"NgayLap <= #{denNgay.Value:yyyy-MM-dd} 23:59:59#");

            if (filters.Count > 0)
                dv.RowFilter = string.Join(" AND ", filters);
            else
                dv.RowFilter = "";

            return dv.ToTable();
        }

        /// <summary>
        /// Tìm kiếm duyệt trên ArrayList / List đối tượng trong bộ nhớ (Yêu cầu Tuần 7 số 2)
        /// </summary>
        public System.Collections.ArrayList TimKiemTrenArrayList(System.Collections.ArrayList danhSach, string tuKhoa)
        {
            System.Collections.ArrayList ketQua = new();
            if (danhSach == null) return ketQua;

            foreach (var item in danhSach)
            {
                if (item is HoaDon hd)
                {
                    if (string.IsNullOrWhiteSpace(tuKhoa) ||
                        hd.MaHD.ToString().Contains(tuKhoa) ||
                        (!string.IsNullOrEmpty(hd.GhiChu) && hd.GhiChu.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase)))
                    {
                        ketQua.Add(hd);
                    }
                }
            }
            return ketQua;
        }
    }
}
