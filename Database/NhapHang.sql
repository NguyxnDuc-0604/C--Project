/*
  Compatibility script to install/refresh the inventory trigger only.
  Table definitions are owned by SetupDatabase.sql; this file never drops data.
*/
USE QuanLyKhachSan;
GO

IF OBJECT_ID(N'dbo.trg_CapNhatTonKho_Insert', N'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_CapNhatTonKho_Insert;
IF OBJECT_ID(N'dbo.trg_CapNhatTonKho_Delete', N'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_CapNhatTonKho_Delete;
GO

CREATE OR ALTER TRIGGER dbo.trg_CapNhatTonKho
ON dbo.CT_PHIEU_NHAP
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    ;WITH BienDong AS
    (
        SELECT MaDV, SoLuong AS ChenhLech FROM inserted
        UNION ALL
        SELECT MaDV, -SoLuong AS ChenhLech FROM deleted
    ), TongBienDong AS
    (
        SELECT MaDV, SUM(ChenhLech) AS ChenhLech
        FROM BienDong
        GROUP BY MaDV
    )
    UPDATE DV
    SET DV.SoLuong = DV.SoLuong + T.ChenhLech
    FROM dbo.DICH_VU DV
    INNER JOIN TongBienDong T ON T.MaDV = DV.MaDV
    WHERE T.ChenhLech <> 0;
END;
GO

PRINT N'Đã cài trigger cập nhật tồn kho. Không có bảng hoặc dữ liệu nào bị xóa.';
GO
