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


        public static List<MatHangThongKe> GetDoanhThuTheoMatHang(DateTime fromDate, DateTime toDate)
        {
            List<MatHangThongKe> data = new List<MatHangThongKe>();
            // Ví dụ câu SQL (cần điều chỉnh cho phù hợp với CSDL của bạn):
            string query = @"
                SELECT
                    ks.TenSach,  -- Hoặc tên cột tương ứng trong bảng sách của bạn
                    SUM(cthdb.ThanhTien) AS TongGiaTri
                FROM dbo.HoaDonBan hdb
                JOIN dbo.ChiTietHDBan cthdb ON hdb.SoHDBan = cthdb.SoHDBan
                JOIN dbo.KhoSach ks ON cthdb.MaSach = ks.MaSach -- Giả sử bảng KhoSach chứa TenSach
                WHERE hdb.NgayBan BETWEEN @FromDate AND @ToDate
                GROUP BY ks.TenSach
                HAVING SUM(cthdb.ThanhTien) > 0 -- Chỉ lấy các mặt hàng có doanh thu
                ORDER BY SUM(cthdb.ThanhTien) DESC;
            ";

            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            data.Add(new MatHangThongKe
                            {
                                TenMatHang = reader["TenSach"].ToString(),
                                GiaTri = Convert.ToSingle(reader["TongGiaTri"])
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        // Xử lý lỗi (ví dụ: ghi log, throw exception)
                        Console.WriteLine("Lỗi khi lấy doanh thu theo mặt hàng: " + ex.Message);
                    }
                }
            }
            
            return data;
        }
    }
}
