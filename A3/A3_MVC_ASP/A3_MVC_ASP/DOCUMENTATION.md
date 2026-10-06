## Tổng quan

`A3_MVC_ASP` là ứng dụng **ASP.NET MVC 5** (C#) theo mô hình **MVC**:
- **Controllers**: nhận request, xử lý nghiệp vụ, trả về View/JSON/File.
- **Models**: EF6 `DbContext` + entity DB + ViewModel/DTO.
- **Views**: Razor `.cshtml` hiển thị UI (Storefront + Admin).
- **Content/Scripts**: CSS/JS phía client.
- **Services**: tích hợp cổng thanh toán (MoMo/VNPAY).

Route mặc định:

```10:21:g:\mvc\A3\A3_MVC_ASP\A3_MVC_ASP\App_Start\RouteConfig.cs
routes.MapRoute(
    name: "Default",
    url: "{controller}/{action}/{id}",
    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
);
```

---

## Chức năng theo folder

### `Controllers/`

#### `HomeController`
- **`Index`**: trang chủ; load:
  - Danh mục nổi bật (4 mục) + số lượng sản phẩm theo danh mục.
  - Sản phẩm bán chạy (fallback: sản phẩm mới) + sản phẩm giảm giá.
  - Một số bài viết “nổi bật” dạng mock data.
- **`About`**: giới thiệu.
- **`Contact`**: trang liên hệ (legacy, không phải form gửi).

#### `SanPhamController`
- **`Index(q, dm, sort, page)`**: danh sách sản phẩm:
  - Filter theo danh mục `dm` (exact match).
  - Search theo từ khoá `q` (Contains).
  - Sort theo `price_asc/price_desc/newest` (mặc định theo tên).
  - Pagination (9 SP/trang), ViewBag: `TotalItems/PageSize/Page/TotalPages`.
- **`ChiTiet(id)`**: chi tiết sản phẩm theo `id`.
- **`GoiYTimKiem(q)`** *(JSON)*: endpoint gợi ý tìm kiếm (tối đa 12), trả về shape:
  - `{ id, ten, danhMuc, hinh, giaGoc, giaBan }`

#### `GioHangController`
- **`Index`**: hiển thị giỏ hàng (lưu trong Session).
- **`Them(id, sl)`**: thêm sản phẩm vào giỏ; kiểm tồn kho.
- **`CapNhat(id, soLuong)`** *(POST)*: cập nhật số lượng; clamp theo tồn.
- **`Xoa(id)`** *(POST)*: xoá dòng giỏ hàng.

#### `DonHangController`
- **`DatHang` (GET)**: trang checkout; nạp thông tin user nếu đã login.
- **`DatHang` (POST)**: tạo `DonHang` + `ChiTietDonHang` trong transaction:
  - Kiểm tồn kho trước khi tạo.
  - Tạo `MaDon` dạng `DHyyyyMMddHHmmssXYZ` (cắt max 20 chars).
  - Điều hướng theo phương thức thanh toán:
    - `MoMo` → `Payment/MomoCreate`
    - `ChuyenKhoan` → `Vnpay/Create`
    - còn lại → tiền mặt, clear giỏ.
- **`HoanTat(id)`**: trang hoàn tất, hiển thị đơn + thanh toán gần nhất.

#### `AccountController`
- **`Login` (GET/POST)**: đăng nhập; set Session: `MaNguoiDung/HoTen/Email/VaiTro`.
  - Hỗ trợ plain text password và verify hash (`Crypto.VerifyHashedPassword`).
- **`Register` (GET/POST)**: đăng ký khách hàng.
- **`ForgotPassword` (GET/POST)**: quên mật khẩu 2 bước:
  - Bước 1: gửi OTP về email (Session: `ForgotOtpCode/ForgotOtpEmail/ForgotOtpExpire`).
  - Bước 2: verify OTP + đổi mật khẩu.

#### `LienHeController`
- **`Index`**: form liên hệ.
- **`Gui`** *(POST)*: lưu `LienHe` vào DB, set `DaDoc=false`.

#### `TinTucController`
- **`Index`**: trang tin tức (view tĩnh).

#### `ChinhSachController`
- **`GiaoHang`**, **`DoiTra`**, **`BaoMat`**: các trang chính sách (view tĩnh).

#### `PaymentController` (MoMo)
- **`MomoCreate(id)`**: tạo thanh toán MoMo và redirect `payUrl`.
- **`MomoReturn(id)`**: return URL; có option demo `Momo:AutoMarkPaidOnReturn`.
- **`MomoIpn()`** *(POST)*: IPN; verify signature (có thể bật `Momo:SkipSignature=true` để demo).

#### `VnpayController` (VNPAY sandbox)
- **`Create(id)`**: tạo URL thanh toán.
- **`Return()`**: nhận querystring, cập nhật DB, lưu bảng `ThanhToan`, hoàn kho nếu thất bại.

#### `AdminController` *(yêu cầu quyền quản trị)*
Auth bằng attribute:

```7:21:g:\mvc\A3\A3_MVC_ASP\A3_MVC_ASP\Filters\QuanTriAuthorizeAttribute.cs
public class QuanTriAuthorizeAttribute : AuthorizeAttribute
{
    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        return httpContext.Session != null
            && httpContext.Session["VaiTro"] as string == "QuanTri";
    }
    // ...
}
```

Các nhóm chức năng chính:
- **Dashboard (`Index`)**: tổng quan + đơn gần đây + sản phẩm sắp hết + dữ liệu chart 6 tháng.
- **Danh mục**: list/tạo/sửa/xoá (chặn xoá nếu còn sản phẩm).
- **Sản phẩm**: list/tạo/sửa/xem chi tiết (có ảnh + thông tin đầy đủ).
- **Liên hệ**: list/xem/xoá + **đếm chưa đọc realtime**:
  - `QuanLyLienHeUnreadCount()` trả JSON `{count}`.
  - `TopbarStats()` trả JSON `{ pendingOrders, unreadContacts }`.
- **Đơn hàng**: list + paging + xem + xoá (xử lý FK: xoá con trước).
- **Phiếu nhập**: list + tạo + xoá (ràng buộc tồn kho để không âm).
- **Tài khoản**: list + paging + xoá (không xoá admin/self; null FK liên quan).

#### `BaoCaoController` *(admin)*
- **`Index(nxPage)`**: thống kê tổng quan + doanh thu theo tháng + bảng Nhập-Xuất-Tồn (paging 10 dòng).
- **`XuatExcel()`**: export `.xls` theo HTML + CSS, **UTF-8 BOM** để không lỗi tiếng Việt.

---

### `Models/`
- EF6: `DoAnVatA3Context` (DBContext) + các entity trong `DatabaseModels.cs`.
- ViewModel/DTO: phục vụ UI và báo cáo (dashboard, danh sách sản phẩm, cart line, báo cáo…).

### `Helpers/`
- `GioHangSessionHelper`: đọc/ghi giỏ hàng trong Session, tính tổng số lượng/tổng tiền.

### `Services/`
- `MomoPaymentService`: tạo payment + ký HMACSHA256 + verify IPN (demo).
- `VnpayPaymentService`: tạo URL + ký HMACSHA512 + verify return.

### `Views/`
- `Views/Shared/_Layout.cshtml`: layout storefront, có JS gợi ý tìm kiếm (fetch JSON).
- `Views/Shared/_LayoutAdmin.cshtml`: layout admin, có:
  - Toggle sidebar responsive
  - Fullscreen
  - Quick search (Ctrl+K)
  - Polling badge topbar + sidebar

### `Content/` (CSS)
Hiện đã tách theo module (để dễ quản lý):
- Storefront: `base.css` (global), `site.header.css`, `site.footer.css`, `site.contact.css`, `home.css`, `product.css`
- Admin: `admin.vars.css`, `admin.layout.css`, `admin.components.css`, `admin.dashboard.css`

Bundle load tại `App_Start/BundleConfig.cs`:
- `~/Content/css` (storefront) và `~/Content/admincss` (admin).

---

## Luồng nghiệp vụ chính

### Mua hàng → tạo đơn → thanh toán
1. User thêm sản phẩm vào giỏ (`GioHang/Them`).
2. Checkout (`DonHang/DatHang` POST) tạo `DonHang` + `ChiTietDonHang`.
3. Điều hướng:
   - MoMo: `Payment/MomoCreate` → `payUrl` → `MomoIpn` cập nhật trạng thái.
   - VNPAY: `Vnpay/Create` → redirect sandbox → `Vnpay/Return` cập nhật trạng thái.
4. `DonHang/HoanTat` hiển thị kết quả.

### Liên hệ realtime trong admin
1. User gửi form (`LienHe/Gui`) → lưu DB `DaDoc=false`.
2. Admin sidebar/topbar polling:
   - `Admin/QuanLyLienHeUnreadCount` cập nhật badge `+N`.
   - `Admin/TopbarStats` cập nhật “đơn chờ duyệt” + “liên hệ chưa đọc”.

---

## Ghi chú refactor (View)
- Đã **đổi tên biến local trong Razor/JS** sang tiếng Việt trong các view có JS inline, nhưng **không đổi** selector/class/id, query key (`q/dm/sort/page`), và shape JSON để tránh lỗi.
- Đã **tách CSS** thành nhiều file theo khu vực; cập nhật `BundleConfig.cs` và `A3_MVC_ASP.csproj` để include các file mới.

