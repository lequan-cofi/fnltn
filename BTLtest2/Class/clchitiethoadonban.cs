using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLtest2.Class
{
    internal class clchitiethoadonban
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public int SoLuong { get; set; }
        public float DonGia { get; set; }
        public float KhuyenMai { get; set; }
        public float ThanhTien { get; set; }
        public clchitiethoadonban(string maSP, string tenSP, int soLuong, float donGia, float khuyenMai, float thanhTien)
        {
            MaSP = maSP;
            TenSP = tenSP;
            SoLuong = soLuong;
            DonGia = donGia;
            KhuyenMai = khuyenMai;
            ThanhTien = thanhTien;
        }
        public clchitiethoadonban()
        {
            MaSP = string.Empty;
            TenSP = string.Empty;
            SoLuong = 0;
            DonGia = 0.0f;
            KhuyenMai = 0.0f;
            ThanhTien = 0.0f;
        }
    }
}
