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
    internal class NhaCungCapBUS
    {
        // !!! THAY THẾ BẰNG CHUỖI KẾT NỐI THỰC TẾ CỦA BẠN !!!
        private string connectionString = "Data Source=DESKTOP-VT5RUI9;Initial Catalog=laptrinh.net;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        // Ví dụ: "Data Source=DESKTOP-YOURPC\\SQLEXPRESS;Initial Catalog=QuanLyBanHang;Integrated Security=True;"

        /// <summary>
        /// Lấy danh sách tất cả các nhà cung cấp từ cơ sở dữ liệu.
        /// </summary>
        /// <returns>DataTable chứa danh sách nhà cung cấp.</returns>
        public DataTable LayDanhSachNhaCungCap()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT MaNCC AS [Mã NCC], TenNhaCungCap AS [Tên Nhà Cung Cấp], DiaChi AS [Địa Chỉ], DienThoai AS [SĐT] FROM NhaCungCap";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.Fill(dt);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi khi tải danh sách nhà cung cấp: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return dt;
        }

        /// <summary>
        /// Thêm một nhà cung cấp mới vào cơ sở dữ liệu.
        /// </summary>
        /// <param name="ncc">Đối tượng NhaCungCap chứa thông tin cần thêm.</param>
        /// <returns>True nếu thêm thành công, False nếu thất bại.</returns>
        public bool ThemNhaCungCap(NhaCungCap ncc)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO NhaCungCap (MaNCC, TenNhaCungCap, DiaChi, DienThoai) VALUES (@MaNCC, @TenNhaCungCap, @DiaChi, @DienThoai)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaNCC", ncc.MaNCC);
                    cmd.Parameters.AddWithValue("@TenNhaCungCap", ncc.TenNhaCungCap);
                    cmd.Parameters.AddWithValue("@DiaChi", ncc.DiaChi);
                    cmd.Parameters.AddWithValue("@DienThoai", ncc.DienThoai);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    // Kiểm tra lỗi trùng khóa chính
                    if (ex.Number == 2627 || ex.Number == 2601) // Lỗi trùng khóa chính (PRIMARY KEY violation)
                    {
                        MessageBox.Show("Lỗi: Mã nhà cung cấp '" + ncc.MaNCC + "' đã tồn tại!", "Lỗi Trùng Mã", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Lỗi khi thêm nhà cung cấp: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// Cập nhật thông tin một nhà cung cấp trong cơ sở dữ liệu.
        /// </summary>
        /// <param name="ncc">Đối tượng NhaCungCap chứa thông tin cần cập nhật.</param>
        /// <returns>True nếu cập nhật thành công, False nếu thất bại.</returns>
        public bool SuaNhaCungCap(NhaCungCap ncc)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE NhaCungCap SET TenNhaCungCap = @TenNhaCungCap, DiaChi = @DiaChi, DienThoai = @DienThoai WHERE MaNCC = @MaNCC";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TenNhaCungCap", ncc.TenNhaCungCap);
                    cmd.Parameters.AddWithValue("@DiaChi", ncc.DiaChi);
                    cmd.Parameters.AddWithValue("@DienThoai", ncc.DienThoai);
                    cmd.Parameters.AddWithValue("@MaNCC", ncc.MaNCC); // Quan trọng: Điều kiện WHERE

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật nhà cung cấp: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        /// <summary>
        /// Xóa một nhà cung cấp khỏi cơ sở dữ liệu dựa vào Mã NCC.
        /// </summary>
        /// <param name="maNCC">Mã nhà cung cấp cần xóa.</param>
        /// <returns>True nếu xóa thành công, False nếu thất bại.</returns>
        public bool XoaNhaCungCap(string maNCC)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM NhaCungCap WHERE MaNCC = @MaNCC";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaNCC", maNCC);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (SqlException ex)
                {
                    // Kiểm tra lỗi ràng buộc khóa ngoại nếu có
                    if (ex.Number == 547) // Foreign key constraint violation
                    {
                        MessageBox.Show("Không thể xóa nhà cung cấp '" + maNCC + "' vì có dữ liệu liên quan (ví dụ: trong bảng sản phẩm hoặc hóa đơn).", "Lỗi Ràng Buộc Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Lỗi khi xóa nhà cung cấp: " + ex.Message, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return false;
                }
            }
        }
    }
}
