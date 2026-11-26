
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKhachSan.BUS;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.Forms
{
    public class FrmChiTietHoaDon : Form
    {
        private readonly ChiTietHoaDonBUS bus = new();

        private TextBox txtMaCTHD = new();
        private ComboBox cboHoaDon = new();
        private ComboBox cboDichVu = new();
        private TextBox txtSoLuong = new();
        private TextBox txtDonGia = new();
        private TextBox txtThanhTien = new();
        private TextBox txtTimKiem = new();

        private DataGridView dgv = new();
        private Label lblTongTien = new();

        public FrmChiTietHoaDon()
        {
            Text = "QUẢN LÝ CHI TIẾT HÓA ĐƠN";
            Size = new Size(1050, 700);
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
                Text = "QUẢN LÝ CHI TIẾT HÓA ĐƠN",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                AutoSize = true,
                Location = new Point(300, 20)
            };
            Controls.Add(lblTitle);

            // Các nhãn và ô nhập
            TaoLabel("Mã chi tiết:", 30, 90);
            TaoTextBox(txtMaCTHD, 160, 88, 200);

            TaoLabel("Mã hóa đơn:", 30, 135);
            TaoComboBox(cboHoaDon, 160, 133, 200);

            TaoLabel("Dịch vụ:", 420, 90);
            TaoComboBox(cboDichVu, 530, 88, 250);

            TaoLabel("Số lượng:", 420, 135);
            TaoTextBox(txtSoLuong, 530, 133, 100);

            TaoLabel("Đơn giá:", 660, 135);
            TaoTextBox(txtDonGia, 730, 133, 120);
            txtDonGia.ReadOnly = true;
            txtDonGia.BackColor = Color.LightGray;

            TaoLabel("Thành tiền:", 30, 180);
            TaoTextBox(txtThanhTien, 160, 178, 200);
            txtThanhTien.ReadOnly = true;
            txtThanhTien.BackColor = Color.LightGray;

            // Tự động tính thành tiền khi thay đổi số lượng
            txtSoLuong.TextChanged += (s, e) => TinhThanhTien();

            // Tự động lấy đơn giá khi chọn dịch vụ
            cboDichVu.SelectedIndexChanged += (s, e) =>
            {
                if (cboDichVu.SelectedIndex >= 0 &&
                    cboDichVu.SelectedValue != null &&
                    cboDichVu.SelectedValue is not DataRowView)
                {
                    // Lấy đơn giá từ DataTable
                    DataTable? dt = cboDichVu.DataSource as DataTable;
                    if (dt != null && cboDichVu.SelectedIndex < dt.Rows.Count)
                    {
                        txtDonGia.Text = dt.Rows[cboDichVu.SelectedIndex]["DonGia"]
                            .ToString();
                        TinhThanhTien();
                    }
                }
            };

            // Các nút chức năng
            TaoButton("Thêm", 30, 225, Them);
            TaoButton("Sửa", 140, 225, Sua);
            TaoButton("Xóa", 250, 225, Xoa);
            TaoButton("Làm mới", 360, 225, LamMoi);

            // Tìm kiếm
            txtTimKiem.Location = new Point(530, 230);
            txtTimKiem.Size = new Size(180, 28);
            Controls.Add(txtTimKiem);

            TaoButton("Tìm kiếm", 720, 225, TimKiem);

            // Bảng dữ liệu
            dgv.Location = new Point(30, 280);
            dgv.Size = new Size(970, 310);
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowHeadersVisible = false;
            dgv.CellClick += Dgv_CellClick;
            Controls.Add(dgv);

            // Tổng tiền
            lblTongTien.Location = new Point(30, 610);
            lblTongTien.AutoSize = true;
            lblTongTien.Font = new Font("Arial", 12, FontStyle.Bold);
            lblTongTien.ForeColor = Color.DarkRed;
            lblTongTien.Text = "Tổng tiền: 0 VNĐ";
            Controls.Add(lblTongTien);

            // Nút Xuất Excel
            Button btnXuatExcel = new Button
            {
                Text = "📊 Xuất Excel (CSV)",
                Location = new Point(780, 605),
                Size = new Size(220, 35),
                BackColor = Color.ForestGreen,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnXuatExcel.Click += (s, e) => FrmThongKe.XuatDuLieuCSV(dgv, "DanhSachChiTietHoaDon");
            Controls.Add(btnXuatExcel);
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

        private void TinhThanhTien()
        {
            if (int.TryParse(txtSoLuong.Text, out int sl) &&
                decimal.TryParse(txtDonGia.Text, out decimal dg))
            {
                decimal tt = sl * dg;
                txtThanhTien.Text = tt.ToString("N0");
            }
            else
            {
                txtThanhTien.Text = "";
            }
        }

        private void LoadComboBox()
        {
            try
            {
                cboHoaDon.DataSource = bus.GetDanhSachHoaDon();
                cboHoaDon.DisplayMember = "MaHD";
                cboHoaDon.ValueMember = "MaHD";
                cboHoaDon.SelectedIndex = -1;

                cboDichVu.DataSource = bus.GetDanhSachDichVu();
                cboDichVu.DisplayMember = "TenDV";
                cboDichVu.ValueMember = "MaDV";
                cboDichVu.SelectedIndex = -1;
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
                dgv.DataSource = bus.GetAll();
                HienThiTongTien();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết hóa đơn: " + ex.Message);
            }
        }

        private void HienThiTongTien()
        {
            if (cboHoaDon.SelectedValue == null)
            {
                lblTongTien.Text = "Tổng tiền: 0 VNĐ";
                return;
            }

            if (int.TryParse(cboHoaDon.SelectedValue.ToString(), out int maHD))
            {
                decimal tong = bus.TinhTongTien(maHD);
                lblTongTien.Text = "Tổng tiền: " + tong.ToString("N0") + " VNĐ";
            }
        }

        private bool KiemTra(out ChiTietHoaDon ct)
        {
            ct = new ChiTietHoaDon();

            if (!int.TryParse(txtMaCTHD.Text.Trim(), out int maCTHD))
            {
                MessageBox.Show("Mã chi tiết phải là số nguyên!");
                txtMaCTHD.Focus();
                return false;
            }

            if (cboHoaDon.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn mã hóa đơn!");
                return false;
            }

            if (cboDichVu.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn dịch vụ!");
                return false;
            }

            if (!int.TryParse(txtSoLuong.Text.Trim(), out int soLuong) ||
                soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên lớn hơn 0!");
                txtSoLuong.Focus();
                return false;
            }

            if (!decimal.TryParse(txtDonGia.Text.Trim(), out decimal donGia))
            {
                MessageBox.Show("Đơn giá không hợp lệ!");
                return false;
            }

            ct.MaCTHD = maCTHD;
            ct.MaHD = Convert.ToInt32(cboHoaDon.SelectedValue);
            ct.MaDV = Convert.ToInt32(cboDichVu.SelectedValue);
            ct.SoLuong = soLuong;
            ct.DonGia = donGia;
            ct.ThanhTien = soLuong * donGia;

            return true;
        }

        private void Them()
        {
            if (!KiemTra(out ChiTietHoaDon ct)) return;

            try
            {
                bus.Them(ct);
                MessageBox.Show("Thêm chi tiết hóa đơn thành công!");
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
            if (!KiemTra(out ChiTietHoaDon ct)) return;

            try
            {
                bus.Sua(ct);
                MessageBox.Show("Sửa chi tiết hóa đơn thành công!");
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
            if (!int.TryParse(txtMaCTHD.Text.Trim(), out int maCTHD))
            {
                MessageBox.Show("Vui lòng chọn chi tiết cần xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa?",
                "Xác nhận", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                bus.Xoa(maCTHD);
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
            try
            {
                dgv.DataSource = bus.TimKiem(txtTimKiem.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }

        private void LamMoi()
        {
            txtMaCTHD.Clear();
            txtSoLuong.Clear();
            txtDonGia.Clear();
            txtThanhTien.Clear();
            txtTimKiem.Clear();
            cboHoaDon.SelectedIndex = -1;
            cboDichVu.SelectedIndex = -1;
            LoadData();
        }

        private void Dgv_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgv.Rows[e.RowIndex];

            txtMaCTHD.Text = row.Cells["MaCTHD"].Value?.ToString() ?? "";
            txtSoLuong.Text = row.Cells["SoLuong"].Value?.ToString() ?? "";
            txtDonGia.Text = row.Cells["DonGia"].Value?.ToString() ?? "";
            txtThanhTien.Text = row.Cells["ThanhTien"].Value?.ToString() ?? "";

            object? maHD = row.Cells["MaHD"].Value;
            if (maHD != null && maHD != DBNull.Value)
                cboHoaDon.SelectedValue = Convert.ToInt32(maHD);

            object? maDV = row.Cells["MaDV"].Value;
            if (maDV != null && maDV != DBNull.Value)
                cboDichVu.SelectedValue = Convert.ToInt32(maDV);

            HienThiTongTien();
        }
    }
}
