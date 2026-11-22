
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
            Size = new Size(700, 550);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;

            TaoGiaoDien();
        }

        private void TaoGiaoDien()
        {
            Label lblTitle = new Label
        
            
            {
                Text = "QUẢN LÝ KHÁCH SẠN",
                Font = new Font("Time New Roman", 19, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                AutoSize = true,
                Location = new Point(190, 35)
            };

            Controls.Add(lblTitle);

            TaoButton("Quản lý khách hàng", 180, 110,
                () => MoForm(new FrmKhachHang()));

            TaoButton("Quản lý đặt phòng", 180, 175,
                () => MoForm(new FrmDatPhong()));

            TaoButton("Chi tiết đặt phòng", 180, 240,
                () => MoForm(new FrmChiTietDatPhong()));

            TaoButton("Thoát chương trình", 180, 370, () =>
            {
                Close();
            });
        }

        private void TaoButton(string ten, int x, int y, Action action)
        {
            Button btn = new Button
            {
                Text = ten,
                Location = new Point(x, y),
                Size = new Size(320, 45),
                Font = new Font("Arial", 12, FontStyle.Bold),
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
