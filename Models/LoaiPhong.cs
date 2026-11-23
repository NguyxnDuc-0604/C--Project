namespace QuanLyKhachSan.Models
{
    public class LoaiPhong
    {
        public int MaLoai { get; set; }
        public string TenLoai { get; set; } = "";
        public decimal GiaPhong { get; set; }
        public int SoNguoiToiDa { get; set; }
        public string? MoTa { get; set; }
        public int SoLuongPhong { get; set; }
    }
}
