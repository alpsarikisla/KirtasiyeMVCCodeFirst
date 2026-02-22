using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YeniKitapKirtasiyeWebApp.Models;

namespace YeniKitapKirtasiyeWebApp.Areas.ManagerPanel.Controllers
{
    public class ProductController : Controller
    {
        YeniKitapKirtasiyeDBModel db = new YeniKitapKirtasiyeDBModel();
        public ActionResult Index()
        {
            List<Product> urunler = db.Products.Where(x=> x.IsDeleted == false).ToList();
            return View(urunler);
        }
        public ActionResult AllIndex()
        {
            List<Product> urunler = db.Products.ToList();
            return View(urunler);
        }
        [HttpGet]
        public ActionResult Create() 
        {

            //List<SelectListItem> dropdownitems = new List<SelectListItem>();
            //List<Category> categories = db.Categories.ToList();
            //dropdownitems.Add(new SelectListItem() { Value = "0", Text = "Seçiniz" });
            //foreach (var category in categories) 
            //{
            //    dropdownitems.Add(new SelectListItem() { Value = category.ID.ToString(), Text = category.Name });
            //}
            //ViewBag.dropdownitems = dropdownitems;
            //ViewBag.kategoriler = db.Categories;
            ViewBag.Category_ID = new SelectList(db.Categories, "ID", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product model, HttpPostedFileBase urunResim)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (urunResim != null)
                    {
                        FileInfo fi = new FileInfo(urunResim.FileName);
                        string uzanti = fi.Extension;
                        string isim = Guid.NewGuid().ToString();
                        string tamisim = isim + uzanti;
                        urunResim.SaveAs(Server.MapPath("~/Assets/ProductImages/"+tamisim));
                        model.ImagePath = tamisim;
                    }
                    else
                    {
                        model.ImagePath = "none.jpg";
                    }
                    db.Products.Add(model);
                    db.SaveChanges();
                    TempData["basarili"] = "Ürün ekleme işlemi başarılı";
                }
                catch
                {
                    TempData["basarisiz"] = "Ürün ekleme işlemi başarısız";
                }
            }
            ViewBag.Category_ID = new SelectList(db.Categories, "ID", "Name", model.Category_ID);
            return View(model);
        }
    }
}