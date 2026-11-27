/**
 * ỨNG DỤNG QUẢN LÝ DỊCH VỤ & HÓA ĐƠN KHÁCH SẠN
 * Sinh viên thực hiện: Võ Văn Đăng Khoa
 * Hỗ trợ trọn vẹn CRUD: Dịch Vụ, Hóa Đơn, Chi Tiết Hóa Đơn (Main-Sub 1-N)
 */

class HotelApp {
    constructor() {
        this.selectedMaHD = null;
        this.initData();
        this.bindEvents();
        this.renderAll();
        this.updateClock();
    }

    // ================= 1. KHỞI TẠO DỮ LIỆU & LOCALSTORAGE =================
    initData() {
        const defaultDV = [
            { maDV: 1, tenDV: "Nước suối Lavie 500ml", donViTinh: "Chai", donGia: 15000, moTa: "Nước khoáng đóng chai minibar" },
            { maDV: 2, tenDV: "Bia Heineken lon 330ml", donViTinh: "Lon", donGia: 35000, moTa: "Bia ướp lạnh minibar" },
            { maDV: 3, tenDV: "Buffet ăn sáng cao cấp", donViTinh: "Suất", donGia: 150000, moTa: "Phục vụ tại nhà hàng tầng 2" },
            { maDV: 4, tenDV: "Giặt ủi quần áo lấy nhanh", donViTinh: "Bộ", donGia: 50000, moTa: "Giặt sấy ủi gấp trong 4h" },
            { maDV: 5, tenDV: "Dịch vụ Massage & Spa", donViTinh: "Suất", donGia: 300000, moTa: "Liệu trình thư giãn 60 phút" },
            { maDV: 6, tenDV: "Thuê xe máy tự lái", donViTinh: "Ngày", donGia: 180000, moTa: "Xe tay ga Airblade kèm mũ BH" }
        ];

        const defaultHD = [
            { maHD: 101, maDP: "DP001", nhanVien: "Nguyễn Văn A", ngayLap: "2026-11-20", tongTien: 180000, ghiChu: "Khách trả tiền mặt" },
            { maHD: 102, maDP: "DP002", nhanVien: "Trần Thị Mai", ngayLap: "2026-11-22", tongTien: 80000, ghiChu: "Khách quẹt thẻ POS" },
            { maHD: 103, maDP: "DP003", nhanVien: "Nguyễn Văn A", ngayLap: "2026-11-24", tongTien: 600000, ghiChu: "Thanh toán chuyển khoản" },
            { maHD: 104, maDP: "DP004", nhanVien: "Lê Văn Hùng", ngayLap: "2026-11-26", tongTien: 335000, ghiChu: "Đã xuất hóa đơn VAT" }
        ];

        const defaultCT = [
            { maCTHD: 201, maHD: 101, maDV: 1, tenDV: "Nước suối Lavie 500ml", donGia: 15000, soLuong: 2, thanhTien: 30000 },
            { maCTHD: 202, maHD: 101, maDV: 3, tenDV: "Buffet ăn sáng cao cấp", donGia: 150000, soLuong: 1, thanhTien: 150000 },
            { maCTHD: 203, maHD: 102, maDV: 1, tenDV: "Nước suối Lavie 500ml", donGia: 15000, soLuong: 2, thanhTien: 30000 },
            { maCTHD: 204, maHD: 102, maDV: 4, tenDV: "Giặt ủi quần áo lấy nhanh", donGia: 50000, soLuong: 1, thanhTien: 50000 },
            { maCTHD: 205, maHD: 103, maDV: 5, tenDV: "Dịch vụ Massage & Spa", donGia: 300000, soLuong: 2, thanhTien: 600000 },
            { maCTHD: 206, maHD: 104, maDV: 2, tenDV: "Bia Heineken lon 330ml", donGia: 35000, soLuong: 1, thanhTien: 35000 },
            { maCTHD: 207, maHD: 104, maDV: 5, tenDV: "Dịch vụ Massage & Spa", donGia: 300000, soLuong: 1, thanhTien: 300000 }
        ];

        this.dichVuList = JSON.parse(localStorage.getItem("SH_DichVu")) || defaultDV;
        this.hoaDonList = JSON.parse(localStorage.getItem("SH_HoaDon")) || defaultHD;
        this.chiTietList = JSON.parse(localStorage.getItem("SH_ChiTiet")) || defaultCT;
        this.saveToStorage();
    }

    saveToStorage() {
        localStorage.setItem("SH_DichVu", JSON.stringify(this.dichVuList));
        localStorage.setItem("SH_HoaDon", JSON.stringify(this.hoaDonList));
        localStorage.setItem("SH_ChiTiet", JSON.stringify(this.chiTietList));
    }

    resetData() {
        if (confirm("Bạn có chắc muốn nạp lại dữ liệu mẫu ban đầu?")) {
            localStorage.removeItem("SH_DichVu");
            localStorage.removeItem("SH_HoaDon");
            localStorage.removeItem("SH_ChiTiet");
            this.initData();
            this.renderAll();
            alert("Đã làm mới dữ liệu thành công!");
        }
    }

    bindEvents() {
        document.querySelectorAll(".nav-item").forEach(btn => {
            btn.addEventListener("click", () => {
                const targetTab = btn.getAttribute("data-tab");
                this.switchTab(targetTab);
            });
        });
    }

    switchTab(tabId) {
        document.querySelectorAll(".nav-item").forEach(b => b.classList.remove("active"));
        document.querySelectorAll(".tab-pane").forEach(p => p.classList.remove("active"));

        const targetBtn = document.querySelector(`[data-tab="${tabId}"]`);
        const targetPane = document.getElementById(tabId);
        if (targetBtn && targetPane) {
            targetBtn.classList.add("active");
            targetPane.classList.add("active");
        }

        const titles = {
            "tab-dashboard": ["Bàn Làm Việc Tổng Quan", "Hệ thống quản lý Dịch vụ, Hóa đơn và Chi tiết sử dụng"],
            "tab-dichvu": ["Quản Lý Danh Mục Dịch Vụ", "Theo dõi đơn giá, thêm sửa xóa các dịch vụ khách sạn"],
            "tab-hoadon": ["Quản Lý Hóa Đơn (Mô Hình Main - Sub 1-N)", "Nhấp chọn một hóa đơn ở trên để xem chi tiết ở dưới"],
            "tab-chitiethoadon": ["Chi Tiết Dịch Vụ Đã Sử Dụng", "Danh sách các dịch vụ kèm theo của từng hóa đơn"]
        };
        if (titles[tabId]) {
            document.getElementById("page-title").innerText = titles[tabId][0];
            document.getElementById("page-subtitle").innerText = titles[tabId][1];
        }
    }

    renderAll() {
        this.renderStats();
        this.renderDichVu();
        this.renderHoaDon();
        this.renderChiTiet();
        this.updateSelectDropdowns();
    }

    updateClock() {
        const el = document.getElementById("current-date");
        const now = new Date();
        el.innerHTML = `<i class="fa-regular fa-calendar"></i> ${now.toLocaleDateString("vi-VN", { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' })}`;
    }

    // ================= 2. DASHBOARD STATS =================
    renderStats() {
        document.getElementById("stat-total-dv").innerText = this.dichVuList.length;
        document.getElementById("stat-total-hd").innerText = this.hoaDonList.length;
        document.getElementById("stat-total-ct").innerText = this.chiTietList.length;

        const totalRev = this.hoaDonList.reduce((acc, hd) => acc + Number(hd.tongTien || 0), 0);
        document.getElementById("stat-total-revenue").innerText = totalRev.toLocaleString("vi-VN") + " VNĐ";

        // Recent table
        const recentTbody = document.getElementById("recent-hd-tbody");
        recentTbody.innerHTML = "";
        const recentList = [...this.hoaDonList].reverse().slice(0, 5);
        recentList.forEach(hd => {
            recentTbody.innerHTML += `
                <tr>
                    <td><strong>HD_${String(hd.maHD).padStart(4, '0')}</strong></td>
                    <td>${hd.maDP}</td>
                    <td>${hd.nhanVien}</td>
                    <td>${hd.ngayLap}</td>
                    <td class="font-bold text-primary">${Number(hd.tongTien).toLocaleString("vi-VN")} VNĐ</td>
                    <td><span class="badge badge-success"><i class="fa-solid fa-check"></i> Đã thanh toán</span></td>
                </tr>
            `;
        });
    }

    // ================= 3. CRUD DỊCH VỤ =================
    renderDichVu() {
        const kw = (document.getElementById("search-dv").value || "").toLowerCase().trim();
        const tbody = document.getElementById("dv-tbody");
        tbody.innerHTML = "";

        const filtered = this.dichVuList.filter(d => 
            String(d.maDV).includes(kw) || d.tenDV.toLowerCase().includes(kw) || (d.moTa && d.moTa.toLowerCase().includes(kw))
        );

        if (filtered.length === 0) {
            tbody.innerHTML = `<tr><td colspan="6" class="text-center text-muted py-4">Không tìm thấy dịch vụ nào!</td></tr>`;
            return;
        }

        filtered.forEach(d => {
            tbody.innerHTML += `
                <tr>
                    <td><strong>DV_${String(d.maDV).padStart(3, '0')}</strong></td>
                    <td class="font-bold">${d.tenDV}</td>
                    <td><span class="badge badge-info">${d.donViTinh}</span></td>
                    <td class="font-bold text-primary">${Number(d.donGia).toLocaleString("vi-VN")} VNĐ</td>
                    <td class="text-muted">${d.moTa || "-"}</td>
                    <td>
                        <div class="action-btns">
                            <button class="btn-icon" title="Chỉnh sửa" onclick="app.editDichVu(${d.maDV})"><i class="fa-solid fa-pen"></i></button>
                            <button class="btn-icon btn-icon-danger" title="Xóa" onclick="app.deleteDichVu(${d.maDV})"><i class="fa-solid fa-trash"></i></button>
                        </div>
                    </td>
                </tr>
            `;
        });
    }

    saveDichVu(e) {
        e.preventDefault();
        const mode = document.getElementById("dv-edit-mode").value;
        const ma = parseInt(document.getElementById("dv-ma").value);
        const ten = document.getElementById("dv-ten").value.trim();
        const dvt = document.getElementById("dv-dvt").value.trim();
        const gia = parseFloat(document.getElementById("dv-dongia").value);
        const mota = document.getElementById("dv-mota").value.trim();

        if (mode === "add") {
            if (this.dichVuList.some(d => d.maDV === ma)) {
                alert(`Lỗi: Mã dịch vụ ${ma} đã tồn tại trong hệ thống!`);
                return;
            }
            this.dichVuList.push({ maDV: ma, tenDV: ten, donViTinh: dvt, donGia: gia, moTa: mota });
            alert("Thêm dịch vụ thành công!");
        } else {
            const idx = this.dichVuList.findIndex(d => d.maDV === ma);
            if (idx !== -1) {
                this.dichVuList[idx] = { maDV: ma, tenDV: ten, donViTinh: dvt, donGia: gia, moTa: mota };
                alert("Cập nhật dịch vụ thành công!");
            }
        }

        this.saveToStorage();
        this.closeModal("modal-dv");
        this.renderAll();
    }

    editDichVu(ma) {
        const item = this.dichVuList.find(d => d.maDV === ma);
        if (!item) return;

        document.getElementById("modal-dv-title").innerText = `Chỉnh Sửa Dịch Vụ DV_${item.maDV}`;
        document.getElementById("dv-edit-mode").value = "edit";
        document.getElementById("dv-ma").value = item.maDV;
        document.getElementById("dv-ma").readOnly = true;
        document.getElementById("dv-ten").value = item.tenDV;
        document.getElementById("dv-dvt").value = item.donViTinh;
        document.getElementById("dv-dongia").value = item.donGia;
        document.getElementById("dv-mota").value = item.moTa || "";

        this.openModal("modal-dv");
    }

    deleteDichVu(ma) {
        if (confirm(`Bạn có chắc muốn xóa dịch vụ mã ${ma}?`)) {
            this.dichVuList = this.dichVuList.filter(d => d.maDV !== ma);
            this.saveToStorage();
            this.renderAll();
            alert("Xóa dịch vụ thành công!");
        }
    }

    // ================= 4. CRUD HÓA ĐƠN & MAIN-SUB 1-N =================
    renderHoaDon() {
        const kw = (document.getElementById("search-hd").value || "").toLowerCase().trim();
        const tbody = document.getElementById("hd-tbody");
        tbody.innerHTML = "";

        const filtered = this.hoaDonList.filter(h =>
            String(h.maHD).includes(kw) || h.maDP.toLowerCase().includes(kw) || h.nhanVien.toLowerCase().includes(kw) || (h.ghiChu && h.ghiChu.toLowerCase().includes(kw))
        );

        if (filtered.length === 0) {
            tbody.innerHTML = `<tr><td colspan="7" class="text-center text-muted py-4">Không tìm thấy hóa đơn nào!</td></tr>`;
            return;
        }

        filtered.forEach(h => {
            const isSelected = this.selectedMaHD === h.maHD;
            tbody.innerHTML += `
                <tr class="${isSelected ? 'row-selected' : ''}" onclick="app.selectHoaDon(${h.maHD})">
                    <td><strong>HD_${String(h.maHD).padStart(4, '0')}</strong></td>
                    <td><span class="badge badge-info">${h.maDP}</span></td>
                    <td>${h.nhanVien}</td>
                    <td>${h.ngayLap}</td>
                    <td class="font-bold text-primary">${Number(h.tongTien).toLocaleString("vi-VN")} VNĐ</td>
                    <td class="text-muted">${h.ghiChu || "-"}</td>
                    <td onclick="event.stopPropagation()">
                        <div class="action-btns">
                            <button class="btn-icon" title="In Hóa Đơn" onclick="app.printInvoice(${h.maHD})"><i class="fa-solid fa-print"></i></button>
                            <button class="btn-icon" title="Chỉnh sửa" onclick="app.editHoaDon(${h.maHD})"><i class="fa-solid fa-pen"></i></button>
                            <button class="btn-icon btn-icon-danger" title="Xóa" onclick="app.deleteHoaDon(${h.maHD})"><i class="fa-solid fa-trash"></i></button>
                        </div>
                    </td>
                </tr>
            `;
        });

        // Cập nhật bảng Sub chi tiết nếu có hóa đơn đang chọn
        if (this.selectedMaHD) {
            this.renderSubDetails(this.selectedMaHD);
        }
    }

    selectHoaDon(maHD) {
        this.selectedMaHD = maHD;
        this.renderHoaDon();
        this.renderSubDetails(maHD);
    }

    renderSubDetails(maHD) {
        const hd = this.hoaDonList.find(h => h.maHD === maHD);
        if (!hd) return;

        const subItems = this.chiTietList.filter(ct => ct.maHD === maHD);
        const subTitle = document.getElementById("sub-title");
        const subActions = document.getElementById("sub-actions");
        const tbody = document.getElementById("sub-ct-tbody");

        subTitle.innerHTML = `<i class="fa-solid fa-list-check"></i> Chi Tiết Dịch Vụ Của Hóa Đơn <strong>HD_${String(maHD).padStart(4, '0')}</strong> (${subItems.length} dịch vụ đã dùng)`;
        subActions.innerHTML = `
            <button class="btn btn-sm btn-primary" onclick="app.openAddSubDetail(${maHD})"><i class="fa-solid fa-plus"></i> Thêm Dịch Vụ Vào HĐ</button>
            <button class="btn btn-sm btn-outline" onclick="app.syncTotalFromDetails(${maHD})"><i class="fa-solid fa-calculator"></i> Tính lại tổng tiền</button>
        `;

        tbody.innerHTML = "";
        if (subItems.length === 0) {
            tbody.innerHTML = `<tr><td colspan="6" class="text-center text-muted py-4">Hóa đơn này chưa có dịch vụ nào! Hãy nhấp nút "+ Thêm Dịch Vụ Vào HĐ"</td></tr>`;
            return;
        }

        let totalSub = 0;
        subItems.forEach((item, index) => {
            totalSub += Number(item.thanhTien);
            tbody.innerHTML += `
                <tr>
                    <td>CT_${String(item.maCTHD).padStart(3, '0')}</td>
                    <td class="font-bold">${item.tenDV}</td>
                    <td>${Number(item.donGia).toLocaleString("vi-VN")} VNĐ</td>
                    <td><span class="badge badge-info">${item.soLuong}</span></td>
                    <td class="font-bold text-primary">${Number(item.thanhTien).toLocaleString("vi-VN")} VNĐ</td>
                    <td>
                        <button class="btn-icon btn-icon-danger btn-sm" title="Xóa dòng này" onclick="app.deleteChiTiet(${item.maCTHD})"><i class="fa-solid fa-trash"></i></button>
                    </td>
                </tr>
            `;
        });

        tbody.innerHTML += `
            <tr style="background:#F8FAFC; font-weight:bold;">
                <td colspan="4" class="text-right">TỔNG CỘNG TIỀN DỊCH VỤ:</td>
                <td colspan="2" class="text-primary font-bold" style="font-size:15px;">${totalSub.toLocaleString("vi-VN")} VNĐ</td>
            </tr>
        `;
    }

    openAddSubDetail(maHD) {
        this.openModal("modal-ct");
        document.getElementById("ct-mahd").value = maHD;
    }

    syncTotalFromDetails(maHD) {
        const sum = this.chiTietList.filter(ct => ct.maHD === maHD).reduce((acc, c) => acc + Number(c.thanhTien || 0), 0);
        const hd = this.hoaDonList.find(h => h.maHD === maHD);
        if (hd) {
            hd.tongTien = sum;
            this.saveToStorage();
            this.renderAll();
            alert(`Đã cập nhật lại tổng tiền Hóa đơn HD_${maHD} thành: ${sum.toLocaleString("vi-VN")} VNĐ`);
        }
    }

    saveHoaDon(e) {
        e.preventDefault();
        const mode = document.getElementById("hd-edit-mode").value;
        const ma = parseInt(document.getElementById("hd-ma").value);
        const madp = document.getElementById("hd-madp").value.trim();
        const nv = document.getElementById("hd-nhanvien").value.trim();
        const ngay = document.getElementById("hd-ngaylap").value;
        const tong = parseFloat(document.getElementById("hd-tongtien").value) || 0;
        const ghichu = document.getElementById("hd-ghichu").value.trim();

        if (mode === "add") {
            if (this.hoaDonList.some(h => h.maHD === ma)) {
                alert(`Lỗi: Mã hóa đơn ${ma} đã tồn tại!`);
                return;
            }
            this.hoaDonList.push({ maHD: ma, maDP: madp, nhanVien: nv, ngayLap: ngay, tongTien: tong, ghiChu: ghichu });
            alert("Lập hóa đơn thành công!");
        } else {
            const idx = this.hoaDonList.findIndex(h => h.maHD === ma);
            if (idx !== -1) {
                this.hoaDonList[idx] = { maHD: ma, maDP: madp, nhanVien: nv, ngayLap: ngay, tongTien: tong, ghiChu: ghichu };
                alert("Cập nhật hóa đơn thành công!");
            }
        }

        this.saveToStorage();
        this.closeModal("modal-hd");
        this.renderAll();
    }

    editHoaDon(ma) {
        const item = this.hoaDonList.find(h => h.maHD === ma);
        if (!item) return;

        document.getElementById("modal-hd-title").innerText = `Chỉnh Sửa Hóa Đơn HD_${item.maHD}`;
        document.getElementById("hd-edit-mode").value = "edit";
        document.getElementById("hd-ma").value = item.maHD;
        document.getElementById("hd-ma").readOnly = true;
        document.getElementById("hd-madp").value = item.maDP;
        document.getElementById("hd-nhanvien").value = item.nhanVien;
        document.getElementById("hd-ngaylap").value = item.ngayLap;
        document.getElementById("hd-tongtien").value = item.tongTien;
        document.getElementById("hd-ghichu").value = item.ghiChu || "";

        this.openModal("modal-hd");
    }

    deleteHoaDon(ma) {
        if (confirm(`Bạn có chắc muốn xóa hóa đơn HD_${ma}? (Các chi tiết dịch vụ đi kèm cũng sẽ bị xóa)`)) {
            this.hoaDonList = this.hoaDonList.filter(h => h.maHD !== ma);
            this.chiTietList = this.chiTietList.filter(ct => ct.maHD !== ma);
            if (this.selectedMaHD === ma) this.selectedMaHD = null;
            this.saveToStorage();
            this.renderAll();
            alert("Xóa hóa đơn thành công!");
        }
    }

    // ================= 5. CRUD CHI TIẾT HÓA ĐƠN =================
    renderChiTiet() {
        const kw = (document.getElementById("search-ct").value || "").toLowerCase().trim();
        const tbody = document.getElementById("ct-tbody");
        tbody.innerHTML = "";

        const filtered = this.chiTietList.filter(ct => 
            String(ct.maCTHD).includes(kw) || String(ct.maHD).includes(kw) || ct.tenDV.toLowerCase().includes(kw)
        );

        if (filtered.length === 0) {
            tbody.innerHTML = `<tr><td colspan="7" class="text-center text-muted py-4">Chưa có chi tiết nào!</td></tr>`;
            return;
        }

        filtered.forEach(ct => {
            tbody.innerHTML += `
                <tr>
                    <td><strong>CT_${String(ct.maCTHD).padStart(3, '0')}</strong></td>
                    <td><span class="badge badge-info">HD_${String(ct.maHD).padStart(4, '0')}</span></td>
                    <td class="font-bold">${ct.tenDV}</td>
                    <td>${Number(ct.donGia).toLocaleString("vi-VN")} VNĐ</td>
                    <td><span class="badge badge-success">${ct.soLuong}</span></td>
                    <td class="font-bold text-primary">${Number(ct.thanhTien).toLocaleString("vi-VN")} VNĐ</td>
                    <td>
                        <div class="action-btns">
                            <button class="btn-icon btn-icon-danger" title="Xóa" onclick="app.deleteChiTiet(${ct.maCTHD})"><i class="fa-solid fa-trash"></i></button>
                        </div>
                    </td>
                </tr>
            `;
        });
    }

    updateSelectDropdowns() {
        const hdSelect = document.getElementById("ct-mahd");
        const dvSelect = document.getElementById("ct-madv");
        if (!hdSelect || !dvSelect) return;

        hdSelect.innerHTML = this.hoaDonList.map(h => `<option value="${h.maHD}">HD_${String(h.maHD).padStart(4, '0')} (Phiếu: ${h.maDP})</option>`).join("");
        dvSelect.innerHTML = `<option value="">-- Chọn dịch vụ --</option>` + this.dichVuList.map(d => `<option value="${d.maDV}">${d.tenDV} (${Number(d.donGia).toLocaleString('vi-VN')} đ)</option>`).join("");
    }

    onSelectDichVu() {
        const maDV = parseInt(document.getElementById("ct-madv").value);
        const item = this.dichVuList.find(d => d.maDV === maDV);
        if (item) {
            document.getElementById("ct-dongia").value = item.donGia;
            this.calcSubtotal();
        } else {
            document.getElementById("ct-dongia").value = 0;
            document.getElementById("ct-thanhtien").value = "0 VNĐ";
        }
    }

    calcSubtotal() {
        const gia = parseFloat(document.getElementById("ct-dongia").value) || 0;
        const sl = parseInt(document.getElementById("ct-soluong").value) || 1;
        const thanhTien = gia * sl;
        document.getElementById("ct-thanhtien").value = thanhTien.toLocaleString("vi-VN") + " VNĐ";
    }

    saveChiTiet(e) {
        e.preventDefault();
        const maCT = parseInt(document.getElementById("ct-ma").value);
        const maHD = parseInt(document.getElementById("ct-mahd").value);
        const maDV = parseInt(document.getElementById("ct-madv").value);
        const sl = parseInt(document.getElementById("ct-soluong").value) || 1;

        if (this.chiTietList.some(c => c.maCTHD === maCT)) {
            alert(`Lỗi: Mã chi tiết ${maCT} đã tồn tại!`);
            return;
        }

        const dv = this.dichVuList.find(d => d.maDV === maDV);
        if (!dv) {
            alert("Vui lòng chọn dịch vụ hợp lệ!");
            return;
        }

        const thanhTien = dv.donGia * sl;
        this.chiTietList.push({
            maCTHD: maCT,
            maHD: maHD,
            maDV: maDV,
            tenDV: dv.tenDV,
            donGia: dv.donGia,
            soLuong: sl,
            thanhTien: thanhTien
        });

        // Tự động cộng tổng tiền hóa đơn cha
        const hd = this.hoaDonList.find(h => h.maHD === maHD);
        if (hd) {
            hd.tongTien = (Number(hd.tongTien) || 0) + thanhTien;
        }

        this.saveToStorage();
        this.closeModal("modal-ct");
        this.renderAll();
        alert("Thêm chi tiết dịch vụ thành công!");
    }

    deleteChiTiet(maCT) {
        if (confirm(`Bạn có chắc muốn xóa chi tiết mã ${maCT}?`)) {
            const item = this.chiTietList.find(c => c.maCTHD === maCT);
            if (item) {
                // Tự động trừ tổng tiền hóa đơn cha
                const hd = this.hoaDonList.find(h => h.maHD === item.maHD);
                if (hd) {
                    hd.tongTien = Math.max(0, (Number(hd.tongTien) || 0) - Number(item.thanhTien));
                }
            }
            this.chiTietList = this.chiTietList.filter(c => c.maCTHD !== maCT);
            this.saveToStorage();
            this.renderAll();
            alert("Đã xóa chi tiết dịch vụ!");
        }
    }

    // ================= 6. IN HÓA ĐƠN & XUẤT EXCEL =================
    printInvoice(maHD) {
        const hd = this.hoaDonList.find(h => h.maHD === maHD);
        if (!hd) return;

        const details = this.chiTietList.filter(c => c.maHD === maHD);
        let itemsText = "";
        let totalItems = 0;

        details.forEach((d, i) => {
            totalItems += d.thanhTien;
            itemsText += `  ${String(i + 1).padEnd(3)} | ${d.tenDV.padEnd(25)} | SL: ${String(d.soLuong).padEnd(2)} | ĐG: ${d.donGia.toLocaleString('vi-VN').padStart(8)} đ | TT: ${d.thanhTien.toLocaleString('vi-VN').padStart(10)} đ\n`;
        });

        const receipt = `
=============================================================================
                          KHÁCH SẠN SAIGON HOTEL
                 HÓA ĐƠN THANH TOÁN DỊCH VỤ & LƯU TRÚ
=============================================================================
  Mã Hóa Đơn : HD_${String(hd.maHD).padStart(4, '0')}              Ngày Lập  : ${hd.ngayLap}
  Mã Phiếu ĐP: ${hd.maDP.padEnd(10)}                   Thu Ngân  : ${hd.nhanVien}
-----------------------------------------------------------------------------
  DANH SÁCH DỊCH VỤ SỬ DỤNG:
-----------------------------------------------------------------------------
${itemsText || '  (Chưa ghi nhận dịch vụ nào)\n'}-----------------------------------------------------------------------------
  TỔNG TIỀN DỊCH VỤ  : ${totalItems.toLocaleString('vi-VN')} VNĐ
  TỔNG TIỀN HÓA ĐƠN  : ${Number(hd.tongTien).toLocaleString('vi-VN')} VNĐ
  Ghi Chú            : ${hd.ghiChu || 'Không có'}
=============================================================================
                  CẢM ƠN QUÝ KHÁCH VÀ HẸN GẶP LẠI!
=============================================================================
        `;

        document.getElementById("invoice-preview").innerText = receipt;
        this.openModal("modal-invoice-print");
    }

    exportExcel(type) {
        let rows = [];
        let filename = "";

        if (type === "dichvu") {
            filename = "DanhSach_DichVu.csv";
            rows.push(["MaDV", "TenDV", "DonViTinh", "DonGia", "MoTa"]);
            this.dichVuList.forEach(d => rows.push([d.maDV, `"${d.tenDV}"`, d.donViTinh, d.donGia, `"${d.moTa || ''}"`]));
        } else if (type === "hoadon") {
            filename = "DanhSach_HoaDon.csv";
            rows.push(["MaHD", "MaDP", "NhanVien", "NgayLap", "TongTien", "GhiChu"]);
            this.hoaDonList.forEach(h => rows.push([h.maHD, h.maDP, `"${h.nhanVien}"`, h.ngayLap, h.tongTien, `"${h.ghiChu || ''}"`]));
        } else if (type === "chitiethoadon") {
            filename = "DanhSach_ChiTietHoaDon.csv";
            rows.push(["MaCTHD", "MaHD", "MaDV", "TenDV", "DonGia", "SoLuong", "ThanhTien"]);
            this.chiTietList.forEach(c => rows.push([c.maCTHD, c.maHD, c.maDV, `"${c.tenDV}"`, c.donGia, c.soLuong, c.thanhTien]));
        }

        const csvContent = "\uFEFF" + rows.map(e => e.join(",")).join("\n");
        const blob = new Blob([csvContent], { type: "text/csv;charset=utf-8;" });
        const link = document.createElement("a");
        link.href = URL.createObjectURL(blob);
        link.setAttribute("download", filename);
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }

    // Modal Helpers
    openModal(id) {
        const m = document.getElementById(id);
        if (m) m.classList.add("show");
    }

    closeModal(id) {
        const m = document.getElementById(id);
        if (m) m.classList.remove("show");
    }
}

// Chạy ứng dụng khi load xong trang
const app = new HotelApp();
