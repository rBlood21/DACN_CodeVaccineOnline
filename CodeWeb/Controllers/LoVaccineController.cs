using CodeWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeWeb.Controllers
{
    public class LoVaccineController : Controller
    {
        // GET: LoVaccine
        VaccineDataContext db = new VaccineDataContext();
        public ActionResult ShowLoVCs()
        {
            var lovcs = db.LoVaccines.OrderByDescending(o=>o.HanSuDung).ToList();
            return View(lovcs);
        }
        public ActionResult Create()
        {
            ViewBag.MAVC = new SelectList(db.Vaccines, "MaVC", "TenVC");
            return View();
        }
        [HttpPost]
        public ActionResult Create(LoVaccine value)
        {
            db.LoVaccines.InsertOnSubmit(value);
            db.SubmitChanges();

            ViewBag.SuccessMessage = "Thêm lô vaccine thành công!";

            return RedirectToAction("ShowLoVCs");
        }
        public ActionResult Details(int id)
        {
            using (VaccineDataContext db = new VaccineDataContext())
            {
                var mavc = db.LoVaccines.Where(o => o.MaLoVC.Equals(id)).FirstOrDefault().MaVC;
                ViewBag.tenvc = db.Vaccines.Where(o => o.MaVC.Equals(mavc)).FirstOrDefault().TenVC;
                return View(db.LoVaccines.Where(x => x.MaLoVC == id).FirstOrDefault());
            }

        }
        public bool Check(int id)
        {
            var ListNK = db.NhapKhos.ToList();
            foreach(NhapKho nhapKho in ListNK)
            {
                if (nhapKho.MaLoVC.Equals(id))
                {
                    return false;
                    break;
                }
            }
            return true;
        }
        public ActionResult Delete(int id)
        {
            try
            {
                if (Check(id))
                {
                    using (VaccineDataContext db = new VaccineDataContext())
                    {
                        LoVaccine emp = db.LoVaccines.Where(x => x.MaLoVC == id).FirstOrDefault();
                        db.LoVaccines.DeleteOnSubmit(emp);
                        db.SubmitChanges();

                    }
                }
                return RedirectToAction("ShowLoVCs");
            }

            catch
            {
                return View();
            }
        }
    }
}