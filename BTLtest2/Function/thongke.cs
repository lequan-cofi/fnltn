using BTLtest2.Class;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLtest2.function
{
    internal class thongke
    {
        private static string connectionString = "Data Source=DESKTOP-VT5RUI9;Initial Catalog=laptrinh.net;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        private static SqlConnection connection = new SqlConnection(connectionString);
        public static List<ThongKeItem> ThongKe(DateTime tuNgay, DateTime denNgay)
        {
            var result = new List<ThongKeItem>();

            string query = @"
                -- Doanh thu theo sách
                SELECT ct.MaSach, SUM(ct.ThanhTien) AS DoanhThu
                INTO #TempDoanhThu
                FROM HoaDonBan hd
                JOIN ChiTietHDBan ct ON hd.SoHDBan = ct.SoHDBan
                WHERE hd.NgayBan BETWEEN @TuNgay AND @DenNgay
                GROUP BY ct.MaSach;

                -- Chi phí theo sách
                SELECT ct.MaSach, SUM(ct.ThanhTien) AS ChiPhi
                INTO #TempChiPhi
                FROM HoaDonNhap hd
                JOIN ChiTietHDNhap ct ON hd.SoHDNhap = ct.SoHDNhap
                WHERE hd.NgayNhap BETWEEN @TuNgay AND @DenNgay
                GROUP BY ct.MaSach;

                -- Gộp lại
                SELECT 
                    COALESCE(dt.MaSach, cp.MaSach) AS MaSach,
                    ISNULL(dt.DoanhThu, 0) AS DoanhThu,
                    ISNULL(cp.ChiPhi, 0) AS ChiPhi
                FROM #TempDoanhThu dt
                FULL OUTER JOIN #TempChiPhi cp ON dt.MaSach = cp.MaSach;

                DROP TABLE #TempDoanhThu;
                DROP TABLE #TempChiPhi;
            ";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TuNgay", tuNgay);
                cmd.Parameters.AddWithValue("@DenNgay", denNgay);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new ThongKeItem
                    {
                        MaSach = reader["MaSach"].ToString(),
                        DoanhThu = Convert.ToSingle(reader["DoanhThu"]),
                        ChiPhi = Convert.ToSingle(reader["ChiPhi"])
                    });
                }

                conn.Close();
            }

            return result;
        }
        }
}
