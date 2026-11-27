# -*- coding: utf-8 -*-
import os

output_sql_path = r'C:\Users\khoav\Downloads\C--Project-newGUI\C--Project-newGUI\Database\Seed_30_Records_Each_Table.sql'
alt_sql_path = r'C:\Users\khoav\.gemini\antigravity\scratch\C--Project\Database\Seed_30_Records_Each_Table.sql'

lines = []
def w(s=''):
    lines.append(s)

w('-- ====================================================================')
w('-- KỊCH BẢN NẠP 30 DÒNG DỮ LIỆU MẪU CHO TỪNG BẢNG (12 BẢNG)')
w('-- ĐỒ ÁN MÔN HỌC: QUẢN LÝ KHÁCH SẠN')
w('-- CHUẨN UNICODE TIẾNG VIỆT 100%')
w('-- ====================================================================')
w('USE [QuanLyKhachSan];')
w('GO')
w('')
w('-- XÓA SẠCH DỮ LIỆU CŨ THEO ĐÚNG THỨ TỰ KHÓA NGOẠI')
w('DELETE FROM CT_PHIEU_NHAP;')
w('DELETE FROM PHIEU_NHAP;')
w('DELETE FROM NHA_CUNG_CAP;')
w('DELETE FROM CHI_TIET_HOA_DON;')
w('DELETE FROM HOA_DON;')
w('DELETE FROM CHI_TIET_DAT_PHONG;')
w('DELETE FROM DAT_PHONG;')
w('DELETE FROM DICH_VU;')
w('DELETE FROM PHONG;')
w('DELETE FROM LOAI_PHONG;')
w('DELETE FROM KHACH_HANG;')
w('DELETE FROM NHAN_VIEN;')
w('')
w('-- RESET IDENTITY CHO CÁC BẢNG TỰ TĂNG')
w("DBCC CHECKIDENT ('CT_PHIEU_NHAP', RESEED, 0);")
w("DBCC CHECKIDENT ('PHIEU_NHAP', RESEED, 0);")
w("DBCC CHECKIDENT ('NHA_CUNG_CAP', RESEED, 0);")
w('GO')
w('')

# 1. LOAI_PHONG (30)
w('-- ====================================================================')
w('-- 1. BẢNG LOAI_PHONG (30 BẢN GHI)')
w('-- ====================================================================')
loai_phong = [
    (1, 'Phòng Đơn Tiêu Chuẩn', 350000, 1, 'Phòng đơn tiêu chuẩn, tiện nghi cơ bản, máy lạnh, wifi', 10),
    (2, 'Phòng Đơn Cao Cấp', 450000, 1, 'Phòng đơn nội thất cao cấp, cửa sổ lớn view phố', 8),
    (3, 'Phòng Đơn Deluxe', 600000, 1, 'Phòng đơn sang trọng, có ban công ngắm hoàng hôn', 6),
    (4, 'Phòng Đôi Tiêu Chuẩn', 550000, 2, 'Phòng 1 giường đôi êm ái, đầy đủ trang thiết bị', 12),
    (5, 'Phòng Đôi Cao Cấp', 700000, 2, 'Phòng đôi rộng rãi, view hồ bơi và vườn cây', 10),
    (6, 'Phòng Đôi Deluxe', 850000, 2, 'Phòng đôi cao cấp, bồn tắm nằm massage nước', 8),
    (7, 'Phòng 2 Giường Tiêu Chuẩn', 550000, 2, 'Phòng 2 giường đơn riêng biệt cho bạn bè đồng nghiệp', 8),
    (8, 'Phòng 2 Giường Cao Cấp', 700000, 2, 'Phòng 2 giường đơn diện tích thoáng, view thành phố', 6),
    (9, 'Phòng 2 Giường Deluxe', 850000, 2, 'Phòng 2 giường cao cấp ban công riêng, minibar', 6),
    (10, 'Phòng Gia Đình Tiêu Chuẩn', 950000, 3, '1 giường đôi lớn và 1 giường đơn tiện lợi', 6),
    (11, 'Phòng Gia Đình Cao Cấp', 1200000, 4, '2 giường đôi lớn, bàn sinh hoạt chung cho cả nhà', 5),
    (12, 'Phòng Gia Đình Suite', 1600000, 5, '2 phòng ngủ liên thông, 2 phòng tắm riêng biệt', 4),
    (13, 'Phòng Studio Hướng Vườn', 800000, 2, 'Căn hộ Studio nhỏ xinh có gian bếp nấu nướng nhẹ', 4),
    (14, 'Phòng Studio Hướng Biển', 1050000, 2, 'Căn hộ Studio view trọn biển, ban công lộng gió', 4),
    (15, 'Phòng Suite Hoàng Gia', 2500000, 2, 'Nội thất phong cách hoàng gia, phòng khách riêng', 2),
    (16, 'Phòng Tổng Thống VIP', 5000000, 4, 'Căn hộ thượng hạng 150m2, quầy bar, phòng họp nhỏ', 1),
    (17, 'Phòng Penthouse Tầng Thượng', 3800000, 4, 'Tầng cao nhất tòa nhà, hồ bơi riêng, view 360 độ', 1),
    (18, 'Bungalow Vườn Nhiệt Đới', 1300000, 2, 'Nhà gỗ biệt lập giữa hoa viên cây xanh thanh bình', 3),
    (19, 'Bungalow Bờ Biển Riêng', 1800000, 2, 'Bungalow sát mép nước, ngắm bình minh ngay tại giường', 3),
    (20, 'Phòng Honeymoon Trăng Mật', 1500000, 2, 'Trang trí nến, hoa hồng, tặng kèm rượu vang', 3),
    (21, 'Phòng Thương Gia Executive', 1100000, 2, 'Bàn làm việc thông minh, dịch vụ là ủi miễn phí', 4),
    (22, 'Phòng Áp Mái Cổ Điển', 650000, 2, 'Không gian gác mái lãng mạn phong cách châu Âu vintage', 3),
    (23, 'Phòng Panorama Kính Tràn', 1400000, 2, 'Tường kính vòng cung ngắm toàn cảnh thành phố lung linh', 3),
    (24, 'Phòng Đơn Tiết Kiệm Eco', 280000, 1, 'Phòng đơn nhỏ gọn cho khách đi công tác tiết kiệm', 6),
    (25, 'Phòng Đôi Tiết Kiệm Eco', 420000, 2, 'Giá cả hợp lý dành cho cặp đôi du lịch ba lô', 6),
    (26, 'Phòng Dorm Tập Thể 4 Giường', 600000, 4, 'Giường tầng sạch sẽ, tủ locker cá nhân an toàn', 4),
    (27, 'Phòng Dorm Tập Thể 6 Giường', 800000, 6, 'Dành cho nhóm phượt, gắn kết bạn bè đông vui', 3),
    (28, 'Biệt Thự Villa Hồ Bơi', 4500000, 6, 'Villa 3 phòng ngủ có sân vườn tiệc nướng BBQ', 1),
    (29, 'Phòng Grand Suite View Phố', 2200000, 3, 'Phòng khách sang trọng và phòng ngủ view trung tâm', 2),
    (30, 'Phòng Trị Liệu Spa & Wellness', 1700000, 2, 'Trang bị phòng xông hơi đá muối Himalaya tại phòng', 2),
]
for item in loai_phong:
    w(f"INSERT INTO LOAI_PHONG (MaLoai, TenLoai, GiaPhong, SoNguoiToiDa, MoTa, SoLuongPhong) VALUES ({item[0]}, N'{item[1]}', {item[2]}, {item[3]}, N'{item[4]}', {item[5]});")
w('GO')
w('')

# 2. NHAN_VIEN (30)
w('-- ====================================================================')
w('-- 2. BẢNG NHAN_VIEN (30 BẢN GHI)')
w('-- ====================================================================')
nhan_vien = [
    (1, 'Nguyễn Minh Anh', '0912345001', 'Lễ tân trưởng', 'minhanh', '123456'),
    (2, 'Trần Thu Hà', '0912345002', 'Lễ tân ca sáng', 'thuha', '123456'),
    (3, 'Lê Quốc Bảo', '0912345003', 'Lễ tân ca đêm', 'quocbao', '123456'),
    (4, 'Phạm Hoàng Nam', '0912345004', 'Quản lý tổng', 'hoangnam', '123456'),
    (5, 'Võ Thị Kim Oanh', '0912345005', 'Kế toán trưởng', 'kimoanh', '123456'),
    (6, 'Đặng Thái Sơn', '0912345006', 'Kế toán kho', 'thaison', '123456'),
    (7, 'Hoàng Thùy Linh', '0912345007', 'Thu ngân', 'thuylinh', '123456'),
    (8, 'Vũ Đình Trọng', '0912345008', 'Tổ trưởng buồng phòng', 'dinhtrong', '123456'),
    (9, 'Đỗ Mỹ Hạnh', '0912345009', 'Nhân viên buồng phòng', 'myhanh', '123456'),
    (10, 'Bùi Văn Hùng', '0912345010', 'Nhân viên buồng phòng', 'vanhung', '123456'),
    (11, 'Ngô Thanh Trúc', '0912345011', 'Nhân viên buồng phòng', 'thanhtruc', '123456'),
    (12, 'Dương Đức Thành', '0912345012', 'Kỹ thuật viên điện nước', 'ducthanh', '123456'),
    (13, 'Lý Minh Khang', '0912345013', 'Kỹ thuật viên IT', 'minhkhang', '123456'),
    (14, 'Trịnh Hoài An', '0912345014', 'Bếp trưởng', 'hoaian', '123456'),
    (15, 'Phan Văn Đạt', '0912345015', 'Phụ bếp', 'vandat', '123456'),
    (16, 'Hồ Thị Bích Ngọc', '0912345016', 'Nhân viên quầy pha chế', 'bichngoc', '123456'),
    (17, 'Lâm Thanh Phong', '0912345017', 'Nhân viên phục vụ bàn', 'thanhphong', '123456'),
    (18, 'Mai Thị Hồng Nhung', '0912345018', 'Nhân viên phục vụ bàn', 'hongnhung', '123456'),
    (19, 'Trương Quang Huy', '0912345019', 'Đội trưởng bảo vệ', 'quanghuy', '123456'),
    (20, 'Đoàn Quốc Cường', '0912345020', 'Nhân viên bảo vệ ca ngày', 'quoccuong', '123456'),
    (21, 'Đinh Văn Long', '0912345021', 'Nhân viên bảo vệ ca đêm', 'vanlong', '123456'),
    (22, 'Huỳnh Ngọc Thúy', '0912345022', 'Chuyên viên Spa & Massage', 'ngocthuy', '123456'),
    (23, 'Lưu Gia Hân', '0912345023', 'Chuyên viên Spa', 'giahan', '123456'),
    (24, 'Châu Minh Tuấn', '0912345024', 'Tài xế đưa đón khách', 'minhtuan', '123456'),
    (25, 'Tạ Văn Kiên', '0912345025', 'Tài xế đưa đón khách', 'vankien', '123456'),
    (26, 'Cao Thị Ngọc Mai', '0912345026', 'Nhân viên giặt ủi', 'ngocmai', '123456'),
    (27, 'Lương Khánh Toàn', '0912345027', 'Nhân viên chăm sóc cảnh quan', 'khanhtoan', '123456'),
    (28, 'Tô Hải Đăng', '0912345028', 'Cứu hộ hồ bơi', 'haidang', '123456'),
    (29, 'Nghiêm Thu Trang', '0912345029', 'Nhân viên chăm sóc khách hàng', 'thutrang', '123456'),
    (30, 'Đào Hữu Phước', '0912345030', 'Nhân viên hành lý Bellman', 'huuphuoc', '123456'),
]
for item in nhan_vien:
    w(f"INSERT INTO NHAN_VIEN (MaNV, HoTen, SDT, ChucVu, TaiKhoan, MatKhau) VALUES ({item[0]}, N'{item[1]}', '{item[2]}', N'{item[3]}', '{item[4]}', '{item[5]}');")
w('GO')
w('')

# 3. KHACH_HANG (30)
w('-- ====================================================================')
w('-- 3. BẢNG KHACH_HANG (30 BẢN GHI)')
w('-- ====================================================================')
khach_hang = [
    (1, 'Phạm Ngọc Lan', '001200001001', '0903111001', 'lan.pham@gmail.com', '125 Hai Bà Trưng, Quận 1, TP. Hồ Chí Minh'),
    (2, 'Nguyễn Hoàng Long', '001200001002', '0903111002', 'long.nguyen@yahoo.com', '45 Hoàng Hoa Thám, Ba Đình, Hà Nội'),
    (3, 'Trần Mỹ Duyên', '001200001003', '0903111003', 'duyen.tran@hotmail.com', '88 Lê Lợi, Hải Châu, Đà Nẵng'),
    (4, 'Võ Thành Đạt', '001200001004', '0903111004', 'dat.vo@gmail.com', '210 Nguyễn Thị Minh Khai, Ninh Kiều, Cần Thơ'),
    (5, 'Lê Thị Thu Hằng', '001200001005', '0903111005', 'hang.le@gmail.com', '15 Trần Phú, Ngô Quyền, Hải Phòng'),
    (6, 'Đặng Minh Khôi', '001200001006', '0903111006', 'khoi.dang@outlook.com', '56 Nguyễn Huệ, Thành phố Huế, Thừa Thiên Huế'),
    (7, 'Hoàng Diễm My', '001200001007', '0903111007', 'my.hoang@gmail.com', '73 Hùng Vương, Nha Trang, Khánh Hòa'),
    (8, 'Vũ Anh Quân', '001200001008', '0903111008', 'quan.vu@gmail.com', '34 Quang Trung, Gò Vấp, TP. Hồ Chí Minh'),
    (9, 'Đỗ Thùy Trang', '001200001009', '0903111009', 'trang.do@gmail.com', '92 Cầu Giấy, Cầu Giấy, Hà Nội'),
    (10, 'Bùi Tấn Tài', '001200001010', '0903111010', 'tai.bui@gmail.com', '18 Lý Thường Kiệt, Quy Nhơn, Bình Định'),
    (11, 'Ngô Gia Huy', '001200001011', '0903111011', 'huy.ngo@gmail.com', '64 Lê Duẩn, Thanh Khê, Đà Nẵng'),
    (12, 'Dương Bảo Châu', '001200001012', '0903111012', 'chau.duong@gmail.com', '112 Pasteur, Bến Nghé, Quận 1, TP. Hồ Chí Minh'),
    (13, 'Lý Thiên Hương', '001200001013', '0903111013', 'huong.ly@gmail.com', '27 Phan Chu Trinh, Hoàn Kiếm, Hà Nội'),
    (14, 'Trịnh Quốc Trung', '001200001014', '0903111014', 'trung.trinh@gmail.com', '80 Phan Đình Phùng, Đà Lạt, Lâm Đồng'),
    (15, 'Phan Như Quỳnh', '001200001015', '0903111015', 'quynh.phan@gmail.com', '53 Nguyễn Văn Cừ, An Khánh, Cần Thơ'),
    (16, 'Hồ Vĩnh Phúc', '001200001016', '0903111016', 'phuc.ho@gmail.com', '140 Cách Mạng Tháng 8, Quận 3, TP. Hồ Chí Minh'),
    (17, 'Lâm Tuyết Mai', '001200001017', '0903111017', 'mai.lam@gmail.com', '19 Bà Triệu, Hai Bà Trưng, Hà Nội'),
    (18, 'Mai Văn Kiệt', '001200001018', '0903111018', 'kiet.mai@gmail.com', '39 Bạch Đằng, Hải Châu, Đà Nẵng'),
    (19, 'Trương Thảo Nhi', '001200001019', '0903111019', 'nhi.truong@gmail.com', '68 Trần Hưng Đạo, Dương Đông, Phú Quốc'),
    (20, 'Đoàn Hữu Nghị', '001200001020', '0903111020', 'nghi.doan@gmail.com', '105 Võ Thị Sáu, Vũng Tàu, Bà Rịa - Vũng Tàu'),
    (21, 'Đinh Kim Phượng', '001200001021', '0903111021', 'phuong.dinh@gmail.com', '42 Đinh Tiên Hoàng, Quận 1, TP. Hồ Chí Minh'),
    (22, 'Huỳnh Tấn Dũng', '001200001022', '0903111022', 'dung.huynh@gmail.com', '15 Nguyễn Thái Học, Ba Đình, Hà Nội'),
    (23, 'Lưu Ánh Tuyết', '001200001023', '0903111023', 'tuyet.luu@gmail.com', '78 Hùng Vương, Buôn Ma Thuột, Đắk Lắk'),
    (24, 'Châu Gia Bảo', '001200001024', '0903111024', 'bao.chau@gmail.com', '230 Nguyễn Trãi, Thanh Xuân, Hà Nội'),
    (25, 'Tạ Hoàng Bách', '001200001025', '0903111025', 'bach.ta@gmail.com', '115 Nam Kỳ Khởi Nghĩa, Quận 3, TP. Hồ Chí Minh'),
    (26, 'Cao Thị Cẩm Tú', '001200001026', '0903111026', 'tu.cao@gmail.com', '62 Hoàng Văn Thụ, Hồng Bàng, Hải Phòng'),
    (27, 'Lương Vĩnh Thụy', '001200001027', '0903111027', 'thuy.luong@gmail.com', '90 Nguyễn Thị Thập, Quận 7, TP. Hồ Chí Minh'),
    (28, 'Tô Bích Trâm', '001200001028', '0903111028', 'tram.to@gmail.com', '33 Trần Phú, Pleiku, Gia Lai'),
    (29, 'Nghiêm Xuân Trường', '001200001029', '0903111029', 'truong.nghiem@gmail.com', '14 Thùy Vân, Vũng Tàu, Bà Rịa - Vũng Tàu'),
    (30, 'Đào Khánh Linh', '001200001030', '0903111030', 'linh.dao@gmail.com', '82 Lạc Long Quân, Tây Hồ, Hà Nội'),
]
for item in khach_hang:
    w(f"INSERT INTO KHACH_HANG (MaKH, HoTen, CCCD, SDT, Email, DiaChi) VALUES ({item[0]}, N'{item[1]}', '{item[2]}', '{item[3]}', '{item[4]}', N'{item[5]}');")
w('GO')
w('')

# 4. PHONG (30)
w('-- ====================================================================')
w('-- 4. BẢNG PHONG (30 BẢN GHI)')
w('-- ====================================================================')
phong = []
trang_thai_phong = ['Trống', 'Đang thuê', 'Đã đặt', 'Đang dọn dẹp']
for i in range(1, 31):
    if i <= 10:
        ma_phong = 100 + i
        vi_tri = f'Tầng 1 - Khu Đông (Phòng {ma_phong})'
    elif i <= 20:
        ma_phong = 200 + (i - 10)
        vi_tri = f'Tầng 2 - Khu Tây (Phòng {ma_phong})'
    else:
        ma_phong = 300 + (i - 20)
        vi_tri = f'Tầng 3 - Khu VIP (Phòng {ma_phong})'
    
    tt = trang_thai_phong[(i - 1) % 4]
    ma_loai = i
    ten_phong = f'P{ma_phong}'
    phong.append((ma_phong, ma_loai, ten_phong, vi_tri, tt))

for item in phong:
    w(f"INSERT INTO PHONG (MaPhong, MaLoai, TenPhong, ViTri, TrangThai) VALUES ({item[0]}, {item[1]}, N'{item[2]}', N'{item[3]}', N'{item[4]}');")
w('GO')
w('')

# 5. DICH_VU (30)
w('-- ====================================================================')
w('-- 5. BẢNG DICH_VU (30 BẢN GHI)')
w('-- ====================================================================')
dich_vu = [
    (1, 'Giặt ủi quần áo', 50000, 'Lượt', 'Giặt sấy và ủi phẳng thơm tho lấy trong ngày', 100),
    (2, 'Ăn sáng buffet', 120000, 'Suất', 'Buffet sáng phong phú hơn 40 món Á - Âu', 200),
    (3, 'Nước suối Lavie 500ml', 15000, 'Chai', 'Nước khoáng thiên nhiên đóng chai ướp lạnh', 500),
    (4, 'Đưa đón sân bay 4 chỗ', 250000, 'Chuyến', 'Xe 4 chỗ đời mới đưa đón tận sảnh sân bay', 50),
    (5, 'Đưa đón sân bay 7 chỗ', 350000, 'Chuyến', 'Xe 7 chỗ rộng rãi thoải mái cho gia đình', 40),
    (6, 'Massage toàn thân Spa 60p', 350000, 'Suất', 'Massage thảo dược thư giãn tinh thần', 80),
    (7, 'Thuê xe máy tay ga', 150000, 'Ngày', 'Xe tay ga Honda Vision/AirBlade kèm 2 nón bảo hiểm', 30),
    (8, 'Thuê xe máy số', 100000, 'Ngày', 'Xe số Honda Wave Alpha bền bỉ tiết kiệm xăng', 25),
    (9, 'Vé hồ bơi vô cực', 60000, 'Vé', 'Tắm hồ bơi tầng thượng và sử dụng khăn tắm miễn phí', 150),
    (10, 'Phòng tập Gym thể hình', 50000, 'Lượt', 'Trang thiết bị máy tập chạy, tạ hiện đại chuẩn quốc tế', 100),
    (11, 'Set trà chiều phong cách Anh', 180000, 'Set', 'Trà hoa cúc, trà Earl Grey và các loại bánh Macaron', 60),
    (12, 'Rượu vang đỏ Đà Lạt', 320000, 'Chai', 'Vang đỏ nồng nàn thích hợp tiệc tối lãng mạn', 50),
    (13, 'Bia Heineken lon 330ml', 30000, 'Lon', 'Bia Heineken ướp lạnh sảng khoái', 300),
    (14, 'Bia Tiger lon 330ml', 25000, 'Lon', 'Bia Tiger lon bạc ướp lạnh', 300),
    (15, 'Nước ngọt Coca-Cola', 20000, 'Lon', 'Nước giải khát có gas lon 320ml', 400),
    (16, 'Nước ngọt Pepsi', 20000, 'Lon', 'Nước giải khát Pepsi mát lạnh', 400),
    (17, 'Cà phê phin Robusta', 25000, 'Ly', 'Cà phê đen đá hoặc sữa đá nguyên chất đậm đà', 250),
    (18, 'Đĩa trái cây tươi nhiệt đới', 80000, 'Đĩa', 'Xoài, thanh long, dưa hấu, nho phục vụ tại phòng', 80),
    (19, 'Bữa tối nến lãng mạn', 550000, 'Bàn', 'Set ăn tối 4 món Á - Âu phục vụ riêng tư tại phòng', 30),
    (20, 'Tour tham quan di tích nửa ngày', 400000, 'Người', 'Xe đưa đón kèm hướng dẫn viên thuyết minh', 50),
    (21, 'Tour cano lặn ngắm san hô', 650000, 'Người', 'Bao gồm cano cao tốc, áo phao, kính lặn chuyên dụng', 40),
    (22, 'Giặt khô là hơi đồ vest cao cấp', 150000, 'Bộ', 'Công nghệ giặt khô bảo vệ giữ phom áo vest', 60),
    (23, 'Trang trí phòng trăng mật', 450000, 'Gói', 'Bóng bay, hoa hồng rải giường, nến thơm cao cấp', 25),
    (24, 'Thuê phòng họp mini 15 người', 1200000, 'Buổi', 'Đầy đủ bảng viết, bút, nước suối và điều hòa', 20),
    (25, 'Thuê máy chiếu & màn chiếu HD', 300000, 'Buổi', 'Máy chiếu độ sáng cao, kết nối HDMI không dây', 20),
    (26, 'Dịch vụ bảo mẫu trông trẻ', 100000, 'Giờ', 'Nhân viên trông giữ bé chu đáo an toàn', 50),
    (27, 'Hướng dẫn viên du lịch riêng', 800000, 'Ngày', 'Hướng dẫn viên am hiểu lịch sử, ngoại ngữ lưu loát', 20),
    (28, 'Set tiệc nướng BBQ hải sản', 750000, 'Set', 'Hải sản tôm, mực, hàu tươi nướng kèm than hoa', 35),
    (29, 'Thuê xe đạp đôi dạo phố', 40000, 'Giờ', 'Xe đạp đôi êm ái ngắm cảnh ven hồ/ven biển', 40),
    (30, 'Kê thêm giường phụ Extra Bed', 200000, 'Đêm', 'Nệm lò xo tiêu chuẩn êm ái kê thêm cho phòng', 30),
]
for item in dich_vu:
    w(f"INSERT INTO DICH_VU (MaDV, TenDV, DonGia, DonViTinh, MoTa, SoLuong) VALUES ({item[0]}, N'{item[1]}', {item[2]}, N'{item[3]}', N'{item[4]}', {item[5]});")
w('GO')
w('')

# 6. DAT_PHONG (30)
w('-- ====================================================================')
w('-- 6. BẢNG DAT_PHONG (30 BẢN GHI - PHÂN BỔ ĐỀU 4 QUÝ 2026)')
w('-- ====================================================================')
dates_dp = [
    # Q1 (1-8)
    ('2026-01-10 09:00:00', '2026-01-12 14:00:00', '2026-01-15 12:00:00', 'Đã trả phòng'),
    ('2026-01-18 10:30:00', '2026-01-20 14:00:00', '2026-01-22 12:00:00', 'Đã trả phòng'),
    ('2026-02-02 11:00:00', '2026-02-05 14:00:00', '2026-02-08 12:00:00', 'Đã trả phòng'),
    ('2026-02-14 08:00:00', '2026-02-14 14:00:00', '2026-02-17 12:00:00', 'Đã trả phòng'),
    ('2026-02-25 15:00:00', '2026-02-27 14:00:00', '2026-03-02 12:00:00', 'Đã trả phòng'),
    ('2026-03-08 09:30:00', '2026-03-10 14:00:00', '2026-03-13 12:00:00', 'Đã trả phòng'),
    ('2026-03-19 14:00:00', '2026-03-22 14:00:00', '2026-03-25 12:00:00', 'Đã trả phòng'),
    ('2026-03-28 10:00:00', '2026-03-29 14:00:00', '2026-03-31 12:00:00', 'Đã trả phòng'),
    # Q2 (9-16)
    ('2026-04-05 08:30:00', '2026-04-07 14:00:00', '2026-04-10 12:00:00', 'Đã trả phòng'),
    ('2026-04-16 13:00:00', '2026-04-18 14:00:00', '2026-04-21 12:00:00', 'Đã trả phòng'),
    ('2026-04-28 09:00:00', '2026-04-30 14:00:00', '2026-05-03 12:00:00', 'Đã trả phòng'),
    ('2026-05-10 11:30:00', '2026-05-12 14:00:00', '2026-05-15 12:00:00', 'Đã trả phòng'),
    ('2026-05-22 14:00:00', '2026-05-24 14:00:00', '2026-05-27 12:00:00', 'Đã trả phòng'),
    ('2026-06-01 10:00:00', '2026-06-03 14:00:00', '2026-06-06 12:00:00', 'Đã trả phòng'),
    ('2026-06-12 16:00:00', '2026-06-15 14:00:00', '2026-06-18 12:00:00', 'Đã trả phòng'),
    ('2026-06-25 09:00:00', '2026-06-27 14:00:00', '2026-06-30 12:00:00', 'Đã trả phòng'),
    # Q3 (17-23)
    ('2026-07-04 10:00:00', '2026-07-06 14:00:00', '2026-07-09 12:00:00', 'Đã trả phòng'),
    ('2026-07-18 11:00:00', '2026-07-20 14:00:00', '2026-07-23 12:00:00', 'Đã trả phòng'),
    ('2026-08-01 14:00:00', '2026-08-03 14:00:00', '2026-08-06 12:00:00', 'Đã trả phòng'),
    ('2026-08-15 08:30:00', '2026-08-18 14:00:00', '2026-08-21 12:00:00', 'Đã trả phòng'),
    ('2026-08-28 10:00:00', '2026-09-01 14:00:00', '2026-09-04 12:00:00', 'Đã trả phòng'),
    ('2026-09-10 13:00:00', '2026-09-12 14:00:00', '2026-09-15 12:00:00', 'Đã trả phòng'),
    ('2026-09-22 15:30:00', '2026-09-25 14:00:00', '2026-09-28 12:00:00', 'Đã trả phòng'),
    # Q4 (24-30)
    ('2026-10-05 09:00:00', '2026-10-08 14:00:00', '2026-10-11 12:00:00', 'Đã trả phòng'),
    ('2026-10-19 11:00:00', '2026-10-22 14:00:00', '2026-10-25 12:00:00', 'Đã trả phòng'),
    ('2026-11-02 14:00:00', '2026-11-05 14:00:00', '2026-11-08 12:00:00', 'Đã trả phòng'),
    ('2026-11-15 10:00:00', '2026-11-18 14:00:00', '2026-11-21 12:00:00', 'Đã trả phòng'),
    ('2026-11-25 09:30:00', '2026-11-27 14:00:00', '2026-11-30 12:00:00', 'Đang ở'),
    ('2026-12-05 10:00:00', '2026-12-10 14:00:00', '2026-12-14 12:00:00', 'Đã xác nhận'),
    ('2026-12-20 15:00:00', '2026-12-24 14:00:00', '2026-12-28 12:00:00', 'Chờ xác nhận'),
]

for i in range(1, 31):
    ma_dp = i
    ma_kh = i
    ma_nv = ((i - 1) % 5) + 1  # Nhân viên lễ tân 1..5
    dt = dates_dp[i - 1]
    w(f"INSERT INTO DAT_PHONG (MaDP, MaKH, MaNV, NgayDat, NgayNhan, NgayTra, TrangThai) VALUES ({ma_dp}, {ma_kh}, {ma_nv}, '{dt[0]}', '{dt[1]}', '{dt[2]}', N'{dt[3]}');")
w('GO')
w('')

# 7. CHI_TIET_DAT_PHONG (30)
w('-- ====================================================================')
w('-- 7. BẢNG CHI_TIET_DAT_PHONG (30 BẢN GHI)')
w('-- ====================================================================')
for i in range(1, 31):
    ma_ctdp = i
    ma_dp = i
    ma_phong = phong[i - 1][0]
    don_gia = loai_phong[i - 1][2]
    w(f"INSERT INTO CHI_TIET_DAT_PHONG (MaCTDP, MaDP, MaPhong, DonGia) VALUES ({ma_ctdp}, {ma_dp}, {ma_phong}, {don_gia});")
w('GO')
w('')

# 8. HOA_DON (30)
w('-- ====================================================================')
w('-- 8. BẢNG HOA_DON (30 BẢN GHI - CÓ ĐỦ DOANH THU 4 QUÝ)')
w('-- ====================================================================')
ghi_chu_hd = [
    'Thanh toán thẻ Visa thành công',
    'Thanh toán chuyển khoản ngân hàng Vietcombank',
    'Thanh toán tiền mặt tại quầy lễ tân',
    'Thanh toán qua quét mã QR MoMo',
    'Thanh toán thẻ Mastercard',
    'Thanh toán qua VNPay-QR'
]
for i in range(1, 31):
    ma_hd = i
    ma_dp = i
    ma_nv = ((i - 1) % 5) + 1
    ngay_lap = dates_dp[i - 1][2]
    tien_phong = loai_phong[i - 1][2] * 3  # 3 đêm
    tien_dv = dich_vu[i - 1][2] * 2        # 2 lượt dv
    tong_tien = tien_phong + tien_dv
    gc = ghi_chu_hd[(i - 1) % len(ghi_chu_hd)]
    w(f"INSERT INTO HOA_DON (MaHD, MaDP, MaNV, NgayLap, TongTien, GhiChu) VALUES ({ma_hd}, {ma_dp}, {ma_nv}, '{ngay_lap}', {tong_tien}, N'{gc}');")
w('GO')
w('')

# 9. CHI_TIET_HOA_DON (30)
w('-- ====================================================================')
w('-- 9. BẢNG CHI_TIET_HOA_DON (30 BẢN GHI)')
w('-- ====================================================================')
for i in range(1, 31):
    ma_cthd = i
    ma_hd = i
    ma_dv = i
    so_luong = 2
    don_gia = dich_vu[i - 1][2]
    thanh_tien = so_luong * don_gia
    w(f"INSERT INTO CHI_TIET_HOA_DON (MaCTHD, MaHD, MaDV, SoLuong, DonGia, ThanhTien) VALUES ({ma_cthd}, {ma_hd}, {ma_dv}, {so_luong}, {don_gia}, {thanh_tien});")
w('GO')
w('')

# 10. NHA_CUNG_CAP (30)
w('-- ====================================================================')
w('-- 10. BẢNG NHA_CUNG_CAP (30 BẢN GHI)')
w('-- ====================================================================')
nha_cung_cap = [
    (1, 'Công ty TNHH Giặt Là Công Nghiệp Ánh Dương', '02838112001', '45 Khu Công Nghiệp Tân Bình, Tân Phú, TP.HCM', 'contact@anhduonglaundry.com'),
    (2, 'Công ty Cổ Phần Thực Phẩm Sạch GreenFarm', '02437661002', '128 Giải Phóng, Phương Liệt, Thanh Xuân, Hà Nội', 'sales@greenfarmvietnam.com'),
    (3, 'Công ty TNHH Nước Giải Khát Suntory PepsiCo', '02838225003', 'Tầng 5 Cao ốc Sheraton, 88 Đồng Khởi, Q.1, TP.HCM', 'info@pepsico.com.vn'),
    (4, 'Tổng Công Ty Cổ Phần Bia - Rượu - NGK Sài Gòn (Sabeco)', '02838294004', '187 Nguyễn Chí Thanh, Phường 12, Quận 5, TP.HCM', 'sabeco@sabeco.com.vn'),
    (5, 'Công ty TNHH Nước Khoáng Thiên Nhiên La Vie', '02723511005', 'Quốc lộ 1A, Khánh Hậu, Tân An, Long An', 'customerservice@laviewater.com'),
    (6, 'Công ty Cổ Phần Sữa Việt Nam (Vinamilk)', '02854155006', '10 Tân Trào, Tân Phú, Quận 7, TP.HCM', 'vinamilk@vinamilk.com.vn'),
    (7, 'Công ty Nệm Cao Su Thiên Nhiên Kymdan', '02839505007', '28 Bình Thới, Phường 14, Quận 11, TP.HCM', 'contact_kymdan@kymdan.vn'),
    (8, 'Công ty Thiết Bị Khách Sạn Cao Cấp Phúc Hòa', '02436883008', '32 Đốc Ngữ, Vĩnh Phúc, Ba Đình, Hà Nội', 'phuchoa_hotel@phuchoa.com.vn'),
    (9, 'Công ty TNHH Hóa Mỹ Phẩm Unilever Việt Nam', '02854135009', 'A2-3 KCN Tây Bắc Củ Chi, Củ Chi, TP.HCM', 'unilever.care@unilever.com'),
    (10, 'Công ty Cổ Phần Chăn Ga Gối Đệm Sông Hồng', '02283649010', '105 Nguyễn Du, Vị Hoàng, TP. Nam Định', 'songhongbedding@songhong.vn'),
    (11, 'Công ty TNHH Rượu Vang Đà Lạt Ladofoods', '02633822011', 'Lô B1-3 KCN Phát Chi, TP. Đà Lạt, Lâm Đồng', 'ladofoods@dalatwine.com.vn'),
    (12, 'Công ty Cổ Phần Cà Phê Trung Nguyên Legend', '02839251012', '82-84 Bùi Thị Xuân, Bến Thành, Quận 1, TP.HCM', 'contact@trungnguyenlegend.com'),
    (13, 'Công ty Cổ Phần Dược Phẩm Hoa Linh', '02437676013', 'B19 X3 KĐT Mỹ Đình 1, Nam Từ Liêm, Hà Nội', 'hoalinh_pharma@hoalinh.vn'),
    (14, 'Công ty TNHH Thiết Bị Điện & Chiếu Sáng Rạng Đông', '02438584014', '87-89 Hạ Đình, Thanh Xuân Trung, Thanh Xuân, Hà Nội', 'rangdong@rangdong.com.vn'),
    (15, 'Công ty Khăn Bông Xuất Khẩu Phong Phú', '02838963015', '48 Tăng Nhơn Phú, Tăng Nhơn Phú B, TP. Thủ Đức', 'phongphu_towel@phongphu.com.vn'),
    (16, 'Công ty TNHH Đồ Gỗ Nội Thất Hoàng Anh Gia Lai', '02693824016', '15 Trường Chinh, Phù Đổng, TP. Pleiku, Gia Lai', 'noithat@hagl.com.vn'),
    (17, 'Công ty Cung Cấp Hải Sản Tươi Sống Biển Đông', '02583881017', '54 Trần Phú, Lộc Thọ, TP. Nha Trang, Khánh Hòa', 'haisanbiendong@gmail.com'),
    (18, 'Công ty Rau Củ Quả Đà Lạt GAP', '02633554018', '241 Hai Bà Trưng, Phường 6, TP. Đà Lạt, Lâm Đồng', 'dalatgap@dalatgap.com'),
    (19, 'Công ty TNHH Thiết Bị Bếp Công Nghiệp Toàn Phát', '02835127019', '471 Điện Biên Phủ, Phường 25, Bình Thạnh, TP.HCM', 'toanphat@toanphat.vn'),
    (20, 'Công ty Vận Tải Du Lịch Mai Linh Express', '02838383020', '64 Hai Bà Trưng, Bến Nghé, Quận 1, TP.HCM', 'mailinh_tourism@mailinh.vn'),
    (21, 'Công ty TNHH Âm Thanh Ánh Sáng Việt Thương', '02839316021', '386 Cách Mạng Tháng 8, Phường 10, Quận 3, TP.HCM', 'vietthuong_audio@vietthuong.vn'),
    (22, 'Công ty Cây Cảnh & Cảnh Quan Đô Thị Sài Gòn', '02838445022', '12 Phan Thúc Duyện, Phường 4, Tân Bình, TP.HCM', 'canhquansaigon@cayxanh.vn'),
    (23, 'Công ty TNHH Thiết Bị An Ninh & Camera Hikvision VN', '02437951023', 'Tầng 8 Tòa nhà Việt Á, Duy Tân, Cầu Giấy, Hà Nội', 'security@hikvision.vn'),
    (24, 'Công ty May Đồng Phục Khách Sạn Mon Amie', '02862923024', '357 Nguyễn Trãi, Phường 7, Quận 5, TP.HCM', 'monamie_uniform@monamie.vn'),
    (25, 'Công ty In Ấn & Thiết Kế Bao Bì Phương Nam', '02838552025', '214 An Dương Vương, Phường 16, Quận 8, TP.HCM', 'inanphuongnam@gmail.com'),
    (26, 'Công ty Cung Cấp Thảo Dược & Tinh Dầu Thiên Nhiên Lam Hà', '02435578026', '68 Hoàng Cầu, Ô Chợ Dừa, Đống Đa, Hà Nội', 'tinhdaulamha@lamha.vn'),
    (27, 'Công ty Cổ Phần Nhựa Duy Tân', '02838762027', '298 Hồ Học Lãm, An Lạc, Bình Tân, TP.HCM', 'info@duytan.com'),
    (28, 'Công ty TNHH Sơn Dulux AkzoNobel Việt Nam', '02838237028', 'Tầng 12 Vincom Center, 72 Lê Thánh Tôn, Quận 1, TP.HCM', 'dulux_vietnam@akzonobel.com'),
    (29, 'Công ty Thang Máy Mitsubishi Electric Việt Nam', '02839105029', 'Tầng 11 Green Power, 35 Tôn Đức Thắng, Quận 1, TP.HCM', 'mitsubishi_elevator@mevn.com.vn'),
    (30, 'Công ty Dịch Vụ Vệ Sinh Môi Trường Hoàn Mỹ', '02433898030', '42 Láng Hạ, Thành Công, Đống Đa, Hà Nội', 'vesinhhoanmy@hoanmy.com.vn'),
]
w('SET IDENTITY_INSERT NHA_CUNG_CAP ON;')
for item in nha_cung_cap:
    w(f"INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, DiaChi, Email) VALUES ({item[0]}, N'{item[1]}', '{item[2]}', N'{item[3]}', '{item[4]}');")
w('SET IDENTITY_INSERT NHA_CUNG_CAP OFF;')
w('GO')
w('')

# 11. PHIEU_NHAP (30)
w('-- ====================================================================')
w('-- 11. BẢNG PHIEU_NHAP (30 BẢN GHI)')
w('-- ====================================================================')
dates_pn = [
    '2026-01-05 08:30:00', '2026-01-15 09:00:00', '2026-01-25 10:15:00',
    '2026-02-05 14:00:00', '2026-02-15 08:45:00', '2026-02-25 11:30:00',
    '2026-03-05 09:30:00', '2026-03-15 15:00:00', '2026-03-25 10:00:00',
    '2026-04-05 08:00:00', '2026-04-15 13:45:00', '2026-04-25 09:15:00',
    '2026-05-05 10:30:00', '2026-05-15 14:20:00', '2026-05-25 08:50:00',
    '2026-06-05 11:10:00', '2026-06-15 09:00:00', '2026-06-25 15:30:00',
    '2026-07-05 08:40:00', '2026-07-15 10:15:00', '2026-07-25 14:00:00',
    '2026-08-05 09:20:00', '2026-08-15 11:45:00', '2026-08-25 16:00:00',
    '2026-09-05 08:30:00', '2026-09-15 13:15:00', '2026-09-25 10:40:00',
    '2026-10-05 09:00:00', '2026-10-20 14:30:00', '2026-11-10 10:00:00',
]
w('SET IDENTITY_INSERT PHIEU_NHAP ON;')
for i in range(1, 31):
    ma_pn = i
    ma_ncc = i
    ma_nv = ((i - 1) % 6) + 1  # Nhân viên kế toán / quản lý kho
    ngay_nhap = dates_pn[i - 1]
    w(f"INSERT INTO PHIEU_NHAP (MaPN, MaNCC, MaNV, NgayNhap) VALUES ({ma_pn}, {ma_ncc}, {ma_nv}, '{ngay_nhap}');")
w('SET IDENTITY_INSERT PHIEU_NHAP OFF;')
w('GO')
w('')

# 12. CT_PHIEU_NHAP (30)
w('-- ====================================================================')
w('-- 12. BẢNG CT_PHIEU_NHAP (30 BẢN GHI)')
w('-- ====================================================================')
w('SET IDENTITY_INSERT CT_PHIEU_NHAP ON;')
for i in range(1, 31):
    ma_ctpn = i
    ma_pn = i
    ma_dv = i
    so_luong_nhap = 50 + (i * 5)
    don_gia_nhap = int(dich_vu[i - 1][2] * 0.6)  # Giá vốn nhập khoảng 60% giá bán
    w(f"INSERT INTO CT_PHIEU_NHAP (MaCTPN, MaPN, MaDV, SoLuong, DonGiaNhap) VALUES ({ma_ctpn}, {ma_pn}, {ma_dv}, {so_luong_nhap}, {don_gia_nhap});")
w('SET IDENTITY_INSERT CT_PHIEU_NHAP OFF;')
w('GO')
w('')

w('-- THÔNG BÁO HOÀN THÀNH')
w("PRINT N'====================================================================';")
w("PRINT N'ĐÃ NẠP THÀNH CÔNG ĐẦY ĐỦ 30 BẢN GHI MẪU CHO TẤT CẢ 12 BẢNG!';")
w("PRINT N'DỮ LIỆU ĐƯỢC PHÂN BỔ ĐẦY ĐỦ CHO CẢ 4 QUÝ NĂM 2026.';")
w("PRINT N'CHUẨN UNICODE TIẾNG VIỆT 100% KHÔNG LỖI FONT.';")
w("PRINT N'====================================================================';")

content = '\n'.join(lines)

# Write to Downloads project (both UTF-16 LE and UTF-8 with BOM)
with open(output_sql_path.replace('.sql', '_utf16.sql'), 'w', encoding='utf-16') as f:
    f.write(content)

with open(output_sql_path, 'w', encoding='utf-8-sig') as f:
    f.write(content)

# Write to scratch project
os.makedirs(os.path.dirname(alt_sql_path), exist_ok=True)
with open(alt_sql_path.replace('.sql', '_utf16.sql'), 'w', encoding='utf-16') as f:
    f.write(content)

with open(alt_sql_path, 'w', encoding='utf-8-sig') as f:
    f.write(content)

print('Generated SQL files successfully!')
