using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLtest2.Class
{
    internal class NhaCungCap
    {

        /// <summary>
        /// Mã nhà cung cấp (Khóa chính)
        /// </summary>
        public string MaNCC { get; set; }

        /// <summary>
        /// Tên nhà cung cấp
        /// </summary>
        public string TenNhaCungCap { get; set; }

        /// <summary>
        /// Địa chỉ của nhà cung cấp
        /// </summary>
        public string DiaChi { get; set; }

        /// <summary>
        /// Số điện thoại của nhà cung cấp
        /// </summary>
        public string DienThoai { get; set; }

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public NhaCungCap() { }

        /// <summary>
        /// Constructor với đầy đủ tham số
        /// </summary>
        /// <param name="maNCC">Mã nhà cung cấp</param>
        /// <param name="tenNhaCungCap">Tên nhà cung cấp</param>
        /// <param name="diaChi">Địa chỉ</param>
        /// <param name="dienThoai">Số điện thoại</param>
        public NhaCungCap(string maNCC, string tenNhaCungCap, string diaChi, string dienThoai)
        {
            MaNCC = maNCC;
            TenNhaCungCap = tenNhaCungCap;
            DiaChi = diaChi;
            DienThoai = dienThoai;
        }
    }
}
