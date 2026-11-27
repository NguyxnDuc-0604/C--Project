
namespace QuanLyKhachSan.Models
{
    public class DichVu
    {
        public int MaDV { get; set; }
        public string TenDV { get; set; } = "";
        public decimal DonGia { get; set; }
        public string? DonViTinh { get; set; }
        public string? MoTa { get; set; }
    }
}
