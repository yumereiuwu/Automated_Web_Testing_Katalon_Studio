-- =============================================
-- CƠ SỞ DỮ LIỆU: DoAnVatA3 (đồng bộ với SQL Server đã triển khai)
-- ỨNG DỤNG BÁN ĐỒ ĂN VẶT - ASP.NET MVC
-- Lưu ý: Không DROP INDEX sau CREATE INDEX — giữ index để truy vấn nhanh.
-- =============================================

USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'DoAnVatA3')
    DROP DATABASE DoAnVatA3;
GO

CREATE DATABASE DoAnVatA3 COLLATE Vietnamese_CI_AS;
GO

USE DoAnVatA3;
GO

-- =============================================
-- BẢNG NGUOIDUNG (Người dùng)
-- =============================================
CREATE TABLE NguoiDung (
    MaNguoiDung INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(150) NOT NULL UNIQUE,
    MatKhau VARCHAR(256) NOT NULL,
    SoDienThoai VARCHAR(15),
    DiaChi NVARCHAR(255),
    VaiTro VARCHAR(20) DEFAULT 'KhachHang' CHECK (VaiTro IN ('QuanTri', 'KhachHang')),
    AnhDaiDien VARCHAR(255) DEFAULT 'default-avatar.png',
    ConHoatDong BIT DEFAULT 1,
    NgayTao DATETIME DEFAULT GETDATE()
);

-- =============================================
-- BẢNG DANHMUC (Danh mục sản phẩm)
-- =============================================
CREATE TABLE DanhMuc (
    MaDanhMuc INT IDENTITY(1,1) PRIMARY KEY,
    TenDanhMuc NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(500),
    HinhAnh VARCHAR(255),
    ConHoatDong BIT DEFAULT 1
);

-- =============================================
-- BẢNG NHACUNGCAP (Nhà cung cấp)
-- =============================================
CREATE TABLE NhaCungCap (
    MaNhaCungCap INT IDENTITY(1,1) PRIMARY KEY,
    TenNhaCungCap NVARCHAR(150) NOT NULL,
    NguoiLienHe NVARCHAR(100),
    SoDienThoai VARCHAR(15),
    Email VARCHAR(150),
    DiaChi NVARCHAR(255),
    ConHoatDong BIT DEFAULT 1
);

-- =============================================
-- BẢNG SANPHAM (Sản phẩm)
-- =============================================
CREATE TABLE SanPham (
    MaSanPham INT IDENTITY(1,1) PRIMARY KEY,
    TenSanPham NVARCHAR(150) NOT NULL,
    MaDanhMuc INT NOT NULL REFERENCES DanhMuc(MaDanhMuc),
    MaNhaCungCap INT REFERENCES NhaCungCap(MaNhaCungCap),
    MoTa NVARCHAR(1000),
    GiaNhap DECIMAL(18,0) NOT NULL DEFAULT 0,
    GiaBan DECIMAL(18,0) NOT NULL,
    PhanTramGiam DECIMAL(5,2) DEFAULT 0 CHECK (PhanTramGiam BETWEEN 0 AND 100),
    SoLuongTon INT DEFAULT 0 CHECK (SoLuongTon >= 0),
    DonViTinh NVARCHAR(30) DEFAULT N'Goi',
    HinhAnh VARCHAR(255) DEFAULT 'no-image.png',
    ConHoatDong BIT DEFAULT 1,
    NgayTao DATETIME DEFAULT GETDATE()
);

-- =============================================
-- BẢNG PHIEUNHAP (Phiếu nhập kho)
-- =============================================
CREATE TABLE PhieuNhap (
    MaPhieuNhap INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieu VARCHAR(20) NOT NULL UNIQUE,
    MaNhaCungCap INT REFERENCES NhaCungCap(MaNhaCungCap),
    NguoiNhap INT REFERENCES NguoiDung(MaNguoiDung),
    TongTien DECIMAL(18,0) DEFAULT 0,
    GhiChu NVARCHAR(500),
    NgayNhap DATETIME DEFAULT GETDATE()
);

-- =============================================
-- BẢNG CHITIETPHIEUNHAP (Chi tiết phiếu nhập)
-- =============================================
CREATE TABLE ChiTietPhieuNhap (
    MaChiTiet INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieuNhap INT NOT NULL REFERENCES PhieuNhap(MaPhieuNhap),
    MaSanPham INT NOT NULL REFERENCES SanPham(MaSanPham),
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGiaNhap DECIMAL(18,0) NOT NULL,
    ThanhTien AS (SoLuong * DonGiaNhap) PERSISTED
);

-- =============================================
-- BẢNG DONHANG (Đơn hàng)
-- =============================================
CREATE TABLE DonHang (
    MaDonHang INT IDENTITY(1,1) PRIMARY KEY,
    MaDon VARCHAR(20) NOT NULL UNIQUE,
    MaNguoiDung INT REFERENCES NguoiDung(MaNguoiDung),
    TenKhachHang NVARCHAR(100),
    SoDienThoai VARCHAR(15),
    DiaChiGiao NVARCHAR(255),
    TongTienHang DECIMAL(18,0) DEFAULT 0,
    PhiVanChuyen DECIMAL(18,0) DEFAULT 0,
    TongThanhToan DECIMAL(18,0) DEFAULT 0,
    TrangThai VARCHAR(30) DEFAULT 'ChoDuyet' CHECK (TrangThai IN ('ChoDuyet','DaXacNhan','DangGiao','DaGiao','DaHuy')),
    HinhThucThanhToan VARCHAR(30) DEFAULT 'TienMat' CHECK (HinhThucThanhToan IN ('TienMat','ChuyenKhoan','MoMo')),
    TrangThaiThanhToan VARCHAR(20) DEFAULT 'ChuaThanhToan' CHECK (TrangThaiThanhToan IN ('ChuaThanhToan','DaThanhToan')),
    GhiChu NVARCHAR(500),
    NgayDat DATETIME DEFAULT GETDATE()
);

-- =============================================
-- BẢNG CHITIETDONHANG (Chi tiết đơn hàng)
-- =============================================
CREATE TABLE ChiTietDonHang (
    MaChiTiet INT IDENTITY(1,1) PRIMARY KEY,
    MaDonHang INT NOT NULL REFERENCES DonHang(MaDonHang),
    MaSanPham INT NOT NULL REFERENCES SanPham(MaSanPham),
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGiaBan DECIMAL(18,0) NOT NULL,
    ThanhTien AS (SoLuong * DonGiaBan) PERSISTED
);

-- =============================================
-- BẢNG GIOHANG (Giỏ hàng)
-- =============================================
CREATE TABLE GioHang (
    MaGioHang INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT REFERENCES NguoiDung(MaNguoiDung),
    MaSanPham INT NOT NULL REFERENCES SanPham(MaSanPham),
    SoLuong INT NOT NULL DEFAULT 1 CHECK (SoLuong > 0),
    NgayThem DATETIME DEFAULT GETDATE(),
    CONSTRAINT UQ_GioHang UNIQUE (MaNguoiDung, MaSanPham)
);

-- =============================================
-- BẢNG DANHGIASANPHAM (Đánh giá sản phẩm)
-- =============================================
CREATE TABLE DanhGiaSanPham (
    MaDanhGia INT IDENTITY(1,1) PRIMARY KEY,
    MaSanPham INT NOT NULL REFERENCES SanPham(MaSanPham),
    MaNguoiDung INT NOT NULL REFERENCES NguoiDung(MaNguoiDung),
    SoSao TINYINT NOT NULL CHECK (SoSao BETWEEN 1 AND 5),
    NhanXet NVARCHAR(1000),
    NgayDanhGia DATETIME DEFAULT GETDATE(),
    CONSTRAINT UQ_DanhGia UNIQUE (MaNguoiDung, MaSanPham)
);

-- =============================================
-- BẢNG LIENHE (Liên hệ / Phản hồi)
-- =============================================
CREATE TABLE LienHe (
    MaLienHe INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(150) NOT NULL,
    SoDienThoai VARCHAR(15),
    TieuDe NVARCHAR(200),
    NoiDung NVARCHAR(2000) NOT NULL,
    DaDoc BIT DEFAULT 0,
    PhanHoi NVARCHAR(2000),
    NgayGui DATETIME DEFAULT GETDATE()
);

-- =============================================
-- BẢNG BANNER (Quảng cáo)
-- =============================================
CREATE TABLE Banner (
    MaBanner INT IDENTITY(1,1) PRIMARY KEY,
    TieuDe NVARCHAR(200),
    HinhAnh VARCHAR(255),
    DuongDan VARCHAR(255),
    ConHoatDong BIT DEFAULT 1,
    ThuTuHienThi INT DEFAULT 0
);

-- =============================================
-- INDEXES
-- =============================================
CREATE INDEX IX_SanPham_DanhMuc ON SanPham(MaDanhMuc);
CREATE INDEX IX_SanPham_HoatDong ON SanPham(ConHoatDong);
CREATE INDEX IX_DonHang_NguoiDung ON DonHang(MaNguoiDung);
CREATE INDEX IX_DonHang_TrangThai ON DonHang(TrangThai);
CREATE INDEX IX_DonHang_NgayDat ON DonHang(NgayDat);
CREATE INDEX IX_PhieuNhap_NgayNhap ON PhieuNhap(NgayNhap);
CREATE INDEX IX_DanhGia_SanPham ON DanhGiaSanPham(MaSanPham);
GO

-- =============================================
-- TRIGGER: Cộng SoLuongTon khi nhập kho
-- =============================================
CREATE TRIGGER trg_SauNhapKho ON ChiTietPhieuNhap
AFTER INSERT AS
BEGIN
    SET NOCOUNT ON;

    UPDATE sp
    SET sp.SoLuongTon = sp.SoLuongTon + ct.SoLuong
    FROM SanPham sp
    INNER JOIN inserted ct ON sp.MaSanPham = ct.MaSanPham;

    UPDATE pn
    SET pn.TongTien = (
        SELECT ISNULL(SUM(ThanhTien), 0)
        FROM ChiTietPhieuNhap
        WHERE MaPhieuNhap = pn.MaPhieuNhap
    )
    FROM PhieuNhap pn
    INNER JOIN inserted ct ON pn.MaPhieuNhap = ct.MaPhieuNhap;
END;
GO

-- =============================================
-- TRIGGER: Trừ SoLuongTon khi đặt hàng mới
-- =============================================
CREATE TRIGGER trg_SauDatHang ON ChiTietDonHang
AFTER INSERT AS
BEGIN
    SET NOCOUNT ON;

    UPDATE sp
    SET sp.SoLuongTon = sp.SoLuongTon - ct.SoLuong
    FROM SanPham sp
    INNER JOIN inserted ct ON sp.MaSanPham = ct.MaSanPham;
END;
GO

-- =============================================
-- TRIGGER: Hoàn kho khi hủy đơn hàng
-- =============================================
CREATE TRIGGER trg_HuyDonHang ON DonHang
AFTER UPDATE AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(TrangThai)
    BEGIN
        UPDATE sp
        SET sp.SoLuongTon = sp.SoLuongTon + ct.SoLuong
        FROM SanPham sp
        INNER JOIN ChiTietDonHang ct ON sp.MaSanPham = ct.MaSanPham
        INNER JOIN inserted i ON ct.MaDonHang = i.MaDonHang
        INNER JOIN deleted d ON i.MaDonHang = d.MaDonHang
        WHERE i.TrangThai = 'DaHuy' AND d.TrangThai != 'DaHuy';
    END
END;
GO

-- =============================================
-- VIEWS
-- =============================================

CREATE VIEW vw_TonKho AS
SELECT
    sp.MaSanPham,
    sp.TenSanPham,
    dm.TenDanhMuc,
    sp.SoLuongTon,
    sp.GiaNhap,
    sp.GiaBan,
    sp.PhanTramGiam,
    (sp.SoLuongTon * sp.GiaNhap) AS GiaTriTonKho
FROM SanPham sp
JOIN DanhMuc dm ON sp.MaDanhMuc = dm.MaDanhMuc
WHERE sp.ConHoatDong = 1;
GO

CREATE VIEW vw_DoanhThuTheoThang AS
SELECT
    YEAR(NgayDat) AS Nam,
    MONTH(NgayDat) AS Thang,
    COUNT(*) AS TongDonHang,
    SUM(TongThanhToan) AS DoanhThu
FROM DonHang
WHERE TrangThai = 'DaGiao'
GROUP BY YEAR(NgayDat), MONTH(NgayDat);
GO

CREATE VIEW vw_NhapXuatTon AS
SELECT
    sp.MaSanPham,
    sp.TenSanPham,
    dm.TenDanhMuc,
    ISNULL(nhap.TongNhap, 0) AS TongSoLuongNhap,
    ISNULL(xuat.TongBan, 0) AS TongSoLuongBan,
    sp.SoLuongTon AS TonKhoHienTai,
    ISNULL(nhap.TongTienNhap, 0) AS TongTienNhap,
    ISNULL(xuat.TongDoanhThu, 0) AS TongDoanhThu
FROM SanPham sp
JOIN DanhMuc dm ON sp.MaDanhMuc = dm.MaDanhMuc
LEFT JOIN (
    SELECT MaSanPham, SUM(SoLuong) AS TongNhap, SUM(ThanhTien) AS TongTienNhap
    FROM ChiTietPhieuNhap
    GROUP BY MaSanPham
) nhap ON sp.MaSanPham = nhap.MaSanPham
LEFT JOIN (
    SELECT ct.MaSanPham, SUM(ct.SoLuong) AS TongBan, SUM(ct.ThanhTien) AS TongDoanhThu
    FROM ChiTietDonHang ct
    JOIN DonHang dh ON ct.MaDonHang = dh.MaDonHang
    WHERE dh.TrangThai IN ('DaGiao','DaXacNhan','DangGiao')
    GROUP BY ct.MaSanPham
) xuat ON sp.MaSanPham = xuat.MaSanPham;
GO

CREATE VIEW vw_SanPhamBanChay AS
SELECT
    sp.MaSanPham,
    sp.TenSanPham,
    dm.TenDanhMuc,
    SUM(ct.SoLuong) AS TongSoBan,
    SUM(ct.ThanhTien) AS TongDoanhThu
FROM ChiTietDonHang ct
JOIN SanPham sp ON ct.MaSanPham = sp.MaSanPham
JOIN DanhMuc dm ON sp.MaDanhMuc = dm.MaDanhMuc
JOIN DonHang dh ON ct.MaDonHang = dh.MaDonHang
WHERE dh.TrangThai = 'DaGiao'
GROUP BY sp.MaSanPham, sp.TenSanPham, dm.TenDanhMuc;
GO

-- =============================================
-- STORED PROCEDURES
-- =============================================
CREATE PROCEDURE sp_CapNhatTonKhoNhap
    @MaSanPham INT,
    @SoLuong INT
AS
BEGIN
    UPDATE SanPham
    SET SoLuongTon = SoLuongTon + @SoLuong
    WHERE MaSanPham = @MaSanPham;
END;
GO

CREATE PROCEDURE sp_CapNhatTonKhoXuat
    @MaSanPham INT,
    @SoLuong INT
AS
BEGIN
    UPDATE SanPham
    SET SoLuongTon = SoLuongTon - @SoLuong
    WHERE MaSanPham = @MaSanPham AND SoLuongTon >= @SoLuong;
END;
GO

CREATE PROCEDURE sp_ThongKeTongQuan AS
BEGIN
    SELECT
        (SELECT COUNT(*) FROM DonHang WHERE TrangThai != 'DaHuy') AS TongDonHang,
        (SELECT ISNULL(SUM(TongThanhToan), 0) FROM DonHang WHERE TrangThai = 'DaGiao') AS TongDoanhThu,
        (SELECT COUNT(*) FROM SanPham WHERE ConHoatDong = 1) AS TongSanPham,
        (SELECT COUNT(*) FROM NguoiDung WHERE VaiTro = 'KhachHang') AS TongKhachHang,
        (SELECT COUNT(*) FROM DonHang WHERE TrangThai = 'ChoDuyet') AS DonChuaDuyet,
        (SELECT COUNT(*) FROM SanPham WHERE SoLuongTon < 10) AS SanPhamSapHet;
END;
GO

-- =============================================
-- DỮ LIỆU MẪU
-- =============================================

SET IDENTITY_INSERT NguoiDung ON;
INSERT INTO NguoiDung (MaNguoiDung, HoTen, Email, MatKhau, SoDienThoai, DiaChi, VaiTro) VALUES
(1, N'Quan Tri Vien', 'admin@doAnvat.vn', 'AQAAAAEAACcQAAAAELPT4r5BsY9Vz6HsW2mK8XnJqI3dR7vNuO1CeP0FbT5gM2wA4hY6kL8jQ9sE3xZ=', '0901234567', N'123 Duong ABC, Ha Noi', 'QuanTri'),
(2, N'Nguyen Van An', 'an@gmail.com', 'AQAAAAEAACcQAAAAELPT4r5BsY9Vz6HsW2mK8XnJqI3dR7vNuO1CeP0FbT5gM2wA4hY6kL8jQ9sE3xZ=', '0912345678', N'456 Duong XYZ, TP.HCM', 'KhachHang'),
(3, N'Tran Thi Binh', 'binh@gmail.com', 'AQAAAAEAACcQAAAAELPT4r5BsY9Vz6HsW2mK8XnJqI3dR7vNuO1CeP0FbT5gM2wA4hY6kL8jQ9sE3xZ=', '0923456789', N'789 Le Loi, Da Nang', 'KhachHang');
SET IDENTITY_INSERT NguoiDung OFF;

-- Danh mục theo yêu cầu giao diện
SET IDENTITY_INSERT DanhMuc ON;
INSERT INTO DanhMuc (MaDanhMuc, TenDanhMuc, MoTa, HinhAnh, ConHoatDong) VALUES
(1, N'Snack', N'Snack các loại', 'dm-snack.jpg', 1),
(2, N'Kẹo Bánh', N'Kẹo, bánh, chocolate', 'dm-keobanh.jpg', 1),
(3, N'Đồ ăn liền', N'Mì, cháo, ăn liền', 'dm-doanlien.jpg', 1),
(4, N'Đồ khô', N'Các loại đồ khô', 'dm-dokho.jpg', 1),
(5, N'Đồ sấy', N'Các loại đồ sấy', 'dm-dosay.jpg', 1),
(6, N'Ô mai', N'Ô mai các loại', 'dm-omai.jpg', 1),
(7, N'Bánh cốm', N'Bánh cốm', 'dm-banhcom.jpg', 1),
(8, N'Đồ ăn healthy', N'Healthy / ít đường / tốt cho sức khỏe', 'dm-healthy.jpg', 1),
(9, N'Tất cả', N'Tổng hợp', 'dm-tatca.jpg', 1);
SET IDENTITY_INSERT DanhMuc OFF;

INSERT INTO NhaCungCap (TenNhaCungCap, NguoiLienHe, SoDienThoai, Email, DiaChi) VALUES
(N'Cong ty Oishi Viet Nam', N'Nguyen Minh Tuan', '02838001234', 'oishi@oishi.com.vn', N'Khu CN Song Than, Binh Duong'),
(N'Cong ty Kinh Do', N'Tran Thi Hoa', '02838005678', 'kinhdo@kinhdo.vn', N'Quan Binh Tan, TP.HCM'),
(N'Cong ty Lotte Viet Nam', N'Le Van Binh', '02462929999', 'lotte@lotte.vn', N'Khu CN Thang Long, Ha Noi'),
(N'Cong ty Acecook Viet Nam', N'Pham Thi Lan', '02838889999', 'acecook@acecook.com.vn', N'Binh Duong');

INSERT INTO SanPham (TenSanPham, MaDanhMuc, MaNhaCungCap, MoTa, GiaNhap, GiaBan, PhanTramGiam, SoLuongTon, DonViTinh, HinhAnh) VALUES
(N'Banh Snack Oishi Tom 60g', 1, 1, N'Banh snack vi tom gion tan, dam da.', 8000, 12000, 0, 0, N'Goi', 'oishi-tom.jpg'),
(N'Banh Snack Oishi BBQ 60g', 1, 1, N'Banh snack vi BBQ thom ngon, hap dan.', 8000, 12000, 0, 0, N'Goi', 'oishi-bbq.jpg'),
(N'Banh Snack Poca Khoai Tay 50g', 1, 1, N'Chips khoai tay gion tan vi muoi bien.', 10000, 15000, 10, 0, N'Goi', 'poca-potato.jpg'),
(N'Keo Deo Trolli Gau 100g', 2, 3, N'Keo deo hinh gau nhieu vi trai cay.', 18000, 25000, 0, 0, N'Goi', 'trolli-bear.jpg'),
(N'Chocolate Lotte 90g', 2, 3, N'Chocolate sua Lotte thom ngon.', 20000, 30000, 15, 0, N'Thanh','lotte-choco.jpg'),
(N'Mi Hao Hao Tom Chua Cay', 3, 4, N'Mi an lien vi tom chua cay dac trung Acecook.', 3500, 5000, 0, 0, N'Goi', 'hao-hao.jpg'),
(N'Mi 3 Mien Bo Ham', 3, 4, N'Mi an lien huong vi bo ham dam da.', 4000, 6000, 0, 0, N'Goi', 'mi-3mien.jpg'),
(N'Tra Xanh 0 Do 455ml', 4, 2, N'Tra xanh khong duong, huong vi tu nhien.', 8000, 12000, 0, 0, N'Chai', 'tra-xanh.jpg'),
(N'Nuoc Ep Tao Tipco 1L', 4, 2, N'Nuoc ep tao 100% tu nhien.', 35000, 50000, 0, 0, N'Hop', 'tipco-apple.jpg'),
(N'Hat Dieu Rang Muoi 200g', 5, 2, N'Hat dieu rang muoi thom bui, bo duong.', 60000, 85000, 0, 0, N'Tui', 'hat-dieu.jpg'),
(N'Bo Kho Dac Biet 100g', 5, 2, N'Bo kho thuong hang, dai ngon, cay vua.', 45000, 65000, 0, 0, N'Goi', 'bo-kho.jpg'),
(N'Banh Quy Kinh Do 300g', 6, 2, N'Banh quy bo thom ngon, gion xop.', 25000, 38000, 0, 0, N'Hop', 'banh-quy.jpg'),
(N'Banh Bong Lan Hura 25g', 6, 2, N'Banh bong lan mem min, vi vani thom ngon.', 3000, 5000, 0, 0, N'Cai', 'hura-cake.jpg'),
(N'Snack Taro Khoai Mon 42g', 1, 1, N'Banh snack vi khoai mon beo ngay.', 7000, 10000, 0, 0, N'Goi', 'taro-snack.jpg'),
(N'Keo Chupa Chups 12g', 2, 3, N'Keo mut Chupa Chups da dang huong vi.', 5000, 8000, 0, 0, N'Cai', 'chupa-chups.jpg');

INSERT INTO PhieuNhap (MaPhieu, MaNhaCungCap, NguoiNhap, GhiChu) VALUES
('PN001', 1, 1, N'Nhap hang thang 1/2025'),
('PN002', 2, 1, N'Nhap hang banh keo'),
('PN003', 4, 1, N'Nhap mi an lien');

INSERT INTO ChiTietPhieuNhap (MaPhieuNhap, MaSanPham, SoLuong, DonGiaNhap) VALUES
(2,  4,  80, 18000),
(2,  5,  80, 20000),
(2,  8, 150,  8000),
(2,  9,  80, 35000),
(1,  1, 200,  8000),
(1,  2, 150,  8000),
(1,  3, 300, 10000),
(1, 14, 180,  7000),
(1, 15, 200,  5000),
(2, 12, 160, 25000),
(2, 13, 300,  3000),
(2, 10,  90, 60000),
(2, 11,  70, 45000),
(3,  6, 500,  3500),
(3,  7, 400,  4000);

INSERT INTO DonHang (MaDon, MaNguoiDung, TenKhachHang, SoDienThoai, DiaChiGiao, TongTienHang, PhiVanChuyen, TongThanhToan, TrangThai, HinhThucThanhToan, TrangThaiThanhToan) VALUES
('DH20250101', 2, N'Nguyen Van An', '0912345678', N'456 Duong XYZ, TP.HCM', 85000, 15000, 100000, 'DaGiao', 'TienMat', 'DaThanhToan'),
('DH20250102', 2, N'Nguyen Van An', '0912345678', N'456 Duong XYZ, TP.HCM', 120000, 15000, 135000, 'DaGiao', 'TienMat', 'DaThanhToan'),
('DH20250103', 2, N'Nguyen Van An', '0912345678', N'456 Duong XYZ, TP.HCM', 65000, 0, 65000, 'ChoDuyet', 'TienMat', 'ChuaThanhToan'),
('DH20250201', 3, N'Tran Thi Binh', '0923456789', N'789 Le Loi, Da Nang', 200000, 15000, 215000, 'DaGiao', 'ChuyenKhoan', 'DaThanhToan'),
('DH20250202', 3, N'Tran Thi Binh', '0923456789', N'789 Le Loi, Da Nang', 150000, 15000, 165000, 'DaXacNhan', 'MoMo', 'DaThanhToan');

INSERT INTO ChiTietDonHang (MaDonHang, MaSanPham, SoLuong, DonGiaBan) VALUES
(1, 1, 3, 12000),
(1, 6, 5, 5000),
(1, 13, 4, 5000),
(2, 10, 1, 85000),
(2, 4, 1, 25000),
(2, 5, 1, 30000),
(3, 11, 1, 65000),
(4, 10, 2, 85000),
(4, 5, 1, 30000),
(5, 12, 2, 38000),
(5, 9, 1, 50000),
(5, 8, 2, 12000);

INSERT INTO DanhGiaSanPham (MaSanPham, MaNguoiDung, SoSao, NhanXet) VALUES
(1, 2, 5, N'Snack ngon, gion, dong goi sach se. Se mua lai!'),
(6, 2, 4, N'Mi vua an, vi tom dac trung. Giao hang nhanh.'),
(10,3, 5, N'Hat dieu to, thom, khong bi moc. Rat hai long!'),
(5, 3, 4, N'Chocolate ngon nhung hoi ngot.');

INSERT INTO LienHe (HoTen, Email, SoDienThoai, TieuDe, NoiDung) VALUES
(N'Le Thi Mai', 'mai@gmail.com', '0987654321', N'Hoi ve san pham', N'Cho toi hoi snack Oishi co han su dung bao lau a?'),
(N'Tran Van Nam','nam@gmail.com', '0976543210', N'Gop y website', N'Website rat dep va de dung. Mong shop them nhieu san pham hon.');

INSERT INTO Banner (TieuDe, HinhAnh, DuongDan, ThuTuHienThi) VALUES
(N'Khuyen mai thang 4 - Giam 15% tat ca snack', 'banner1.jpg', '/san-pham?danhmuc=1', 1),
(N'Hang moi ve - Chocolate Lotte', 'banner2.jpg', '/san-pham/5', 2),
(N'Mien phi ship don tu 150.000d', 'banner3.jpg', '/san-pham', 3);

-- =============================================
-- KIỂM TRA KẾT QUẢ SAU KHI CHẠY
-- =============================================
SELECT 'NguoiDung' AS [Bang], COUNT(*) AS [SoBanGhi] FROM NguoiDung
UNION ALL SELECT 'DanhMuc', COUNT(*) FROM DanhMuc
UNION ALL SELECT 'NhaCungCap', COUNT(*) FROM NhaCungCap
UNION ALL SELECT 'SanPham', COUNT(*) FROM SanPham
UNION ALL SELECT 'PhieuNhap', COUNT(*) FROM PhieuNhap
UNION ALL SELECT 'ChiTietPhieuNhap', COUNT(*) FROM ChiTietPhieuNhap
UNION ALL SELECT 'DonHang', COUNT(*) FROM DonHang
UNION ALL SELECT 'ChiTietDonHang', COUNT(*) FROM ChiTietDonHang
UNION ALL SELECT 'GioHang', COUNT(*) FROM GioHang
UNION ALL SELECT 'DanhGiaSanPham', COUNT(*) FROM DanhGiaSanPham
UNION ALL SELECT 'LienHe', COUNT(*) FROM LienHe
UNION ALL SELECT 'Banner', COUNT(*) FROM Banner;

SELECT TenSanPham, SoLuongTon AS TonKhoHienTai FROM SanPham ORDER BY MaSanPham;

SELECT * FROM vw_NhapXuatTon ORDER BY MaSanPham;

PRINT 'Database DoAnVatA3 tao thanh cong!';
GO
