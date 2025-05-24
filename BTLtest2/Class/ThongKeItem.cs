using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLtest2.Class
{
    internal class ThongKeItem
    {
        public string MaSach { get; set; }
        public float DoanhThu { get; set; }
        public float ChiPhi { get; set; }
        public float LoiNhuan => DoanhThu - ChiPhi;
        public ThongKeItem(string maSach, float doanhThu, float chiPhi)
        {
            MaSach = maSach;
            DoanhThu = doanhThu;
            ChiPhi = chiPhi;
        }
        public ThongKeItem()
        {
            MaSach = string.Empty;
            DoanhThu = 0.0f;
            ChiPhi = 0.0f;
        }
        public class InventoryDataPoint
        {
            public string TenSach { get; set; } // Tên sách
            public int LuongTon { get; set; }    // Số lượng tồn cuối kỳ
        }
    }
}
