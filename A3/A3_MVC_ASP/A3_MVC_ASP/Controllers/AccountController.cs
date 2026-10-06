using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Helpers;
using System.Web.Mvc;
using A3_MVC_ASP.Models;

namespace A3_MVC_ASP.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login(string returnUrl)
        {
            ViewBag.Title = "Đăng nhập";
            return View(new LoginVm { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVm model)
        {
            ViewBag.Title = "Đăng nhập";
            if (!ModelState.IsValid)
                return View(model);

            using (var db = new DoAnVatA3Context())
            {
                var email = model.Email.Trim();
                var u = db.NguoiDungs.FirstOrDefault(x => x.Email == email);
                if (u == null || !KiemTraMatKhau(u, model.MatKhau))
                {
                    ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
                    return View(model);
                }
                if (!u.ConHoatDong)
                {
                    ModelState.AddModelError("", "Tài khoản đã bị khóa.");
                    return View(model);
                }

                Session["MaNguoiDung"] = u.MaNguoiDung;
                Session["HoTen"] = u.HoTen;
                Session["Email"] = u.Email;
                Session["VaiTro"] = u.VaiTro;

                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);

                if (u.VaiTro == "QuanTri")
                    return RedirectToAction("Index", "Admin");

                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>So khớp mật khẩu: ưu tiên plain text; tài khoản seed (hash Identity) có thể đăng nhập bằng Admin@123 / 123456 nếu khớp email demo.</summary>
        private static bool KiemTraMatKhau(NguoiDung u, string mk)
        {
            if (u == null || string.IsNullOrEmpty(mk)) return false;
            if (string.Equals(u.MatKhau, mk, System.StringComparison.Ordinal)) return true;

            // Hỗ trợ tài khoản lưu mật khẩu dạng hash (Crypto.HashPassword).
            if (!string.IsNullOrWhiteSpace(u.MatKhau))
            {
                try
                {
                    if (Crypto.VerifyHashedPassword(u.MatKhau, mk))
                        return true;
                }
                catch
                {
                    // Nếu không phải chuỗi hash hợp lệ thì bỏ qua, tiếp tục các rule khác.
                }
            }

            if (string.Equals(u.Email, "admin@doAnvat.vn", System.StringComparison.OrdinalIgnoreCase) && mk == "Admin@123")
                return true;
            if ((u.Email == "an@gmail.com" || u.Email == "binh@gmail.com") && mk == "123456")
                return true;
            return false;
        }

        public ActionResult Register()
        {
            ViewBag.Title = "Đăng ký";
            return View(new RegisterVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVm model)
        {
            ViewBag.Title = "Đăng ký";
            if (!ModelState.IsValid)
                return View(model);

            using (var db = new DoAnVatA3Context())
            {
                var email = model.Email.Trim();
                if (db.NguoiDungs.Any(x => x.Email == email))
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng.");
                    return View(model);
                }

                var u = new NguoiDung
                {
                    HoTen = model.HoTen.Trim(),
                    Email = email,
                    MatKhau = model.MatKhau,
                    SoDienThoai = model.SoDienThoai,
                    DiaChi = model.DiaChi,
                    VaiTro = "KhachHang",
                    AnhDaiDien = "default-avatar.png",
                    ConHoatDong = true,
                    NgayTao = System.DateTime.Now
                };
                db.NguoiDungs.Add(u);
                db.SaveChanges();

                Session["MaNguoiDung"] = u.MaNguoiDung;
                Session["HoTen"] = u.HoTen;
                Session["Email"] = u.Email;
                Session["VaiTro"] = u.VaiTro;

                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult ForgotPassword()
        {
            ViewBag.Title = "Quên mật khẩu";
            return View(new ForgotPasswordVm { IsOtpStep = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordVm model)
        {
            ViewBag.Title = "Quên mật khẩu";
            if (model == null) model = new ForgotPasswordVm();

            var isOtpPost = model.IsOtpStep
                            || !string.IsNullOrWhiteSpace(model.Otp)
                            || !string.IsNullOrWhiteSpace(model.MatKhauMoi)
                            || !string.IsNullOrWhiteSpace(model.XacNhanMatKhauMoi);

            if (!isOtpPost)
            {
                if (string.IsNullOrWhiteSpace(model.Email))
                {
                    ModelState.AddModelError("Email", "Nhập email");
                    return View(model);
                }

                var email = model.Email.Trim();
                using (var db = new DoAnVatA3Context())
                {
                    var u = db.NguoiDungs.FirstOrDefault(x => x.Email == email && x.ConHoatDong);
                    if (u == null)
                    {
                        ModelState.AddModelError("Email", "Không tìm thấy tài khoản với email này.");
                        return View(model);
                    }
                }

                var otp = new System.Random().Next(100000, 999999).ToString();
                Session["ForgotOtpCode"] = otp;
                Session["ForgotOtpEmail"] = email;
                Session["ForgotOtpExpire"] = System.DateTime.Now.AddMinutes(10);

                string err;
                if (!TrySendOtpEmail(email, otp, out err))
                {
                    ModelState.AddModelError("", err);
                    return View(model);
                }

                ViewBag.Info = "Đã gửi mã OTP về email. Vui lòng nhập OTP và mật khẩu mới trong 10 phút.";
                return View(new ForgotPasswordVm { Email = email, IsOtpStep = true });
            }

            // Bước xác thực OTP + đổi mật khẩu
            var ssEmail = Session["ForgotOtpEmail"] as string;
            var ssOtp = Session["ForgotOtpCode"] as string;
            var ssExpireObj = Session["ForgotOtpExpire"];
            var ssExpire = ssExpireObj is System.DateTime dt ? dt : (System.DateTime?)null;
            if (string.IsNullOrWhiteSpace(ssEmail) || string.IsNullOrWhiteSpace(ssOtp) || !ssExpire.HasValue)
            {
                ModelState.AddModelError("", "Phiên đặt lại mật khẩu đã hết hạn. Vui lòng gửi OTP lại.");
                model.IsOtpStep = false;
                return View(model);
            }

            if (!string.Equals((model.Email ?? "").Trim(), ssEmail, System.StringComparison.OrdinalIgnoreCase))
                ModelState.AddModelError("Email", "Email không khớp với email đã gửi OTP.");
            if (System.DateTime.Now > ssExpire.Value)
                ModelState.AddModelError("Otp", "Mã OTP đã hết hạn.");
            if (string.IsNullOrWhiteSpace(model.Otp) || model.Otp.Trim() != ssOtp)
                ModelState.AddModelError("Otp", "Mã OTP không đúng.");
            if (string.IsNullOrWhiteSpace(model.MatKhauMoi) || model.MatKhauMoi.Length < 4)
                ModelState.AddModelError("MatKhauMoi", "Mật khẩu mới tối thiểu 4 ký tự.");
            if (!string.Equals(model.MatKhauMoi ?? "", model.XacNhanMatKhauMoi ?? "", System.StringComparison.Ordinal))
                ModelState.AddModelError("XacNhanMatKhauMoi", "Mật khẩu xác nhận không khớp.");

            if (!ModelState.IsValid)
            {
                model.IsOtpStep = true;
                return View(model);
            }

            using (var db = new DoAnVatA3Context())
            {
                var email = ssEmail;
                var u = db.NguoiDungs.FirstOrDefault(x => x.Email == email && x.ConHoatDong);
                if (u == null)
                {
                    ModelState.AddModelError("Email", "Không tìm thấy tài khoản với email này.");
                    model.IsOtpStep = false;
                    return View(model);
                }

                u.MatKhau = model.MatKhauMoi;
                db.SaveChanges();
            }

            Session.Remove("ForgotOtpCode");
            Session.Remove("ForgotOtpEmail");
            Session.Remove("ForgotOtpExpire");
            TempData["Msg"] = "Đã cập nhật mật khẩu. Bạn có thể đăng nhập lại.";
            return RedirectToAction("Login");
        }

        private static bool TrySendOtpEmail(string toEmail, string otp, out string error)
        {
            error = "";
            try
            {
                var host = System.Configuration.ConfigurationManager.AppSettings["Mail:SmtpHost"];
                var portText = System.Configuration.ConfigurationManager.AppSettings["Mail:SmtpPort"];
                var user = System.Configuration.ConfigurationManager.AppSettings["Mail:Username"];
                var pass = System.Configuration.ConfigurationManager.AppSettings["Mail:Password"];
                var from = System.Configuration.ConfigurationManager.AppSettings["Mail:From"] ?? user;
                var sslText = System.Configuration.ConfigurationManager.AppSettings["Mail:EnableSsl"];

                int port;
                if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass) ||
                    !int.TryParse(portText, out port))
                {
                    error = "Chưa cấu hình SMTP gửi mail (Mail:* trong Web.config).";
                    return false;
                }

                var ssl = string.Equals(sslText, "true", System.StringComparison.OrdinalIgnoreCase);
                using (var msg = new MailMessage(from, toEmail))
                {
                    msg.Subject = "Ma OTP dat lai mat khau";
                    msg.Body = "Ma OTP cua ban la: " + otp + "\nMa co hieu luc trong 10 phut.";
                    msg.IsBodyHtml = false;

                    using (var smtp = new SmtpClient(host, port))
                    {
                        smtp.EnableSsl = ssl;
                        smtp.Credentials = new NetworkCredential(user, pass);
                        smtp.Send(msg);
                    }
                }
                return true;
            }
            catch (System.Exception ex)
            {
                error = "Không gửi được email OTP: " + ex.Message;
                return false;
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
