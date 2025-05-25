using BTLtest2.Class;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLtest2.function
{
    internal class thongkedoanhthu
    {
        // ... (Các phương thức hiện có của bạn) ...

        // Chuỗi kết nối CSDL của bạn
        private static string connectionString = "Data Source=DESKTOP-VT5RUI9;Initial Catalog=laptrinh.net;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        private static SqlConnection connection = new SqlConnection(connectionString);


        public static List<MatHangThongKe> GetDoanhThuTheoMatHang(DateTime fromDate, DateTime toDate, float? filterTren, float? filterDuoi)
        {
            List<MatHangThongKe> list = new List<MatHangThongKe>();
            StringBuilder queryBuilder = new StringBuilder(@"
                SELECT 
                    k.TenSach, 
                    SUM(ct.ThanhTien) as TongDoanhThuMatHang
                FROM HoaDonBan h
                JOIN ChiTietHDBan ct ON h.SoHDBan = ct.SoHDBan
                JOIN KhoSach k ON ct.MaSach = k.MaSach
                WHERE h.NgayBan BETWEEN @fromDate AND @toDate");

            var parameters = new Dictionary<string, object>
            {
                { "@fromDate", fromDate },
                { "@toDate", toDate }
            };

            // Lưu ý: filterTren và filterDuoi ở đây sẽ áp dụng cho TỔNG DOANH THU CỦA MỖI MẶT HÀNG
            // Nếu bạn muốn áp dụng cho giá trị của từng hóa đơn bán lẻ, logic cần thay đổi.
            // Hiện tại, nó sẽ lọc sau khi đã SUM.

            queryBuilder.Append(" GROUP BY k.TenSach");

            // HAVING clause để lọc trên kết quả của SUM
            List<string> havingClauses = new List<string>();
            if (filterTren.HasValue)
            {
                havingClauses.Add("SUM(ct.ThanhTien) >= @filterTren");
                parameters["@filterTren"] = filterTren.Value;
            }
            if (filterDuoi.HasValue)
            {
                havingClauses.Add("SUM(ct.ThanhTien) <= @filterDuoi");
                parameters["@filterDuoi"] = filterDuoi.Value;
            }

            if (havingClauses.Any())
            {
                queryBuilder.Append(" HAVING " + string.Join(" AND ", havingClauses));
            }
            queryBuilder.Append(" ORDER BY TongDoanhThuMatHang DESC");


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
                            list.Add(new MatHangThongKe
                            {
                                TenMatHang = reader["TenSach"].ToString(),
                                GiaTri = Convert.ToSingle(reader["TongDoanhThuMatHang"])
                            });
                        }
                    }
                }
            }
            return list;
        }
    }
}
