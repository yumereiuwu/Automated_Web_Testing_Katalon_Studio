using System.Collections.Generic;
using System.Web;
using A3_MVC_ASP.Models;

namespace A3_MVC_ASP.Helpers
{
    public static class GioHangSessionHelper
    {
        public const string SessionKey = "GioHang";

        public static List<CartLine> Doc(HttpSessionStateBase session)
        {
            var g = session[SessionKey] as List<CartLine>;
            return g ?? new List<CartLine>();
        }

        public static void Ghi(HttpSessionStateBase session, List<CartLine> gio)
        {
            session[SessionKey] = gio;
        }

        public static int TongSoLuong(HttpSessionStateBase session)
        {
            var g = Doc(session);
            var n = 0;
            foreach (var x in g) n += x.SoLuong;
            return n;
        }

        public static decimal TongTien(HttpSessionStateBase session)
        {
            var g = Doc(session);
            decimal t = 0;
            foreach (var x in g) t += x.ThanhTienHang;
            return t;
        }
    }
}
