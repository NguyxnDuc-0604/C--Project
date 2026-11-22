namespace QuanLyKhachSan.Models
{
    public class Phong
    {
        public int MaPhong { get; set; }
        public int MaLoai { get; set; }
        public string TenPhong { get; set; } = "";
        public string? ViTri { get; set; }
        public string TrangThai { get; set; } = "Trống";
    }
}
