
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKhachSan.BUS;

namespace QuanLyKhachSan.Forms
{
    public class FrmKhachHang : Form
    {
        private readonly KhachHangBUS bus = new KhachHangBUS();

        private TextBox txtMaKH = new TextBox();
        private TextBox txtHoTen = new TextBox();
        private TextBox txtCCCD = new TextBox();
        private TextBox txtSDT = new TextBox();
        private TextBox txtEmail = new TextBox();
        private TextBox txtDiaChi = new TextBox();
        private TextBox txtTimKiem = new TextBox();

        private DataGridView dgvKhachHang = new DataGridView();

        public FrmKhachHang()
        {
            InitializeForm();
            LoadData();
        }

        private void InitializeForm()
        {
            Text = "QUẢN LÝ KHÁCH HÀNG";
            Size = new Size(1050, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ KHÁCH HÀNG",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Location = new Point(320, 20),
                Size = new Size(450, 40)
            };
            Controls.Add(lblTitle);

            AddLabel("Mã khách hàng:", 30, 90);
            AddLabel("Họ tên:", 30, 135);
            AddLabel("CCCD:", 30, 180);
            AddLabel("SDT:", 530, 90);
            AddLabel("Email:", 530, 135);
            AddLabel("Địa chỉ:", 530, 180);

            AddTextBox(txtMaKH, 160, 88, 300);
            AddTextBox(txtHoTen, 160, 133, 300);
            AddTextBox(txtCCCD, 160, 178, 300);
            AddTextBox(txtSDT, 660, 88, 300);
            AddTextBox(txtEmail, 660, 133, 300);
            AddTextBox(txtDiaChi, 660, 178, 300);

            AddButton("Thêm", 30, 235, ThemKhachHang);
            AddButton("Sửa", 140, 235, SuaKhachHang);
            AddButton("Xóa", 250, 235, XoaKhachHang);
            AddButton("Làm mới", 360, 235, LamMoi);

            Label lblSearch = new Label
            {
                Text = "Tìm theo tên hoặc SĐT:",
                Location = new Point(530, 245),
                Size = new Size(160, 25)
            };
            Controls.Add(lblSearch);

            txtTimKiem.Location = new Point(690, 240);
            txtTimKiem.Size = new Size(180, 28);
            Controls.Add(txtTimKiem);

            Button btnTimKiem = new Button
            {
                Text = "Tìm kiếm",
                Location = new Point(880, 238),
                Size = new Size(90, 32)
            };
            btnTimKiem.Click += (s, e) => TimKiemKhachHang();
            Controls.Add(btnTimKiem);

            dgvKhachHang.Location = new Point(30, 300);
            dgvKhachHang.Size = new Size(940, 320);
            dgvKhachHang.ReadOnly = true;
            dgvKhachHang.AllowUserToAddRows = false;
            dgvKhachHang.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvKhachHang.MultiSelect = false;
            dgvKhachHang.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvKhachHang.RowHeadersVisible = false;
            dgvKhachHang.CellClick += DgvKhachHang_CellClick;

            Controls.Add(dgvKhachHang);
        }

        private void AddLabel(string text, int x, int y)
        {
            Label lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(125, 25),
                Font = new Font("Arial", 10)
            };

            Controls.Add(lbl);
        }

        private void AddTextBox(TextBox txt, int x, int y, int width)
        {
            txt.Location = new Point(x, y);
            txt.Size = new Size(width, 28);
            txt.Font = new Font("Arial", 10);

            Controls.Add(txt);
        }

        private void AddButton(
            string text, int x, int y, EventHandler action)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(100, 35),
                BackColor = Color.LightSteelBlue,
                Font = new Font("Arial", 10)
            };

            btn.Click += action;
            Controls.Add(btn);
        }

        // Hiển thị danh sách khách hàng
        private void LoadData()
        {
            try
            {
                dgvKhachHang.DataSource = bus.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        // Kiểm tra dữ liệu nhập
        private bool KiemTraDuLieu()
        {
            if (!int.TryParse(txtMaKH.Text.Trim(), out _))
            {
                MessageBox.Show("Mã khách hàng phải là số nguyên!");
                txtMaKH.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!");
                txtHoTen.Focus();
                return false;
            }

            return true;
        }

        // Thêm khách hàng
        private void ThemKhachHang(object? sender, EventArgs e)
        {
            if (!KiemTraDuLieu()) return;

            try
            {
                bus.ThemKhachHang(
                    int.Parse(txtMaKH.Text.Trim()),
                    txtHoTen.Text.Trim(),
                    txtCCCD.Text.Trim(),
                    txtSDT.Text.Trim(),
                    txtEmail.Text.Trim(),
                    txtDiaChi.Text.Trim()
                );

                MessageBox.Show("Thêm khách hàng thành công!");
                LoadData();
                LamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể thêm khách hàng: " + ex.Message);
            }
        }

        // Sửa khách hàng
        private void SuaKhachHang(object? sender, EventArgs e)
        {
            if (!KiemTraDuLieu()) return;

            try
            {
                bus.SuaKhachHang(
                    int.Parse(txtMaKH.Text.Trim()),
                    txtHoTen.Text.Trim(),
                    txtCCCD.Text.Trim(),
                    txtSDT.Text.Trim(),
                    txtEmail.Text.Trim(),
                    txtDiaChi.Text.Trim()
                );

                MessageBox.Show("Sửa khách hàng thành công!");
                LoadData();
                LamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể sửa khách hàng: " + ex.Message);
            }
        }

        // Xóa khách hàng
        private void XoaKhachHang(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaKH.Text.Trim(), out int maKH))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa khách hàng này?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes) return;

            try
            {
                bus.XoaKhachHang(maKH);

                MessageBox.Show("Xóa khách hàng thành công!");
                LoadData();
                LamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa khách hàng: " + ex.Message);
            }
        }

        // Tìm kiếm khách hàng
        private void TimKiemKhachHang()
        {
            try
            {
                dgvKhachHang.DataSource =
                    bus.TimKiemKhachHang(txtTimKiem.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }

        // Làm mới dữ liệu
        private void LamMoi(object? sender = null, EventArgs? e = null)
        {
            txtMaKH.Clear();
            txtHoTen.Clear();
            txtCCCD.Clear();
            txtSDT.Clear();
            txtEmail.Clear();
            txtDiaChi.Clear();
            txtTimKiem.Clear();

            LoadData();
        }

        // Chọn một dòng trong bảng để sửa hoặc xóa
        private void DgvKhachHang_CellClick(
            object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvKhachHang.Rows[e.RowIndex];

            txtMaKH.Text = row.Cells["MaKH"].Value?.ToString() ?? "";
            txtHoTen.Text = row.Cells["HoTen"].Value?.ToString() ?? "";
            txtCCCD.Text = row.Cells["CCCD"].Value?.ToString() ?? "";
            txtSDT.Text = row.Cells["SDT"].Value?.ToString() ?? "";
            txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";
            txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString() ?? "";
        }
    }
}
