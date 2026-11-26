using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyKhachSan.Forms
{
    public class FrmMain : Form
    {
        private Panel panelSidebar = null!;
        private Panel panelTop = null!;
        private Panel panelBody = null!;
        private Label lblHeaderTitle = null!;

        public FrmMain()
        {
            Text = "HỆ THỐNG QUẢN LÝ KHÁCH SẠN (MÔ HÌNH 3 LỚP - ARCHITECTURE 3-LAYER)";
            Size = new Size(1300, 850);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;
            Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            KhoiTaoGiaoDienDesktop();
        }

        private void KhoiTaoGiaoDienDesktop()
        {
            // 1. Sidebar Bên Trái (Menu Left)
            panelSidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = Color.FromArgb(24, 34, 56),
                Padding = new Padding(0, 0, 0, 0)
            };
            Controls.Add(panelSidebar);

            // Logo & Tiêu đề ứng dụng ở Sidebar
            Panel pnlLogo = new Panel
            {
                Dock = DockStyle.Top,
                Height = 85,
                BackColor = Color.FromArgb(18, 25, 42)
            };
            Label lblLogo = new Label
            {
                Text = "🏨 SAIGON HOTEL\nQUẢN LÝ KHÁCH SẠN",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            pnlLogo.Controls.Add(lblLogo);
            panelSidebar.Controls.Add(pnlLogo);

            // Các nút Menu chức năng ở Sidebar
            TaoNutMenu("🏠 Bàn làm việc (Dashboard)", () => HienThiDashboard(), pnlLogo);
            TaoNutMenu("🛏️ Quản lý Loại phòng", () => MoFormCon(new FrmLoaiPhong(), "QUẢN LÝ LOẠI PHÒNG"));
            TaoNutMenu("🚪 Quản lý Phòng", () => MoFormCon(new FrmPhong(), "QUẢN LÝ PHÒNG"));
            TaoNutMenu("👥 Quản lý Nhân viên", () => MoFormCon(new FrmNhanVien(), "QUẢN LÝ NHÂN VIÊN"));
            TaoNutMenu("👤 Quản lý Khách hàng", () => MoFormCon(new FrmKhachHang(), "QUẢN LÝ KHÁCH HÀNG"));
            TaoNutMenu("📅 Quản lý Đặt phòng", () => MoFormCon(new FrmDatPhong(), "QUẢN LÝ ĐẶT PHÒNG"));
            TaoNutMenu("📋 Chi tiết Đặt phòng", () => MoFormCon(new FrmChiTietDatPhong(), "CHI TIẾT ĐẶT PHÒNG"));
            TaoNutMenu("☕ Quản lý Dịch vụ", () => MoFormCon(new FrmDichVu(), "QUẢN LÝ DỊCH VỤ KHÁCH SẠN"));
            TaoNutMenu("🧾 Quản lý Hóa đơn (Main-Sub)", () => MoFormCon(new FrmHoaDon(), "QUẢN LÝ HÓA ĐƠN & THANH TOÁN (MAIN-SUB 1-N)"));
            TaoNutMenu("📝 Chi tiết Hóa đơn", () => MoFormCon(new FrmChiTietHoaDon(), "CHI TIẾT HÓA ĐƠN DỊCH VỤ"));
            TaoNutMenu("🏢 Quản lý Nhà cung cấp", () => MoFormCon(new FrmNhaCungCap(), "QUẢN LÝ NHÀ CUNG CẤP"));
            TaoNutMenu("📦 Quản lý Phiếu nhập", () => MoFormCon(new FrmPhieuNhap(), "QUẢN LÝ PHIẾU NHẬP VẬT TƯ"));
            TaoNutMenu("📄 Chi tiết Phiếu nhập", () => MoFormCon(new FrmCTPhieuNhap(), "CHI TIẾT PHIẾU NHẬP"));
            TaoNutMenu("📊 Báo cáo & Thống kê", () => MoFormCon(new FrmThongKe(), "BÁO CÁO & THỐNG KÊ DOANH THU"));

            // 2. Thanh Tiêu Đề Trên (Top Banner)
            panelTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(panelTop);

            lblHeaderTitle = new Label
            {
                Text = "HỆ THỐNG QUẢN LÝ KHÁCH SẠN THEO MÔ HÌNH 3 LỚP (3-LAYER)",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Location = new Point(20, 16),
                AutoSize = true
            };
            panelTop.Controls.Add(lblHeaderTitle);

            Label lblRole = new Label
            {
                Text = "GVHD: ThS. Phùng Thái Thiên Trang | SV: Võ Văn Đăng Khoa",
                Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                ForeColor = Color.DimGray,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(panelTop.Width - 450, 18),
                AutoSize = true
            };
            panelTop.Controls.Add(lblRole);

            // 3. Vùng Thân Chứa Form Con (Body Panel)
            panelBody = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke
            };
            Controls.Add(panelBody);

            // Mặc định hiển thị Dashboard khi mở
            HienThiDashboard();
        }

        private void TaoNutMenu(string tieuDe, Action action, Control? previous = null)
        {
            Button btn = new Button
            {
                Text = "  " + tieuDe,
                Dock = DockStyle.Top,
                Height = 44,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                ForeColor = Color.WhiteSmoke,
                BackColor = Color.FromArgb(24, 34, 56),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(40, 56, 90);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(24, 34, 56);
            btn.Click += (s, e) => action();

            panelSidebar.Controls.Add(btn);
            btn.BringToFront();
        }

        private void HienThiDashboard()
        {
            panelBody.Controls.Clear();
            lblHeaderTitle.Text = "BÀN LÀM VIỆC TỔNG QUAN (DASHBOARD)";

            Panel pnlDash = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(30)
            };

            Label lblWelcome = new Label
            {
                Text = "CHÀO MỪNG ĐẾN VỚI HỆ THỐNG QUẢN LÝ KHÁCH SẠN",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.DarkSlateBlue,
                Location = new Point(30, 25),
                AutoSize = true
            };
            pnlDash.Controls.Add(lblWelcome);

            Label lblSub = new Label
            {
                Text = "Đồ án Học phần Lập trình C# — Kiến trúc phân lớp GUI - BUS - DAL - Models - Database\nThành viên thực hiện: Võ Văn Đăng Khoa, Nguyễn Đức, Hoàng Nhật Anh, Lương Quang Vinh",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(80, 80, 80),
                Location = new Point(30, 65),
                AutoSize = true
            };
            pnlDash.Controls.Add(lblSub);

            // 4 Thẻ chỉ số Dashboard
            TaoTheThongTin(pnlDash, "PHÒNG & LƯU TRÚ", "Quản lý 12 phòng nghỉ, tình trạng đặt phòng, check-in, check-out", Color.DarkCyan, 30, 130, () => MoFormCon(new FrmPhong(), "QUẢN LÝ PHÒNG"));
            TaoTheThongTin(pnlDash, "DỊCH VỤ & HÓA ĐƠN", "Theo dõi tiêu thụ minibar, giặt ủi, xuất hóa đơn thanh toán Main-Sub", Color.Crimson, 370, 130, () => MoFormCon(new FrmHoaDon(), "QUẢN LÝ HÓA ĐƠN"));
            TaoTheThongTin(pnlDash, "NHẬP KHO VẬT TƯ", "Kiểm soát nhà cung cấp, lập phiếu nhập hàng, quản lý số lượng tồn", Color.DarkGoldenrod, 710, 130, () => MoFormCon(new FrmPhieuNhap(), "QUẢN LÝ PHIẾU NHẬP"));
            TaoTheThongTin(pnlDash, "BÁO CÁO THỐNG KÊ", "Phân tích doanh thu 4 Quý theo nhân viên, khách hàng và thu chi", Color.ForestGreen, 30, 270, () => MoFormCon(new FrmThongKe(), "BÁO CÁO & THỐNG KÊ DOANH THU"));

            panelBody.Controls.Add(pnlDash);
        }

        private void TaoTheThongTin(Panel parent, string tieuDe, string moTa, Color mau, int x, int y, Action onClick)
        {
            Panel p = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(320, 120),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };

            Label lblT = new Label
            {
                Text = tieuDe,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = mau,
                Location = new Point(15, 15),
                AutoSize = true
            };

            Label lblD = new Label
            {
                Text = moTa,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = Color.DimGray,
                Location = new Point(15, 45),
                Size = new Size(290, 60)
            };

            p.Controls.Add(lblT);
            p.Controls.Add(lblD);

            p.Click += (s, e) => onClick();
            lblT.Click += (s, e) => onClick();
            lblD.Click += (s, e) => onClick();

            parent.Controls.Add(p);
        }

        private void MoFormCon(Form childForm, string tieuDe)
        {
            panelBody.Controls.Clear();
            lblHeaderTitle.Text = tieuDe;

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            panelBody.Controls.Add(childForm);
            panelBody.Tag = childForm;
            childForm.Show();
        }
    }
}
