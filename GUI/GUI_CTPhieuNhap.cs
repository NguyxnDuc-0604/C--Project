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
    /// Nhiệm vụ: Tự động tính tiền, hiển thị tổng tiền, cập nhật tồn kho qua CTPhieuNhapBUS
    /// </summary>
    public class GUI_CTPhieuNhap : Form
    {
        private readonly CTPhieuNhapBUS ctpnBUS = new();

        private TextBox  txtMaCTPN    = new();
        private ComboBox cboPhieuNhap = new();
        private ComboBox cboDichVu    = new();
        private TextBox  txtSoLuong   = new();
        private TextBox  txtDonGia    = new();
        private TextBox  txtThanhTien = new();
        private TextBox  txtTimKiem   = new();
        private Label    lblTongTien  = new();
        private DataGridView dgvCTPN  = new();


        public GUI_CTPhieuNhap()
        {
            KhoiTaoGiaoDien();
            Load += (s, e) =>
            {
                TaiDanhSachComboBox();
                TaiDuLieu();
            };
        }

        private void KhoiTaoGiaoDien()
        {
            Text = "QUẢN LÝ CHI TIẾT PHIẾU NHẬP - [TẦNG GUI]";
            Size = new Size(1100, 720);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ CHI TIẾT HÀNG HÓA NHẬP KHO",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Location = new Point(310, 20),
                AutoSize = true
            };
            Controls.Add(lblTitle);

            GroupBox grpInfo = new GroupBox
            {
                Text = "Thông tin chi tiết mặt hàng",
                Location = new Point(25, 65),
                Size = new Size(1035, 150),
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

            ThemNut("Thêm mới", 25, 230, BtnThem_Click);
            ThemNut("Cập nhật", 140, 230, BtnSua_Click);
            ThemNut("Xóa", 255, 230, BtnXoa_Click);
            ThemNut("Làm mới", 370, 230, BtnLamMoi_Click);

            Label lblTim = new Label { Text = "Tìm kiếm:", Location = new Point(550, 238), AutoSize = true, Font = new Font("Arial", 10) };
            Controls.Add(lblTim);

            txtTimKiem.SetBounds(630, 234, 280, 26);
            txtTimKiem.Font = new Font("Arial", 10);
            Controls.Add(txtTimKiem);

            ThemNut("Tìm", 930, 230, BtnTimKiem_Click);

            dgvCTPN.SetBounds(25, 280, 1035, 370);
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

        private void ThemNhanVaO(GroupBox grp, string nhan, Control c, int x, int y, int w)
        {
            Label lbl = new Label { Text = nhan, Location = new Point(x, y + 3), AutoSize = true, Font = new Font("Arial", 9.5f, FontStyle.Regular) };
            grp.Controls.Add(lbl);
            c.SetBounds(x + 100, y, w, 26);
            c.Font = new Font("Arial", 9.5f, FontStyle.Regular);
            grp.Controls.Add(c);
        }

        private void ThemNut(string text, int x, int y, EventHandler onClick)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(100, 35),
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
            try
            {
                dgvCTPN.DataSource = ctpnBUS.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void CapNhatTongTien()
        {
            if (cboPhieuNhap.SelectedValue != null && int.TryParse(cboPhieuNhap.SelectedValue.ToString(), out int id))
            {
                decimal total = ctpnBUS.TinhTongTien(id);
                lblTongTien.Text = $"Tổng tiền phiếu #{id}: {total:N0} VNĐ";
            }
        }

        private bool KiemTraHopLe(out CTPhieuNhap ct)
        {
            ct = new CTPhieuNhap();

            if (cboPhieuNhap.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (cboDichVu.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn mặt hàng/dịch vụ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!int.TryParse(txtSoLuong.Text.Trim(), out int sl) || sl <= 0)
            {
                MessageBox.Show("Số lượng nhập phải lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return false;
            }
            if (!decimal.TryParse(txtDonGia.Text.Trim(), out decimal dg) || dg <= 0)
            {
                MessageBox.Show("Đơn giá nhập phải lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            ct.MaCTPN = int.TryParse(txtMaCTPN.Text, out int id) ? id : 0;
            ct.MaPN = Convert.ToInt32(cboPhieuNhap.SelectedValue);
            ct.MaDV = Convert.ToInt32(cboDichVu.SelectedValue);
            ct.SoLuong = sl;
            ct.DonGiaNhap = dg;
            return true;
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            if (!KiemTraHopLe(out CTPhieuNhap ct)) return;
            try
            {
                ctpnBUS.Them(ct);
                MessageBox.Show("Thêm thành công! Tồn kho mặt hàng đã tự động được cộng thêm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Thêm thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Cảnh báo nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sửa thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaCTPN.Text, out int id))
            {
                MessageBox.Show("Vui lòng chọn một chi tiết cần xóa từ bảng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                catch (Exception ex)
                {
                    MessageBox.Show("Xóa thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnTimKiem_Click(object? sender, EventArgs e)
        {
            try
            {
                dgvCTPN.DataSource = ctpnBUS.TimKiem(txtTimKiem.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            LamMoi();
        }

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
