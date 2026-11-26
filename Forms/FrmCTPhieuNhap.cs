using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKhachSan.BUS;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.Forms
{
    public class FrmCTPhieuNhap : Form
    {
        private readonly CTPhieuNhapBUS bus = new();

        private TextBox  txtMaCTPN    = new();
        private ComboBox cboPhieuNhap = new();
        private ComboBox cboDichVu    = new();
        private TextBox  txtSoLuong   = new();
        private TextBox  txtDonGia    = new();
        private TextBox  txtThanhTien = new();
        private TextBox  txtTimKiem   = new();

        private DataGridView dgv        = new();
        private Label        lblTongTien = new();


        public FrmCTPhieuNhap()
        {
            Text = "QUẢN LÝ CHI TIẾT PHIẾU NHẬP";
            Size = new Size(1100, 720);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            Build();

            Load += (s, e) =>
            {
                LoadCombos();
                LoadData();
            };
        }

        private void Build()
        {
            Controls.Add(new Label
            {
                Text = "QUẢN LÝ CHI TIẾT PHIẾU NHẬP",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Location = new Point(290, 20),
                AutoSize = true
            });

            Field("Mã chi tiết:", txtMaCTPN, 30, 85);
            txtMaCTPN.ReadOnly = true;
            txtMaCTPN.BackColor = Color.LightGray;

            Field("Phiếu nhập:", cboPhieuNhap, 580, 85);
            cboPhieuNhap.DropDownStyle = ComboBoxStyle.DropDownList;

            Field("Dịch vụ:", cboDichVu, 30, 130);
            cboDichVu.DropDownStyle = ComboBoxStyle.DropDownList;

            cboDichVu.SelectedIndexChanged += (s, e) =>
            {
                if (cboDichVu.SelectedIndex >= 0 &&
                    cboDichVu.SelectedValue != null &&
                    cboDichVu.SelectedValue is not DataRowView)
                {
                    DataTable? dt = cboDichVu.DataSource as DataTable;
                    if (dt != null && cboDichVu.SelectedIndex < dt.Rows.Count)
                    {
                        txtDonGia.Text = dt.Rows[cboDichVu.SelectedIndex]["DonGia"].ToString();
                        TinhThanhTien();
                    }
                }
            };

            Field("Số lượng:", txtSoLuong, 30, 175);
            txtSoLuong.TextChanged += (s, e) => TinhThanhTien();

            Field("Đơn giá nhập:", txtDonGia, 580, 130);
            txtDonGia.ReadOnly = true;
            txtDonGia.BackColor = Color.LightGray;

            Field("Thành tiền:", txtThanhTien, 580, 175);
            txtThanhTien.ReadOnly = true;
            txtThanhTien.BackColor = Color.LightGray;

            Btn("Thêm",    30,  225, Them);
            Btn("Sửa",     140, 225, Sua);
            Btn("Xóa",     250, 225, Xoa);
            Btn("Làm mới", 360, 225, Reset);

            Controls.Add(new Label { Text = "Tìm kiếm:", Location = new Point(580, 235), AutoSize = true });
            txtTimKiem.SetBounds(665, 230, 200, 28);
            Controls.Add(txtTimKiem);
            Btn("Tìm", 875, 228, Search);

            Grid(dgv, 30, 280, 1030, 320);
            dgv.CellClick += Dgv_CellClick;

            lblTongTien.Location = new Point(30, 615);
            lblTongTien.AutoSize = true;
            lblTongTien.Font = new Font("Arial", 12, FontStyle.Bold);
            lblTongTien.ForeColor = Color.DarkRed;
            lblTongTien.Text = "Tổng tiền phiếu: 0 VNĐ";
            Controls.Add(lblTongTien);
        }

        private void Field(string label, Control c, int x, int y)
        {
            Controls.Add(new Label { Text = label, Location = new Point(x, y + 4), Size = new Size(120, 24) });
            c.SetBounds(x + 125, y, 350, 28);
            Controls.Add(c);
        }

        private void Btn(string text, int x, int y, EventHandler h)
        {
            var b = new Button { Text = text, Location = new Point(x, y), Size = new Size(100, 34), BackColor = Color.LightSteelBlue };
            b.Click += h;
            Controls.Add(b);
        }

        private static void Grid(DataGridView g, int x, int y, int w, int h)
        {
            g.SetBounds(x, y, w, h);
            g.ReadOnly = true;
            g.AllowUserToAddRows = false;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.MultiSelect = false;
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            g.RowHeadersVisible = false;
        }

        private static string V(DataGridViewRow r, string col) => r.Cells[col].Value?.ToString() ?? "";

        private void LoadCombos()
        {
            try
            {
                cboPhieuNhap.DataSource    = bus.GetDanhSachPhieuNhap();
                cboPhieuNhap.DisplayMember = "MaPN";
                cboPhieuNhap.ValueMember   = "MaPN";
                cboPhieuNhap.SelectedIndex = -1;

                cboDichVu.DataSource    = bus.GetDanhSachDichVu();
                cboDichVu.DisplayMember = "TenDV";
                cboDichVu.ValueMember   = "MaDV";
                cboDichVu.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải ComboBox: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            try
            {
                dgv.DataSource = bus.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TinhThanhTien()
        {
            if (int.TryParse(txtSoLuong.Text.Trim(), out int sl) &&
                decimal.TryParse(txtDonGia.Text.Trim(), out decimal dg))
                txtThanhTien.Text = (sl * dg).ToString("N0");
            else
                txtThanhTien.Text = "";
        }

        private void HienThiTongTien()
        {
            if (cboPhieuNhap.SelectedValue != null &&
                int.TryParse(cboPhieuNhap.SelectedValue.ToString(), out int maPN))
            {
                try
                {
                    decimal tong = bus.TinhTongTien(maPN);
                    lblTongTien.Text = "Tổng tiền phiếu: " + tong.ToString("N0") + " VNĐ";
                }
                catch
                {
                    lblTongTien.Text = "Tổng tiền phiếu: 0 VNĐ";
                }
            }
        }

        private bool Valid(out CTPhieuNhap ct)
        {
            ct = new CTPhieuNhap();

            if (cboPhieuNhap.SelectedValue == null || cboPhieuNhap.SelectedValue is DataRowView)
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (cboDichVu.SelectedValue == null || cboDichVu.SelectedValue is DataRowView)
            {
                MessageBox.Show("Vui lòng chọn dịch vụ/mặt hàng nhập!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!int.TryParse(txtSoLuong.Text.Trim(), out int sl) || sl <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return false;
            }
            if (!decimal.TryParse(txtDonGia.Text.Trim(), out decimal dg) || dg <= 0)
            {
                MessageBox.Show("Đơn giá nhập không hợp lệ hoặc phải lớn hơn 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            ct.MaCTPN     = int.TryParse(txtMaCTPN.Text, out int id) ? id : 0;
            ct.MaPN       = Convert.ToInt32(cboPhieuNhap.SelectedValue);
            ct.MaDV       = Convert.ToInt32(cboDichVu.SelectedValue);
            ct.SoLuong    = sl;
            ct.DonGiaNhap = dg;

            return true;
        }

        private void Them(object? s, EventArgs e)
        {
            if (!Valid(out CTPhieuNhap ct)) return;
            try
            {
                bus.Them(ct);
                MessageBox.Show("Thêm chi tiết thành công! Tồn kho dịch vụ đã được tự động cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                HienThiTongTien();
                Reset(null!, EventArgs.Empty);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Sua(object? s, EventArgs e)
        {
            if (!Valid(out CTPhieuNhap ct)) return;
            try
            {
                bus.Sua(ct);
                MessageBox.Show("Sửa chi tiết thành công! Tồn kho đã được tự động điều chỉnh theo chênh lệch.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                HienThiTongTien();
                Reset(null!, EventArgs.Empty);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sửa thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Xoa(object? s, EventArgs e)
        {
            if (!int.TryParse(txtMaCTPN.Text, out int ma))
            {
                MessageBox.Show("Vui lòng chọn chi tiết cần xóa từ bảng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa chi tiết này? Tồn kho dịch vụ sẽ được trừ lại tự động.", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    bus.Xoa(ma);
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    Reset(null!, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xóa thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Search(object? s, EventArgs e)
        {
            try
            {
                dgv.DataSource = bus.TimKiem(txtTimKiem.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Reset(object? s, EventArgs e)
        {
            txtMaCTPN.Clear();
            txtSoLuong.Clear();
            txtDonGia.Clear();
            txtThanhTien.Clear();
            txtTimKiem.Clear();
            cboPhieuNhap.SelectedIndex = -1;
            cboDichVu.SelectedIndex    = -1;
            lblTongTien.Text = "Tổng tiền phiếu: 0 VNĐ";
            LoadData();
        }

        private void Dgv_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var r = dgv.Rows[e.RowIndex];

            txtMaCTPN.Text    = V(r, "MaCTPN");
            txtSoLuong.Text   = V(r, "SoLuong");
            txtDonGia.Text    = V(r, "DonGiaNhap");
            txtThanhTien.Text = V(r, "ThanhTien");


            object? maPN = r.Cells["MaPN"].Value;
            if (maPN != null && maPN != DBNull.Value)
                cboPhieuNhap.SelectedValue = Convert.ToInt32(maPN);

            object? maDV = r.Cells["MaDV"].Value;
            if (maDV != null && maDV != DBNull.Value)
                cboDichVu.SelectedValue = Convert.ToInt32(maDV);

            HienThiTongTien();
        }
    }
}
