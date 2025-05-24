using BTLtest2.Class;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLtest2.function
{
    internal class thongkechiphi
    {
        // ... (Các phương thức hiện có của bạn) ...

        // Chuỗi kết nối CSDL của bạn
        private static string connectionString = "Data Source=DESKTOP-VT5RUI9;Initial Catalog=laptrinh.net;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        private static SqlConnection connection = new SqlConnection(connectionString);

        // Chi phí ở đây được hiểu là tổng tiền nhập hàng
        public static List<MatHangThongKe> GetChiPhiTheoMatHang(DateTime fromDate, DateTime toDate)
        {
            List<MatHangThongKe> data = new List<MatHangThongKe>();
            // Ví dụ câu SQL (cần điều chỉnh):
            string query = @"
                SELECT
                    ks.TenSach,
                    SUM(cthdn.ThanhTien) AS TongGiaTri -- Giả sử ThanhTien trong ChiTietHDNhap là chi phí nhập
                FROM dbo.HoaDonNhap hdn
                JOIN dbo.ChiTietHDNhap cthdn ON hdn.SoHDNhap = cthdn.SoHDNhap
                JOIN dbo.KhoSach ks ON cthdn.MaSach = ks.MaSach
                WHERE hdn.NgayNhap BETWEEN @FromDate AND @ToDate
                GROUP BY ks.TenSach
                HAVING SUM(cthdn.ThanhTien) > 0
                ORDER BY SUM(cthdn.ThanhTien) DESC;
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
                        Console.WriteLine("Lỗi khi lấy chi phí theo mặt hàng: " + ex.Message);
                    }
                }
            }
           
            return data;
        }
    }
}
