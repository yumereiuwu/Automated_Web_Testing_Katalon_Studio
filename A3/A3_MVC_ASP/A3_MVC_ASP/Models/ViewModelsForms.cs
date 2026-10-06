using System.ComponentModel.DataAnnotations;

namespace A3_MVC_ASP.Models
{
    public class LoginVm
    {
        [Required(ErrorMessage = "Nhập email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class RegisterVm
    {
        [Required(ErrorMessage = "Nhập họ tên")]
        [StringLength(100)]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Nhập email")]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nhập mật khẩu")]
        [StringLength(100, MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

        [DataType(DataType.Password)]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string MatKhauXacNhan { get; set; }

        [StringLength(15)]
        public string SoDienThoai { get; set; }

        [StringLength(255)]
        public string DiaChi { get; set; }
    }

    public class ForgotPasswordVm
    {
        [Required(ErrorMessage = "Nhập email")]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string MatKhauMoi { get; set; }

        [DataType(DataType.Password)]
        [Compare("MatKhauMoi", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string XacNhanMatKhauMoi { get; set; }

        public string Otp { get; set; }
        public bool IsOtpStep { get; set; }
    }

    public class DatHangVm
    {
        [StringLength(100)]
        public string TenKhachHang { get; set; }

        [StringLength(15)]
        public string SoDienThoai { get; set; }

        [Required(ErrorMessage = "Nhập địa chỉ giao hàng")]
        [StringLength(255)]
        public string DiaChiGiao { get; set; }

        [Required(ErrorMessage = "Chọn tỉnh/thành nhận hàng")]
        [StringLength(100)]
        public string TinhThanh { get; set; }

        public decimal PhiVanChuyen { get; set; }

        [Required]
        public string HinhThucThanhToan { get; set; }

        [StringLength(500)]
        public string GhiChu { get; set; }
    }

    public class LienHeGuiVm
    {
        [Required]
        [StringLength(100)]
        public string HoTen { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; }

        [StringLength(15)]
        public string SoDienThoai { get; set; }

        [StringLength(200)]
        public string TieuDe { get; set; }

        [Required]
        [StringLength(2000)]
        public string NoiDung { get; set; }
    }

    public class SanPhamFormVm
    {
        public int? MaSanPham { get; set; }

        [Required]
        [StringLength(150)]
        public string TenSanPham { get; set; }

        [Required]
        public int MaDanhMuc { get; set; }

        public int? MaNhaCungCap { get; set; }

        [StringLength(1000)]
        public string MoTa { get; set; }

        public decimal GiaNhap { get; set; }

        public decimal GiaBan { get; set; }

        public decimal PhanTramGiam { get; set; }

        [Range(0, int.MaxValue)]
        public int SoLuongTon { get; set; }

        [StringLength(30)]
        public string DonViTinh { get; set; }

        [StringLength(255)]
        public string HinhAnh { get; set; }

        public bool ConHoatDong { get; set; }
    }

    public class PhieuNhapTaoVm
    {
        [Required]
        [StringLength(20)]
        public string MaPhieu { get; set; }

        public int? MaNhaCungCap { get; set; }

        [StringLength(500)]
        public string GhiChu { get; set; }

        [Required]
        public int MaSanPham { get; set; }

        [Range(1, int.MaxValue)]
        public int SoLuong { get; set; }

        public decimal DonGiaNhap { get; set; }
    }
}
