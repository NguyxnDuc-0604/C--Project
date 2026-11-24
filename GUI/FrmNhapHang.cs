using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyKhachSan.GUI
{
    /// <summary>
    /// TẦNG GUI: FORM ĐIỀU HƯỚNG TRUNG TÂM QUẢN LÝ NHẬP HÀNG
    /// Nhiệm vụ: Tích hợp 3 màn hình Nhà Cung Cấp, Phiếu Nhập, Chi Tiết Phiếu Nhập
    /// </summary>
    public class FrmNhapHang : Form
    {
        public FrmNhapHang()
        {
            Text = "HỆ THỐNG QUẢN LÝ NHẬP HÀNG - [TẦNG GUI]";
            Size = new Size(1180, 760);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            KhoiTaoGiaoDien();
        }

        private void KhoiTaoGiaoDien()
        {
            // Header
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.FromArgb(24, 43, 73)
            };

            Label lblTitle = new Label
            {
                Text = "HỆ THỐNG QUẢN LÝ NHẬP HÀNG KHÁCH SẠN (MÔ HÌNH 3 LỚP - GUI)",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 18),
                AutoSize = true
            };
            pnlHeader.Controls.Add(lblTitle);
            Controls.Add(pnlHeader);

            // TabControl chứa 3 màn hình
            TabControl tabMain = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            // 1. Tab Nhà Cung Cấp
            TabPage tabNCC = new TabPage("  1. Nhà Cung Cấp  ");
            NhungFormVaoTab(tabNCC, new GUI_NhaCungCap());
            tabMain.TabPages.Add(tabNCC);

            // 2. Tab Phiếu Nhập
            TabPage tabPN = new TabPage("  2. Phiếu Nhập  ");
            NhungFormVaoTab(tabPN, new GUI_PhieuNhap());
            tabMain.TabPages.Add(tabPN);

            // 3. Tab Chi Tiết Phiếu Nhập
            TabPage tabCT = new TabPage("  3. Chi Tiết Phiếu Nhập  ");
            NhungFormVaoTab(tabCT, new GUI_CTPhieuNhap());
            tabMain.TabPages.Add(tabCT);

            Controls.Add(tabMain);
        }

        private void NhungFormVaoTab(TabPage tab, Form form)
        {
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            tab.Controls.Add(form);
            form.Show();
        }
    }
}
