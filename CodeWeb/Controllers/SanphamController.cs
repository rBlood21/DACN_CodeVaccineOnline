using CodeWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeWeb.Controllers
{
    public class SanphamController : Controller
    {
        VaccineDataContext db = new VaccineDataContext();
        // GET: Sanpham
        public ActionResult ShowVaccine(int page = 1, string SortColumn = "MaHH", string IconClass = "fa-sort-asc")
        {
            List<Vaccine> vaccines = db.Vaccines.ToList();

            //sort
            ViewBag.SortColumn = SortColumn;
            ViewBag.IconClass = IconClass;
            if (SortColumn == "GiaBan")
            {
                if (IconClass == "fa-sort-asc")
                {
                    vaccines = vaccines.OrderBy(row => row.GiaBanVC).ToList();
                }
                else
                {
                    vaccines = vaccines.OrderByDescending(row => row.GiaBanVC).ToList();
                }
            }
            else if (SortColumn == "MaVaccin")
            {
                if (IconClass == "fa-sort-asc")
                {
                    vaccines = vaccines.OrderBy(row => row.MaVC).ToList();
                }
                else
                {
                    vaccines = vaccines.OrderByDescending(row => row.MaVC).ToList();
                }
            }

            //paging
            int NoOfRecordPerPage = 9;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(vaccines.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPage = NoOfPages;
            vaccines = vaccines.Skip(NoOfRecordSkip).Take(NoOfRecordPerPage).ToList();

            return View(vaccines);

        }
        public ActionResult Details(int id)
        {
            var Vaccine = db.Vaccines.SingleOrDefault(t => t.MaVC == id);
            if (Vaccine == null)
            {
                return HttpNotFound();
            }

            ViewBag.Title = "Sản phẩm";
            ViewBag.ProductName = Vaccine.TenVC;

            return View(Vaccine);
        }



        public ActionResult LoaiVaccine(int Maloai)
        {
            var list = db.Vaccines.Where(s => s.MaLoai == Maloai).ToList();

            if (list.Count == 0)
            {
                ViewBag.TB = "Khong tim thay";
            }
            else
            {
                string ten = db.LoaiVaccines.Where(s => s.MaLoai == Maloai).ToString();
                ViewBag.Loai = ten;
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult TimKiem(string tenVC)
        {
            var danhSachVaccine = db.Vaccines
                .Where(v => v.TenVC.Contains(tenVC))
                .ToList();

            return View("TimKiem", danhSachVaccine);
        }
    }
}