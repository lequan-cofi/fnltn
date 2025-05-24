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
    internal class InventoryDataAccess
    {
        private string connectionString = "Data Source=DESKTOP-VT5RUI9;Initial Catalog=laptrinh.net;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        public List<Sach> GetInventoryItems(
            DateTime? startDate, DateTime? endDate, // <<< ADDED date parameters
            int? filterSoLuongTon = null, string filterSoLuongTonType = null,
            int? filterLuongBan = null, string filterLuongBanType = null)
        {
            List<Sach> inventoryItems = new List<Sach>();
            var queryBuilder = new StringBuilder();
            var parameters = new List<SqlParameter>();
            var whereConditions = new List<string>(); // For date filtering in the main WHERE clause

            queryBuilder.Append(@"
                SELECT
                    ks.MaSach,
                    ks.TenSach,
                    ks.SoLuong AS SoLuongTonKho, 
                    ks.DonGiaNhap,
                    ks.DonGiaBan,
                    ks.MaLoaiSach,
                    -- Add other fields from KhoSach if needed and ensure they are in GROUP BY
                    ISNULL(SUM(CASE WHEN hdb.NgayBan IS NOT NULL THEN cthdb.SLBan ELSE 0 END), 0) AS LuongBanDaTinh
                    -- We use a CASE statement inside SUM to only sum SLBan if NgayBan (from HoaDonBan) falls within the date range.
                    -- The date range filtering will be applied in the WHERE clause of the main query that affects the JOIN with HoaDonBan.
                FROM
                    dbo.KhoSach ks
                LEFT JOIN
                    dbo.ChiTietHDBan cthdb ON ks.MaSach = cthdb.MaSach
            ");

            // Conditionally join HoaDonBan and apply date filters in the WHERE clause
            // This ensures SUM(cthdb.SLBan) is affected by the date range.
            if (startDate.HasValue || endDate.HasValue)
            {
                queryBuilder.Append(@"
                LEFT JOIN
                    dbo.HoaDonBan hdb ON cthdb.SoHDBan = hdb.SoHDBan
                "); // The WHERE clause below will filter these joined records

                if (startDate.HasValue)
                {
                    // Add a condition that applies to the hdb.NgayBan OR allows rows from ks if no sales match
                    // This structure is tricky with LEFT JOIN and date-filtered aggregates.
                    // A subquery for LuongBan might be cleaner, or ensure the WHERE clause on dates
                    // doesn't eliminate KhoSach records that have no sales in the period.

                    // Let's adjust the approach: The main query gets all KhoSach.
                    // The LuongBanDaTinh will be correctly calculated if the WHERE clause for dates is applied
                    // to the records contributing to SUM.
                    // The current structure of SUMMING and then filtering by date range in WHERE on hdb might be tricky.

                    // Revised approach: Filter sales data *before* aggregation or ensure the GROUP BY includes all ks items.
                    // The provided query with LEFT JOIN and GROUP BY ks.* should correctly sum sales only for matching hdb records
                    // if the date condition is in a WHERE clause that applies to hdb.
                }
            }

            // Add date conditions to the main WHERE clause if dates are provided
            if (startDate.HasValue)
            {
                // This condition will apply to HoaDonBan if joined, potentially filtering out KhoSach items
                // if they don't have sales in the period AND we are not careful.
                // For an accurate "LuongBanDaTinh" within a period while keeping all "KhoSach" items,
                // the SUM needs to be conditional or the join filtered.
                // The ISNULL(SUM(CASE WHEN hdb.NgayBan IS NOT NULL THEN cthdb.SLBan ELSE 0 END), 0)
                // combined with WHERE clause on hdb.NgayBan should work.
                whereConditions.Add("(hdb.SoHDBan IS NULL OR hdb.NgayBan >= @StartDate)"); // Allow ks rows with no sales, or sales within date
                parameters.Add(new SqlParameter("@StartDate", startDate.Value));
            }
            if (endDate.HasValue)
            {
                whereConditions.Add("(hdb.SoHDBan IS NULL OR hdb.NgayBan < @EndDatePlusOne)");
                parameters.Add(new SqlParameter("@EndDatePlusOne", endDate.Value.AddDays(1)));
            }


            if (whereConditions.Any())
            {
                queryBuilder.Append(" WHERE " + string.Join(" AND ", whereConditions));
            }

            queryBuilder.Append(@"
                GROUP BY
                    ks.MaSach, ks.TenSach, ks.SoLuong, ks.DonGiaNhap, ks.DonGiaBan, ks.MaLoaiSach
                    -- Add other fields from KhoSach here if selected in the main SELECT
            ");

            var havingConditions = new List<string>();

            if (filterSoLuongTon.HasValue && !string.IsNullOrEmpty(filterSoLuongTonType))
            {
                string opSLTon = GetOperator(filterSoLuongTonType);
                if (!string.IsNullOrEmpty(opSLTon))
                {
                    havingConditions.Add($"ks.SoLuong {opSLTon} @FilterSoLuongTon");
                    parameters.Add(new SqlParameter("@FilterSoLuongTon", filterSoLuongTon.Value));
                }
            }

            if (filterLuongBan.HasValue && !string.IsNullOrEmpty(filterLuongBanType))
            {
                string opSLBan = GetOperator(filterLuongBanType);
                if (!string.IsNullOrEmpty(opSLBan))
                {
                    // The SUM in HAVING refers to the sum over the records that made it through the WHERE clause
                    havingConditions.Add($"ISNULL(SUM(CASE WHEN hdb.NgayBan IS NOT NULL THEN cthdb.SLBan ELSE 0 END), 0) {opSLBan} @FilterLuongBan");
                    parameters.Add(new SqlParameter("@FilterLuongBan", filterLuongBan.Value));
                }
            }

            if (havingConditions.Any())
            {
                queryBuilder.Append(" HAVING " + string.Join(" AND ", havingConditions));
            }

            queryBuilder.Append(" ORDER BY ks.SoLuong ASC, ks.TenSach ASC;");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection))
                {
                    if (parameters.Any())
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                    }

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Sach item = new Sach // Ensure your Sach class has these properties
                            {
                                MaSach = reader["MaSach"].ToString(),
                                TenSach = reader["TenSach"].ToString(),
                                SoLuong = Convert.ToInt32(reader["SoLuongTonKho"]),
                                DonGiaNhap = reader["DonGiaNhap"].ToString(), // Or convert to decimal
                                DonGiaBan = reader["DonGiaBan"].ToString(),   // Or convert to decimal
                                MaLoaiSach = reader["MaLoaiSach"].ToString(),
                                // Make sure Sach class has LuongBanDaTinh
                                LuongBanDaTinh = Convert.ToInt32(reader["LuongBanDaTinh"])
                            };
                            inventoryItems.Add(item);
                        }
                        reader.Close();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Lỗi SQL khi lấy hàng tồn kho: " + ex.Message);
                        MessageBox.Show($"Lỗi truy vấn cơ sở dữ liệu: {ex.ToString()}", "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show full error
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Lỗi chung khi lấy hàng tồn kho: " + ex.Message);
                        MessageBox.Show($"Đã xảy ra lỗi: {ex.ToString()}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show full error
                    }
                }
            }
            return inventoryItems;
        }

        private string GetOperator(string filterType)
        {
            switch (filterType.ToLower())
            {
                case "lessequal": return "<=";
                case "greaterequal": return ">=";
                case "equal": return "=";
                default: return null;
            }
        }
    }
}
