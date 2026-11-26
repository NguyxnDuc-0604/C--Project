using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKhachSan.BUS;

namespace QuanLyKhachSan.Forms
{
    public class FrmNhaCungCap : Form
    {
        private readonly NhaCungCapBUS bus = new();

        private TextBox txtMaNCC   = new();
        private TextBox txtTenNCC  = new();
        private TextBox txtSDT     = new();
        private TextBox txtDiaChi  = new();
        private TextBox txtEmail   = new();
        private TextBox txtTimKiem = new();

        private DataGridView dgv = new();

        public FrmNhaCungCap()
        {
            Text = "QUẢN LÝ NHÀ CUNG CẤP";
            Size = new Size(1100, 680);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            Build();
            LoadData();
        }

        private void Build()
        {
            Controls.Add(new Label
            {
                Text = "QUẢN LÝ NHÀ CUNG CẤP",
                Font = new Font("Arial", 19, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Location = new Point(330, 20),
                AutoSize = true
            });

            Field("Mã NCC:",    txtMaNCC,  30,  85);
            txtMaNCC.ReadOnly = true;
            txtMaNCC.BackColor = Color.LightGray;

            Field("Tên NCC:",   txtTenNCC, 30,  130);
            Field("Số ĐT:",     txtSDT,    30,  175);
            Field("Email:",     txtEmail,  560, 85);
            Field("Địa chỉ:",   txtDiaChi, 560, 130);

            Btn("Thêm",    30,  225, Them);
            Btn("Sửa",     140, 225, Sua);
            Btn("Xóa",     250, 225, Xoa);
            Btn("Làm mới", 360, 225, Reset);

            Controls.Add(new Label { Text = "Tìm kiếm:", Location = new Point(560, 235), AutoSize = true });
            txtTimKiem.SetBounds(650, 230, 200, 28);
            Controls.Add(txtTimKiem);
            Btn("Tìm", 860, 228, Search);

            Grid(dgv, 30, 280, 1020, 330);
            dgv.CellClick += (_, e) =>
            {
                if (e.RowIndex < 0) return;
                var r = dgv.Rows[e.RowIndex];
                txtMaNCC.Text  = V(r, "MaNCC");
                txtTenNCC.Text = V(r, "TenNCC");
                txtSDT.Text    = V(r, "SDT");
                txtDiaChi.Text = V(r, "DiaChi");
                txtEmail.Text  = V(r, "Email");
            };
        }

        private void Field(string label, Control c, int x, int y)
        {
            Controls.Add(new Label { Text = label, Location = new Point(x, y + 4), Size = new Size(100, 24) });
            c.SetBounds(x + 105, y, 300, 28);
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

        private void LoadData()
        {
            try
            {
                dgv.DataSource = bus.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Them(object? s, EventArgs e)
        {
            try
            {
                bus.Them(txtTenNCC.Text, txtSDT.Text, txtDiaChi.Text, txtEmail.Text);
                MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (!int.TryParse(txtMaNCC.Text, out int ma))
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp cần sửa từ bảng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bus.Sua(ma, txtTenNCC.Text, txtSDT.Text, txtDiaChi.Text, txtEmail.Text);
                MessageBox.Show("Sửa thông tin nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (!int.TryParse(txtMaNCC.Text, out int ma))
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp cần xóa từ bảng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa nhà cung cấp này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    bus.Xoa(ma);
                    MessageBox.Show("Xóa nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txtMaNCC.Clear();
            txtTenNCC.Clear();
            txtSDT.Clear();
            txtDiaChi.Clear();
            txtEmail.Clear();
            txtTimKiem.Clear();
            LoadData();
        }
    }
}
