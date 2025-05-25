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
            DateTime? startDate, DateTime? endDate,
            int? filterSoLuongTonTren, // MODIFIED: For "greater than or equal to"
            int? filterSoLuongTonDuoi, // ADDED: For "less than or equal to"
            int? filterLuongBan = null, string filterLuongBanType = null)
        {
            List<Sach> inventoryItems = new List<Sach>();
            var queryBuilder = new StringBuilder();
            var parameters = new List<SqlParameter>();
            var whereConditions = new List<string>(); // For date filtering on hdb
            var havingConditions = new List<string>(); // For ks.SoLuong and SUM(cthdb.SLBan)

            queryBuilder.Append(@"
                SELECT
                    ks.MaSach,
                    ks.TenSach,
                    ks.SoLuong AS SoLuongTonKho,
                    ks.DonGiaNhap,
                    ks.DonGiaBan,
                    ks.MaLoaiSach,
                    ISNULL(SUM(CASE WHEN hdb.NgayBan IS NOT NULL THEN cthdb.SLBan ELSE 0 END), 0) AS LuongBanDaTinh
                FROM
                    dbo.KhoSach ks
                LEFT JOIN
                    dbo.ChiTietHDBan cthdb ON ks.MaSach = cthdb.MaSach
            ");

            // Conditionally join HoaDonBan if date filters are present or LuongBan filter is present
            // This is crucial for the LuongBanDaTinh calculation and its filtering.
            if (startDate.HasValue || endDate.HasValue || filterLuongBan.HasValue)
            {
                queryBuilder.Append(@"
                LEFT JOIN
                    dbo.HoaDonBan hdb ON cthdb.SoHDBan = hdb.SoHDBan
                ");
            }
            else // If no date or LuongBan filters, no need to join HoaDonBan for filtering purposes,
                 // but still need it for SUM if it's not always 0.
                 // However, the LuongBanDaTinh still needs the hdb join.
                 // Let's simplify: always join hdb if cthdb is joined for LuongBanDaTinh.
            {
                // The initial LEFT JOIN cthdb then requires hdb for NgayBan check in SUM.
                // So, if cthdb is joined, hdb should be too.
                // A better structure for LuongBanDaTinh that is always correct regardless of other filters
                // might involve a subquery for sales, but current approach is okay if WHERE conditions are careful.
                // For clarity, ensure HoaDonBan is joined if ChiTietHDBan is.
                queryBuilder.Append(@"
                LEFT JOIN 
                    dbo.HoaDonBan hdb ON cthdb.SoHDBan = hdb.SoHDBan 
                        AND (1=1 "); // Start an AND block for potential date filters on the JOIN itself
                                     // This can be cleaner than putting date logic in the main WHERE for a LEFT JOIN aggregate.

                if (startDate.HasValue)
                {
                    queryBuilder.Append(" AND hdb.NgayBan >= @StartDate ");
                    parameters.Add(new SqlParameter("@StartDate", startDate.Value.Date));
                }
                if (endDate.HasValue)
                {
                    queryBuilder.Append(" AND hdb.NgayBan < @EndDatePlusOne ");
                    parameters.Add(new SqlParameter("@EndDatePlusOne", endDate.Value.Date.AddDays(1)));
                }
                queryBuilder.Append(" ) "); // Close the AND block for the join condition
            }


            // WHERE clause for date filters (applies to hdb if joined for date filtering context)
            // If we filtered hdb in the JOIN, we might not need these in the WHERE for LuongBanDaTinh accuracy
            // However, if the LEFT JOIN was structured without date filters on the join itself, then WHERE is needed.
            // The code had date conditions on hdb in WHERE, which is tricky with LEFT JOIN if you want to keep all KhoSach.
            // Let's refine: if dates are for LuongBan, they should filter hdb.
            // The `CASE WHEN hdb.NgayBan IS NOT NULL` in SUM handles non-matching sales.
            // But if you want LuongBan for ONLY a specific period, the rows from hdb must be filtered.

            // Corrected logic for date filtering effect on LuongBanDaTinh:
            // The join to hdb should be filtered by date if you want LuongBanDaTinh for that period.
            // The current query already filters hdb.NgayBan within the SUM implicitly by its presence.
            // The `whereConditions` were for this.
            // Let's stick to filtering hdb within the SUM or JOIN for LuongBan.
            // The query logic for date filtering:
            // The SUM(CASE WHEN hdb.NgayBan IS NOT NULL AND hdb.NgayBan >= @StartDate AND hdb.NgayBan < @EndDatePlusOne THEN cthdb.SLBan ELSE 0 END)
            // would be more precise if NgayBan is always available.
            // Given the current structure, the WHERE conditions on hdb.NgayBan are necessary to limit the sales period.

            if (startDate.HasValue && (filterLuongBan.HasValue || (startDate.HasValue || endDate.HasValue))) // only add date filters to WHERE if they are relevant to hdb
            {
                whereConditions.Add("(hdb.SoHDBan IS NULL OR hdb.NgayBan >= @StartDateFromWhere)");
                parameters.Add(new SqlParameter("@StartDateFromWhere", startDate.Value.Date)); // Use a different param name to avoid conflict if used in JOIN
            }
            if (endDate.HasValue && (filterLuongBan.HasValue || (startDate.HasValue || endDate.HasValue)))
            {
                whereConditions.Add("(hdb.SoHDBan IS NULL OR hdb.NgayBan < @EndDatePlusOneFromWhere)");
                parameters.Add(new SqlParameter("@EndDatePlusOneFromWhere", endDate.Value.Date.AddDays(1)));
            }


            if (whereConditions.Any())
            {
                queryBuilder.Append(" WHERE " + string.Join(" AND ", whereConditions));
            }

            queryBuilder.Append(@"
                GROUP BY
                    ks.MaSach, ks.TenSach, ks.SoLuong, ks.DonGiaNhap, ks.DonGiaBan, ks.MaLoaiSach
            ");

            // HAVING clause for inventory quantity and sold quantity filters
            if (filterSoLuongTonTren.HasValue)
            {
                havingConditions.Add($"ks.SoLuong >= @FilterSoLuongTonTren");
                parameters.Add(new SqlParameter("@FilterSoLuongTonTren", filterSoLuongTonTren.Value));
            }
            if (filterSoLuongTonDuoi.HasValue)
            {
                havingConditions.Add($"ks.SoLuong <= @FilterSoLuongTonDuoi");
                parameters.Add(new SqlParameter("@FilterSoLuongTonDuoi", filterSoLuongTonDuoi.Value));
            }

            if (filterLuongBan.HasValue && !string.IsNullOrEmpty(filterLuongBanType))
            {
                string opSLBan = GetOperator(filterLuongBanType);
                if (!string.IsNullOrEmpty(opSLBan))
                {
                    // This SUM reflects sales within the date range if dates were applied to hdb
                    havingConditions.Add($"ISNULL(SUM(CASE WHEN hdb.NgayBan IS NOT NULL THEN cthdb.SLBan ELSE 0 END), 0) {opSLBan} @FilterLuongBan");
                    parameters.Add(new SqlParameter("@FilterLuongBan", filterLuongBan.Value));
                }
            }

            if (havingConditions.Any())
            {
                queryBuilder.Append(" HAVING " + string.Join(" AND ", havingConditions));
            }

            queryBuilder.Append(" ORDER BY ks.TenSach ASC;"); // Changed default sort for better readability

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection))
                {
                    if (parameters.Any())
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                    }
                    // For debugging the query:
                    // Console.WriteLine("Executing SQL: " + command.CommandText);
                    // foreach (SqlParameter p in command.Parameters) { Console.WriteLine($"{p.ParameterName}: {p.Value}"); }


                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Sach item = new Sach
                            {
                                MaSach = reader["MaSach"].ToString(),
                                TenSach = reader["TenSach"].ToString(),
                                SoLuong = Convert.ToInt32(reader["SoLuongTonKho"]),
                                DonGiaNhap = reader["DonGiaNhap"].ToString(), // Or Convert.ToDecimal
                                DonGiaBan = reader["DonGiaBan"].ToString(),   // Or Convert.ToDecimal
                                MaLoaiSach = reader["MaLoaiSach"].ToString(),
                                LuongBanDaTinh = Convert.ToInt32(reader["LuongBanDaTinh"])
                            };
                            inventoryItems.Add(item);
                        }
                        reader.Close();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Lỗi SQL khi lấy hàng tồn kho: " + ex.Message);
                        MessageBox.Show($"Lỗi truy vấn cơ sở dữ liệu: {ex.ToString()}\nQuery:\n{command.CommandText}", "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Lỗi chung khi lấy hàng tồn kho: " + ex.Message);
                        MessageBox.Show($"Đã xảy ra lỗi: {ex.ToString()}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                default: return null; // Or throw an exception for an unsupported type
            }
        }
    }
}
