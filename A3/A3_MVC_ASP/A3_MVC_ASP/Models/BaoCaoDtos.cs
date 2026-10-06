namespace A3_MVC_ASP.Models
{
    public class ThongKeTongQuanDto
    {
        public int TongDonHang { get; set; }
        public decimal TongDoanhThu { get; set; }
        public int TongSanPham { get; set; }
        public int TongKhachHang { get; set; }
        public int DonChuaDuyet { get; set; }
        public int SanPhamSapHet { get; set; }
    }

    public class NhapXuatTonRow
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string TenDanhMuc { get; set; }
        public int TongSoLuongNhap { get; set; }
        public int TongSoLuongBan { get; set; }
        public int TonKhoHienTai { get; set; }
        public decimal TongTienNhap { get; set; }
        public decimal TongDoanhThu { get; set; }
    }

    public class DoanhThuThangRow
    {
        public int Nam { get; set; }
        public int Thang { get; set; }
        public int TongDonHang { get; set; }
        public decimal DoanhThu { get; set; }
    }
}
