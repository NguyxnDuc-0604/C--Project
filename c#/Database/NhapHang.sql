-- ============================================================
-- Script: NhapHang.sql
-- Mô tả: Tạo bảng NHA_CUNG_CAP, PHIEU_NHAP, CT_PHIEU_NHAP
--        + Trigger tự động cập nhật tồn kho DICH_VU
-- Database: QuanLyKhachSan
-- ============================================================

USE QuanLyKhachSan;
GO

-- ============================================================
-- Thêm cột SoLuong vào DICH_VU nếu chưa có (tồn kho)
-- ============================================================
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('DICH_VU') AND name = 'SoLuong'
)
BEGIN
    ALTER TABLE DICH_VU ADD SoLuong INT NOT NULL DEFAULT 0;
    PRINT N'Đã thêm cột SoLuong vào DICH_VU';
END
GO

-- ============================================================
-- Xoá bảng cũ nếu tồn tại (theo thứ tự FK)
-- ============================================================
IF OBJECT_ID('dbo.CT_PHIEU_NHAP', 'U') IS NOT NULL
    DROP TABLE dbo.CT_PHIEU_NHAP;
IF OBJECT_ID('dbo.PHIEU_NHAP',    'U') IS NOT NULL
    DROP TABLE dbo.PHIEU_NHAP;
IF OBJECT_ID('dbo.NHA_CUNG_CAP',  'U') IS NOT NULL
    DROP TABLE dbo.NHA_CUNG_CAP;
GO

-- ============================================================
-- 1. Bảng NHA_CUNG_CAP
-- ============================================================
CREATE TABLE NHA_CUNG_CAP
(
    MaNCC   INT           NOT NULL IDENTITY(1,1),
    TenNCC  NVARCHAR(100) NOT NULL,
    SDT     NVARCHAR(15)  NULL,
    DiaChi  NVARCHAR(200) NULL,
    Email   NVARCHAR(100) NULL,
    CONSTRAINT PK_NhaCungCap PRIMARY KEY (MaNCC)
);
GO

-- ============================================================
-- 2. Bảng PHIEU_NHAP
-- ============================================================
CREATE TABLE PHIEU_NHAP
(
    MaPN     INT      NOT NULL IDENTITY(1,1),
    MaNCC    INT      NOT NULL,
    MaNV     INT      NOT NULL,
    NgayNhap DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_PhieuNhap PRIMARY KEY (MaPN),
    CONSTRAINT FK_PhieuNhap_NhaCungCap
        FOREIGN KEY (MaNCC) REFERENCES NHA_CUNG_CAP (MaNCC),
    CONSTRAINT FK_PhieuNhap_NhanVien
        FOREIGN KEY (MaNV)  REFERENCES NHAN_VIEN (MaNV)
);
GO

-- ============================================================
-- 3. Bảng CT_PHIEU_NHAP
-- ============================================================
CREATE TABLE CT_PHIEU_NHAP
(
    MaCTPN    INT           NOT NULL IDENTITY(1,1),
    MaPN      INT           NOT NULL,
    MaDV      INT           NOT NULL,
    SoLuong   INT           NOT NULL CHECK (SoLuong > 0),
    DonGiaNhap DECIMAL(18,2) NOT NULL CHECK (DonGiaNhap >= 0),
    CONSTRAINT PK_CTPhieuNhap PRIMARY KEY (MaCTPN),
    CONSTRAINT FK_CTPhieuNhap_PhieuNhap
        FOREIGN KEY (MaPN) REFERENCES PHIEU_NHAP (MaPN)
            ON DELETE CASCADE,
    CONSTRAINT FK_CTPhieuNhap_DichVu
        FOREIGN KEY (MaDV) REFERENCES DICH_VU (MaDV)
);
GO

-- ============================================================
-- 4. TRIGGER: Cộng tồn kho khi THÊM chi tiết phiếu nhập
--    Nghiệp vụ: DICH_VU.SoLuong += SoLuong nhập vào
--    Ví dụ: SoLuong = 50, nhập 20 => SoLuong = 70
-- ============================================================
IF OBJECT_ID('dbo.trg_CapNhatTonKho_Insert', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_CapNhatTonKho_Insert;
GO

CREATE TRIGGER trg_CapNhatTonKho_Insert
ON CT_PHIEU_NHAP
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dv
    SET    dv.SoLuong = ISNULL(dv.SoLuong, 0) + i.TongNhap
    FROM   DICH_VU dv
    INNER JOIN (
        SELECT MaDV, SUM(SoLuong) AS TongNhap
        FROM   inserted
        GROUP BY MaDV
    ) i ON dv.MaDV = i.MaDV;
END;
GO

-- ============================================================
-- 5. TRIGGER: Trừ tồn kho khi XOÁ chi tiết phiếu nhập
-- ============================================================
IF OBJECT_ID('dbo.trg_CapNhatTonKho_Delete', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_CapNhatTonKho_Delete;
GO

CREATE TRIGGER trg_CapNhatTonKho_Delete
ON CT_PHIEU_NHAP
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dv
    SET    dv.SoLuong = dv.SoLuong - d.TongXoa
    FROM   DICH_VU dv
    INNER JOIN (
        SELECT MaDV, SUM(SoLuong) AS TongXoa
        FROM   deleted
        GROUP BY MaDV
    ) d ON dv.MaDV = d.MaDV;
END;
GO

-- ============================================================
-- Dữ liệu mẫu
-- ============================================================
INSERT INTO NHA_CUNG_CAP (TenNCC, SDT, DiaChi, Email) VALUES
(N'Công ty Tân Hiệp Phát',    '0274123456', N'Bình Dương',       N'thp@thp.com.vn'),
(N'Công ty Thực phẩm Á Châu', '0283456789', N'TP. Hồ Chí Minh', N'achau@food.vn'),
(N'Đại lý Tiện nghi Sao Việt','0243987654', N'Hà Nội',           N'saoviet@gmail.com');
GO

-- Thêm phiếu nhập mẫu (giả sử MaNV=1 đã tồn tại trong NHAN_VIEN)
INSERT INTO PHIEU_NHAP (MaNCC, MaNV, NgayNhap) VALUES
(1, 1, '2026-11-20 09:30:00'),
(2, 1, '2026-11-21 14:00:00');
GO

-- Khi INSERT bên dưới => Trigger tự cộng SoLuong vào DICH_VU
INSERT INTO CT_PHIEU_NHAP (MaPN, MaDV, SoLuong, DonGiaNhap) VALUES
(1, 1, 20, 10000),
(1, 2, 30, 22000),
(2, 3, 15, 14000);
GO

-- Kiểm tra kết quả tồn kho
SELECT MaDV, TenDV, SoLuong FROM DICH_VU WHERE MaDV IN (1,2,3);
GO
