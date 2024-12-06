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
    public class ChiTietGioHangs_64131236Controller : Controller
    {
        private project_webtaphoa_64131236Entities db = new project_webtaphoa_64131236Entities();

        // GET: ChiTietGioHangs_64131236
        public ActionResult Index()
        {
            var chiTietGioHangs = db.ChiTietGioHangs.Include(c => c.GioHang).Include(c => c.HangHoa);
            return View(chiTietGioHangs.ToList());
        }

        // GET: ChiTietGioHangs_64131236/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietGioHang chiTietGioHang = db.ChiTietGioHangs.Find(id);
            if (chiTietGioHang == null)
            {
                return HttpNotFound();
            }
            return View(chiTietGioHang);
        }

        // GET: ChiTietGioHangs_64131236/Create
        public ActionResult Create()
        {
            ViewBag.MaGioHang = new SelectList(db.GioHangs, "MaGioHang", "MaKH");
            ViewBag.MaHH = new SelectList(db.HangHoas, "MaHH", "MaLH");
            return View();
        }

        // POST: ChiTietGioHangs_64131236/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaGioHang,MaHH,SoLuong")] ChiTietGioHang chiTietGioHang)
        {
            if (ModelState.IsValid)
            {
                db.ChiTietGioHangs.Add(chiTietGioHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaGioHang = new SelectList(db.GioHangs, "MaGioHang", "MaKH", chiTietGioHang.MaGioHang);
            ViewBag.MaHH = new SelectList(db.HangHoas, "MaHH", "MaLH", chiTietGioHang.MaHH);
            return View(chiTietGioHang);
        }

        // GET: ChiTietGioHangs_64131236/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietGioHang chiTietGioHang = db.ChiTietGioHangs.Find(id);
            if (chiTietGioHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaGioHang = new SelectList(db.GioHangs, "MaGioHang", "MaKH", chiTietGioHang.MaGioHang);
            ViewBag.MaHH = new SelectList(db.HangHoas, "MaHH", "MaLH", chiTietGioHang.MaHH);
            return View(chiTietGioHang);
        }

        // POST: ChiTietGioHangs_64131236/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaGioHang,MaHH,SoLuong")] ChiTietGioHang chiTietGioHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTietGioHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaGioHang = new SelectList(db.GioHangs, "MaGioHang", "MaKH", chiTietGioHang.MaGioHang);
            ViewBag.MaHH = new SelectList(db.HangHoas, "MaHH", "MaLH", chiTietGioHang.MaHH);
            return View(chiTietGioHang);
        }

        // GET: ChiTietGioHangs_64131236/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietGioHang chiTietGioHang = db.ChiTietGioHangs.Find(id);
            if (chiTietGioHang == null)
            {
                return HttpNotFound();
            }
            return View(chiTietGioHang);
        }

        // POST: ChiTietGioHangs_64131236/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChiTietGioHang chiTietGioHang = db.ChiTietGioHangs.Find(id);
            db.ChiTietGioHangs.Remove(chiTietGioHang);
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

    }
}
