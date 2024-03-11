using CodeWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeWeb.Controllers
{
    public class NhapKhoController : Controller
    {
        // GET: NhapKho
        VaccineDataContext db = new VaccineDataContext();
        public ActionResult ShowNhapKho()
        {
            List<NhapKho> nhapKhos = db.NhapKhos.OrderByDescending(o => o.ThoiGianNK).ToList();
            return View(nhapKhos);
        }
        public List<LoVaccine> ListLoVC()
        {
            List<LoVaccine> LoVCNK = new List<LoVaccine>();
            List<LoVaccine> listlovc = db.LoVaccines.ToList();
            List<NhapKho> nhapKhos = db.NhapKhos.OrderByDescending(o => o.ThoiGianNK).ToList();
            LoVCNK = new List<LoVaccine>();
            foreach (LoVaccine l in listlovc)
            {
                if (nhapKhos.Where(o => o.MaLoVC.Equals(l.MaLoVC)).Count() == 0)
                {
                    LoVCNK.Add(l);
                }
            }
            return LoVCNK;
        }
        [HttpGet]
        public ActionResult Create()
        {
            
            ViewBag.ListLoVCNK = ListLoVC();
            ViewBag.MANCC = new SelectList(db.NhaCCs, "MaNCC", "TenNCC");

            return View();
        }
        public int createMaNK()
        {
            List<NhapKho> nks = db.NhapKhos.ToList();
            
            int kq=0;
            for(int i=1;i<=1000000;i++)
            {
                
                if (db.NhapKhos.Where(o=>o.MaNK.Equals(i)).Count() == 0)
                {
                    kq = i; break;
                }
            }
            return kq;
        }
        [HttpPost]
        public ActionResult Create(NhapKho emp)
        {
            NhanVien nv = Session["NV"] as NhanVien;

            DateTime tg = DateTime.Now;
            int mank = createMaNK();
            foreach (LoVaccine lo in ListLoVC())
            {
                NhapKho nk = new NhapKho();
                nk.MaNK = mank;
                nk.MaNV = nv.MaNV;
                nk.MaNCC = emp.MaNCC;
                nk.ThoiGianNK = tg;
                nk.MaLoVC = lo.MaLoVC;
                updateSLVC(lo.MaVC, lo.SoLuongLoVC);
                db.NhapKhos.InsertOnSubmit(nk);
                db.SubmitChanges();
            }
            
            ViewBag.SuccessMessage = "Nhập kho thành công!";
            return RedirectToAction("ShowNhapKho");
            
        }
        public void updateSLVC(int? mavc, int? sl)
        {
            Vaccine vc = db.Vaccines.Single(o => o.MaVC.Equals(mavc));
            if (vc != null)
            {
                vc.SoLuongVC += sl;
                db.SubmitChanges();
            }

        }
        public ActionResult Details(int id)
        {
            using (VaccineDataContext db = new VaccineDataContext())
            {
                var query = from nk in db.NhapKhos
                            join nv in db.NhanViens on nk.MaNV equals nv.MaNV
                            join ncc in db.NhaCCs on nk.MaNCC equals ncc.MaNCC
                            join lo in db.LoVaccines on nk.MaLoVC equals lo.MaLoVC into loGroup
                            where nk.MaNK == id
                            select new NhapKhoDetailsViewModel
                            {
                                NhapKho = nk,
                                NhanVien = nv,
                                NhaCungCap = ncc,
                                LoVaccines = loGroup.ToList()
                            };

                var result = query.FirstOrDefault();

                if (result != null)
                {
                    return View(result);
                }
            }

            return HttpNotFound();
        }





        public ActionResult Delete(int id)
        {
            try
            {
                using (VaccineDataContext db = new VaccineDataContext())
                {
                    NhapKho emp = db.NhapKhos.Where(x => x.MaNK == id).FirstOrDefault();
                    db.NhapKhos.DeleteOnSubmit(emp);
                    db.SubmitChanges();

                }
                return RedirectToAction("ShowNhapKho");
            }

            catch
            {
                return View();
            }
        }
    }
}