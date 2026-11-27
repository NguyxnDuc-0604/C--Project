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

        private TextBox        txtMaPN    = new();
        private ComboBox       cboNCC     = new();
        private ComboBox       cboNV      = new();
        private DateTimePicker dtpNgay    = new();
        private TextBox        txtTimKiem = new();

        private DataGridView dgv = new();

        public FrmPhieuNhap()
        {
            Text = "QUẢN LÝ PHIẾU NHẬP";
            Size = new Size(1100, 680);
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
                Text = "QUẢN LÝ PHIẾU NHẬP",
                Font = new Font("Arial", 19, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Location = new Point(380, 20),
                AutoSize = true
            });

            Field("Mã phiếu:", txtMaPN, 30, 85);
            txtMaPN.ReadOnly = true;
            txtMaPN.BackColor = Color.LightGray;

            Field("Nhà cung cấp:", cboNCC, 30, 130);
            cboNCC.DropDownStyle = ComboBoxStyle.DropDownList;

            Field("Nhân viên:", cboNV, 30, 175);
            cboNV.DropDownStyle = ComboBoxStyle.DropDownList;

            Controls.Add(new Label { Text = "Ngày nhập:", Location = new Point(600, 89), Size = new Size(110, 24) });
            dtpNgay.SetBounds(715, 85, 280, 28);
            dtpNgay.Format = DateTimePickerFormat.Short;
            Controls.Add(dtpNgay);

            Btn("Thêm",    30,  225, Them);
            Btn("Sửa",     140, 225, Sua);
            Btn("Xóa",     250, 225, Xoa);
            Btn("Làm mới", 360, 225, Reset);

            Controls.Add(new Label { Text = "Tìm kiếm:", Location = new Point(600, 235), AutoSize = true });
            txtTimKiem.SetBounds(690, 230, 200, 28);
            Controls.Add(txtTimKiem);
            Btn("Tìm", 900, 228, Search);

            Grid(dgv, 30, 280, 1020, 340);
            Controls.Add(dgv);
            dgv.CellClick += (_, e) =>
            {
                if (e.RowIndex < 0) return;
                var r = dgv.Rows[e.RowIndex];

                txtMaPN.Text = V(r, "MaPN");

                object? maNCC = r.Cells["MaNCC"].Value;
                if (maNCC != null && maNCC != DBNull.Value)
                    cboNCC.SelectedValue = Convert.ToInt32(maNCC);

                object? maNV = r.Cells["MaNV"].Value;
                if (maNV != null && maNV != DBNull.Value)
                    cboNV.SelectedValue = Convert.ToInt32(maNV);

                if (r.Cells["NgayNhap"].Value != DBNull.Value)
                    dtpNgay.Value = Convert.ToDateTime(r.Cells["NgayNhap"].Value);
            };
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
                cboNCC.DataSource    = bus.GetNhaCungCap();
                cboNCC.DisplayMember = "TenNCC";
                cboNCC.ValueMember   = "MaNCC";
                cboNCC.SelectedIndex = -1;

                cboNV.DataSource    = bus.GetNhanVien();
                cboNV.DisplayMember = "HoTen";
                cboNV.ValueMember   = "MaNV";
                cboNV.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void Them(object? s, EventArgs e)
        {
            if (cboNCC.SelectedValue == null || cboNCC.SelectedValue is DataRowView)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cboNV.SelectedValue == null || cboNV.SelectedValue is DataRowView)
            {
                MessageBox.Show("Vui lòng chọn nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bus.Them(Convert.ToInt32(cboNCC.SelectedValue), Convert.ToInt32(cboNV.SelectedValue), dtpNgay.Value.Date);
                MessageBox.Show("Thêm phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
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
            if (!int.TryParse(txtMaPN.Text, out int ma))
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập cần sửa từ bảng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboNCC.SelectedValue == null || cboNCC.SelectedValue is DataRowView)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cboNV.SelectedValue == null || cboNV.SelectedValue is DataRowView)
            {
                MessageBox.Show("Vui lòng chọn nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bus.Sua(ma, Convert.ToInt32(cboNCC.SelectedValue), Convert.ToInt32(cboNV.SelectedValue), dtpNgay.Value.Date);
                MessageBox.Show("Sửa phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
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
            if (!int.TryParse(txtMaPN.Text, out int ma))
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập cần xóa từ bảng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Xóa phiếu nhập này? Các chi tiết nhập hàng liên quan cũng sẽ bị xóa.", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    bus.Xoa(ma);
                    MessageBox.Show("Xóa phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txtMaPN.Clear();
            txtTimKiem.Clear();
            cboNCC.SelectedIndex = -1;
            cboNV.SelectedIndex  = -1;
            dtpNgay.Value = DateTime.Today;
            LoadData();
        }
    }
}
