-- =============================================
-- Script tạo Database + TẤT CẢ các bảng cần thiết
-- Server: localhost\SQLEXPRESS
-- =============================================

-- Tạo database nếu chưa có
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'QuanLyKhachSan')
BEGIN
    CREATE DATABASE QuanLyKhachSan;
END
GO

USE QuanLyKhachSan;
GO

-- ========== BẢNG CỦA ĐỨC ==========

-- Bảng LOAI_PHONG
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LOAI_PHONG')
BEGIN
    CREATE TABLE LOAI_PHONG (
        MaLoaiPhong INT PRIMARY KEY,
        TenLoaiPhong NVARCHAR(100) NOT NULL,
        MoTa NVARCHAR(200) NULL
    );
    
    INSERT INTO LOAI_PHONG VALUES
    (1, N'Phòng đơn', N'Phòng 1 giường đơn'),
    (2, N'Phòng đôi', N'Phòng 1 giường đôi'),
    (3, N'Phòng VIP', N'Phòng cao cấp');
END
GO

-- Bảng NHAN_VIEN
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'NHAN_VIEN')
BEGIN
    CREATE TABLE NHAN_VIEN (
        MaNV INT PRIMARY KEY,
        HoTen NVARCHAR(100) NOT NULL,
        SDT NVARCHAR(15) NULL,
        ChucVu NVARCHAR(50) NULL
    );
    
    INSERT INTO NHAN_VIEN VALUES
    (1, N'Nguyễn Văn A', '0901234567', N'Lễ tân'),
    (2, N'Trần Thị B', '0912345678', N'Quản lý'),
    (3, N'Lê Văn C', '0923456789', N'Nhân viên');
END
GO

-- Bảng PHONG
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PHONG')
BEGIN
    CREATE TABLE PHONG (
        MaPhong INT PRIMARY KEY,
        TenPhong NVARCHAR(100) NOT NULL,
        MaLoaiPhong INT NULL,
        GiaPhong DECIMAL(18,2) NULL,
        TrangThai NVARCHAR(50) NULL,
        
        CONSTRAINT FK_PHONG_LOAI_PHONG
            FOREIGN KEY (MaLoaiPhong) REFERENCES LOAI_PHONG(MaLoaiPhong)
    );
    
    INSERT INTO PHONG VALUES
    (1, N'P101', 1, 300000, N'Trống'),
    (2, N'P102', 1, 300000, N'Trống'),
    (3, N'P201', 2, 500000, N'Trống'),
    (4, N'P202', 2, 500000, N'Đang sử dụng'),
    (5, N'P301', 3, 1000000, N'Trống');
END
GO

-- ========== BẢNG CỦA NHẬT ANH ==========

-- Bảng KHACH_HANG
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'KHACH_HANG')
BEGIN
    CREATE TABLE KHACH_HANG (
        MaKH INT PRIMARY KEY,
        HoTen NVARCHAR(100) NOT NULL,
        CCCD NVARCHAR(20) NULL,
        SDT NVARCHAR(15) NULL,
        Email NVARCHAR(100) NULL,
        DiaChi NVARCHAR(200) NULL
    );
    
    INSERT INTO KHACH_HANG VALUES
    (1, N'Phạm Minh D', '012345678901', '0934567890', 'pmd@gmail.com', N'TP.HCM'),
    (2, N'Hoàng Thị E', '012345678902', '0945678901', 'hte@gmail.com', N'Hà Nội'),
    (3, N'Võ Văn F', '012345678903', '0956789012', 'vvf@gmail.com', N'Đà Nẵng');
END
GO

-- Bảng DAT_PHONG
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DAT_PHONG')
BEGIN
    CREATE TABLE DAT_PHONG (
        MaDP INT PRIMARY KEY,
        MaKH INT NOT NULL,
        MaNV INT NOT NULL,
        NgayDat DATETIME NOT NULL DEFAULT GETDATE(),
        NgayNhan DATETIME NOT NULL,
        NgayTra DATETIME NOT NULL,
        TrangThai NVARCHAR(50) NOT NULL,
        
        CONSTRAINT FK_DAT_PHONG_KHACH_HANG
            FOREIGN KEY (MaKH) REFERENCES KHACH_HANG(MaKH),
        CONSTRAINT FK_DAT_PHONG_NHAN_VIEN
            FOREIGN KEY (MaNV) REFERENCES NHAN_VIEN(MaNV)
    );
    
    INSERT INTO DAT_PHONG VALUES
    (1, 1, 1, '2026-11-20', '2026-11-22', '2026-11-25', N'Đã xác nhận'),
    (2, 2, 2, '2026-11-21', '2026-11-23', '2026-11-26', N'Chờ xác nhận');
END
GO

-- Bảng CHI_TIET_DAT_PHONG
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CHI_TIET_DAT_PHONG')
BEGIN
    CREATE TABLE CHI_TIET_DAT_PHONG (
        MaCTDP INT PRIMARY KEY,
        MaDP INT NOT NULL,
        MaPhong INT NOT NULL,
        DonGia DECIMAL(18,2) NOT NULL,
        
        CONSTRAINT FK_CTDP_DAT_PHONG
            FOREIGN KEY (MaDP) REFERENCES DAT_PHONG(MaDP),
        CONSTRAINT FK_CTDP_PHONG
            FOREIGN KEY (MaPhong) REFERENCES PHONG(MaPhong)
    );
    
    INSERT INTO CHI_TIET_DAT_PHONG VALUES
    (1, 1, 1, 300000),
    (2, 1, 3, 500000),
    (3, 2, 5, 1000000);
END
GO

-- ========== BẢNG CỦA KHOA ==========

-- Bảng DICH_VU
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DICH_VU')
BEGIN
    CREATE TABLE DICH_VU (
        MaDV INT PRIMARY KEY,
        TenDV NVARCHAR(100) NOT NULL,
        DonGia DECIMAL(18, 2) NOT NULL,
        DonViTinh NVARCHAR(50) NULL,
        MoTa NVARCHAR(200) NULL
    );
    
    INSERT INTO DICH_VU VALUES
    (1, N'Giặt ủi', 50000, N'Kg', N'Dịch vụ giặt ủi quần áo'),
    (2, N'Ăn sáng', 80000, N'Suất', N'Buffet sáng tại nhà hàng'),
    (3, N'Thuê xe máy', 150000, N'Ngày', N'Thuê xe máy tự lái'),
    (4, N'Spa & Massage', 300000, N'Lần', N'Dịch vụ spa thư giãn'),
    (5, N'Minibar', 100000, N'Lần', N'Đồ uống trong phòng');
END
GO

-- Bảng HOA_DON
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'HOA_DON')
BEGIN
    CREATE TABLE HOA_DON (
        MaHD INT PRIMARY KEY,
        MaDP INT NOT NULL,
        MaNV INT NOT NULL,
        NgayLap DATETIME NOT NULL DEFAULT GETDATE(),
        TongTien DECIMAL(18, 2) NOT NULL DEFAULT 0,
        GhiChu NVARCHAR(200) NULL,
        
        CONSTRAINT FK_HOA_DON_DAT_PHONG
            FOREIGN KEY (MaDP) REFERENCES DAT_PHONG(MaDP),
        CONSTRAINT FK_HOA_DON_NHAN_VIEN
            FOREIGN KEY (MaNV) REFERENCES NHAN_VIEN(MaNV)
    );
    
    INSERT INTO HOA_DON VALUES
    (1, 1, 1, '2026-11-25', 880000, N'Thanh toán khi trả phòng'),
    (2, 2, 2, '2026-11-26', 1000000, NULL);
END
GO

-- Bảng CHI_TIET_HOA_DON
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CHI_TIET_HOA_DON')
BEGIN
    CREATE TABLE CHI_TIET_HOA_DON (
        MaCTHD INT PRIMARY KEY,
        MaHD INT NOT NULL,
        MaDV INT NOT NULL,
        SoLuong INT NOT NULL DEFAULT 1,
        DonGia DECIMAL(18, 2) NOT NULL,
        ThanhTien DECIMAL(18, 2) NOT NULL,
        
        CONSTRAINT FK_CTHD_HOA_DON
            FOREIGN KEY (MaHD) REFERENCES HOA_DON(MaHD),
        CONSTRAINT FK_CTHD_DICH_VU
            FOREIGN KEY (MaDV) REFERENCES DICH_VU(MaDV)
    );
    
    INSERT INTO CHI_TIET_HOA_DON VALUES
    (1, 1, 1, 2, 50000, 100000),
    (2, 1, 2, 3, 80000, 240000),
    (3, 1, 4, 1, 300000, 300000),
    (4, 2, 2, 2, 80000, 160000),
    (5, 2, 5, 1, 100000, 100000);
END
GO

PRINT N'=== Hoàn tất tạo tất cả bảng và dữ liệu mẫu! ===';
GO
