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
        [HttpGet]
        public ActionResult Edit(int? id) 
        {
            if (id == null)
            {
                return RedirectToAction("Index","Product");
            }
            Product p = db.Products.Find(id);
            if (p == null)
            {
                return RedirectToAction("Index", "Product");
            }
            ViewBag.Category_ID = new SelectList(db.Categories, "ID", "Name", p.Category_ID);
            return View(p);
        }
        [HttpPost]
        public ActionResult Edit(Product model, HttpPostedFileBase urunResim)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    if (urunResim != null)
                    {
                        FileInfo fi = new FileInfo(urunResim.FileName);
                        string uzanti = fi.Extension;
                        string isim = Guid.NewGuid().ToString();
                        string tamisim = isim + uzanti;
                        urunResim.SaveAs(Server.MapPath("~/Assets/ProductImages/" + tamisim));
                        model.ImagePath = tamisim;
                    }
                    TempData["basarili"] = "Ürün güncelleme işlemi başarılı";
                    db.SaveChanges();
                }
                catch { TempData["basarisiz"] = "Ürün güncelleme işlemi başarısız"; }
            }
            ViewBag.Category_ID = new SelectList(db.Categories, "ID", "Name", model.Category_ID);
            return View(model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            Product product = db.Products.Find(id);
            product.IsDeleted = true;
            product.IsActive = false;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult BackUp(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            Product product = db.Products.Find(id);
            product.IsDeleted = false;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}