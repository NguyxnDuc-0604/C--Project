
namespace QuanLyKhachSan.Models
{
    public class DatPhong
    {
        public int MaDP { get; set; }

        public int MaKH { get; set; }

        public int MaNV { get; set; }

        public DateTime NgayDat { get; set; }

        public DateTime NgayNhan { get; set; }

        public DateTime NgayTra { get; set; }

        public string TrangThai { get; set; } = "";
    }
}
