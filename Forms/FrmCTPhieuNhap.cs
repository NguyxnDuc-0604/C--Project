using System.Data;
using System.Drawing;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.Forms
{
    public class FrmCTPhieuNhap : Form
    {
        private readonly CTPhieuNhapDAL dal = new();

        private TextBox  txtMaCTPN    = new();
        private ComboBox cboPhieuNhap = new();
        private ComboBox cboDichVu    = new();
        private TextBox  txtSoLuong   = new();
        private TextBox  txtDonGia    = new();   // chỉ đọc, tự điền theo DV
        private TextBox  txtThanhTien = new();   // chỉ đọc, tự tính
        private TextBox  txtTimKiem   = new();

        private DataGridView dgv        = new();
        private Label        lblTongTien = new();

        // Lưu số lượng cũ khi click lưới (dùng cho Sửa tồn kho)
        private int _soLuongCu = 0;

        public FrmCTPhieuNhap()
        {
            Text = "QUẢN LÝ CHI TIẾT PHIẾU NHẬP";
            Size = new Size(1100, 720);
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
                Text = "QUẢN LÝ CHI TIẾT PHIẾU NHẬP",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Location = new Point(290, 20),
                AutoSize = true
            });

            // Dòng 1
            Field("Mã chi tiết:", txtMaCTPN, 30, 85);
            txtMaCTPN.ReadOnly = true;
            txtMaCTPN.BackColor = Color.LightGray;

            Field("Phiếu nhập:", cboPhieuNhap, 580, 85);
            cboPhieuNhap.DropDownStyle = ComboBoxStyle.DropDownList;

            // Dòng 2: ComboBox Dịch Vụ — hiển thị TenDV thay vì MaDV
            Field("Dịch vụ:", cboDichVu, 30, 130);
            cboDichVu.DropDownStyle = ComboBoxStyle.DropDownList;

            // Khi chọn dịch vụ: tự điền đơn giá nhập
            cboDichVu.SelectedIndexChanged += (s, e) =>
            {
                if (cboDichVu.SelectedIndex >= 0 &&
                    cboDichVu.SelectedValue != null &&
                    cboDichVu.SelectedValue is not DataRowView)
                {
                    DataTable? dt = cboDichVu.DataSource as DataTable;
                    if (dt != null && cboDichVu.SelectedIndex < dt.Rows.Count)
                    {
                        txtDonGia.Text = dt.Rows[cboDichVu.SelectedIndex]["DonGia"].ToString();
                        TinhThanhTien();
                    }
                }
            };

            // Dòng 3
            Field("Số lượng:", txtSoLuong, 30, 175);
            txtSoLuong.TextChanged += (s, e) => TinhThanhTien();

            Field("Đơn giá nhập:", txtDonGia, 580, 130);
            txtDonGia.ReadOnly = true;
            txtDonGia.BackColor = Color.LightGray;

            Field("Thành tiền:", txtThanhTien, 580, 175);
            txtThanhTien.ReadOnly = true;
            txtThanhTien.BackColor = Color.LightGray;

            // Nút chức năng
            Btn("Thêm",    30,  225, Them);
            Btn("Sửa",     140, 225, Sua);
            Btn("Xóa",     250, 225, Xoa);
            Btn("Làm mới", 360, 225, Reset);

            // Tìm kiếm
            Controls.Add(new Label { Text = "Tìm kiếm:", Location = new Point(580, 235), AutoSize = true });
            txtTimKiem.SetBounds(665, 230, 200, 28);
            Controls.Add(txtTimKiem);
            Btn("Tìm", 875, 228, Search);

            // DataGridView
            Grid(dgv, 30, 280, 1030, 320);
            dgv.CellClick += Dgv_CellClick;

            // Label tổng tiền
            lblTongTien.Location = new Point(30, 615);
            lblTongTien.AutoSize = true;
            lblTongTien.Font = new Font("Arial", 12, FontStyle.Bold);
            lblTongTien.ForeColor = Color.DarkRed;
            lblTongTien.Text = "Tổng tiền phiếu: 0 VNĐ";
            Controls.Add(lblTongTien);
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
                // ComboBox Phiếu Nhập
                cboPhieuNhap.DataSource    = dal.GetDanhSachPhieuNhap();
                cboPhieuNhap.DisplayMember = "MaPN";
                cboPhieuNhap.ValueMember   = "MaPN";
                cboPhieuNhap.SelectedIndex = -1;

                // ComboBox Dịch Vụ: DisplayMember = TenDV, ValueMember = MaDV
                cboDichVu.DataSource    = dal.GetDanhSachDichVu();
                cboDichVu.DisplayMember = "TenDV";
                cboDichVu.ValueMember   = "MaDV";
                cboDichVu.SelectedIndex = -1;
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tải ComboBox: " + ex.Message); }
        }

        private void LoadData()
        {
            try { dgv.DataSource = dal.GetAll(); }
            catch (Exception ex) { MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message); }
        }

        private void TinhThanhTien()
        {
            if (int.TryParse(txtSoLuong.Text.Trim(), out int sl) &&
                decimal.TryParse(txtDonGia.Text.Trim(), out decimal dg))
                txtThanhTien.Text = (sl * dg).ToString("N0");
            else
                txtThanhTien.Text = "";
        }

        private void HienThiTongTien()
        {
            if (cboPhieuNhap.SelectedValue != null &&
                int.TryParse(cboPhieuNhap.SelectedValue.ToString(), out int maPN))
            {
                try
                {
                    decimal tong = dal.TinhTongTien(maPN);
                    lblTongTien.Text = "Tổng tiền phiếu: " + tong.ToString("N0") + " VNĐ";
                }
                catch { lblTongTien.Text = "Tổng tiền phiếu: 0 VNĐ"; }
            }
        }

        private bool Valid(out CTPhieuNhap ct)
        {
            ct = new CTPhieuNhap();

            if (cboPhieuNhap.SelectedValue == null || cboPhieuNhap.SelectedValue is DataRowView)
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập!");
                return false;
            }
            if (cboDichVu.SelectedValue == null || cboDichVu.SelectedValue is DataRowView)
            {
                MessageBox.Show("Vui lòng chọn dịch vụ!");
                return false;
            }
            if (!int.TryParse(txtSoLuong.Text.Trim(), out int sl) || sl <= 0)
            {
                MessageBox.Show("Số lượng phải là số nguyên lớn hơn 0!");
                txtSoLuong.Focus();
                return false;
            }
            if (!decimal.TryParse(txtDonGia.Text.Trim(), out decimal dg))
            {
                MessageBox.Show("Đơn giá không hợp lệ!");
                return false;
            }

            ct.MaCTPN     = int.TryParse(txtMaCTPN.Text, out int id) ? id : 0;
            ct.MaPN       = Convert.ToInt32(cboPhieuNhap.SelectedValue);
            ct.MaDV       = Convert.ToInt32(cboDichVu.SelectedValue);
            ct.SoLuong    = sl;
            ct.DonGiaNhap = dg;

            return true;
        }

        private void Them(object? s, EventArgs e)
        {
            if (!Valid(out CTPhieuNhap ct)) return;
            try
            {
                // Trigger SQL tự cộng SoLuong vào DICH_VU
                dal.Them(ct);
                MessageBox.Show("Thêm thành công! Tồn kho đã được cập nhật tự động.");
                LoadData();
                HienThiTongTien();
            }
            catch (Exception ex) { MessageBox.Show("Thêm thất bại: " + ex.Message); }
        }

        private void Sua(object? s, EventArgs e)
        {
            if (!Valid(out CTPhieuNhap ct)) return;
            try
            {
                // DAL tính chênh lệch và cập nhật tồn kho
                dal.Sua(ct, _soLuongCu);
                MessageBox.Show("Sửa thành công! Tồn kho đã được điều chỉnh.");
                LoadData();
                HienThiTongTien();
            }
            catch (Exception ex) { MessageBox.Show("Sửa thất bại: " + ex.Message); }
        }

        private void Xoa(object? s, EventArgs e)
        {
            if (!int.TryParse(txtMaCTPN.Text, out int ma)) { MessageBox.Show("Vui lòng chọn chi tiết cần xóa!"); return; }
            if (MessageBox.Show("Xóa chi tiết này? Tồn kho sẽ được trừ lại.", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    // Trigger SQL tự trừ SoLuong khỏi DICH_VU
                    dal.Xoa(ma);
                    MessageBox.Show("Xóa thành công!");
                    LoadData();
                    Reset(null!, EventArgs.Empty);
                }
                catch (Exception ex) { MessageBox.Show("Xóa thất bại: " + ex.Message); }
            }
        }

        private void Search(object? s, EventArgs e)
        {
            try { dgv.DataSource = dal.TimKiem(txtTimKiem.Text.Trim()); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void Reset(object? s, EventArgs e)
        {
            txtMaCTPN.Clear();
            txtSoLuong.Clear();
            txtDonGia.Clear();
            txtThanhTien.Clear();
            txtTimKiem.Clear();
            cboPhieuNhap.SelectedIndex = -1;
            cboDichVu.SelectedIndex    = -1;
            lblTongTien.Text = "Tổng tiền phiếu: 0 VNĐ";
            _soLuongCu = 0;
            LoadData();
        }

        private void Dgv_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var r = dgv.Rows[e.RowIndex];

            txtMaCTPN.Text    = V(r, "MaCTPN");
            txtSoLuong.Text   = V(r, "SoLuong");
            txtDonGia.Text    = V(r, "DonGiaNhap");
            txtThanhTien.Text = V(r, "ThanhTien");

            // Lưu số lượng cũ cho nghiệp vụ Sửa tồn kho
            int.TryParse(V(r, "SoLuong"), out _soLuongCu);

            object? maPN = r.Cells["MaPN"].Value;
            if (maPN != null && maPN != DBNull.Value)
                cboPhieuNhap.SelectedValue = Convert.ToInt32(maPN);

            object? maDV = r.Cells["MaDV"].Value;
            if (maDV != null && maDV != DBNull.Value)
                cboDichVu.SelectedValue = Convert.ToInt32(maDV);

            HienThiTongTien();
        }
    }
}
