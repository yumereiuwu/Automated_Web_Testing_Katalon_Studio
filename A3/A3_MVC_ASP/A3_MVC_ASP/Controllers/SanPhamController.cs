using System.Linq;
using System.Web.Mvc;
using A3_MVC_ASP.Models;

namespace A3_MVC_ASP.Controllers
{
    public class SanPhamController : Controller
    {
        /// <summary>Danh sách sản phẩm — LINQ (join, where, orderby); tìm kiếm q từ ô search header.</summary>
        public ActionResult Index(string q, string dm, string sort, int page = 1)
        {
            using (var db = new DoAnVatA3Context())
            {
                const int pageSize = 9;
                var query =
                    from sp in db.SanPhams
                    join d in db.DanhMucs on sp.MaDanhMuc equals d.MaDanhMuc
                    where sp.ConHoatDong && d.ConHoatDong
                    select new SanPhamListItemVm
                    {
                        MaSanPham = sp.MaSanPham,
                        TenSanPham = sp.TenSanPham,
                        TenDanhMuc = d.TenDanhMuc,
                        GiaBan = sp.GiaBan,
                        PhanTramGiam = sp.PhanTramGiam,
                        SoLuongTon = sp.SoLuongTon,
                        DonViTinh = sp.DonViTinh,
                        HinhAnh = sp.HinhAnh
                    };

                if (!string.IsNullOrWhiteSpace(dm))
                {
                    var danhMuc = dm.Trim();
                    query = query.Where(x => x.TenDanhMuc == danhMuc);
                }

                if (!string.IsNullOrWhiteSpace(q))
                {
                    var key = q.Trim();
                    query = query.Where(x => x.TenSanPham.Contains(key) || x.TenDanhMuc.Contains(key));
                }

                switch ((sort ?? string.Empty).Trim().ToLowerInvariant())
                {
                    case "price_asc":
                        query = query.OrderBy(x => x.GiaBan * (100 - x.PhanTramGiam) / 100m).ThenBy(x => x.TenSanPham);
                        break;
                    case "price_desc":
                        query = query.OrderByDescending(x => x.GiaBan * (100 - x.PhanTramGiam) / 100m).ThenBy(x => x.TenSanPham);
                        break;
                    case "newest":
                        query = query.OrderByDescending(x => x.MaSanPham);
                        break;
                    default:
                        query = query.OrderBy(x => x.TenSanPham);
                        break;
                }

                ViewBag.DanhMucList = db.DanhMucs
                    .Where(x => x.ConHoatDong)
                    .OrderBy(x => x.TenDanhMuc)
                    .Select(x => x.TenDanhMuc)
                    .ToList();
                ViewBag.Title = "Sản phẩm";
                ViewBag.SearchQuery = q;
                ViewBag.DanhMuc = dm;
                ViewBag.Sort = sort;
                var totalItems = query.Count();
                var totalPages = totalItems > 0 ? (int)System.Math.Ceiling(totalItems / (double)pageSize) : 1;
                if (page < 1) page = 1;
                if (page > totalPages) page = totalPages;

                var list = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                ViewBag.TotalItems = totalItems;
                ViewBag.PageSize = pageSize;
                ViewBag.Page = page;
                ViewBag.TotalPages = totalPages;
                return View(list);
            }
        }

        public ActionResult ChiTiet(int? id)
        {
            if (id == null)
                return HttpNotFound();

            using (var db = new DoAnVatA3Context())
            {
                var q = from sp in db.SanPhams
                    join dm in db.DanhMucs on sp.MaDanhMuc equals dm.MaDanhMuc
                    where sp.MaSanPham == id && sp.ConHoatDong && dm.ConHoatDong
                    select new SanPhamChiTietVm
                    {
                        MaSanPham = sp.MaSanPham,
                        TenSanPham = sp.TenSanPham,
                        TenDanhMuc = dm.TenDanhMuc,
                        MoTa = sp.MoTa,
                        GiaBan = sp.GiaBan,
                        PhanTramGiam = sp.PhanTramGiam,
                        SoLuongTon = sp.SoLuongTon,
                        DonViTinh = sp.DonViTinh,
                        HinhAnh = sp.HinhAnh
                    };
                var vm = q.FirstOrDefault();
                if (vm == null)
                    return HttpNotFound();

                ViewBag.Title = vm.TenSanPham;
                return View(vm);
            }
        }

        [HttpGet]
        public JsonResult GoiYTimKiem(string q)
        {
            var key = (q ?? string.Empty).Trim();
            if (key.Length < 1)
                return Json(new object[0], JsonRequestBehavior.AllowGet);

            using (var db = new DoAnVatA3Context())
            {
                var data = (from sp in db.SanPhams
                            join dm in db.DanhMucs on sp.MaDanhMuc equals dm.MaDanhMuc
                            where sp.ConHoatDong && dm.ConHoatDong
                                  && (sp.TenSanPham.Contains(key) || dm.TenDanhMuc.Contains(key))
                            orderby sp.TenSanPham
                            select new
                            {
                                sp.MaSanPham,
                                sp.TenSanPham,
                                dm.TenDanhMuc,
                                sp.HinhAnh,
                                sp.GiaBan,
                                sp.PhanTramGiam
                            })
                    .Take(12)
                    .ToList()
                    .Select(x => new
                    {
                        id = x.MaSanPham,
                        ten = x.TenSanPham,
                        danhMuc = x.TenDanhMuc,
                        hinh = Url.Content("~/Content/SanPhamImages/" + (string.IsNullOrWhiteSpace(x.HinhAnh) ? "no-image.png" : x.HinhAnh)),
                        giaGoc = x.GiaBan,
                        giaBan = x.PhanTramGiam > 0 ? (x.GiaBan * (100 - x.PhanTramGiam) / 100) : x.GiaBan
                    })
                    .ToList();

                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
