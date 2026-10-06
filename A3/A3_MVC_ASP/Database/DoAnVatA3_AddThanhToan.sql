/*
  Tạo bảng ThanhToan để lưu log thanh toán (demo/real).
  Chạy 1 lần trên DB DoAnVatA3.
*/

IF OBJECT_ID('dbo.ThanhToan', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ThanhToan (
        MaThanhToan INT IDENTITY(1,1) PRIMARY KEY,
        MaDonHang INT NOT NULL,
        Provider VARCHAR(30) NOT NULL,
        MaGiaoDich VARCHAR(50) NULL,
        SoTien DECIMAL(18,0) NOT NULL,
        TrangThai VARCHAR(20) NOT NULL,
        NgayThanhToan DATETIME NOT NULL DEFAULT GETDATE(),
        RawJson NVARCHAR(MAX) NULL,
        CONSTRAINT FK_ThanhToan_DonHang FOREIGN KEY (MaDonHang) REFERENCES dbo.DonHang(MaDonHang)
    );
    CREATE INDEX IX_ThanhToan_MaDonHang ON dbo.ThanhToan(MaDonHang);
END
GO

SELECT TOP 50 * FROM dbo.ThanhToan ORDER BY MaThanhToan DESC;

