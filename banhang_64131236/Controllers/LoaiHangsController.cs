using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using banhang_64131236.Models;

namespace banhang_64131236.Controllers
{
    public class LoaiHangsController : Controller
    {
        private project_webtaphoa_64131236Entities db = new project_webtaphoa_64131236Entities();

        // GET: LoaiHangs
        public ActionResult Index()
        {
            return View(db.LoaiHangs.ToList());
        }

        // GET: LoaiHangs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiHang loaiHang = db.LoaiHangs.Find(id);
            if (loaiHang == null)
            {
                return HttpNotFound();
            }
            return View(loaiHang);
        }

        // GET: LoaiHangs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoaiHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLH,AnhLH,TenLH")] LoaiHang loaiHang)
        {
            if (ModelState.IsValid)
            {
                db.LoaiHangs.Add(loaiHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(loaiHang);
        }

        // GET: LoaiHangs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiHang loaiHang = db.LoaiHangs.Find(id);
            if (loaiHang == null)
            {
                return HttpNotFound();
            }
            return View(loaiHang);
        }

        // POST: LoaiHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLH,AnhLH,TenLH")] LoaiHang loaiHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loaiHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(loaiHang);
        }

        // GET: LoaiHangs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiHang loaiHang = db.LoaiHangs.Find(id);
            if (loaiHang == null)
            {
                return HttpNotFound();
            }
            return View(loaiHang);
        }

        // POST: LoaiHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LoaiHang loaiHang = db.LoaiHangs.Find(id);
            db.LoaiHangs.Remove(loaiHang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [CustomAuthorize]
        public ActionResult ShowProducts(string id)

        {

            if (Session["Admin"] != null && (bool)Session["Admin"])
            {
                TempData["Warning"] = "Đây là trang dành cho khách hàng!";
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var products = db.HangHoas.Where(p => p.MaLH == id).ToList();
            if (products == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryName = db.LoaiHangs.Find(id).TenLH;
            return View(products);
        }

        // Action để lấy menu danh mục (dùng làm partial view)

        [CustomAuthorize]
        public ActionResult CategoryMenu()
        {
            var categories = db.LoaiHangs.ToList();
            return PartialView("_CategoryMenu", categories);
        }

        // Action để hiển thị tất cả danh mục cho người dùng
        [CustomAuthorize]
        public ActionResult Categories()
        {
            if (Session["Admin"] != null && (bool)Session["Admin"])
            {
                TempData["Warning"] = "Đây là trang dành cho khách hàng!";
                return RedirectToAction("Index", "Home");
            }
            var categories = db.LoaiHangs.ToList();
            return View(categories);
        }
    }
}
