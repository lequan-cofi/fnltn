using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLtest2.Class
{
    public class MatSachDisplay
    {
        public int MaLanMat { get; set; } // Mã lần mất
        public string MaSach { get; set; } // Mã sách
        public string TenSach { get; set; } // Tên sách (liên kết từ KhoSach)
        public int SLMat { get; set; } // Số lượng mất
        public DateTime NgayMat { get; set; } // Ngày mất
    }

    // Lớp đại diện cho dữ liệu để thêm/cập nhật MatSach (không có TenSach)
    public class MatSachData
    {
        public int MaLanMat { get; set; } // Khóa chính, có thể là 0 cho mục mới nếu là identity
        public string MaSach { get; set; } // Mã sách
        public int SLMat { get; set; } // Số lượng mất
        public DateTime NgayMat { get; set; } // Ngày mất
    }

    // Lớp chứa thông tin cơ bản của sách cho ComboBox
    public class SachInfo
    {
        public string MaSach { get; set; } // Mã sách
        public string TenSach { get; set; } // Tên sách
        public int SoLuongHienCo { get; set; } // Số lượng hiện có trong KhoSach

        public override string ToString()
        {
            // Chuỗi này sẽ được hiển thị trong ComboBox
            return $"{TenSach} ({MaSach})";
        }
    }
}
