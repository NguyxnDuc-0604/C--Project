using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyKhachSan.Database;

namespace QuanLyKhachSan.DAL
{
    public class PhongDAL
    {
        public DataTable GetAll() => Query("SELECT P.MaPhong, P.MaLoai, L.TenLoai, P.TenPhong, P.ViTri, P.TrangThai FROM PHONG P INNER JOIN LOAI_PHONG L ON P.MaLoai=L.MaLoai");
        public DataTable GetLoaiPhong() => Query("SELECT MaLoai, TenLoai FROM LOAI_PHONG");
        public DataTable TimKiem(string q)
        {
            using var conn = DBConnection.GetConnection();
            using var cmd = new SqlCommand("SELECT P.MaPhong, P.MaLoai, L.TenLoai, P.TenPhong, P.ViTri, P.TrangThai FROM PHONG P INNER JOIN LOAI_PHONG L ON P.MaLoai=L.MaLoai WHERE P.TenPhong LIKE @q OR CAST(P.MaPhong AS NVARCHAR(20)) LIKE @q OR L.TenLoai LIKE @q", conn);
            cmd.Parameters.AddWithValue("@q", "%" + q + "%"); using var a = new SqlDataAdapter(cmd); var dt = new DataTable(); a.Fill(dt); return dt;
        }
        private static DataTable Query(string sql) { using var c=DBConnection.GetConnection(); using var a=new SqlDataAdapter(sql,c); var d=new DataTable(); a.Fill(d); return d; }
        public void Them(int ma, int loai, string ten, string viTri, string trangThai) => Execute("INSERT INTO PHONG (MaPhong,MaLoai,TenPhong,ViTri,TrangThai) VALUES (@ma,@loai,@ten,@vt,@tt)", ma,loai,ten,viTri,trangThai);
        public void Sua(int ma, int loai, string ten, string viTri, string trangThai) => Execute("UPDATE PHONG SET MaLoai=@loai,TenPhong=@ten,ViTri=@vt,TrangThai=@tt WHERE MaPhong=@ma", ma,loai,ten,viTri,trangThai);
        public void Xoa(int ma) { using var c=DBConnection.GetConnection(); using var cmd=new SqlCommand("DELETE FROM PHONG WHERE MaPhong=@ma",c); cmd.Parameters.AddWithValue("@ma",ma); c.Open(); cmd.ExecuteNonQuery(); }
        private static void Execute(string sql,int ma,int loai,string ten,string viTri,string tt) { using var c=DBConnection.GetConnection(); using var cmd=new SqlCommand(sql,c); cmd.Parameters.AddWithValue("@ma",ma); cmd.Parameters.AddWithValue("@loai",loai); cmd.Parameters.AddWithValue("@ten",ten); cmd.Parameters.AddWithValue("@vt",string.IsNullOrWhiteSpace(viTri)?DBNull.Value:viTri); cmd.Parameters.AddWithValue("@tt",tt); c.Open(); cmd.ExecuteNonQuery(); }
    }
}
