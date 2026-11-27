using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKhachSan.BUS;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.GUI
{
    /// <summary>
    /// TẦNG GUI: MÀN HÌNH QUẢN LÝ CHI TIẾT PHIẾU NHẬP
    /// Hỗ trợ nhập NHIỀU dịch vụ cho 1 phiếu nhập trong 1 lần thao tác (mô hình giỏ hàng).
    /// </summary>
    public class GUI_CTPhieuNhap : Form
    {
        private readonly CTPhieuNhapBUS ctpnBUS = new();

        // ── Các control nhập liệu ──────────────────────────────────────────────
        private TextBox      txtMaCTPN     = new();
        private ComboBox     cboPhieuNhap  = new();
        private ComboBox     cboDichVu     = new();
        private TextBox      txtSoLuong    = new();
        private TextBox      txtDonGia     = new();
        private TextBox      txtThanhTien  = new();
        private TextBox      txtTimKiem    = new();
        private Label        lblTongTien   = new();

        // ── Giỏ hàng tạm (nhiều dịch vụ cho 1 phiếu) ─────────────────────────
        private DataTable    dtGioHang     = new();
        private DataGridView dgvGioHang    = new();   // bảng tạm chờ lưu
        private Label        lblGioHang    = new();
        private Button       btnThemVaoGio = new();
        private Button       btnXoaKhoiGio = new();
        private Button       btnLuuTatCa   = new();
        private Label        lblTongGio    = new();

        // ── Bảng danh sách đã lưu (đọc từ DB) ────────────────────────────────
        private DataGridView dgvCTPN       = new();

        public GUI_CTPhieuNhap()
        {
            KhoiTaoGiaoDien();
            KhoiTaoGioHang();
            Load += (s, e) =>
            {
                TaiDanhSachComboBox();
                TaiDuLieu();
            };
        }

        // ══════════════════════════════════════════════════════════════════════
        //  KHỞI TẠO GIAO DIỆN
        // ══════════════════════════════════════════════════════════════════════
        private void KhoiTaoGiaoDien()
        {
            Text = "QUẢN LÝ CHI TIẾT PHIẾU NHẬP - [TẦNG GUI]";
            Size = new Size(1100, 820);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            var lblTitle = new Label
            {
                Text = "QUẢN LÝ CHI TIẾT HÀNG HÓA NHẬP KHO",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Location = new Point(260, 14),
                AutoSize = true
            };
            Controls.Add(lblTitle);

            // ── GroupBox: Thông tin nhập ───────────────────────────────────
            var grpInfo = new GroupBox
            {
                Text = "Thông tin chi tiết mặt hàng",
                Location = new Point(25, 55),
                Size = new Size(1035, 148),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            ThemNhanVaO(grpInfo, "Mã chi tiết:", txtMaCTPN, 20, 28, 200);
            txtMaCTPN.ReadOnly = true;
            txtMaCTPN.BackColor = Color.LightGray;

            ThemNhanVaO(grpInfo, "Phiếu nhập:", cboPhieuNhap, 420, 28, 200);
            cboPhieuNhap.DropDownStyle = ComboBoxStyle.DropDownList;

            ThemNhanVaO(grpInfo, "Dịch vụ/Hàng:", cboDichVu, 20, 68, 300);
            cboDichVu.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDichVu.SelectedIndexChanged += CboDichVu_SelectedIndexChanged;

            ThemNhanVaO(grpInfo, "Số lượng:", txtSoLuong, 420, 68, 120);
            txtSoLuong.TextChanged += (s, e) => TinhThanhTien();

            ThemNhanVaO(grpInfo, "Đơn giá nhập:", txtDonGia, 650, 68, 150);
            txtDonGia.ReadOnly = true;
            txtDonGia.BackColor = Color.LightGray;

            ThemNhanVaO(grpInfo, "Thành tiền:", txtThanhTien, 20, 108, 200);
            txtThanhTien.ReadOnly = true;
            txtThanhTien.BackColor = Color.LightGray;

            lblTongTien.SetBounds(420, 110, 450, 25);
            lblTongTien.Font = new Font("Arial", 11, FontStyle.Bold);
            lblTongTien.ForeColor = Color.DarkRed;
            lblTongTien.Text = "Tổng tiền phiếu: 0 VNĐ";
            grpInfo.Controls.Add(lblTongTien);

            Controls.Add(grpInfo);

            // ── Hàng nút chính ─────────────────────────────────────────────
            int btnY = 212;

            // Nút THÊM VÀO GIỎ (mới)
            btnThemVaoGio.Text = "➕ Thêm vào giỏ";
            btnThemVaoGio.Location = new Point(25, btnY);
            btnThemVaoGio.Size = new Size(140, 35);
            btnThemVaoGio.BackColor = Color.FromArgb(37, 99, 235);
            btnThemVaoGio.ForeColor = Color.White;
            btnThemVaoGio.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            btnThemVaoGio.FlatStyle = FlatStyle.Flat;
            btnThemVaoGio.Click += BtnThemVaoGio_Click;
            Controls.Add(btnThemVaoGio);

            // Nút XÓA KHỎI GIỎ (mới)
            btnXoaKhoiGio.Text = "➖ Xóa khỏi giỏ";
            btnXoaKhoiGio.Location = new Point(175, btnY);
            btnXoaKhoiGio.Size = new Size(130, 35);
            btnXoaKhoiGio.BackColor = Color.FromArgb(239, 68, 68);
            btnXoaKhoiGio.ForeColor = Color.White;
            btnXoaKhoiGio.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            btnXoaKhoiGio.FlatStyle = FlatStyle.Flat;
            btnXoaKhoiGio.Click += BtnXoaKhoiGio_Click;
            Controls.Add(btnXoaKhoiGio);

            // Nút LƯU TẤT CẢ (mới)
            btnLuuTatCa.Text = "💾 LƯU TẤT CẢ VÀO DB";
            btnLuuTatCa.Location = new Point(315, btnY);
            btnLuuTatCa.Size = new Size(185, 35);
            btnLuuTatCa.BackColor = Color.FromArgb(16, 185, 129);
            btnLuuTatCa.ForeColor = Color.White;
            btnLuuTatCa.Font = new Font("Arial", 9.5f, FontStyle.Bold);
            btnLuuTatCa.FlatStyle = FlatStyle.Flat;
            btnLuuTatCa.Click += BtnLuuTatCa_Click;
            Controls.Add(btnLuuTatCa);

            ThemNut("✏️ Cập nhật", 510, btnY, BtnSua_Click);
            ThemNut("🗑️ Xóa", 620, btnY, BtnXoa_Click);
            ThemNut("🔄 Làm mới", 730, btnY, BtnLamMoi_Click);

            var lblTim = new Label { Text = "Tìm kiếm:", Location = new Point(850, btnY + 8), AutoSize = true, Font = new Font("Arial", 10) };
            Controls.Add(lblTim);
            txtTimKiem.SetBounds(930, btnY + 4, 170, 26);
            txtTimKiem.Font = new Font("Arial", 10);
            Controls.Add(txtTimKiem);
            ThemNut("🔍 Tìm", 1010, btnY - 2, BtnTimKiem_Click, 80);

            // ── GroupBox: Giỏ hàng tạm ────────────────────────────────────
            var grpGio = new GroupBox
            {
                Text = "🛒 GIỎ HÀNG TẠM - Danh sách dịch vụ chuẩn bị nhập cho phiếu này",
                Location = new Point(25, 258),
                Size = new Size(1035, 170),
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };

            dgvGioHang.SetBounds(10, 25, 820, 130);
            dgvGioHang.ReadOnly = true;
            dgvGioHang.AllowUserToAddRows = false;
            dgvGioHang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGioHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvGioHang.RowHeadersVisible = false;
            dgvGioHang.BackgroundColor = Color.White;
            grpGio.Controls.Add(dgvGioHang);

            lblTongGio.SetBounds(840, 30, 185, 60);
            lblTongGio.Font = new Font("Arial", 10, FontStyle.Bold);
            lblTongGio.ForeColor = Color.DarkGreen;
            lblTongGio.Text = "Giỏ hàng: 0 mặt hàng\nTổng: 0 VNĐ";
            grpGio.Controls.Add(lblTongGio);

            Controls.Add(grpGio);

            // ── Bảng danh sách đã lưu ─────────────────────────────────────
            var lblDB = new Label
            {
                Text = "📋 DANH SÁCH CHI TIẾT ĐÃ LƯU TRONG DATABASE:",
                Location = new Point(25, 438),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(24, 43, 73)
            };
            Controls.Add(lblDB);

            dgvCTPN.SetBounds(25, 462, 1035, 300);
            dgvCTPN.ReadOnly = true;
            dgvCTPN.AllowUserToAddRows = false;
            dgvCTPN.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCTPN.MultiSelect = false;
            dgvCTPN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCTPN.RowHeadersVisible = false;
            dgvCTPN.BackgroundColor = Color.White;
            dgvCTPN.CellClick += DgvCTPN_CellClick;
            Controls.Add(dgvCTPN);
        }

        // ══════════════════════════════════════════════════════════════════════
        //  KHỞI TẠO GIỎ HÀNG (DataTable tạm)
        // ══════════════════════════════════════════════════════════════════════
        private void KhoiTaoGioHang()
        {
            dtGioHang = new DataTable();
            dtGioHang.Columns.Add("MaDV",      typeof(int));
            dtGioHang.Columns.Add("TenDV",     typeof(string));
            dtGioHang.Columns.Add("SoLuong",   typeof(int));
            dtGioHang.Columns.Add("DonGia",    typeof(decimal));
            dtGioHang.Columns.Add("ThanhTien", typeof(decimal));

            dgvGioHang.DataSource = dtGioHang;

            // Đặt header sau khi bind
            dgvGioHang.DataBindingComplete += (s, e) =>
            {
                if (dgvGioHang.Columns["MaDV"]      != null) dgvGioHang.Columns["MaDV"].HeaderText      = "Mã DV";
                if (dgvGioHang.Columns["TenDV"]      != null) dgvGioHang.Columns["TenDV"].HeaderText      = "Tên dịch vụ";
                if (dgvGioHang.Columns["SoLuong"]    != null) dgvGioHang.Columns["SoLuong"].HeaderText    = "Số lượng";
                if (dgvGioHang.Columns["DonGia"]     != null) { dgvGioHang.Columns["DonGia"].HeaderText = "Đơn giá"; dgvGioHang.Columns["DonGia"].DefaultCellStyle.Format = "#,##0 VNĐ"; }
                if (dgvGioHang.Columns["ThanhTien"]  != null) { dgvGioHang.Columns["ThanhTien"].HeaderText = "Thành tiền"; dgvGioHang.Columns["ThanhTien"].DefaultCellStyle.Format = "#,##0 VNĐ"; }
            };
        }

        // ══════════════════════════════════════════════════════════════════════
        //  HELPERS
        // ══════════════════════════════════════════════════════════════════════
        private void ThemNhanVaO(GroupBox grp, string nhan, Control c, int x, int y, int w)
        {
            var lbl = new Label { Text = nhan, Location = new Point(x, y + 3), AutoSize = true, Font = new Font("Arial", 9.5f, FontStyle.Regular) };
            grp.Controls.Add(lbl);
            c.SetBounds(x + 100, y, w, 26);
            c.Font = new Font("Arial", 9.5f, FontStyle.Regular);
            grp.Controls.Add(c);
        }

        private void ThemNut(string text, int x, int y, EventHandler onClick, int w = 100)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(w, 35),
                BackColor = Color.LightSteelBlue,
                Font = new Font("Arial", 9.5f, FontStyle.Bold)
            };
            btn.Click += onClick;
            Controls.Add(btn);
        }

        private void TaiDanhSachComboBox()
        {
            try
            {
                cboPhieuNhap.DataSource    = ctpnBUS.GetDanhSachPhieuNhap();
                cboPhieuNhap.DisplayMember = "MaPN";
                cboPhieuNhap.ValueMember   = "MaPN";
                cboPhieuNhap.SelectedIndex = -1;

                cboDichVu.DataSource    = ctpnBUS.GetDanhSachDichVu();
                cboDichVu.DisplayMember = "TenDV";
                cboDichVu.ValueMember   = "MaDV";
                cboDichVu.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TaiDuLieu()
        {
            try { dgvCTPN.DataSource = ctpnBUS.GetAll(); }
            catch (Exception ex) { MessageBox.Show("Lỗi tải chi tiết: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void CboDichVu_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboDichVu.SelectedIndex >= 0 && cboDichVu.DataSource is DataTable dt && cboDichVu.SelectedIndex < dt.Rows.Count)
            {
                txtDonGia.Text = dt.Rows[cboDichVu.SelectedIndex]["DonGia"].ToString();
                TinhThanhTien();
            }
        }

        private void TinhThanhTien()
        {
            if (int.TryParse(txtSoLuong.Text.Trim(), out int sl) && decimal.TryParse(txtDonGia.Text.Trim(), out decimal dg))
                txtThanhTien.Text = (sl * dg).ToString("N0");
            else
                txtThanhTien.Text = "";
        }

        private void CapNhatTongGio()
        {
            int soMH = dtGioHang.Rows.Count;
            decimal tong = 0;
            foreach (DataRow r in dtGioHang.Rows)
                tong += Convert.ToDecimal(r["ThanhTien"]);
            lblTongGio.Text = $"Giỏ hàng: {soMH} mặt hàng\nTổng: {tong:N0} VNĐ";
        }

        private void CapNhatTongTien()
        {
            if (cboPhieuNhap.SelectedValue != null && int.TryParse(cboPhieuNhap.SelectedValue.ToString(), out int id))
            {
                decimal total = ctpnBUS.TinhTongTien(id);
                lblTongTien.Text = $"Tổng tiền phiếu #{id}: {total:N0} VNĐ";
            }
        }

        private bool KiemTraHopLeGio(out int maDV, out string tenDV, out int soLuong, out decimal donGia)
        {
            maDV = 0; tenDV = ""; soLuong = 0; donGia = 0;

            if (cboPhieuNhap.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập trước!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (cboDichVu.SelectedValue == null || cboDichVu.SelectedItem is not DataRowView dv)
            {
                MessageBox.Show("Vui lòng chọn dịch vụ/hàng hóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!int.TryParse(txtSoLuong.Text.Trim(), out soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return false;
            }
            if (!decimal.TryParse(txtDonGia.Text.Trim(), out donGia) || donGia <= 0)
            {
                MessageBox.Show("Đơn giá phải lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            maDV = Convert.ToInt32(dv["MaDV"]);
            tenDV = dv["TenDV"].ToString() ?? "";
            return true;
        }

        private bool KiemTraHopLe(out CTPhieuNhap ct)
        {
            ct = new CTPhieuNhap();
            if (cboPhieuNhap.SelectedValue == null) { MessageBox.Show("Vui lòng chọn phiếu nhập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (cboDichVu.SelectedValue == null) { MessageBox.Show("Vui lòng chọn mặt hàng/dịch vụ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            if (!int.TryParse(txtSoLuong.Text.Trim(), out int sl) || sl <= 0) { MessageBox.Show("Số lượng nhập phải lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtSoLuong.Focus(); return false; }
            if (!decimal.TryParse(txtDonGia.Text.Trim(), out decimal dg) || dg <= 0) { MessageBox.Show("Đơn giá nhập phải lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; }
            ct.MaCTPN = int.TryParse(txtMaCTPN.Text, out int id) ? id : 0;
            ct.MaPN = Convert.ToInt32(cboPhieuNhap.SelectedValue);
            ct.MaDV = Convert.ToInt32(cboDichVu.SelectedValue);
            ct.SoLuong = sl;
            ct.DonGiaNhap = dg;
            return true;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  XỬ LÝ NÚT GIỎ HÀNG
        // ══════════════════════════════════════════════════════════════════════

        /// <summary>Thêm 1 dịch vụ vào giỏ hàng tạm (chưa lưu DB).</summary>
        private void BtnThemVaoGio_Click(object? sender, EventArgs e)
        {
            if (!KiemTraHopLeGio(out int maDV, out string tenDV, out int soLuong, out decimal donGia)) return;

            // ── Lớp 1: Kiểm tra trùng trong giỏ tạm ─────────────────────────
            foreach (DataRow r in dtGioHang.Rows)
            {
                if (Convert.ToInt32(r["MaDV"]) == maDV)
                {
                    MessageBox.Show(
                        $"⚠️ Dịch vụ '{tenDV}' đã có trong giỏ hàng!\n" +
                        "Hãy xóa khỏi giỏ rồi thêm lại nếu muốn thay đổi số lượng.",
                        "Trùng dịch vụ trong giỏ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // ── Lớp 2: Kiểm tra trùng với dữ liệu đã lưu trong DB ───────────
            int maPN = Convert.ToInt32(cboPhieuNhap.SelectedValue);
            try
            {
                if (ctpnBUS.KiemTraDichVuDaTonTaiTrongPhieu(maPN, maDV))
                {
                    MessageBox.Show(
                        $"⚠️ Dịch vụ '{tenDV}' đã tồn tại trong phiếu nhập #{maPN} (đã lưu trong DB)!\n" +
                        "Dùng chức năng '✏️ Cập nhật' bên dưới để điều chỉnh số lượng.",
                        "Trùng dịch vụ trong DB", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kiểm tra trùng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal thanhTien = soLuong * donGia;
            dtGioHang.Rows.Add(maDV, tenDV, soLuong, donGia, thanhTien);
            CapNhatTongGio();

            // Reset phần dịch vụ để chọn tiếp
            cboDichVu.SelectedIndex = -1;
            txtSoLuong.Clear();
            txtDonGia.Clear();
            txtThanhTien.Clear();
        }

        /// <summary>Xóa dòng đang chọn khỏi giỏ hàng tạm.</summary>
        private void BtnXoaKhoiGio_Click(object? sender, EventArgs e)
        {
            if (dgvGioHang.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa khỏi giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (DataGridViewRow r in dgvGioHang.SelectedRows)
                if (!r.IsNewRow) dgvGioHang.Rows.Remove(r);
            CapNhatTongGio();
        }

        /// <summary>Lưu toàn bộ giỏ hàng vào database trong 1 lần.</summary>
        private void BtnLuuTatCa_Click(object? sender, EventArgs e)
        {
            if (cboPhieuNhap.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dtGioHang.Rows.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống! Hãy thêm ít nhất 1 dịch vụ trước khi lưu.", "Giỏ hàng trống", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maPN = Convert.ToInt32(cboPhieuNhap.SelectedValue);
            try
            {
                ctpnBUS.ThemNhieuCTPN(maPN, dtGioHang);
                MessageBox.Show(
                    $"✅ Đã lưu thành công {dtGioHang.Rows.Count} dịch vụ vào phiếu nhập #{maPN}!\nTồn kho đã được cập nhật tự động.",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dtGioHang.Clear();
                CapNhatTongGio();
                TaiDuLieu();
                CapNhatTongTien();
                LamMoi();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Cảnh báo nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lưu thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════════════════
        //  XỬ LÝ NÚT SỬA / XÓA ĐƠN LẺ (thao tác trực tiếp DB)
        // ══════════════════════════════════════════════════════════════════════
        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (!KiemTraHopLe(out CTPhieuNhap ct)) return;
            try
            {
                ctpnBUS.Sua(ct);
                MessageBox.Show("Sửa thành công! Tồn kho đã tự động điều chỉnh theo chênh lệch.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDuLieu();
                CapNhatTongTien();
                LamMoi();
            }
            catch (ArgumentException ex) { MessageBox.Show(ex.Message, "Cảnh báo nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            catch (Exception ex) { MessageBox.Show("Sửa thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaCTPN.Text, out int id))
            {
                MessageBox.Show("Vui lòng chọn một chi tiết cần xóa từ bảng dưới!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa chi tiết này? Tồn kho mặt hàng sẽ được trừ lại tự động.", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    ctpnBUS.Xoa(id);
                    MessageBox.Show("Xóa thành công! Tồn kho đã được hoàn lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDuLieu();
                    LamMoi();
                }
                catch (Exception ex) { MessageBox.Show("Xóa thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void BtnTimKiem_Click(object? sender, EventArgs e)
        {
            try { dgvCTPN.DataSource = ctpnBUS.TimKiem(txtTimKiem.Text); }
            catch (Exception ex) { MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void BtnLamMoi_Click(object? sender, EventArgs e) => LamMoi();

        private void LamMoi()
        {
            txtMaCTPN.Clear();
            txtSoLuong.Clear();
            txtDonGia.Clear();
            txtThanhTien.Clear();
            txtTimKiem.Clear();
            cboPhieuNhap.SelectedIndex = -1;
            cboDichVu.SelectedIndex    = -1;
            lblTongTien.Text = "Tổng tiền phiếu: 0 VNĐ";
            TaiDuLieu();
        }

        private void DgvCTPN_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvCTPN.Rows[e.RowIndex];

            txtMaCTPN.Text    = row.Cells["MaCTPN"].Value?.ToString() ?? "";
            txtSoLuong.Text   = row.Cells["SoLuong"].Value?.ToString() ?? "";
            txtDonGia.Text    = row.Cells["DonGiaNhap"].Value?.ToString() ?? "";
            txtThanhTien.Text = row.Cells["ThanhTien"].Value?.ToString() ?? "";

            if (row.Cells["MaPN"].Value != null)
                cboPhieuNhap.SelectedValue = Convert.ToInt32(row.Cells["MaPN"].Value);
            if (row.Cells["MaDV"].Value != null)
                cboDichVu.SelectedValue = Convert.ToInt32(row.Cells["MaDV"].Value);

            CapNhatTongTien();
        }
    }
}
