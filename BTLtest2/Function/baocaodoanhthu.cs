using BTLtest2.Class;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLtest2.function
{
    internal class baocaodoanhthu
    {
        private static string connectionString = "Data Source=DESKTOP-VT5RUI9;Initial Catalog=laptrinh.net;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        public static List<cldoanhthu> GetDoanhThu(DateTime fromDate, DateTime toDate, float doanhThuMin)
        {
            float tongDoanhthu = 0;
            List<cldoanhthu> list = new List<cldoanhthu>();

            string query = @"SELECT SoHDBan, NgayBan, MaKhach, TongTien
                         FROM HoaDonBan
                         WHERE NgayBan BETWEEN @fromDate AND @toDate
                         AND TongTien >= @doanhThuMin";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@fromDate", fromDate);
                    command.Parameters.AddWithValue("@toDate", toDate);
                    command.Parameters.AddWithValue("@doanhThuMin", doanhThuMin);

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
                            

                            tongDoanhthu += dt.TongTien;
                            list.Add(dt);


                        }

                    }
                }
                connection.Close();
            }

            return list;

        }
        public static List<clchitiethoadonban> GetChiTietHoaDonBan(string soHDNhap)
        {
            List<clchitiethoadonban> list = new List<clchitiethoadonban>();

            string query = @"
      SELECT 
    c.MaSach,
    k.TenSach,
    c.SLBan,
    c.KhuyenMai,
    c.ThanhTien,
    n.DonGiaNhap 
FROM ChiTietHDBan c
JOIN KhoSach k ON c.MaSach = k.MaSach
JOIN ChiTietHDNhap n ON c.MaSach = n.MaSach 
WHERE c.SoHDBan =  @SoHDBan";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SoHDNhap", soHDNhap);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var ct = new clchitiethoadonban
                            {
                                MaSP = reader["MaSach"].ToString(),
                                TenSP = reader["TenSach"].ToString(),
                                SoLuong = Convert.ToInt32(reader["SLBan"]),
                                DonGia = Convert.ToSingle(reader["DonGiaNhap"]) * 1.1f,
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
