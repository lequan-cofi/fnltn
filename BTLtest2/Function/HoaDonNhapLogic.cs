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
    public class HoaDonNhapLogic
    {
        private string connectionString = "Data Source=DESKTOP-VT5RUI9;Initial Catalog=laptrinh.net;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"; // Thay thế bằng chuỗi kết nối thực tế của bạn
       
        // Các hàm khác của bạn như AddHoaDonNhap (nếu có), GetHoaDonNhapById...

        /// <summary>
        /// Lấy danh sách chi tiết hóa đơn nhập cho một số hóa đơn cụ thể (dùng trong transaction).
        /// </summary>
        private List<ChiTietHDNhap> GetChiTietHDNhapListTrongTransaction(string soHDNhap, SqlConnection con, SqlTransaction transaction)
        {
            List<ChiTietHDNhap> list = new List<ChiTietHDNhap>();
            string query = @"SELECT ct.SoHDNhap, ct.MaSach, ks.TenSach, ct.SLNhap, ct.DonGiaNhap, ct.KhuyenMai, ct.ThanhTien
                             FROM dbo.ChiTietHDNhap ct
                             JOIN dbo.KhoSach ks ON ct.MaSach = ks.MaSach
                             WHERE ct.SoHDNhap = @SoHDNhap;";
            using (SqlCommand cmd = new SqlCommand(query, con, transaction))
            {
                cmd.Parameters.AddWithValue("@SoHDNhap", soHDNhap);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ChiTietHDNhap
                        {
                            SoHDNhap = reader["SoHDNhap"].ToString(),
                            MaSach = reader["MaSach"].ToString(),
                            TenSach = reader["TenSach"].ToString(),
                            SLNhap = Convert.ToInt32(reader["SLNhap"]),
                            DonGiaNhap = Convert.ToSingle(reader["DonGiaNhap"]),
                            KhuyenMai = Convert.ToSingle(reader["KhuyenMai"]),
                            ThanhTien = Convert.ToSingle(reader["ThanhTien"])
                        });
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Xóa tất cả chi tiết hóa đơn nhập theo Số Hóa Đơn (dùng trong transaction).
        /// </summary>
        private bool DeleteAllChiTietHDNhapBySoHD(string soHDNhap, SqlConnection con, SqlTransaction transaction)
        {
            string query = "DELETE FROM dbo.ChiTietHDNhap WHERE SoHDNhap = @SoHDNhap";
            using (SqlCommand cmd = new SqlCommand(query, con, transaction))
            {
                cmd.Parameters.AddWithValue("@SoHDNhap", soHDNhap);
                // ExecuteNonQuery trả về số dòng bị ảnh hưởng. >= 0 có nghĩa là lệnh chạy không lỗi.
                return cmd.ExecuteNonQuery() >= 0;
            }
        }

        public bool UpdateHoaDonNhap(HoaDonNhap hdn)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    // 1. Cập nhật thông tin chính (header) của hóa đơn
                    string queryHD = "UPDATE dbo.HoaDonNhap SET MaNhanVien = @MaNhanVien, NgayNhap = @NgayNhap, MaNCC = @MaNCC, TongTien = @TongTien WHERE SoHDNhap = @SoHDNhap";
                    using (SqlCommand cmdHD = new SqlCommand(queryHD, con, transaction))
                    {
                        cmdHD.Parameters.AddWithValue("@SoHDNhap", hdn.SoHDNhap);
                        cmdHD.Parameters.AddWithValue("@MaNhanVien", hdn.MaNhanVien);
                        cmdHD.Parameters.AddWithValue("@NgayNhap", hdn.NgayNhap);
                        cmdHD.Parameters.AddWithValue("@MaNCC", hdn.MaNCC);
                        cmdHD.Parameters.AddWithValue("@TongTien", hdn.TongTien);
                        cmdHD.ExecuteNonQuery();
                    }

                    // 2. Lấy danh sách chi tiết hóa đơn CŨ từ CSDL để hoàn tác kho
                    List<ChiTietHDNhap> oldChiTietList = GetChiTietHDNhapListTrongTransaction(hdn.SoHDNhap, con, transaction);

                    // 3. Hoàn tác số lượng trong KhoSach dựa trên ChiTietHDNhap CŨ
                    foreach (var oldCt in oldChiTietList)
                    {
                        if (!UpdateKhoSachQuantity(oldCt.MaSach, -oldCt.SLNhap, 0, 0, con, transaction, false))
                        {
                            // Nếu hoàn tác kho thất bại, ném lỗi để rollback transaction
                            throw new Exception($"Lỗi khi hoàn tác số lượng kho cho sản phẩm {oldCt.MaSach} của hóa đơn {hdn.SoHDNhap}.");
                        }
                    }

                    // 4. Xóa tất cả ChiTietHDNhap CŨ cho SoHDNhap này khỏi CSDL
                    if (!DeleteAllChiTietHDNhapBySoHD(hdn.SoHDNhap, con, transaction))
                    {
                        throw new Exception($"Lỗi khi xóa chi tiết cũ của hóa đơn {hdn.SoHDNhap}.");
                    }

                    // 5. Thêm lại tất cả ChiTietHDNhap MỚI từ danh sách hdn.ChiTiet
                    foreach (var newCt in hdn.ChiTiet)
                    {
                        newCt.SoHDNhap = hdn.SoHDNhap; // Đảm bảo SoHDNhap đúng
                        if (!AddChiTietHDNhap(newCt, con, transaction, true))
                        {
                            // Nếu thêm một chi tiết mới thất bại (có thể do UpdateKhoSachQuantity bên trong nó thất bại),
                            // ném lỗi để rollback toàn bộ transaction.
                            throw new Exception($"Lỗi khi thêm chi tiết mới cho sản phẩm {newCt.MaSach} vào hóa đơn {hdn.SoHDNhap}.");
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Lỗi khi cập nhật Hóa Đơn Nhập ({hdn?.SoHDNhap}): {ex.ToString()}");
                    return false;
                }
            }
        }

        // Hàm AddChiTietHDNhap (phiên bản dùng trong transaction và có cờ updateKho)
        public bool AddChiTietHDNhap(ChiTietHDNhap cthdn, SqlConnection con, SqlTransaction transaction, bool updateKho)
        {
            string query = "INSERT INTO dbo.ChiTietHDNhap (SoHDNhap, MaSach, SLNhap, DonGiaNhap, KhuyenMai, ThanhTien) VALUES (@SoHDNhap, @MaSach, @SLNhap, @DonGiaNhap, @KhuyenMai, @ThanhTien)";
            using (SqlCommand cmd = new SqlCommand(query, con, transaction))
            {
                cmd.Parameters.AddWithValue("@SoHDNhap", cthdn.SoHDNhap);
                cmd.Parameters.AddWithValue("@MaSach", cthdn.MaSach);
                cmd.Parameters.AddWithValue("@SLNhap", cthdn.SLNhap);
                cmd.Parameters.AddWithValue("@DonGiaNhap", cthdn.DonGiaNhap);
                cmd.Parameters.AddWithValue("@KhuyenMai", cthdn.KhuyenMai);
                cmd.Parameters.AddWithValue("@ThanhTien", cthdn.ThanhTien);
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0 && updateKho)
                {
                    float newDonGiaBan = (float)(cthdn.DonGiaNhap * 1.10);
                    // Nếu UpdateKhoSachQuantity thất bại, AddChiTietHDNhap sẽ trả về false
                    return UpdateKhoSachQuantity(cthdn.MaSach, cthdn.SLNhap, cthdn.DonGiaNhap, newDonGiaBan, con, transaction, true);
                }
                return rowsAffected > 0;
            }
        }

        // Hàm UpdateKhoSachQuantity (phiên bản dùng trong transaction)
        public bool UpdateKhoSachQuantity(string maSach, int quantityChange, float newDonGiaNhap, float newDonGiaBan, SqlConnection con, SqlTransaction transaction, bool updatePrices)
        {
            int currentQuantity = 0;
            string querySelect = "SELECT SoLuong FROM dbo.KhoSach WHERE MaSach = @MaSach";
            using (SqlCommand cmdSelect = new SqlCommand(querySelect, con, transaction))
            {
                cmdSelect.Parameters.AddWithValue("@MaSach", maSach);
                object result = cmdSelect.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    currentQuantity = Convert.ToInt32(result);
                }
                else
                {
                    Console.WriteLine($"Lỗi: Sản phẩm {maSach} không tìm thấy trong KhoSach để cập nhật số lượng.");
                    return false;
                }
            }

            int updatedQuantity = currentQuantity + quantityChange;
            if (updatedQuantity < 0)
            {
                Console.WriteLine($"Lỗi: Số lượng tồn kho của sản phẩm {maSach} không thể âm ({updatedQuantity}).");
                return false;
            }

            string queryUpdate;
            // Chỉ cập nhật giá nếu updatePrices là true VÀ đang thêm số lượng (quantityChange > 0)
            if (updatePrices && quantityChange > 0)
            {
                queryUpdate = "UPDATE dbo.KhoSach SET SoLuong = @SoLuong, DonGiaNhap = @DonGiaNhap, DonGiaBan = @DonGiaBan WHERE MaSach = @MaSach";
            }
            else
            {
                queryUpdate = "UPDATE dbo.KhoSach SET SoLuong = @SoLuong WHERE MaSach = @MaSach";
            }

            using (SqlCommand cmdUpdate = new SqlCommand(queryUpdate, con, transaction))
            {
                cmdUpdate.Parameters.AddWithValue("@MaSach", maSach);
                cmdUpdate.Parameters.AddWithValue("@SoLuong", updatedQuantity);
                if (updatePrices && quantityChange > 0)
                {
                    cmdUpdate.Parameters.AddWithValue("@DonGiaNhap", newDonGiaNhap);
                    cmdUpdate.Parameters.AddWithValue("@DonGiaBan", newDonGiaBan);
                }
                return cmdUpdate.ExecuteNonQuery() > 0;
            }
        }

        // Hàm AddHoaDonNhap (bạn cần có hàm này để thêm mới hóa đơn)
        public bool AddHoaDonNhap(HoaDonNhap hdn)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    string queryHD = "INSERT INTO dbo.HoaDonNhap (SoHDNhap, MaNhanVien, NgayNhap, MaNCC, TongTien) VALUES (@SoHDNhap, @MaNhanVien, @NgayNhap, @MaNCC, @TongTien)";
                    using (SqlCommand cmdHD = new SqlCommand(queryHD, con, transaction))
                    {
                        cmdHD.Parameters.AddWithValue("@SoHDNhap", hdn.SoHDNhap);
                        cmdHD.Parameters.AddWithValue("@MaNhanVien", hdn.MaNhanVien);
                        cmdHD.Parameters.AddWithValue("@NgayNhap", hdn.NgayNhap);
                        cmdHD.Parameters.AddWithValue("@MaNCC", hdn.MaNCC);
                        cmdHD.Parameters.AddWithValue("@TongTien", hdn.TongTien);
                        cmdHD.ExecuteNonQuery();
                    }

                    foreach (var ct in hdn.ChiTiet)
                    {
                        ct.SoHDNhap = hdn.SoHDNhap; // Đảm bảo SoHDNhap cho chi tiết
                        if (!AddChiTietHDNhap(ct, con, transaction, true))
                        {
                            throw new Exception($"Lỗi khi thêm chi tiết cho sản phẩm {ct.MaSach} vào hóa đơn {hdn.SoHDNhap}.");
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Lỗi khi thêm Hóa Đơn Nhập ({hdn?.SoHDNhap}): {ex.ToString()}");
                    return false;
                }
            }
        }


        public bool DeleteHoaDonNhap(string soHDNhap)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    List<ChiTietHDNhap> detailsToDelete = GetChiTietHDNhapListTrongTransaction(soHDNhap, con, transaction);
                    foreach (var ct in detailsToDelete)
                    {
                        if (!UpdateKhoSachQuantity(ct.MaSach, -ct.SLNhap, 0, 0, con, transaction, false))
                        {
                            throw new Exception($"Lỗi khi hoàn tác kho cho sản phẩm {ct.MaSach} khi xóa hóa đơn {soHDNhap}.");
                        }
                    }

                    if (!DeleteAllChiTietHDNhapBySoHD(soHDNhap, con, transaction))
                    {
                        throw new Exception($"Lỗi khi xóa chi tiết của hóa đơn {soHDNhap}.");
                    }

                    string queryHD = "DELETE FROM dbo.HoaDonNhap WHERE SoHDNhap = @SoHDNhap";
                    using (SqlCommand cmdHD = new SqlCommand(queryHD, con, transaction))
                    {
                        cmdHD.Parameters.AddWithValue("@SoHDNhap", soHDNhap);
                        if (cmdHD.ExecuteNonQuery() <= 0 && detailsToDelete.Count > 0) // Nếu có chi tiết mà không xóa được header thì lỗi
                        {
                            // Hoặc nếu không có chi tiết mà cũng không xóa được header (nếu hóa đơn phải tồn tại)
                            // throw new Exception($"Lỗi khi xóa header của hóa đơn {soHDNhap}.");
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Lỗi khi xóa Hóa Đơn Nhập ({soHDNhap}): {ex.ToString()}");
                    return false;
                }
            }
        }

        public List<ChiTietHDNhap> GetChiTietHDNhapList(string soHDNhap)
        {
            List<ChiTietHDNhap> list = new List<ChiTietHDNhap>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT ct.SoHDNhap, ct.MaSach, ks.TenSach, ct.SLNhap, ct.DonGiaNhap, ct.KhuyenMai, ct.ThanhTien
                                 FROM dbo.ChiTietHDNhap ct
                                 JOIN dbo.KhoSach ks ON ct.MaSach = ks.MaSach
                                 WHERE ct.SoHDNhap = @SoHDNhap;";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@SoHDNhap", soHDNhap);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ChiTietHDNhap
                            {
                                SoHDNhap = reader["SoHDNhap"].ToString(),
                                MaSach = reader["MaSach"].ToString(),
                                TenSach = reader["TenSach"].ToString(),
                                SLNhap = Convert.ToInt32(reader["SLNhap"]),
                                DonGiaNhap = Convert.ToSingle(reader["DonGiaNhap"]),
                                KhuyenMai = Convert.ToSingle(reader["KhuyenMai"]),
                                ThanhTien = Convert.ToSingle(reader["ThanhTien"])
                            });
                        }
                    }
                }
            }
            return list;
        }

        // Hàm GetHoaDonNhapById (cần thiết để lấy thông tin hóa đơn khi click trên DataGridView)
        public HoaDonNhap GetHoaDonNhapById(string soHDNhap)
        {
            HoaDonNhap hdn = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Câu lệnh SQL để lấy thông tin chung của hóa đơn, có thể JOIN với NhanVien, NhaCungCap nếu cần hiển thị tên
                string query = @"SELECT h.SoHDNhap, h.NgayNhap, h.MaNhanVien, nv.TenNhanVien, 
                                h.MaNCC, ncc.TenNhaCungCap, ncc.DiaChi AS DiaChiNCC, ncc.DienThoai AS DienThoaiNCC, h.TongTien
                         FROM dbo.HoaDonNhap h
                         LEFT JOIN dbo.NhanVien nv ON h.MaNhanVien = nv.MaNhanVien 
                         LEFT JOIN dbo.NhaCungCap ncc ON h.MaNCC = ncc.MaNCC
                         WHERE h.SoHDNhap = @SoHDNhap";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@SoHDNhap", soHDNhap);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            hdn = new HoaDonNhap
                            {
                                SoHDNhap = reader["SoHDNhap"].ToString(),
                                NgayNhap = Convert.ToDateTime(reader["NgayNhap"]),
                                MaNhanVien = reader["MaNhanVien"].ToString(),
                                TenNhanVien = reader["TenNhanVien"] != DBNull.Value ? reader["TenNhanVien"].ToString() : string.Empty,
                                MaNCC = reader["MaNCC"].ToString(),
                                TenNhaCungCap = reader["TenNhaCungCap"] != DBNull.Value ? reader["TenNhaCungCap"].ToString() : string.Empty,
                                DiaChiNCC = reader["DiaChiNCC"] != DBNull.Value ? reader["DiaChiNCC"].ToString() : string.Empty,
                                DienThoaiNCC = reader["DienThoaiNCC"] != DBNull.Value ? reader["DienThoaiNCC"].ToString() : string.Empty,
                                TongTien = Convert.ToSingle(reader["TongTien"])
                            };
                        }
                    }
                }
                if (hdn != null)
                {
                    // Sau khi đọc header, đóng DataReader hiện tại trước khi gọi GetChiTietHDNhapList
                    // Hoặc sử dụng MARS=true trong connection string, hoặc lấy chi tiết trong một kết nối riêng.
                    // Cách đơn giản nhất là gọi GetChiTietHDNhapList sau khi DataReader của header đã đóng.
                    // (Tuy nhiên, GetChiTietHDNhapList đang mở kết nối riêng nên không sao)
                    hdn.ChiTiet = GetChiTietHDNhapList(soHDNhap);
                }
            }
            return hdn;
        }


        public bool UpdateChiTietHDNhap(ChiTietHDNhap cthdn, int oldSLNhap, float oldDonGiaNhap)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    if (!UpdateKhoSachQuantity(cthdn.MaSach, -oldSLNhap, 0, 0, con, transaction, false))
                    {
                        throw new Exception($"Lỗi hoàn tác kho cho sản phẩm {cthdn.MaSach} khi cập nhật chi tiết.");
                    }

                    string query = "UPDATE dbo.ChiTietHDNhap SET SLNhap = @SLNhap, DonGiaNhap = @DonGiaNhap, KhuyenMai = @KhuyenMai, ThanhTien = @ThanhTien WHERE SoHDNhap = @SoHDNhap AND MaSach = @MaSach";
                    using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@SoHDNhap", cthdn.SoHDNhap);
                        cmd.Parameters.AddWithValue("@MaSach", cthdn.MaSach);
                        cmd.Parameters.AddWithValue("@SLNhap", cthdn.SLNhap);
                        cmd.Parameters.AddWithValue("@DonGiaNhap", cthdn.DonGiaNhap);
                        cmd.Parameters.AddWithValue("@KhuyenMai", cthdn.KhuyenMai);
                        cmd.Parameters.AddWithValue("@ThanhTien", cthdn.ThanhTien);
                        cmd.ExecuteNonQuery();
                    }

                    float newDonGiaBan = (float)(cthdn.DonGiaNhap * 1.10);
                    if (!UpdateKhoSachQuantity(cthdn.MaSach, cthdn.SLNhap, cthdn.DonGiaNhap, newDonGiaBan, con, transaction, true))
                    {
                        throw new Exception($"Lỗi cập nhật kho cho sản phẩm {cthdn.MaSach} sau khi cập nhật chi tiết.");
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Lỗi khi cập nhật Chi Tiết Hóa Đơn Nhập ({cthdn?.SoHDNhap} - {cthdn?.MaSach}): {ex.ToString()}");
                    return false;
                }
            }
        }

        public bool DeleteChiTietHDNhap(string soHDNhap, string maSach, int slNhapToRemove)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    string query = "DELETE FROM dbo.ChiTietHDNhap WHERE SoHDNhap = @SoHDNhap AND MaSach = @MaSach";
                    using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@SoHDNhap", soHDNhap);
                        cmd.Parameters.AddWithValue("@MaSach", maSach);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            if (!UpdateKhoSachQuantity(maSach, -slNhapToRemove, 0, 0, con, transaction, false))
                            {
                                throw new Exception($"Lỗi hoàn tác kho cho sản phẩm {maSach} khi xóa chi tiết hóa đơn {soHDNhap}.");
                            }
                            transaction.Commit();
                            return true;
                        }
                        else
                        {
                            // Không có dòng nào bị xóa, có thể coi là lỗi hoặc không tùy logic
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Lỗi khi xóa Chi Tiết Hóa Đơn Nhập ({soHDNhap} - {maSach}): {ex.ToString()}");
                    return false;
                }
            }
        }

        public KhoSach GetKhoSachById(string maSach)
        {
            KhoSach ks = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT MaSach, TenSach, SoLuong, DonGiaNhap, DonGiaBan FROM dbo.KhoSach WHERE MaSach = @MaSach";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MaSach", maSach);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ks = new KhoSach
                            {
                                MaSach = reader["MaSach"].ToString(),
                                TenSach = reader["TenSach"].ToString(),
                                SoLuong = Convert.ToInt32(reader["SoLuong"]),
                                DonGiaNhap = reader["DonGiaNhap"] != DBNull.Value ? Convert.ToSingle(reader["DonGiaNhap"]) : 0,
                                DonGiaBan = reader["DonGiaBan"] != DBNull.Value ? Convert.ToSingle(reader["DonGiaBan"]) : 0
                            };
                        }
                    }
                }
            }
            return ks;
        }

        public DataTable GetNhanVienForComboBox()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT MaNhanVien, TenNhanVien FROM dbo.NhanVien";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public DataTable GetNhaCungCapForComboBox()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT MaNCC, TenNhaCungCap FROM dbo.NhaCungCap";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        // Giả sử bạn có class NhaCungCaphdn trong BTLtest2.Class
        // Nếu tên class thực tế là NhaCungCap, hãy đổi NhaCungCaphdn thành NhaCungCap ở đây.
        public NhaCungCaphdn GetNhaCungCapDetails(string maNCC)
        {
            NhaCungCaphdn ncc = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT MaNCC, TenNhaCungCap, DiaChi, DienThoai FROM dbo.NhaCungCap WHERE MaNCC = @MaNCC";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@MaNCC", maNCC);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ncc = new NhaCungCaphdn
                            {
                                MaNCC = reader["MaNCC"].ToString(),
                                TenNhaCungCap = reader["TenNhaCungCap"].ToString(),
                                DiaChi = reader["DiaChi"].ToString(),
                                DienThoai = reader["DienThoai"].ToString()
                            };
                        }
                    }
                }
            }
            return ncc;
        }

        public DataTable GetSachForComboBox()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT MaSach, TenSach, DonGiaNhap FROM dbo.KhoSach";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        // Hàm GetHoaDonNhapList (cần thiết để tải danh sách hóa đơn lên DataGridView chính)
        public DataTable GetHoaDonNhapList()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT h.SoHDNhap, h.NgayNhap, h.MaNhanVien, nv.TenNhanVien, 
                                h.MaNCC, ncc.TenNhaCungCap, h.TongTien
                         FROM dbo.HoaDonNhap h
                         LEFT JOIN dbo.NhanVien nv ON h.MaNhanVien = nv.MaNhanVien 
                         LEFT JOIN dbo.NhaCungCap ncc ON h.MaNCC = ncc.MaNCC
                         ORDER BY h.NgayNhap DESC, h.SoHDNhap DESC"; // Sắp xếp để hóa đơn mới nhất lên đầu
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }
    }
}
