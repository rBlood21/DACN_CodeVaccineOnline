using CodeWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeWeb.Controllers
{
    public class VaccineController : Controller
    {
        // GET: Vaccine
        VaccineDataContext db = new VaccineDataContext();
        public ActionResult ShowVaccine(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            var vaccines = db.Vaccines.OrderBy(v => v.TenVC)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            int totalItems = db.Vaccines.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            ViewBag.TotalPages = totalPages;

            return View(vaccines);
        }
        public ActionResult Create()
        {
            ViewBag.MALOAI = new SelectList(db.LoaiVaccines, "Maloai", "TenLoai");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Vaccine emp, HttpPostedFileBase uploadFile)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra số lượng
                if (emp.SoLuongVC <= 0)
                {
                    ViewBag.Message = "Số lượng phải lớn hơn 0. Vui lòng nhập lại.";
                    return View(emp);
                }

                if (uploadFile != null && uploadFile.ContentLength > 0)
                {
                    var _FileName = Path.GetFileName(uploadFile.FileName);
                    var _path = Path.Combine(Server.MapPath("~/Images/Vaccine"), _FileName);

                    uploadFile.SaveAs(_path);
                    emp.HinhAnhVC = _FileName;
                }

                emp.TinhTrangVC = "Còn";
                db.Vaccines.InsertOnSubmit(emp);
                db.SubmitChanges();

                return RedirectToAction("ShowVaccine");
            }

            ViewBag.Message = "Không thành công!!";
            return View(emp);
        }


        public ActionResult Details(int id)
        {
            using (VaccineDataContext db = new VaccineDataContext())
            {
                return View(db.Vaccines.Where(x => x.MaVC == id).FirstOrDefault());
            }

        }
        //[HttpGet]
        public ActionResult Edit(int id)
        {

            var databyid = db.Vaccines.Single(x => x.MaVC == id);
            ViewBag.MALOAI = new SelectList(db.LoaiVaccines, "Maloai", "TenLoai");
            ViewBag.NhomVC = db.NhomVaccines.ToList();
            return View(databyid);
        }
        [HttpPost]
        public ActionResult Edit(int id, Vaccine emp, HttpPostedFileBase uploadFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (VaccineDataContext db = new VaccineDataContext())
                    {
                        Vaccine nvs = db.Vaccines.Single(x => x.MaVC == id);
                        nvs.TenVC = emp.TenVC;

                        if (uploadFile != null && uploadFile.ContentLength > 0)
                        {
                            var _FileName = Path.GetFileName(uploadFile.FileName);
                            var _path = Path.Combine(Server.MapPath("~/Images/Vaccine"), _FileName);

                            uploadFile.SaveAs(_path);
                            nvs.HinhAnhVC = _FileName; 
                        }

                        nvs.NguonGocVC = emp.NguonGocVC;
                        nvs.MoTaVC = emp.MoTaVC;

                        if (emp.SoLuongVC == 0)
                        {
                            nvs.TinhTrangVC = "Hết";
                        }
                        else
                        {
                            nvs.TinhTrangVC = "Còn";
                        }

                        nvs.GiaBanVC = emp.GiaBanVC;
                        nvs.GiaNhapVC = emp.GiaNhapVC;
                        nvs.SoLuongVC = emp.SoLuongVC;
                        nvs.MaLoai = emp.MaLoai;
                        nvs.MaNhom = emp.MaNhom;

                        db.SubmitChanges();
                        return RedirectToAction("ShowVaccine");
                    }
                }
                catch
                {
                    ViewBag.Message = "Không thành công!!";
                }
            }
            return RedirectToAction("ShowVaccine");
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (VaccineDataContext db = new VaccineDataContext())
                {
                    Vaccine emp = db.Vaccines.Where(x => x.MaVC == id).FirstOrDefault();
                    db.Vaccines.DeleteOnSubmit(emp);
                    db.SubmitChanges();

                }
                return RedirectToAction("ShowVaccine");
            }

            catch
            {
                return View();
            }
        }
    }
}