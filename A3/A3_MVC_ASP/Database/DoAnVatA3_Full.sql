-- =============================================
-- DoAnVatA3 — chạy 1 lần trên SQL Server (SSMS)
-- Gồm: schema đầy đủ + trigger + view + stored procedure + dữ liệu mẫu
-- Khớp Web.config: Initial Catalog=DoAnVatA3
--
-- CẢNH BÁO: DROP database DoAnVatA3 nếu đã tồn tại
-- Đăng nhập Admin:  admin@doanvat.vn  /  Admin123
-- Khách mẫu:        an@gmail.com      /  123456
--                   binh@gmail.com    /  123456
-- =============================================

USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'DoAnVatA3')
BEGIN
    ALTER DATABASE [DoAnVatA3] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [DoAnVatA3];
END
GO

CREATE DATABASE [DoAnVatA3] COLLATE Vietnamese_CI_AS;
GO

USE [DoAnVatA3];
GO

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- BẢNG
-- =============================================

CREATE TABLE dbo.NguoiDung (
    MaNguoiDung INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(150) NOT NULL UNIQUE,
    MatKhau VARCHAR(256) NOT NULL,
    SoDienThoai VARCHAR(15) NULL,
    DiaChi NVARCHAR(255) NULL,
    VaiTro VARCHAR(20) NULL DEFAULT 'KhachHang' CHECK (VaiTro IN ('QuanTri', 'KhachHang')),
    AnhDaiDien VARCHAR(255) NULL DEFAULT 'default-avatar.png',
    ConHoatDong BIT NULL DEFAULT 1,
    NgayTao DATETIME NULL DEFAULT GETDATE()
);
GO

CREATE TABLE dbo.DanhMuc (
    MaDanhMuc INT IDENTITY(1,1) PRIMARY KEY,
    TenDanhMuc NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(500) NULL,
    HinhAnh VARCHAR(255) NULL,
    ConHoatDong BIT NULL DEFAULT 1
);
GO

CREATE TABLE dbo.NhaCungCap (
    MaNhaCungCap INT IDENTITY(1,1) PRIMARY KEY,
    TenNhaCungCap NVARCHAR(150) NOT NULL,
    NguoiLienHe NVARCHAR(100) NULL,
    SoDienThoai VARCHAR(15) NULL,
    Email VARCHAR(150) NULL,
    DiaChi NVARCHAR(255) NULL,
    ConHoatDong BIT NULL DEFAULT 1
);
GO

CREATE TABLE dbo.SanPham (
    MaSanPham INT IDENTITY(1,1) PRIMARY KEY,
    TenSanPham NVARCHAR(150) NOT NULL,
    MaDanhMuc INT NOT NULL REFERENCES dbo.DanhMuc(MaDanhMuc),
    MaNhaCungCap INT NULL REFERENCES dbo.NhaCungCap(MaNhaCungCap),
    MoTa NVARCHAR(1000) NULL,
    GiaNhap DECIMAL(18, 0) NOT NULL DEFAULT 0,
    GiaBan DECIMAL(18, 0) NOT NULL,
    PhanTramGiam DECIMAL(5, 2) NULL DEFAULT 0 CHECK (PhanTramGiam BETWEEN 0 AND 100),
    SoLuongTon INT NULL DEFAULT 0 CHECK (SoLuongTon >= 0),
    DonViTinh NVARCHAR(30) NULL DEFAULT N'Goi',
    HinhAnh VARCHAR(255) NULL DEFAULT 'no-image.png',
    ConHoatDong BIT NULL DEFAULT 1,
    NgayTao DATETIME NULL DEFAULT GETDATE()
);
GO

CREATE TABLE dbo.PhieuNhap (
    MaPhieuNhap INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieu VARCHAR(20) NOT NULL UNIQUE,
    MaNhaCungCap INT NULL REFERENCES dbo.NhaCungCap(MaNhaCungCap),
    NguoiNhap INT NULL REFERENCES dbo.NguoiDung(MaNguoiDung),
    TongTien DECIMAL(18, 0) NULL DEFAULT 0,
    GhiChu NVARCHAR(500) NULL,
    NgayNhap DATETIME NULL DEFAULT GETDATE()
);
GO

CREATE TABLE dbo.ChiTietPhieuNhap (
    MaChiTiet INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieuNhap INT NOT NULL REFERENCES dbo.PhieuNhap(MaPhieuNhap),
    MaSanPham INT NOT NULL REFERENCES dbo.SanPham(MaSanPham),
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGiaNhap DECIMAL(18, 0) NOT NULL,
    ThanhTien AS (SoLuong * DonGiaNhap) PERSISTED
);
GO

CREATE TABLE dbo.DonHang (
    MaDonHang INT IDENTITY(1,1) PRIMARY KEY,
    MaDon VARCHAR(20) NOT NULL UNIQUE,
    MaNguoiDung INT NULL REFERENCES dbo.NguoiDung(MaNguoiDung),
    TenKhachHang NVARCHAR(100) NULL,
    SoDienThoai VARCHAR(15) NULL,
    DiaChiGiao NVARCHAR(255) NULL,
    TongTienHang DECIMAL(18, 0) NULL DEFAULT 0,
    PhiVanChuyen DECIMAL(18, 0) NULL DEFAULT 0,
    TongThanhToan DECIMAL(18, 0) NULL DEFAULT 0,
    TrangThai VARCHAR(30) NULL DEFAULT 'ChoDuyet' CHECK (TrangThai IN ('ChoDuyet','DaXacNhan','DangGiao','DaGiao','DaHuy')),
    HinhThucThanhToan VARCHAR(30) NULL DEFAULT 'TienMat' CHECK (HinhThucThanhToan IN ('TienMat','ChuyenKhoan','MoMo')),
    TrangThaiThanhToan VARCHAR(20) NULL DEFAULT 'ChuaThanhToan' CHECK (TrangThaiThanhToan IN ('ChuaThanhToan','DaThanhToan')),
    GhiChu NVARCHAR(500) NULL,
    NgayDat DATETIME NULL DEFAULT GETDATE()
);
GO

CREATE TABLE dbo.ChiTietDonHang (
    MaChiTiet INT IDENTITY(1,1) PRIMARY KEY,
    MaDonHang INT NOT NULL REFERENCES dbo.DonHang(MaDonHang),
    MaSanPham INT NOT NULL REFERENCES dbo.SanPham(MaSanPham),
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGiaBan DECIMAL(18, 0) NOT NULL,
    ThanhTien AS (SoLuong * DonGiaBan) PERSISTED
);
GO

CREATE TABLE dbo.GioHang (
    MaGioHang INT IDENTITY(1,1) PRIMARY KEY,
    MaNguoiDung INT NULL REFERENCES dbo.NguoiDung(MaNguoiDung),
    MaSanPham INT NOT NULL REFERENCES dbo.SanPham(MaSanPham),
    SoLuong INT NOT NULL DEFAULT 1 CHECK (SoLuong > 0),
    NgayThem DATETIME NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_GioHang UNIQUE (MaNguoiDung, MaSanPham)
);
GO

CREATE TABLE dbo.DanhGiaSanPham (
    MaDanhGia INT IDENTITY(1,1) PRIMARY KEY,
    MaSanPham INT NOT NULL REFERENCES dbo.SanPham(MaSanPham),
    MaNguoiDung INT NOT NULL REFERENCES dbo.NguoiDung(MaNguoiDung),
    SoSao TINYINT NOT NULL CHECK (SoSao BETWEEN 1 AND 5),
    NhanXet NVARCHAR(1000) NULL,
    NgayDanhGia DATETIME NULL DEFAULT GETDATE(),
    CONSTRAINT UQ_DanhGia UNIQUE (MaNguoiDung, MaSanPham)
);
GO

CREATE TABLE dbo.LienHe (
    MaLienHe INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(150) NOT NULL,
    SoDienThoai VARCHAR(15) NULL,
    TieuDe NVARCHAR(200) NULL,
    NoiDung NVARCHAR(2000) NOT NULL,
    DaDoc BIT NULL DEFAULT 0,
    PhanHoi NVARCHAR(2000) NULL,
    NgayGui DATETIME NULL DEFAULT GETDATE()
);
GO

CREATE TABLE dbo.Banner (
    MaBanner INT IDENTITY(1,1) PRIMARY KEY,
    TieuDe NVARCHAR(200) NULL,
    HinhAnh VARCHAR(255) NULL,
    DuongDan VARCHAR(255) NULL,
    ConHoatDong BIT NULL DEFAULT 1,
    ThuTuHienThi INT NULL DEFAULT 0
);
GO

CREATE TABLE dbo.ThanhToan (
    MaThanhToan INT IDENTITY(1,1) PRIMARY KEY,
    MaDonHang INT NOT NULL,
    Provider VARCHAR(30) NOT NULL,
    MaGiaoDich VARCHAR(50) NULL,
    SoTien DECIMAL(18, 0) NOT NULL,
    TrangThai VARCHAR(20) NOT NULL,
    NgayThanhToan DATETIME NOT NULL DEFAULT GETDATE(),
    RawJson NVARCHAR(MAX) NULL,
    CONSTRAINT FK_ThanhToan_DonHang FOREIGN KEY (MaDonHang) REFERENCES dbo.DonHang(MaDonHang)
);
GO

-- =============================================
-- INDEX
-- =============================================
CREATE INDEX IX_SanPham_DanhMuc ON dbo.SanPham(MaDanhMuc);
CREATE INDEX IX_SanPham_HoatDong ON dbo.SanPham(ConHoatDong);
CREATE INDEX IX_DonHang_NguoiDung ON dbo.DonHang(MaNguoiDung);
CREATE INDEX IX_DonHang_TrangThai ON dbo.DonHang(TrangThai);
CREATE INDEX IX_DonHang_NgayDat ON dbo.DonHang(NgayDat);
CREATE INDEX IX_PhieuNhap_NgayNhap ON dbo.PhieuNhap(NgayNhap);
CREATE INDEX IX_DanhGia_SanPham ON dbo.DanhGiaSanPham(MaSanPham);
CREATE INDEX IX_ThanhToan_MaDonHang ON dbo.ThanhToan(MaDonHang);
GO

-- =============================================
-- TRIGGER tồn kho
-- =============================================
CREATE TRIGGER dbo.trg_SauNhapKho ON dbo.ChiTietPhieuNhap
AFTER INSERT AS
BEGIN
    SET NOCOUNT ON;

    UPDATE sp
    SET sp.SoLuongTon = sp.SoLuongTon + ct.SoLuong
    FROM dbo.SanPham sp
    INNER JOIN inserted ct ON sp.MaSanPham = ct.MaSanPham;

    UPDATE pn
    SET pn.TongTien = (
        SELECT ISNULL(SUM(ThanhTien), 0)
        FROM dbo.ChiTietPhieuNhap
        WHERE MaPhieuNhap = pn.MaPhieuNhap
    )
    FROM dbo.PhieuNhap pn
    INNER JOIN inserted ct ON pn.MaPhieuNhap = ct.MaPhieuNhap;
END;
GO

CREATE TRIGGER dbo.trg_SauDatHang ON dbo.ChiTietDonHang
AFTER INSERT AS
BEGIN
    SET NOCOUNT ON;

    UPDATE sp
    SET sp.SoLuongTon = sp.SoLuongTon - ct.SoLuong
    FROM dbo.SanPham sp
    INNER JOIN inserted ct ON sp.MaSanPham = ct.MaSanPham;
END;
GO

CREATE TRIGGER dbo.trg_HuyDonHang ON dbo.DonHang
AFTER UPDATE AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(TrangThai)
    BEGIN
        UPDATE sp
        SET sp.SoLuongTon = sp.SoLuongTon + ct.SoLuong
        FROM dbo.SanPham sp
        INNER JOIN dbo.ChiTietDonHang ct ON sp.MaSanPham = ct.MaSanPham
        INNER JOIN inserted i ON ct.MaDonHang = i.MaDonHang
        INNER JOIN deleted d ON i.MaDonHang = d.MaDonHang
        WHERE i.TrangThai = 'DaHuy' AND d.TrangThai <> 'DaHuy';
    END
END;
GO

-- =============================================
-- VIEW
-- =============================================
CREATE VIEW dbo.vw_TonKho AS
SELECT
    sp.MaSanPham,
    sp.TenSanPham,
    dm.TenDanhMuc,
    sp.SoLuongTon,
    sp.GiaNhap,
    sp.GiaBan,
    sp.PhanTramGiam,
    (sp.SoLuongTon * sp.GiaNhap) AS GiaTriTonKho
FROM dbo.SanPham sp
JOIN dbo.DanhMuc dm ON sp.MaDanhMuc = dm.MaDanhMuc
WHERE sp.ConHoatDong = 1;
GO

CREATE VIEW dbo.vw_DoanhThuTheoThang AS
SELECT
    YEAR(NgayDat) AS Nam,
    MONTH(NgayDat) AS Thang,
    COUNT(*) AS TongDonHang,
    SUM(TongThanhToan) AS DoanhThu
FROM dbo.DonHang
WHERE TrangThai = 'DaGiao'
GROUP BY YEAR(NgayDat), MONTH(NgayDat);
GO

CREATE VIEW dbo.vw_NhapXuatTon AS
SELECT
    sp.MaSanPham,
    sp.TenSanPham,
    dm.TenDanhMuc,
    ISNULL(nhap.TongNhap, 0) AS TongSoLuongNhap,
    ISNULL(xuat.TongBan, 0) AS TongSoLuongBan,
    sp.SoLuongTon AS TonKhoHienTai,
    ISNULL(nhap.TongTienNhap, 0) AS TongTienNhap,
    ISNULL(xuat.TongDoanhThu, 0) AS TongDoanhThu
FROM dbo.SanPham sp
JOIN dbo.DanhMuc dm ON sp.MaDanhMuc = dm.MaDanhMuc
LEFT JOIN (
    SELECT MaSanPham, SUM(SoLuong) AS TongNhap, SUM(ThanhTien) AS TongTienNhap
    FROM dbo.ChiTietPhieuNhap
    GROUP BY MaSanPham
) nhap ON sp.MaSanPham = nhap.MaSanPham
LEFT JOIN (
    SELECT ct.MaSanPham, SUM(ct.SoLuong) AS TongBan, SUM(ct.ThanhTien) AS TongDoanhThu
    FROM dbo.ChiTietDonHang ct
    JOIN dbo.DonHang dh ON ct.MaDonHang = dh.MaDonHang
    WHERE dh.TrangThai IN ('DaGiao','DaXacNhan','DangGiao')
    GROUP BY ct.MaSanPham
) xuat ON sp.MaSanPham = xuat.MaSanPham;
GO

CREATE VIEW dbo.vw_SanPhamBanChay AS
SELECT
    sp.MaSanPham,
    sp.TenSanPham,
    dm.TenDanhMuc,
    SUM(ct.SoLuong) AS TongSoBan,
    SUM(ct.ThanhTien) AS TongDoanhThu
FROM dbo.ChiTietDonHang ct
JOIN dbo.SanPham sp ON ct.MaSanPham = sp.MaSanPham
JOIN dbo.DanhMuc dm ON sp.MaDanhMuc = dm.MaDanhMuc
JOIN dbo.DonHang dh ON ct.MaDonHang = dh.MaDonHang
WHERE dh.TrangThai = 'DaGiao'
GROUP BY sp.MaSanPham, sp.TenSanPham, dm.TenDanhMuc;
GO

-- =============================================
-- STORED PROCEDURE
-- =============================================
CREATE PROCEDURE dbo.sp_CapNhatTonKhoNhap
    @MaSanPham INT,
    @SoLuong INT
AS
BEGIN
    UPDATE dbo.SanPham
    SET SoLuongTon = SoLuongTon + @SoLuong
    WHERE MaSanPham = @MaSanPham;
END;
GO

CREATE PROCEDURE dbo.sp_CapNhatTonKhoXuat
    @MaSanPham INT,
    @SoLuong INT
AS
BEGIN
    UPDATE dbo.SanPham
    SET SoLuongTon = SoLuongTon - @SoLuong
    WHERE MaSanPham = @MaSanPham AND SoLuongTon >= @SoLuong;
END;
GO

CREATE PROCEDURE dbo.sp_ThongKeTongQuan AS
BEGIN
    SELECT
        (SELECT COUNT(*) FROM dbo.DonHang WHERE TrangThai <> 'DaHuy') AS TongDonHang,
        (SELECT ISNULL(SUM(TongThanhToan), 0) FROM dbo.DonHang WHERE TrangThai = 'DaGiao') AS TongDoanhThu,
        (SELECT COUNT(*) FROM dbo.SanPham WHERE ConHoatDong = 1) AS TongSanPham,
        (SELECT COUNT(*) FROM dbo.NguoiDung WHERE VaiTro = 'KhachHang') AS TongKhachHang,
        (SELECT COUNT(*) FROM dbo.DonHang WHERE TrangThai = 'ChoDuyet') AS DonChuaDuyet,
        (SELECT COUNT(*) FROM dbo.SanPham WHERE SoLuongTon < 10) AS SanPhamSapHet;
END;
GO

-- =============================================
-- DỮ LIỆU MẪU
-- =============================================

SET IDENTITY_INSERT dbo.NguoiDung ON;
INSERT INTO dbo.NguoiDung (MaNguoiDung, HoTen, Email, MatKhau, SoDienThoai, DiaChi, VaiTro, AnhDaiDien, ConHoatDong, NgayTao) VALUES
(1, N'Quản trị viên', 'admin@doanvat.vn', 'Admin123', '0901234567', N'123 Đường ABC, Hà Nội', 'QuanTri', 'default-avatar.png', 1, GETDATE()),
(2, N'Nguyễn Văn An', 'an@gmail.com', '123456', '0912345678', N'456 Đường XYZ, TP.HCM', 'KhachHang', 'default-avatar.png', 1, GETDATE()),
(3, N'Trần Thị Bình', 'binh@gmail.com', '123456', '0923456789', N'789 Lê Lợi, Đà Nẵng', 'KhachHang', 'default-avatar.png', 1, GETDATE());
SET IDENTITY_INSERT dbo.NguoiDung OFF;
GO

SET IDENTITY_INSERT dbo.DanhMuc ON;
INSERT INTO dbo.DanhMuc (MaDanhMuc, TenDanhMuc, MoTa, HinhAnh, ConHoatDong) VALUES
(1, N'Snack', N'Snack các loại', 'dm-snack.jpg', 1),
(2, N'Kẹo Bánh', N'Kẹo, bánh, chocolate', 'dm-keobanh.jpg', 1),
(3, N'Đồ ăn liền', N'Mì, cháo, ăn liền', 'dm-doanlien.jpg', 1),
(4, N'Đồ khô', N'Các loại đồ khô', 'dm-dokho.jpg', 1),
(5, N'Đồ sấy', N'Các loại đồ sấy', 'dm-dosay.jpg', 1),
(6, N'Ô mai', N'Ô mai các loại', 'dm-omai.jpg', 1),
(7, N'Bánh cốm', N'Bánh cốm', 'dm-banhcom.jpg', 1),
(8, N'Đồ ăn healthy', N'Healthy / ít đường / tốt cho sức khỏe', 'dm-healthy.jpg', 1),
(9, N'Tất cả', N'Tổng hợp', 'dm-tatca.jpg', 1);
SET IDENTITY_INSERT dbo.DanhMuc OFF;
GO

INSERT INTO dbo.NhaCungCap (TenNhaCungCap, NguoiLienHe, SoDienThoai, Email, DiaChi) VALUES
(N'Công ty Oishi Việt Nam', N'Nguyễn Minh Tuấn', '02838001234', 'oishi@oishi.com.vn', N'Khu CN Sóng Thần, Bình Dương'),
(N'Công ty Kinh Đô', N'Trần Thị Hoa', '02838005678', 'kinhdo@kinhdo.vn', N'Quận Bình Tân, TP.HCM'),
(N'Công ty Lotte Việt Nam', N'Lê Văn Bình', '02462929999', 'lotte@lotte.vn', N'Khu CN Thăng Long, Hà Nội'),
(N'Công ty Acecook Việt Nam', N'Phạm Thị Lan', '02838889999', 'acecook@acecook.com.vn', N'Bình Dương');
GO

INSERT INTO dbo.SanPham (TenSanPham, MaDanhMuc, MaNhaCungCap, MoTa, GiaNhap, GiaBan, PhanTramGiam, SoLuongTon, DonViTinh, HinhAnh) VALUES
(N'Bánh Snack Oishi Tôm 60g', 1, 1, N'Bánh snack vị tôm giòn tan, đậm đà.', 8000, 12000, 0, 0, N'Gói', 'oishi-tom.jpg'),
(N'Bánh Snack Oishi BBQ 60g', 1, 1, N'Bánh snack vị BBQ thơm ngon, hấp dẫn.', 8000, 12000, 0, 0, N'Gói', 'oishi-bbq.jpg'),
(N'Bánh Snack Poca Khoai Tây 50g', 1, 1, N'Chips khoai tây giòn tan vị muối biển.', 10000, 15000, 10, 0, N'Gói', 'poca-potato.jpg'),
(N'Kẹo Dẻo Trolli Gấu 100g', 2, 3, N'Kẹo dẻo hình gấu nhiều vị trái cây.', 18000, 25000, 0, 0, N'Gói', 'trolli-bear.jpg'),
(N'Chocolate Lotte 90g', 2, 3, N'Chocolate sữa Lotte thơm ngon.', 20000, 30000, 15, 0, N'Thanh','lotte-choco.jpg'),
(N'Mì Hảo Hảo Tôm Chua Cay', 3, 4, N'Mì ăn liền vị tôm chua cay đặc trưng Acecook.', 3500, 5000, 0, 0, N'Gói', 'hao-hao.jpg'),
(N'Mì 3 Miền Bò Hầm', 3, 4, N'Mì ăn liền hương vị bò hầm đậm đà.', 4000, 6000, 0, 0, N'Gói', 'mi-3mien.jpg'),
(N'Trà Xanh 0 Độ 455ml', 4, 2, N'Trà xanh không đường, hương vị tự nhiên.', 8000, 12000, 0, 0, N'Chai', 'tra-xanh.jpg'),
(N'Nước Ép Táo Tipco 1L', 4, 2, N'Nước ép táo 100% tự nhiên.', 35000, 50000, 0, 0, N'Hộp', 'tipco-apple.jpg'),
(N'Hạt Điều Rang Muối 200g', 5, 2, N'Hạt điều rang muối thơm bùi, bổ dưỡng.', 60000, 85000, 0, 0, N'Túi', 'hat-dieu.jpg'),
(N'Bò Khô Đặc Biệt 100g', 5, 2, N'Bò khô thượng hạng, dai ngon, cay vừa.', 45000, 65000, 0, 0, N'Gói', 'bo-kho.jpg'),
(N'Bánh Quy Kinh Đô 300g', 6, 2, N'Bánh quy bơ thơm ngon, giòn xốp.', 25000, 38000, 0, 0, N'Hộp', 'banh-quy.jpg'),
(N'Bánh Bông Lan Hura 25g', 6, 2, N'Bánh bông lan mềm mịn, vị vani thơm ngon.', 3000, 5000, 0, 0, N'Cái', 'hura-cake.jpg'),
(N'Snack Taro Khoai Môn 42g', 1, 1, N'Bánh snack vị khoai môn béo ngậy.', 7000, 10000, 0, 0, N'Gói', 'taro-snack.jpg'),
(N'Kẹo Chupa Chups 12g', 2, 3, N'Kẹo mút Chupa Chups đa dạng hương vị.', 5000, 8000, 0, 0, N'Cái', 'chupa-chups.jpg');
GO

INSERT INTO dbo.PhieuNhap (MaPhieu, MaNhaCungCap, NguoiNhap, GhiChu) VALUES
('PN001', 1, 1, N'Nhập hàng tháng 1/2025'),
('PN002', 2, 1, N'Nhập hàng bánh kẹo'),
('PN003', 4, 1, N'Nhập mì ăn liền');
GO

INSERT INTO dbo.ChiTietPhieuNhap (MaPhieuNhap, MaSanPham, SoLuong, DonGiaNhap) VALUES
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
GO

INSERT INTO dbo.DonHang (MaDon, MaNguoiDung, TenKhachHang, SoDienThoai, DiaChiGiao, TongTienHang, PhiVanChuyen, TongThanhToan, TrangThai, HinhThucThanhToan, TrangThaiThanhToan) VALUES
('DH20250101', 2, N'Nguyễn Văn An', '0912345678', N'456 Đường XYZ, TP.HCM', 85000, 15000, 100000, 'DaGiao', 'TienMat', 'DaThanhToan'),
('DH20250102', 2, N'Nguyễn Văn An', '0912345678', N'456 Đường XYZ, TP.HCM', 120000, 15000, 135000, 'DaGiao', 'TienMat', 'DaThanhToan'),
('DH20250103', 2, N'Nguyễn Văn An', '0912345678', N'456 Đường XYZ, TP.HCM', 65000, 0, 65000, 'ChoDuyet', 'TienMat', 'ChuaThanhToan'),
('DH20250201', 3, N'Trần Thị Bình', '0923456789', N'789 Lê Lợi, Đà Nẵng', 200000, 15000, 215000, 'DaGiao', 'ChuyenKhoan', 'DaThanhToan'),
('DH20250202', 3, N'Trần Thị Bình', '0923456789', N'789 Lê Lợi, Đà Nẵng', 150000, 15000, 165000, 'DaXacNhan', 'MoMo', 'DaThanhToan');
GO

INSERT INTO dbo.ChiTietDonHang (MaDonHang, MaSanPham, SoLuong, DonGiaBan) VALUES
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
GO

INSERT INTO dbo.DanhGiaSanPham (MaSanPham, MaNguoiDung, SoSao, NhanXet) VALUES
(1, 2, 5, N'Snack ngon, giòn, đóng gói sạch sẽ. Sẽ mua lại!'),
(6, 2, 4, N'Mì vừa ăn, vị tôm đặc trưng. Giao hàng nhanh.'),
(10, 3, 5, N'Hạt điều to, thơm, không bị mốc. Rất hài lòng!'),
(5, 3, 4, N'Chocolate ngon nhưng hơi ngọt.');
GO

INSERT INTO dbo.LienHe (HoTen, Email, SoDienThoai, TieuDe, NoiDung) VALUES
(N'Lê Thị Mai', 'mai@gmail.com', '0987654321', N'Hỏi về sản phẩm', N'Cho tôi hỏi snack Oishi có hạn sử dụng bao lâu ạ?'),
(N'Trần Văn Nam', 'nam@gmail.com', '0976543210', N'Góp ý website', N'Website rất đẹp và dễ dùng. Mong shop thêm nhiều sản phẩm hơn.');
GO

INSERT INTO dbo.Banner (TieuDe, HinhAnh, DuongDan, ThuTuHienThi) VALUES
(N'Khuyến mãi tháng 4 - Giảm 15% tất cả snack', 'banner1.jpg', '/san-pham?danhmuc=1', 1),
(N'Hàng mới về - Chocolate Lotte', 'banner2.jpg', '/san-pham/5', 2),
(N'Miễn phí ship đơn từ 150.000đ', 'banner3.jpg', '/san-pham', 3);
GO

SELECT 'NguoiDung' AS Bang, COUNT(*) AS SoBanGhi FROM dbo.NguoiDung
UNION ALL SELECT 'DanhMuc', COUNT(*) FROM dbo.DanhMuc
UNION ALL SELECT 'NhaCungCap', COUNT(*) FROM dbo.NhaCungCap
UNION ALL SELECT 'SanPham', COUNT(*) FROM dbo.SanPham
UNION ALL SELECT 'PhieuNhap', COUNT(*) FROM dbo.PhieuNhap
UNION ALL SELECT 'ChiTietPhieuNhap', COUNT(*) FROM dbo.ChiTietPhieuNhap
UNION ALL SELECT 'DonHang', COUNT(*) FROM dbo.DonHang
UNION ALL SELECT 'ChiTietDonHang', COUNT(*) FROM dbo.ChiTietDonHang
UNION ALL SELECT 'GioHang', COUNT(*) FROM dbo.GioHang
UNION ALL SELECT 'DanhGiaSanPham', COUNT(*) FROM dbo.DanhGiaSanPham
UNION ALL SELECT 'LienHe', COUNT(*) FROM dbo.LienHe
UNION ALL SELECT 'Banner', COUNT(*) FROM dbo.Banner
UNION ALL SELECT 'ThanhToan', COUNT(*) FROM dbo.ThanhToan;

PRINT N'DoAnVatA3 tạo thành công.';
PRINT N'Admin: admin@doanvat.vn / Admin123';
PRINT N'Khách: an@gmail.com / 123456 | binh@gmail.com / 123456';
GO
