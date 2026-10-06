using System.Web.Mvc;

namespace A3_MVC_ASP.Controllers
{
    public class ChinhSachController : Controller
    {
        public ActionResult GiaoHang()
        {
            ViewBag.Title = "Chính sách giao hàng";
            return View();
        }

        public ActionResult DoiTra()
        {
            ViewBag.Title = "Chính sách đổi trả";
            return View();
        }

        public ActionResult BaoMat()
        {
            ViewBag.Title = "Chính sách bảo mật";
            return View();
        }

        public ActionResult Faq()
        {
            ViewBag.Title = "Câu hỏi thường gặp";
            return View();
        }
    }
}
