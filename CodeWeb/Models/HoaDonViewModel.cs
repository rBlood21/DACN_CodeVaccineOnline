using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeWeb.Models
{
    public class HoaDonViewModel
    {
        public HoaDon HoaDon { get; set; }
        public NguoiTiemChung NguoiTiemChung { get; set; }
        public List<ChiTietHoaDon> ChiTietHoaDon { get; set; }
    }
}