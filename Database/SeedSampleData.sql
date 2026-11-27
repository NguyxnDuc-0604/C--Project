/*
  Du lieu mau cho QuanLyKhachSan.
  Co the chay lai: moi dong chi duoc them neu khoa chinh chua ton tai.
*/
USE QuanLyKhachSan;
GO

-- 3 loai phong
IF NOT EXISTS (SELECT 1 FROM dbo.LOAI_PHONG WHERE MaLoai = 1)
    INSERT dbo.LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong)
    VALUES (1, N'Phòng đơn', 350000, 1, N'Phòng một giường đơn', 4);
IF NOT EXISTS (SELECT 1 FROM dbo.LOAI_PHONG WHERE MaLoai = 2)
    INSERT dbo.LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong)
    VALUES (2, N'Phòng đôi', 550000, 2, N'Phòng hai khách, giường đôi', 4);
IF NOT EXISTS (SELECT 1 FROM dbo.LOAI_PHONG WHERE MaLoai = 3)
    INSERT dbo.LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong)
    VALUES (3, N'Phòng VIP', 1200000, 3, N'Phòng cao cấp, có ban công', 2);
GO

-- 3 nhan vien
IF NOT EXISTS (SELECT 1 FROM dbo.NHAN_VIEN WHERE MaNV = 1)
    INSERT dbo.NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau)
    VALUES (1, N'Nguyễn Minh Anh', N'0901000001', N'Lễ tân', N'anh.nm', N'123456');
IF NOT EXISTS (SELECT 1 FROM dbo.NHAN_VIEN WHERE MaNV = 2)
    INSERT dbo.NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau)
    VALUES (2, N'Trần Thu Hà', N'0901000002', N'Quản lý', N'ha.tt', N'123456');
IF NOT EXISTS (SELECT 1 FROM dbo.NHAN_VIEN WHERE MaNV = 3)
    INSERT dbo.NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau)
    VALUES (3, N'Lê Quốc Bảo', N'0901000003', N'Nhân viên', N'bao.lq', N'123456');
GO

-- 4 khach hang
IF NOT EXISTS (SELECT 1 FROM dbo.KHACH_HANG WHERE MaKH = 1)
    INSERT dbo.KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi)
    VALUES (1, N'Phạm Ngọc Lan', N'079201000001', N'0912000001', N'lan.pham@example.com', N'TP. Hồ Chí Minh');
IF NOT EXISTS (SELECT 1 FROM dbo.KHACH_HANG WHERE MaKH = 2)
    INSERT dbo.KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi)
    VALUES (2, N'Nguyễn Hoàng Long', N'079201000002', N'0912000002', N'long.nguyen@example.com', N'Hà Nội');
IF NOT EXISTS (SELECT 1 FROM dbo.KHACH_HANG WHERE MaKH = 3)
    INSERT dbo.KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi)
    VALUES (3, N'Trần Mỹ Duyên', N'079201000003', N'0912000003', N'duyen.tran@example.com', N'Đà Nẵng');
IF NOT EXISTS (SELECT 1 FROM dbo.KHACH_HANG WHERE MaKH = 4)
    INSERT dbo.KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi)
    VALUES (4, N'Võ Thành Đạt', N'079201000004', N'0912000004', N'dat.vo@example.com', N'Cần Thơ');
GO

-- 10 phong
IF NOT EXISTS (SELECT 1 FROM dbo.PHONG WHERE MaPhong = 101) INSERT dbo.PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (101, 1, N'P101', N'Tầng 1', N'Trống');
IF NOT EXISTS (SELECT 1 FROM dbo.PHONG WHERE MaPhong = 102) INSERT dbo.PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (102, 1, N'P102', N'Tầng 1', N'Trống');
IF NOT EXISTS (SELECT 1 FROM dbo.PHONG WHERE MaPhong = 103) INSERT dbo.PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (103, 1, N'P103', N'Tầng 1', N'Trống');
IF NOT EXISTS (SELECT 1 FROM dbo.PHONG WHERE MaPhong = 104) INSERT dbo.PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (104, 1, N'P104', N'Tầng 1', N'Trống');
IF NOT EXISTS (SELECT 1 FROM dbo.PHONG WHERE MaPhong = 201) INSERT dbo.PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (201, 2, N'P201', N'Tầng 2', N'Trống');
IF NOT EXISTS (SELECT 1 FROM dbo.PHONG WHERE MaPhong = 202) INSERT dbo.PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (202, 2, N'P202', N'Tầng 2', N'Trống');
IF NOT EXISTS (SELECT 1 FROM dbo.PHONG WHERE MaPhong = 203) INSERT dbo.PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (203, 2, N'P203', N'Tầng 2', N'Trống');
IF NOT EXISTS (SELECT 1 FROM dbo.PHONG WHERE MaPhong = 204) INSERT dbo.PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (204, 2, N'P204', N'Tầng 2', N'Trống');
IF NOT EXISTS (SELECT 1 FROM dbo.PHONG WHERE MaPhong = 301) INSERT dbo.PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (301, 3, N'P301', N'Tầng 3', N'Trống');
IF NOT EXISTS (SELECT 1 FROM dbo.PHONG WHERE MaPhong = 302) INSERT dbo.PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (302, 3, N'P302', N'Tầng 3', N'Trống');
GO

-- 3 dich vu
IF NOT EXISTS (SELECT 1 FROM dbo.DICH_VU WHERE MaDV = 1) INSERT dbo.DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (1, N'Giặt ủi', 50000, N'Kg', N'Dịch vụ giặt và ủi quần áo', 30);
IF NOT EXISTS (SELECT 1 FROM dbo.DICH_VU WHERE MaDV = 2) INSERT dbo.DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (2, N'Ăn sáng', 80000, N'Suất', N'Bữa sáng tại khách sạn', 40);
IF NOT EXISTS (SELECT 1 FROM dbo.DICH_VU WHERE MaDV = 3) INSERT dbo.DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (3, N'Nước suối', 15000, N'Chai', N'Nước suối đóng chai', 100);
GO

-- 1 nha cung cap va 1 phieu nhap kem chi tiet
IF NOT EXISTS (SELECT 1 FROM dbo.NHA_CUNG_CAP WHERE MaNCC = 1)
BEGIN
    SET IDENTITY_INSERT dbo.NHA_CUNG_CAP ON;
    INSERT dbo.NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email)
    VALUES (1, N'Công ty Tiện ích An Phát', N'0283000001', N'TP. Hồ Chí Minh', N'lienhe@anphat.example');
    SET IDENTITY_INSERT dbo.NHA_CUNG_CAP OFF;
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.PHIEU_NHAP WHERE MaPN = 1)
BEGIN
    SET IDENTITY_INSERT dbo.PHIEU_NHAP ON;
    INSERT dbo.PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (1, 1, 2, '2026-11-20');
    SET IDENTITY_INSERT dbo.PHIEU_NHAP OFF;
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.CT_PHIEU_NHAP WHERE MaCTPN = 1)
BEGIN
    SET IDENTITY_INSERT dbo.CT_PHIEU_NHAP ON;
    INSERT dbo.CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (1, 1, 3, 20, 8000);
    SET IDENTITY_INSERT dbo.CT_PHIEU_NHAP OFF;
END;
GO

-- 1 dat phong, 1 chi tiet dat phong, 1 hoa don, 1 chi tiet hoa don
IF NOT EXISTS (SELECT 1 FROM dbo.DAT_PHONG WHERE MaDP = 1)
    INSERT dbo.DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai)
    VALUES (1, 1, 1, '2026-11-26', '2026-12-01', '2026-12-03', N'Đã xác nhận');
IF NOT EXISTS (SELECT 1 FROM dbo.CHI_TIET_DAT_PHONG WHERE MaCTDP = 1)
    INSERT dbo.CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (1, 1, 101, 350000);
IF NOT EXISTS (SELECT 1 FROM dbo.HOA_DON WHERE MaHD = 1)
    INSERT dbo.HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu)
    VALUES (1, 1, 1, '2026-11-26', 15000, N'Hóa đơn mẫu');
IF NOT EXISTS (SELECT 1 FROM dbo.CHI_TIET_HOA_DON WHERE MaCTHD = 1)
    INSERT dbo.CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien)
    VALUES (1, 1, 3, 1, 15000, 15000);
GO

PRINT N'Đã nạp dữ liệu mẫu. Các mã được dùng: loại phòng 1-3, nhân viên 1-3, khách hàng 1-4, phòng 101-302, dịch vụ 1-3.';
GO
