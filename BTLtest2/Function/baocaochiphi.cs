using BTLtest2.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTLtest2.function
{
    internal class baocaochiphi
    {
        private static string connectionString = "Data Source=DESKTOP-VT5RUI9;Initial Catalog=laptrinh.net;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        public static List<clchiphi> GetChiPhi(DateTime fromDate, DateTime toDate, float chiPhiMin)
        {
            float tongChiPhi = 0;
            List<clchiphi> list = new List<clchiphi>();

            string query = @"SELECT SoHDNhap, NgayNhap, MaNCC, TongTien
                         FROM HoaDonNhap
                         WHERE NgayNhap BETWEEN @fromDate AND @toDate
                         AND TongTien >= @chiPhiMin";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@fromDate", fromDate);
                    command.Parameters.AddWithValue("@toDate", toDate);
                    command.Parameters.AddWithValue("@chiPhiMin", chiPhiMin);

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
                          
                            
                            tongChiPhi += cp.TongTien;
                            list.Add(cp);

                            
                        }

                    }
                }
                connection.Close();
            }
           
            return list;
       
        }
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


        // Có thể trả về kèm tổng chi phí nếu cần, hoặc gán vào TextBox/Label

    }
    }

