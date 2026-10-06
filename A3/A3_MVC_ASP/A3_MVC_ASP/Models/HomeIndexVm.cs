using System.Collections.Generic;

namespace A3_MVC_ASP.Models
{
    public class HomeIndexVm
    {
        public IList<HomeDanhMucVm> DanhMucs { get; set; }
        public IList<HomeSanPhamVm> SanPhamBanChays { get; set; }
        public IList<HomeSanPhamVm> SanPhamGiamGias { get; set; }
        public IList<HomeBaiVietVm> BaiVietNoiBats { get; set; }
    }

    public class HomeDanhMucVm
    {
        public int MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }
        public string HinhAnh { get; set; }
        public int SoLuongSanPham { get; set; }
    }

    public class HomeSanPhamVm
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string TenDanhMuc { get; set; }
        public string HinhAnh { get; set; }
        public decimal GiaBan { get; set; }
        public decimal PhanTramGiam { get; set; }
    }

    public class HomeBaiVietVm
    {
        public int MaBaiViet { get; set; }
        public string TieuDe { get; set; }
        public string TomTat { get; set; }
        public string NgayText { get; set; }
    }
}
