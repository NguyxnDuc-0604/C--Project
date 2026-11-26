using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyKhachSan.Forms
{
    /// <summary>Main tabbed workspace. Feature forms are created only when first opened.</summary>
    public class FrmMain : Form
    {
        private readonly TabControl _tabs = new()
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10F),
            Padding = new Point(14, 6),
            SizeMode = TabSizeMode.Normal
        };

        public FrmMain()
        {
            Text = "HỆ THỐNG QUẢN LÝ KHÁCH SẠN";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(1100, 760);
            BackColor = Color.WhiteSmoke;
            Controls.Add(_tabs);

            AddPage("Khách hàng", () => new FrmKhachHang());
            AddPage("Phòng", () => new FrmPhong());
            AddPage("Loại phòng", () => new FrmLoaiPhong());
            AddPage("Nhân viên", () => new FrmNhanVien());
            AddPage("Đặt phòng", () => new FrmDatPhong());
            AddPage("Chi tiết đặt phòng", () => new FrmChiTietDatPhong());
            AddPage("Dịch vụ", () => new FrmDichVu());
            AddPage("Hóa đơn", () => new FrmHoaDon());
            AddPage("Chi tiết hóa đơn", () => new FrmChiTietHoaDon());
            AddPage("Nhà cung cấp", () => new FrmNhaCungCap());
            AddPage("Phiếu nhập", () => new FrmPhieuNhap());
            AddPage("Chi tiết phiếu nhập", () => new FrmCTPhieuNhap());

            _tabs.SelectedIndexChanged += (_, _) => LoadSelectedPage();
            LoadSelectedPage();
        }

        private void AddPage(string title, Func<Form> createForm)
        {
            var page = new TabPage(title)
            {
                BackColor = Color.WhiteSmoke,
                Tag = createForm,
                Padding = new Padding(0)
            };
            _tabs.TabPages.Add(page);
        }

        private void LoadSelectedPage()
        {
            if (_tabs.SelectedTab is not { } page || page.Controls.Count > 0)
                return;

            if (page.Tag is not Func<Form> createForm)
                return;

            var form = createForm();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.TopMost = false;

            // Existing screens use fixed coordinates; let their data grids grow with the tab.
            SetResponsiveGridAnchors(form.Controls);

            page.Controls.Add(form);
            form.Show();
        }

        private static void SetResponsiveGridAnchors(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is DataGridView)
                    control.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

                if (control.HasChildren)
                    SetResponsiveGridAnchors(control.Controls);
            }
        }
    }
}
