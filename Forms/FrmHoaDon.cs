
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.Forms
{
    public class FrmHoaDon : Form
    {
        private readonly HoaDonDAL dal = new();

        private TextBox txtMaHD = new();
        private ComboBox cboDatPhong = new();
        private ComboBox cboNhanVien = new();
        private DateTimePicker dtpNgayLap = new();
        private TextBox txtTongTien = new();
        private TextBox txtGhiChu = new();
        private TextBox txtTimKiem = new();

        private DataGridView dgv = new();

        public FrmHoaDon()
        {
            Text = "QUẢN LÝ HÓA ĐƠN";
            Size = new Size(1100, 750);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            TaoGiaoDien();

            Load += (s, e) =>
            {
                LoadComboBox();
                LoadData();
            };
        }

        private void TaoGiaoDien()
        {
            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ HÓA ĐƠN",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Location = new Point(370, 20),
                Size = new Size(400, 40)
            };
            Controls.Add(lblTitle);

            // Các nhãn và ô nhập
            TaoLabel("Mã hóa đơn:", 30, 90);
            TaoTextBox(txtMaHD, 160, 88, 300);

            TaoLabel("Mã đặt phòng:", 30, 135);
            TaoComboBox(cboDatPhong, 160, 133, 300);

            TaoLabel("Nhân viên:", 30, 180);
            TaoComboBox(cboNhanVien, 160, 178, 300);

            TaoLabel("Ngày lập:", 530, 90);
            dtpNgayLap.Location = new Point(660, 88);
            dtpNgayLap.Size = new Size(300, 28);
            dtpNgayLap.Format = DateTimePickerFormat.Short;
            Controls.Add(dtpNgayLap);

            TaoLabel("Tổng tiền:", 530, 135);
            TaoTextBox(txtTongTien, 660, 133, 300);
            txtTongTien.ReadOnly = true;
            txtTongTien.BackColor = Color.LightGray;

            TaoLabel("Ghi chú:", 530, 180);
            TaoTextBox(txtGhiChu, 660, 178, 300);

            // Nút tính tổng tiền từ chi tiết
            Button btnTinhTong = new Button
            {
                Text = "Tính tổng từ CTHD",
                Location = new Point(530, 230),
                Size = new Size(150, 35),
                BackColor = Color.LightGreen,
                Font = new Font("Arial", 9)
            };
            btnTinhTong.Click += (s, e) => TinhTongTien();
            Controls.Add(btnTinhTong);

            // Các nút chức năng
            TaoButton("Thêm", 30, 230, Them);
            TaoButton("Sửa", 140, 230, Sua);
            TaoButton("Xóa", 250, 230, Xoa);
            TaoButton("Làm mới", 360, 230, LamMoi);

            // Tìm kiếm
            Label lblSearch = new Label
            {
                Text = "Tìm kiếm:",
                Location = new Point(700, 238),
                Size = new Size(80, 25)
            };
            Controls.Add(lblSearch);

            txtTimKiem.Location = new Point(780, 235);
            txtTimKiem.Size = new Size(170, 28);
            Controls.Add(txtTimKiem);

            TaoButton("Tìm kiếm", 960, 230, TimKiem);

            // Bảng dữ liệu
            dgv.Location = new Point(25, 290);
            dgv.Size = new Size(1030, 370);
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowHeadersVisible = false;
            dgv.CellClick += Dgv_CellClick;
            Controls.Add(dgv);
        }

        private void TaoLabel(string text, int x, int y)
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

        private void TaoTextBox(TextBox txt, int x, int y, int width)
        {
            txt.Location = new Point(x, y);
            txt.Size = new Size(width, 28);
            txt.Font = new Font("Arial", 10);
            Controls.Add(txt);
        }

        private void TaoComboBox(ComboBox cbo, int x, int y, int width)
        {
            cbo.Location = new Point(x, y);
            cbo.Size = new Size(width, 28);
            cbo.Font = new Font("Arial", 10);
            cbo.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(cbo);
        }

        private void TaoButton(string text, int x, int y, Action action)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(100, 35),
                BackColor = Color.LightSteelBlue,
                Font = new Font("Arial", 10)
            };
            btn.Click += (s, e) => action();
            Controls.Add(btn);
        }

        private void LoadComboBox()
        {
            try
            {
                cboDatPhong.DataSource = dal.GetDanhSachDatPhong();
                cboDatPhong.DisplayMember = "MaDP";
                cboDatPhong.ValueMember = "MaDP";
                cboDatPhong.SelectedIndex = -1;

                cboNhanVien.DataSource = dal.GetDanhSachNhanVien();
                cboNhanVien.DisplayMember = "HoTen";
                cboNhanVien.ValueMember = "MaNV";
                cboNhanVien.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void LoadData()
        {
            try
            {
                dgv.DataSource = dal.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải hóa đơn: " + ex.Message);
            }
        }

        private void TinhTongTien()
        {
            if (!int.TryParse(txtMaHD.Text.Trim(), out int maHD))
            {
                MessageBox.Show("Vui lòng nhập mã hóa đơn!");
                return;
            }

            try
            {
                decimal tong = dal.TinhTongTien(maHD);
                txtTongTien.Text = tong.ToString("N0");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private bool KiemTra(out HoaDon hd)
        {
            hd = new HoaDon();

            if (!int.TryParse(txtMaHD.Text.Trim(), out int maHD))
            {
                MessageBox.Show("Mã hóa đơn phải là số nguyên!");
                txtMaHD.Focus();
                return false;
            }

            if (cboDatPhong.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn mã đặt phòng!");
                return false;
            }

            if (cboNhanVien.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên!");
                return false;
            }

            decimal tongTien = 0;
            if (!string.IsNullOrWhiteSpace(txtTongTien.Text))
            {
                decimal.TryParse(txtTongTien.Text.Replace(",", "")
                    .Replace(".", ""), out tongTien);
            }

            hd.MaHD = maHD;
            hd.MaDP = Convert.ToInt32(cboDatPhong.SelectedValue);
            hd.MaNV = Convert.ToInt32(cboNhanVien.SelectedValue);
            hd.NgayLap = dtpNgayLap.Value.Date;
            hd.TongTien = tongTien;
            hd.GhiChu = txtGhiChu.Text.Trim();

            return true;
        }

        private void Them()
        {
            if (!KiemTra(out HoaDon hd)) return;

            try
            {
                dal.Them(hd);
                MessageBox.Show("Thêm hóa đơn thành công!");
                LoadData();
                LamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void Sua()
        {
            if (!KiemTra(out HoaDon hd)) return;

            try
            {
                dal.Sua(hd);
                MessageBox.Show("Sửa hóa đơn thành công!");
                LoadData();
                LamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void Xoa()
        {
            if (!int.TryParse(txtMaHD.Text.Trim(), out int maHD))
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa hóa đơn này?",
                "Xác nhận", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                dal.Xoa(maHD);
                MessageBox.Show("Xóa hóa đơn thành công!");
                LoadData();
                LamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void TimKiem()
        {
            try
            {
                dgv.DataSource = dal.TimKiem(txtTimKiem.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }

        private void LamMoi()
        {
            txtMaHD.Clear();
            txtTongTien.Clear();
            txtGhiChu.Clear();
            txtTimKiem.Clear();
            cboDatPhong.SelectedIndex = -1;
            cboNhanVien.SelectedIndex = -1;
            dtpNgayLap.Value = DateTime.Today;
            LoadData();
        }

        private void Dgv_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgv.Rows[e.RowIndex];

            txtMaHD.Text = row.Cells["MaHD"].Value?.ToString() ?? "";
            txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString() ?? "";
            txtTongTien.Text = row.Cells["TongTien"].Value?.ToString() ?? "";

            object? maDP = row.Cells["MaDP"].Value;
            if (maDP != null && maDP != DBNull.Value)
                cboDatPhong.SelectedValue = Convert.ToInt32(maDP);

            object? maNV = row.Cells["MaNV"].Value;
            if (maNV != null && maNV != DBNull.Value)
                cboNhanVien.SelectedValue = Convert.ToInt32(maNV);

            if (row.Cells["NgayLap"].Value != DBNull.Value)
                dtpNgayLap.Value =
                    Convert.ToDateTime(row.Cells["NgayLap"].Value);
        }
    }
}
