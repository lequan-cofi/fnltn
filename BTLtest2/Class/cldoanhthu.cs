using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLtest2.Class
{
    internal class cldoanhthu
    {
        public string SoHDBan { get; set; }
        public DateTime NgayBan { get; set; }
        public string MaKH { get; set; }
        public float TongTien { get; set; }
        public cldoanhthu(string soHDBan, DateTime ngayBan, string maKH, float tongTien)
        {
            SoHDBan = soHDBan;
            NgayBan = ngayBan;
            MaKH = maKH;
            TongTien = tongTien;
        }
        public cldoanhthu()
        {
            SoHDBan = string.Empty;
            NgayBan = DateTime.MinValue;
            MaKH = string.Empty;
            TongTien = 0.0f;
        }
    }
}
