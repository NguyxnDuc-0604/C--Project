
using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyKhachSan.Database;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.DAL
{
    public class DichVuDAL
    {
        // Lấy danh sách dịch vụ
        public DataTable GetAll()
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = "SELECT * FROM DICH_VU ORDER BY MaDV";

            using SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        // Thêm dịch vụ
        public void Them(DichVu dv)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            string sql = @"
                INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa)
                VALUES (@MaDV, @TenDV, @DonGia, @DonViTinh, @MoTa)";

            using SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@MaDV", SqlDbType.Int).Value = dv.MaDV;
            cmd.Parameters.Add("@TenDV", SqlDbType.NVarChar, 100).Value = dv.TenDV;
            cmd.Parameters.Add("@DonGia", SqlDbType.Decimal).Value = dv.DonGia;
            cmd.Parameters.Add("@DonViTinh", SqlDbType.NVarChar, 50).Value =
                (object?)dv.DonViTinh ?? DBNull.Value;
            cmd.Parameters.Add("@MoTa", SqlDbType.NVarChar, 200).Value =
                (object?)dv.MoTa ?? DBNull.Value;

            cmd.ExecuteNonQuery();
        }

        // Sửa dịch vụ
        public void Sua(DichVu dv)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            string sql = @"
                UPDATE DICH_VU SET
                    TenDV = @TenDV,
                    DonGia = @DonGia,
                    DonViTinh = @DonViTinh,
                    MoTa = @MoTa
                WHERE MaDV = @MaDV";

            using SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.Add("@MaDV", SqlDbType.Int).Value = dv.MaDV;
            cmd.Parameters.Add("@TenDV", SqlDbType.NVarChar, 100).Value = dv.TenDV;
            cmd.Parameters.Add("@DonGia", SqlDbType.Decimal).Value = dv.DonGia;
            cmd.Parameters.Add("@DonViTinh", SqlDbType.NVarChar, 50).Value =
                (object?)dv.DonViTinh ?? DBNull.Value;
            cmd.Parameters.Add("@MoTa", SqlDbType.NVarChar, 200).Value =
                (object?)dv.MoTa ?? DBNull.Value;

            cmd.ExecuteNonQuery();
        }

        // Xóa dịch vụ
        public void Xoa(int maDV)
        {
            using SqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            string sql = "DELETE FROM DICH_VU WHERE MaDV = @MaDV";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@MaDV", SqlDbType.Int).Value = maDV;

            cmd.ExecuteNonQuery();
        }

        // Tìm kiếm dịch vụ
        public DataTable TimKiem(string tuKhoa)
        {
            using SqlConnection conn = DBConnection.GetConnection();

            string sql = @"
                SELECT * FROM DICH_VU
                WHERE TenDV LIKE @TuKhoa
                   OR CAST(MaDV AS NVARCHAR(20)) LIKE @TuKhoa
                ORDER BY MaDV";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@TuKhoa", SqlDbType.NVarChar, 100)
                .Value = "%" + tuKhoa + "%";

            using SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }
    }
}
