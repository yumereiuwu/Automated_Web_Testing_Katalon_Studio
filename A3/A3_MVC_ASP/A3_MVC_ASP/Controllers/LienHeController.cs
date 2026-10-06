using System.Web.Mvc;
using A3_MVC_ASP.Models;

namespace A3_MVC_ASP.Controllers
{
    public class LienHeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Liên hệ — Gửi phản hồi";
            return View(new LienHeGuiVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Gui(LienHeGuiVm model)
        {
            ViewBag.Title = "Liên hệ — Gửi phản hồi";
            if (!ModelState.IsValid)
                return View("Index", model);

            using (var db = new DoAnVatA3Context())
            {
                db.LienHes.Add(new LienHe
                {
                    HoTen = model.HoTen.Trim(),
                    Email = model.Email.Trim(),
                    SoDienThoai = model.SoDienThoai,
                    TieuDe = model.TieuDe,
                    NoiDung = model.NoiDung,
                    DaDoc = false,
                    NgayGui = System.DateTime.Now
                });
                db.SaveChanges();
            }

            TempData["Msg"] = "Cảm ơn bạn! Chúng tôi đã nhận phản hồi.";
            return RedirectToAction("Index");
        }
    }
}
