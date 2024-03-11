using CodeWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace CodeWeb.Controllers
{
    [HandleError]
    public class CartController : Controller
    {
        VaccineDataContext db = new VaccineDataContext();
        // GET: Cart
        public List<GioHang> LaygioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }

        //them vao gio hang
        public ActionResult ThemGioHang(int ma, int sl, string strURL)
        {
            KhachHang kh = Session["KH"] as KhachHang;
            if (kh == null)
            {
                return RedirectToAction("DangNhap", "Auth", new { strURL = strURL });
            }

            //lay ra gio hang
            var lstGioHang = db.GioHangs.Where(x => x.MaKH == kh.MaKH).ToList();
            //kiem tra vc nay da co trong session "Gio Hang" hay chua ?
            GioHang vc = lstGioHang.SingleOrDefault(t => t.MaVC == ma);
            if (vc == null)// chua co trong gio hang
            {
                vc = new GioHang();
                vc.MaKH = kh.MaKH;
                vc.MaVC = ma;
                vc.SoLuong = sl;
                db.GioHangs.InsertOnSubmit(vc);
                db.SubmitChanges();
                return Redirect(strURL);
            }
            else
            {
                vc.SoLuong++;
                db.SubmitChanges();
                return Redirect(strURL);
            }
        }
        private int TongSoLuong()
        {
            KhachHang kh = Session["KH"] as KhachHang;
            if (kh == null)
            {
                return 0;
            }
            var lstGioHang = db.GioHangs.Where(x => x.MaKH == kh.MaKH).ToList();
            if (lstGioHang != null)
            {
                return (int)lstGioHang.Sum(x => x.SoLuong);
            }

            return 0;
        }
        private int TongThanhTien()
        {
            int ttt = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                ttt += lstGioHang.Sum(t => (int)t.TongThanhTien);
            }
            return ttt;
        }
        public ActionResult GioHang(string strURL)
        {
            KhachHang kh = Session["KH"] as KhachHang;
            if (kh == null)
            {
                return RedirectToAction("DangNhap", "Auth", new { strURL = strURL });
            }
            var lstGioHang = db.GioHangs.Where(x => x.MaKH == kh.MaKH).ToList();
            var lstSach = db.Vaccines.ToList();
            ViewBag.lstGioHang = lstGioHang;
            ViewBag.lstSach = lstSach;
            return View();
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            return PartialView();
        }

        public ActionResult XoaGioHang(int MaSP, int idkh)
        {

            GioHang s = db.GioHangs.SingleOrDefault(t => t.MaVC == MaSP && t.MaKH == idkh);
            if (s != null)
            {
                db.GioHangs.DeleteOnSubmit(s);
                db.SubmitChanges();
                Session["DeleteCart"] = "Xóa thành công.";
                Session["ResultDelete"] = "t";
            }
            else
            {
                Session["DeleteCart"] = "Không tìm thấy!";
                Session["ResultDelete"] = "f";
            }
            return RedirectToAction("GioHang", "Cart");
        }
        public ActionResult XoaAll(int idkh)
        {
            db.GioHangs.DeleteAllOnSubmit(db.GioHangs.Where(x => x.MaKH == idkh));
            db.SubmitChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CapNhatGioHang(int MaSP, int idkh, FormCollection f)
        {
            int amount;
            bool kt = int.TryParse(f["amount"], out amount);
            if (kt)
            {
                GioHang s = db.GioHangs.SingleOrDefault(x => x.MaVC == MaSP && x.MaKH == idkh);
                if (s != null)
                {
                    s.SoLuong = amount;
                    db.SubmitChanges();
                    Session["UpdateCart"] = "Cập nhật thành công.";
                    Session["ResultUpdate"] = "t";
                }
                else
                {
                    Session["UpdateCart"] = "Không tìm thấy!";
                    Session["ResultUpdate"] = "f";
                }
            }
            else
            {
                Session["UpdateCart"] = "Cập nhật thất bại!";
                Session["ResultUpdate"] = "f";
            }
            return RedirectToAction("GioHang", "Cart");
        }

        public ActionResult DangkyTiem(int idKH)
        {
            List<GioHang> carts = Session["ToPay"] as List<GioHang>;
            Session["PayList"] = carts;
            Session["Vaccines"] = db.Vaccines.ToList();

            ViewBag.ListQH = db.MoiQuanHes.ToList();

            return View();
        }
        [HttpPost]
        public ActionResult DangkyTiem(FormCollection fc, NguoiTiemChung ntc)
        {
            int soluong = TongSoLuong();
            int tongTien = TongThanhTien();
            string total = fc["total"];
            double totalAmount = Convert.ToDouble(total);
            string addressDetails = fc["address"];
            string name, phone;
            var giotinh = fc["GioiTinhKH"];
            var ngaysinh = fc["Ngaysinh"];

            var ngaytiem = fc["Ngaytiem"];

            name = fc["name"];
            phone = fc["phone"];

            List<GioHang> GHs = Session["PayList"] as List<GioHang>;


            int idBill = 0;
            int idKH = GHs.FirstOrDefault().MaKH;

            ntc.HoTenNTC = name;
            ntc.GioiTinhNTC = giotinh;
            ntc.NgaySinhNTC = DateTime.Parse(ngaysinh);
            ntc.SoDienThoaiNTC = phone;
            ntc.DiaChiNTC = addressDetails;
            ntc.MaKH = idKH;


            db.NguoiTiemChungs.InsertOnSubmit(ntc);
            db.SubmitChanges();

            foreach (var item in GHs)
            {
                if (idBill == 0)
                {
                    try
                    {
                        HoaDon Donhang = new HoaDon();
                        Donhang.NgayLap = DateTime.Now;
                        Donhang.ThoiGianTiem = DateTime.Parse(ngaytiem);
                        Donhang.TongSL = soluong;
                        Donhang.MaNTC = ntc.MaNTC;
                        if (totalAmount != 0)
                        {
                            Donhang.TongTien = (int)totalAmount;
                        }

                        string paymentMethod = fc["paymentMethod"];
                        Donhang.TrangThaiHD = paymentMethod == "virtualMethod2" ? "Đã thanh toán" : "Đang xác nhận";
                        db.HoaDons.InsertOnSubmit(Donhang);
                        db.SubmitChanges();

                        idBill = (int)Donhang.MaNTC;
                    }
                    catch
                    {
                        idBill = 0;
                    }
                }
                if (idBill > 0)
                {
                    try
                    {
                        GioHang gioHangItem = db.GioHangs.SingleOrDefault(x => x.MaVC == item.MaVC && x.MaKH == idKH);

                        if (gioHangItem != null)
                        {
                            ChiTietHoaDon ct = new ChiTietHoaDon();
                            ct.MaHD = idBill;
                            ct.MaVC = item.MaVC;
                            ct.SoLuong = item.SoLuong;

                            Vaccine vaccine = db.Vaccines.SingleOrDefault(v => v.MaVC == item.MaVC);

                            if (vaccine != null)
                            {
   
                                ct.ThanhTien = vaccine.GiaBanVC * item.SoLuong;

                                db.ChiTietHoaDons.InsertOnSubmit(ct);
                                db.SubmitChanges();

                                // Xóa GioHang
                                db.GioHangs.DeleteOnSubmit(gioHangItem);
                                db.SubmitChanges();


                                vaccine.SoLuongVC -= item.SoLuong;
                                db.SubmitChanges();
                            }
                            else
                            {
                                idBill = 0; 
                            }
                        }
                        else
                        {
                            idBill = 0;
                        }
                    }
                    catch
                    {
                        idBill = 0;
                    }
                }




            }
            return RedirectToAction("Index", "Home");
        }


        public ActionResult PaymentList(int idKH)
        {
            List<GioHang> carts = db.GioHangs.Where(x => x.MaKH == idKH).ToList();
            int countRemove = carts.RemoveAll(x => x.Vaccine.SoLuongVC < x.SoLuong);
            if (carts.Count <= 0)
                return RedirectToAction("Index", "Home");
            Session["ToPay"] = carts;
            if (countRemove > 0)
                Session["Remove"] = countRemove;
            return RedirectToAction("DangkyTiem", "Cart", new { idKH = idKH });
        }

        public ActionResult DanhSachDonHang(int idKH)
        {
            ViewBag.TB = null;

            List<HoaDon> donhangs =
                (from ntc in db.NguoiTiemChungs
                 join hd in db.HoaDons on ntc.MaNTC equals hd.MaNTC
                 where ntc.MaKH == idKH
                 orderby ntc.MaNTC
                 select hd).ToList();

            if (donhangs.Count <= 0)
            {
                ViewBag.TB = "Bạn chưa mua đơn nào!";
                return PartialView();
            }

            return PartialView(donhangs);
        }



    }
}