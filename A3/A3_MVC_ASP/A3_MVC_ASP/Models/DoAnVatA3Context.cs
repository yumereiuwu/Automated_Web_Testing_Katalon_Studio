using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace A3_MVC_ASP.Models
{
    /// <summary>Entity Framework 6 — map tới CSDL DoAnVatA3 (Code First map tới DB có sẵn). Truy vấn bằng LINQ (LINQ to Entities).</summary>
    public class DoAnVatA3Context : DbContext
    {
        public DoAnVatA3Context()
            : base("name=DoAnVatA3Connection")
        {
        }

        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<PhieuNhap> PhieuNhaps { get; set; }
        public DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<DanhGiaSanPham> DanhGiaSanPhams { get; set; }
        public DbSet<LienHe> LienHes { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<ThanhToan> ThanhToans { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<NguoiDung>().HasKey(e => e.MaNguoiDung);
            modelBuilder.Entity<DanhMuc>().HasKey(e => e.MaDanhMuc);
            modelBuilder.Entity<NhaCungCap>().HasKey(e => e.MaNhaCungCap);
            modelBuilder.Entity<SanPham>().HasKey(e => e.MaSanPham);
            modelBuilder.Entity<PhieuNhap>().HasKey(e => e.MaPhieuNhap);
            modelBuilder.Entity<ChiTietPhieuNhap>().HasKey(e => e.MaChiTiet);
            modelBuilder.Entity<DonHang>().HasKey(e => e.MaDonHang);
            modelBuilder.Entity<ChiTietDonHang>().HasKey(e => e.MaChiTiet);
            modelBuilder.Entity<GioHang>().HasKey(e => e.MaGioHang);
            modelBuilder.Entity<DanhGiaSanPham>().HasKey(e => e.MaDanhGia);
            modelBuilder.Entity<LienHe>().HasKey(e => e.MaLienHe);
            modelBuilder.Entity<Banner>().HasKey(e => e.MaBanner);
            modelBuilder.Entity<ThanhToan>().HasKey(e => e.MaThanhToan);

            const byte moneyPrec = 18;
            const byte moneyScale = 0;

            modelBuilder.Entity<ChiTietPhieuNhap>()
                .Property(e => e.ThanhTien)
                .HasPrecision(moneyPrec, moneyScale)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            modelBuilder.Entity<ChiTietDonHang>()
                .Property(e => e.ThanhTien)
                .HasPrecision(moneyPrec, moneyScale)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            modelBuilder.Entity<SanPham>().Property(e => e.GiaNhap).HasPrecision(moneyPrec, moneyScale);
            modelBuilder.Entity<SanPham>().Property(e => e.GiaBan).HasPrecision(moneyPrec, moneyScale);
            modelBuilder.Entity<SanPham>().Property(e => e.PhanTramGiam).HasPrecision((byte)5, (byte)2);
            modelBuilder.Entity<PhieuNhap>().Property(e => e.TongTien).HasPrecision(moneyPrec, moneyScale);
            modelBuilder.Entity<ChiTietPhieuNhap>().Property(e => e.DonGiaNhap).HasPrecision(moneyPrec, moneyScale);
            modelBuilder.Entity<DonHang>().Property(e => e.TongTienHang).HasPrecision(moneyPrec, moneyScale);
            modelBuilder.Entity<DonHang>().Property(e => e.PhiVanChuyen).HasPrecision(moneyPrec, moneyScale);
            modelBuilder.Entity<DonHang>().Property(e => e.TongThanhToan).HasPrecision(moneyPrec, moneyScale);
            modelBuilder.Entity<ChiTietDonHang>().Property(e => e.DonGiaBan).HasPrecision(moneyPrec, moneyScale);
            modelBuilder.Entity<ThanhToan>().Property(e => e.SoTien).HasPrecision(moneyPrec, moneyScale);
        }
    }
}
