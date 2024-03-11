using CodeWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace CodeWeb.Controllers
{
    [HandleError]
    public class AuthController : Controller
    {
        //GET: NguoiDung
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


        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(KhachHang kh, TaiKhoan ac, FormCollection f)
        {
            var hoten = f["HotenKH"];
            var tendn = f["TenDN"];
            var matkhau = f["MatKhau"];
            var rematkhau = f["ReMatkhau"];
            var dienthoai = f["Dienthoai"];
            var ngaysinh = f["Ngaysinh"];
            var diachi = f["diachi"];
            var email = f["email"];
            var giotinh = f["GioiTinhKH"];

            if (string.IsNullOrEmpty(hoten) ||
                string.IsNullOrEmpty(tendn) ||
                string.IsNullOrEmpty(matkhau) ||
                string.IsNullOrEmpty(rematkhau) ||
                string.IsNullOrEmpty(dienthoai) ||
                string.IsNullOrEmpty(ngaysinh) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(diachi))
            {
                ViewData["Loi"] = "Vui lòng điền đầy đủ thông tin!";
                return View();
            }
            if (string.IsNullOrEmpty(kh.GioiTinhKH) || !Regex.IsMatch(kh.GioiTinhKH, "^(Nam|Nữ)$", RegexOptions.IgnoreCase))
            {
                ViewData["Loi7"] = "Vui lòng điền giới tính hợp lệ!";
                return View();
            }

            if (matkhau != rematkhau)
            {
                ViewData["Loi"] = "Mật khẩu nhập lại không khớp!";
                return View();
            }

            // Kiểm tra độ dài số điện thoại 
            if (dienthoai.Length != 10)
            {
                ViewData["Loi"] = "Số điện thoại phải có 10 số!";
                return View();
            }

            // Kiểm tra sự tồn tại của tên đăng nhập
            if (db.TaiKhoans.Any(t => t.TenTK == tendn))
            {
                ViewData["Loi"] = "Tên đăng nhập đã tồn tại!";
                return View();
            }

            ac.TenTK = tendn;
            ac.MatKhau = HashPassword(matkhau);

          
            if (ac.MatKhau.Length > 100)
            {
                ac.MatKhau = ac.MatKhau.Substring(0, 100);
            }

            db.TaiKhoans.InsertOnSubmit(ac);
            db.SubmitChanges();


            kh.TenTK = ac.TenTK; 
            kh.HoTenKH = hoten;
            if (giotinh == null)
            {
                kh.GioiTinhKH = "Không xác định";
            }
            else
            {
                kh.GioiTinhKH = giotinh;
            }

            kh.NgaySinhKH = DateTime.Parse(ngaysinh);
            kh.SoDienThoaiKH = dienthoai;
            kh.DiaChiKH = diachi;
            kh.EmailKH = email;
            

            db.KhachHangs.InsertOnSubmit(kh);
            db.SubmitChanges();

            ViewBag.TB = "Đăng ký thành công!";
            return RedirectToAction("DangNhap", "Auth");
        }


        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            // Khai báo các biến nhận giá trị từ form f
            var tendn = f["TenDN"];
            var matkhau = f["MatKhau"];

            if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Tên đăng nhập không được bỏ trống!";
            }
            if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Mật khẩu không được bỏ trống!";
            }

            if (!string.IsNullOrEmpty(tendn) && !string.IsNullOrEmpty(matkhau))
            {
                TaiKhoan ac = db.TaiKhoans.SingleOrDefault(t => t.TenTK == tendn);

                if (ac != null)
                {
                    // Mã hóa mật khẩu nhập vào và so sánh với mật khẩu đã lưu trong cơ sở dữ liệu
                    string hashedPasswordInput = HashPassword(matkhau);

                    if (hashedPasswordInput == ac.MatKhau)
                    {
                        KhachHang kh = db.KhachHangs.SingleOrDefault(t => t.TenTK == tendn);
                        NhanVien nv = db.NhanViens.SingleOrDefault(t => t.TenTK == tendn);

                        if (nv != null && ac.TenTK == nv.TenTK)
                        {
                            ViewBag.TB = "Đăng nhập thành công!";
                            Session["NV"] = nv;
                            return RedirectToAction("Admin", "Home");
                        }

                        if (kh != null && ac.TenTK == kh.TenTK)
                        {
                            ViewBag.TB = "Đăng nhập thành công!";
                            Session["KH"] = kh;
                            return RedirectToAction("Index", "Home");
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewData["Loi5"] = "Mật khẩu sai, vui lòng nhập lại!";
                    }
                }
                else
                {
                    ViewData["Loi5"] = "Tên đăng nhập hoặc mật khẩu sai, vui lòng nhập lại!";
                }
            }


            return View();
        }


    }
}