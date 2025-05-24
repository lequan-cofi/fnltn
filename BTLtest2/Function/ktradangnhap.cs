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
    
    internal class ktradangnhap
    {
        private static string connectionString = "Data Source=DESKTOP-VT5RUI9;Initial Catalog=laptrinh.net;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        private static SqlConnection connection = new SqlConnection(connectionString);

        public static void EnsureConnectionOpen()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }
        public static bool Kiemtra(string username, string password)
        {
            try
            {
                EnsureConnectionOpen();
                string sql = "SELECT * FROM taikhoan WHERE usename = @username AND password = @password";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                SqlDataReader reader = cmd.ExecuteReader();
                bool isValid = reader.HasRows;
                reader.Close();
                return isValid;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
                return false;
            }
        }
        // kiem tra phan quyen
        public static string KiemtraPhanquyen(string username)
        {
            try
            {
                EnsureConnectionOpen();
                string sql = "SELECT phanquyen FROM taikhoan WHERE usename = @username";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@username", username);
                SqlDataReader reader = cmd.ExecuteReader();
                string phanquyen = null;
                if (reader.Read())
                {
                    phanquyen = reader["phanquyen"].ToString();
                }
                reader.Close();
                return phanquyen;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
                return null;
            }
        }

    }
}
