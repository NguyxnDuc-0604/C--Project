-- =============================================
-- Script tạo 3 bảng: DICH_VU, HOA_DON, CHI_TIET_HOA_DON
-- Chạy trên database QuanLyKhachSan (SQL Server)
-- =============================================

USE QuanLyKhachSan;
GO

-- 1. Bảng DICH_VU
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DICH_VU')
BEGIN
    CREATE TABLE DICH_VU (
        MaDV        INT PRIMARY KEY,
        TenDV       NVARCHAR(100) NOT NULL,
        DonGia      DECIMAL(18, 2) NOT NULL,
        DonViTinh   NVARCHAR(50) NULL,
        MoTa        NVARCHAR(200) NULL
    );
    PRINT N'Đã tạo bảng DICH_VU';
END
GO

-- 2. Bảng HOA_DON
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'HOA_DON')
BEGIN
    CREATE TABLE HOA_DON (
        MaHD        INT PRIMARY KEY,
        MaDP        INT NOT NULL,
        MaNV        INT NOT NULL,
        NgayLap     DATETIME NOT NULL DEFAULT GETDATE(),
        TongTien    DECIMAL(18, 2) NOT NULL DEFAULT 0,
        GhiChu      NVARCHAR(200) NULL,

        CONSTRAINT FK_HOA_DON_DAT_PHONG
            FOREIGN KEY (MaDP) REFERENCES DAT_PHONG(MaDP),
        CONSTRAINT FK_HOA_DON_NHAN_VIEN
            FOREIGN KEY (MaNV) REFERENCES NHAN_VIEN(MaNV)
    );
    PRINT N'Đã tạo bảng HOA_DON';
END
GO

-- 3. Bảng CHI_TIET_HOA_DON
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CHI_TIET_HOA_DON')
BEGIN
    CREATE TABLE CHI_TIET_HOA_DON (
        MaCTHD      INT PRIMARY KEY,
        MaHD        INT NOT NULL,
        MaDV        INT NOT NULL,
        SoLuong     INT NOT NULL DEFAULT 1,
        DonGia      DECIMAL(18, 2) NOT NULL,
        ThanhTien   DECIMAL(18, 2) NOT NULL,

        CONSTRAINT FK_CTHD_HOA_DON
            FOREIGN KEY (MaHD) REFERENCES HOA_DON(MaHD),
        CONSTRAINT FK_CTHD_DICH_VU
            FOREIGN KEY (MaDV) REFERENCES DICH_VU(MaDV)
    );
    PRINT N'Đã tạo bảng CHI_TIET_HOA_DON';
END
GO

-- =============================================
-- Thêm dữ liệu mẫu
-- =============================================

-- Dữ liệu mẫu DICH_VU
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa) VALUES
(1, N'Giặt ủi', 50000, N'Kg', N'Dịch vụ giặt ủi quần áo'),
(2, N'Ăn sáng', 80000, N'Suất', N'Buffet sáng tại nhà hàng'),
(3, N'Thuê xe máy', 150000, N'Ngày', N'Thuê xe máy tự lái'),
(4, N'Spa & Massage', 300000, N'Lần', N'Dịch vụ spa thư giãn'),
(5, N'Minibar', 100000, N'Lần', N'Đồ uống trong phòng');
GO

PRINT N'Hoàn tất tạo bảng và dữ liệu mẫu!';
GO
