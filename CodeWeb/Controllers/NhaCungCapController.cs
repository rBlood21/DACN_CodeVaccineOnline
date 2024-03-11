using CodeWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeWeb.Controllers
{
    public class NhaCungCapController : Controller
    {
        // GET: NhaCungCap
        VaccineDataContext db = new VaccineDataContext();
        public ActionResult ShowNhaCC()
        {
            List<NhaCC> nhaCCs = db.NhaCCs.ToList();
            return View(nhaCCs);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(NhaCC emp)
        {
            db.NhaCCs.InsertOnSubmit(emp);
            db.SubmitChanges();

            ViewBag.SuccessMessage = "Thêm nhà cung cấp thành công!";

            return RedirectToAction("ShowNhaCC");
        }

        public ActionResult Details(int id)
        {
            using (VaccineDataContext db = new VaccineDataContext())
            {
                return View(db.NhaCCs.Where(x => x.MaNCC == id).FirstOrDefault());
            }

        }

        public ActionResult Edit(int id)
        {

            var databyid = db.NhaCCs.Single(x => x.MaNCC == id);

            return View(databyid);
        }
        [HttpPost]
        public ActionResult Edit(int id, NhaCC emp)
        {
            try
            {
                using (VaccineDataContext db = new VaccineDataContext())
                {
                    NhaCC nvs = db.NhaCCs.Single(x => x.MaNCC == id);
                    nvs.TenNCC = emp.TenNCC;
                    nvs.DiaChiNCC = emp.DiaChiNCC;
                    nvs.SoDienThoaiNCC = emp.SoDienThoaiNCC;

                    db.SubmitChanges();

                }
                return RedirectToAction("ShowNhaCC");
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
                    NhaCC emp = db.NhaCCs.Where(x => x.MaNCC == id).FirstOrDefault();
                    db.NhaCCs.DeleteOnSubmit(emp);
                    db.SubmitChanges();

                }
                return RedirectToAction("ShowNhaCC");
            }

            catch
            {
                return View();
            }
        }

    }
}