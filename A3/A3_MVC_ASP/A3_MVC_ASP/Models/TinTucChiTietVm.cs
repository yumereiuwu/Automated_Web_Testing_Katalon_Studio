using System.Collections.Generic;

namespace A3_MVC_ASP.Models
{
    public class TinTucChiTietVm
    {
        public int MaBaiViet { get; set; }
        public string NgayText { get; set; }
        public string TieuDe { get; set; }
        public IList<string> DoanNoiDung { get; set; }
    }
}
