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
    public class NhanViens_64131236Controller : Controller
    {
        private project_webtaphoa_64131236Entities db = new project_webtaphoa_64131236Entities();

        // GET: NhanViens_64131236
        public ActionResult Index()
        {
            var nhanViens = db.NhanViens.Include(n => n.QuanTri);
            return View(nhanViens.ToList());
        }

        // GET: NhanViens_64131236/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // GET: NhanViens_64131236/Create
        public ActionResult Create()
        {
            ViewBag.username = new SelectList(db.QuanTris, "username", "Password");
            return View();
        }

        // POST: NhanViens_64131236/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNV,username,Ho,Ten,NgaySinh,NgayLamViec,DiaChi,DienThoai")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                db.NhanViens.Add(nhanVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.username = new SelectList(db.QuanTris, "username", "Password", nhanVien.username);
            return View(nhanVien);
        }

        // GET: NhanViens_64131236/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            ViewBag.username = new SelectList(db.QuanTris, "username", "Password", nhanVien.username);
            return View(nhanVien);
        }

        // POST: NhanViens_64131236/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNV,username,Ho,Ten,NgaySinh,NgayLamViec,DiaChi,DienThoai")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhanVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.username = new SelectList(db.QuanTris, "username", "Password", nhanVien.username);
            return View(nhanVien);
        }

        // GET: NhanViens_64131236/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: NhanViens_64131236/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NhanVien nhanVien = db.NhanViens.Find(id);
            db.NhanViens.Remove(nhanVien);
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
