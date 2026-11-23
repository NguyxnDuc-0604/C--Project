namespace QuanLyKhachSan.Models
{
    public class PhieuNhap
    {
        public int MaPN { get; set; }
        public int MaNCC { get; set; }
        public int MaNV { get; set; }
        public DateTime NgayNhap { get; set; } = DateTime.Now;
    }
}
