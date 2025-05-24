using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLtest2.Class
{
    internal class qlhoadonnhap
    {
    }
    public class HoaDonNhap
    {
        public string SoHDNhap { get; set; } // Số hóa đơn nhập (Khóa chính)
        public string MaNhanVien { get; set; } // Mã nhân viên (Khóa ngoại)
        public DateTime NgayNhap { get; set; } // Ngày nhập hóa đơn
        public string MaNCC { get; set; } // Mã nhà cung cấp (Khóa ngoại)
        public float TongTien { get; set; } // Tổng tiền của hóa đơn

        // Tùy chọn: Để hiển thị dữ liệu liên quan trên UI
        public string TenNhanVien { get; set; } // Giả sử bạn sẽ lấy thông tin này
        public string TenNhaCungCap { get; set; } // Giả sử bạn sẽ lấy thông tin này
        public string DiaChiNCC { get; set; } // Địa chỉ nhà cung cấp
        public string DienThoaiNCC { get; set; } // Điện thoại nhà cung cấp
        public List<ChiTietHDNhap> ChiTiet { get; set; } = new List<ChiTietHDNhap>(); // Danh sách chi tiết hóa đơn
    }

    public class ChiTietHDNhap
    {
        public string SoHDNhap { get; set; } // Số hóa đơn nhập (Khóa chính, Khóa ngoại)
        public string MaSach { get; set; } // Mã sách (Khóa chính, Khóa ngoại)
        public int SLNhap { get; set; } // Số lượng nhập
        public float DonGiaNhap { get; set; } // Đơn giá nhập
        public float KhuyenMai { get; set; } // Khuyến mãi (giả sử là % hoặc số tiền cố định)
        public float ThanhTien { get; set; } // Thành tiền cho sản phẩm này

        // Tùy chọn: Để hiển thị dữ liệu liên quan trên UI
        public string TenSach { get; set; } // Giả sử bạn sẽ lấy thông tin này
    }

    public class NhaCungCaphdn
    {
        public string MaNCC { get; set; } // Mã nhà cung cấp (Khóa chính)
        public string TenNhaCungCap { get; set; } // Tên nhà cung cấp
        public string DiaChi { get; set; } // Địa chỉ
        public string DienThoai { get; set; } // Số điện thoại
        public string SoHDNhap { get; internal set; }
    }

    public class KhoSach
    {
        public string MaSach { get; set; } // Mã sách (Khóa chính)
        public string TenSach { get; set; } // Tên sách
        public int SoLuong { get; set; } // Số lượng tồn kho
        public float DonGiaNhap { get; set; } // Đơn giá nhập gần nhất
        public float DonGiaBan { get; set; } // Đơn giá bán
        public string MaLoaiSach { get; set; } // Mã loại sách (Khóa ngoại)
        public string MaTacGia { get; set; } // Mã tác giả (Khóa ngoại)
        public string MaNXB { get; set; } // Mã nhà xuất bản (Khóa ngoại)
        public string MaLinhVuc { get; set; } // Mã lĩnh vực (Khóa ngoại)
        public string MaNgonNgu { get; set; } // Mã ngôn ngữ (Khóa ngoại)
        public string Anh { get; set; } // Đường dẫn ảnh bìa sách
        public int SoTrang { get; set; } // Số trang sách
    }

    // Bạn cũng có thể muốn có một class cho NhanVien nếu cần quản lý/hiển thị chi tiết của họ
  
}
