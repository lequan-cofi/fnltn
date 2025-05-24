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
    internal class KhachHang_Function
    {
        // Chuỗi kết nối đến cơ sở dữ liệu - CẦN THAY ĐỔI CHO PHÙ HỢP VỚI CẤU HÌNH CỦA BẠN
        // Ví dụ: "Data Source=TEN_MAY_CHU;Initial Catalog=TEN_CSDL;Integrated Security=True;"
        // Hoặc: "Data Source=TEN_MAY_CHU;Initial Catalog=TEN_CSDL;User ID=your_user;Password=your_password;"
        private string connectionString = "Data Source=DESKTOP-VT5RUI9;Initial Catalog=laptrinh.net;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        /// <summary>
        /// Lấy toàn bộ danh sách khách hàng từ cơ sở dữ liệu.
        /// </summary>
        /// <returns>DataTable chứa danh sách khách hàng.</returns>
        public DataTable LayDanhSachKhachHang()
        {
            DataTable dtKhachHang = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT MaKhach, TenKhach, GioiTinh, DiaChi, DienThoai FROM dbo.KhachHang";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.Fill(dtKhachHang);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải danh sách khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return dtKhachHang;
        }

        /// <summary>
        /// Thêm một khách hàng mới vào cơ sở dữ liệu.
        /// </summary>
        /// <param name="kh">Đối tượng KhachHang chứa thông tin cần thêm.</param>
        /// <returns>True nếu thêm thành công, False nếu thất bại.</returns>
        public bool ThemKhachHang(KhachHang kh)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO dbo.KhachHang (MaKhach, TenKhach, GioiTinh, DiaChi, DienThoai) VALUES (@MaKhach, @TenKhach, @GioiTinh, @DiaChi, @DienThoai)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MaKhach", kh.MaKhach);
                    command.Parameters.AddWithValue("@TenKhach", kh.TenKhach);
                    command.Parameters.AddWithValue("@GioiTinh", kh.GioiTinh);
                    command.Parameters.AddWithValue("@DiaChi", kh.DiaChi);
                    command.Parameters.AddWithValue("@DienThoai", kh.DienThoai);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627) // Lỗi vi phạm ràng buộc khóa chính (trùng mã)
                    {
                        MessageBox.Show("Lỗi: Mã khách hàng này đã tồn tại trong hệ thống.", "Lỗi Trùng Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Lỗi SQL khi thêm khách hàng: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi không xác định khi thêm khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        /// <summary>
        /// Cập nhật thông tin một khách hàng trong cơ sở dữ liệu.
        /// </summary>
        /// <param name="kh">Đối tượng KhachHang chứa thông tin cần cập nhật.</param>
        /// <returns>True nếu cập nhật thành công, False nếu thất bại.</returns>
        public bool SuaKhachHang(KhachHang kh)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE dbo.KhachHang SET TenKhach = @TenKhach, GioiTinh = @GioiTinh, DiaChi = @DiaChi, DienThoai = @DienThoai WHERE MaKhach = @MaKhach";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TenKhach", kh.TenKhach);
                    command.Parameters.AddWithValue("@GioiTinh", kh.GioiTinh);
                    command.Parameters.AddWithValue("@DiaChi", kh.DiaChi);
                    command.Parameters.AddWithValue("@DienThoai", kh.DienThoai);
                    command.Parameters.AddWithValue("@MaKhach", kh.MaKhach); // Điều kiện để biết sửa dòng nào

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật thông tin khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        /// <summary>
        /// Xóa một khách hàng khỏi cơ sở dữ liệu dựa vào Mã Khách.
        /// </summary>
        /// <param name="maKhach">Mã khách hàng cần xóa.</param>
        /// <returns>True nếu xóa thành công, False nếu thất bại.</returns>
        public bool XoaKhachHang(string maKhach)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Lưu ý: Cần xem xét các ràng buộc khóa ngoại. Nếu khách hàng này đã có trong bảng Hóa Đơn chẳng hạn,
                    // việc xóa trực tiếp có thể gây lỗi. Cần có cơ chế xử lý phù hợp (ví dụ: không cho xóa, hoặc xóa mềm).
                    string query = "DELETE FROM dbo.KhachHang WHERE MaKhach = @MaKhach";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MaKhach", maKhach);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    // Mã lỗi 547 thường là lỗi vi phạm ràng buộc khóa ngoại
                    if (ex.Number == 547)
                    {
                        MessageBox.Show("Không thể xóa khách hàng này do có dữ liệu liên quan (ví dụ: trong bảng hóa đơn).", "Lỗi Ràng Buộc Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Lỗi SQL khi xóa khách hàng: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi không xác định khi xóa khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
    }
}
