using CodeWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeWeb.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        VaccineDataContext db = new VaccineDataContext();
        public ActionResult ShowKhachHang()
        {
            List<KhachHang> khachHangs = db.KhachHangs.ToList();
            return View(khachHangs);
        }

        public ActionResult Details(int id)
        {
            using (VaccineDataContext db = new VaccineDataContext())
            {
                return View(db.KhachHangs.Where(x => x.MaKH == id).FirstOrDefault());
            }

        }

        public ActionResult Edit(int id)
        {
            var databyid = db.KhachHangs.Single(x => x.MaKH == id);
            return View(databyid);
        }
        [HttpPost]
        public ActionResult Edit(int id, KhachHang emp)
        {
            
            try
            {
                using (VaccineDataContext db = new VaccineDataContext())
                {
                    KhachHang nvs = db.KhachHangs.Single(x => x.MaKH == id);
                    nvs.HoTenKH = emp.HoTenKH;
                    nvs.GioiTinhKH = emp.GioiTinhKH;
                    nvs.NgaySinhKH = emp.NgaySinhKH;
                    nvs.SoDienThoaiKH = emp.SoDienThoaiKH;
                    nvs.EmailKH = emp.EmailKH;
                    nvs.DiaChiKH = emp.DiaChiKH;
                    nvs.TenTK = emp.TenTK;
                    db.SubmitChanges();

                }
                return RedirectToAction("ShowKhachHang");
            }

            catch
            {
                return View();
            }

        }

        //public ActionResult Delete(int id)
        //{
        //    using (VaccineDataContext db = new VaccineDataContext())
        //    {
        //        return View(db.NhanViens.Where(x => x.MaNV == id).FirstOrDefault());
        //    }
        //}
        public ActionResult Delete(int id)
        {
            try
            {
                using (VaccineDataContext db = new VaccineDataContext())
                {
                    KhachHang emp = db.KhachHangs.Where(x => x.MaKH == id).FirstOrDefault();
                    db.KhachHangs.DeleteOnSubmit(emp);
                    db.SubmitChanges();

                }
                return RedirectToAction("ShowKhachHang");
            }

            catch
            {
                return View();
            }
        }
    }
}