using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLtest2.Class
{
    internal class nhanvien_congviec
    {
    }
    public class NhanVien
    {
        public string MaNhanVien { get; set; }
        public string TenNhanVien { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string MaCv { get; set; } // Foreign Key to CongViec
        public string GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; } // Nullable DateTime for date of birth

        // Constructor
        public NhanVien() { }

        public NhanVien(string maNhanVien, string tenNhanVien, string diaChi, string dienThoai, string maCv, string gioiTinh, DateTime? ngaySinh)
        {
            MaNhanVien = maNhanVien;
            TenNhanVien = tenNhanVien;
            DiaChi = diaChi;
            DienThoai = dienThoai;
            MaCv = maCv;
            GioiTinh = gioiTinh;
            NgaySinh = ngaySinh;
        }
    }

    public class CongViec // Optional: Class for Job/Position, if you want to load job titles
    {
        public string MaCv { get; set; }
        public string TenCongViec { get; set; }

        public CongViec(string maCv, string tenCongViec)
        {
            MaCv = maCv;
            TenCongViec = tenCongViec;
        }

        // To display nicely in ComboBox
        public override string ToString()
        {
            return TenCongViec;
        }
    }
}
