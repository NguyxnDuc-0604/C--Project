using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyKhachSan.BUS;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.Forms
{
    public class FrmHoaDon : Form
    {
        private readonly HoaDonBUS bus = new();
        private readonly ChiTietHoaDonBUS cthdBus = new();

        // Biến static hỗ trợ tìm kiếm và điền form (Yêu cầu Tuần 7 số 4)
        public static int MaDPDuocChon = 0;

        private TextBox txtMaHD = new();
        private ComboBox cboDatPhong = new();
        private ComboBox cboNhanVien = new();
        private DateTimePicker dtpNgayLap = new();
        private TextBox txtTongTien = new();
        private TextBox txtGhiChu = new();
        private TextBox txtTimKiem = new();

        // Mô hình Main - Sub (Master - Detail)
        private DataGridView dgv = new();           // Master (Hóa đơn)
        private DataGridView dgvChiTiet = new();    // Sub (Chi tiết dịch vụ của hóa đơn)
        private Label lblSubTitle = new();

        public FrmHoaDon()
        {
            Text = "QUẢN LÝ HÓA ĐƠN - MÔ HÌNH MAIN/SUB (1-N)";
            Size = new Size(1120, 830);
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
                Text = "QUẢN LÝ HÓA ĐƠN & THANH TOÁN (MAIN - SUB)",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Location = new Point(310, 15),
                Size = new Size(550, 35)
            };
            Controls.Add(lblTitle);

            // Các nhãn và ô nhập Master
            TaoLabel("Mã hóa đơn:", 30, 65);
            TaoTextBox(txtMaHD, 160, 63, 300);

            TaoLabel("Mã đặt phòng:", 30, 105);
            TaoComboBox(cboDatPhong, 160, 103, 220);
            Button btnChonDP = new Button
            {
                Text = "Tra cứu...",
                Location = new Point(390, 102),
                Size = new Size(70, 29),
                Font = new Font("Arial", 9)
            };
            btnChonDP.Click += (s, e) => TraCuuDatPhong();
            Controls.Add(btnChonDP);

            TaoLabel("Nhân viên:", 30, 145);
            TaoComboBox(cboNhanVien, 160, 143, 300);

            TaoLabel("Ngày lập:", 530, 65);
            dtpNgayLap.Location = new Point(660, 63);
            dtpNgayLap.Size = new Size(300, 28);
            dtpNgayLap.Format = DateTimePickerFormat.Short;
            Controls.Add(dtpNgayLap);

            TaoLabel("Tổng tiền:", 530, 105);
            TaoTextBox(txtTongTien, 660, 103, 300);
            txtTongTien.ReadOnly = true;
            txtTongTien.BackColor = Color.LightGray;

            TaoLabel("Ghi chú:", 530, 145);
            TaoTextBox(txtGhiChu, 660, 143, 300);

            // Nút tính tổng tiền từ chi tiết
            Button btnTinhTong = new Button
            {
                Text = "Tính tổng từ CTHD",
                Location = new Point(530, 185),
                Size = new Size(140, 32),
                BackColor = Color.LightGreen,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            btnTinhTong.Click += (s, e) => TinhTongTien();
            Controls.Add(btnTinhTong);

            // Các nút chức năng chính
            TaoButton("Thêm", 30, 185, Them);
            TaoButton("Sửa", 140, 185, Sua);
            TaoButton("Xóa", 250, 185, Xoa);
            TaoButton("Làm mới", 360, 185, LamMoi);

            // Tìm kiếm cơ bản & nâng cao
            Label lblSearch = new Label
            {
                Text = "Tìm kiếm:",
                Location = new Point(685, 190),
                Size = new Size(70, 25)
            };
            Controls.Add(lblSearch);

            txtTimKiem.Location = new Point(760, 187);
            txtTimKiem.Size = new Size(180, 28);
            Controls.Add(txtTimKiem);

            TaoButton("Tìm", 950, 185, TimKiem, 60);
            TaoButton("Lọc nâng cao", 1020, 185, MoLocNangCao, 100);

            // ================= 1. BẢNG MASTER: HÓA ĐƠN =================
            Label lblMasterTitle = new Label
            {
                Text = "📁 DANH SÁCH HÓA ĐƠN (MASTER):",
                Location = new Point(25, 225),
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.DarkSlateBlue,
                AutoSize = true
            };
            Controls.Add(lblMasterTitle);

            dgv.Location = new Point(25, 250);
            dgv.Size = new Size(1055, 210);
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowHeadersVisible = false;
            dgv.CellClick += Dgv_CellClick;
            Controls.Add(dgv);

            // ================= 2. BẢNG SUB: CHI TIẾT HÓA ĐƠN (1-N) =================
            lblSubTitle = new Label
            {
                Text = "📋 CHI TIẾT DỊCH VỤ CỦA HÓA ĐƠN ĐANG CHỌN (SUB 1-N): Nhấp chọn 1 hóa đơn ở trên để xem",
                Location = new Point(25, 470),
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Crimson,
                AutoSize = true
            };
            Controls.Add(lblSubTitle);

            dgvChiTiet.Location = new Point(25, 495);
            dgvChiTiet.Size = new Size(1055, 200);
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTiet.MultiSelect = false;
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTiet.RowHeadersVisible = false;
            dgvChiTiet.BackgroundColor = Color.WhiteSmoke;
            Controls.Add(dgvChiTiet);

            // ================= 3. CÁC NÚT BỔ TRỢ ĐIỂM CỘNG =================
            Button btnXuatExcel = new Button
            {
                Text = "📊 Xuất Excel (CSV)",
                Location = new Point(25, 715),
                Size = new Size(180, 36),
                BackColor = Color.ForestGreen,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnXuatExcel.Click += (s, e) => FrmThongKe.XuatDuLieuCSV(dgv, "DanhSachHoaDon");
            Controls.Add(btnXuatExcel);

            Button btnInHoaDon = new Button
            {
                Text = "🖨️ In / Xuất hóa đơn chi tiết",
                Location = new Point(220, 715),
                Size = new Size(240, 36),
                BackColor = Color.DarkSlateBlue,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnInHoaDon.Click += (s, e) => InHoaDonChiTiet();
            Controls.Add(btnInHoaDon);
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

        private void TaoButton(string text, int x, int y, Action action, int width = 100)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, 32),
                BackColor = Color.LightSteelBlue,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            btn.Click += (s, e) => action();
            Controls.Add(btn);
        }

        private void LoadComboBox()
        {
            try
            {
                cboDatPhong.DataSource = bus.GetDanhSachDatPhong();
                cboDatPhong.DisplayMember = "MaDP";
                cboDatPhong.ValueMember = "MaDP";
                cboDatPhong.SelectedIndex = -1;

                cboNhanVien.DataSource = bus.GetDanhSachNhanVien();
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
                dgv.DataSource = bus.GetAll();
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
                decimal tong = bus.TinhTongTien(maHD);
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
                decimal.TryParse(txtTongTien.Text.Replace(",", "").Replace(".", ""), out tongTien);
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
                bus.Them(hd);
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
                bus.Sua(hd);
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

            if (MessageBox.Show("Bạn có chắc muốn xóa hóa đơn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                bus.Xoa(maHD);
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
                dgv.DataSource = bus.TimKiem(txtTimKiem.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }

        private void MoLocNangCao()
        {
            Form frmFilter = new Form
            {
                Text = "TÌM KIẾM NÂNG CAO (AND, OR, KHOẢNG GIÁ, KHOẢNG NGÀY)",
                Size = new Size(450, 320),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label l1 = new Label { Text = "Từ khóa:", Location = new Point(20, 20), AutoSize = true };
            TextBox txtKW = new TextBox { Text = txtTimKiem.Text, Location = new Point(140, 18), Width = 250 };

            Label l2 = new Label { Text = "Tổng tiền Từ:", Location = new Point(20, 60), AutoSize = true };
            NumericUpDown numTuGia = new NumericUpDown { Location = new Point(140, 58), Width = 110, Maximum = 1000000000, Increment = 50000 };

            Label l3 = new Label { Text = "Đến:", Location = new Point(260, 60), AutoSize = true };
            NumericUpDown numDenGia = new NumericUpDown { Location = new Point(295, 58), Width = 95, Maximum = 1000000000, Value = 100000000, Increment = 50000 };

            Label l4 = new Label { Text = "Từ ngày:", Location = new Point(20, 100), AutoSize = true };
            DateTimePicker dtpTu = new DateTimePicker { Location = new Point(140, 98), Width = 120, Format = DateTimePickerFormat.Short, Value = new DateTime(2020, 1, 1) };

            Label l5 = new Label { Text = "Đến ngày:", Location = new Point(20, 140), AutoSize = true };
            DateTimePicker dtpDen = new DateTimePicker { Location = new Point(140, 138), Width = 120, Format = DateTimePickerFormat.Short, Value = DateTime.Now };

            Button btnLoc = new Button { Text = "Áp dụng lọc", Location = new Point(140, 200), Size = new Size(120, 36), BackColor = Color.DodgerBlue, ForeColor = Color.White, Font = new Font("Arial", 9, FontStyle.Bold) };
            btnLoc.Click += (s, e) =>
            {
                try
                {
                    decimal? giaTu = numTuGia.Value > 0 ? numTuGia.Value : null;
                    decimal? giaDen = numDenGia.Value > 0 ? numDenGia.Value : null;
                    dgv.DataSource = bus.TimKiemNangCao(txtKW.Text.Trim(), giaTu, giaDen, dtpTu.Value.Date, dtpDen.Value.Date);
                    frmFilter.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            };

            frmFilter.Controls.AddRange(new Control[] { l1, txtKW, l2, numTuGia, l3, numDenGia, l4, dtpTu, l5, dtpDen, btnLoc });
            frmFilter.ShowDialog();
        }

        private void TraCuuDatPhong()
        {
            // Nút hỗ trợ tìm kiếm sử dụng biến static (Yêu cầu Tuần 7 số 4)
            Form frmLookup = new Form
            {
                Text = "TRA CỨU PHIẾU ĐẶT PHÒNG",
                Size = new Size(600, 400),
                StartPosition = FormStartPosition.CenterParent
            };

            DataGridView dgvLookup = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 300,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                DataSource = bus.GetDanhSachDatPhong()
            };
            frmLookup.Controls.Add(dgvLookup);

            Button btnChon = new Button
            {
                Text = "Chọn phiếu đặt phòng",
                Dock = DockStyle.Bottom,
                Height = 45,
                BackColor = Color.ForestGreen,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            btnChon.Click += (s, e) =>
            {
                if (dgvLookup.CurrentRow != null)
                {
                    MaDPDuocChon = Convert.ToInt32(dgvLookup.CurrentRow.Cells["MaDP"].Value);
                    cboDatPhong.SelectedValue = MaDPDuocChon;
                    frmLookup.Close();
                }
            };
            frmLookup.Controls.Add(btnChon);
            frmLookup.ShowDialog();
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
            lblSubTitle.Text = "📋 CHI TIẾT DỊCH VỤ CỦA HÓA ĐƠN ĐANG CHỌN (SUB 1-N): Nhấp chọn 1 hóa đơn ở trên để xem";
            dgvChiTiet.DataSource = null;
            LoadData();
        }

        // Sự kiện CellClick Master -> Tải Sub 1-n
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
                dtpNgayLap.Value = Convert.ToDateTime(row.Cells["NgayLap"].Value);

            // TỰ ĐỘNG TẢI DANH SÁCH CHI TIẾT DỊCH VỤ CỦA HÓA ĐƠN ĐÓ (MÔ HÌNH MAIN - SUB 1-N)
            if (int.TryParse(txtMaHD.Text.Trim(), out int maHD))
            {
                try
                {
                    DataTable dtSub = cthdBus.GetByMaHD(maHD);
                    dgvChiTiet.DataSource = dtSub;
                    lblSubTitle.Text = $"📋 CHI TIẾT DỊCH VỤ CỦA HÓA ĐƠN HD_{maHD:D4} (MÔ HÌNH MAIN - SUB 1-N): {dtSub.Rows.Count} DỊCH VỤ ĐÃ SỬ DỤNG";
                }
                catch
                {
                    dgvChiTiet.DataSource = null;
                }
            }
        }

        private void InHoaDonChiTiet()
        {
            if (!int.TryParse(txtMaHD.Text.Trim(), out int maHD))
            {
                MessageBox.Show("Vui lòng chọn hoặc nhập một hóa đơn để in!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataTable dtCT = cthdBus.GetByMaHD(maHD);

                string tenNV = cboNhanVien.Text;
                string maDP = cboDatPhong.Text;
                string ngayLap = dtpNgayLap.Value.ToString("dd/MM/yyyy");
                string tongTien = txtTongTien.Text;
                string ghiChu = txtGhiChu.Text;

                System.Text.StringBuilder sb = new();
                sb.AppendLine("==========================================================================");
                sb.AppendLine("                    KHÁCH SẠN QUẢN LÝ NGHỈ DƯỠNG SGU");
                sb.AppendLine("                       HÓA ĐƠN THANH TOÁN DỊCH VỤ");
                sb.AppendLine("==========================================================================");
                sb.AppendLine($"Mã hóa đơn    : HD_{maHD:D4}");
                sb.AppendLine($"Mã đặt phòng  : {maDP}");
                sb.AppendLine($"Nhân viên lập : {tenNV}");
                sb.AppendLine($"Ngày lập      : {ngayLap}");
                sb.AppendLine("--------------------------------------------------------------------------");
                sb.AppendLine(string.Format("{0,-6} | {1,-30} | {2,-8} | {3,-12} | {4,-15}", "STT", "Tên dịch vụ", "SL", "Đơn giá", "Thành tiền"));
                sb.AppendLine("--------------------------------------------------------------------------");

                int stt = 1;
                decimal tongCongCT = 0;
                foreach (DataRow r in dtCT.Rows)
                {
                    string tenDV = r["TenDV"].ToString() ?? "";
                    int sl = Convert.ToInt32(r["SoLuong"]);
                    decimal donGia = Convert.ToDecimal(r["DonGia"]);
                    decimal thanhTien = Convert.ToDecimal(r["ThanhTien"]);
                    tongCongCT += thanhTien;

                    sb.AppendLine(string.Format("{0,-6} | {1,-30} | {2,-8} | {3,-12:N0} | {4,-15:N0}", stt++, tenDV, sl, donGia, thanhTien));
                }

                sb.AppendLine("--------------------------------------------------------------------------");
                sb.AppendLine($"TỔNG CỘNG TIỀN DỊCH VỤ : {tongCongCT:N0} VNĐ");
                sb.AppendLine($"TỔNG TIỀN HÓA ĐƠN      : {tongTien} VNĐ");
                if (!string.IsNullOrWhiteSpace(ghiChu))
                {
                    sb.AppendLine($"Ghi chú                : {ghiChu}");
                }
                sb.AppendLine("==========================================================================");
                sb.AppendLine("                 Cảm ơn Quý khách & Hẹn gặp lại!");
                sb.AppendLine("==========================================================================");

                Form frmPreview = new Form
                {
                    Text = $"XEM TRƯỚC HÓA ĐƠN HD_{maHD}",
                    Size = new Size(680, 580),
                    StartPosition = FormStartPosition.CenterParent,
                    BackColor = Color.White
                };

                RichTextBox rtb = new RichTextBox
                {
                    Text = sb.ToString(),
                    Location = new Point(20, 20),
                    Size = new Size(625, 450),
                    Font = new Font("Consolas", 10F, FontStyle.Regular),
                    ReadOnly = true,
                    BackColor = Color.Ivory
                };
                frmPreview.Controls.Add(rtb);

                Button btnLuu = new Button
                {
                    Text = "💾 Lưu file hóa đơn (.txt / .doc)",
                    Location = new Point(20, 485),
                    Size = new Size(240, 38),
                    BackColor = Color.SeaGreen,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                };
                btnLuu.Click += (s, e) =>
                {
                    using SaveFileDialog sfd = new SaveFileDialog
                    {
                        Filter = "Text Document (*.txt)|*.txt|Word Document (*.doc)|*.doc",
                        FileName = $"HoaDon_{maHD}_{DateTime.Now:yyyyMMdd}.txt"
                    };
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        System.IO.File.WriteAllText(sfd.FileName, sb.ToString(), System.Text.Encoding.UTF8);
                        MessageBox.Show("Lưu hóa đơn thành công!\n" + sfd.FileName, "Thông báo");
                    }
                };
                frmPreview.Controls.Add(btnLuu);

                Button btnDong = new Button
                {
                    Text = "Đóng",
                    Location = new Point(510, 485),
                    Size = new Size(135, 38),
                    Font = new Font("Segoe UI", 10F, FontStyle.Regular)
                };
                btnDong.Click += (s, e) => frmPreview.Close();
                frmPreview.Controls.Add(btnDong);

                frmPreview.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo hóa đơn in: " + ex.Message, "Lỗi");
            }
        }
    }
}
