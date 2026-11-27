using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKhachSan.BUS;

namespace QuanLyKhachSan.Forms
{
    public class FrmDatPhong : Form
    {
        private readonly DatPhongBUS bus = new();

        // Master controls
        private TextBox txtMaDP = new();
        private ComboBox cboKhachHang = new();
        private ComboBox cboNhanVien = new();
        private DateTimePicker dtpNgayDat = new();
        private DateTimePicker dtpNgayNhan = new();
        private DateTimePicker dtpNgayTra = new();
        private ComboBox cboTrangThai = new();

        // Detail controls (Chọn nhiều phòng)
        private ComboBox cboPhongTrong = new();
        private TextBox txtDonGiaPhong = new();
        private Button btnThemPhong = new();
        private Button btnXoaPhong = new();
        private DataGridView dgvPhongTam = new();
        private DataTable dtPhongTam = new();
        private Label lblTongTienPhong = new();

        // Buttons
        private Button btnLuuDatPhong = new();
        private Button btnSua = new();
        private Button btnXoa = new();
        private Button btnLamMoi = new();
        private TextBox txtTimKiem = new();
        private Button btnTimKiem = new();

        // Master - Detail Grids
        private DataGridView dgvDatPhong = new();       // Master (Danh sách phiếu)
        private DataGridView dgvChiTietPhong = new();   // Detail (Chi tiết phòng của phiếu đang chọn)
        private Label lblDetailHeader = new();

        public FrmDatPhong()
        {
            InitializeForm();
            InitDataTablePhongTam();
            LoadComboBoxes();
            LoadData();
        }

        private void InitializeForm()
        {
            Text = "QUẢN LÝ ĐẶT PHÒNG - MÔ HÌNH MASTER/DETAIL (1 KHÁCH ĐẶT NHIỀU PHÒNG)";
            Size = new Size(1250, 840);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            // Tiêu đề trang
            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ ĐẶT PHÒNG (1 KHÁCH CÓ THỂ ĐẶT NHIỀU PHÒNG)",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(24, 43, 73),
                Location = new Point(25, 12),
                AutoSize = true
            };
            Controls.Add(lblTitle);

            // 1. GroupBox Thông tin phiếu đặt phòng (Master)
            GroupBox grpMaster = new GroupBox
            {
                Text = "1. Thông tin phiếu đặt phòng",
                Location = new Point(25, 48),
                Size = new Size(540, 240),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59)
            };
            Controls.Add(grpMaster);

            AddLabel(grpMaster, "Mã đặt phòng:", 15, 30);
            txtMaDP.Location = new Point(130, 27);
            txtMaDP.Size = new Size(120, 26);
            txtMaDP.ReadOnly = true;
            txtMaDP.BackColor = Color.FromArgb(240, 240, 240);
            grpMaster.Controls.Add(txtMaDP);

            AddLabel(grpMaster, "Trạng thái:", 270, 30);
            cboTrangThai.Location = new Point(355, 27);
            cboTrangThai.Size = new Size(165, 26);
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTrangThai.Items.AddRange(new object[] { "Đã xác nhận", "Chờ xác nhận", "Đang sử dụng", "Đã hoàn thành", "Đã hủy" });
            cboTrangThai.SelectedIndex = 0;
            grpMaster.Controls.Add(cboTrangThai);

            AddLabel(grpMaster, "Khách hàng:", 15, 68);
            cboKhachHang.Location = new Point(130, 65);
            cboKhachHang.Size = new Size(390, 26);
            cboKhachHang.DropDownStyle = ComboBoxStyle.DropDownList;
            grpMaster.Controls.Add(cboKhachHang);

            AddLabel(grpMaster, "Nhân viên:", 15, 106);
            cboNhanVien.Location = new Point(130, 103);
            cboNhanVien.Size = new Size(390, 26);
            cboNhanVien.DropDownStyle = ComboBoxStyle.DropDownList;
            grpMaster.Controls.Add(cboNhanVien);

            AddLabel(grpMaster, "Ngày đặt:", 15, 144);
            dtpNgayDat.Location = new Point(130, 141);
            dtpNgayDat.Size = new Size(130, 26);
            dtpNgayDat.Format = DateTimePickerFormat.Short;
            grpMaster.Controls.Add(dtpNgayDat);

            AddLabel(grpMaster, "Ngày nhận:", 270, 144);
            dtpNgayNhan.Location = new Point(355, 141);
            dtpNgayNhan.Size = new Size(165, 26);
            dtpNgayNhan.Format = DateTimePickerFormat.Short;
            grpMaster.Controls.Add(dtpNgayNhan);

            AddLabel(grpMaster, "Ngày trả:", 15, 182);
            dtpNgayTra.Location = new Point(130, 179);
            dtpNgayTra.Size = new Size(130, 26);
            dtpNgayTra.Format = DateTimePickerFormat.Short;
            grpMaster.Controls.Add(dtpNgayTra);

            // 2. GroupBox Chọn nhiều phòng (Detail)
            GroupBox grpDetail = new GroupBox
            {
                Text = "2. Chọn danh sách phòng đặt (1 người đặt nhiều phòng)",
                Location = new Point(580, 48),
                Size = new Size(635, 240),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59)
            };
            Controls.Add(grpDetail);

            AddLabel(grpDetail, "Chọn phòng trống:", 15, 28);
            cboPhongTrong.Location = new Point(140, 25);
            cboPhongTrong.Size = new Size(240, 26);
            cboPhongTrong.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPhongTrong.SelectedIndexChanged += CboPhongTrong_SelectedIndexChanged;
            grpDetail.Controls.Add(cboPhongTrong);

            AddLabel(grpDetail, "Đơn giá (VNĐ):", 390, 28);
            txtDonGiaPhong.Location = new Point(490, 25);
            txtDonGiaPhong.Size = new Size(130, 26);
            grpDetail.Controls.Add(txtDonGiaPhong);

            btnThemPhong.Text = "➕ Thêm vào phiếu";
            btnThemPhong.Location = new Point(140, 60);
            btnThemPhong.Size = new Size(150, 32);
            btnThemPhong.BackColor = Color.FromArgb(37, 99, 235);
            btnThemPhong.ForeColor = Color.White;
            btnThemPhong.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnThemPhong.FlatStyle = FlatStyle.Flat;
            btnThemPhong.Click += BtnThemPhong_Click;
            grpDetail.Controls.Add(btnThemPhong);

            btnXoaPhong.Text = "➖ Bỏ chọn";
            btnXoaPhong.Location = new Point(300, 60);
            btnXoaPhong.Size = new Size(110, 32);
            btnXoaPhong.BackColor = Color.FromArgb(239, 68, 68);
            btnXoaPhong.ForeColor = Color.White;
            btnXoaPhong.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnXoaPhong.FlatStyle = FlatStyle.Flat;
            btnXoaPhong.Click += BtnXoaPhong_Click;
            grpDetail.Controls.Add(btnXoaPhong);

            lblTongTienPhong.Text = "Tổng cộng: 0 phòng | Tổng tiền: 0 VNĐ";
            lblTongTienPhong.Location = new Point(420, 65);
            lblTongTienPhong.Size = new Size(205, 26);
            lblTongTienPhong.ForeColor = Color.FromArgb(180, 83, 9);
            lblTongTienPhong.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            grpDetail.Controls.Add(lblTongTienPhong);

            // Bảng tạm các phòng được chọn
            dgvPhongTam.Location = new Point(15, 100);
            dgvPhongTam.Size = new Size(605, 125);
            dgvPhongTam.ReadOnly = true;
            dgvPhongTam.AllowUserToAddRows = false;
            dgvPhongTam.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPhongTam.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPhongTam.RowHeadersVisible = false;
            dgvPhongTam.BackgroundColor = Color.White;
            grpDetail.Controls.Add(dgvPhongTam);

            // 3. Panel các nút chức năng (Thao tác)
            Panel pnlActions = new Panel
            {
                Location = new Point(25, 298),
                Size = new Size(1190, 48),
                BackColor = Color.Transparent
            };
            Controls.Add(pnlActions);

            btnLuuDatPhong.Text = "💾 LẬP PHIẾU ĐẶT PHÒNG (LƯU TẤT CẢ)";
            btnLuuDatPhong.Location = new Point(0, 4);
            btnLuuDatPhong.Size = new Size(270, 38);
            btnLuuDatPhong.BackColor = Color.FromArgb(16, 185, 129);
            btnLuuDatPhong.ForeColor = Color.White;
            btnLuuDatPhong.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLuuDatPhong.FlatStyle = FlatStyle.Flat;
            btnLuuDatPhong.Click += BtnLuuDatPhong_Click;
            pnlActions.Controls.Add(btnLuuDatPhong);

            btnSua.Text = "✏️ Cập nhật phiếu";
            btnSua.Location = new Point(285, 4);
            btnSua.Size = new Size(130, 38);
            btnSua.BackColor = Color.FromArgb(245, 158, 11);
            btnSua.ForeColor = Color.White;
            btnSua.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnSua.FlatStyle = FlatStyle.Flat;
            btnSua.Click += (s, e) => SuaDatPhong();
            pnlActions.Controls.Add(btnSua);

            btnXoa.Text = "🗑️ Xóa phiếu";
            btnXoa.Location = new Point(425, 4);
            btnXoa.Size = new Size(110, 38);
            btnXoa.BackColor = Color.FromArgb(220, 38, 38);
            btnXoa.ForeColor = Color.White;
            btnXoa.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.Click += (s, e) => XoaDatPhong();
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

            // 4. Bảng Master: Danh sách các phiếu đặt phòng
            Label lblMasterHeader = new Label
            {
                Text = "📋 DANH SÁCH CÁC PHIẾU ĐẶT PHÒNG (MASTER):",
                Location = new Point(25, 355),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(24, 43, 73)
            };
            Controls.Add(lblMasterHeader);

            dgvDatPhong.Location = new Point(25, 380);
            dgvDatPhong.Size = new Size(680, 400);
            dgvDatPhong.ReadOnly = true;
            dgvDatPhong.AllowUserToAddRows = false;
            dgvDatPhong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDatPhong.MultiSelect = false;
            dgvDatPhong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDatPhong.RowHeadersVisible = false;
            dgvDatPhong.BackgroundColor = Color.White;
            dgvDatPhong.CellClick += DgvDatPhong_CellClick;
            Controls.Add(dgvDatPhong);

            // 5. Bảng Detail: Chi tiết các phòng của phiếu đang chọn
            lblDetailHeader.Text = "🔍 CHI TIẾT CÁC PHÒNG THUỘC PHIẾU ĐANG CHỌN (DETAIL):";
            lblDetailHeader.Location = new Point(720, 355);
            lblDetailHeader.AutoSize = true;
            lblDetailHeader.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDetailHeader.ForeColor = Color.FromArgb(24, 43, 73);
            Controls.Add(lblDetailHeader);

            dgvChiTietPhong.Location = new Point(720, 380);
            dgvChiTietPhong.Size = new Size(495, 400);
            dgvChiTietPhong.ReadOnly = true;
            dgvChiTietPhong.AllowUserToAddRows = false;
            dgvChiTietPhong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTietPhong.MultiSelect = false;
            dgvChiTietPhong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTietPhong.RowHeadersVisible = false;
            dgvChiTietPhong.BackgroundColor = Color.White;
            Controls.Add(dgvChiTietPhong);
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

        private void InitDataTablePhongTam()
        {
            dtPhongTam = new DataTable();
            dtPhongTam.Columns.Add("MaPhong", typeof(int));
            dtPhongTam.Columns.Add("TenPhong", typeof(string));
            dtPhongTam.Columns.Add("TenLoai", typeof(string));
            dtPhongTam.Columns.Add("DonGia", typeof(decimal));

            dgvPhongTam.DataSource = dtPhongTam;
            if (dgvPhongTam.Columns["MaPhong"] != null)
                dgvPhongTam.Columns["MaPhong"].Width = 60;
            if (dgvPhongTam.Columns["TenPhong"] != null)
                dgvPhongTam.Columns["TenPhong"].HeaderText = "Tên phòng";
            if (dgvPhongTam.Columns["TenLoai"] != null)
                dgvPhongTam.Columns["TenLoai"].HeaderText = "Loại phòng";
            if (dgvPhongTam.Columns["DonGia"] != null)
            {
                dgvPhongTam.Columns["DonGia"].HeaderText = "Đơn giá";
                dgvPhongTam.Columns["DonGia"].DefaultCellStyle.Format = "#,##0 VNĐ";
            }
        }

        private void LoadComboBoxes()
        {
            try
            {
                // Khách hàng
                cboKhachHang.DataSource = bus.GetKhachHang();
                cboKhachHang.DisplayMember = "HoTen";
                cboKhachHang.ValueMember = "MaKH";
                cboKhachHang.SelectedIndex = -1;

                // Nhân viên
                cboNhanVien.DataSource = bus.GetNhanVien();
                cboNhanVien.DisplayMember = "HoTen";
                cboNhanVien.ValueMember = "MaNV";
                cboNhanVien.SelectedIndex = -1;

                // Phòng trống
                LoadDanhSachPhongTrong();

                // Sinh mã đặt phòng tiếp theo
                txtMaDP.Text = bus.GetNextMaDP().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhSachPhongTrong()
        {
            DataTable dtPhongTrong = bus.GetDanhSachPhongTrong();
            cboPhongTrong.DataSource = dtPhongTrong;
            cboPhongTrong.DisplayMember = "TenPhong";
            cboPhongTrong.ValueMember = "MaPhong";
            if (dtPhongTrong.Rows.Count > 0)
                cboPhongTrong.SelectedIndex = 0;
            else
                cboPhongTrong.SelectedIndex = -1;
        }

        private void CboPhongTrong_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboPhongTrong.SelectedItem is DataRowView rowView)
            {
                decimal gia = Convert.ToDecimal(rowView["GiaPhong"]);
                txtDonGiaPhong.Text = gia.ToString("0");
            }
        }

        private void BtnThemPhong_Click(object? sender, EventArgs e)
        {
            if (cboPhongTrong.SelectedItem is not DataRowView rowView)
            {
                MessageBox.Show("Vui lòng chọn một phòng còn trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maPhong = Convert.ToInt32(rowView["MaPhong"]);
            string tenPhong = rowView["TenPhong"].ToString() ?? "";
            string tenLoai = rowView["TenLoai"].ToString() ?? "";

            if (!decimal.TryParse(txtDonGiaPhong.Text.Trim(), out decimal donGia) || donGia < 0)
            {
                MessageBox.Show("Đơn giá phòng không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra trùng trong bảng tạm
            foreach (DataRow r in dtPhongTam.Rows)
            {
                if (Convert.ToInt32(r["MaPhong"]) == maPhong)
                {
                    MessageBox.Show($"Phòng '{tenPhong}' đã có trong danh sách đặt của phiếu này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Thêm vào bảng tạm
            dtPhongTam.Rows.Add(maPhong, tenPhong, tenLoai, donGia);
            CapNhatTongTienTam();
        }

        private void BtnXoaPhong_Click(object? sender, EventArgs e)
        {
            if (dgvPhongTam.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phòng cần bỏ khỏi danh sách tạm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (DataGridViewRow r in dgvPhongTam.SelectedRows)
            {
                if (!r.IsNewRow)
                    dgvPhongTam.Rows.Remove(r);
            }
            CapNhatTongTienTam();
        }

        private void CapNhatTongTienTam()
        {
            decimal tongTien = 0;
            foreach (DataRow r in dtPhongTam.Rows)
            {
                tongTien += Convert.ToDecimal(r["DonGia"]);
            }
            lblTongTienPhong.Text = $"Tổng cộng: {dtPhongTam.Rows.Count} phòng | {tongTien:N0} VNĐ";
        }

        // Lập phiếu đặt phòng: Lưu Master + N Detail cùng lúc
        private void BtnLuuDatPhong_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtMaDP.Text.Trim(), out int maDP) || maDP <= 0)
                {
                    MessageBox.Show("Mã đặt phòng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cboKhachHang.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboKhachHang.Focus();
                    return;
                }

                if (cboNhanVien.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên tiếp nhận!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboNhanVien.Focus();
                    return;
                }

                if (dtpNgayTra.Value.Date <= dtpNgayNhan.Value.Date)
                {
                    MessageBox.Show("Ngày trả phòng phải sau ngày nhận phòng!", "Lỗi ngày tháng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dtPhongTam.Rows.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm ít nhất 1 phòng vào phiếu đặt trước khi lưu!", "Chưa chọn phòng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maKH = Convert.ToInt32(cboKhachHang.SelectedValue);
                int maNV = Convert.ToInt32(cboNhanVien.SelectedValue);
                DateTime ngayDat = dtpNgayDat.Value;
                DateTime ngayNhan = dtpNgayNhan.Value;
                DateTime ngayTra = dtpNgayTra.Value;
                string trangThai = cboTrangThai.Text;

                // Gọi BUS lưu Master và toàn bộ Details trong 1 Transaction
                bus.ThemDatPhongKemChiTiet(maDP, maKH, maNV, ngayDat, ngayNhan, ngayTra, trangThai, dtPhongTam);

                MessageBox.Show(
                    $"🎉 Đã lập thành công phiếu đặt phòng #{maDP} cho khách hàng '{cboKhachHang.Text}' với {dtPhongTam.Rows.Count} phòng!\nTrạng thái các phòng đã được cập nhật thành 'Đang sử dụng'.",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LamMoiForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lập phiếu đặt phòng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SuaDatPhong()
        {
            try
            {
                if (!int.TryParse(txtMaDP.Text.Trim(), out int maDP))
                {
                    MessageBox.Show("Vui lòng chọn phiếu đặt phòng cần sửa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maKH = Convert.ToInt32(cboKhachHang.SelectedValue);
                int maNV = Convert.ToInt32(cboNhanVien.SelectedValue);
                DateTime ngayDat = dtpNgayDat.Value;
                DateTime ngayNhan = dtpNgayNhan.Value;
                DateTime ngayTra = dtpNgayTra.Value;
                string trangThai = cboTrangThai.Text;

                bus.SuaDatPhong(maDP, maKH, maNV, ngayDat, ngayNhan, ngayTra, trangThai);

                MessageBox.Show($"Cập nhật thông tin phiếu đặt phòng #{maDP} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void XoaDatPhong()
        {
            try
            {
                if (!int.TryParse(txtMaDP.Text.Trim(), out int maDP))
                {
                    MessageBox.Show("Vui lòng chọn phiếu đặt phòng cần xóa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var hoi = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa phiếu đặt phòng #{maDP}?\nCác phòng trong phiếu này sẽ được tự động giải phóng về trạng thái 'Trống'!",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (hoi == DialogResult.Yes)
                {
                    bus.XoaDatPhong(maDP);
                    MessageBox.Show($"Đã xóa phiếu đặt phòng #{maDP} và trả lại trạng thái phòng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                dgvDatPhong.DataSource = dt;

                if (dgvDatPhong.Columns["MaDP"] != null)
                    dgvDatPhong.Columns["MaDP"].HeaderText = "Mã ĐP";
                if (dgvDatPhong.Columns["MaKH"] != null)
                    dgvDatPhong.Columns["MaKH"].Visible = false;
                if (dgvDatPhong.Columns["TenKhachHang"] != null)
                    dgvDatPhong.Columns["TenKhachHang"].HeaderText = "Khách hàng";
                if (dgvDatPhong.Columns["MaNV"] != null)
                    dgvDatPhong.Columns["MaNV"].Visible = false;
                if (dgvDatPhong.Columns["TenNhanVien"] != null)
                    dgvDatPhong.Columns["TenNhanVien"].HeaderText = "Nhân viên";
                if (dgvDatPhong.Columns["NgayDat"] != null)
                {
                    dgvDatPhong.Columns["NgayDat"].HeaderText = "Ngày đặt";
                    dgvDatPhong.Columns["NgayDat"].DefaultCellStyle.Format = "dd/MM/yyyy";
                }
                if (dgvDatPhong.Columns["NgayNhan"] != null)
                {
                    dgvDatPhong.Columns["NgayNhan"].HeaderText = "Ngày nhận";
                    dgvDatPhong.Columns["NgayNhan"].DefaultCellStyle.Format = "dd/MM/yyyy";
                }
                if (dgvDatPhong.Columns["NgayTra"] != null)
                {
                    dgvDatPhong.Columns["NgayTra"].HeaderText = "Ngày trả";
                    dgvDatPhong.Columns["NgayTra"].DefaultCellStyle.Format = "dd/MM/yyyy";
                }
                if (dgvDatPhong.Columns["TrangThai"] != null)
                    dgvDatPhong.Columns["TrangThai"].HeaderText = "Trạng thái";

                if (dt.Rows.Count > 0)
                {
                    dgvDatPhong.ClearSelection();
                    dgvDatPhong.Rows[0].Selected = true;
                    HienThiChiTietTheoRow(dgvDatPhong.Rows[0]);
                }
                else
                {
                    dgvChiTietPhong.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nạp danh sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvDatPhong_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvDatPhong.Rows.Count)
            {
                HienThiChiTietTheoRow(dgvDatPhong.Rows[e.RowIndex]);
            }
        }

        private void HienThiChiTietTheoRow(DataGridViewRow row)
        {
            try
            {
                txtMaDP.Text = row.Cells["MaDP"].Value?.ToString() ?? "";
                if (row.Cells["MaKH"].Value != null && row.Cells["MaKH"].Value != DBNull.Value)
                    cboKhachHang.SelectedValue = Convert.ToInt32(row.Cells["MaKH"].Value);

                if (row.Cells["MaNV"].Value != null && row.Cells["MaNV"].Value != DBNull.Value)
                    cboNhanVien.SelectedValue = Convert.ToInt32(row.Cells["MaNV"].Value);

                if (row.Cells["NgayDat"].Value != null && row.Cells["NgayDat"].Value != DBNull.Value)
                    dtpNgayDat.Value = Convert.ToDateTime(row.Cells["NgayDat"].Value);

                if (row.Cells["NgayNhan"].Value != null && row.Cells["NgayNhan"].Value != DBNull.Value)
                    dtpNgayNhan.Value = Convert.ToDateTime(row.Cells["NgayNhan"].Value);

                if (row.Cells["NgayTra"].Value != null && row.Cells["NgayTra"].Value != DBNull.Value)
                    dtpNgayTra.Value = Convert.ToDateTime(row.Cells["NgayTra"].Value);

                cboTrangThai.Text = row.Cells["TrangThai"].Value?.ToString() ?? "";

                // Nạp chi tiết các phòng của phiếu đặt phòng này
                if (int.TryParse(txtMaDP.Text, out int maDP))
                {
                    string tenKH = row.Cells["TenKhachHang"].Value?.ToString() ?? "";
                    lblDetailHeader.Text = $"🔍 CÁC PHÒNG ĐƯỢC ĐẶT TRONG PHIẾU #{maDP} ({tenKH}):";

                    DataTable dtCT = bus.GetChiTietPhongTheoMaDP(maDP);
                    dgvChiTietPhong.DataSource = dtCT;

                    if (dgvChiTietPhong.Columns["MaCTDP"] != null)
                        dgvChiTietPhong.Columns["MaCTDP"].HeaderText = "Mã CT";
                    if (dgvChiTietPhong.Columns["MaDP"] != null)
                        dgvChiTietPhong.Columns["MaDP"].Visible = false;
                    if (dgvChiTietPhong.Columns["MaPhong"] != null)
                        dgvChiTietPhong.Columns["MaPhong"].HeaderText = "Mã P";
                    if (dgvChiTietPhong.Columns["TenPhong"] != null)
                        dgvChiTietPhong.Columns["TenPhong"].HeaderText = "Tên phòng";
                    if (dgvChiTietPhong.Columns["TenLoai"] != null)
                        dgvChiTietPhong.Columns["TenLoai"].HeaderText = "Loại phòng";
                    if (dgvChiTietPhong.Columns["DonGia"] != null)
                    {
                        dgvChiTietPhong.Columns["DonGia"].HeaderText = "Đơn giá";
                        dgvChiTietPhong.Columns["DonGia"].DefaultCellStyle.Format = "#,##0 VNĐ";
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
                DataTable dt = bus.TimKiemDatPhong(kw);
                dgvDatPhong.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LamMoiForm()
        {
            dtPhongTam.Clear();
            CapNhatTongTienTam();
            LoadDanhSachPhongTrong();
            LoadData();

            txtMaDP.Text = bus.GetNextMaDP().ToString();
            cboKhachHang.SelectedIndex = -1;
            cboNhanVien.SelectedIndex = -1;
            cboTrangThai.SelectedIndex = 0;
            dtpNgayDat.Value = DateTime.Now;
            dtpNgayNhan.Value = DateTime.Now;
            dtpNgayTra.Value = DateTime.Now.AddDays(1);
            txtTimKiem.Clear();
        }
    }
}
