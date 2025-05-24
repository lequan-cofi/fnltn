using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static BTLtest2.Class.ThongKeItem;

namespace BTLtest2.function
{
    internal class thongkehanghoa
    {
        private const string ConnectionString = "Data Source=DESKTOP-VT5RUI9;Initial Catalog=laptrinh.net;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        public static List<InventoryDataPoint> GetInventoryReportData(DateTime tuNgay, DateTime denNgay)
        {
            var reportData = new List<InventoryDataPoint>();
            DateTime denNgayThucTe = denNgay.Date.AddDays(1).AddTicks(-1);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = @"
                    SELECT
                        ks.TenSach,
                        (ks.SoLuong - ISNULL(Sales.TotalSold, 0)) AS LuongTonCuoiKy
                    FROM
                        dbo.KhoSach ks
                    LEFT JOIN
                        (SELECT
                            cthd.MaSach,
                            SUM(cthd.SLBan) AS TotalSold
                        FROM
                            dbo.ChiTietHDBan cthd
                        JOIN
                            dbo.HDBan hd ON cthd.SoHDBan = hd.SoHDBan
                        WHERE
                            hd.NgayBan >= @TuNgayParam AND hd.NgayBan <= @DenNgayParam
                        GROUP BY
                            cthd.MaSach
                        ) AS Sales ON ks.MaSach = Sales.MaSach
                    WHERE (ks.SoLuong - ISNULL(Sales.TotalSold, 0)) > 0
                    ORDER BY LuongTonCuoiKy DESC;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TuNgayParam", tuNgay.Date);
                    command.Parameters.AddWithValue("@DenNgayParam", denNgayThucTe);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                reportData.Add(new InventoryDataPoint
                                {
                                    TenSach = reader["TenSach"].ToString(),
                                    LuongTon = Convert.ToInt32(reader["LuongTonCuoiKy"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("SQL Error (GetInventoryReportData): " + ex.Message);
                        // Bạn có thể chọn ném lỗi ra ngoài để Form xử lý,
                        // hoặc hiển thị MessageBox tại đây (nhưng không lý tưởng lắm cho lớp helper)
                        MessageBox.Show($"Lỗi khi lấy dữ liệu báo cáo: {ex.Message}", "Lỗi Cơ Sở Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return reportData;
        }
        public static int CalculateTotalSoldInPeriod(DateTime tuNgay, DateTime denNgay)
        {
            int totalSold = 0;
            DateTime denNgayThucTe = denNgay.Date.AddDays(1).AddTicks(-1);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = @"
                    SELECT SUM(ISNULL(cthd.SLBan, 0))
                    FROM dbo.ChiTietHDBan cthd
                    JOIN dbo.HDBan hd ON cthd.SoHDBan = hd.SoHDBan
                    WHERE hd.NgayBan >= @TuNgayParam AND hd.NgayBan <= @DenNgayParam;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TuNgayParam", tuNgay.Date);
                    command.Parameters.AddWithValue("@DenNgayParam", denNgayThucTe);
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            totalSold = Convert.ToInt32(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("SQL Error (CalculateTotalSoldInPeriod): " + ex.Message);
                        MessageBox.Show($"Lỗi khi tính tổng số lượng bán: {ex.Message}", "Lỗi Cơ Sở Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return totalSold;
        }

    }
    }

