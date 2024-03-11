using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeWeb.Models
{
    public class NhapKhoDetailsViewModel
    {
        public NhapKho NhapKho { get; set; }
        public NhanVien NhanVien { get; set; }
        public NhaCC NhaCungCap { get; set; }
        public List<LoVaccine> LoVaccines { get; set; }
    }
}