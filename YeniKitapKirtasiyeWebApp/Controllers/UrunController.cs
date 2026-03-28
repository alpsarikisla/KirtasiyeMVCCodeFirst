using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YeniKitapKirtasiyeWebApp.Models;

namespace YeniKitapKirtasiyeWebApp.Controllers
{
    public class UrunController : Controller
    {
        YeniKitapKirtasiyeDBModel db = new YeniKitapKirtasiyeDBModel();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SepeteEkle(int id)
        {
            Product urun = db.Products.Find(id);
            if (urun == null)
            {
                return RedirectToAction("UrunBulunamadi");
            }
            List<CartItem> Items = new List<CartItem>();

            if (Request.Cookies["sepet"] != null)
            {

            }
            else
            {
                HttpCookie kurabiye = new HttpCookie("sepet");
                
            }

            return View();
        }
    }
}