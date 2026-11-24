using System.Data;
using System.Drawing;
using QuanLyKhachSan.DAL;

namespace QuanLyKhachSan.Forms
{
    public class FrmNhaCungCap : Form
    {
        private readonly NhaCungCapDAL dal = new();

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
            // Tiêu đề
            Controls.Add(new Label
            {
                Text = "QUẢN LÝ NHÀ CUNG CẤP",
                Font = new Font("Arial", 19, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Location = new Point(330, 20),
                AutoSize = true
            });

            // Các trường nhập liệu
            Field("Mã NCC:",    txtMaNCC,  30,  85); txtMaNCC.ReadOnly = true; txtMaNCC.BackColor = Color.LightGray;
            Field("Tên NCC:",   txtTenNCC, 30,  130);
            Field("Số ĐT:",     txtSDT,    30,  175);
            Field("Email:",     txtEmail,  560, 85);
            Field("Địa chỉ:",   txtDiaChi, 560, 130);

            // Nút chức năng
            Btn("Thêm",    30,  225, Them);
            Btn("Sửa",     140, 225, Sua);
            Btn("Xóa",     250, 225, Xoa);
            Btn("Làm mới", 360, 225, Reset);

            // Tìm kiếm
            Controls.Add(new Label { Text = "Tìm kiếm:", Location = new Point(560, 235), AutoSize = true });
            txtTimKiem.SetBounds(650, 230, 200, 28);
            Controls.Add(txtTimKiem);
            Btn("Tìm", 860, 228, Search);

            // DataGridView
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
            try { dgv.DataSource = dal.GetAll(); }
            catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message); }
        }

        private bool Valid()
        {
            if (string.IsNullOrWhiteSpace(txtTenNCC.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhà cung cấp!");
                txtTenNCC.Focus();
                return false;
            }
            return true;
        }

        private void Them(object? s, EventArgs e)
        {
            if (!Valid()) return;
            Run(() => dal.Them(txtTenNCC.Text.Trim(), txtSDT.Text.Trim(), txtDiaChi.Text.Trim(), txtEmail.Text.Trim()), "Thêm nhà cung cấp");
        }

        private void Sua(object? s, EventArgs e)
        {
            if (!int.TryParse(txtMaNCC.Text, out int ma))
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp cần sửa!");
                return;
            }
            if (!Valid()) return;
            Run(() => dal.Sua(ma, txtTenNCC.Text.Trim(), txtSDT.Text.Trim(), txtDiaChi.Text.Trim(), txtEmail.Text.Trim()), "Sửa nhà cung cấp");
        }

        private void Xoa(object? s, EventArgs e)
        {
            if (!int.TryParse(txtMaNCC.Text, out int ma))
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp cần xóa!");
                return;
            }
            if (MessageBox.Show("Xóa nhà cung cấp này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                Run(() => dal.Xoa(ma), "Xóa nhà cung cấp");
        }

        private void Search(object? s, EventArgs e)
        {
            try { dgv.DataSource = dal.TimKiem(txtTimKiem.Text.Trim()); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Reset(object? s, EventArgs e)
        {
            txtMaNCC.Clear(); txtTenNCC.Clear(); txtSDT.Clear();
            txtDiaChi.Clear(); txtEmail.Clear(); txtTimKiem.Clear();
            LoadData();
        }

        private void Run(Action action, string op)
        {
            try { action(); LoadData(); MessageBox.Show(op + " thành công."); Reset(null!, EventArgs.Empty); }
            catch (Exception ex) { MessageBox.Show(op + " thất bại: " + ex.Message); }
        }
    }
}
