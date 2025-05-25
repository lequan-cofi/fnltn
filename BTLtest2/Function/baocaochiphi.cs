// using BTLtest2.Class; // Đảm bảo using class clchiphi
using BTLtest2.Class;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text; // Thêm

namespace BTLtest2.function
{
    internal class baocaochiphi
    {
        private static string connectionString = "Data Source=DESKTOP-VT5RUI9;Initial Catalog=laptrinh.net;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        // Cập nhật hàm GetChiPhi
        public static List<clchiphi> GetChiPhi(DateTime fromDate, DateTime toDate, float? filterTren, float? filterDuoi)
        {
            List<clchiphi> list = new List<clchiphi>();
            StringBuilder queryBuilder = new StringBuilder("SELECT SoHDNhap, NgayNhap, MaNCC, TongTien FROM HoaDonNhap WHERE NgayNhap BETWEEN @fromDate AND @toDate");

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
                            clchiphi cp = new clchiphi
                            {
                                SoHDNhap = reader["SoHDNhap"].ToString(),
                                NgayNhap = Convert.ToDateTime(reader["NgayNhap"]),
                                MaNCC = reader["MaNCC"].ToString(),
                                TongTien = Convert.ToSingle(reader["TongTien"])
                            };
                            list.Add(cp);
                        }
                    }
                }
            }
            return list;
        }
        // Hàm GetChiTietHoaDonNhap giữ nguyên
        public static List<clchitiethoadonnhap> GetChiTietHoaDonNhap(string soHDNhap)
        {
            List<clchitiethoadonnhap> list = new List<clchitiethoadonnhap>();
            string query = @"
                SELECT  
                    c.MaSach,
                    k.TenSach,
                    c.SLNhap,
                    c.DonGiaNhap,
                    c.KhuyenMai,
                    c.ThanhTien
                FROM ChiTietHDNhap c
                JOIN KhoSach k ON c.MaSach = k.MaSach
                WHERE c.SoHDNhap = @SoHDNhap";

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
                            var ct = new clchitiethoadonnhap
                            {
                                MaSP = reader["MaSach"].ToString(),
                                TenSP = reader["TenSach"].ToString(),
                                SoLuong = Convert.ToInt32(reader["SLNhap"]),
                                DonGia = Convert.ToSingle(reader["DonGiaNhap"]),
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