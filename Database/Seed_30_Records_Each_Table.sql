-- ====================================================================
-- KỊCH BẢN NẠP 30 DÒNG DỮ LIỆU MẪU CHO TỪNG BẢNG (12 BẢNG)
-- ĐỒ ÁN MÔN HỌC: QUẢN LÝ KHÁCH SẠN
-- CHUẨN UNICODE TIẾNG VIỆT 100%
-- ====================================================================
USE [QuanLyKhachSan];
GO

-- XÓA SẠCH DỮ LIỆU CŨ THEO ĐÚNG THỨ TỰ KHÓA NGOẠI
DELETE FROM CT_PHIEU_NHAP;
DELETE FROM PHIEU_NHAP;
DELETE FROM NHA_CUNG_CAP;
DELETE FROM CHI_TIET_HOA_DON;
DELETE FROM HOA_DON;
DELETE FROM CHI_TIET_DAT_PHONG;
DELETE FROM DAT_PHONG;
DELETE FROM DICH_VU;
DELETE FROM PHONG;
DELETE FROM LOAI_PHONG;
DELETE FROM KHACH_HANG;
DELETE FROM NHAN_VIEN;

-- RESET IDENTITY CHO CÁC BẢNG TỰ TĂNG
DBCC CHECKIDENT ('CT_PHIEU_NHAP', RESEED, 0);
DBCC CHECKIDENT ('PHIEU_NHAP', RESEED, 0);
DBCC CHECKIDENT ('NHA_CUNG_CAP', RESEED, 0);
GO

-- ====================================================================
-- 1. BẢNG LOAI_PHONG (30 BẢN GHI)
-- ====================================================================
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (1, N'Phòng Đơn Tiêu Chuẩn', 350000, 1, N'Phòng đơn tiêu chuẩn, tiện nghi cơ bản, máy lạnh, wifi', 10);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (2, N'Phòng Đơn Cao Cấp', 450000, 1, N'Phòng đơn nội thất cao cấp, cửa sổ lớn view phố', 8);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (3, N'Phòng Đơn Deluxe', 600000, 1, N'Phòng đơn sang trọng, có ban công ngắm hoàng hôn', 6);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (4, N'Phòng Đôi Tiêu Chuẩn', 550000, 2, N'Phòng 1 giường đôi êm ái, đầy đủ trang thiết bị', 12);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (5, N'Phòng Đôi Cao Cấp', 700000, 2, N'Phòng đôi rộng rãi, view hồ bơi và vườn cây', 10);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (6, N'Phòng Đôi Deluxe', 850000, 2, N'Phòng đôi cao cấp, bồn tắm nằm massage nước', 8);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (7, N'Phòng 2 Giường Tiêu Chuẩn', 550000, 2, N'Phòng 2 giường đơn riêng biệt cho bạn bè đồng nghiệp', 8);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (8, N'Phòng 2 Giường Cao Cấp', 700000, 2, N'Phòng 2 giường đơn diện tích thoáng, view thành phố', 6);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (9, N'Phòng 2 Giường Deluxe', 850000, 2, N'Phòng 2 giường cao cấp ban công riêng, minibar', 6);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (10, N'Phòng Gia Đình Tiêu Chuẩn', 950000, 3, N'1 giường đôi lớn và 1 giường đơn tiện lợi', 6);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (11, N'Phòng Gia Đình Cao Cấp', 1200000, 4, N'2 giường đôi lớn, bàn sinh hoạt chung cho cả nhà', 5);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (12, N'Phòng Gia Đình Suite', 1600000, 5, N'2 phòng ngủ liên thông, 2 phòng tắm riêng biệt', 4);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (13, N'Phòng Studio Hướng Vườn', 800000, 2, N'Căn hộ Studio nhỏ xinh có gian bếp nấu nướng nhẹ', 4);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (14, N'Phòng Studio Hướng Biển', 1050000, 2, N'Căn hộ Studio view trọn biển, ban công lộng gió', 4);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (15, N'Phòng Suite Hoàng Gia', 2500000, 2, N'Nội thất phong cách hoàng gia, phòng khách riêng', 2);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (16, N'Phòng Tổng Thống VIP', 5000000, 4, N'Căn hộ thượng hạng 150m2, quầy bar, phòng họp nhỏ', 1);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (17, N'Phòng Penthouse Tầng Thượng', 3800000, 4, N'Tầng cao nhất tòa nhà, hồ bơi riêng, view 360 độ', 1);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (18, N'Bungalow Vườn Nhiệt Đới', 1300000, 2, N'Nhà gỗ biệt lập giữa hoa viên cây xanh thanh bình', 3);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (19, N'Bungalow Bờ Biển Riêng', 1800000, 2, N'Bungalow sát mép nước, ngắm bình minh ngay tại giường', 3);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (20, N'Phòng Honeymoon Trăng Mật', 1500000, 2, N'Trang trí nến, hoa hồng, tặng kèm rượu vang', 3);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (21, N'Phòng Thương Gia Executive', 1100000, 2, N'Bàn làm việc thông minh, dịch vụ là ủi miễn phí', 4);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (22, N'Phòng Áp Mái Cổ Điển', 650000, 2, N'Không gian gác mái lãng mạn phong cách châu Âu vintage', 3);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (23, N'Phòng Panorama Kính Tràn', 1400000, 2, N'Tường kính vòng cung ngắm toàn cảnh thành phố lung linh', 3);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (24, N'Phòng Đơn Tiết Kiệm Eco', 280000, 1, N'Phòng đơn nhỏ gọn cho khách đi công tác tiết kiệm', 6);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (25, N'Phòng Đôi Tiết Kiệm Eco', 420000, 2, N'Giá cả hợp lý dành cho cặp đôi du lịch ba lô', 6);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (26, N'Phòng Dorm Tập Thể 4 Giường', 600000, 4, N'Giường tầng sạch sẽ, tủ locker cá nhân an toàn', 4);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (27, N'Phòng Dorm Tập Thể 6 Giường', 800000, 6, N'Dành cho nhóm phượt, gắn kết bạn bè đông vui', 3);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (28, N'Biệt Thự Villa Hồ Bơi', 4500000, 6, N'Villa 3 phòng ngủ có sân vườn tiệc nướng BBQ', 1);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (29, N'Phòng Grand Suite View Phố', 2200000, 3, N'Phòng khách sang trọng và phòng ngủ view trung tâm', 2);
INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES (30, N'Phòng Trị Liệu Spa & Wellness', 1700000, 2, N'Trang bị phòng xông hơi đá muối Himalaya tại phòng', 2);
GO

-- ====================================================================
-- 2. BẢNG NHAN_VIEN (30 BẢN GHI)
-- ====================================================================
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (1, N'Nguyễn Minh Anh', '0912345001', N'Lễ tân trưởng', 'minhanh', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (2, N'Trần Thu Hà', '0912345002', N'Lễ tân ca sáng', 'thuha', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (3, N'Lê Quốc Bảo', '0912345003', N'Lễ tân ca đêm', 'quocbao', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (4, N'Phạm Hoàng Nam', '0912345004', N'Quản lý tổng', 'hoangnam', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (5, N'Võ Thị Kim Oanh', '0912345005', N'Kế toán trưởng', 'kimoanh', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (6, N'Đặng Thái Sơn', '0912345006', N'Kế toán kho', 'thaison', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (7, N'Hoàng Thùy Linh', '0912345007', N'Thu ngân', 'thuylinh', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (8, N'Vũ Đình Trọng', '0912345008', N'Tổ trưởng buồng phòng', 'dinhtrong', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (9, N'Đỗ Mỹ Hạnh', '0912345009', N'Nhân viên buồng phòng', 'myhanh', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (10, N'Bùi Văn Hùng', '0912345010', N'Nhân viên buồng phòng', 'vanhung', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (11, N'Ngô Thanh Trúc', '0912345011', N'Nhân viên buồng phòng', 'thanhtruc', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (12, N'Dương Đức Thành', '0912345012', N'Kỹ thuật viên điện nước', 'ducthanh', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (13, N'Lý Minh Khang', '0912345013', N'Kỹ thuật viên IT', 'minhkhang', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (14, N'Trịnh Hoài An', '0912345014', N'Bếp trưởng', 'hoaian', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (15, N'Phan Văn Đạt', '0912345015', N'Phụ bếp', 'vandat', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (16, N'Hồ Thị Bích Ngọc', '0912345016', N'Nhân viên quầy pha chế', 'bichngoc', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (17, N'Lâm Thanh Phong', '0912345017', N'Nhân viên phục vụ bàn', 'thanhphong', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (18, N'Mai Thị Hồng Nhung', '0912345018', N'Nhân viên phục vụ bàn', 'hongnhung', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (19, N'Trương Quang Huy', '0912345019', N'Đội trưởng bảo vệ', 'quanghuy', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (20, N'Đoàn Quốc Cường', '0912345020', N'Nhân viên bảo vệ ca ngày', 'quoccuong', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (21, N'Đinh Văn Long', '0912345021', N'Nhân viên bảo vệ ca đêm', 'vanlong', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (22, N'Huỳnh Ngọc Thúy', '0912345022', N'Chuyên viên Spa & Massage', 'ngocthuy', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (23, N'Lưu Gia Hân', '0912345023', N'Chuyên viên Spa', 'giahan', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (24, N'Châu Minh Tuấn', '0912345024', N'Tài xế đưa đón khách', 'minhtuan', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (25, N'Tạ Văn Kiên', '0912345025', N'Tài xế đưa đón khách', 'vankien', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (26, N'Cao Thị Ngọc Mai', '0912345026', N'Nhân viên giặt ủi', 'ngocmai', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (27, N'Lương Khánh Toàn', '0912345027', N'Nhân viên chăm sóc cảnh quan', 'khanhtoan', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (28, N'Tô Hải Đăng', '0912345028', N'Cứu hộ hồ bơi', 'haidang', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (29, N'Nghiêm Thu Trang', '0912345029', N'Nhân viên chăm sóc khách hàng', 'thutrang', '123456');
INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES (30, N'Đào Hữu Phước', '0912345030', N'Nhân viên hành lý Bellman', 'huuphuoc', '123456');
GO

-- ====================================================================
-- 3. BẢNG KHACH_HANG (30 BẢN GHI)
-- ====================================================================
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (1, N'Phạm Ngọc Lan', '001200001001', '0903111001', 'lan.pham@gmail.com', N'125 Hai Bà Trưng, Quận 1, TP. Hồ Chí Minh');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (2, N'Nguyễn Hoàng Long', '001200001002', '0903111002', 'long.nguyen@yahoo.com', N'45 Hoàng Hoa Thám, Ba Đình, Hà Nội');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (3, N'Trần Mỹ Duyên', '001200001003', '0903111003', 'duyen.tran@hotmail.com', N'88 Lê Lợi, Hải Châu, Đà Nẵng');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (4, N'Võ Thành Đạt', '001200001004', '0903111004', 'dat.vo@gmail.com', N'210 Nguyễn Thị Minh Khai, Ninh Kiều, Cần Thơ');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (5, N'Lê Thị Thu Hằng', '001200001005', '0903111005', 'hang.le@gmail.com', N'15 Trần Phú, Ngô Quyền, Hải Phòng');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (6, N'Đặng Minh Khôi', '001200001006', '0903111006', 'khoi.dang@outlook.com', N'56 Nguyễn Huệ, Thành phố Huế, Thừa Thiên Huế');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (7, N'Hoàng Diễm My', '001200001007', '0903111007', 'my.hoang@gmail.com', N'73 Hùng Vương, Nha Trang, Khánh Hòa');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (8, N'Vũ Anh Quân', '001200001008', '0903111008', 'quan.vu@gmail.com', N'34 Quang Trung, Gò Vấp, TP. Hồ Chí Minh');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (9, N'Đỗ Thùy Trang', '001200001009', '0903111009', 'trang.do@gmail.com', N'92 Cầu Giấy, Cầu Giấy, Hà Nội');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (10, N'Bùi Tấn Tài', '001200001010', '0903111010', 'tai.bui@gmail.com', N'18 Lý Thường Kiệt, Quy Nhơn, Bình Định');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (11, N'Ngô Gia Huy', '001200001011', '0903111011', 'huy.ngo@gmail.com', N'64 Lê Duẩn, Thanh Khê, Đà Nẵng');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (12, N'Dương Bảo Châu', '001200001012', '0903111012', 'chau.duong@gmail.com', N'112 Pasteur, Bến Nghé, Quận 1, TP. Hồ Chí Minh');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (13, N'Lý Thiên Hương', '001200001013', '0903111013', 'huong.ly@gmail.com', N'27 Phan Chu Trinh, Hoàn Kiếm, Hà Nội');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (14, N'Trịnh Quốc Trung', '001200001014', '0903111014', 'trung.trinh@gmail.com', N'80 Phan Đình Phùng, Đà Lạt, Lâm Đồng');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (15, N'Phan Như Quỳnh', '001200001015', '0903111015', 'quynh.phan@gmail.com', N'53 Nguyễn Văn Cừ, An Khánh, Cần Thơ');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (16, N'Hồ Vĩnh Phúc', '001200001016', '0903111016', 'phuc.ho@gmail.com', N'140 Cách Mạng Tháng 8, Quận 3, TP. Hồ Chí Minh');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (17, N'Lâm Tuyết Mai', '001200001017', '0903111017', 'mai.lam@gmail.com', N'19 Bà Triệu, Hai Bà Trưng, Hà Nội');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (18, N'Mai Văn Kiệt', '001200001018', '0903111018', 'kiet.mai@gmail.com', N'39 Bạch Đằng, Hải Châu, Đà Nẵng');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (19, N'Trương Thảo Nhi', '001200001019', '0903111019', 'nhi.truong@gmail.com', N'68 Trần Hưng Đạo, Dương Đông, Phú Quốc');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (20, N'Đoàn Hữu Nghị', '001200001020', '0903111020', 'nghi.doan@gmail.com', N'105 Võ Thị Sáu, Vũng Tàu, Bà Rịa - Vũng Tàu');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (21, N'Đinh Kim Phượng', '001200001021', '0903111021', 'phuong.dinh@gmail.com', N'42 Đinh Tiên Hoàng, Quận 1, TP. Hồ Chí Minh');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (22, N'Huỳnh Tấn Dũng', '001200001022', '0903111022', 'dung.huynh@gmail.com', N'15 Nguyễn Thái Học, Ba Đình, Hà Nội');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (23, N'Lưu Ánh Tuyết', '001200001023', '0903111023', 'tuyet.luu@gmail.com', N'78 Hùng Vương, Buôn Ma Thuột, Đắk Lắk');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (24, N'Châu Gia Bảo', '001200001024', '0903111024', 'bao.chau@gmail.com', N'230 Nguyễn Trãi, Thanh Xuân, Hà Nội');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (25, N'Tạ Hoàng Bách', '001200001025', '0903111025', 'bach.ta@gmail.com', N'115 Nam Kỳ Khởi Nghĩa, Quận 3, TP. Hồ Chí Minh');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (26, N'Cao Thị Cẩm Tú', '001200001026', '0903111026', 'tu.cao@gmail.com', N'62 Hoàng Văn Thụ, Hồng Bàng, Hải Phòng');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (27, N'Lương Vĩnh Thụy', '001200001027', '0903111027', 'thuy.luong@gmail.com', N'90 Nguyễn Thị Thập, Quận 7, TP. Hồ Chí Minh');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (28, N'Tô Bích Trâm', '001200001028', '0903111028', 'tram.to@gmail.com', N'33 Trần Phú, Pleiku, Gia Lai');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (29, N'Nghiêm Xuân Trường', '001200001029', '0903111029', 'truong.nghiem@gmail.com', N'14 Thùy Vân, Vũng Tàu, Bà Rịa - Vũng Tàu');
INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES (30, N'Đào Khánh Linh', '001200001030', '0903111030', 'linh.dao@gmail.com', N'82 Lạc Long Quân, Tây Hồ, Hà Nội');
GO

-- ====================================================================
-- 4. BẢNG PHONG (30 BẢN GHI)
-- ====================================================================
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (101, 1, N'P101', N'Tầng 1 - Khu Đông (Phòng 101)', N'Trống');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (102, 2, N'P102', N'Tầng 1 - Khu Đông (Phòng 102)', N'Đang thuê');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (103, 3, N'P103', N'Tầng 1 - Khu Đông (Phòng 103)', N'Đã đặt');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (104, 4, N'P104', N'Tầng 1 - Khu Đông (Phòng 104)', N'Đang dọn dẹp');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (105, 5, N'P105', N'Tầng 1 - Khu Đông (Phòng 105)', N'Trống');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (106, 6, N'P106', N'Tầng 1 - Khu Đông (Phòng 106)', N'Đang thuê');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (107, 7, N'P107', N'Tầng 1 - Khu Đông (Phòng 107)', N'Đã đặt');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (108, 8, N'P108', N'Tầng 1 - Khu Đông (Phòng 108)', N'Đang dọn dẹp');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (109, 9, N'P109', N'Tầng 1 - Khu Đông (Phòng 109)', N'Trống');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (110, 10, N'P110', N'Tầng 1 - Khu Đông (Phòng 110)', N'Đang thuê');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (201, 11, N'P201', N'Tầng 2 - Khu Tây (Phòng 201)', N'Đã đặt');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (202, 12, N'P202', N'Tầng 2 - Khu Tây (Phòng 202)', N'Đang dọn dẹp');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (203, 13, N'P203', N'Tầng 2 - Khu Tây (Phòng 203)', N'Trống');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (204, 14, N'P204', N'Tầng 2 - Khu Tây (Phòng 204)', N'Đang thuê');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (205, 15, N'P205', N'Tầng 2 - Khu Tây (Phòng 205)', N'Đã đặt');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (206, 16, N'P206', N'Tầng 2 - Khu Tây (Phòng 206)', N'Đang dọn dẹp');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (207, 17, N'P207', N'Tầng 2 - Khu Tây (Phòng 207)', N'Trống');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (208, 18, N'P208', N'Tầng 2 - Khu Tây (Phòng 208)', N'Đang thuê');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (209, 19, N'P209', N'Tầng 2 - Khu Tây (Phòng 209)', N'Đã đặt');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (210, 20, N'P210', N'Tầng 2 - Khu Tây (Phòng 210)', N'Đang dọn dẹp');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (301, 21, N'P301', N'Tầng 3 - Khu VIP (Phòng 301)', N'Trống');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (302, 22, N'P302', N'Tầng 3 - Khu VIP (Phòng 302)', N'Đang thuê');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (303, 23, N'P303', N'Tầng 3 - Khu VIP (Phòng 303)', N'Đã đặt');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (304, 24, N'P304', N'Tầng 3 - Khu VIP (Phòng 304)', N'Đang dọn dẹp');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (305, 25, N'P305', N'Tầng 3 - Khu VIP (Phòng 305)', N'Trống');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (306, 26, N'P306', N'Tầng 3 - Khu VIP (Phòng 306)', N'Đang thuê');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (307, 27, N'P307', N'Tầng 3 - Khu VIP (Phòng 307)', N'Đã đặt');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (308, 28, N'P308', N'Tầng 3 - Khu VIP (Phòng 308)', N'Đang dọn dẹp');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (309, 29, N'P309', N'Tầng 3 - Khu VIP (Phòng 309)', N'Trống');
INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES (310, 30, N'P310', N'Tầng 3 - Khu VIP (Phòng 310)', N'Đang thuê');
GO

-- ====================================================================
-- 5. BẢNG DICH_VU (30 BẢN GHI)
-- ====================================================================
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (1, N'Giặt ủi quần áo', 50000, N'Lượt', N'Giặt sấy và ủi phẳng thơm tho lấy trong ngày', 100);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (2, N'Ăn sáng buffet', 120000, N'Suất', N'Buffet sáng phong phú hơn 40 món Á - Âu', 200);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (3, N'Nước suối Lavie 500ml', 15000, N'Chai', N'Nước khoáng thiên nhiên đóng chai ướp lạnh', 500);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (4, N'Đưa đón sân bay 4 chỗ', 250000, N'Chuyến', N'Xe 4 chỗ đời mới đưa đón tận sảnh sân bay', 50);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (5, N'Đưa đón sân bay 7 chỗ', 350000, N'Chuyến', N'Xe 7 chỗ rộng rãi thoải mái cho gia đình', 40);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (6, N'Massage toàn thân Spa 60p', 350000, N'Suất', N'Massage thảo dược thư giãn tinh thần', 80);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (7, N'Thuê xe máy tay ga', 150000, N'Ngày', N'Xe tay ga Honda Vision/AirBlade kèm 2 nón bảo hiểm', 30);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (8, N'Thuê xe máy số', 100000, N'Ngày', N'Xe số Honda Wave Alpha bền bỉ tiết kiệm xăng', 25);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (9, N'Vé hồ bơi vô cực', 60000, N'Vé', N'Tắm hồ bơi tầng thượng và sử dụng khăn tắm miễn phí', 150);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (10, N'Phòng tập Gym thể hình', 50000, N'Lượt', N'Trang thiết bị máy tập chạy, tạ hiện đại chuẩn quốc tế', 100);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (11, N'Set trà chiều phong cách Anh', 180000, N'Set', N'Trà hoa cúc, trà Earl Grey và các loại bánh Macaron', 60);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (12, N'Rượu vang đỏ Đà Lạt', 320000, N'Chai', N'Vang đỏ nồng nàn thích hợp tiệc tối lãng mạn', 50);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (13, N'Bia Heineken lon 330ml', 30000, N'Lon', N'Bia Heineken ướp lạnh sảng khoái', 300);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (14, N'Bia Tiger lon 330ml', 25000, N'Lon', N'Bia Tiger lon bạc ướp lạnh', 300);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (15, N'Nước ngọt Coca-Cola', 20000, N'Lon', N'Nước giải khát có gas lon 320ml', 400);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (16, N'Nước ngọt Pepsi', 20000, N'Lon', N'Nước giải khát Pepsi mát lạnh', 400);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (17, N'Cà phê phin Robusta', 25000, N'Ly', N'Cà phê đen đá hoặc sữa đá nguyên chất đậm đà', 250);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (18, N'Đĩa trái cây tươi nhiệt đới', 80000, N'Đĩa', N'Xoài, thanh long, dưa hấu, nho phục vụ tại phòng', 80);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (19, N'Bữa tối nến lãng mạn', 550000, N'Bàn', N'Set ăn tối 4 món Á - Âu phục vụ riêng tư tại phòng', 30);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (20, N'Tour tham quan di tích nửa ngày', 400000, N'Người', N'Xe đưa đón kèm hướng dẫn viên thuyết minh', 50);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (21, N'Tour cano lặn ngắm san hô', 650000, N'Người', N'Bao gồm cano cao tốc, áo phao, kính lặn chuyên dụng', 40);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (22, N'Giặt khô là hơi đồ vest cao cấp', 150000, N'Bộ', N'Công nghệ giặt khô bảo vệ giữ phom áo vest', 60);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (23, N'Trang trí phòng trăng mật', 450000, N'Gói', N'Bóng bay, hoa hồng rải giường, nến thơm cao cấp', 25);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (24, N'Thuê phòng họp mini 15 người', 1200000, N'Buổi', N'Đầy đủ bảng viết, bút, nước suối và điều hòa', 20);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (25, N'Thuê máy chiếu & màn chiếu HD', 300000, N'Buổi', N'Máy chiếu độ sáng cao, kết nối HDMI không dây', 20);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (26, N'Dịch vụ bảo mẫu trông trẻ', 100000, N'Giờ', N'Nhân viên trông giữ bé chu đáo an toàn', 50);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (27, N'Hướng dẫn viên du lịch riêng', 800000, N'Ngày', N'Hướng dẫn viên am hiểu lịch sử, ngoại ngữ lưu loát', 20);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (28, N'Set tiệc nướng BBQ hải sản', 750000, N'Set', N'Hải sản tôm, mực, hàu tươi nướng kèm than hoa', 35);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (29, N'Thuê xe đạp đôi dạo phố', 40000, N'Giờ', N'Xe đạp đôi êm ái ngắm cảnh ven hồ/ven biển', 40);
INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES (30, N'Kê thêm giường phụ Extra Bed', 200000, N'Đêm', N'Nệm lò xo tiêu chuẩn êm ái kê thêm cho phòng', 30);
GO

-- ====================================================================
-- 6. BẢNG DAT_PHONG (30 BẢN GHI - PHÂN BỔ ĐỀU 4 QUÝ 2026)
-- ====================================================================
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (1, 1, 1, '2026-01-10 09:00:00', '2026-01-12 14:00:00', '2026-01-15 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (2, 2, 2, '2026-01-18 10:30:00', '2026-01-20 14:00:00', '2026-01-22 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (3, 3, 3, '2026-02-02 11:00:00', '2026-02-05 14:00:00', '2026-02-08 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (4, 4, 4, '2026-02-14 08:00:00', '2026-02-14 14:00:00', '2026-02-17 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (5, 5, 5, '2026-02-25 15:00:00', '2026-02-27 14:00:00', '2026-03-02 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (6, 6, 1, '2026-03-08 09:30:00', '2026-03-10 14:00:00', '2026-03-13 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (7, 7, 2, '2026-03-19 14:00:00', '2026-03-22 14:00:00', '2026-03-25 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (8, 8, 3, '2026-03-28 10:00:00', '2026-03-29 14:00:00', '2026-03-31 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (9, 9, 4, '2026-04-05 08:30:00', '2026-04-07 14:00:00', '2026-04-10 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (10, 10, 5, '2026-04-16 13:00:00', '2026-04-18 14:00:00', '2026-04-21 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (11, 11, 1, '2026-04-28 09:00:00', '2026-04-30 14:00:00', '2026-05-03 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (12, 12, 2, '2026-05-10 11:30:00', '2026-05-12 14:00:00', '2026-05-15 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (13, 13, 3, '2026-05-22 14:00:00', '2026-05-24 14:00:00', '2026-05-27 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (14, 14, 4, '2026-06-01 10:00:00', '2026-06-03 14:00:00', '2026-06-06 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (15, 15, 5, '2026-06-12 16:00:00', '2026-06-15 14:00:00', '2026-06-18 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (16, 16, 1, '2026-06-25 09:00:00', '2026-06-27 14:00:00', '2026-06-30 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (17, 17, 2, '2026-07-04 10:00:00', '2026-07-06 14:00:00', '2026-07-09 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (18, 18, 3, '2026-07-18 11:00:00', '2026-07-20 14:00:00', '2026-07-23 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (19, 19, 4, '2026-08-01 14:00:00', '2026-08-03 14:00:00', '2026-08-06 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (20, 20, 5, '2026-08-15 08:30:00', '2026-08-18 14:00:00', '2026-08-21 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (21, 21, 1, '2026-08-28 10:00:00', '2026-09-01 14:00:00', '2026-09-04 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (22, 22, 2, '2026-09-10 13:00:00', '2026-09-12 14:00:00', '2026-09-15 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (23, 23, 3, '2026-09-22 15:30:00', '2026-09-25 14:00:00', '2026-09-28 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (24, 24, 4, '2026-10-05 09:00:00', '2026-10-08 14:00:00', '2026-10-11 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (25, 25, 5, '2026-10-19 11:00:00', '2026-10-22 14:00:00', '2026-10-25 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (26, 26, 1, '2026-11-02 14:00:00', '2026-11-05 14:00:00', '2026-11-08 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (27, 27, 2, '2026-11-15 10:00:00', '2026-11-18 14:00:00', '2026-11-21 12:00:00', N'Đã trả phòng');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (28, 28, 3, '2026-11-25 09:30:00', '2026-11-27 14:00:00', '2026-11-30 12:00:00', N'Đang ở');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (29, 29, 4, '2026-12-05 10:00:00', '2026-12-10 14:00:00', '2026-12-14 12:00:00', N'Đã xác nhận');
INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES (30, 30, 5, '2026-12-20 15:00:00', '2026-12-24 14:00:00', '2026-12-28 12:00:00', N'Chờ xác nhận');
GO

-- ====================================================================
-- 7. BẢNG CHI_TIET_DAT_PHONG (30 BẢN GHI)
-- ====================================================================
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (1, 1, 101, 350000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (2, 2, 102, 450000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (3, 3, 103, 600000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (4, 4, 104, 550000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (5, 5, 105, 700000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (6, 6, 106, 850000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (7, 7, 107, 550000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (8, 8, 108, 700000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (9, 9, 109, 850000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (10, 10, 110, 950000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (11, 11, 201, 1200000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (12, 12, 202, 1600000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (13, 13, 203, 800000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (14, 14, 204, 1050000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (15, 15, 205, 2500000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (16, 16, 206, 5000000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (17, 17, 207, 3800000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (18, 18, 208, 1300000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (19, 19, 209, 1800000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (20, 20, 210, 1500000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (21, 21, 301, 1100000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (22, 22, 302, 650000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (23, 23, 303, 1400000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (24, 24, 304, 280000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (25, 25, 305, 420000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (26, 26, 306, 600000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (27, 27, 307, 800000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (28, 28, 308, 4500000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (29, 29, 309, 2200000);
INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES (30, 30, 310, 1700000);
GO

-- ====================================================================
-- 8. BẢNG HOA_DON (30 BẢN GHI - CÓ ĐỦ DOANH THU 4 QUÝ)
-- ====================================================================
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (1, 1, 1, '2026-01-15 12:00:00', 1150000, N'Thanh toán thẻ Visa thành công');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (2, 2, 2, '2026-01-22 12:00:00', 1590000, N'Thanh toán chuyển khoản ngân hàng Vietcombank');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (3, 3, 3, '2026-02-08 12:00:00', 1830000, N'Thanh toán tiền mặt tại quầy lễ tân');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (4, 4, 4, '2026-02-17 12:00:00', 2150000, N'Thanh toán qua quét mã QR MoMo');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (5, 5, 5, '2026-03-02 12:00:00', 2800000, N'Thanh toán thẻ Mastercard');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (6, 6, 1, '2026-03-13 12:00:00', 3250000, N'Thanh toán qua VNPay-QR');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (7, 7, 2, '2026-03-25 12:00:00', 1950000, N'Thanh toán thẻ Visa thành công');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (8, 8, 3, '2026-03-31 12:00:00', 2300000, N'Thanh toán chuyển khoản ngân hàng Vietcombank');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (9, 9, 4, '2026-04-10 12:00:00', 2670000, N'Thanh toán tiền mặt tại quầy lễ tân');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (10, 10, 5, '2026-04-21 12:00:00', 2950000, N'Thanh toán qua quét mã QR MoMo');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (11, 11, 1, '2026-05-03 12:00:00', 3960000, N'Thanh toán thẻ Mastercard');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (12, 12, 2, '2026-05-15 12:00:00', 5440000, N'Thanh toán qua VNPay-QR');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (13, 13, 3, '2026-05-27 12:00:00', 2460000, N'Thanh toán thẻ Visa thành công');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (14, 14, 4, '2026-06-06 12:00:00', 3200000, N'Thanh toán chuyển khoản ngân hàng Vietcombank');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (15, 15, 5, '2026-06-18 12:00:00', 7540000, N'Thanh toán tiền mặt tại quầy lễ tân');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (16, 16, 1, '2026-06-30 12:00:00', 15040000, N'Thanh toán qua quét mã QR MoMo');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (17, 17, 2, '2026-07-09 12:00:00', 11450000, N'Thanh toán thẻ Mastercard');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (18, 18, 3, '2026-07-23 12:00:00', 4060000, N'Thanh toán qua VNPay-QR');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (19, 19, 4, '2026-08-06 12:00:00', 6500000, N'Thanh toán thẻ Visa thành công');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (20, 20, 5, '2026-08-21 12:00:00', 5300000, N'Thanh toán chuyển khoản ngân hàng Vietcombank');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (21, 21, 1, '2026-09-04 12:00:00', 4600000, N'Thanh toán tiền mặt tại quầy lễ tân');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (22, 22, 2, '2026-09-15 12:00:00', 2250000, N'Thanh toán qua quét mã QR MoMo');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (23, 23, 3, '2026-09-28 12:00:00', 5100000, N'Thanh toán thẻ Mastercard');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (24, 24, 4, '2026-10-11 12:00:00', 3240000, N'Thanh toán qua VNPay-QR');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (25, 25, 5, '2026-10-25 12:00:00', 1860000, N'Thanh toán thẻ Visa thành công');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (26, 26, 1, '2026-11-08 12:00:00', 2000000, N'Thanh toán chuyển khoản ngân hàng Vietcombank');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (27, 27, 2, '2026-11-21 12:00:00', 4000000, N'Thanh toán tiền mặt tại quầy lễ tân');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (28, 28, 3, '2026-11-30 12:00:00', 15000000, N'Thanh toán qua quét mã QR MoMo');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (29, 29, 4, '2026-12-14 12:00:00', 6680000, N'Thanh toán thẻ Mastercard');
INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES (30, 30, 5, '2026-12-28 12:00:00', 5500000, N'Thanh toán qua VNPay-QR');
GO

-- ====================================================================
-- 9. BẢNG CHI_TIET_HOA_DON (30 BẢN GHI)
-- ====================================================================
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (1, 1, 1, 2, 50000, 100000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (2, 2, 2, 2, 120000, 240000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (3, 3, 3, 2, 15000, 30000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (4, 4, 4, 2, 250000, 500000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (5, 5, 5, 2, 350000, 700000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (6, 6, 6, 2, 350000, 700000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (7, 7, 7, 2, 150000, 300000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (8, 8, 8, 2, 100000, 200000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (9, 9, 9, 2, 60000, 120000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (10, 10, 10, 2, 50000, 100000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (11, 11, 11, 2, 180000, 360000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (12, 12, 12, 2, 320000, 640000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (13, 13, 13, 2, 30000, 60000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (14, 14, 14, 2, 25000, 50000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (15, 15, 15, 2, 20000, 40000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (16, 16, 16, 2, 20000, 40000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (17, 17, 17, 2, 25000, 50000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (18, 18, 18, 2, 80000, 160000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (19, 19, 19, 2, 550000, 1100000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (20, 20, 20, 2, 400000, 800000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (21, 21, 21, 2, 650000, 1300000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (22, 22, 22, 2, 150000, 300000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (23, 23, 23, 2, 450000, 900000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (24, 24, 24, 2, 1200000, 2400000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (25, 25, 25, 2, 300000, 600000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (26, 26, 26, 2, 100000, 200000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (27, 27, 27, 2, 800000, 1600000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (28, 28, 28, 2, 750000, 1500000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (29, 29, 29, 2, 40000, 80000);
INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES (30, 30, 30, 2, 200000, 400000);
GO

-- ====================================================================
-- 10. BẢNG NHA_CUNG_CAP (30 BẢN GHI)
-- ====================================================================
SET IDENTITY_INSERT NHA_CUNG_CAP ON;
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (1, N'Công ty TNHH Giặt Là Công Nghiệp Ánh Dương', '02838112001', N'45 Khu Công Nghiệp Tân Bình, Tân Phú, TP.HCM', 'contact@anhduonglaundry.com');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (2, N'Công ty Cổ Phần Thực Phẩm Sạch GreenFarm', '02437661002', N'128 Giải Phóng, Phương Liệt, Thanh Xuân, Hà Nội', 'sales@greenfarmvietnam.com');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (3, N'Công ty TNHH Nước Giải Khát Suntory PepsiCo', '02838225003', N'Tầng 5 Cao ốc Sheraton, 88 Đồng Khởi, Q.1, TP.HCM', 'info@pepsico.com.vn');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (4, N'Tổng Công Ty Cổ Phần Bia - Rượu - NGK Sài Gòn (Sabeco)', '02838294004', N'187 Nguyễn Chí Thanh, Phường 12, Quận 5, TP.HCM', 'sabeco@sabeco.com.vn');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (5, N'Công ty TNHH Nước Khoáng Thiên Nhiên La Vie', '02723511005', N'Quốc lộ 1A, Khánh Hậu, Tân An, Long An', 'customerservice@laviewater.com');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (6, N'Công ty Cổ Phần Sữa Việt Nam (Vinamilk)', '02854155006', N'10 Tân Trào, Tân Phú, Quận 7, TP.HCM', 'vinamilk@vinamilk.com.vn');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (7, N'Công ty Nệm Cao Su Thiên Nhiên Kymdan', '02839505007', N'28 Bình Thới, Phường 14, Quận 11, TP.HCM', 'contact_kymdan@kymdan.vn');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (8, N'Công ty Thiết Bị Khách Sạn Cao Cấp Phúc Hòa', '02436883008', N'32 Đốc Ngữ, Vĩnh Phúc, Ba Đình, Hà Nội', 'phuchoa_hotel@phuchoa.com.vn');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (9, N'Công ty TNHH Hóa Mỹ Phẩm Unilever Việt Nam', '02854135009', N'A2-3 KCN Tây Bắc Củ Chi, Củ Chi, TP.HCM', 'unilever.care@unilever.com');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (10, N'Công ty Cổ Phần Chăn Ga Gối Đệm Sông Hồng', '02283649010', N'105 Nguyễn Du, Vị Hoàng, TP. Nam Định', 'songhongbedding@songhong.vn');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (11, N'Công ty TNHH Rượu Vang Đà Lạt Ladofoods', '02633822011', N'Lô B1-3 KCN Phát Chi, TP. Đà Lạt, Lâm Đồng', 'ladofoods@dalatwine.com.vn');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (12, N'Công ty Cổ Phần Cà Phê Trung Nguyên Legend', '02839251012', N'82-84 Bùi Thị Xuân, Bến Thành, Quận 1, TP.HCM', 'contact@trungnguyenlegend.com');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (13, N'Công ty Cổ Phần Dược Phẩm Hoa Linh', '02437676013', N'B19 X3 KĐT Mỹ Đình 1, Nam Từ Liêm, Hà Nội', 'hoalinh_pharma@hoalinh.vn');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (14, N'Công ty TNHH Thiết Bị Điện & Chiếu Sáng Rạng Đông', '02438584014', N'87-89 Hạ Đình, Thanh Xuân Trung, Thanh Xuân, Hà Nội', 'rangdong@rangdong.com.vn');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (15, N'Công ty Khăn Bông Xuất Khẩu Phong Phú', '02838963015', N'48 Tăng Nhơn Phú, Tăng Nhơn Phú B, TP. Thủ Đức', 'phongphu_towel@phongphu.com.vn');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (16, N'Công ty TNHH Đồ Gỗ Nội Thất Hoàng Anh Gia Lai', '02693824016', N'15 Trường Chinh, Phù Đổng, TP. Pleiku, Gia Lai', 'noithat@hagl.com.vn');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (17, N'Công ty Cung Cấp Hải Sản Tươi Sống Biển Đông', '02583881017', N'54 Trần Phú, Lộc Thọ, TP. Nha Trang, Khánh Hòa', 'haisanbiendong@gmail.com');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (18, N'Công ty Rau Củ Quả Đà Lạt GAP', '02633554018', N'241 Hai Bà Trưng, Phường 6, TP. Đà Lạt, Lâm Đồng', 'dalatgap@dalatgap.com');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (19, N'Công ty TNHH Thiết Bị Bếp Công Nghiệp Toàn Phát', '02835127019', N'471 Điện Biên Phủ, Phường 25, Bình Thạnh, TP.HCM', 'toanphat@toanphat.vn');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (20, N'Công ty Vận Tải Du Lịch Mai Linh Express', '02838383020', N'64 Hai Bà Trưng, Bến Nghé, Quận 1, TP.HCM', 'mailinh_tourism@mailinh.vn');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (21, N'Công ty TNHH Âm Thanh Ánh Sáng Việt Thương', '02839316021', N'386 Cách Mạng Tháng 8, Phường 10, Quận 3, TP.HCM', 'vietthuong_audio@vietthuong.vn');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (22, N'Công ty Cây Cảnh & Cảnh Quan Đô Thị Sài Gòn', '02838445022', N'12 Phan Thúc Duyện, Phường 4, Tân Bình, TP.HCM', 'canhquansaigon@cayxanh.vn');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (23, N'Công ty TNHH Thiết Bị An Ninh & Camera Hikvision VN', '02437951023', N'Tầng 8 Tòa nhà Việt Á, Duy Tân, Cầu Giấy, Hà Nội', 'security@hikvision.vn');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (24, N'Công ty May Đồng Phục Khách Sạn Mon Amie', '02862923024', N'357 Nguyễn Trãi, Phường 7, Quận 5, TP.HCM', 'monamie_uniform@monamie.vn');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (25, N'Công ty In Ấn & Thiết Kế Bao Bì Phương Nam', '02838552025', N'214 An Dương Vương, Phường 16, Quận 8, TP.HCM', 'inanphuongnam@gmail.com');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (26, N'Công ty Cung Cấp Thảo Dược & Tinh Dầu Thiên Nhiên Lam Hà', '02435578026', N'68 Hoàng Cầu, Ô Chợ Dừa, Đống Đa, Hà Nội', 'tinhdaulamha@lamha.vn');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (27, N'Công ty Cổ Phần Nhựa Duy Tân', '02838762027', N'298 Hồ Học Lãm, An Lạc, Bình Tân, TP.HCM', 'info@duytan.com');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (28, N'Công ty TNHH Sơn Dulux AkzoNobel Việt Nam', '02838237028', N'Tầng 12 Vincom Center, 72 Lê Thánh Tôn, Quận 1, TP.HCM', 'dulux_vietnam@akzonobel.com');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (29, N'Công ty Thang Máy Mitsubishi Electric Việt Nam', '02839105029', N'Tầng 11 Green Power, 35 Tôn Đức Thắng, Quận 1, TP.HCM', 'mitsubishi_elevator@mevn.com.vn');
INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES (30, N'Công ty Dịch Vụ Vệ Sinh Môi Trường Hoàn Mỹ', '02433898030', N'42 Láng Hạ, Thành Công, Đống Đa, Hà Nội', 'vesinhhoanmy@hoanmy.com.vn');
SET IDENTITY_INSERT NHA_CUNG_CAP OFF;
GO

-- ====================================================================
-- 11. BẢNG PHIEU_NHAP (30 BẢN GHI)
-- ====================================================================
SET IDENTITY_INSERT PHIEU_NHAP ON;
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (1, 1, 1, '2026-01-05 08:30:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (2, 2, 2, '2026-01-15 09:00:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (3, 3, 3, '2026-01-25 10:15:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (4, 4, 4, '2026-02-05 14:00:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (5, 5, 5, '2026-02-15 08:45:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (6, 6, 6, '2026-02-25 11:30:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (7, 7, 1, '2026-03-05 09:30:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (8, 8, 2, '2026-03-15 15:00:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (9, 9, 3, '2026-03-25 10:00:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (10, 10, 4, '2026-04-05 08:00:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (11, 11, 5, '2026-04-15 13:45:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (12, 12, 6, '2026-04-25 09:15:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (13, 13, 1, '2026-05-05 10:30:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (14, 14, 2, '2026-05-15 14:20:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (15, 15, 3, '2026-05-25 08:50:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (16, 16, 4, '2026-06-05 11:10:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (17, 17, 5, '2026-06-15 09:00:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (18, 18, 6, '2026-06-25 15:30:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (19, 19, 1, '2026-07-05 08:40:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (20, 20, 2, '2026-07-15 10:15:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (21, 21, 3, '2026-07-25 14:00:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (22, 22, 4, '2026-08-05 09:20:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (23, 23, 5, '2026-08-15 11:45:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (24, 24, 6, '2026-08-25 16:00:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (25, 25, 1, '2026-09-05 08:30:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (26, 26, 2, '2026-09-15 13:15:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (27, 27, 3, '2026-09-25 10:40:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (28, 28, 4, '2026-10-05 09:00:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (29, 29, 5, '2026-10-20 14:30:00');
INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES (30, 30, 6, '2026-11-10 10:00:00');
SET IDENTITY_INSERT PHIEU_NHAP OFF;
GO

-- ====================================================================
-- 12. BẢNG CT_PHIEU_NHAP (30 BẢN GHI)
-- ====================================================================
SET IDENTITY_INSERT CT_PHIEU_NHAP ON;
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (1, 1, 1, 55, 30000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (2, 2, 2, 60, 72000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (3, 3, 3, 65, 9000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (4, 4, 4, 70, 150000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (5, 5, 5, 75, 210000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (6, 6, 6, 80, 210000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (7, 7, 7, 85, 90000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (8, 8, 8, 90, 60000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (9, 9, 9, 95, 36000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (10, 10, 10, 100, 30000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (11, 11, 11, 105, 108000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (12, 12, 12, 110, 192000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (13, 13, 13, 115, 18000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (14, 14, 14, 120, 15000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (15, 15, 15, 125, 12000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (16, 16, 16, 130, 12000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (17, 17, 17, 135, 15000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (18, 18, 18, 140, 48000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (19, 19, 19, 145, 330000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (20, 20, 20, 150, 240000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (21, 21, 21, 155, 390000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (22, 22, 22, 160, 90000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (23, 23, 23, 165, 270000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (24, 24, 24, 170, 720000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (25, 25, 25, 175, 180000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (26, 26, 26, 180, 60000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (27, 27, 27, 185, 480000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (28, 28, 28, 190, 450000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (29, 29, 29, 195, 24000);
INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES (30, 30, 30, 200, 120000);
SET IDENTITY_INSERT CT_PHIEU_NHAP OFF;
GO

-- THÔNG BÁO HOÀN THÀNH
PRINT N'====================================================================';
PRINT N'ĐÃ NẠP THÀNH CÔNG ĐẦY ĐỦ 30 BẢN GHI MẪU CHO TẤT CẢ 12 BẢNG!';
PRINT N'DỮ LIỆU ĐƯỢC PHÂN BỔ ĐẦY ĐỦ CHO CẢ 4 QUÝ NĂM 2026.';
PRINT N'CHUẨN UNICODE TIẾNG VIỆT 100% KHÔNG LỖI FONT.';
PRINT N'====================================================================';