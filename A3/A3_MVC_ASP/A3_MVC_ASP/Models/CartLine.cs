using System;

namespace A3_MVC_ASP.Models
{
    [Serializable]
    public class CartLine
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public decimal GiaBan { get; set; }
        public decimal PhanTramGiam { get; set; }
        public int SoLuong { get; set; }
        public string HinhAnh { get; set; }
        public string DonViTinh { get; set; }

        public decimal ThanhTienHang
        {
            get
            {
                var gia = GiaBan * (100 - PhanTramGiam) / 100m;
                return Math.Round(gia * SoLuong, 0, MidpointRounding.AwayFromZero);
            }
        }
    }
}
