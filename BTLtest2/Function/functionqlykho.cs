using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BTLtest2.Class;
using DevExpress.Utils.Drawing.Animation;
using static BTLtest2.Class.Sach;

namespace BTLtest2.function
{
    internal class functionqlykho
    {
        //ket noi csdl
        const string connectionString = "Data Source=DESKTOP-VT5RUI9;Initial Catalog=laptrinh.net;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        static SqlConnection connection;
        //ham ket noi csdl
        public static void connect()
        {
            connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                Console.WriteLine("Ket noi thanh cong");
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Loi ket noi: " + ex.Message);
            }
        }
        public static void EnsureConnectionOpen()
        {
            if (connection == null)
                connection = new SqlConnection(connectionString);
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }
        public DataTable GetNeonNguForComboBox()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT MaNgonNgu, TenNgonNgu FROM dbo.NgonNgu";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }


        public DataTable GetNgonNguForComboBox()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT MaNgonNgu, TenNgonNgu FROM dbo.NgonNgu ORDER BY TenNgonNgu";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Lỗi SQL khi lấy Ngôn Ngữ: " + ex.Message);
                        // Xem xét việc ném lỗi hoặc hiển thị thông báo cho người dùng từ tầng gọi
                        // throw; // Hoặc trả về DataTable trống để form xử lý
                    }
                }
            }
            return dt;
        }

        public DataTable GetLoaiSachForComboBox()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Đảm bảo tên bảng và cột là chính xác
                string query = "SELECT MaLoaiSach, TenLoai FROM LoaiSach ORDER BY TenLoai";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Lỗi SQL khi lấy Loại Sách: " + ex.Message);
                        // throw; 
                    }
                }
            }
            return dt;
        }

        public DataTable GetTacGiaForComboBox()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Đảm bảo tên bảng và cột là chính xác
                string query = "SELECT MaTacGia, TenTacGia FROM dbo.TacGia ORDER BY TenTacGia";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Lỗi SQL khi lấy Tác Giả: " + ex.Message);
                        // throw;
                    }
                }
            }
            return dt;
        }

        public DataTable GetNXBForComboBox()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Đảm bảo tên bảng và cột là chính xác
                string query = "SELECT MaNXB, TenNXB FROM dbo.NhaXuatBan ORDER BY TenNXB";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Lỗi SQL khi lấy Nhà Xuất Bản: " + ex.Message);
                        // throw;
                    }
                }
            }
            return dt;
        }

        public DataTable GetLinhVucForComboBox()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Đảm bảo tên bảng và cột là chính xác
                string query = "SELECT MaLinhVuc, TenLinhVuc FROM dbo.LinhVuc ORDER BY TenLinhVuc";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Lỗi SQL khi lấy Lĩnh Vực: " + ex.Message);
                        // throw;
                    }
                }
            }
            return dt;
        }

        //phuong thuc them sach
        // Giả sử bạn có một chuỗi kết nối (connection string)
        // private static string YourConnectionString = "Chuỗi kết nối của bạn ở đây";

        /// <summary>
        /// Thêm một cuốn sách mới vào cơ sở dữ liệu.
        /// </summary>
        public static bool AddSach(khosach sachToAdd)
        {
            if (sachToAdd == null) return false;

            // 1. Kiểm tra MaSach đã tồn tại chưa?
            if (MaSachExists(sachToAdd.MaSach))
            {
                Console.WriteLine($"Lỗi: Mã sách '{sachToAdd.MaSach}' đã tồn tại.");
                // Thông báo cho người dùng: MessageBox.Show($"Mã sách '{sachToAdd.MaSach}' đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // 2. Kiểm tra các khóa ngoại (ví dụ)
            if (!ForeignKeyExists("LoaiSach", "MaLoaiSach", sachToAdd.MaLoaiSach))
            {
                Console.WriteLine($"Lỗi: Mã loại sách '{sachToAdd.MaLoaiSach}' không tồn tại.");
                return false;
            }
            if (!ForeignKeyExists("TacGia", "MaTacGia", sachToAdd.MaTacGia)) // Giả sử bảng tblTacGia, cột MaTacGia
            {
                Console.WriteLine($"Lỗi: Mã tác giả '{sachToAdd.MaTacGia}' không tồn tại.");
                return false;
            }
            // Thêm các kiểm tra tương tự cho MaNXB, MaLinhVuc, MaNgonNgu nếu cần

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO KhoSach
                                    (MaSach, TenSach, SoLuong, DonGiaNhap, DonGiaBan,
                                     MaLoaiSach, MaTacGia, MaNXB, MaLinhVuc, MaNgonNgu,
                                     Anh, SoTrang)
                                 VALUES
                                    (@MaSach, @TenSach, @SoLuong, @DonGiaNhap, @DonGiaBan,
                                     @MaLoaiSach, @MaTacGia, @MaNXB, @MaLinhVuc, @MaNgonNgu,
                                     @Anh, @SoTrang)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaSach", sachToAdd.MaSach);
                    command.Parameters.AddWithValue("@TenSach", sachToAdd.TenSach);
                    // ... (Thêm các parameters còn lại như trong các ví dụ trước) ...
                    command.Parameters.AddWithValue("@SoLuong", sachToAdd.SoLuong);
                    command.Parameters.AddWithValue("@DonGiaNhap", sachToAdd.DonGiaNhap);
                    command.Parameters.AddWithValue("@DonGiaBan", sachToAdd.DonGiaBan); // Lấy từ thuộc tính đã tính
                    command.Parameters.AddWithValue("@MaLoaiSach", sachToAdd.MaLoaiSach);
                    command.Parameters.AddWithValue("@MaTacGia", sachToAdd.MaTacGia);
                    command.Parameters.AddWithValue("@MaNXB", sachToAdd.MaNXB);
                    command.Parameters.AddWithValue("@MaLinhVuc", sachToAdd.MaLinhVuc);
                    command.Parameters.AddWithValue("@MaNgonNgu", sachToAdd.MaNgonNgu);
                    command.Parameters.AddWithValue("@Anh", sachToAdd.Anh);
                    command.Parameters.AddWithValue("@SoTrang", sachToAdd.SoTrang);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (SqlException ex)
                    {
                        // Lỗi 2627: Vi phạm ràng buộc UNIQUE KEY. (Có thể đã được kiểm tra bởi MaSachExists)
                        // Lỗi 2601: Vi phạm ràng buộc UNIQUE trong một index.
                        if (ex.Number == 2627 || ex.Number == 2601)
                        {
                            Console.WriteLine($"Lỗi SQL: Mã sách '{sachToAdd.MaSach}' có thể đã bị trùng lặp (unique constraint violation).");
                        }
                        else
                        {
                            Console.WriteLine("Lỗi SQL khi thêm sách: " + ex.Message);
                        }
                        return false;
                    }
                }
            }
        }


        public static bool UpdateSach(khosach sachToUpdate)
        {
            if (sachToUpdate == null || string.IsNullOrWhiteSpace(sachToUpdate.MaSach))
            {
                Console.WriteLine("Lỗi: Thông tin sách không hợp lệ để cập nhật.");
                return false;
            }

            // 1. Kiểm tra MaSach có tồn tại để cập nhật không? (Tùy chọn, thường thì nếu không tìm thấy sẽ không có dòng nào được update)
            // if (!MaSachExists(sachToUpdate.MaSach))
            // {
            //     Console.WriteLine($"Lỗi: Không tìm thấy Mã sách '{sachToUpdate.MaSach}' để cập nhật.");
            //     return false;
            // }

            // 2. Kiểm tra các khóa ngoại (ví dụ)
            if (!ForeignKeyExists("LoaiSach", "MaLoaiSach", sachToUpdate.MaLoaiSach))
            {
                Console.WriteLine($"Lỗi: Mã loại sách '{sachToUpdate.MaLoaiSach}' không tồn tại.");
                return false;
            }
            if (!ForeignKeyExists("TacGia", "MaTacGia", sachToUpdate.MaTacGia))
            {
                Console.WriteLine($"Lỗi: Mã tác giả '{sachToUpdate.MaTacGia}' không tồn tại.");
                return false;
            }
            // Thêm các kiểm tra tương tự cho MaNXB, MaLinhVuc, MaNgonNgu nếu cần

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"UPDATE KhoSach SET
                                    TenSach = @TenSach, SoLuong = @SoLuong, DonGiaNhap = @DonGiaNhap, DonGiaBan = @DonGiaBan,
                                    MaLoaiSach = @MaLoaiSach, MaTacGia = @MaTacGia, MaNXB = @MaNXB,
                                    MaLinhVuc = @MaLinhVuc, MaNgonNgu = @MaNgonNgu, Anh = @Anh, SoTrang = @SoTrang
                                 WHERE MaSach = @MaSach";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // ... (Thêm các parameters như trong AddSach) ...
                    command.Parameters.AddWithValue("@TenSach", sachToUpdate.TenSach);
                    command.Parameters.AddWithValue("@SoLuong", sachToUpdate.SoLuong);
                    command.Parameters.AddWithValue("@DonGiaNhap", sachToUpdate.DonGiaNhap);
                    command.Parameters.AddWithValue("@DonGiaBan", sachToUpdate.DonGiaBan);
                    command.Parameters.AddWithValue("@MaLoaiSach", sachToUpdate.MaLoaiSach);
                    command.Parameters.AddWithValue("@MaTacGia", sachToUpdate.MaTacGia);
                    command.Parameters.AddWithValue("@MaNXB", sachToUpdate.MaNXB);
                    command.Parameters.AddWithValue("@MaLinhVuc", sachToUpdate.MaLinhVuc);
                    command.Parameters.AddWithValue("@MaNgonNgu", sachToUpdate.MaNgonNgu);
                    command.Parameters.AddWithValue("@Anh", sachToUpdate.Anh);
                    command.Parameters.AddWithValue("@SoTrang", sachToUpdate.SoTrang);
                    command.Parameters.AddWithValue("@MaSach", sachToUpdate.MaSach);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Lỗi SQL khi cập nhật sách: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public static bool DeleteSach(string maSachToDelete)
        {
            if (string.IsNullOrWhiteSpace(maSachToDelete))
            {
                Console.WriteLine("Lỗi: Mã sách không hợp lệ để xóa.");
                return false;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM KhoSach WHERE MaSach = @MaSach";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaSach", maSachToDelete);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (SqlException ex)
                    {
                        // Lỗi 547: Vi phạm ràng buộc FOREIGN KEY.
                        if (ex.Number == 547)
                        {
                            Console.WriteLine($"Không thể xóa sách '{maSachToDelete}' vì nó đang được tham chiếu bởi dữ liệu khác (ví dụ: chi tiết hóa đơn).");
                            //MessageBox.Show("Không thể xóa sách này vì đang được sử dụng!", "Lỗi ràng buộc", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            Console.WriteLine("Lỗi SQL khi xóa sách: " + ex.Message);
                        }
                        return false;
                    }
                }
            }
        }
        public static List<khosach> GetSach()
        {

            List<khosach> sachList = new List<khosach>();
            // Câu lệnh SQL được cập nhật để tính tổng số lượng đã bán từ ChiTietHDBan
            // Sử dụng LEFT JOIN để vẫn lấy được sách dù chưa bán được cuốn nào (TongLuongBan sẽ là 0)
            // Đảm bảo tên cột SoLuong trong ChiTietHDBan là chính xác. Nếu khác, hãy thay đổi ct.SoLuong.
            string query = @"
                SELECT
                    ks.MaSach,
                    ks.TenSach,
                    ks.SoLuong AS SoLuongTonKho, -- Đổi tên alias để tránh nhầm lẫn nếu ChiTietHDBan cũng có cột SoLuong
                    ks.DonGiaNhap,
                    -- ks.DonGiaBan, -- DonGiaBan được tính trong class khosach
                    ks.MaLoaiSach,
                    ks.MaTacGia,
                    ks.MaNXB,
                    ks.MaLinhVuc,
                    ks.MaNgonNgu,
                    ks.Anh,
                    ks.SoTrang,
                    COALESCE(SUM(ct.SLBan), 0) AS TongLuongBanDaTinh
                FROM
                    KhoSach ks
                LEFT JOIN
                    ChiTietHDBan ct ON ks.MaSach = ct.MaSach
                GROUP BY
                    ks.MaSach,
                    ks.TenSach,
                    ks.SoLuong,
                    ks.DonGiaNhap,
                    -- ks.DonGiaBan,
                    ks.MaLoaiSach,
                    ks.MaTacGia,
                    ks.MaNXB,
                    ks.MaLinhVuc,
                    ks.MaNgonNgu,
                    ks.Anh,
                    ks.SoTrang
                ORDER BY
                    ks.MaSach; -- Tùy chọn: thêm ORDER BY nếu muốn danh sách được sắp xếp";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("Không có dữ liệu sách nào trong bảng KhoSach.");
                        }
                        while (reader.Read())
                        {
                            khosach sach = new khosach
                            {
                                MaSach = reader["MaSach"].ToString(),
                                TenSach = reader["TenSach"].ToString(),
                                SoLuong = int.Parse(reader["SoLuongTonKho"].ToString()), // Đọc từ alias SoLuongTonKho
                                DonGiaNhap = decimal.Parse(reader["DonGiaNhap"].ToString()),
                                // DonGiaBan sẽ được tự động tính trong class khosach
                                MaLoaiSach = reader["MaLoaiSach"].ToString(),
                                MaTacGia = reader["MaTacGia"].ToString(),
                                MaNXB = reader["MaNXB"].ToString(),
                                MaLinhVuc = reader["MaLinhVuc"].ToString(),
                                MaNgonNgu = reader["MaNgonNgu"].ToString(),
                                Anh = reader["Anh"].ToString(),
                                SoTrang = int.Parse(reader["SoTrang"].ToString()),
                                // Gán giá trị TongLuongBanDaTinh đã được tính từ SQL
                                LuongBanDaTinh = int.Parse(reader["TongLuongBanDaTinh"].ToString())
                            };
                            sachList.Add(sach);
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Lỗi SQL khi lấy danh sách sách: " + ex.Message);
                        MessageBox.Show("Lỗi SQL khi tải dữ liệu sách: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (FormatException fx)
                    {
                        Console.WriteLine("Lỗi định dạng dữ liệu khi đọc sách: " + fx.Message);
                        MessageBox.Show("Lỗi định dạng dữ liệu khi tải sách: " + fx.Message, "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    // Không cần reader.Close() vì khối using của SqlDataReader sẽ tự động đóng khi kết thúc
                }
            }
            Console.WriteLine($"GetSach() trả về {sachList.Count} cuốn sách.");
            return sachList;
        }

            public static bool MaSachExists(string maSach)
            {
                if (string.IsNullOrWhiteSpace(maSach)) return false;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT COUNT(1) FROM KhoSach WHERE MaSach = @MaSach";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MaSach", maSach);
                        try
                        {
                            connection.Open();
                            int count = (int)command.ExecuteScalar();
                            return count > 0;
                        }
                        catch (SqlException ex)
                        {
                            Console.WriteLine("Lỗi SQL khi kiểm tra MaSachExists: " + ex.Message);
                            // Xem xét ném lỗi hoặc trả về true để ngăn thao tác nếu không chắc chắn
                            return true; // An toàn hơn là cho rằng nó tồn tại nếu có lỗi DB
                        }
                    }
                }
            }
        
            // const string connectionString = "..."; // Giữ nguyên connectionString của bạn
            // ... (các phương thức connect, EnsureConnectionOpen, AddSach, UpdateSach, DeleteSach, GetSach, MaSachExists, ForeignKeyExists đã có) ...

            /// <summary>
            /// Tìm kiếm sách dựa trên các tiêu chí khác nhau.
            /// </summary>
            /// <param name="tuKhoaMaSach">Từ khóa tìm theo Mã Sách (tìm gần đúng nếu không rỗng).</param>
            /// <param name="tuKhoaTenSach">Từ khóa tìm theo Tên Sách (tìm gần đúng nếu không rỗng).</param>
            /// <param name="maLoaiSachFilter">Lọc theo Mã Loại Sách (tìm chính xác nếu không rỗng).</param>
            /// <param name="maTacGiaFilter">Lọc theo Mã Tác Giả (tìm chính xác nếu không rỗng).</param>
            /// <param name="maNXBFilter">Lọc theo Mã Nhà Xuất Bản (tìm chính xác nếu không rỗng).</param>
            /// <returns>Danh sách các cuốn sách phù hợp.</returns>
            public static List<khosach> TimKiemSach(string tuKhoaMaSach, string tuKhoaTenSach, string maLoaiSachFilter, string maTacGiaFilter, string maNXBFilter)
            {
                List<khosach> sachList = new List<khosach>();
                List<string> conditions = new List<string>();
                List<SqlParameter> parameters = new List<SqlParameter>();

                // Xây dựng câu truy vấn cơ bản
                // Chọn các cột cần thiết để tạo đối tượng khosach, bao gồm DonGiaNhap để DonGiaBan được tính
                string baseQuery = "SELECT MaSach, TenSach, SoLuong, DonGiaNhap, MaLoaiSach, MaTacGia, MaNXB, MaLinhVuc, MaNgonNgu, Anh, SoTrang FROM KhoSach";

                // Thêm điều kiện tìm kiếm nếu tham số không rỗng
                if (!string.IsNullOrWhiteSpace(tuKhoaMaSach))
                {
                    conditions.Add("MaSach LIKE @MaSachParam");
                    parameters.Add(new SqlParameter("@MaSachParam", "%" + tuKhoaMaSach.Trim() + "%"));
                }

                if (!string.IsNullOrWhiteSpace(tuKhoaTenSach))
                {
                    conditions.Add("TenSach LIKE @TenSachParam"); // Sử dụng NVARCHAR cho TenSach nên không cần N''
                    parameters.Add(new SqlParameter("@TenSachParam", "%" + tuKhoaTenSach.Trim() + "%"));
                }

                //if (!string.IsNullOrWhiteSpace(maLoaiSachFilter))
                //{
                //    conditions.Add("MaLoaiSach = @MaLoaiSachParam");
                //    parameters.Add(new SqlParameter("@MaLoaiSachParam", maLoaiSachFilter.Trim()));
                //}

                //if (!string.IsNullOrWhiteSpace(maTacGiaFilter))
                //{
                //    conditions.Add("MaTacGia = @MaTacGiaParam");
                //    parameters.Add(new SqlParameter("@MaTacGiaParam", maTacGiaFilter.Trim()));
                //}

                //if (!string.IsNullOrWhiteSpace(maNXBFilter))
                //{
                //    conditions.Add("MaNXB = @MaNXBParam");
                //    parameters.Add(new SqlParameter("@MaNXBParam", maNXBFilter.Trim()));
                //}

                // Nối các điều kiện vào câu truy vấn
                string finalQuery = baseQuery;
                if (conditions.Count > 0)
                {
                    finalQuery += " WHERE " + string.Join(" AND ", conditions);
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(finalQuery, connection))
                    {
                        // Thêm các tham số đã tạo vào command
                        if (parameters.Count > 0)
                        {
                            command.Parameters.AddRange(parameters.ToArray());
                        }

                        try
                        {
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                khosach sach = new khosach // Sử dụng tên lớp khosach mà bạn đang dùng
                                {
                                    MaSach = reader["MaSach"].ToString(),
                                    TenSach = reader["TenSach"].ToString(),
                                    SoLuong = int.Parse(reader["SoLuong"].ToString()),
                                    DonGiaNhap = decimal.Parse(reader["DonGiaNhap"].ToString()),
                                    // DonGiaBan sẽ được tự động tính trong class khosach
                                    MaLoaiSach = reader["MaLoaiSach"].ToString(),
                                    MaTacGia = reader["MaTacGia"].ToString(),
                                    MaNXB = reader["MaNXB"].ToString(),
                                    MaLinhVuc = reader["MaLinhVuc"].ToString(),
                                    MaNgonNgu = reader["MaNgonNgu"].ToString(),
                                    Anh = reader["Anh"].ToString(),
                                    SoTrang = int.Parse(reader["SoTrang"].ToString())
                                };
                                sachList.Add(sach);
                            }
                        }
                        catch (SqlException ex)
                        {
                            Console.WriteLine("Lỗi SQL khi tìm kiếm sách: " + ex.Message);
                            MessageBox.Show("Lỗi SQL khi tìm kiếm sách: " + ex.Message, "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (FormatException fx)
                        {
                            Console.WriteLine("Lỗi định dạng dữ liệu khi đọc sách (tìm kiếm): " + fx.Message);
                            MessageBox.Show("Lỗi định dạng dữ liệu khi đọc sách (tìm kiếm): " + fx.Message, "Lỗi Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                Console.WriteLine($"TimKiemSach() trả về {sachList.Count} cuốn sách.");
                return sachList;
            }
            public static bool ForeignKeyExists(string tableName, string columnName, string value)
            {
                if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(tableName) || string.IsNullOrWhiteSpace(columnName))
                {
                    // Nếu giá trị khóa ngoại được phép null/empty và đó là hợp lệ, bạn có thể trả về true.
                    // Ở đây, giả sử nếu giá trị là null/empty thì không cần check và coi như không hợp lệ nếu trường đó bắt buộc.
                    // Hoặc, nếu trường khóa ngoại đó không bắt buộc (cho phép NULL), bạn có thể trả về true nếu value là null/empty.
                    // Ví dụ: Nếu MaTacGia được phép NULL, và value của MaTacGia là NULL, thì nên trả về true.
                    // Đoạn code này cần tùy chỉnh theo logic nghiệp vụ của bạn về việc FK có được phép NULL không.
                    // Hiện tại, nếu value rỗng, coi như không tồn tại để chặt chẽ.
                    return false;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Chú ý: Cần cẩn thận với việc truyền trực tiếp tableName và columnName vào query nếu chúng đến từ nguồn không tin cậy.
                    // Tuy nhiên, trong trường hợp này, chúng thường là hằng số từ code.
                    string query = $"SELECT COUNT(1) FROM {tableName} WHERE {columnName} = @Value";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Value", value);
                        try
                        {
                            connection.Open();
                            int count = (int)command.ExecuteScalar();
                            return count > 0;
                        }
                        catch (SqlException ex)
                        {
                            Console.WriteLine($"Lỗi SQL khi kiểm tra ForeignKeyExists ({tableName}.{columnName}): " + ex.Message);
                            return false; // Coi như không tồn tại nếu có lỗi DB
                        }
                    }
                }
            }
        }
    }

