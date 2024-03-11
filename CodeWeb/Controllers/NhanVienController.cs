using CodeWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CodeWeb.Controllers
{
    public class NhanVienController : Controller
    {
        // GET: NhanVien
        VaccineDataContext db = new VaccineDataContext();
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                string hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                if (hashedPassword.Length > 100)
                {

                    hashedPassword = hashedPassword.Substring(0, 100);
                }

                return hashedPassword;
            }
        }
        public ActionResult ShowNhanVien()
        {

            List<NhanVien> nhanviens = db.NhanViens.ToList();
            return View(nhanviens);
        }

        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NhanVien emp)
        {
            if (db.TaiKhoans.Any(t => t.TenTK == emp.TenTK))
            {
                ModelState.AddModelError("TenTK", "Tên đăng nhập đã tồn tại!");
                return View(emp);
            }
            if (ModelState.IsValid)
            {
                TaiKhoan taiKhoan = new TaiKhoan
                {
                    TenTK = emp.TenTK,
                    MatKhau = HashPassword("123"),
                };

                db.TaiKhoans.InsertOnSubmit(taiKhoan);
                db.SubmitChanges();

                NhanVien newEmp = new NhanVien
                {
                    CCCD = emp.CCCD,
                    HoTenNV = emp.HoTenNV,
                    GioiTinhNV = emp.GioiTinhNV,
                    SoDienThoaiNV = emp.SoDienThoaiNV,
                    DiaChiNV = emp.DiaChiNV,
                    NgaySinhNV = emp.NgaySinhNV,
                    EmailNV = emp.EmailNV,
                    TenTK = emp.TenTK,
                };

                db.NhanViens.InsertOnSubmit(newEmp);
                db.SubmitChanges();

                return RedirectToAction("ShowNhanVien");
            }

            return View(emp);
        }

        public ActionResult Details(int id)
        {
            using (VaccineDataContext db = new VaccineDataContext())
            {
                return View(db.NhanViens.Where(x => x.MaNV == id).FirstOrDefault());
            }

        }

        public ActionResult Edit(int id)
        {
            var databyid = db.NhanViens.Single(x => x.MaNV == id);

            return View(databyid);
        }
        [HttpPost]
        public ActionResult Edit(int id, NhanVien emp)
        {
            
            try
            {
                using (VaccineDataContext db = new VaccineDataContext())
                {
                    NhanVien nvs = db.NhanViens.Single(x => x.MaNV == id);
                    nvs.CCCD = emp.CCCD;
                    nvs.HoTenNV = emp.HoTenNV;
                    nvs.GioiTinhNV = emp.GioiTinhNV;
                    nvs.SoDienThoaiNV = emp.SoDienThoaiNV;
                    nvs.DiaChiNV = emp.DiaChiNV;
                    nvs.NgaySinhNV = emp.NgaySinhNV;

                    nvs.EmailNV = emp.EmailNV;

                    nvs.TenTK = emp.TenTK;
                    db.SubmitChanges();

                }
                return RedirectToAction("ShowNhanVien");
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
                    NhanVien emp = db.NhanViens.Where(x => x.MaNV == id).FirstOrDefault();
                    db.NhanViens.DeleteOnSubmit(emp);
                    db.SubmitChanges();

                }
                return RedirectToAction("ShowNhanVien");
            }

            catch
            {
                return View();
            }
        }

    }
}