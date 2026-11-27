
using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKhachSan.BUS;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.Forms
{
    public class FrmChiTietDatPhong : Form
    {
        private readonly ChiTietDatPhongBUS bus = new();

        private TextBox txtMaCTDP = new();
        private TextBox txtDonGia = new();
        private TextBox txtTimKiem = new();

        private ComboBox cboDatPhong = new();
        private ComboBox cboPhong = new();

        private DataGridView dgv = new();

        private Label lblTongTien = new();

        public FrmChiTietDatPhong()
        {
            Text = "QUẢN LÝ CHI TIẾT ĐẶT PHÒNG";
            Size = new Size(1000, 650);
            StartPosition = FormStartPosition.CenterScreen;

            TaoGiaoDien();

            Load += (s, e) =>
            {
                LoadComboBox();
                LoadData();
            };
        }

        private void TaoGiaoDien()
        {
            Label title = new Label
            {
                Text = "QUẢN LÝ CHI TIẾT ĐẶT PHÒNG",
                Font = new Font("Arial", 18, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(300, 20)
            };

            Controls.Add(title);

            TaoLabel("Mã chi tiết:", 40, 90);
            txtMaCTDP.Location = new Point(150, 90);
            txtMaCTDP.Width = 200;
            Controls.Add(txtMaCTDP);

            TaoLabel("Mã đặt phòng:", 40, 135);
            cboDatPhong.Location = new Point(150, 135);
            cboDatPhong.Width = 200;
            cboDatPhong.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(cboDatPhong);

            TaoLabel("Phòng:", 420, 90);
            cboPhong.Location = new Point(530, 90);
            cboPhong.Width = 200;
            cboPhong.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(cboPhong);

            TaoLabel("Đơn giá:", 420, 135);
            txtDonGia.Location = new Point(530, 135);
            txtDonGia.Width = 200;
            Controls.Add(txtDonGia);

            ThemButton("Thêm", 40, 190, Them);
            ThemButton("Sửa", 140, 190, Sua);
            ThemButton("Xóa", 240, 190, Xoa);
            ThemButton("Làm mới", 340, 190, LamMoi);

            txtTimKiem.Location = new Point(530, 195);
            txtTimKiem.Width = 150;
            Controls.Add(txtTimKiem);

            ThemButton("Tìm kiếm", 690, 190, TimKiem);

            dgv.Location = new Point(40, 250);
            dgv.Size = new Size(900, 280);
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.MultiSelect = false;
            dgv.CellClick += Dgv_CellClick;
            Controls.Add(dgv);

            lblTongTien.Location = new Point(40, 550);
            lblTongTien.AutoSize = true;
            lblTongTien.Font = new Font("Arial", 12, FontStyle.Bold);
            Controls.Add(lblTongTien);
        }

        private void TaoLabel(string text, int x, int y)
        {
            Label lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true
            };

            Controls.Add(lbl);
        }

        private void ThemButton(string text, int x, int y, Action action)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(90, 32)
            };

            btn.Click += (s, e) => action();
            Controls.Add(btn);
        }

        private void LoadComboBox()
        {
            cboDatPhong.DataSource = bus.GetDanhSachDatPhong();
            cboDatPhong.DisplayMember = "MaDP";
            cboDatPhong.ValueMember = "MaDP";
            cboDatPhong.SelectedIndex = -1;

            cboPhong.DataSource = bus.GetDanhSachPhong();
            cboPhong.DisplayMember = "TenPhong";
            cboPhong.ValueMember = "MaPhong";
            cboPhong.SelectedIndex = -1;
        }

        private void LoadData()
        {
            dgv.DataSource = bus.GetAll();
            HienThiTongTien();
        }

        private void HienThiTongTien()
        {
            if (cboDatPhong.SelectedValue == null)
            {
                lblTongTien.Text = "Tổng tiền: 0 VNĐ";
                return;
            }

            if (int.TryParse(cboDatPhong.SelectedValue.ToString(), out int maDP))
            {
                decimal tong = bus.TinhTongTien(maDP);
                lblTongTien.Text = "Tổng tiền: " + tong.ToString("N0") + " VNĐ";
            }
        }

        private bool KiemTra(out ChiTietDatPhong ct)
        {
            ct = new ChiTietDatPhong();

            if (!int.TryParse(txtMaCTDP.Text, out int maCTDP))
            {
                MessageBox.Show("Mã chi tiết phải là số nguyên!");
                return false;
            }

            if (cboDatPhong.SelectedValue == null ||
                cboPhong.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn mã đặt phòng và phòng!");
                return false;
            }

            if (!decimal.TryParse(txtDonGia.Text, out decimal donGia) ||
                donGia <= 0)
            {
                MessageBox.Show("Đơn giá phải lớn hơn 0!");
                return false;
            }

            ct.MaCTDP = maCTDP;
            ct.MaDP = Convert.ToInt32(cboDatPhong.SelectedValue);
            ct.MaPhong = Convert.ToInt32(cboPhong.SelectedValue);
            ct.DonGia = donGia;

            return true;
        }

        private void Them()
        {
            if (!KiemTra(out ChiTietDatPhong ct))
                return;

            try
            {
                bus.Them(ct);
                MessageBox.Show("Thêm chi tiết đặt phòng thành công!");
                LoadData();
                HienThiTongTien();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void Sua()
        {
            if (!KiemTra(out ChiTietDatPhong ct))
                return;

            try
            {
                bus.Sua(ct);
                MessageBox.Show("Sửa chi tiết đặt phòng thành công!");
                LoadData();
                HienThiTongTien();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void Xoa()
        {
            if (!int.TryParse(txtMaCTDP.Text, out int maCTDP))
            {
                MessageBox.Show("Vui lòng nhập mã chi tiết cần xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa?",
                "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            try
            {
                bus.Xoa(maCTDP);
                MessageBox.Show("Xóa thành công!");
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
            dgv.DataSource = bus.TimKiem(txtTimKiem.Text.Trim());
        }

        private void LamMoi()
        {
            txtMaCTDP.Clear();
            txtDonGia.Clear();
            txtTimKiem.Clear();

            cboDatPhong.SelectedIndex = -1;
            cboPhong.SelectedIndex = -1;

            LoadData();
        }

        private void Dgv_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgv.Rows[e.RowIndex];

            txtMaCTDP.Text = row.Cells["MaCTDP"].Value?.ToString() ?? "";
            txtDonGia.Text = row.Cells["DonGia"].Value?.ToString() ?? "";

            cboDatPhong.SelectedValue = Convert.ToInt32(
                row.Cells["MaDP"].Value);

            cboPhong.SelectedValue = Convert.ToInt32(
                row.Cells["MaPhong"].Value);

            HienThiTongTien();
        }
    }
}
