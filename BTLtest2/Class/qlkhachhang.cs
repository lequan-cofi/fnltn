using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLtest2.Class
{
    internal class qlkhachhang
    {
        // Các thuộc tính tương ứng với các cột trong bảng dbo.KhachHang
        public string MaKhach { get; set; }
        public string TenKhach { get; set; }
        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public qlkhachhang()
        {
        }

        /// <summary>
        /// Constructor có tham số để khởi tạo đối tượng KhachHang
        /// </summary>
        /// <param name="maKhach">Mã khách hàng</param>
        /// <param name="tenKhach">Tên khách hàng</param>
        /// <param name="gioiTinh">Giới tính</param>
        /// <param name="diaChi">Địa chỉ</param>
        /// <param name="dienThoai">Số điện thoại</param>
        public qlkhachhang(string maKhach, string tenKhach, string gioiTinh, string diaChi, string dienThoai)
        {
            MaKhach = maKhach;
            TenKhach = tenKhach;
            GioiTinh = gioiTinh;
            DiaChi = diaChi;
            DienThoai = dienThoai;
        }
    }
}
