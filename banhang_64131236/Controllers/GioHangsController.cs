using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using banhang_64131236.Models;
using Microsoft.AspNet.Identity;

namespace banhang_64131236.Controllers
{
    public class GioHangsController : Controller
    {
        private project_webtaphoa_64131236Entities db = new project_webtaphoa_64131236Entities();

        // GET: GioHangs
        public ActionResult Index()
        {
            string maKH = Session["MaKH"]?.ToString();
            if (string.IsNullOrEmpty(maKH))
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if not authenticated
            }

            var gioHang = db.GioHangs
                .Include(g => g.ChiTietGioHangs.Select(ct => ct.HangHoa))
                .FirstOrDefault(g => g.MaKH == maKH);

            return View(gioHang);
        }

        // POST: Add to Cart
        [HttpPost]
        [ValidateAntiForgeryToken]

      
        public JsonResult AddToCart(string maHH, int soLuong)
        {
            try
            {
                string maKH = Session["MaKH"]?.ToString();
                if (string.IsNullOrEmpty(maKH))
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập để thêm vào giỏ hàng" });
                }

                var khachHang = db.KhachHangs.Find(maKH);
                if (khachHang == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy thông tin khách hàng" });
                }

                var hangHoa = db.HangHoas.Find(maHH);
                if (hangHoa == null)
                {
                    return Json(new { success = false, message = "Sản phẩm không tồn tại" });
                }

                if (hangHoa.SoLuongHangTon < soLuong)
                {
                    return Json(new { success = false, message = "Số lượng hàng trong kho không đủ" });
                }

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Tìm hoặc tạo giỏ hàng mới
                        var gioHang = db.GioHangs.FirstOrDefault(g => g.MaKH == maKH);
                        if (gioHang == null)
                        {
                            gioHang = new GioHang
                            {
                                // Tìm MaGioHang lớn nhất và cộng thêm 1
                                MaGioHang = (db.GioHangs.Any() ? db.GioHangs.Max(x => x.MaGioHang) : 0) + 1,
                                MaKH = maKH,
                                NgayTao = DateTime.Now
                            };
                            db.GioHangs.Add(gioHang);
                            db.SaveChanges();
                        }

                        var chiTietGioHang = db.ChiTietGioHangs
                            .FirstOrDefault(ct => ct.MaGioHang == gioHang.MaGioHang && ct.MaHH == maHH);

                        if (chiTietGioHang != null)
                        {
                            chiTietGioHang.SoLuong += soLuong;
                        }
                        else
                        {
                            chiTietGioHang = new ChiTietGioHang
                            {
                                MaGioHang = gioHang.MaGioHang,
                                MaHH = maHH,
                                SoLuong = soLuong
                            };
                            db.ChiTietGioHangs.Add(chiTietGioHang);
                        }

                        db.SaveChanges();
                        transaction.Commit();

                        return Json(new { success = true, message = "Đã thêm vào giỏ hàng thành công" });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the inner exception details
                var innerMsg = ex.InnerException?.Message ?? "No inner exception";
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}, Inner: {innerMsg}");
                return Json(new { success = false, message = "Lỗi khi thêm vào giỏ hàng: " + innerMsg });
            }
        }
        // POST: Update Cart Item Quantity
        [HttpPost]
        public JsonResult UpdateQuantity(string maHH, int soLuong)
        {
            try
            {
                string maKH = Session["MaKH"]?.ToString();
                if (string.IsNullOrEmpty(maKH))
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập" });
                }

                var gioHang = db.GioHangs.FirstOrDefault(g => g.MaKH == maKH);
                if (gioHang == null)
                {
                    return Json(new { success = false, message = "Giỏ hàng không tồn tại" });
                }

                var chiTietGioHang = db.ChiTietGioHangs
                    .FirstOrDefault(ct => ct.MaGioHang == gioHang.MaGioHang && ct.MaHH == maHH);

                if (chiTietGioHang == null)
                {
                    return Json(new { success = false, message = "Sản phẩm không có trong giỏ hàng" });
                }

                var hangHoa = db.HangHoas.Find(maHH);
                if (hangHoa.SoLuongHangTon < soLuong)
                {
                    return Json(new { success = false, message = "Số lượng hàng trong kho không đủ" });
                }

                chiTietGioHang.SoLuong = soLuong;
                db.SaveChanges();

                return Json(new
                {
                    success = true,
                    message = "Cập nhật số lượng thành công",
                    newTotal = chiTietGioHang.SoLuong * hangHoa.GiaBan
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        // POST: Remove Item from Cart
        [HttpPost]
        public JsonResult RemoveItem(string maHH)
        {
            try
            {
                string maKH = Session["MaKH"]?.ToString();
                if (string.IsNullOrEmpty(maKH))
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập" });
                }

                var gioHang = db.GioHangs.FirstOrDefault(g => g.MaKH == maKH);
                if (gioHang == null)
                {
                    return Json(new { success = false, message = "Giỏ hàng không tồn tại" });
                }

                var chiTietGioHang = db.ChiTietGioHangs
                    .FirstOrDefault(ct => ct.MaGioHang == gioHang.MaGioHang && ct.MaHH == maHH);

                if (chiTietGioHang != null)
                {
                    db.ChiTietGioHangs.Remove(chiTietGioHang);
                    db.SaveChanges();
                }

                return Json(new { success = true, message = "Đã xóa sản phẩm khỏi giỏ hàng" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        // GET: Cart Count
        // In GioHangsController.cs
        public JsonResult GetCartCount()
        {
            string maKH = Session["MaKH"]?.ToString();
            if (string.IsNullOrEmpty(maKH))
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            var cartCount = db.GioHangs
                .Where(g => g.MaKH == maKH)
                .Join(db.ChiTietGioHangs,
                    g => g.MaGioHang,
                    ct => ct.MaGioHang,
                    (g, ct) => ct.SoLuong)
                .Sum();

            return Json(cartCount, JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            string maKH = Session["MaKH"]?.ToString();

            if (!string.IsNullOrEmpty(maKH))
            {
                var cartCount = db.GioHangs
                    .Where(g => g.MaKH == maKH)
                    .Join(db.ChiTietGioHangs,
                        g => g.MaGioHang,
                        ct => ct.MaGioHang,
                        (g, ct) => ct.SoLuong)
                    .Sum();

                ViewBag.CartCount = cartCount;
            }
            else
            {
                ViewBag.CartCount = 0;
            }
        }
    }
}
