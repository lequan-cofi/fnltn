using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLtest2.Class
{
    internal class clqlhoadonban
    {
        public class HoaDonBan
        {
            public string SoHDBan { get; set; }
            public string MaNhanVien { get; set; }
            public DateTime NgayBan { get; set; }
            public string MaKhach { get; set; }
            public float TongTien { get; set; }
            public string TrangThai { get; set; } // Ví dụ: "Đã thanh toán", "Chưa thanh toán", "Đã hủy"

            // Thuộc tính tùy chọn để hiển thị trên UI
            public string TenNhanVien { get; set; }
            public string TenKhachHang { get; set; }
            public List<ChiTietHDBan> ChiTiet { get; set; } = new List<ChiTietHDBan>();
        }

        public class ChiTietHDBan
        {
            public string SoHDBan { get; set; }
            public string MaSach { get; set; }
            public int SLBan { get; set; }
            // Thuộc tính này vẫn tồn tại trong C# model để giữ giá trị tạm thời khi tính toán,
            // nhưng sẽ không được lưu vào cột riêng trong DB ChiTietHDBan nếu cột đó không tồn tại.
            public float DonGiaBanKhiBan { get; set; }
            public float KhuyenMai { get; set; } // Lưu dưới dạng thập phân (ví dụ: 0.1 cho 10%)
            public float ThanhTien { get; set; }

            // Thuộc tính tùy chọn để hiển thị
            public string TenSach { get; set; }
        }

        public class KhachHang // Giả sử bạn đã có class này
        {
            public string MaKhach { get; set; }
            public string TenKhach { get; set; }
            public string GioiTinh { get; set; }
            public string DiaChi { get; set; }
            public string DienThoai { get; set; }
        }

        // Giả sử bạn đã có các class NhanVien và KhoSach từ phần Hóa Đơn Nhập
        public class NhanVien
        {
            public string MaNhanVien { get; set; }
            public string TenNhanVien { get; set; }
            // Các thuộc tính khác nếu có
        }
        public class KhoSach
        {
            public string MaSach { get; set; }
            public string TenSach { get; set; }
            public int SoLuong { get; set; }
            public float DonGiaNhap { get; set; } // Giả sử có
            public float DonGiaBan { get; set; }
            // Các thuộc tính khác nếu có
        }
    }
}
