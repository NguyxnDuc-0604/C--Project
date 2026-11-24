using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyKhachSan.Forms
{
    public class FrmMain : Form
    {
        public FrmMain()
        {
            Text = "HỆ THỐNG QUẢN LÝ KHÁCH SẠN";
            Size = new Size(820, 620);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            TaoGiaoDien();
        }

        private void TaoGiaoDien()
        {
            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ KHÁCH SẠN",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                AutoSize = true,
                Location = new Point(260, 25)
            };
            Controls.Add(lblTitle);

            // Cột 1 (Bên trái: Đức & Nhật Anh)
            TaoButton("Quản lý loại phòng", 60, 85, () => MoForm(new FrmLoaiPhong()));
            TaoButton("Quản lý phòng", 60, 145, () => MoForm(new FrmPhong()));
            TaoButton("Quản lý nhân viên", 60, 205, () => MoForm(new FrmNhanVien()));
            TaoButton("Quản lý khách hàng", 60, 265, () => MoForm(new FrmKhachHang()));
            TaoButton("Quản lý đặt phòng", 60, 325, () => MoForm(new FrmDatPhong()));
            TaoButton("Chi tiết đặt phòng", 60, 385, () => MoForm(new FrmChiTietDatPhong()));

            // Cột 2 (Bên phải: Khoa & Vinh)
            TaoButton("Quản lý dịch vụ", 420, 85, () => MoForm(new FrmDichVu()));
            TaoButton("Quản lý hóa đơn", 420, 145, () => MoForm(new FrmHoaDon()));
            TaoButton("Chi tiết hóa đơn", 420, 205, () => MoForm(new FrmChiTietHoaDon()));
            TaoButton("Quản lý nhà cung cấp", 420, 265, () => MoForm(new FrmNhaCungCap()));
            TaoButton("Quản lý phiếu nhập", 420, 325, () => MoForm(new FrmPhieuNhap()));
            TaoButton("Chi tiết phiếu nhập", 420, 385, () => MoForm(new FrmCTPhieuNhap()));

            // Nút Thoát (Ở giữa dưới cùng)
            Button btnExit = new Button
            {
                Text = "Thoát chương trình",
                Location = new Point(240, 470),
                Size = new Size(320, 45),
                Font = new Font("Arial", 11, FontStyle.Bold),
                BackColor = Color.LightCoral,
                ForeColor = Color.White
            };
            btnExit.Click += (s, e) => Close();
            Controls.Add(btnExit);
        }

        private void TaoButton(string ten, int x, int y, Action action)
        {
            Button btn = new Button
            {
                Text = ten,
                Location = new Point(x, y),
                Size = new Size(320, 45),
                Font = new Font("Arial", 11, FontStyle.Bold),
                BackColor = Color.LightSteelBlue
            };

            btn.Click += (s, e) => action();
            Controls.Add(btn);
        }

        private void MoForm(Form form)
        {
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog();
        }
    }
}
