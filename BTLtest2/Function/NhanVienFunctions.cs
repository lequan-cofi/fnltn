using BTLtest2.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLtest2.function
{
    internal class NhanVienFunctions
    {
        private string connectionString = "Data Source=DESKTOP-VT5RUI9;Initial Catalog=laptrinh.net;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"; // Replace with your actual connection string

        // Method to get all employees
        public DataTable GetAllNhanVien()
        {
            DataTable dtNhanVien = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT MaNhanVien, TenNhanVien, DiaChi, DienThoai, MaCv, GioiTinh, NgaySinh FROM NhanVien", con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dtNhanVien);
                }
            }
            return dtNhanVien;
        }

        // Method to add a new employee
        public bool AddNhanVien(NhanVien nv)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO NhanVien (MaNhanVien, TenNhanVien, DiaChi, DienThoai, MaCv, GioiTinh, NgaySinh) VALUES (@MaNhanVien, @TenNhanVien, @DiaChi, @DienThoai, @MaCv, @GioiTinh, @NgaySinh)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MaNhanVien", nv.MaNhanVien);
                    cmd.Parameters.AddWithValue("@TenNhanVien", nv.TenNhanVien);
                    cmd.Parameters.AddWithValue("@DiaChi", nv.DiaChi);
                    cmd.Parameters.AddWithValue("@DienThoai", nv.DienThoai);
                    cmd.Parameters.AddWithValue("@MaCv", nv.MaCv);
                    cmd.Parameters.AddWithValue("@GioiTinh", nv.GioiTinh);
                    cmd.Parameters.AddWithValue("@NgaySinh", (object)nv.NgaySinh ?? DBNull.Value); // Handle nullable DateTime

                    con.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        // Method to update an existing employee
        public bool UpdateNhanVien(NhanVien nv)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE NhanVien SET TenNhanVien = @TenNhanVien, DiaChi = @DiaChi, DienThoai = @DienThoai, MaCv = @MaCv, GioiTinh = @GioiTinh, NgaySinh = @NgaySinh WHERE MaNhanVien = @MaNhanVien";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TenNhanVien", nv.TenNhanVien);
                    cmd.Parameters.AddWithValue("@DiaChi", nv.DiaChi);
                    cmd.Parameters.AddWithValue("@DienThoai", nv.DienThoai);
                    cmd.Parameters.AddWithValue("@MaCv", nv.MaCv);
                    cmd.Parameters.AddWithValue("@GioiTinh", nv.GioiTinh);
                    cmd.Parameters.AddWithValue("@NgaySinh", (object)nv.NgaySinh ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaNhanVien", nv.MaNhanVien);

                    con.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        // Method to delete an employee
        public bool DeleteNhanVien(string maNhanVien)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM NhanVien WHERE MaNhanVien = @MaNhanVien";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);

                    con.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        // Method to get all job titles (CongViec) for the ComboBox
        public List<CongViec> GetCongViecList()
        {
            List<CongViec> cvList = new List<CongViec>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT MaCv, TenCongViec FROM CongViec", con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cvList.Add(new CongViec(reader["MaCv"].ToString(), reader["TenCongViec"].ToString()));
                    }
                }
            }
            return cvList;
        }
    }
}
