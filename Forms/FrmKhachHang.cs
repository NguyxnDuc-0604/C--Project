using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKhachSan.BUS;

namespace QuanLyKhachSan.Forms
{
    public class FrmKhachHang : Form
    {
        private readonly KhachHangBUS _bus = new();
        private readonly TextBox _maKh = CreateTextBox();
        private readonly TextBox _hoTen = CreateTextBox();
        private readonly TextBox _cccd = CreateTextBox();
        private readonly TextBox _sdt = CreateTextBox();
        private readonly TextBox _email = CreateTextBox();
        private readonly TextBox _diaChi = CreateTextBox();
        private readonly TextBox _timKiem = CreateTextBox();
        private readonly DataGridView _grid = new();

        public FrmKhachHang()
        {
            Text = "QUẢN LÝ KHÁCH HÀNG";
            BackColor = Color.WhiteSmoke;
            Font = new Font("Segoe UI", 9.5F);
            Dock = DockStyle.Fill;

            BuildLayout();
            LoadData();
        }

        private void BuildLayout()
        {
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(8),
                BackColor = Color.WhiteSmoke
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 57));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 36));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
            Controls.Add(layout);

            layout.Controls.Add(BuildSearchBar(), 0, 0);
            layout.Controls.Add(BuildGrid(), 0, 1);
            layout.Controls.Add(BuildDetails(), 0, 2);
            layout.Controls.Add(BuildActions(), 0, 3);
        }

        private Control BuildSearchBar()
        {
            var bar = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(2, 7, 2, 2),
                BackColor = Color.WhiteSmoke
            };
            bar.Controls.Add(new Label
            {
                Text = "Tìm kiếm:",
                AutoSize = true,
                Margin = new Padding(2, 7, 8, 0)
            });
            _timKiem.Width = 250;
            _timKiem.Margin = new Padding(0, 1, 8, 0);
            _timKiem.KeyDown += (_, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    TimKiem();
                }
            };
            bar.Controls.Add(_timKiem);
            bar.Controls.Add(CreateButton("Tìm theo tên / SĐT", (_, _) => TimKiem(), 150));
            return bar;
        }

        private Control BuildGrid()
        {
            _grid.Dock = DockStyle.Fill;
            _grid.ReadOnly = true;
            _grid.AllowUserToAddRows = false;
            _grid.AllowUserToDeleteRows = false;
            _grid.MultiSelect = false;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _grid.RowHeadersVisible = false;
            _grid.BackgroundColor = Color.White;
            _grid.BorderStyle = BorderStyle.FixedSingle;
            _grid.ColumnHeadersHeight = 34;
            _grid.RowTemplate.Height = 28;
            _grid.EnableHeadersVisualStyles = false;
            _grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(226, 233, 242);
            _grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            _grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(193, 214, 239);
            _grid.DefaultCellStyle.SelectionForeColor = Color.Black;
            _grid.CellClick += GridCellClick;
            return _grid;
        }

        private Control BuildDetails()
        {
            var group = new GroupBox
            {
                Text = "Thông tin khách hàng",
                Dock = DockStyle.Fill,
                Padding = new Padding(8),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular)
            };

            var fields = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 3,
                Padding = new Padding(4)
            };
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12));
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12));
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
            for (int row = 0; row < 3; row++)
                fields.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));

            AddField(fields, "Mã khách hàng:", _maKh, 0, 0);
            AddField(fields, "Họ tên:", _hoTen, 1, 0);
            AddField(fields, "CCCD:", _cccd, 0, 1);
            AddField(fields, "SĐT:", _sdt, 1, 1);
            AddField(fields, "Email:", _email, 0, 2);
            AddField(fields, "Địa chỉ:", _diaChi, 1, 2);

            group.Controls.Add(fields);
            return group;
        }

        private Control BuildActions()
        {
            var row = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1
            };
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            var buttons = new FlowLayoutPanel
            {
                AutoSize = true,
                Anchor = AnchorStyles.None,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };
            buttons.Controls.Add(CreateButton("Thêm mới", ThemKhachHang));
            buttons.Controls.Add(CreateButton("Sửa thông tin", SuaKhachHang, 125));
            buttons.Controls.Add(CreateButton("Xóa chọn", XoaKhachHang));
            buttons.Controls.Add(CreateButton("Làm mới / Hiện tất cả", LamMoi, 190));
            row.Controls.Add(buttons, 1, 0);
            return row;
        }

        private static void AddField(TableLayoutPanel panel, string label, Control input, int pair, int row)
        {
            int labelColumn = pair == 0 ? 0 : 2;
            int inputColumn = labelColumn + 1;
            panel.Controls.Add(new Label
            {
                Text = label,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                Margin = new Padding(3)
            }, labelColumn, row);
            input.Dock = DockStyle.Fill;
            input.Margin = new Padding(3, 6, 12, 6);
            panel.Controls.Add(input, inputColumn, row);
        }

        private static TextBox CreateTextBox() => new()
        {
            Font = new Font("Segoe UI", 9.5F),
            Margin = new Padding(3)
        };

        private static Button CreateButton(string text, EventHandler onClick, int width = 105)
        {
            var button = new Button
            {
                Text = text,
                Width = width,
                Height = 32,
                Margin = new Padding(4, 2, 4, 2),
                BackColor = Color.FromArgb(218, 229, 243),
                FlatStyle = FlatStyle.Standard
            };
            button.Click += onClick;
            return button;
        }

        private void LoadData()
        {
            try
            {
                _grid.DataSource = _bus.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu khách hàng: " + ex.Message,
                    "Quản lý khách hàng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput(out int maKh)
        {
            if (!int.TryParse(_maKh.Text.Trim(), out maKh) || maKh <= 0)
            {
                MessageBox.Show("Mã khách hàng phải là số nguyên dương.", "Dữ liệu chưa hợp lệ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _maKh.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(_hoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên khách hàng.", "Dữ liệu chưa hợp lệ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _hoTen.Focus();
                return false;
            }

            return true;
        }

        private void ThemKhachHang(object? sender, EventArgs e)
        {
            if (!ValidateInput(out int maKh)) return;
            RunOperation(() => _bus.ThemKhachHang(maKh, _hoTen.Text.Trim(), _cccd.Text.Trim(),
                _sdt.Text.Trim(), _email.Text.Trim(), _diaChi.Text.Trim()), "Thêm khách hàng");
        }

        private void SuaKhachHang(object? sender, EventArgs e)
        {
            if (!ValidateInput(out int maKh)) return;
            RunOperation(() => _bus.SuaKhachHang(maKh, _hoTen.Text.Trim(), _cccd.Text.Trim(),
                _sdt.Text.Trim(), _email.Text.Trim(), _diaChi.Text.Trim()), "Cập nhật khách hàng");
        }

        private void XoaKhachHang(object? sender, EventArgs e)
        {
            if (!int.TryParse(_maKh.Text.Trim(), out int maKh) || maKh <= 0)
            {
                MessageBox.Show("Chọn khách hàng trong bảng trước khi xóa.", "Chưa chọn khách hàng",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            RunOperation(() => _bus.XoaKhachHang(maKh), "Xóa khách hàng");
        }

        private void TimKiem()
        {
            try
            {
                _grid.DataSource = _bus.TimKiemKhachHang(_timKiem.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm khách hàng: " + ex.Message,
                    "Quản lý khách hàng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LamMoi(object? sender, EventArgs e)
        {
            _maKh.Clear();
            _hoTen.Clear();
            _cccd.Clear();
            _sdt.Clear();
            _email.Clear();
            _diaChi.Clear();
            _timKiem.Clear();
            LoadData();
        }

        private void RunOperation(Action action, string title)
        {
            try
            {
                action();
                LoadData();
                LamMoi(null, EventArgs.Empty);
                MessageBox.Show(title + " thành công.", "Hoàn tất",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(title + " thất bại: " + ex.Message, "Không thể thực hiện",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GridCellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = _grid.Rows[e.RowIndex];
            _maKh.Text = Cell(row, "MaKH");
            _hoTen.Text = Cell(row, "HoTen");
            _cccd.Text = Cell(row, "CCCD");
            _sdt.Text = Cell(row, "SDT");
            _email.Text = Cell(row, "Email");
            _diaChi.Text = Cell(row, "DiaChi");
        }

        private static string Cell(DataGridViewRow row, string column) =>
            row.Cells[column].Value?.ToString() ?? string.Empty;
    }
}
