using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyGiangDay.Models.EFC
{
    public class PhanCongModel
    {
        public IEnumerable<PhanCongGD> PhanCong { get; set; }
        public IEnumerable<PhanCongGD> TongHop { get; set; }
        public string NgayPC { get; set; }
        public string NgayXuatBC { get; set; }
    }

    public class PhanCongGD
    {
        public string MaLHMH { get; set; }
        public string MaMon { get; set; }
        public string MaLop { get; set; }
        public string MaMH { get; set; }
        public string MaGio { get; set; }
        public string MoTa { get; set; }
        public string NgayKT { get; set; }
        public string NgayBD { get; set; }
        public string NgoaiGio { get; set; }
        public string MaGV { get; set; }
        public string Tong { get; set; }
        public string TrongGio { get; set; }
    }
}