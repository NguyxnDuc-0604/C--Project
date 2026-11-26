using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyKhachSan.Database;

namespace QuanLyKhachSan.DAL
{
    public class NhanVienDAL
    {
        public DataTable GetAll() { using var c=DBConnection.GetConnection(); using var a=new SqlDataAdapter("SELECT MaNV,HoTen,SDT,ChucVu,TaiKhoan FROM NHAN_VIEN",c); var d=new DataTable(); a.Fill(d); return d; }
        public DataTable TimKiem(string q) { using var c=DBConnection.GetConnection(); using var cmd=new SqlCommand("SELECT MaNV,HoTen,SDT,ChucVu,TaiKhoan FROM NHAN_VIEN WHERE HoTen LIKE @q OR SDT LIKE @q OR TaiKhoan LIKE @q OR CAST(MaNV AS NVARCHAR(20)) LIKE @q",c); cmd.Parameters.AddWithValue("@q","%"+q+"%"); using var a=new SqlDataAdapter(cmd); var d=new DataTable(); a.Fill(d); return d; }
        public void Them(int ma,string ten,string sdt,string chucVu,string taiKhoan,string matKhau) => Execute("INSERT INTO NHAN_VIEN (MaNV,HoTen,SDT,ChucVu,TaiKhoan,MatKhau) VALUES (@ma,@ten,@sdt,@cv,@tk,@mk)",ma,ten,sdt,chucVu,taiKhoan,matKhau);
        public void Sua(int ma,string ten,string sdt,string chucVu,string taiKhoan,string matKhau) => Execute("UPDATE NHAN_VIEN SET HoTen=@ten,SDT=@sdt,ChucVu=@cv,TaiKhoan=@tk,MatKhau=CASE WHEN NULLIF(@mk,N'') IS NULL THEN MatKhau ELSE @mk END WHERE MaNV=@ma",ma,ten,sdt,chucVu,taiKhoan,matKhau);
        public void Xoa(int ma) { using var c=DBConnection.GetConnection(); using var cmd=new SqlCommand("DELETE FROM NHAN_VIEN WHERE MaNV=@ma",c); cmd.Parameters.AddWithValue("@ma",ma); c.Open(); cmd.ExecuteNonQuery(); }
        private static void Execute(string sql,int ma,string ten,string sdt,string cv,string tk,string mk) { using var c=DBConnection.GetConnection(); using var cmd=new SqlCommand(sql,c); cmd.Parameters.AddWithValue("@ma",ma); cmd.Parameters.AddWithValue("@ten",ten); cmd.Parameters.AddWithValue("@sdt",string.IsNullOrWhiteSpace(sdt)?DBNull.Value:sdt); cmd.Parameters.AddWithValue("@cv",string.IsNullOrWhiteSpace(cv)?DBNull.Value:cv); cmd.Parameters.AddWithValue("@tk",string.IsNullOrWhiteSpace(tk)?DBNull.Value:tk); cmd.Parameters.AddWithValue("@mk",string.IsNullOrWhiteSpace(mk)?DBNull.Value:mk); c.Open(); cmd.ExecuteNonQuery(); }
    }
}
