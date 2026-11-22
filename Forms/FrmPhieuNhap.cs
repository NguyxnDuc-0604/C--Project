using System.Data;
using System.Drawing;
using QuanLyKhachSan.DAL;

namespace QuanLyKhachSan.Forms
{
    public class FrmPhieuNhap : Form
    {
        private readonly PhieuNhapDAL dal = new();

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
            // Tiêu đề
            Controls.Add(new Label
            {
                Text = "QUẢN LÝ PHIẾU NHẬP",
                Font = new Font("Arial", 19, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Location = new Point(380, 20),
                AutoSize = true
            });

            // Mã phiếu (auto, chỉ đọc)
            Field("Mã phiếu:", txtMaPN, 30, 85);
            txtMaPN.ReadOnly = true;
            txtMaPN.BackColor = Color.LightGray;

            // ComboBox Nhà Cung Cấp — hiển thị TenNCC thay vì MaNCC
            Field("Nhà cung cấp:", cboNCC, 30, 130);
            cboNCC.DropDownStyle = ComboBoxStyle.DropDownList;

            // ComboBox Nhân Viên — hiển thị HoTen thay vì MaNV
            Field("Nhân viên:", cboNV, 30, 175);
            cboNV.DropDownStyle = ComboBoxStyle.DropDownList;

            // DateTimePicker Ngày Nhập
            Controls.Add(new Label { Text = "Ngày nhập:", Location = new Point(600, 89), Size = new Size(110, 24) });
            dtpNgay.SetBounds(715, 85, 280, 28);
            dtpNgay.Format = DateTimePickerFormat.Short;
            Controls.Add(dtpNgay);

            // Nút chức năng
            Btn("Thêm",    30,  225, Them);
            Btn("Sửa",     140, 225, Sua);
            Btn("Xóa",     250, 225, Xoa);
            Btn("Làm mới", 360, 225, Reset);

            // Tìm kiếm
            Controls.Add(new Label { Text = "Tìm kiếm:", Location = new Point(600, 235), AutoSize = true });
            txtTimKiem.SetBounds(690, 230, 200, 28);
            Controls.Add(txtTimKiem);
            Btn("Tìm", 900, 228, Search);

            // DataGridView
            Grid(dgv, 30, 280, 1020, 340);
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
                // ComboBox Nhà Cung Cấp: DisplayMember = TenNCC, ValueMember = MaNCC
                cboNCC.DataSource    = dal.GetNhaCungCap();
                cboNCC.DisplayMember = "TenNCC";
                cboNCC.ValueMember   = "MaNCC";
                cboNCC.SelectedIndex = -1;

                // ComboBox Nhân Viên: DisplayMember = HoTen, ValueMember = MaNV
                cboNV.DataSource    = dal.GetNhanVien();
                cboNV.DisplayMember = "HoTen";
                cboNV.ValueMember   = "MaNV";
                cboNV.SelectedIndex = -1;
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải ComboBox: " + ex.Message); }
        }

        private void LoadData()
        {
            try { dgv.DataSource = dal.GetAll(); }
            catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message); }
        }

        private bool Valid()
        {
            if (cboNCC.SelectedValue == null || cboNCC.SelectedValue is DataRowView)
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp!");
                return false;
            }
            if (cboNV.SelectedValue == null || cboNV.SelectedValue is DataRowView)
            {
                MessageBox.Show("Vui lòng chọn nhân viên!");
                return false;
            }
            return true;
        }

        private void Them(object? s, EventArgs e)
        {
            if (!Valid()) return;
            Run(() => dal.Them(
                Convert.ToInt32(cboNCC.SelectedValue),
                Convert.ToInt32(cboNV.SelectedValue),
                dtpNgay.Value.Date
            ), "Thêm phiếu nhập");
        }

        private void Sua(object? s, EventArgs e)
        {
            if (!int.TryParse(txtMaPN.Text, out int ma)) { MessageBox.Show("Vui lòng chọn phiếu nhập cần sửa!"); return; }
            if (!Valid()) return;
            Run(() => dal.Sua(
                ma,
                Convert.ToInt32(cboNCC.SelectedValue),
                Convert.ToInt32(cboNV.SelectedValue),
                dtpNgay.Value.Date
            ), "Sửa phiếu nhập");
        }

        private void Xoa(object? s, EventArgs e)
        {
            if (!int.TryParse(txtMaPN.Text, out int ma)) { MessageBox.Show("Vui lòng chọn phiếu nhập cần xóa!"); return; }
            if (MessageBox.Show("Xóa phiếu nhập này? Các chi tiết liên quan cũng sẽ bị xóa.", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                Run(() => dal.Xoa(ma), "Xóa phiếu nhập");
        }

        private void Search(object? s, EventArgs e)
        {
            try { dgv.DataSource = dal.TimKiem(txtTimKiem.Text.Trim()); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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

        private void Run(Action action, string op)
        {
            try { action(); LoadData(); MessageBox.Show(op + " thành công."); Reset(null!, EventArgs.Empty); }
            catch (Exception ex) { MessageBox.Show(op + " thất bại: " + ex.Message); }
        }
    }
}
