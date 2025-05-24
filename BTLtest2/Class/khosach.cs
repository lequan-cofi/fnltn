using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLtest2.Class
{
    internal class khosach
    {
        public string MaSach { get; set; }
        public string TenSach { get; set; }
        public int SoLuong { get; set; }

        private decimal _donGiaNhap;
        public decimal DonGiaNhap
        {
            get { return _donGiaNhap; }
            set { _donGiaNhap = value; }
        }

        public decimal DonGiaBan
        {
            get { return _donGiaNhap * 1.10m; }
        }

        public string MaLoaiSach { get; set; }
        public string MaTacGia { get; set; }
        public string MaNXB { get; set; }
        public string MaLinhVuc { get; set; }
        public string MaNgonNgu { get; set; }
        public string Anh { get; set; }
        public int SoTrang { get; set; }
        public int LuongBanDaTinh { get; set; }

        public khosach()
        {
            LuongBanDaTinh = 0;
            _donGiaNhap = 0m;
            MaSach = string.Empty;
            TenSach = string.Empty;
            MaLoaiSach = string.Empty;
            MaTacGia = string.Empty;
            MaNXB = string.Empty;
            MaLinhVuc = string.Empty;
            MaNgonNgu = string.Empty;
            Anh = string.Empty;
        }

        public khosach(string maSach, string tenSach, int soLuong, decimal donGiaNhap, string maLoaiSach,
                    string maTacGia, string maNXB, string maLinhVuc, string maNgonNgu,
                    string anh, int soTrang, int luongBanDaTinh = 0)
        {
            MaSach = maSach;
            TenSach = tenSach;
            SoLuong = soLuong;
            this.DonGiaNhap = donGiaNhap;
            MaLoaiSach = maLoaiSach;
            MaTacGia = maTacGia;
            MaNXB = maNXB;
            MaLinhVuc = maLinhVuc;
            MaNgonNgu = maNgonNgu;
            Anh = anh;
            SoTrang = soTrang;
            LuongBanDaTinh = luongBanDaTinh;
        }
    }
}
