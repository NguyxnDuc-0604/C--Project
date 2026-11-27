using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using QuanLyKhachSan.BUS;

namespace QuanLyKhachSan.Forms
{
    public class FrmThongKe : Form
    {
        private readonly ThongKeBUS _bus = new();

        private TabControl tabControl = null!;
        
        // Tab 1: Thu Chi
        private DateTimePicker dtpTuNgay = null!;
        private DateTimePicker dtpDenNgay = null!;
        private Label lblTongThu = null!;
        private Label lblTongChi = null!;
        private Label lblLoiNhuan = null!;
        private DataGridView dgvThuChi = null!;

        // Tab 2: Nhan Vien
        private NumericUpDown numNamNV = null!;
        private DataGridView dgvNhanVien = null!;

        // Tab 3: Khach Hang
        private NumericUpDown numNamKH = null!;
        private DataGridView dgvKhachHang = null!;

        // Tab 4: Dich Vu
        private NumericUpDown numNamDV = null!;
        private DataGridView dgvDichVu = null!;

        public FrmThongKe()
        {
            Text = "BÁO CÁO & THỐNG KÊ DOANH THU KHÁCH SẠN";
            Size = new Size(950, 650);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;
            Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            KhoiTaoGiaoDien();
        }

        private void KhoiTaoGiaoDien()
        {
            Label lblHeader = new Label
            {
                Text = "BÁO CÁO & THỐNG KÊ DOANH THU",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.DarkSlateBlue,
                AutoSize = true,
                Location = new Point(280, 15)
            };
            Controls.Add(lblHeader);

            tabControl = new TabControl
            {
                Location = new Point(20, 60),
                Size = new Size(895, 530),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            tabControl.TabPages.Add(TaoTabThuChi());
            tabControl.TabPages.Add(TaoTabNhanVien());
            tabControl.TabPages.Add(TaoTabKhachHang());
            tabControl.TabPages.Add(TaoTabDichVu());

            Controls.Add(tabControl);
        }

        // ================= TAB 1: THU - CHI - LỢI NHUẬN =================
        private TabPage TaoTabThuChi()
        {
            TabPage tab = new TabPage("Tổng Thu - Tổng Chi - Lợi Nhuận")
            {
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                BackColor = Color.White
            };

            Label lbl1 = new Label { Text = "Từ ngày:", Location = new Point(20, 25), AutoSize = true };
            dtpTuNgay = new DateTimePicker
            {
                Location = new Point(90, 22),
                Width = 140,
                Format = DateTimePickerFormat.Short,
                Value = new DateTime(DateTime.Now.Year, 1, 1)
            };

            Label lbl2 = new Label { Text = "Đến ngày:", Location = new Point(250, 25), AutoSize = true };
            dtpDenNgay = new DateTimePicker
            {
                Location = new Point(330, 22),
                Width = 140,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now
            };

            Button btnXem = new Button
            {
                Text = "Xem thống kê",
                Location = new Point(490, 20),
                Size = new Size(130, 32),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnXem.Click += (s, e) => TaiDuLieuThuChi();

            Button btnXuatExcel = new Button
            {
                Text = "Xuất Excel (CSV)",
                Location = new Point(630, 20),
                Size = new Size(140, 32),
                BackColor = Color.ForestGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnXuatExcel.Click += (s, e) => XuatDuLieuCSV(dgvThuChi, "ThongKe_ThuChi");

            // Panel Thẻ tóm tắt chỉ số
            Panel pnlTongThu = TaoTheChiSo("TỔNG THU (HÓA ĐƠN)", "0 VNĐ", Color.MediumSeaGreen, 20, 70, out lblTongThu);
            Panel pnlTongChi = TaoTheChiSo("TỔNG CHI (NHẬP HÀNG)", "0 VNĐ", Color.Crimson, 310, 70, out lblTongChi);
            Panel pnlLoiNhuan = TaoTheChiSo("LỢI NHUẬN RÒNG", "0 VNĐ", Color.RoyalBlue, 600, 70, out lblLoiNhuan);

            dgvThuChi = new DataGridView
            {
                Location = new Point(20, 180),
                Size = new Size(845, 290),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                BackgroundColor = Color.WhiteSmoke
            };

            tab.Controls.AddRange(new Control[] { lbl1, dtpTuNgay, lbl2, dtpDenNgay, btnXem, btnXuatExcel, pnlTongThu, pnlTongChi, pnlLoiNhuan, dgvThuChi });
            return tab;
        }

        private Panel TaoTheChiSo(string tieuDe, string giaTri, Color mau, int x, int y, out Label lblVal)
        {
            Panel p = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(265, 90),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.AliceBlue
            };

            Label lblT = new Label
            {
                Text = tieuDe,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.DimGray,
                Location = new Point(10, 10),
                AutoSize = true
            };

            lblVal = new Label
            {
                Text = giaTri,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = mau,
                Location = new Point(10, 40),
                AutoSize = true
            };

            p.Controls.Add(lblT);
            p.Controls.Add(lblVal);
            return p;
        }

        private void TaiDuLieuThuChi()
        {
            try
            {
                DataTable dt = _bus.ThongKeThuChi(dtpTuNgay.Value, dtpDenNgay.Value);
                dgvThuChi.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    decimal thu = Convert.ToDecimal(dt.Rows[0]["TongThu"]);
                    decimal chi = Convert.ToDecimal(dt.Rows[0]["TongChi"]);
                    decimal ln = Convert.ToDecimal(dt.Rows[0]["LoiNhuan"]);

                    lblTongThu.Text = $"{thu:N0} VNĐ";
                    lblTongChi.Text = $"{chi:N0} VNĐ";
                    lblLoiNhuan.Text = $"{ln:N0} VNĐ";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thống kê: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================= TAB 2: DOANH THU THEO NHÂN VIÊN =================
        private TabPage TaoTabNhanVien()
        {
            TabPage tab = new TabPage("Doanh thu theo Nhân viên")
            {
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                BackColor = Color.White
            };

            Label lbl = new Label { Text = "Năm thống kê:", Location = new Point(20, 25), AutoSize = true };
            numNamNV = new NumericUpDown
            {
                Location = new Point(130, 22),
                Width = 100,
                Minimum = 2000,
                Maximum = 2100,
                Value = DateTime.Now.Year
            };

            Button btnXem = new Button
            {
                Text = "Thống kê 4 Quý",
                Location = new Point(250, 20),
                Size = new Size(140, 32),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnXem.Click += (s, e) =>
            {
                try
                {
                    dgvNhanVien.DataSource = _bus.ThongKeHoaDonTheoNhanVien((int)numNamNV.Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            Button btnExcel = new Button
            {
                Text = "Xuất Excel (CSV)",
                Location = new Point(400, 20),
                Size = new Size(140, 32),
                BackColor = Color.ForestGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnExcel.Click += (s, e) => XuatDuLieuCSV(dgvNhanVien, $"DoanhThu_NhanVien_{numNamNV.Value}");

            dgvNhanVien = new DataGridView
            {
                Location = new Point(20, 70),
                Size = new Size(845, 410),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                BackgroundColor = Color.WhiteSmoke
            };

            tab.Controls.AddRange(new Control[] { lbl, numNamNV, btnXem, btnExcel, dgvNhanVien });
            return tab;
        }

        // ================= TAB 3: DOANH THU THEO KHÁCH HÀNG =================
        private TabPage TaoTabKhachHang()
        {
            TabPage tab = new TabPage("Doanh thu theo Khách hàng")
            {
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                BackColor = Color.White
            };

            Label lbl = new Label { Text = "Năm thống kê:", Location = new Point(20, 25), AutoSize = true };
            numNamKH = new NumericUpDown
            {
                Location = new Point(130, 22),
                Width = 100,
                Minimum = 2000,
                Maximum = 2100,
                Value = DateTime.Now.Year
            };

            Button btnXem = new Button
            {
                Text = "Thống kê 4 Quý",
                Location = new Point(250, 20),
                Size = new Size(140, 32),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnXem.Click += (s, e) =>
            {
                try
                {
                    dgvKhachHang.DataSource = _bus.ThongKeHoaDonTheoKhachHang((int)numNamKH.Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            Button btnExcel = new Button
            {
                Text = "Xuất Excel (CSV)",
                Location = new Point(400, 20),
                Size = new Size(140, 32),
                BackColor = Color.ForestGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnExcel.Click += (s, e) => XuatDuLieuCSV(dgvKhachHang, $"DoanhThu_KhachHang_{numNamKH.Value}");

            dgvKhachHang = new DataGridView
            {
                Location = new Point(20, 70),
                Size = new Size(845, 410),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                BackgroundColor = Color.WhiteSmoke
            };

            tab.Controls.AddRange(new Control[] { lbl, numNamKH, btnXem, btnExcel, dgvKhachHang });
            return tab;
        }

        // ================= TAB 4: TIÊU THỤ DỊCH VỤ =================
        private TabPage TaoTabDichVu()
        {
            TabPage tab = new TabPage("Doanh thu Dịch vụ theo Quý")
            {
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                BackColor = Color.White
            };

            Label lbl = new Label { Text = "Năm thống kê:", Location = new Point(20, 25), AutoSize = true };
            numNamDV = new NumericUpDown
            {
                Location = new Point(130, 22),
                Width = 100,
                Minimum = 2000,
                Maximum = 2100,
                Value = DateTime.Now.Year
            };

            Button btnXem = new Button
            {
                Text = "Thống kê 4 Quý",
                Location = new Point(250, 20),
                Size = new Size(140, 32),
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnXem.Click += (s, e) =>
            {
                try
                {
                    dgvDichVu.DataSource = _bus.ThongKeDichVuTheoQuy((int)numNamDV.Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            Button btnExcel = new Button
            {
                Text = "Xuất Excel (CSV)",
                Location = new Point(400, 20),
                Size = new Size(140, 32),
                BackColor = Color.ForestGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnExcel.Click += (s, e) => XuatDuLieuCSV(dgvDichVu, $"DoanhThu_DichVu_{numNamDV.Value}");

            dgvDichVu = new DataGridView
            {
                Location = new Point(20, 70),
                Size = new Size(845, 410),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                BackgroundColor = Color.WhiteSmoke
            };

            tab.Controls.AddRange(new Control[] { lbl, numNamDV, btnXem, btnExcel, dgvDichVu });
            return tab;
        }

        // ================= HÀM XUẤT CSV MỞ BẰNG EXCEL =================
        public static void XuatDuLieuCSV(DataGridView dgv, string tenMacDinh)
        {
            if (dgv.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "CSV File (*.csv)|*.csv|Excel Spreadsheet (*.xlsx)|*.xlsx",
                FileName = $"{tenMacDinh}_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();

                    // Ghi tiêu đề cột
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        sb.Append(dgv.Columns[i].HeaderText);
                        if (i < dgv.Columns.Count - 1) sb.Append(",");
                    }
                    sb.AppendLine();

                    // Ghi dữ liệu từng dòng
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (row.IsNewRow) continue;
                        for (int i = 0; i < dgv.Columns.Count; i++)
                        {
                            string val = row.Cells[i].Value?.ToString() ?? "";
                            val = val.Replace(",", " "); // tránh vỡ cột CSV
                            sb.Append(val);
                            if (i < dgv.Columns.Count - 1) sb.Append(",");
                        }
                        sb.AppendLine();
                    }

                    // Lưu với UTF-8 có BOM để Excel mở không bị lỗi font tiếng Việt
                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Xuất file thành công!\nĐường dẫn: " + sfd.FileName, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
