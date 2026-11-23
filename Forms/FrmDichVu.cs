
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.Forms
{
    public class FrmDichVu : Form
    {
        private readonly DichVuDAL dal = new();

        private TextBox txtMaDV = new();
        private TextBox txtTenDV = new();
        private TextBox txtDonGia = new();
        private TextBox txtDonViTinh = new();
        private TextBox txtMoTa = new();
        private TextBox txtTimKiem = new();

        private DataGridView dgv = new();

        public FrmDichVu()
        {
            Text = "QUẢN LÝ DỊCH VỤ";
            Size = new Size(1050, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            TaoGiaoDien();
            LoadData();
        }

        private void TaoGiaoDien()
        {
            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ DỊCH VỤ",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Location = new Point(350, 20),
                Size = new Size(400, 40)
            };
            Controls.Add(lblTitle);

            // Các nhãn và ô nhập
            TaoLabel("Mã dịch vụ:", 30, 90);
            TaoTextBox(txtMaDV, 160, 88, 300);

            TaoLabel("Tên dịch vụ:", 30, 135);
            TaoTextBox(txtTenDV, 160, 133, 300);

            TaoLabel("Đơn giá:", 530, 90);
            TaoTextBox(txtDonGia, 660, 88, 300);

            TaoLabel("Đơn vị tính:", 530, 135);
            TaoTextBox(txtDonViTinh, 660, 133, 300);

            TaoLabel("Mô tả:", 30, 180);
            TaoTextBox(txtMoTa, 160, 178, 800);

            // Các nút chức năng
            TaoButton("Thêm", 30, 230, Them);
            TaoButton("Sửa", 140, 230, Sua);
            TaoButton("Xóa", 250, 230, Xoa);
            TaoButton("Làm mới", 360, 230, LamMoi);

            // Tìm kiếm
            Label lblSearch = new Label
            {
                Text = "Tìm kiếm:",
                Location = new Point(530, 238),
                Size = new Size(80, 25)
            };
            Controls.Add(lblSearch);

            txtTimKiem.Location = new Point(610, 235);
            txtTimKiem.Size = new Size(200, 28);
            Controls.Add(txtTimKiem);

            TaoButton("Tìm kiếm", 820, 230, TimKiem);

            // Bảng dữ liệu
            dgv.Location = new Point(30, 290);
            dgv.Size = new Size(970, 330);
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

        private void LoadData()
        {
            try
            {
                dgv.DataSource = dal.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private bool KiemTra(out DichVu dv)
        {
            dv = new DichVu();

            if (!int.TryParse(txtMaDV.Text.Trim(), out int maDV))
            {
                MessageBox.Show("Mã dịch vụ phải là số nguyên!");
                txtMaDV.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTenDV.Text))
            {
                MessageBox.Show("Vui lòng nhập tên dịch vụ!");
                txtTenDV.Focus();
                return false;
            }

            if (!decimal.TryParse(txtDonGia.Text.Trim(), out decimal donGia) ||
                donGia <= 0)
            {
                MessageBox.Show("Đơn giá phải là số lớn hơn 0!");
                txtDonGia.Focus();
                return false;
            }

            dv.MaDV = maDV;
            dv.TenDV = txtTenDV.Text.Trim();
            dv.DonGia = donGia;
            dv.DonViTinh = txtDonViTinh.Text.Trim();
            dv.MoTa = txtMoTa.Text.Trim();

            return true;
        }

        private void Them()
        {
            if (!KiemTra(out DichVu dv)) return;

            try
            {
                dal.Them(dv);
                MessageBox.Show("Thêm dịch vụ thành công!");
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
            if (!KiemTra(out DichVu dv)) return;

            try
            {
                dal.Sua(dv);
                MessageBox.Show("Sửa dịch vụ thành công!");
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
            if (!int.TryParse(txtMaDV.Text.Trim(), out int maDV))
            {
                MessageBox.Show("Vui lòng chọn dịch vụ cần xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa dịch vụ này?",
                "Xác nhận", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                dal.Xoa(maDV);
                MessageBox.Show("Xóa dịch vụ thành công!");
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
            txtMaDV.Clear();
            txtTenDV.Clear();
            txtDonGia.Clear();
            txtDonViTinh.Clear();
            txtMoTa.Clear();
            txtTimKiem.Clear();
            LoadData();
        }

        private void Dgv_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgv.Rows[e.RowIndex];

            txtMaDV.Text = row.Cells["MaDV"].Value?.ToString() ?? "";
            txtTenDV.Text = row.Cells["TenDV"].Value?.ToString() ?? "";
            txtDonGia.Text = row.Cells["DonGia"].Value?.ToString() ?? "";
            txtDonViTinh.Text = row.Cells["DonViTinh"].Value?.ToString() ?? "";
            txtMoTa.Text = row.Cells["MoTa"].Value?.ToString() ?? "";
        }
    }
}
