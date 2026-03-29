using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using YeniKitapKirtasiyeWebApp.Models;
using Newtonsoft.Json;
using System.Linq;

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
            Product p = db.Products.Find(id);
            if (p == null)
            {
                RedirectToAction("UrunBulunamadi");
            }
            
            if (Request.Cookies["sepet"] != null)
            {
                HttpCookie kurabiye = Request.Cookies["sepet"];
                List<CartItem> Items = new List<CartItem>();
                if (kurabiye.Value != null)
                {
                   Items = JsonConvert.DeserializeObject<List<CartItem>>(kurabiye.Value);
                }
                int sayi = Items.Where(x => x.ID == id).Count();
                if (sayi > 0)
                {
                    Items.First(x=> x.ID == id).Quantity += 1;
                }
                else
                {
                    Items.Add(new CartItem() { ID = p.ID, Name = p.Name, Price = p.Price, Quantity = 1 });
                }
                var settings = new JsonSerializerSettings { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii };
                string jsoncart = JsonConvert.SerializeObject(Items, Formatting.None, settings);
                kurabiye.Value = jsoncart;
                kurabiye.Expires = DateTime.Now.AddDays(30);
                Response.Cookies.Add(kurabiye);
            }
            else
            {
                HttpCookie kurabiye = new HttpCookie("sepet");
                List<CartItem> Items = new List<CartItem>();
                Items.Add(new CartItem() { ID = p.ID, Name = p.Name, Price = p.Price, Quantity = 1 });
                var settings = new JsonSerializerSettings { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii };
                string jsoncart = JsonConvert.SerializeObject(Items, Formatting.None, settings);
                kurabiye.Value = jsoncart;
                kurabiye.Expires = DateTime.Now.AddDays(30);
                Response.Cookies.Add(kurabiye);
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult sepetiBosalt()
        {
            if (Request.Cookies["sepet"] != null)
            {
                Response.Cookies.Remove("sepet");
                HttpCookie kurabiye = new HttpCookie("sepet");
                kurabiye.Value = null;
                kurabiye.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(kurabiye);
            }
            return RedirectToAction("Sepet");
        }
        public ActionResult Sepet()
        {
            List<CartItem> Items = new List<CartItem>();
            if (Request.Cookies["sepet"] != null)
            {
                HttpCookie kurabiye = Request.Cookies["sepet"];
                Items = JsonConvert.DeserializeObject<List<CartItem>>(kurabiye.Value);
            }
            return View(Items);
        }
    }
}