using System.Collections.Generic;
using System.Web.Mvc;
using A3_MVC_ASP.Models;

namespace A3_MVC_ASP.Controllers
{
    public class TinTucController : Controller
    {
        private static TinTucChiTietVm TaoBaiViet(int id)
        {
            switch (id)
            {
                case 1:
                    return new TinTucChiTietVm
                    {
                        MaBaiViet = 1,
                        NgayText = "09 Th7",
                        TieuDe = "Bạn có biết xúc xích ăn liền bao nhiêu calo không?",
                        DoanNoiDung = new List<string>
                        {
                            "Xúc xích ăn liền là món ăn vặt quen thuộc với nhiều người nhờ hương vị thơm ngon, tiện lợi và dễ sử dụng. Tuy nhiên, nhiều người vẫn thắc mắc xúc xích ăn liền chứa bao nhiêu calo và có nên ăn thường xuyên hay không.",
                            "Thông thường, một cây xúc xích ăn liền có trọng lượng từ 40g đến 60g sẽ chứa khoảng 120 đến 180 calo, tùy theo thành phần và thương hiệu sản xuất. Những loại xúc xích có thêm phô mai, thịt xông khói hoặc kích thước lớn sẽ có lượng calo cao hơn.",
                            "Xúc xích cung cấp năng lượng nhanh, phù hợp để dùng trong các bữa phụ hoặc khi cần bổ sung năng lượng tức thời. Tuy nhiên, sản phẩm này cũng thường chứa chất béo, muối và chất bảo quản, vì vậy không nên sử dụng quá nhiều trong thời gian dài.",
                            "Để ăn xúc xích lành mạnh hơn, bạn nên kết hợp cùng rau xanh, bánh mì hoặc salad và sử dụng với tần suất hợp lý. Bên cạnh đó, hãy lựa chọn các sản phẩm có nguồn gốc rõ ràng, còn hạn sử dụng và được bảo quản đúng cách.",
                            "Nếu bạn yêu thích xúc xích ăn liền, hãy sử dụng một cách cân đối để vừa ngon miệng vừa đảm bảo sức khỏe."
                        }
                    };
                case 2:
                    return new TinTucChiTietVm
                    {
                        MaBaiViet = 2,
                        NgayText = "12 Th7",
                        TieuDe = "Ăn trái cây sấy có tốt không?",
                        DoanNoiDung = new List<string>
                        {
                            "Trái cây sấy là món ăn vặt tiện lợi, giàu hương vị và được nhiều người ưa chuộng. Nếu sử dụng đúng cách, đây có thể là lựa chọn tốt cho bữa phụ trong ngày.",
                            "Bạn nên ưu tiên sản phẩm có hàm lượng đường thấp, ít phụ gia và ăn với khẩu phần vừa phải để tránh nạp quá nhiều năng lượng.",
                            "Kết hợp trái cây sấy cùng hạt dinh dưỡng hoặc sữa chua không đường sẽ giúp cân bằng dinh dưỡng tốt hơn."
                        }
                    };
                case 3:
                    return new TinTucChiTietVm
                    {
                        MaBaiViet = 3,
                        NgayText = "08 Th10",
                        TieuDe = "Cách làm khô heo cháy tỏi ngon chuẩn vị",
                        DoanNoiDung = new List<string>
                        {
                            "Khô heo cháy tỏi là món ăn vặt được yêu thích nhờ vị mặn ngọt hài hòa và mùi thơm đặc trưng. Bạn có thể tự làm tại nhà với các nguyên liệu dễ tìm.",
                            "Bí quyết quan trọng là ướp thịt đủ thời gian, sấy ở nhiệt độ phù hợp và đảo đều tay để sợi thịt tơi, không bị khô cứng.",
                            "Bảo quản trong hũ kín, để nơi khô ráo để dùng dần và giữ hương vị lâu hơn."
                        }
                    };
                case 4:
                    return new TinTucChiTietVm
                    {
                        MaBaiViet = 4,
                        NgayText = "08 Th10",
                        TieuDe = "Cách làm khô gà xé sợi tại nhà thơm ngon",
                        DoanNoiDung = new List<string>
                        {
                            "Khô gà xé sợi là món ăn dễ làm, phù hợp để ăn vặt hoặc ăn kèm cơm, bánh mì. Để ngon, bạn cần chọn phần ức gà tươi và nêm nếm vừa vị.",
                            "Trong quá trình sấy hoặc rang, nên giữ lửa nhỏ và đảo liên tục để sợi gà khô đều, không bị cháy cạnh.",
                            "Sau khi hoàn thành, để nguội hoàn toàn rồi cho vào hộp kín để đảm bảo độ giòn dai."
                        }
                    };
                default:
                    return null;
            }
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Tin tức";
            return View();
        }

        public ActionResult ChiTiet(int id)
        {
            var baiViet = TaoBaiViet(id);
            if (baiViet == null)
            {
                return HttpNotFound();
            }

            ViewBag.Title = baiViet.TieuDe;
            return View(baiViet);
        }
    }
}
