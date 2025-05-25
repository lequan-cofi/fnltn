// using BTLtest2.Class; // Đảm bảo using class cldoanhthu
using BTLtest2.Class;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text; // Thêm cho StringBuilder
// using System.Threading.Tasks; // Có thể không cần

namespace BTLtest2.function
{
    internal class baocaodoanhthu
    {
        private static string connectionString = "Data Source=DESKTOP-VT5RUI9;Initial Catalog=laptrinh.net;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        // Cập nhật hàm GetDoanhThu
        public static List<cldoanhthu> GetDoanhThu(DateTime fromDate, DateTime toDate, float? filterTren, float? filterDuoi)
        {
            List<cldoanhthu> list = new List<cldoanhthu>();
            StringBuilder queryBuilder = new StringBuilder("SELECT SoHDBan, NgayBan, MaKhach, TongTien FROM HoaDonBan WHERE NgayBan BETWEEN @fromDate AND @toDate");

            // Dictionary để lưu trữ parameters, tránh thêm nhiều lần nếu tên giống nhau (mặc dù ở đây không có trường hợp đó)
            var parameters = new Dictionary<string, object>
            {
                { "@fromDate", fromDate },
                { "@toDate", toDate }
            };

            if (filterTren.HasValue)
            {
                queryBuilder.Append(" AND TongTien >= @filterTren");
                parameters["@filterTren"] = filterTren.Value;
            }
            if (filterDuoi.HasValue)
            {
                queryBuilder.Append(" AND TongTien <= @filterDuoi");
                parameters["@filterDuoi"] = filterDuoi.Value;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cldoanhthu dt = new cldoanhthu
                            {
                                SoHDBan = reader["SoHDBan"].ToString(),
                                NgayBan = Convert.ToDateTime(reader["NgayBan"]),
                                MaKH = reader["MaKhach"].ToString(),
                                TongTien = Convert.ToSingle(reader["TongTien"])
                            };
                            list.Add(dt);
                        }
                    }
                }
            }
            return list;
        }
        // Hàm GetChiTietHoaDonBan giữ nguyên như bạn cung cấp
        public static List<clchitiethoadonban> GetChiTietHoaDonBan(string soHDBan) // Đổi tên tham số cho rõ ràng
        {
            List<clchitiethoadonban> list = new List<clchitiethoadonban>();
            // Câu query này có vẻ đang join ChiTietHDNhap để lấy DonGiaNhap làm cơ sở cho DonGia bán.
            // Điều này cần xem lại: DonGia bán nên lấy trực tiếp từ ChiTietHDBan hoặc bảng GiaBan nếu có.
            // Nếu bạn lưu DonGiaBan trong ChiTietHDBan thì dùng nó.
            // Giả sử DonGiaBan được lưu trong ChiTietHDBan.dbo.DonGia (ví dụ)
            // Nếu không, logic tính DonGia = reader["DonGiaNhap"] * 1.1f là một quy tắc nghiệp vụ.
            string query = @"
                SELECT  
                    c.MaSach,
                    k.TenSach,
                    c.SLBan,
                    cthdb.DonGia AS DonGiaBanThucTe, -- Giả sử bạn có cột DonGia trong ChiTietHDBan
                    c.KhuyenMai,
                    c.ThanhTien
                FROM ChiTietHDBan c
                JOIN KhoSach k ON c.MaSach = k.MaSach
                -- LEFT JOIN ChiTietHDBan cthdb ON c.SoHDBan = cthdb.SoHDBan AND c.MaSach = cthdb.MaSach -- Nếu DonGiaBan nằm trong ChiTietHDBan
                WHERE c.SoHDBan = @SoHDBan";
            // Nếu DonGiaBan không có sẵn và phải tính từ DonGiaNhap, câu query cần sửa:
            query = @"
                SELECT  
                    c.MaSach,
                    k.TenSach,
                    c.SLBan,
                    ISNULL(cthdn.DonGiaNhap, 0) AS DonGiaNhapCoBan, -- Lấy giá nhập gần nhất hoặc trung bình làm cơ sở
                    c.KhuyenMai,
                    c.ThanhTien
                FROM ChiTietHDBan c
                JOIN KhoSach k ON c.MaSach = k.MaSach
                LEFT JOIN (
                    SELECT MaSach, AVG(DonGiaNhap) AS DonGiaNhap -- Hoặc MAX(NgayNhap) để lấy giá nhập mới nhất
                    FROM ChiTietHDNhap 
                    GROUP BY MaSach
                ) cthdn ON c.MaSach = cthdn.MaSach
                WHERE c.SoHDBan = @SoHDBan";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SoHDBan", soHDBan);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            float donGiaBanTinhToan = Convert.ToSingle(reader["DonGiaNhapCoBan"]) * 1.1f; // Hoặc lấy từ reader["DonGiaBanThucTe"] nếu có
                            var ct = new clchitiethoadonban
                            {
                                MaSP = reader["MaSach"].ToString(),
                                TenSP = reader["TenSach"].ToString(),
                                SoLuong = Convert.ToInt32(reader["SLBan"]),
                                DonGia = donGiaBanTinhToan, // Cập nhật
                                KhuyenMai = Convert.ToSingle(reader["KhuyenMai"]),
                                ThanhTien = Convert.ToSingle(reader["ThanhTien"])
                            };
                            list.Add(ct);
                        }
                    }
                }
            }
            return list;
        }
    }
}