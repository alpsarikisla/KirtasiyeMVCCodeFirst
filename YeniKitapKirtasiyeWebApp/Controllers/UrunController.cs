using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using YeniKitapKirtasiyeWebApp.Models;
using Newtonsoft.Json;
using System.Linq;
using YeniKitapKirtasiyeWebApp.Models.ViewModels;
using System.Net.Http;


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
                    Items.First(x => x.ID == id).Quantity += 1;
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
        public ActionResult Arttir(int id)
        {
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
                    Items.First(x => x.ID == id).Quantity += 1;
                }

                var settings = new JsonSerializerSettings { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii };
                string jsoncart = JsonConvert.SerializeObject(Items, Formatting.None, settings);
                kurabiye.Value = jsoncart;
                kurabiye.Expires = DateTime.Now.AddDays(30);
                Response.Cookies.Add(kurabiye);

            }
            return RedirectToAction("Sepet", "Urun");
        }
        public ActionResult Azalt(int id)
        {
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
                    CartItem ci = Items.First(x => x.ID == id);
                    if (ci.Quantity == 1)
                    {
                        Items.Remove(ci);
                    }
                    else
                    {
                        Items.First(x => x.ID == id).Quantity -= 1;
                    }
                }

                var settings = new JsonSerializerSettings { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii };
                string jsoncart = JsonConvert.SerializeObject(Items, Formatting.None, settings);
                kurabiye.Value = jsoncart;
                kurabiye.Expires = DateTime.Now.AddDays(30);
                Response.Cookies.Add(kurabiye);
            }
            return RedirectToAction("Sepet", "Urun");
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
        public ActionResult Ode()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Ode(OdeViewModel model)
        {
            if (ModelState.IsValid)
            {
                string[] tarih = model.expiry.Split('/');
                List<CartItem> Items = new List<CartItem>();
                if (Request.Cookies["sepet"] != null)
                {
                    HttpCookie kurabiye = Request.Cookies["sepet"];
                    Items = JsonConvert.DeserializeObject<List<CartItem>>(kurabiye.Value);
                }

                decimal price = Items.Sum(x => x.Price * x.Quantity);
                string fiyatstr = price.ToString().Replace(",", ".");
                string apiurl = "https://localhost:44349/API/PayAPI?merchandID=1597537894&merchandPassword=12341234&price="+ fiyatstr + "&CardNumber=" + model.cardnumber + "&Cvv=" + model.cvv + "&month=" + tarih[0] +"&year="+tarih[1];
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(apiurl).Result;
                var strinResp = response.Content.ReadAsStringAsync();
               
                if (strinResp.Result == "\"900\"" || strinResp.Result == "\"901\"" || strinResp.Result == "\"800\"" || strinResp.Result == "\"801\"" || strinResp.Result == "\"902\"" || strinResp.Result == "\"903\"")
                {
                    ViewBag.durum = "Ödeme sisteminde geçici bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
                }
                else if(strinResp.Result == "\"600\"")
                {
                    ViewBag.durum = "Kart numarası hatalı";
                }
                else if (strinResp.Result == "\"625\"")
                {
                    ViewBag.durum = "Son kullanma tarihi hatalı";
                }
                else if (strinResp.Result == "\"602\"")
                {
                    ViewBag.durum = "CVV Hatalı";
                }
                else if (strinResp.Result == "\"603\"")
                {
                    ViewBag.durum = "Tutar Hatası";
                }
                else if (strinResp.Result == "\"313\"")
                {
                    ViewBag.durum = "Kart bakiyesi yetersiz";
                }
                else if (strinResp.Result == "\"101\"")
                {
                    //Dikkat sepeti boşaltmadan önce OrderDetails gibi bir tabloya kaydetmeyi unutmayın
                    Response.Cookies.Remove("sepet");
                    HttpCookie kurabiye = new HttpCookie("sepet");
                    kurabiye.Value = null;
                    kurabiye.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(kurabiye);
                    return RedirectToAction("PaymentSuccess");
                }
            }
            else
            {
                ViewBag.durum = "Lütfen işaretli alanları doldurun";
            }
            
            return View(model);
        }
        [HttpGet]
        public ActionResult PaymentSuccess()
        {
            return View();
        }
    }
}