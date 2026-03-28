using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YeniKitapKirtasiyeWebApp.Models;

namespace YeniKitapKirtasiyeWebApp.Controllers
{
    public class KategoriController : Controller
    {
        YeniKitapKirtasiyeDBModel db = new YeniKitapKirtasiyeDBModel();

        // GET: Kategori
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _MenuOlustur()
        {
            return View(db.Categories.Where(x=> x.IsActive).ToList());
        }
    }
}