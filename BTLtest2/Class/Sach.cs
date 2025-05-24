using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLtest2.Class
{
    public class Sach
    {
        public string MaSach { get; set; }
        public string TenSach { get; set; }
        public int SoLuong { get; set; } // Số lượng tồn kho hiện tại
        public string DonGiaNhap { get; set; }
        public string DonGiaBan { get; set; }
        public string MaLoaiSach { get; set; }
        public string MaTacGia { get; set; } // Thêm các trường còn thiếu từ CSDL nếu cần cho hiển thị/logic
        public string MaNXB { get; set; }
        public string MaLinhVuc { get; set; }
        public string MaNgonNgu { get; set; }
        public string Anh { get; set; }
        public int SoTrang { get; set; }

        // Thuộc tính mới để lưu tổng lượng đã bán được tính toán
        public int LuongBanDaTinh { get; set; }

        public Sach()
        {
            LuongBanDaTinh = 0; // Khởi tạo giá trị mặc định
        }

        // Constructor có thể được cập nhật nếu dữ liệu này được lấy cùng lúc
        public Sach(string maSach, string tenSach, int soLuong, string donGiaBan, string donGiaNhap, string maLoaiSach, int luongBanDaTinh = 0)
        {
            MaSach = maSach;
            TenSach = tenSach;
            SoLuong = soLuong;
            DonGiaBan = donGiaBan;
            DonGiaNhap = donGiaNhap;
            MaLoaiSach = maLoaiSach; // Ví dụ thêm MaLoaiSach
            LuongBanDaTinh = luongBanDaTinh;
        }
        public class NgonNgu
        {
            public string MaNgonNgu { get; set; }
            public string TenNgonNgu { get; set; }
            public NgonNgu(string maNgonNgu, string tenNgonNgu)
            {
                MaNgonNgu = maNgonNgu;
                TenNgonNgu = tenNgonNgu;
            }
        }
    }
}
