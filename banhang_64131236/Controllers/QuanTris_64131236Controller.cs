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
    public class QuanTris_64131236Controller : Controller
    {
        private project_webtaphoa_64131236Entities db = new project_webtaphoa_64131236Entities();

        // GET: QuanTris_64131236
        public ActionResult Index()
        {
            return View(db.QuanTris.ToList());
        }

        // GET: QuanTris_64131236/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuanTri quanTri = db.QuanTris.Find(id);
            if (quanTri == null)
            {
                return HttpNotFound();
            }
            return View(quanTri);
        }

        // GET: QuanTris_64131236/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuanTris_64131236/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "username,Password,Admin")] QuanTri quanTri)
        {
            if (ModelState.IsValid)
            {
                db.QuanTris.Add(quanTri);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(quanTri);
        }

        // GET: QuanTris_64131236/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuanTri quanTri = db.QuanTris.Find(id);
            if (quanTri == null)
            {
                return HttpNotFound();
            }
            return View(quanTri);
        }

        // POST: QuanTris_64131236/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "username,Password,Admin")] QuanTri quanTri)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quanTri).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(quanTri);
        }

        // GET: QuanTris_64131236/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuanTri quanTri = db.QuanTris.Find(id);
            if (quanTri == null)
            {
                return HttpNotFound();
            }
            return View(quanTri);
        }

        // POST: QuanTris_64131236/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            QuanTri quanTri = db.QuanTris.Find(id);
            db.QuanTris.Remove(quanTri);
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




        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(QuanTri qt)
        {
            var user = db.QuanTris.FirstOrDefault(u => u.username == qt.username && u.Password == qt.Password);
            if (user != null)
            {
                Session["username"] = user.username;
                Session["Admin"] = user.Admin;

                // Nếu không phải admin, lấy MaKH từ bảng KhachHang
                if (user.Admin != true)
                {
                    var khachHang = db.KhachHangs.FirstOrDefault(k => k.username == user.username);
                    if (khachHang != null)
                    {
                        Session["MaKH"] = khachHang.MaKH;
                    }
                }

                TempData["SuccessMessage"] = "Đăng nhập thành công!";

                if (user.Admin == true)
                {
                    return RedirectToAction("Index", "QuanTris_64131236");
                }
                else
                {
                    return RedirectToAction("Categories", "LoaiHangs");
                }
            }

            ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng";
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register([Bind(Include = "username,Password")] QuanTri qt)
        {
            if (ModelState.IsValid)
            {
                var exists = db.QuanTris.Any(x => x.username == qt.username);
                if (exists)
                {
                    ViewBag.Error = "Username already exists";
                    return View();
                }

                qt.Admin = false;
                db.QuanTris.Add(qt);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(qt);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        public ActionResult AccessDenied()
        {
            return View("~/Views/Shared/AccessDenied.cshtml");
        }

    }
}
