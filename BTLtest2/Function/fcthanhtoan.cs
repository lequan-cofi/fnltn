using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BTLtest2.function
{
    internal class fcthanhtoan
    {
        // !!! IMPORTANT: Replace with your actual SQL Server connection string !!!
        private string connectionString = "Data Source=DESKTOP-VT5RUI9;Initial Catalog=laptrinh.net;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        // Example: "Data Source=DESKTOP-123XYZ\\SQLEXPRESS;Initial Catalog=MySalesDB;Integrated Security=True;"

        public fcthanhtoan() { }

        // Optional: Constructor to allow passing a connection string if needed from elsewhere
        public fcthanhtoan(string dbConnectionString)
        {
            if (!string.IsNullOrWhiteSpace(dbConnectionString))
            {
                this.connectionString = dbConnectionString;
            }
        }

        /// <summary>
        /// Fetches invoices that are not yet paid in cash or by bank transfer.
        /// </summary>
        /// <returns>A DataTable containing the invoices.</returns>
        public DataTable GetHoaDonChuaThanhToan()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Selects invoices where TrangThai is NULL or not 'Tiền mặt' and not 'Chuyển khoản'
                    // The N prefix is important for Unicode strings (Vietnamese).
                    string query = @"SELECT SoHDBan, MaNhanVien, NgayBan, MaKhach, TongTien, TrangThai
                                 FROM dbo.HoaDonBan
                                 WHERE TrangThai IS NULL OR (TRIM(TrangThai) NOT IN (N'Tiền mặt', N'Chuyển khoản'))
                                 ORDER BY NgayBan DESC;";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.Fill(dt);
                }
                catch (SqlException ex)
                {
                    // Log error or throw a custom exception
                    Console.WriteLine("SQL Error in GetHoaDonChuaThanhToan: " + ex.Message);
                    throw; // Re-throw to allow UI to handle it
                }
            }
            return dt;
        }

        /// <summary>
        /// Fetches the details for a specific invoice.
        /// </summary>
        /// <param name="soHDBan">The invoice number.</param>
        /// <returns>A DataTable containing the invoice details.</returns>
        public DataTable GetChiTietHoaDon(string soHDBan)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT MaSach, SLBan, KhuyenMai, ThanhTien, DonGiaBanKhiBan
                                 FROM dbo.ChiTietHDBan
                                 WHERE SoHDBan = @SoHDBan;";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SoHDBan", soHDBan);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL Error in GetChiTietHoaDon: " + ex.Message);
                    throw;
                }
            }
            return dt;
        }

        /// <summary>
        /// Updates the status (TrangThai) of a given invoice.
        /// </summary>
        /// <param name="soHDBan">The invoice number to update.</param>
        /// <param name="trangThaiMoi">The new status for the invoice.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        public bool UpdateTrangThaiHoaDon(string soHDBan, string trangThaiMoi)
        {
            int rowsAffected = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE dbo.HoaDonBan SET TrangThai = @TrangThai WHERE SoHDBan = @SoHDBan;";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TrangThai", trangThaiMoi);
                    cmd.Parameters.AddWithValue("@SoHDBan", soHDBan);
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL Error in UpdateTrangThaiHoaDon: " + ex.Message);
                    throw;
                }
            }
            return rowsAffected > 0;
        }
    }
}
