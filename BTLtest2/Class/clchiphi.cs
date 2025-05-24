using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLtest2.Class
{
    internal class clchiphi
    {
        public string SoHDNhap { get; set; }
        public DateTime NgayNhap { get; set; }
        public string MaNCC { get; internal set; }
        public float TongTien { get; internal set; }
        public  clchiphi(string soHDNhap, DateTime ngayNhap, string maNCC, float tongTien)
        {
            SoHDNhap = soHDNhap;
            NgayNhap = ngayNhap;
            MaNCC = maNCC;
            TongTien = tongTien;
        }
        public clchiphi()
        {
            SoHDNhap = string.Empty;
            NgayNhap = DateTime.MinValue;
            MaNCC = string.Empty;
            TongTien = 0.0f;
        }

    }
}
