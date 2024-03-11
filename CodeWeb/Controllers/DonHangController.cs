using CodeWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeWeb.Controllers
{
    public class DonHangController : Controller
    {
        // GET: DonHang
        VaccineDataContext db = new VaccineDataContext();
        public ActionResult ShowDonHang(int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;

            var hoaDons = db.HoaDons
                .OrderByDescending(h => h.NgayLap) 
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            int totalItems = db.HoaDons.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            ViewBag.TotalPages = totalPages;
            return View(hoaDons);
        }


        public ActionResult Details(int id)
        {
            HoaDon hoaDon = db.HoaDons.SingleOrDefault(x => x.MaHD == id);

            if (hoaDon == null)
            {
                return HttpNotFound();
            }

            NguoiTiemChung nguoiTiemChung = db.NguoiTiemChungs.SingleOrDefault(x => x.MaNTC == hoaDon.MaNTC);
            if (nguoiTiemChung == null)
            {
                return HttpNotFound();
            }

            List<ChiTietHoaDon> chiTietHoaDon = db.ChiTietHoaDons.Where(x => x.MaHD == id).ToList();
            var viewModel = new HoaDonViewModel
            {
                HoaDon = hoaDon,
                NguoiTiemChung = nguoiTiemChung,
                ChiTietHoaDon = chiTietHoaDon
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult UpdateStatus(int maHD)
        {
            try
            {
                var hoaDon = db.HoaDons.SingleOrDefault(x => x.MaHD == maHD);

                if (hoaDon != null)
                {
                    hoaDon.TrangThaiHD = "Đã thanh toán";
                    db.SubmitChanges();
                }

                return RedirectToAction("Details", new { id = maHD });
            }
            catch (Exception ex)
            {

                return RedirectToAction("Details", new { id = maHD });
            }
        }
    }
}