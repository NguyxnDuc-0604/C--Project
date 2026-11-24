using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKhachSan.BUS;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.GUI
{
    public class FrmNhapHang : Form
    {
        private readonly NhaCungCapBUS nccBUS = new();
        private readonly PhieuNhapBUS pnBUS = new();
        private readonly CTPhieuNhapBUS ctpnBUS = new();

        private TabControl tabControl = new();

        // Controls Tab 1: Nhà Cung Cấp
        private TextBox txtNCC_Ma = new();
        private TextBox txtNCC_Ten = new();
        private TextBox txtNCC_SDT = new();
        private TextBox txtNCC_DiaChi = new();
        private TextBox txtNCC_Email = new();
        private TextBox txtNCC_Tim = new();
        private DataGridView dgvNCC = new();

        // Controls Tab 2: Phiếu Nhập
        private TextBox txtPN_Ma = new();
        private ComboBox cboPN_NCC = new();
        private ComboBox cboPN_NV = new();
        private DateTimePicker dtpPN_Ngay = new();
        private TextBox txtPN_Tim = new();
        private DataGridView dgvPN = new();

        // Controls Tab 3: Chi Tiết Phiếu Nhập
        private TextBox txtCT_Ma = new();
        private ComboBox cboCT_PN = new();
        private ComboBox cboCT_DV = new();
        private TextBox txtCT_SoLuong = new();
        private TextBox txtCT_DonGia = new();
        private TextBox txtCT_ThanhTien = new();
        private TextBox txtCT_Tim = new();
        private Label lblCT_TongTien = new();
        private DataGridView dgvCT = new();
        private int _soLuongCu = 0;

        public FrmNhapHang()
        {
            Text = "HỆ THỐNG QUẢN LÝ NHẬP HÀNG";
            Size = new Size(1150, 750);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            BuildUI();

            Load += (s, e) =>
            {
                LoadDataNCC();
                LoadDataPN();
                LoadCombosPN();
                LoadDataCT();
                LoadCombosCT();
            };
        }

        private void BuildUI()
        {
            Label lblHeader = new Label
            {
                Text = "QUẢN LÝ NHẬP HÀNG",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Location = new Point(400, 15),
                AutoSize = true
            };
            Controls.Add(lblHeader);

            tabControl.SetBounds(20, 60, 1095, 630);
            tabControl.Font = new Font("Arial", 10, FontStyle.Regular);

            TabPage tabNCC = new TabPage("1. Nhà Cung Cấp");
            TabPage tabPN  = new TabPage("2. Phiếu Nhập");
            TabPage tabCT  = new TabPage("3. Chi Tiết Phiếu Nhập");

            BuildTabNCC(tabNCC);
            BuildTabPN(tabPN);
            BuildTabCT(tabCT);

            tabControl.TabPages.Add(tabNCC);
            tabControl.TabPages.Add(tabPN);
            tabControl.TabPages.Add(tabCT);

            Controls.Add(tabControl);
        }

        // ==========================================
        // TAB 1: NHÀ CUNG CẤP
        // ==========================================
        private void BuildTabNCC(TabPage tab)
        {
            tab.BackColor = Color.WhiteSmoke;

            AddLabel(tab, "Mã NCC:", 25, 25);
            txtNCC_Ma.SetBounds(120, 22, 250, 26);
            txtNCC_Ma.ReadOnly = true;
            txtNCC_Ma.BackColor = Color.LightGray;
            tab.Controls.Add(txtNCC_Ma);

            AddLabel(tab, "Tên NCC:", 25, 65);
            txtNCC_Ten.SetBounds(120, 62, 250, 26);
            tab.Controls.Add(txtNCC_Ten);

            AddLabel(tab, "Số ĐT:", 25, 105);
            txtNCC_SDT.SetBounds(120, 102, 250, 26);
            tab.Controls.Add(txtNCC_SDT);

            AddLabel(tab, "Email:", 430, 25);
            txtNCC_Email.SetBounds(520, 22, 530, 26);
            tab.Controls.Add(txtNCC_Email);

            AddLabel(tab, "Địa chỉ:", 430, 65);
            txtNCC_DiaChi.SetBounds(520, 62, 530, 66);
            txtNCC_DiaChi.Multiline = true;
            tab.Controls.Add(txtNCC_DiaChi);

            AddBtn(tab, "Thêm", 25, 150, (s, e) =>
            {
                try
                {
                    nccBUS.Them(txtNCC_Ten.Text, txtNCC_SDT.Text, txtNCC_DiaChi.Text, txtNCC_Email.Text);
                    MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataNCC();
                    LoadCombosPN();
                    ResetNCC();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            });

            AddBtn(tab, "Sửa", 135, 150, (s, e) =>
            {
                if (!int.TryParse(txtNCC_Ma.Text, out int id)) { MessageBox.Show("Vui lòng chọn NCC cần sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                try
                {
                    nccBUS.Sua(id, txtNCC_Ten.Text, txtNCC_SDT.Text, txtNCC_DiaChi.Text, txtNCC_Email.Text);
                    MessageBox.Show("Sửa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataNCC();
                    LoadCombosPN();
                    ResetNCC();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            });

            AddBtn(tab, "Xóa", 245, 150, (s, e) =>
            {
                if (!int.TryParse(txtNCC_Ma.Text, out int id)) { MessageBox.Show("Vui lòng chọn NCC cần xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        nccBUS.Xoa(id);
                        MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataNCC();
                        LoadCombosPN();
                        ResetNCC();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                }
            });

            AddBtn(tab, "Làm mới", 355, 150, (s, e) => ResetNCC());

            AddLabel(tab, "Tìm kiếm:", 550, 155);
            txtNCC_Tim.SetBounds(630, 152, 300, 26);
            tab.Controls.Add(txtNCC_Tim);

            AddBtn(tab, "Tìm", 945, 150, (s, e) =>
            {
                try { dgvNCC.DataSource = nccBUS.TimKiem(txtNCC_Tim.Text); }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            });

            SetupGrid(dgvNCC, 25, 200, 1030, 370);
            dgvNCC.CellClick += (_, e) =>
            {
                if (e.RowIndex < 0) return;
                var r = dgvNCC.Rows[e.RowIndex];
                txtNCC_Ma.Text = r.Cells["MaNCC"].Value?.ToString() ?? "";
                txtNCC_Ten.Text = r.Cells["TenNCC"].Value?.ToString() ?? "";
                txtNCC_SDT.Text = r.Cells["SDT"].Value?.ToString() ?? "";
                txtNCC_DiaChi.Text = r.Cells["DiaChi"].Value?.ToString() ?? "";
                txtNCC_Email.Text = r.Cells["Email"].Value?.ToString() ?? "";
            };
            tab.Controls.Add(dgvNCC);
        }

        private void LoadDataNCC()
        {
            try { dgvNCC.DataSource = nccBUS.GetAll(); }
            catch (Exception ex) { MessageBox.Show("Lỗi tải NCC: " + ex.Message); }
        }

        private void ResetNCC()
        {
            txtNCC_Ma.Clear();
            txtNCC_Ten.Clear();
            txtNCC_SDT.Clear();
            txtNCC_DiaChi.Clear();
            txtNCC_Email.Clear();
            txtNCC_Tim.Clear();
            LoadDataNCC();
        }

        // ==========================================
        // TAB 2: PHIẾU NHẬP
        // ==========================================
        private void BuildTabPN(TabPage tab)
        {
            tab.BackColor = Color.WhiteSmoke;

            AddLabel(tab, "Mã phiếu:", 25, 25);
            txtPN_Ma.SetBounds(140, 22, 280, 26);
            txtPN_Ma.ReadOnly = true;
            txtPN_Ma.BackColor = Color.LightGray;
            tab.Controls.Add(txtPN_Ma);

            AddLabel(tab, "Nhà cung cấp:", 25, 70);
            cboPN_NCC.SetBounds(140, 67, 360, 26);
            cboPN_NCC.DropDownStyle = ComboBoxStyle.DropDownList;
            tab.Controls.Add(cboPN_NCC);

            AddLabel(tab, "Nhân viên lập:", 25, 115);
            cboPN_NV.SetBounds(140, 112, 360, 26);
            cboPN_NV.DropDownStyle = ComboBoxStyle.DropDownList;
            tab.Controls.Add(cboPN_NV);

            AddLabel(tab, "Ngày nhập:", 560, 25);
            dtpPN_Ngay.SetBounds(660, 22, 280, 26);
            dtpPN_Ngay.Format = DateTimePickerFormat.Short;
            tab.Controls.Add(dtpPN_Ngay);

            AddBtn(tab, "Thêm", 25, 160, (s, e) =>
            {
                if (cboPN_NCC.SelectedValue == null) { MessageBox.Show("Vui lòng chọn NCC!"); return; }
                if (cboPN_NV.SelectedValue == null) { MessageBox.Show("Vui lòng chọn NV!"); return; }
                try
                {
                    pnBUS.Them(Convert.ToInt32(cboPN_NCC.SelectedValue), Convert.ToInt32(cboPN_NV.SelectedValue), dtpPN_Ngay.Value.Date);
                    MessageBox.Show("Thêm phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataPN();
                    LoadCombosCT();
                    ResetPN();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            });

            AddBtn(tab, "Sửa", 135, 160, (s, e) =>
            {
                if (!int.TryParse(txtPN_Ma.Text, out int id)) { MessageBox.Show("Chọn phiếu nhập cần sửa!"); return; }
                try
                {
                    pnBUS.Sua(id, Convert.ToInt32(cboPN_NCC.SelectedValue), Convert.ToInt32(cboPN_NV.SelectedValue), dtpPN_Ngay.Value.Date);
                    MessageBox.Show("Sửa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataPN();
                    LoadCombosCT();
                    ResetPN();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            });

            AddBtn(tab, "Xóa", 245, 160, (s, e) =>
            {
                if (!int.TryParse(txtPN_Ma.Text, out int id)) { MessageBox.Show("Chọn phiếu nhập cần xóa!"); return; }
                if (MessageBox.Show("Xóa phiếu nhập này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        pnBUS.Xoa(id);
                        MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataPN();
                        LoadCombosCT();
                        ResetPN();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                }
            });

            AddBtn(tab, "Làm mới", 355, 160, (s, e) => ResetPN());

            AddLabel(tab, "Tìm kiếm:", 550, 165);
            txtPN_Tim.SetBounds(630, 162, 300, 26);
            tab.Controls.Add(txtPN_Tim);

            AddBtn(tab, "Tìm", 945, 160, (s, e) =>
            {
                try { dgvPN.DataSource = pnBUS.TimKiem(txtPN_Tim.Text); }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            });

            SetupGrid(dgvPN, 25, 210, 1030, 360);
            dgvPN.CellClick += (_, e) =>
            {
                if (e.RowIndex < 0) return;
                var r = dgvPN.Rows[e.RowIndex];
                txtPN_Ma.Text = r.Cells["MaPN"].Value?.ToString() ?? "";
                if (r.Cells["MaNCC"].Value != null) cboPN_NCC.SelectedValue = Convert.ToInt32(r.Cells["MaNCC"].Value);
                if (r.Cells["MaNV"].Value != null) cboPN_NV.SelectedValue = Convert.ToInt32(r.Cells["MaNV"].Value);
                if (r.Cells["NgayNhap"].Value != null) dtpPN_Ngay.Value = Convert.ToDateTime(r.Cells["NgayNhap"].Value);
            };
            tab.Controls.Add(dgvPN);
        }

        private void LoadCombosPN()
        {
            try
            {
                cboPN_NCC.DataSource = pnBUS.GetNhaCungCap();
                cboPN_NCC.DisplayMember = "TenNCC";
                cboPN_NCC.ValueMember = "MaNCC";
                cboPN_NCC.SelectedIndex = -1;

                cboPN_NV.DataSource = pnBUS.GetNhanVien();
                cboPN_NV.DisplayMember = "HoTen";
                cboPN_NV.ValueMember = "MaNV";
                cboPN_NV.SelectedIndex = -1;
            }
            catch { }
        }

        private void LoadDataPN()
        {
            try { dgvPN.DataSource = pnBUS.GetAll(); }
            catch (Exception ex) { MessageBox.Show("Lỗi tải PN: " + ex.Message); }
        }

        private void ResetPN()
        {
            txtPN_Ma.Clear();
            txtPN_Tim.Clear();
            cboPN_NCC.SelectedIndex = -1;
            cboPN_NV.SelectedIndex = -1;
            dtpPN_Ngay.Value = DateTime.Today;
            LoadDataPN();
        }

        // ==========================================
        // TAB 3: CHI TIẾT PHIẾU NHẬP
        // ==========================================
        private void BuildTabCT(TabPage tab)
        {
            tab.BackColor = Color.WhiteSmoke;

            AddLabel(tab, "Mã chi tiết:", 25, 20);
            txtCT_Ma.SetBounds(130, 17, 200, 26);
            txtCT_Ma.ReadOnly = true;
            txtCT_Ma.BackColor = Color.LightGray;
            tab.Controls.Add(txtCT_Ma);

            AddLabel(tab, "Phiếu nhập:", 380, 20);
            cboCT_PN.SetBounds(480, 17, 180, 26);
            cboCT_PN.DropDownStyle = ComboBoxStyle.DropDownList;
            tab.Controls.Add(cboCT_PN);

            AddLabel(tab, "Dịch vụ/Hàng:", 25, 60);
            cboCT_DV.SetBounds(130, 57, 300, 26);
            cboCT_DV.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCT_DV.SelectedIndexChanged += (s, e) =>
            {
                if (cboCT_DV.SelectedIndex >= 0 && cboCT_DV.DataSource is DataTable dt && cboCT_DV.SelectedIndex < dt.Rows.Count)
                {
                    txtCT_DonGia.Text = dt.Rows[cboCT_DV.SelectedIndex]["DonGia"].ToString();
                    CalcThanhTien();
                }
            };
            tab.Controls.Add(cboCT_DV);

            AddLabel(tab, "Số lượng:", 460, 60);
            txtCT_SoLuong.SetBounds(540, 57, 120, 26);
            txtCT_SoLuong.TextChanged += (s, e) => CalcThanhTien();
            tab.Controls.Add(txtCT_SoLuong);

            AddLabel(tab, "Đơn giá:", 690, 60);
            txtCT_DonGia.SetBounds(770, 57, 140, 26);
            txtCT_DonGia.ReadOnly = true;
            txtCT_DonGia.BackColor = Color.LightGray;
            tab.Controls.Add(txtCT_DonGia);

            AddLabel(tab, "Thành tiền:", 25, 100);
            txtCT_ThanhTien.SetBounds(130, 97, 200, 26);
            txtCT_ThanhTien.ReadOnly = true;
            txtCT_ThanhTien.BackColor = Color.LightGray;
            tab.Controls.Add(txtCT_ThanhTien);

            lblCT_TongTien.SetBounds(380, 100, 450, 25);
            lblCT_TongTien.Font = new Font("Arial", 11, FontStyle.Bold);
            lblCT_TongTien.ForeColor = Color.DarkRed;
            lblCT_TongTien.Text = "Tổng tiền phiếu: 0 VNĐ";
            tab.Controls.Add(lblCT_TongTien);

            AddBtn(tab, "Thêm", 25, 145, (s, e) =>
            {
                if (!ValidateCT(out CTPhieuNhap ct)) return;
                try
                {
                    ctpnBUS.Them(ct);
                    MessageBox.Show("Thêm thành công! Tồn kho đã tự động cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataCT();
                    CalcTongTien();
                    ResetCT();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            });

            AddBtn(tab, "Sửa", 135, 145, (s, e) =>
            {
                if (!ValidateCT(out CTPhieuNhap ct)) return;
                try
                {
                    ctpnBUS.Sua(ct, _soLuongCu);
                    MessageBox.Show("Sửa thành công! Tồn kho đã tự động điều chỉnh.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataCT();
                    CalcTongTien();
                    ResetCT();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            });

            AddBtn(tab, "Xóa", 245, 145, (s, e) =>
            {
                if (!int.TryParse(txtCT_Ma.Text, out int id)) { MessageBox.Show("Chọn chi tiết cần xóa!"); return; }
                if (MessageBox.Show("Xóa chi tiết này? Tồn kho sẽ trừ lại.", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        ctpnBUS.Xoa(id);
                        MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataCT();
                        ResetCT();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                }
            });

            AddBtn(tab, "Làm mới", 355, 145, (s, e) => ResetCT());

            AddLabel(tab, "Tìm kiếm:", 550, 150);
            txtCT_Tim.SetBounds(630, 147, 300, 26);
            tab.Controls.Add(txtCT_Tim);

            AddBtn(tab, "Tìm", 945, 145, (s, e) =>
            {
                try { dgvCT.DataSource = ctpnBUS.TimKiem(txtCT_Tim.Text); }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            });

            SetupGrid(dgvCT, 25, 195, 1030, 375);
            dgvCT.CellClick += (_, e) =>
            {
                if (e.RowIndex < 0) return;
                var r = dgvCT.Rows[e.RowIndex];
                txtCT_Ma.Text = r.Cells["MaCTPN"].Value?.ToString() ?? "";
                txtCT_SoLuong.Text = r.Cells["SoLuong"].Value?.ToString() ?? "";
                txtCT_DonGia.Text = r.Cells["DonGiaNhap"].Value?.ToString() ?? "";
                txtCT_ThanhTien.Text = r.Cells["ThanhTien"].Value?.ToString() ?? "";
                int.TryParse(txtCT_SoLuong.Text, out _soLuongCu);

                if (r.Cells["MaPN"].Value != null) cboCT_PN.SelectedValue = Convert.ToInt32(r.Cells["MaPN"].Value);
                if (r.Cells["MaDV"].Value != null) cboCT_DV.SelectedValue = Convert.ToInt32(r.Cells["MaDV"].Value);
                CalcTongTien();
            };
            tab.Controls.Add(dgvCT);
        }

        private void CalcThanhTien()
        {
            if (int.TryParse(txtCT_SoLuong.Text.Trim(), out int sl) && decimal.TryParse(txtCT_DonGia.Text.Trim(), out decimal dg))
                txtCT_ThanhTien.Text = (sl * dg).ToString("N0");
            else
                txtCT_ThanhTien.Text = "";
        }

        private void CalcTongTien()
        {
            if (cboCT_PN.SelectedValue != null && int.TryParse(cboCT_PN.SelectedValue.ToString(), out int id))
            {
                decimal total = ctpnBUS.TinhTongTien(id);
                lblCT_TongTien.Text = "Tổng tiền phiếu " + id + ": " + total.ToString("N0") + " VNĐ";
            }
        }

        private bool ValidateCT(out CTPhieuNhap ct)
        {
            ct = new CTPhieuNhap();
            if (cboCT_PN.SelectedValue == null) { MessageBox.Show("Vui lòng chọn phiếu nhập!"); return false; }
            if (cboCT_DV.SelectedValue == null) { MessageBox.Show("Vui lòng chọn dịch vụ!"); return false; }
            if (!int.TryParse(txtCT_SoLuong.Text.Trim(), out int sl) || sl <= 0) { MessageBox.Show("Số lượng phải > 0!"); return false; }
            if (!decimal.TryParse(txtCT_DonGia.Text.Trim(), out decimal dg) || dg <= 0) { MessageBox.Show("Đơn giá phải > 0!"); return false; }

            ct.MaCTPN = int.TryParse(txtCT_Ma.Text, out int id) ? id : 0;
            ct.MaPN = Convert.ToInt32(cboCT_PN.SelectedValue);
            ct.MaDV = Convert.ToInt32(cboCT_DV.SelectedValue);
            ct.SoLuong = sl;
            ct.DonGiaNhap = dg;
            return true;
        }

        private void LoadCombosCT()
        {
            try
            {
                cboCT_PN.DataSource = ctpnBUS.GetDanhSachPhieuNhap();
                cboCT_PN.DisplayMember = "MaPN";
                cboCT_PN.ValueMember = "MaPN";
                cboCT_PN.SelectedIndex = -1;

                cboCT_DV.DataSource = ctpnBUS.GetDanhSachDichVu();
                cboCT_DV.DisplayMember = "TenDV";
                cboCT_DV.ValueMember = "MaDV";
                cboCT_DV.SelectedIndex = -1;
            }
            catch { }
        }

        private void LoadDataCT()
        {
            try { dgvCT.DataSource = ctpnBUS.GetAll(); }
            catch (Exception ex) { MessageBox.Show("Lỗi tải chi tiết: " + ex.Message); }
        }

        private void ResetCT()
        {
            txtCT_Ma.Clear();
            txtCT_SoLuong.Clear();
            txtCT_DonGia.Clear();
            txtCT_ThanhTien.Clear();
            txtCT_Tim.Clear();
            cboCT_PN.SelectedIndex = -1;
            cboCT_DV.SelectedIndex = -1;
            lblCT_TongTien.Text = "Tổng tiền phiếu: 0 VNĐ";
            _soLuongCu = 0;
            LoadDataCT();
        }

        // ==========================================
        // HELPERS
        // ==========================================
        private void AddLabel(TabPage tab, string text, int x, int y)
        {
            tab.Controls.Add(new Label { Text = text, Location = new Point(x, y + 3), AutoSize = true, Font = new Font("Arial", 10) });
        }

        private void AddBtn(TabPage tab, string text, int x, int y, EventHandler h)
        {
            var b = new Button { Text = text, Location = new Point(x, y), Size = new Size(100, 34), BackColor = Color.LightSteelBlue, Font = new Font("Arial", 9.5f) };
            b.Click += h;
            tab.Controls.Add(b);
        }

        private void SetupGrid(DataGridView g, int x, int y, int w, int h)
        {
            g.SetBounds(x, y, w, h);
            g.ReadOnly = true;
            g.AllowUserToAddRows = false;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.MultiSelect = false;
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            g.RowHeadersVisible = false;
            g.BackgroundColor = Color.White;
        }
    }
}
