using BTLtest2.Class;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTLtest2.function
{
    internal class DataAccess
    {
        // !!! THAY THẾ BẰNG CHUỖI KẾT NỐI CỦA BẠN !!!
        private string connectionString = "Data Source=DESKTOP-VT5RUI9;Initial Catalog=laptrinh.net;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        // Hoặc nếu dùng Windows Authentication:
        // private string connectionString = "Server=TEN_SERVER_CUA_BAN;Database=TEN_DATABASE_CUA_BAN;Integrated Security=True;";

        /// <summary>
        /// Lấy danh sách khách hàng thân thiết dựa trên các tiêu chí.
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu khoảng thời gian.</param>
        /// <param name="endDate">Ngày kết thúc khoảng thời gian.</param>
        /// <param name="minSoLanMua">Số lần mua tối thiểu.</param>
        /// <param name="minTongHoaDon">Tổng giá trị hóa đơn tối thiểu.</param>
        /// <returns>Danh sách đối tượng KhachHang thỏa mãn điều kiện.</returns>
        public List<KhachHang> GetLoyalCustomers(DateTime startDate, DateTime endDate, int minSoLanMua, double minTongHoaDon)
        {
            List<KhachHang> loyalCustomers = new List<KhachHang>();

            // Câu SQL để lấy thông tin khách hàng và tổng hợp số lần mua, tổng tiền hóa đơn
            // từ bảng HoaDonBan trong khoảng thời gian và các điều kiện đưa ra.
            string query = @"
                SELECT
                    KH.MaKhach,
                    KH.TenKhach,
                    KH.DienThoai,
                    COUNT(HDB.SoHDBan) AS SoLanMua,
                    SUM(HDB.TongTien) AS TongTienHoaDon
                FROM
                    dbo.KhachHang KH
                INNER JOIN
                    dbo.HoaDonBan HDB ON KH.MaKhach = HDB.MaKhach
                WHERE
                    HDB.NgayBan BETWEEN @StartDate AND @EndDate
                GROUP BY
                    KH.MaKhach, KH.TenKhach, KH.DienThoai
                HAVING
                    COUNT(HDB.SoHDBan) >= @MinSoLanMua
                    AND SUM(HDB.TongTien) >= @MinTongHoaDon
                ORDER BY
                    TongTienHoaDon DESC, SoLanMua DESC, KH.TenKhach;
            ";
            // INNER JOIN đảm bảo chỉ lấy khách hàng có hóa đơn.
            // Nếu muốn lấy cả khách hàng không có hóa đơn (nhưng các tiêu chí minSoLanMua, minTongHoaDon sẽ loại họ),
            // bạn có thể cân nhắc LEFT JOIN và xử lý ISNULL ở HAVING. Tuy nhiên, với "khách hàng thân thiết", INNER JOIN thường phù hợp hơn.

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Thêm tham số để tránh SQL Injection và xử lý kiểu dữ liệu đúng cách
                    command.Parameters.AddWithValue("@StartDate", startDate.Date); // Chỉ lấy phần ngày
                    command.Parameters.AddWithValue("@EndDate", endDate.Date.AddDays(1).AddTicks(-1)); // Bao gồm cả ngày kết thúc đến cuối ngày
                    command.Parameters.AddWithValue("@MinSoLanMua", minSoLanMua);
                    command.Parameters.AddWithValue("@MinTongHoaDon", minTongHoaDon);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            KhachHang kh = new KhachHang(
                                reader["MaKhach"].ToString(),
                                reader["TenKhach"].ToString(),
                                reader["DienThoai"].ToString(),
                                Convert.ToInt32(reader["SoLanMua"]),
                                Convert.ToDouble(reader["TongTienHoaDon"])
                            );
                            loyalCustomers.Add(kh);
                        }
                        reader.Close();
                    }
                    catch (SqlException ex)
                    {
                        // Xử lý lỗi kết nối hoặc truy vấn (ví dụ: ghi log, hiển thị thông báo)
                        Console.WriteLine("Lỗi SQL: " + ex.Message);
                        // Trong ứng dụng thực tế, bạn có thể throw lại lỗi hoặc trả về null/thông báo lỗi
                        MessageBox.Show($"Lỗi truy vấn cơ sở dữ liệu: {ex.Message}", "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Lỗi chung: " + ex.Message);
                        MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return loyalCustomers;
        }
    }
}
