using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKhachSan.BUS;

namespace QuanLyKhachSan.Forms
{
    public class FrmPhieuNhap : Form
    {
        private readonly PhieuNhapBUS bus = new();

        // Master controls
        private TextBox txtMaPN = new();
        private ComboBox cboNCC = new();
        private ComboBox cboNV = new();
        private DateTimePicker dtpNgayNhap = new();

        // Detail controls (Nhập nhiều dịch vụ / hàng hóa vào 1 phiếu)
        private ComboBox cboDichVu = new();
        private TextBox txtDonGiaNhap = new();
        private NumericUpDown nudSoLuong = new();
        private TextBox txtThanhTienTam = new();
        private Button btnThemMon = new();
        private Button btnXoaMon = new();
        private DataGridView dgvHangTam = new();
        private DataTable dtHangTam = new();
        private Label lblTongTienPhieu = new();

        // Action controls
        private Button btnLuuPhieuNhap = new();
        private Button btnSua = new();
        private Button btnXoa = new();
        private Button btnLamMoi = new();
        private TextBox txtTimKiem = new();
        private Button btnTimKiem = new();

        // Master - Detail Grids
        private DataGridView dgvPhieuNhap = new();          // Master Grid
        private DataGridView dgvChiTietPhieu = new();       // Detail Grid
        private Label lblDetailHeader = new();

        public FrmPhieuNhap()
        {
            InitializeForm();
            InitDataTableHangTam();
            LoadComboBoxes();
            LoadData();
        }

        private void InitializeForm()
        {
            Text = "QUẢN LÝ PHIẾU NHẬP HÀNG - MÔ HÌNH MASTER/DETAIL (1 PHIẾU NHẬP NHIỀU DỊCH VỤ)";
            Size = new Size(1250, 840);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            // Tiêu đề
            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ NHẬP HÀNG (1 PHIẾU NHẬP NHIỀU MẶT HÀNG / DỊCH VỤ)",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(24, 43, 73),
                Location = new Point(25, 12),
                AutoSize = true
            };
            Controls.Add(lblTitle);

            // 1. GroupBox Master (Thông tin phiếu nhập)
            GroupBox grpMaster = new GroupBox
            {
                Text = "1. Thông tin phiếu nhập (Master)",
                Location = new Point(25, 48),
                Size = new Size(520, 240),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59)
            };
            Controls.Add(grpMaster);

            AddLabel(grpMaster, "Mã phiếu nhập:", 15, 32);
            txtMaPN.Location = new Point(135, 28);
            txtMaPN.Size = new Size(365, 26);
            txtMaPN.ReadOnly = true;
            txtMaPN.BackColor = Color.FromArgb(240, 240, 240);
            txtMaPN.Text = "[Tự động sinh khi lưu]";
            grpMaster.Controls.Add(txtMaPN);

            AddLabel(grpMaster, "Nhà cung cấp:", 15, 75);
            cboNCC.Location = new Point(135, 71);
            cboNCC.Size = new Size(365, 26);
            cboNCC.DropDownStyle = ComboBoxStyle.DropDownList;
            grpMaster.Controls.Add(cboNCC);

            AddLabel(grpMaster, "Nhân viên lập:", 15, 118);
            cboNV.Location = new Point(135, 114);
            cboNV.Size = new Size(365, 26);
            cboNV.DropDownStyle = ComboBoxStyle.DropDownList;
            grpMaster.Controls.Add(cboNV);

            AddLabel(grpMaster, "Ngày nhập:", 15, 161);
            dtpNgayNhap.Location = new Point(135, 157);
            dtpNgayNhap.Size = new Size(365, 26);
            dtpNgayNhap.Format = DateTimePickerFormat.Short;
            grpMaster.Controls.Add(dtpNgayNhap);

            // 2. GroupBox Detail (Chọn nhiều dịch vụ / mặt hàng)
            GroupBox grpDetail = new GroupBox
            {
                Text = "2. Chọn danh sách dịch vụ / hàng hóa nhập (Nhập nhiều món 1 lúc)",
                Location = new Point(560, 48),
                Size = new Size(655, 240),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59)
            };
            Controls.Add(grpDetail);

            AddLabel(grpDetail, "Chọn mặt hàng:", 15, 28);
            cboDichVu.Location = new Point(120, 25);
            cboDichVu.Size = new Size(210, 26);
            cboDichVu.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDichVu.SelectedIndexChanged += CboDichVu_SelectedIndexChanged;
            grpDetail.Controls.Add(cboDichVu);

            AddLabel(grpDetail, "Số lượng:", 340, 28);
            nudSoLuong.Location = new Point(410, 25);
            nudSoLuong.Size = new Size(70, 26);
            nudSoLuong.Minimum = 1;
            nudSoLuong.Maximum = 10000;
            nudSoLuong.Value = 1;
            nudSoLuong.ValueChanged += (s, e) => TinhThanhTienTam();
            grpDetail.Controls.Add(nudSoLuong);

            AddLabel(grpDetail, "Đơn giá nhập:", 15, 66);
            txtDonGiaNhap.Location = new Point(120, 62);
            txtDonGiaNhap.Size = new Size(130, 26);
            txtDonGiaNhap.TextChanged += (s, e) => TinhThanhTienTam();
            grpDetail.Controls.Add(txtDonGiaNhap);

            AddLabel(grpDetail, "Thành tiền:", 260, 66);
            txtThanhTienTam.Location = new Point(340, 62);
            txtThanhTienTam.Size = new Size(140, 26);
            txtThanhTienTam.ReadOnly = true;
            txtThanhTienTam.BackColor = Color.FromArgb(240, 240, 240);
            grpDetail.Controls.Add(txtThanhTienTam);

            btnThemMon.Text = "➕ Thêm món";
            btnThemMon.Location = new Point(495, 22);
            btnThemMon.Size = new Size(145, 32);
            btnThemMon.BackColor = Color.FromArgb(37, 99, 235);
            btnThemMon.ForeColor = Color.White;
            btnThemMon.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnThemMon.FlatStyle = FlatStyle.Flat;
            btnThemMon.Click += BtnThemMon_Click;
            grpDetail.Controls.Add(btnThemMon);

            btnXoaMon.Text = "➖ Bỏ món";
            btnXoaMon.Location = new Point(495, 59);
            btnXoaMon.Size = new Size(145, 32);
            btnXoaMon.BackColor = Color.FromArgb(239, 68, 68);
            btnXoaMon.ForeColor = Color.White;
            btnXoaMon.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnXoaMon.FlatStyle = FlatStyle.Flat;
            btnXoaMon.Click += BtnXoaMon_Click;
            grpDetail.Controls.Add(btnXoaMon);

            // Bảng tạm các mặt hàng được chọn
            dgvHangTam.Location = new Point(15, 100);
            dgvHangTam.Size = new Size(625, 98);
            dgvHangTam.ReadOnly = true;
            dgvHangTam.AllowUserToAddRows = false;
            dgvHangTam.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHangTam.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHangTam.RowHeadersVisible = false;
            dgvHangTam.BackgroundColor = Color.White;
            grpDetail.Controls.Add(dgvHangTam);

            lblTongTienPhieu.Text = "Tổng cộng: 0 mặt hàng | Tổng tiền: 0 VNĐ";
            lblTongTienPhieu.Location = new Point(15, 208);
            lblTongTienPhieu.Size = new Size(625, 24);
            lblTongTienPhieu.ForeColor = Color.FromArgb(180, 83, 9);
            lblTongTienPhieu.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpDetail.Controls.Add(lblTongTienPhieu);

            // 3. Panel các nút chức năng
            Panel pnlActions = new Panel
            {
                Location = new Point(25, 298),
                Size = new Size(1190, 48),
                BackColor = Color.Transparent
            };
            Controls.Add(pnlActions);

            btnLuuPhieuNhap.Text = "💾 LẬP PHIẾU NHẬP (LƯU TẤT CẢ)";
            btnLuuPhieuNhap.Location = new Point(0, 4);
            btnLuuPhieuNhap.Size = new Size(270, 38);
            btnLuuPhieuNhap.BackColor = Color.FromArgb(16, 185, 129);
            btnLuuPhieuNhap.ForeColor = Color.White;
            btnLuuPhieuNhap.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLuuPhieuNhap.FlatStyle = FlatStyle.Flat;
            btnLuuPhieuNhap.Click += BtnLuuPhieuNhap_Click;
            pnlActions.Controls.Add(btnLuuPhieuNhap);

            btnSua.Text = "✏️ Cập nhật phiếu";
            btnSua.Location = new Point(285, 4);
            btnSua.Size = new Size(130, 38);
            btnSua.BackColor = Color.FromArgb(245, 158, 11);
            btnSua.ForeColor = Color.White;
            btnSua.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnSua.FlatStyle = FlatStyle.Flat;
            btnSua.Click += (s, e) => SuaPhieuNhap();
            pnlActions.Controls.Add(btnSua);

            btnXoa.Text = "🗑️ Xóa phiếu";
            btnXoa.Location = new Point(425, 4);
            btnXoa.Size = new Size(110, 38);
            btnXoa.BackColor = Color.FromArgb(220, 38, 38);
            btnXoa.ForeColor = Color.White;
            btnXoa.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.Click += (s, e) => XoaPhieuNhap();
            pnlActions.Controls.Add(btnXoa);

            btnLamMoi.Text = "🔄 Làm mới";
            btnLamMoi.Location = new Point(545, 4);
            btnLamMoi.Size = new Size(100, 38);
            btnLamMoi.BackColor = Color.FromArgb(100, 116, 139);
            btnLamMoi.ForeColor = Color.White;
            btnLamMoi.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Click += (s, e) => LamMoiForm();
            pnlActions.Controls.Add(btnLamMoi);

            Label lblTimKiem = new Label
            {
                Text = "Tìm kiếm phiếu:",
                Location = new Point(680, 12),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };
            pnlActions.Controls.Add(lblTimKiem);

            txtTimKiem.Location = new Point(795, 8);
            txtTimKiem.Size = new Size(260, 26);
            txtTimKiem.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) TimKiem(); };
            pnlActions.Controls.Add(txtTimKiem);

            btnTimKiem.Text = "🔍 Tìm";
            btnTimKiem.Location = new Point(1065, 6);
            btnTimKiem.Size = new Size(80, 32);
            btnTimKiem.BackColor = Color.FromArgb(59, 130, 246);
            btnTimKiem.ForeColor = Color.White;
            btnTimKiem.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTimKiem.FlatStyle = FlatStyle.Flat;
            btnTimKiem.Click += (s, e) => TimKiem();
            pnlActions.Controls.Add(btnTimKiem);

            // 4. Bảng Master: Danh sách các phiếu nhập
            Label lblMasterHeader = new Label
            {
                Text = "📋 DANH SÁCH CÁC PHIẾU NHẬP (MASTER):",
                Location = new Point(25, 355),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(24, 43, 73)
            };
            Controls.Add(lblMasterHeader);

            dgvPhieuNhap.Location = new Point(25, 380);
            dgvPhieuNhap.Size = new Size(650, 400);
            dgvPhieuNhap.ReadOnly = true;
            dgvPhieuNhap.AllowUserToAddRows = false;
            dgvPhieuNhap.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPhieuNhap.MultiSelect = false;
            dgvPhieuNhap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPhieuNhap.RowHeadersVisible = false;
            dgvPhieuNhap.BackgroundColor = Color.White;
            dgvPhieuNhap.CellClick += DgvPhieuNhap_CellClick;
            Controls.Add(dgvPhieuNhap);

            // 5. Bảng Detail: Chi tiết các mặt hàng của phiếu nhập đang chọn
            lblDetailHeader.Text = "🔍 CHI TIẾT CÁC MẶT HÀNG CỦA PHIẾU NHẬP ĐANG CHỌN (DETAIL):";
            lblDetailHeader.Location = new Point(695, 355);
            lblDetailHeader.AutoSize = true;
            lblDetailHeader.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDetailHeader.ForeColor = Color.FromArgb(24, 43, 73);
            Controls.Add(lblDetailHeader);

            dgvChiTietPhieu.Location = new Point(695, 380);
            dgvChiTietPhieu.Size = new Size(520, 400);
            dgvChiTietPhieu.ReadOnly = true;
            dgvChiTietPhieu.AllowUserToAddRows = false;
            dgvChiTietPhieu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTietPhieu.MultiSelect = false;
            dgvChiTietPhieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTietPhieu.RowHeadersVisible = false;
            dgvChiTietPhieu.BackgroundColor = Color.White;
            Controls.Add(dgvChiTietPhieu);
        }

        private void AddLabel(Control parent, string text, int x, int y)
        {
            Label lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 65, 85)
            };
            parent.Controls.Add(lbl);
        }

        private void InitDataTableHangTam()
        {
            dtHangTam = new DataTable();
            dtHangTam.Columns.Add("MaDV", typeof(int));
            dtHangTam.Columns.Add("TenDV", typeof(string));
            dtHangTam.Columns.Add("SoLuong", typeof(int));
            dtHangTam.Columns.Add("DonGiaNhap", typeof(decimal));
            dtHangTam.Columns.Add("ThanhTien", typeof(decimal));

            dgvHangTam.DataSource = dtHangTam;
            if (dgvHangTam.Columns["MaDV"] != null)
                dgvHangTam.Columns["MaDV"].Width = 55;
            if (dgvHangTam.Columns["TenDV"] != null)
                dgvHangTam.Columns["TenDV"].HeaderText = "Tên mặt hàng / Dịch vụ";
            if (dgvHangTam.Columns["SoLuong"] != null)
                dgvHangTam.Columns["SoLuong"].HeaderText = "SL";
            if (dgvHangTam.Columns["DonGiaNhap"] != null)
            {
                dgvHangTam.Columns["DonGiaNhap"].HeaderText = "Đơn giá nhập";
                dgvHangTam.Columns["DonGiaNhap"].DefaultCellStyle.Format = "#,##0 VNĐ";
            }
            if (dgvHangTam.Columns["ThanhTien"] != null)
            {
                dgvHangTam.Columns["ThanhTien"].HeaderText = "Thành tiền";
                dgvHangTam.Columns["ThanhTien"].DefaultCellStyle.Format = "#,##0 VNĐ";
            }
        }

        private void LoadComboBoxes()
        {
            try
            {
                // Nhà cung cấp
                cboNCC.DataSource = bus.GetNhaCungCap();
                cboNCC.DisplayMember = "TenNCC";
                cboNCC.ValueMember = "MaNCC";
                cboNCC.SelectedIndex = -1;

                // Nhân viên
                cboNV.DataSource = bus.GetNhanVien();
                cboNV.DisplayMember = "HoTen";
                cboNV.ValueMember = "MaNV";
                cboNV.SelectedIndex = -1;

                // Dịch vụ / Hàng hóa
                DataTable dtDV = bus.GetDanhSachDichVu();
                cboDichVu.DataSource = dtDV;
                cboDichVu.DisplayMember = "TenDV";
                cboDichVu.ValueMember = "MaDV";
                if (dtDV.Rows.Count > 0)
                    cboDichVu.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CboDichVu_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboDichVu.SelectedItem is DataRowView rowView)
            {
                decimal donGia = Convert.ToDecimal(rowView["DonGia"]);
                txtDonGiaNhap.Text = donGia.ToString("0");
                TinhThanhTienTam();
            }
        }

        private void TinhThanhTienTam()
        {
            if (decimal.TryParse(txtDonGiaNhap.Text.Trim(), out decimal dg))
            {
                decimal tt = dg * nudSoLuong.Value;
                txtThanhTienTam.Text = tt.ToString("N0") + " VNĐ";
            }
            else
            {
                txtThanhTienTam.Text = "0 VNĐ";
            }
        }

        private void BtnThemMon_Click(object? sender, EventArgs e)
        {
            if (cboDichVu.SelectedItem is not DataRowView rowView)
            {
                MessageBox.Show("Vui lòng chọn một mặt hàng / dịch vụ cần nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maDV = Convert.ToInt32(rowView["MaDV"]);
            string tenDV = rowView["TenDV"].ToString() ?? "";
            int soLuong = (int)nudSoLuong.Value;

            if (!decimal.TryParse(txtDonGiaNhap.Text.Trim(), out decimal donGiaNhap) || donGiaNhap < 0)
            {
                MessageBox.Show("Đơn giá nhập không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra xem mặt hàng đã có trong bảng tạm chưa, nếu có thì cộng dồn số lượng
            bool daCo = false;
            foreach (DataRow r in dtHangTam.Rows)
            {
                if (Convert.ToInt32(r["MaDV"]) == maDV)
                {
                    int slCu = Convert.ToInt32(r["SoLuong"]);
                    int slMoi = slCu + soLuong;
                    r["SoLuong"] = slMoi;
                    r["DonGiaNhap"] = donGiaNhap;
                    r["ThanhTien"] = slMoi * donGiaNhap;
                    daCo = true;
                    break;
                }
            }

            if (!daCo)
            {
                decimal thanhTien = soLuong * donGiaNhap;
                dtHangTam.Rows.Add(maDV, tenDV, soLuong, donGiaNhap, thanhTien);
            }

            CapNhatTongTienPhieuTam();
        }

        private void BtnXoaMon_Click(object? sender, EventArgs e)
        {
            if (dgvHangTam.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn mặt hàng cần bỏ khỏi danh sách tạm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (DataGridViewRow r in dgvHangTam.SelectedRows)
            {
                if (!r.IsNewRow)
                    dgvHangTam.Rows.Remove(r);
            }
            CapNhatTongTienPhieuTam();
        }

        private void CapNhatTongTienPhieuTam()
        {
            decimal tongTien = 0;
            int tongSL = 0;
            foreach (DataRow r in dtHangTam.Rows)
            {
                tongSL += Convert.ToInt32(r["SoLuong"]);
                tongTien += Convert.ToDecimal(r["ThanhTien"]);
            }
            lblTongTienPhieu.Text = $"Tổng cộng: {dtHangTam.Rows.Count} mặt hàng (Tổng SL: {tongSL}) | Tổng tiền: {tongTien:N0} VNĐ";
        }

        // Lập phiếu nhập: Lưu Master + N Detail cùng lúc trong 1 Transaction
        private void BtnLuuPhieuNhap_Click(object? sender, EventArgs e)
        {
            try
            {
                if (cboNCC.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn nhà cung cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboNCC.Focus();
                    return;
                }

                if (cboNV.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên lập phiếu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboNV.Focus();
                    return;
                }

                if (dtHangTam.Rows.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm ít nhất 1 mặt hàng vào phiếu nhập trước khi lưu!", "Chưa có mặt hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maNCC = Convert.ToInt32(cboNCC.SelectedValue);
                int maNV = Convert.ToInt32(cboNV.SelectedValue);
                DateTime ngayNhap = dtpNgayNhap.Value;

                // Gọi BUS để lưu Master và toàn bộ Details trong 1 Transaction
                int maPNMoi = bus.ThemPhieuNhapKemChiTiet(maNCC, maNV, ngayNhap, dtHangTam);

                decimal tongTien = 0;
                foreach (DataRow r in dtHangTam.Rows)
                {
                    tongTien += Convert.ToDecimal(r["ThanhTien"]);
                }

                MessageBox.Show(
                    $"🎉 Đã lập thành công phiếu nhập #{maPNMoi} từ nhà cung cấp '{cboNCC.Text}'!\nSố mặt hàng: {dtHangTam.Rows.Count}\nTổng giá trị: {tongTien:N0} VNĐ\nTồn kho các mặt hàng đã được tự động cập nhật.",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LamMoiForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lập phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SuaPhieuNhap()
        {
            try
            {
                if (!int.TryParse(txtMaPN.Text.Trim(), out int maPN))
                {
                    MessageBox.Show("Vui lòng chọn phiếu nhập cần sửa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maNCC = Convert.ToInt32(cboNCC.SelectedValue);
                int maNV = Convert.ToInt32(cboNV.SelectedValue);
                DateTime ngayNhap = dtpNgayNhap.Value;

                bus.Sua(maPN, maNCC, maNV, ngayNhap);

                MessageBox.Show($"Cập nhật thông tin phiếu nhập #{maPN} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void XoaPhieuNhap()
        {
            try
            {
                if (!int.TryParse(txtMaPN.Text.Trim(), out int maPN))
                {
                    MessageBox.Show("Vui lòng chọn phiếu nhập cần xóa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var hoi = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa phiếu nhập #{maPN} cùng tất cả chi tiết mặt hàng bên trong?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (hoi == DialogResult.Yes)
                {
                    bus.Xoa(maPN);
                    MessageBox.Show($"Đã xóa phiếu nhập #{maPN} thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LamMoiForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            try
            {
                DataTable dt = bus.GetAll();
                dgvPhieuNhap.DataSource = dt;

                if (dgvPhieuNhap.Columns["MaPN"] != null)
                    dgvPhieuNhap.Columns["MaPN"].HeaderText = "Mã PN";
                if (dgvPhieuNhap.Columns["MaNCC"] != null)
                    dgvPhieuNhap.Columns["MaNCC"].Visible = false;
                if (dgvPhieuNhap.Columns["TenNCC"] != null)
                    dgvPhieuNhap.Columns["TenNCC"].HeaderText = "Nhà cung cấp";
                if (dgvPhieuNhap.Columns["MaNV"] != null)
                    dgvPhieuNhap.Columns["MaNV"].Visible = false;
                if (dgvPhieuNhap.Columns["TenNhanVien"] != null)
                    dgvPhieuNhap.Columns["TenNhanVien"].HeaderText = "Nhân viên lập";
                if (dgvPhieuNhap.Columns["NgayNhap"] != null)
                {
                    dgvPhieuNhap.Columns["NgayNhap"].HeaderText = "Ngày nhập";
                    dgvPhieuNhap.Columns["NgayNhap"].DefaultCellStyle.Format = "dd/MM/yyyy";
                }
                if (dgvPhieuNhap.Columns["TongTien"] != null)
                {
                    dgvPhieuNhap.Columns["TongTien"].HeaderText = "Tổng tiền";
                    dgvPhieuNhap.Columns["TongTien"].DefaultCellStyle.Format = "#,##0 VNĐ";
                }

                if (dt.Rows.Count > 0)
                {
                    dgvPhieuNhap.ClearSelection();
                    dgvPhieuNhap.Rows[0].Selected = true;
                    HienThiChiTietTheoRow(dgvPhieuNhap.Rows[0]);
                }
                else
                {
                    dgvChiTietPhieu.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvPhieuNhap_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvPhieuNhap.Rows.Count)
            {
                HienThiChiTietTheoRow(dgvPhieuNhap.Rows[e.RowIndex]);
            }
        }

        private void HienThiChiTietTheoRow(DataGridViewRow row)
        {
            try
            {
                txtMaPN.Text = row.Cells["MaPN"].Value?.ToString() ?? "";
                if (row.Cells["MaNCC"].Value != null && row.Cells["MaNCC"].Value != DBNull.Value)
                    cboNCC.SelectedValue = Convert.ToInt32(row.Cells["MaNCC"].Value);

                if (row.Cells["MaNV"].Value != null && row.Cells["MaNV"].Value != DBNull.Value)
                    cboNV.SelectedValue = Convert.ToInt32(row.Cells["MaNV"].Value);

                if (row.Cells["NgayNhap"].Value != null && row.Cells["NgayNhap"].Value != DBNull.Value)
                    dtpNgayNhap.Value = Convert.ToDateTime(row.Cells["NgayNhap"].Value);

                if (int.TryParse(txtMaPN.Text, out int maPN))
                {
                    string tenNCC = row.Cells["TenNCC"].Value?.ToString() ?? "";
                    lblDetailHeader.Text = $"🔍 CHI TIẾT MẶT HÀNG TRONG PHIẾU #{maPN} ({tenNCC}):";

                    DataTable dtCT = bus.GetChiTietTheoMaPN(maPN);
                    dgvChiTietPhieu.DataSource = dtCT;

                    if (dgvChiTietPhieu.Columns["MaCTPN"] != null)
                        dgvChiTietPhieu.Columns["MaCTPN"].HeaderText = "Mã CT";
                    if (dgvChiTietPhieu.Columns["MaPN"] != null)
                        dgvChiTietPhieu.Columns["MaPN"].Visible = false;
                    if (dgvChiTietPhieu.Columns["MaDV"] != null)
                        dgvChiTietPhieu.Columns["MaDV"].HeaderText = "Mã DV";
                    if (dgvChiTietPhieu.Columns["TenDV"] != null)
                        dgvChiTietPhieu.Columns["TenDV"].HeaderText = "Tên mặt hàng / Dịch vụ";
                    if (dgvChiTietPhieu.Columns["SoLuong"] != null)
                        dgvChiTietPhieu.Columns["SoLuong"].HeaderText = "SL";
                    if (dgvChiTietPhieu.Columns["DonGiaNhap"] != null)
                    {
                        dgvChiTietPhieu.Columns["DonGiaNhap"].HeaderText = "Đơn giá nhập";
                        dgvChiTietPhieu.Columns["DonGiaNhap"].DefaultCellStyle.Format = "#,##0 VNĐ";
                    }
                    if (dgvChiTietPhieu.Columns["ThanhTien"] != null)
                    {
                        dgvChiTietPhieu.Columns["ThanhTien"].HeaderText = "Thành tiền";
                        dgvChiTietPhieu.Columns["ThanhTien"].DefaultCellStyle.Format = "#,##0 VNĐ";
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi hiển thị chi tiết: " + ex.Message);
            }
        }

        private void TimKiem()
        {
            try
            {
                string kw = txtTimKiem.Text.Trim();
                DataTable dt = bus.TimKiem(kw);
                dgvPhieuNhap.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LamMoiForm()
        {
            dtHangTam.Clear();
            CapNhatTongTienPhieuTam();
            LoadData();

            txtMaPN.Text = "[Tự động sinh khi lưu]";
            cboNCC.SelectedIndex = -1;
            cboNV.SelectedIndex = -1;
            dtpNgayNhap.Value = DateTime.Now;
            nudSoLuong.Value = 1;
            txtTimKiem.Clear();
        }
    }
}
