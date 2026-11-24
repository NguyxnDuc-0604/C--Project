using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKhachSan.BUS;

namespace QuanLyKhachSan.GUI
{
    /// <summary>
    /// TẦNG GUI: MÀN HÌNH QUẢN LÝ PHIẾU NHẬP
    /// Nhiệm vụ: Xử lý giao diện, ComboBox Nhà cung cấp & Nhân viên, gọi PhieuNhapBUS
    /// </summary>
    public class GUI_PhieuNhap : Form
    {
        private readonly PhieuNhapBUS pnBUS = new();

        private TextBox        txtMaPN    = new();
        private ComboBox       cboNCC     = new();
        private ComboBox       cboNV      = new();
        private DateTimePicker dtpNgay    = new();
        private TextBox        txtTimKiem = new();
        private DataGridView   dgvPN      = new();

        public GUI_PhieuNhap()
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
            Text = "QUẢN LÝ PHIẾU NHẬP - [TẦNG GUI]";
            Size = new Size(1100, 680);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ THÔNG TIN PHIẾU NHẬP HÀNG",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Location = new Point(310, 20),
                AutoSize = true
            };
            Controls.Add(lblTitle);

            GroupBox grpInfo = new GroupBox
            {
                Text = "Thông tin phiếu nhập",
                Location = new Point(25, 65),
                Size = new Size(1035, 140),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            ThemNhanVaO(grpInfo, "Mã phiếu:", txtMaPN, 20, 30, 250);
            txtMaPN.ReadOnly = true;
            txtMaPN.BackColor = Color.LightGray;

            ThemNhanVaO(grpInfo, "Nhà cung cấp:", cboNCC, 20, 75, 380);
            cboNCC.DropDownStyle = ComboBoxStyle.DropDownList;

            ThemNhanVaO(grpInfo, "Nhân viên lập:", cboNV, 480, 75, 380);
            cboNV.DropDownStyle = ComboBoxStyle.DropDownList;

            Label lblNgay = new Label { Text = "Ngày nhập:", Location = new Point(480, 33), AutoSize = true, Font = new Font("Arial", 9.5f, FontStyle.Regular) };
            grpInfo.Controls.Add(lblNgay);
            dtpNgay.SetBounds(580, 30, 280, 26);
            dtpNgay.Format = DateTimePickerFormat.Short;
            dtpNgay.Font = new Font("Arial", 9.5f, FontStyle.Regular);
            grpInfo.Controls.Add(dtpNgay);

            Controls.Add(grpInfo);

            ThemNut("Thêm mới", 25, 220, BtnThem_Click);
            ThemNut("Cập nhật", 140, 220, BtnSua_Click);
            ThemNut("Xóa", 255, 220, BtnXoa_Click);
            ThemNut("Làm mới", 370, 220, BtnLamMoi_Click);

            Label lblTim = new Label { Text = "Tìm kiếm:", Location = new Point(550, 228), AutoSize = true, Font = new Font("Arial", 10) };
            Controls.Add(lblTim);

            txtTimKiem.SetBounds(630, 224, 280, 26);
            txtTimKiem.Font = new Font("Arial", 10);
            Controls.Add(txtTimKiem);

            ThemNut("Tìm", 930, 220, BtnTimKiem_Click);

            dgvPN.SetBounds(25, 270, 1035, 350);
            dgvPN.ReadOnly = true;
            dgvPN.AllowUserToAddRows = false;
            dgvPN.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPN.MultiSelect = false;
            dgvPN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPN.RowHeadersVisible = false;
            dgvPN.BackgroundColor = Color.White;
            dgvPN.CellClick += DgvPN_CellClick;
            Controls.Add(dgvPN);
        }

        private void ThemNhanVaO(GroupBox grp, string nhan, Control c, int x, int y, int w)
        {
            Label lbl = new Label { Text = nhan, Location = new Point(x, y + 3), AutoSize = true, Font = new Font("Arial", 9.5f, FontStyle.Regular) };
            grp.Controls.Add(lbl);
            c.SetBounds(x + 105, y, w, 26);
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
                // Gọi qua PhieuNhapBUS để nạp dữ liệu
                cboNCC.DataSource    = pnBUS.GetNhaCungCap();
                cboNCC.DisplayMember = "TenNCC";
                cboNCC.ValueMember   = "MaNCC";
                cboNCC.SelectedIndex = -1;

                cboNV.DataSource    = pnBUS.GetNhanVien();
                cboNV.DisplayMember = "HoTen";
                cboNV.ValueMember   = "MaNV";
                cboNV.SelectedIndex = -1;
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
                dgvPN.DataSource = pnBUS.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            if (cboNCC.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cboNV.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                pnBUS.Them(Convert.ToInt32(cboNCC.SelectedValue), Convert.ToInt32(cboNV.SelectedValue), dtpNgay.Value.Date);
                MessageBox.Show("Thêm phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDuLieu();
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
            if (!int.TryParse(txtMaPN.Text, out int id))
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập từ bảng để sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboNCC.SelectedValue == null || cboNV.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ NCC và Nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                pnBUS.Sua(id, Convert.ToInt32(cboNCC.SelectedValue), Convert.ToInt32(cboNV.SelectedValue), dtpNgay.Value.Date);
                MessageBox.Show("Sửa phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDuLieu();
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
            if (!int.TryParse(txtMaPN.Text, out int id))
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập từ bảng để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Xóa phiếu nhập này? Các chi tiết tương ứng cũng sẽ bị xóa.", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    pnBUS.Xoa(id);
                    MessageBox.Show("Xóa phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                dgvPN.DataSource = pnBUS.TimKiem(txtTimKiem.Text);
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
            txtMaPN.Clear();
            txtTimKiem.Clear();
            cboNCC.SelectedIndex = -1;
            cboNV.SelectedIndex  = -1;
            dtpNgay.Value = DateTime.Today;
            TaiDuLieu();
        }

        private void DgvPN_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvPN.Rows[e.RowIndex];

            txtMaPN.Text = row.Cells["MaPN"].Value?.ToString() ?? "";

            if (row.Cells["MaNCC"].Value != null)
                cboNCC.SelectedValue = Convert.ToInt32(row.Cells["MaNCC"].Value);

            if (row.Cells["MaNV"].Value != null)
                cboNV.SelectedValue = Convert.ToInt32(row.Cells["MaNV"].Value);

            if (row.Cells["NgayNhap"].Value != null)
                dtpNgay.Value = Convert.ToDateTime(row.Cells["NgayNhap"].Value);
        }
    }
}
