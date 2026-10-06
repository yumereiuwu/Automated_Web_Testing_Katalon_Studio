namespace A3_MVC_ASP.Models
{
    /// <summary>Dòng hiển thị danh sách sản phẩm (kết quả LINQ join DanhMuc).</summary>
    public class SanPhamListItemVm
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string TenDanhMuc { get; set; }
        public decimal GiaBan { get; set; }
        public decimal PhanTramGiam { get; set; }
        public int SoLuongTon { get; set; }
        public string DonViTinh { get; set; }
        public string HinhAnh { get; set; }
    }
}
