using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLtest2.Class
{
    internal class KhachHang
    {
        // Thông tin cơ bản từ bảng dbo.KhachHang
        public string MaKhach { get; set; }
        public string TenKhach { get; set; }
        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }

        // Thông tin tổng hợp cho báo cáo khách hàng thân thiết
        public int SoLanMua { get; set; }
        public double TongTienHoaDon { get; set; } // Sửa từ TongHoaDon để rõ nghĩa hơn

        // Constructor cơ bản
        public KhachHang() { }

        // Constructor đầy đủ thông tin từ bảng KhachHang
        public KhachHang(string maKhach, string tenKhach, string gioiTinh, string diaChi, string dienThoai)
        {
            MaKhach = maKhach;
            TenKhach = tenKhach;
            GioiTinh = gioiTinh;
            DiaChi = diaChi;
            DienThoai = dienThoai;
        }

        // Constructor cho báo cáo khách hàng thân thiết (bao gồm các chỉ số)
        public KhachHang(string maKhach, string tenKhach, string dienThoai, int soLanMua, double tongTienHoaDon)
        {
            MaKhach = maKhach;
            TenKhach = tenKhach;
            DienThoai = dienThoai; // Có thể bạn chỉ cần một vài trường cơ bản cho báo cáo
            SoLanMua = soLanMua;
            TongTienHoaDon = tongTienHoaDon;
        }
    }
}
