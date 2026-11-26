
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKhachSan.BUS;

namespace QuanLyKhachSan.Forms
{
    public class FrmDatPhong : Form
    {
        private readonly DatPhongBUS bus = new DatPhongBUS();

        private TextBox txtMaDP = new TextBox();
        private ComboBox cboKhachHang = new ComboBox();
        private ComboBox cboNhanVien = new ComboBox();
        private DateTimePicker dtpNgayDat = new DateTimePicker();
        private DateTimePicker dtpNgayNhan = new DateTimePicker();
        private DateTimePicker dtpNgayTra = new DateTimePicker();
        private ComboBox cboTrangThai = new ComboBox();
        private TextBox txtTimKiem = new TextBox();

        private DataGridView dgvDatPhong = new DataGridView();

        public FrmDatPhong()
        {
            InitializeForm();
            LoadComboBox();
            LoadData();
        }

        private void InitializeForm()
        {
            Text = "QUẢN LÝ ĐẶT PHÒNG";
            Size = new Size(1100, 750);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ ĐẶT PHÒNG",
                Font = new Font("Arial", 21, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Location = new Point(340, 20),
                Size = new Size(450, 45)
            };
            Controls.Add(lblTitle);

            // Các nhãn
            AddLabel("Mã đặt phòng:", 30, 95);
            AddLabel("Khách hàng:", 30, 140);
            AddLabel("Nhân viên:", 30, 185);

            AddLabel("Ngày đặt:", 550, 95);
            AddLabel("Ngày nhận:", 550, 140);
            AddLabel("Ngày trả:", 550, 185);
            AddLabel("Trạng thái:", 550, 230);

            // Các ô nhập
            AddTextBox(txtMaDP, 160, 92, 300);

            AddComboBox(cboKhachHang, 160, 137, 300);
            AddComboBox(cboNhanVien, 160, 182, 300);

            AddDatePicker(dtpNgayDat, 680, 92);
            AddDatePicker(dtpNgayNhan, 680, 137);
            AddDatePicker(dtpNgayTra, 680, 182);

            AddComboBox(cboTrangThai, 680, 227, 300);

            cboTrangThai.Items.AddRange(new object[]
            {
                "Chờ xác nhận",
                "Đã xác nhận",
                "Đang sử dụng",
                "Đã hoàn thành",
                "Đã hủy"
            });

            cboTrangThai.SelectedIndex = 0;

            // Các nút chức năng
            AddButton("Thêm", 30, 285, ThemDatPhong);
            AddButton("Sửa", 140, 285, SuaDatPhong);
            AddButton("Xóa", 250, 285, XoaDatPhong);
            AddButton("Làm mới", 360, 285, LamMoi);

            // Tìm kiếm
            Label lblSearch = new Label
            {
                Text = "Tìm mã đặt phòng:",
                Location = new Point(550, 293),
                Size = new Size(130, 25)
            };
            Controls.Add(lblSearch);

            txtTimKiem.Location = new Point(680, 290);
            txtTimKiem.Size = new Size(190, 28);
            Controls.Add(txtTimKiem);

            Button btnTimKiem = new Button
            {
                Text = "Tìm kiếm",
                Location = new Point(880, 287),
                Size = new Size(100, 35)
            };

            btnTimKiem.Click += (s, e) => TimKiemDatPhong();
            Controls.Add(btnTimKiem);

            // Bảng dữ liệu
            dgvDatPhong.Location = new Point(25, 350);
            dgvDatPhong.Size = new Size(1030, 320);
            dgvDatPhong.ReadOnly = true;
            dgvDatPhong.AllowUserToAddRows = false;
            dgvDatPhong.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvDatPhong.MultiSelect = false;
            dgvDatPhong.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvDatPhong.RowHeadersVisible = false;

            dgvDatPhong.CellClick += DgvDatPhong_CellClick;

            Controls.Add(dgvDatPhong);
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

        private void AddComboBox(ComboBox cbo, int x, int y, int width)
        {
            cbo.Location = new Point(x, y);
            cbo.Size = new Size(width, 28);
            cbo.Font = new Font("Arial", 10);
            cbo.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(cbo);
        }

        private void AddDatePicker(DateTimePicker dtp, int x, int y)
        {
            dtp.Location = new Point(x, y);
            dtp.Size = new Size(300, 28);
            dtp.Format = DateTimePickerFormat.Short;
            Controls.Add(dtp);
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

        // Nạp khách hàng và nhân viên vào ComboBox
        private void LoadComboBox()
        {
            try
            {
                cboKhachHang.DataSource = bus.GetKhachHang();
                cboKhachHang.DisplayMember = "HoTen";
                cboKhachHang.ValueMember = "MaKH";
                cboKhachHang.SelectedIndex = -1;

                cboNhanVien.DataSource = bus.GetNhanVien();
                cboNhanVien.DisplayMember = "HoTen";
                cboNhanVien.ValueMember = "MaNV";
                cboNhanVien.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải khách hàng hoặc nhân viên: " + ex.Message);
            }
        }

        // Hiển thị danh sách đặt phòng
        private void LoadData()
        {
            try
            {
                dgvDatPhong.DataSource = bus.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải đặt phòng: " + ex.Message);
            }
        }

        // Kiểm tra dữ liệu
        private bool KiemTraDuLieu()
        {
            if (!int.TryParse(txtMaDP.Text.Trim(), out _))
            {
                MessageBox.Show("Mã đặt phòng phải là số nguyên!");
                txtMaDP.Focus();
                return false;
            }

            if (cboKhachHang.SelectedValue == null ||
                cboKhachHang.SelectedValue is DataRowView)
            {
                MessageBox.Show("Vui lòng chọn khách hàng!");
                return false;
            }

            if (cboNhanVien.SelectedValue == null ||
                cboNhanVien.SelectedValue is DataRowView)
            {
                MessageBox.Show("Vui lòng chọn nhân viên!");
                return false;
            }

            if (dtpNgayTra.Value.Date <= dtpNgayNhan.Value.Date)
            {
                MessageBox.Show("Ngày trả phải sau ngày nhận!");
                return false;
            }

            if (dtpNgayNhan.Value.Date < dtpNgayDat.Value.Date)
            {
                MessageBox.Show("Ngày nhận không được trước ngày đặt!");
                return false;
            }

            if (cboTrangThai.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn trạng thái!");
                return false;
            }

            return true;
        }

        // Thêm đặt phòng
        private void ThemDatPhong(object? sender, EventArgs e)
        {
            if (!KiemTraDuLieu()) return;

            try
            {
                bus.ThemDatPhong(
                    int.Parse(txtMaDP.Text.Trim()),
                    Convert.ToInt32(cboKhachHang.SelectedValue),
                    Convert.ToInt32(cboNhanVien.SelectedValue),
                    dtpNgayDat.Value.Date,
                    dtpNgayNhan.Value.Date,
                    dtpNgayTra.Value.Date,
                    cboTrangThai.Text
                );

                MessageBox.Show("Thêm đặt phòng thành công!");

                LoadData();
                LamMoi(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể thêm đặt phòng: " + ex.Message);
            }
        }

        // Sửa đặt phòng
        private void SuaDatPhong(object? sender, EventArgs e)
        {
            if (!KiemTraDuLieu()) return;

            try
            {
                bus.SuaDatPhong(
                    int.Parse(txtMaDP.Text.Trim()),
                    Convert.ToInt32(cboKhachHang.SelectedValue),
                    Convert.ToInt32(cboNhanVien.SelectedValue),
                    dtpNgayDat.Value.Date,
                    dtpNgayNhan.Value.Date,
                    dtpNgayTra.Value.Date,
                    cboTrangThai.Text
                );

                MessageBox.Show("Sửa đặt phòng thành công!");

                LoadData();
                LamMoi(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể sửa đặt phòng: " + ex.Message);
            }
        }

        // Xóa đặt phòng
        private void XoaDatPhong(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaDP.Text.Trim(), out int maDP))
            {
                MessageBox.Show("Vui lòng chọn phiếu đặt phòng cần xóa!");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa phiếu đặt phòng này?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes) return;

            try
            {
                bus.XoaDatPhong(maDP);

                MessageBox.Show("Xóa đặt phòng thành công!");

                LoadData();
                LamMoi(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa đặt phòng: " + ex.Message);
            }
        }

        // Tìm kiếm đặt phòng
        private void TimKiemDatPhong()
        {
            try
            {
                dgvDatPhong.DataSource =
                    bus.TimKiemDatPhong(txtTimKiem.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }

        // Làm mới
        private void LamMoi(object? sender, EventArgs e)
        {
            txtMaDP.Clear();

            cboKhachHang.SelectedIndex = -1;
            cboNhanVien.SelectedIndex = -1;

            dtpNgayDat.Value = DateTime.Today;
            dtpNgayNhan.Value = DateTime.Today;
            dtpNgayTra.Value = DateTime.Today.AddDays(1);

            cboTrangThai.SelectedIndex = 0;

            txtTimKiem.Clear();

            LoadData();
        }

        // Chọn dòng trong bảng
        private void DgvDatPhong_CellClick(
            object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvDatPhong.Rows[e.RowIndex];

            txtMaDP.Text =
                row.Cells["MaDP"].Value?.ToString() ?? "";

           object? maKH = row.Cells["MaKH"].Value;
object? maNV = row.Cells["MaNV"].Value;

if (maKH != null && maKH != DBNull.Value)
{
    cboKhachHang.SelectedValue = Convert.ToInt32(maKH);
}

if (maNV != null && maNV != DBNull.Value)
{
    cboNhanVien.SelectedValue = Convert.ToInt32(maNV);
}

            if (row.Cells["NgayDat"].Value != DBNull.Value)
                dtpNgayDat.Value =
                    Convert.ToDateTime(row.Cells["NgayDat"].Value);

            if (row.Cells["NgayNhan"].Value != DBNull.Value)
                dtpNgayNhan.Value =
                    Convert.ToDateTime(row.Cells["NgayNhan"].Value);

            if (row.Cells["NgayTra"].Value != DBNull.Value)
                dtpNgayTra.Value =
                    Convert.ToDateTime(row.Cells["NgayTra"].Value);

            cboTrangThai.Text =
                row.Cells["TrangThai"].Value?.ToString() ?? "";
        }
    }
}
