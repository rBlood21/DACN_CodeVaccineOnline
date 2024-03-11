using CodeWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeWeb.Controllers
{
    public class HomeController : Controller
    {
        VaccineDataContext db = new VaccineDataContext();
        public ActionResult Index()
        {
            return View();
        }

        

        public ActionResult Loai()
        {
            var model = db.LoaiVaccines.OrderByDescending(p => p.TenLoai);
            return PartialView(model);
        }

        


        public ActionResult TrangTinTuc()
        {
            return View();
        }
        public ActionResult LiDoTiemVC()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Admin()
        {
            ViewBag.TongHD = db.HoaDons.Where(o => o.TrangThaiHD.Equals("Đã thanh toán")).Count();
            ViewBag.TongSLVC = db.HoaDons.Where(o => o.TrangThaiHD.Equals("Đã thanh toán")).Sum(o => o.TongSL);
            ViewBag.TongDoanhThu = db.HoaDons.Where(o => o.TrangThaiHD.Equals("Đã thanh toán")).Sum(o => o.TongTien);
            ViewBag.VaccineQuantity = db.Vaccines.Sum(v => v.SoLuongVC);
            ViewBag.TotalCustomers = db.KhachHangs.Count();
            ViewBag.NCC = db.NhaCCs.Count();
            ViewBag.Nhanvien = db.NhanViens.Count();
            return View();
        }
        public ActionResult QuanLyDoanhThuTatCa()
        {
            List<HoaDon> list = db.HoaDons.Where(o => o.TrangThaiHD.Equals("Đã thanh toán")).ToList();
            ViewBag.TongHD = db.HoaDons.Where(o => o.TrangThaiHD.Equals("Đã thanh toán")).Count();
            ViewBag.TongSLVC = db.HoaDons.Where(o => o.TrangThaiHD.Equals("Đã thanh toán")).Sum(o => o.TongSL);
            ViewBag.TongDoanhThu = db.HoaDons.Where(o => o.TrangThaiHD.Equals("Đã thanh toán")).Sum(o => o.TongTien);
            return View(list);
        }
        public ActionResult QuanLyVaccine()
        {
            return View();
        }
        public ActionResult QuanLyNhaCC()
        {
            return View();
        }
        public ActionResult QuanLyNhapKho()
        {
            return View();
        }
        public ActionResult QuanLyDonHang()
        {
            return View();
        }
        public ActionResult QuanLyKhachHang()
        {
            return View();
        }
        public ActionResult QuanLyNguoiTiemChung()
        {
            return View();
        }
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}