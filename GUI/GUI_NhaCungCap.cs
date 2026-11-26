using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKhachSan.BUS;

namespace QuanLyKhachSan.GUI
{
    /// <summary>
    /// TẦNG GUI: MÀN HÌNH QUẢN LÝ NHÀ CUNG CẤP
    /// Nhiệm vụ: Xử lý giao diện, bắt sự kiện người dùng và gọi tầng NhaCungCapBUS
    /// </summary>
    public class GUI_NhaCungCap : Form
    {
        // 1. Khởi tạo đối tượng tầng nghiệp vụ BUS (Không gọi trực tiếp DAL)
        private readonly NhaCungCapBUS nccBUS = new();

        // 2. Các điều khiển nhập liệu (Controls)
        private TextBox txtMaNCC   = new();
        private TextBox txtTenNCC  = new();
        private TextBox txtSDT     = new();
        private TextBox txtDiaChi  = new();
        private TextBox txtEmail   = new();
        private TextBox txtTimKiem = new();
        private DataGridView dgvNCC = new();

        public GUI_NhaCungCap()
        {
            KhoiTaoGiaoDien();
            TaiDuLieu();
        }

        private void KhoiTaoGiaoDien()
        {
            Text = "QUẢN LÝ NHÀ CUNG CẤP - [TẦNG GUI]";
            Size = new Size(1100, 680);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            // Tiêu đề
            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ DANH MỤC NHÀ CUNG CẤP",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Location = new Point(320, 20),
                AutoSize = true
            };
            Controls.Add(lblTitle);

            // GroupBox: Thông tin chi tiết
            GroupBox grpInfo = new GroupBox
            {
                Text = "Thông tin nhà cung cấp",
                Location = new Point(25, 65),
                Size = new Size(1035, 150),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            ThemNhanVaO(grpInfo, "Mã NCC:", txtMaNCC, 20, 30, 220);
            txtMaNCC.ReadOnly = true;
            txtMaNCC.BackColor = Color.LightGray;

            ThemNhanVaO(grpInfo, "Tên NCC:", txtTenNCC, 20, 70, 220);
            ThemNhanVaO(grpInfo, "Số ĐT:", txtSDT, 20, 110, 220);
            ThemNhanVaO(grpInfo, "Email:", txtEmail, 420, 30, 580);
            ThemNhanVaO(grpInfo, "Địa chỉ:", txtDiaChi, 420, 70, 580);

            Controls.Add(grpInfo);

            // Các nút chức năng
            ThemNut("Thêm mới", 25, 230, BtnThem_Click);
            ThemNut("Cập nhật", 140, 230, BtnSua_Click);
            ThemNut("Xóa", 255, 230, BtnXoa_Click);
            ThemNut("Làm mới", 370, 230, BtnLamMoi_Click);

            // Ô tìm kiếm
            Label lblTim = new Label { Text = "Tìm kiếm:", Location = new Point(550, 238), AutoSize = true, Font = new Font("Arial", 10) };
            Controls.Add(lblTim);

            txtTimKiem.SetBounds(630, 234, 280, 26);
            txtTimKiem.Font = new Font("Arial", 10);
            Controls.Add(txtTimKiem);

            ThemNut("Tìm", 930, 230, BtnTimKiem_Click);

            // DataGridView hiển thị danh sách
            dgvNCC.SetBounds(25, 280, 1035, 340);
            dgvNCC.ReadOnly = true;
            dgvNCC.AllowUserToAddRows = false;
            dgvNCC.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNCC.MultiSelect = false;
            dgvNCC.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNCC.RowHeadersVisible = false;
            dgvNCC.BackgroundColor = Color.White;
            dgvNCC.CellClick += DgvNCC_CellClick;
            Controls.Add(dgvNCC);
        }

        private void ThemNhanVaO(GroupBox grp, string nhan, Control c, int x, int y, int w)
        {
            Label lbl = new Label { Text = nhan, Location = new Point(x, y + 3), AutoSize = true, Font = new Font("Arial", 9.5f, FontStyle.Regular) };
            grp.Controls.Add(lbl);
            c.SetBounds(x + 85, y, w, 26);
            c.Font = new Font("Arial", 9.5f, FontStyle.Regular);
            grp.Controls.Add(c);
        }

        private void ThemNut(string text, int x, int y, EventHandler onClick)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(100, 35),
                BackColor = Color.LightSteelBlue,
                Font = new Font("Arial", 9.5f, FontStyle.Bold)
            };
            btn.Click += onClick;
            Controls.Add(btn);
        }

        // ==========================================
        // XỬ LÝ DỮ LIỆU & GỌI TẦNG BUS
        // ==========================================
        private void TaiDuLieu()
        {
            try
            {
                dgvNCC.DataSource = nccBUS.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnThem_Click(object? sender, EventArgs e)
        {
            try
            {
                nccBUS.Them(txtTenNCC.Text, txtSDT.Text, txtDiaChi.Text, txtEmail.Text);
                MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDuLieu();
                LamMoi();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Cảnh báo nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm thất bại: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaNCC.Text, out int id))
            {
                MessageBox.Show("Vui lòng chọn một nhà cung cấp từ bảng để sửa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                nccBUS.Sua(id, txtTenNCC.Text, txtSDT.Text, txtDiaChi.Text, txtEmail.Text);
                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TaiDuLieu();
                LamMoi();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Cảnh báo nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cập nhật thất bại: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtMaNCC.Text, out int id))
            {
                MessageBox.Show("Vui lòng chọn một nhà cung cấp từ bảng để xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa nhà cung cấp này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    nccBUS.Xoa(id);
                    MessageBox.Show("Xóa nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDuLieu();
                    LamMoi();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xóa thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnTimKiem_Click(object? sender, EventArgs e)
        {
            try
            {
                dgvNCC.DataSource = nccBUS.TimKiem(txtTimKiem.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            LamMoi();
        }

        private void LamMoi()
        {
            txtMaNCC.Clear();
            txtTenNCC.Clear();
            txtSDT.Clear();
            txtDiaChi.Clear();
            txtEmail.Clear();
            txtTimKiem.Clear();
            TaiDuLieu();
        }

        private void DgvNCC_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvNCC.Rows[e.RowIndex];

            txtMaNCC.Text  = row.Cells["MaNCC"].Value?.ToString() ?? "";
            txtTenNCC.Text = row.Cells["TenNCC"].Value?.ToString() ?? "";
            txtSDT.Text    = row.Cells["SDT"].Value?.ToString() ?? "";
            txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString() ?? "";
            txtEmail.Text  = row.Cells["Email"].Value?.ToString() ?? "";
        }
    }
}
