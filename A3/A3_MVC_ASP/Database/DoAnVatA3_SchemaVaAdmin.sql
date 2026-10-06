-- =============================================
-- DoAnVatA3 — Schema đầy đủ + chỉ 1 bản ghi: tài khoản Admin
-- Nguồn schema: DoAnVatA3.sql | CREATE DATABASE mặc định theo instance (không cứng đường dẫn ổ đĩa)
-- Đăng nhập: admin@doanvat.vn / Admin123 (plain text, khớp AccountController)
-- CẢNH BÁO: DROP database DoAnVatA3 nếu đã tồn tại
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
USE [DoAnVatA3]
GO
/****** Object:  Table [dbo].[DanhMuc]    Script Date: 4/23/2026 6:34:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DanhMuc](
	[MaDanhMuc] [int] IDENTITY(1,1) NOT NULL,
	[TenDanhMuc] [nvarchar](100) NOT NULL,
	[MoTa] [nvarchar](500) NULL,
	[HinhAnh] [varchar](255) NULL,
	[ConHoatDong] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaDanhMuc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SanPham]    Script Date: 4/23/2026 6:34:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SanPham](
	[MaSanPham] [int] IDENTITY(1,1) NOT NULL,
	[TenSanPham] [nvarchar](150) NOT NULL,
	[MaDanhMuc] [int] NOT NULL,
	[MaNhaCungCap] [int] NULL,
	[MoTa] [nvarchar](1000) NULL,
	[GiaNhap] [decimal](18, 0) NOT NULL,
	[GiaBan] [decimal](18, 0) NOT NULL,
	[PhanTramGiam] [decimal](5, 2) NULL,
	[SoLuongTon] [int] NULL,
	[DonViTinh] [nvarchar](30) NULL,
	[HinhAnh] [varchar](255) NULL,
	[ConHoatDong] [bit] NULL,
	[NgayTao] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaSanPham] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_TonKho]    Script Date: 4/23/2026 6:34:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- VIEWS
-- =============================================

-- Xem tồn kho hiện tại
CREATE VIEW [dbo].[vw_TonKho] AS
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
/****** Object:  Table [dbo].[DonHang]    Script Date: 4/23/2026 6:34:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonHang](
	[MaDonHang] [int] IDENTITY(1,1) NOT NULL,
	[MaDon] [varchar](20) NOT NULL,
	[MaNguoiDung] [int] NULL,
	[TenKhachHang] [nvarchar](100) NULL,
	[SoDienThoai] [varchar](15) NULL,
	[DiaChiGiao] [nvarchar](255) NULL,
	[TongTienHang] [decimal](18, 0) NULL,
	[PhiVanChuyen] [decimal](18, 0) NULL,
	[TongThanhToan] [decimal](18, 0) NULL,
	[TrangThai] [varchar](30) NULL,
	[HinhThucThanhToan] [varchar](30) NULL,
	[TrangThaiThanhToan] [varchar](20) NULL,
	[GhiChu] [nvarchar](500) NULL,
	[NgayDat] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaDonHang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[MaDon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_DoanhThuTheoThang]    Script Date: 4/23/2026 6:34:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Doanh thu theo tháng
CREATE VIEW [dbo].[vw_DoanhThuTheoThang] AS
SELECT 
    YEAR(NgayDat) AS Nam, 
    MONTH(NgayDat) AS Thang, 
    COUNT(*) AS TongDonHang, 
    SUM(TongThanhToan) AS DoanhThu 
FROM DonHang 
WHERE TrangThai = 'DaGiao' 
GROUP BY YEAR(NgayDat), MONTH(NgayDat);
GO
/****** Object:  Table [dbo].[ChiTietDonHang]    Script Date: 4/23/2026 6:34:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietDonHang](
	[MaChiTiet] [int] IDENTITY(1,1) NOT NULL,
	[MaDonHang] [int] NOT NULL,
	[MaSanPham] [int] NOT NULL,
	[SoLuong] [int] NOT NULL,
	[DonGiaBan] [decimal](18, 0) NOT NULL,
	[ThanhTien]  AS ([SoLuong]*[DonGiaBan]) PERSISTED,
PRIMARY KEY CLUSTERED 
(
	[MaChiTiet] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietPhieuNhap]    Script Date: 4/23/2026 6:34:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietPhieuNhap](
	[MaChiTiet] [int] IDENTITY(1,1) NOT NULL,
	[MaPhieuNhap] [int] NOT NULL,
	[MaSanPham] [int] NOT NULL,
	[SoLuong] [int] NOT NULL,
	[DonGiaNhap] [decimal](18, 0) NOT NULL,
	[ThanhTien]  AS ([SoLuong]*[DonGiaNhap]) PERSISTED,
PRIMARY KEY CLUSTERED 
(
	[MaChiTiet] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_NhapXuatTon]    Script Date: 4/23/2026 6:34:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Báo cáo Nhập - Xuất - Tồn theo sản phẩm
CREATE VIEW [dbo].[vw_NhapXuatTon] AS
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
/****** Object:  View [dbo].[vw_SanPhamBanChay]    Script Date: 4/23/2026 6:34:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Sản phẩm bán chạy (dùng kèm ORDER BY bên ngoài)
CREATE VIEW [dbo].[vw_SanPhamBanChay] AS
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
/****** Object:  Table [dbo].[Banner]    Script Date: 4/23/2026 6:34:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Banner](
	[MaBanner] [int] IDENTITY(1,1) NOT NULL,
	[TieuDe] [nvarchar](200) NULL,
	[HinhAnh] [varchar](255) NULL,
	[DuongDan] [varchar](255) NULL,
	[ConHoatDong] [bit] NULL,
	[ThuTuHienThi] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaBanner] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DanhGiaSanPham]    Script Date: 4/23/2026 6:34:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DanhGiaSanPham](
	[MaDanhGia] [int] IDENTITY(1,1) NOT NULL,
	[MaSanPham] [int] NOT NULL,
	[MaNguoiDung] [int] NOT NULL,
	[SoSao] [tinyint] NOT NULL,
	[NhanXet] [nvarchar](1000) NULL,
	[NgayDanhGia] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaDanhGia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_DanhGia] UNIQUE NONCLUSTERED 
(
	[MaNguoiDung] ASC,
	[MaSanPham] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GioHang]    Script Date: 4/23/2026 6:34:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GioHang](
	[MaGioHang] [int] IDENTITY(1,1) NOT NULL,
	[MaNguoiDung] [int] NULL,
	[MaSanPham] [int] NOT NULL,
	[SoLuong] [int] NOT NULL,
	[NgayThem] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaGioHang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_GioHang] UNIQUE NONCLUSTERED 
(
	[MaNguoiDung] ASC,
	[MaSanPham] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LienHe]    Script Date: 4/23/2026 6:34:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LienHe](
	[MaLienHe] [int] IDENTITY(1,1) NOT NULL,
	[HoTen] [nvarchar](100) NOT NULL,
	[Email] [varchar](150) NOT NULL,
	[SoDienThoai] [varchar](15) NULL,
	[TieuDe] [nvarchar](200) NULL,
	[NoiDung] [nvarchar](2000) NOT NULL,
	[DaDoc] [bit] NULL,
	[PhanHoi] [nvarchar](2000) NULL,
	[NgayGui] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaLienHe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NguoiDung]    Script Date: 4/23/2026 6:34:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NguoiDung](
	[MaNguoiDung] [int] IDENTITY(1,1) NOT NULL,
	[HoTen] [nvarchar](100) NOT NULL,
	[Email] [varchar](150) NOT NULL,
	[MatKhau] [varchar](256) NOT NULL,
	[SoDienThoai] [varchar](15) NULL,
	[DiaChi] [nvarchar](255) NULL,
	[VaiTro] [varchar](20) NULL,
	[AnhDaiDien] [varchar](255) NULL,
	[ConHoatDong] [bit] NULL,
	[NgayTao] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaNguoiDung] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NhaCungCap]    Script Date: 4/23/2026 6:34:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhaCungCap](
	[MaNhaCungCap] [int] IDENTITY(1,1) NOT NULL,
	[TenNhaCungCap] [nvarchar](150) NOT NULL,
	[NguoiLienHe] [nvarchar](100) NULL,
	[SoDienThoai] [varchar](15) NULL,
	[Email] [varchar](150) NULL,
	[DiaChi] [nvarchar](255) NULL,
	[ConHoatDong] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaNhaCungCap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhieuNhap]    Script Date: 4/23/2026 6:34:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhieuNhap](
	[MaPhieuNhap] [int] IDENTITY(1,1) NOT NULL,
	[MaPhieu] [varchar](20) NOT NULL,
	[MaNhaCungCap] [int] NULL,
	[NguoiNhap] [int] NULL,
	[TongTien] [decimal](18, 0) NULL,
	[GhiChu] [nvarchar](500) NULL,
	[NgayNhap] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaPhieuNhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[MaPhieu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThanhToan]    Script Date: 4/23/2026 6:34:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThanhToan](
	[MaThanhToan] [int] IDENTITY(1,1) NOT NULL,
	[MaDonHang] [int] NOT NULL,
	[Provider] [varchar](30) NOT NULL,
	[MaGiaoDich] [varchar](50) NULL,
	[SoTien] [decimal](18, 0) NOT NULL,
	[TrangThai] [varchar](20) NOT NULL,
	[NgayThanhToan] [datetime] NOT NULL,
	[RawJson] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaThanhToan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_DanhGia_SanPham]    Script Date: 4/23/2026 6:34:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_DanhGia_SanPham] ON [dbo].[DanhGiaSanPham]
(
	[MaSanPham] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DonHang_NgayDat]    Script Date: 4/23/2026 6:34:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_DonHang_NgayDat] ON [dbo].[DonHang]
(
	[NgayDat] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_DonHang_NguoiDung]    Script Date: 4/23/2026 6:34:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_DonHang_NguoiDung] ON [dbo].[DonHang]
(
	[MaNguoiDung] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DonHang_TrangThai]    Script Date: 4/23/2026 6:34:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_DonHang_TrangThai] ON [dbo].[DonHang]
(
	[TrangThai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PhieuNhap_NgayNhap]    Script Date: 4/23/2026 6:34:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_PhieuNhap_NgayNhap] ON [dbo].[PhieuNhap]
(
	[NgayNhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_SanPham_DanhMuc]    Script Date: 4/23/2026 6:34:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_SanPham_DanhMuc] ON [dbo].[SanPham]
(
	[MaDanhMuc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_SanPham_HoatDong]    Script Date: 4/23/2026 6:34:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_SanPham_HoatDong] ON [dbo].[SanPham]
(
	[ConHoatDong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ThanhToan_MaDonHang]    Script Date: 4/23/2026 6:34:26 PM ******/
CREATE NONCLUSTERED INDEX [IX_ThanhToan_MaDonHang] ON [dbo].[ThanhToan]
(
	[MaDonHang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Banner] ADD  DEFAULT ((1)) FOR [ConHoatDong]
GO
ALTER TABLE [dbo].[Banner] ADD  DEFAULT ((0)) FOR [ThuTuHienThi]
GO
ALTER TABLE [dbo].[DanhGiaSanPham] ADD  DEFAULT (getdate()) FOR [NgayDanhGia]
GO
ALTER TABLE [dbo].[DanhMuc] ADD  DEFAULT ((1)) FOR [ConHoatDong]
GO
ALTER TABLE [dbo].[DonHang] ADD  DEFAULT ((0)) FOR [TongTienHang]
GO
ALTER TABLE [dbo].[DonHang] ADD  DEFAULT ((0)) FOR [PhiVanChuyen]
GO
ALTER TABLE [dbo].[DonHang] ADD  DEFAULT ((0)) FOR [TongThanhToan]
GO
ALTER TABLE [dbo].[DonHang] ADD  DEFAULT ('ChoDuyet') FOR [TrangThai]
GO
ALTER TABLE [dbo].[DonHang] ADD  DEFAULT ('TienMat') FOR [HinhThucThanhToan]
GO
ALTER TABLE [dbo].[DonHang] ADD  DEFAULT ('ChuaThanhToan') FOR [TrangThaiThanhToan]
GO
ALTER TABLE [dbo].[DonHang] ADD  DEFAULT (getdate()) FOR [NgayDat]
GO
ALTER TABLE [dbo].[GioHang] ADD  DEFAULT ((1)) FOR [SoLuong]
GO
ALTER TABLE [dbo].[GioHang] ADD  DEFAULT (getdate()) FOR [NgayThem]
GO
ALTER TABLE [dbo].[LienHe] ADD  DEFAULT ((0)) FOR [DaDoc]
GO
ALTER TABLE [dbo].[LienHe] ADD  DEFAULT (getdate()) FOR [NgayGui]
GO
ALTER TABLE [dbo].[NguoiDung] ADD  DEFAULT ('KhachHang') FOR [VaiTro]
GO
ALTER TABLE [dbo].[NguoiDung] ADD  DEFAULT ('default-avatar.png') FOR [AnhDaiDien]
GO
ALTER TABLE [dbo].[NguoiDung] ADD  DEFAULT ((1)) FOR [ConHoatDong]
GO
ALTER TABLE [dbo].[NguoiDung] ADD  DEFAULT (getdate()) FOR [NgayTao]
GO
ALTER TABLE [dbo].[NhaCungCap] ADD  DEFAULT ((1)) FOR [ConHoatDong]
GO
ALTER TABLE [dbo].[PhieuNhap] ADD  DEFAULT ((0)) FOR [TongTien]
GO
ALTER TABLE [dbo].[PhieuNhap] ADD  DEFAULT (getdate()) FOR [NgayNhap]
GO
ALTER TABLE [dbo].[SanPham] ADD  DEFAULT ((0)) FOR [GiaNhap]
GO
ALTER TABLE [dbo].[SanPham] ADD  DEFAULT ((0)) FOR [PhanTramGiam]
GO
ALTER TABLE [dbo].[SanPham] ADD  DEFAULT ((0)) FOR [SoLuongTon]
GO
ALTER TABLE [dbo].[SanPham] ADD  DEFAULT (N'Goi') FOR [DonViTinh]
GO
ALTER TABLE [dbo].[SanPham] ADD  DEFAULT ('no-image.png') FOR [HinhAnh]
GO
ALTER TABLE [dbo].[SanPham] ADD  DEFAULT ((1)) FOR [ConHoatDong]
GO
ALTER TABLE [dbo].[SanPham] ADD  DEFAULT (getdate()) FOR [NgayTao]
GO
ALTER TABLE [dbo].[ThanhToan] ADD  DEFAULT (getdate()) FOR [NgayThanhToan]
GO
ALTER TABLE [dbo].[ChiTietDonHang]  WITH CHECK ADD FOREIGN KEY([MaDonHang])
REFERENCES [dbo].[DonHang] ([MaDonHang])
GO
ALTER TABLE [dbo].[ChiTietDonHang]  WITH CHECK ADD FOREIGN KEY([MaSanPham])
REFERENCES [dbo].[SanPham] ([MaSanPham])
GO
ALTER TABLE [dbo].[ChiTietPhieuNhap]  WITH CHECK ADD FOREIGN KEY([MaPhieuNhap])
REFERENCES [dbo].[PhieuNhap] ([MaPhieuNhap])
GO
ALTER TABLE [dbo].[ChiTietPhieuNhap]  WITH CHECK ADD FOREIGN KEY([MaSanPham])
REFERENCES [dbo].[SanPham] ([MaSanPham])
GO
ALTER TABLE [dbo].[DanhGiaSanPham]  WITH CHECK ADD FOREIGN KEY([MaNguoiDung])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[DanhGiaSanPham]  WITH CHECK ADD FOREIGN KEY([MaSanPham])
REFERENCES [dbo].[SanPham] ([MaSanPham])
GO
ALTER TABLE [dbo].[DonHang]  WITH CHECK ADD FOREIGN KEY([MaNguoiDung])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[GioHang]  WITH CHECK ADD FOREIGN KEY([MaNguoiDung])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[GioHang]  WITH CHECK ADD FOREIGN KEY([MaSanPham])
REFERENCES [dbo].[SanPham] ([MaSanPham])
GO
ALTER TABLE [dbo].[PhieuNhap]  WITH CHECK ADD FOREIGN KEY([MaNhaCungCap])
REFERENCES [dbo].[NhaCungCap] ([MaNhaCungCap])
GO
ALTER TABLE [dbo].[PhieuNhap]  WITH CHECK ADD FOREIGN KEY([NguoiNhap])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[SanPham]  WITH CHECK ADD FOREIGN KEY([MaDanhMuc])
REFERENCES [dbo].[DanhMuc] ([MaDanhMuc])
GO
ALTER TABLE [dbo].[SanPham]  WITH CHECK ADD FOREIGN KEY([MaNhaCungCap])
REFERENCES [dbo].[NhaCungCap] ([MaNhaCungCap])
GO
ALTER TABLE [dbo].[ThanhToan]  WITH CHECK ADD  CONSTRAINT [FK_ThanhToan_DonHang] FOREIGN KEY([MaDonHang])
REFERENCES [dbo].[DonHang] ([MaDonHang])
GO
ALTER TABLE [dbo].[ThanhToan] CHECK CONSTRAINT [FK_ThanhToan_DonHang]
GO
ALTER TABLE [dbo].[ChiTietDonHang]  WITH CHECK ADD CHECK  (([SoLuong]>(0)))
GO
ALTER TABLE [dbo].[ChiTietPhieuNhap]  WITH CHECK ADD CHECK  (([SoLuong]>(0)))
GO
ALTER TABLE [dbo].[DanhGiaSanPham]  WITH CHECK ADD CHECK  (([SoSao]>=(1) AND [SoSao]<=(5)))
GO
ALTER TABLE [dbo].[DonHang]  WITH CHECK ADD CHECK  (([HinhThucThanhToan]='MoMo' OR [HinhThucThanhToan]='ChuyenKhoan' OR [HinhThucThanhToan]='TienMat'))
GO
ALTER TABLE [dbo].[DonHang]  WITH CHECK ADD CHECK  (([TrangThai]='DaHuy' OR [TrangThai]='DaGiao' OR [TrangThai]='DangGiao' OR [TrangThai]='DaXacNhan' OR [TrangThai]='ChoDuyet'))
GO
ALTER TABLE [dbo].[DonHang]  WITH CHECK ADD CHECK  (([TrangThaiThanhToan]='DaThanhToan' OR [TrangThaiThanhToan]='ChuaThanhToan'))
GO
ALTER TABLE [dbo].[GioHang]  WITH CHECK ADD CHECK  (([SoLuong]>(0)))
GO
ALTER TABLE [dbo].[NguoiDung]  WITH CHECK ADD CHECK  (([VaiTro]='KhachHang' OR [VaiTro]='QuanTri'))
GO
ALTER TABLE [dbo].[SanPham]  WITH CHECK ADD CHECK  (([PhanTramGiam]>=(0) AND [PhanTramGiam]<=(100)))
GO
ALTER TABLE [dbo].[SanPham]  WITH CHECK ADD CHECK  (([SoLuongTon]>=(0)))
GO
-- =============================================
-- Dữ liệu duy nhất: Admin (MaNguoiDung = 1)
-- =============================================
SET IDENTITY_INSERT [dbo].[NguoiDung] ON;
GO
INSERT INTO [dbo].[NguoiDung] ([MaNguoiDung], [HoTen], [Email], [MatKhau], [SoDienThoai], [DiaChi], [VaiTro], [AnhDaiDien], [ConHoatDong], [NgayTao])
VALUES (1, N'Quản trị viên', N'admin@doanvat.vn', N'Admin123', N'0901234567', N'Hà Nội', N'QuanTri', N'default-avatar.png', 1, GETDATE());
GO
SET IDENTITY_INSERT [dbo].[NguoiDung] OFF;
GO
/****** Object:  StoredProcedure [dbo].[sp_CapNhatTonKhoNhap]    Script Date: 4/23/2026 6:34:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CapNhatTonKhoNhap]
    @MaSanPham INT, 
    @SoLuong INT
AS 
BEGIN
    UPDATE SanPham 
    SET SoLuongTon = SoLuongTon + @SoLuong 
    WHERE MaSanPham = @MaSanPham;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_CapNhatTonKhoXuat]    Script Date: 4/23/2026 6:34:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CapNhatTonKhoXuat]
    @MaSanPham INT, 
    @SoLuong INT
AS 
BEGIN
    UPDATE SanPham 
    SET SoLuongTon = SoLuongTon - @SoLuong 
    WHERE MaSanPham = @MaSanPham AND SoLuongTon >= @SoLuong;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_ThongKeTongQuan]    Script Date: 4/23/2026 6:34:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Thống kê tổng quan cho Dashboard
CREATE PROCEDURE [dbo].[sp_ThongKeTongQuan] AS
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
PRINT N'DoAnVatA3: hoàn tất. Đăng nhập admin — Email: admin@doanvat.vn | Mật khẩu: Admin123';
GO