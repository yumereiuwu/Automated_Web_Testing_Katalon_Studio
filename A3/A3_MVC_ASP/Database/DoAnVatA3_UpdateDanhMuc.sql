/*
  Cập nhật danh mục theo yêu cầu (KHÔNG sửa/ thêm sản phẩm).
  - Giữ nguyên MaDanhMuc hiện có để các sản phẩm đang trỏ FK không bị lỗi.
  - Đổi tên 1..6, thêm 7..9 nếu chưa có.
*/

SET NOCOUNT ON;

-- 1..6: update theo ID (đảm bảo không đụng FK của SanPham)
IF EXISTS (SELECT 1 FROM DanhMuc WHERE MaDanhMuc = 1)
    UPDATE DanhMuc SET TenDanhMuc = N'Snack', ConHoatDong = 1 WHERE MaDanhMuc = 1;

IF EXISTS (SELECT 1 FROM DanhMuc WHERE MaDanhMuc = 2)
    UPDATE DanhMuc SET TenDanhMuc = N'Kẹo Bánh', ConHoatDong = 1 WHERE MaDanhMuc = 2;

IF EXISTS (SELECT 1 FROM DanhMuc WHERE MaDanhMuc = 3)
    UPDATE DanhMuc SET TenDanhMuc = N'Đồ ăn liền', ConHoatDong = 1 WHERE MaDanhMuc = 3;

IF EXISTS (SELECT 1 FROM DanhMuc WHERE MaDanhMuc = 4)
    UPDATE DanhMuc SET TenDanhMuc = N'Đồ khô', ConHoatDong = 1 WHERE MaDanhMuc = 4;

IF EXISTS (SELECT 1 FROM DanhMuc WHERE MaDanhMuc = 5)
    UPDATE DanhMuc SET TenDanhMuc = N'Đồ sấy', ConHoatDong = 1 WHERE MaDanhMuc = 5;

IF EXISTS (SELECT 1 FROM DanhMuc WHERE MaDanhMuc = 6)
    UPDATE DanhMuc SET TenDanhMuc = N'Ô mai', ConHoatDong = 1 WHERE MaDanhMuc = 6;

-- 7..9: insert nếu thiếu (không ảnh hưởng SanPham)
IF NOT EXISTS (SELECT 1 FROM DanhMuc WHERE MaDanhMuc = 7)
    INSERT INTO DanhMuc (MaDanhMuc, TenDanhMuc, MoTa, HinhAnh, ConHoatDong)
    VALUES (7, N'Bánh cốm', N'Bánh cốm', N'dm-banhcom.jpg', 1);

IF NOT EXISTS (SELECT 1 FROM DanhMuc WHERE MaDanhMuc = 8)
    INSERT INTO DanhMuc (MaDanhMuc, TenDanhMuc, MoTa, HinhAnh, ConHoatDong)
    VALUES (8, N'Đồ ăn healthy', N'Healthy / ít đường / tốt cho sức khỏe', N'dm-healthy.jpg', 1);

IF NOT EXISTS (SELECT 1 FROM DanhMuc WHERE MaDanhMuc = 9)
    INSERT INTO DanhMuc (MaDanhMuc, TenDanhMuc, MoTa, HinhAnh, ConHoatDong)
    VALUES (9, N'Tất cả', N'Tổng hợp', N'dm-tatca.jpg', 1);

-- Nếu IDENTITY đang bật, đảm bảo reseed >= 9
DECLARE @mx INT = (SELECT ISNULL(MAX(MaDanhMuc), 0) FROM DanhMuc);
IF @mx >= 9
    DBCC CHECKIDENT ('DanhMuc', RESEED, @mx);

SELECT MaDanhMuc, TenDanhMuc, ConHoatDong FROM DanhMuc ORDER BY MaDanhMuc;

