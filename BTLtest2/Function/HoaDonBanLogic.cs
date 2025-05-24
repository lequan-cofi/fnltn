using BTLtest2.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BTLtest2.Class.clqlhoadonban;
using KhoSach = BTLtest2.Class.clqlhoadonban.KhoSach;

namespace BTLtest2.function
{
    internal class HoaDonBanLogic
    {
        private string connectionString = "Data Source=DESKTOP-VT5RUI9;Initial Catalog=laptrinh.net;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"; // Thay thế!



        public bool UpdateKhoSachQuantity(string maSach, int quantityChange, SqlConnection con, SqlTransaction transaction)
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
                    Console.WriteLine($"Lỗi (UpdateKhoSachQuantity): Sản phẩm {maSach} không tìm thấy trong KhoSach.");
                    return false;
                }
            }

            int updatedQuantity = currentQuantity + quantityChange;

            if (updatedQuantity < 0)
            {
                Console.WriteLine($"Lỗi (UpdateKhoSachQuantity): Không đủ số lượng tồn kho cho sản phẩm {maSach}. Hiện có: {currentQuantity}, Yêu cầu thay đổi: {quantityChange}, Số lượng sau cập nhật sẽ là: {updatedQuantity}");
                return false;
            }

            string queryUpdate = "UPDATE dbo.KhoSach SET SoLuong = @SoLuong WHERE MaSach = @MaSach";
            using (SqlCommand cmdUpdate = new SqlCommand(queryUpdate, con, transaction))
            {
                cmdUpdate.Parameters.AddWithValue("@MaSach", maSach);
                cmdUpdate.Parameters.AddWithValue("@SoLuong", updatedQuantity);
                return cmdUpdate.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// Lấy thông tin sách (bao gồm giá bán và số lượng tồn) cho việc bán hàng.
        /// </summary>
        public KhoSach GetSachForBanHang(string maSach, SqlConnection con, SqlTransaction transaction = null)
        {
            KhoSach ks = null;
            string query = "SELECT MaSach, TenSach, SoLuong, DonGiaBan FROM dbo.KhoSach WHERE MaSach = @MaSach";
            SqlCommand cmd;
            if (transaction != null)
                cmd = new SqlCommand(query, con, transaction);
            else
                cmd = new SqlCommand(query, con);

            try
            {
                cmd.Parameters.AddWithValue("@MaSach", maSach);
                bool wasOpen = con.State == ConnectionState.Open;
                if (!wasOpen && transaction == null) con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ks = new KhoSach
                        {
                            MaSach = reader["MaSach"].ToString(),
                            TenSach = reader["TenSach"].ToString(),
                            SoLuong = Convert.ToInt32(reader["SoLuong"]),
                            DonGiaBan = reader["DonGiaBan"] != DBNull.Value ? Convert.ToSingle(reader["DonGiaBan"]) : 0
                        };
                    }
                }
                if (!wasOpen && transaction == null) con.Close();
            }
            finally
            {
                cmd.Dispose();
            }
            return ks;
        }


        // --- THAO TÁC VỚI HÓA ĐƠN BÁN ---
        public DataTable GetHoaDonBanList()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT hdb.SoHDBan, hdb.NgayBan, hdb.MaKhach, kh.TenKhach, 
                                        hdb.MaNhanVien, nv.TenNhanVien, hdb.TongTien, hdb.TrangThai
                                 FROM dbo.HoaDonBan hdb
                                 LEFT JOIN dbo.KhachHang kh ON hdb.MaKhach = kh.MaKhach
                                 LEFT JOIN dbo.NhanVien nv ON hdb.MaNhanVien = nv.MaNhanVien
                                 ORDER BY hdb.NgayBan DESC, hdb.SoHDBan DESC";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public HoaDonBan GetHoaDonBanById(string soHDBan)
        {
            HoaDonBan hdb = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT hdb.SoHDBan, hdb.NgayBan, hdb.MaKhach, kh.TenKhach, 
                                        hdb.MaNhanVien, nv.TenNhanVien, hdb.TongTien, hdb.TrangThai
                                 FROM dbo.HoaDonBan hdb
                                 LEFT JOIN dbo.KhachHang kh ON hdb.MaKhach = kh.MaKhach
                                 LEFT JOIN dbo.NhanVien nv ON hdb.MaNhanVien = nv.MaNhanVien
                                 WHERE hdb.SoHDBan = @SoHDBan";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@SoHDBan", soHDBan);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            hdb = new HoaDonBan
                            {
                                SoHDBan = reader["SoHDBan"].ToString(),
                                NgayBan = Convert.ToDateTime(reader["NgayBan"]),
                                MaKhach = reader["MaKhach"].ToString(),
                                TenKhachHang = reader["TenKhach"] != DBNull.Value ? reader["TenKhach"].ToString() : "",
                                MaNhanVien = reader["MaNhanVien"].ToString(),
                                TenNhanVien = reader["TenNhanVien"] != DBNull.Value ? reader["TenNhanVien"].ToString() : "",
                                TongTien = Convert.ToSingle(reader["TongTien"]),
                                TrangThai = reader["TrangThai"] != DBNull.Value ? reader["TrangThai"].ToString() : ""
                            };
                        }
                    }
                }
                if (hdb != null)
                {
                    hdb.ChiTiet = GetChiTietHDBanList(soHDBan);
                }
            }
            return hdb;
        }

        public bool AddHoaDonBan(HoaDonBan hdb)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    foreach (var ct in hdb.ChiTiet)
                    {
                        KhoSach sachTrongKho = GetSachForBanHang(ct.MaSach, con, transaction);
                        if (sachTrongKho == null)
                        {
                            throw new Exception($"Sản phẩm '{ct.TenSach}' (Mã: {ct.MaSach}) không tồn tại trong kho.");
                        }
                        if (sachTrongKho.SoLuong < ct.SLBan)
                        {
                            throw new Exception($"Không đủ số lượng tồn kho cho sản phẩm '{ct.TenSach}'. Tồn: {sachTrongKho.SoLuong}, Yêu cầu bán: {ct.SLBan}");
                        }
                        ct.DonGiaBanKhiBan = sachTrongKho.DonGiaBan;
                    }

                    string queryHD = "INSERT INTO dbo.HoaDonBan (SoHDBan, MaNhanVien, NgayBan, MaKhach, TongTien, TrangThai) VALUES (@SoHDBan, @MaNhanVien, @NgayBan, @MaKhach, @TongTien, @TrangThai)";
                    using (SqlCommand cmdHD = new SqlCommand(queryHD, con, transaction))
                    {
                        cmdHD.Parameters.AddWithValue("@SoHDBan", hdb.SoHDBan);
                        cmdHD.Parameters.AddWithValue("@MaNhanVien", hdb.MaNhanVien);
                        cmdHD.Parameters.AddWithValue("@NgayBan", hdb.NgayBan);
                        cmdHD.Parameters.AddWithValue("@MaKhach", hdb.MaKhach);
                        cmdHD.Parameters.AddWithValue("@TongTien", hdb.TongTien);
                        cmdHD.Parameters.AddWithValue("@TrangThai", string.IsNullOrEmpty(hdb.TrangThai) ? (object)DBNull.Value : hdb.TrangThai);
                        cmdHD.ExecuteNonQuery();
                    }

                    foreach (var ct in hdb.ChiTiet)
                    {
                        ct.SoHDBan = hdb.SoHDBan;
                        if (!AddChiTietHDBan_TrongTransaction(ct, con, transaction, true))
                        {
                            throw new Exception($"Lỗi khi thêm chi tiết cho sản phẩm {ct.MaSach} vào hóa đơn bán {hdb.SoHDBan}.");
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Lỗi khi thêm Hóa Đơn Bán ({hdb?.SoHDBan}): {ex.ToString()}");
                    throw;
                }
            }
        }

        public bool UpdateHoaDonBan(HoaDonBan hdb, List<ChiTietHDBan> originalChiTietList)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    string queryHD = "UPDATE dbo.HoaDonBan SET MaNhanVien = @MaNhanVien, NgayBan = @NgayBan, MaKhach = @MaKhach, TongTien = @TongTien, TrangThai = @TrangThai WHERE SoHDBan = @SoHDBan";
                    using (SqlCommand cmdHD = new SqlCommand(queryHD, con, transaction))
                    {
                        cmdHD.Parameters.AddWithValue("@SoHDBan", hdb.SoHDBan);
                        cmdHD.Parameters.AddWithValue("@MaNhanVien", hdb.MaNhanVien);
                        cmdHD.Parameters.AddWithValue("@NgayBan", hdb.NgayBan);
                        cmdHD.Parameters.AddWithValue("@MaKhach", hdb.MaKhach);
                        cmdHD.Parameters.AddWithValue("@TongTien", hdb.TongTien);
                        cmdHD.Parameters.AddWithValue("@TrangThai", string.IsNullOrEmpty(hdb.TrangThai) ? (object)DBNull.Value : hdb.TrangThai);
                        cmdHD.ExecuteNonQuery();
                    }

                    foreach (var oldCt in originalChiTietList)
                    {
                        if (!UpdateKhoSachQuantity(oldCt.MaSach, oldCt.SLBan, con, transaction))
                        {
                            throw new Exception($"Lỗi khi hoàn tác kho cho sản phẩm {oldCt.MaSach} của HĐ bán {hdb.SoHDBan}.");
                        }
                    }

                    if (!DeleteAllChiTietHDBanBySoHD(hdb.SoHDBan, con, transaction))
                    {
                        throw new Exception($"Lỗi khi xóa chi tiết cũ của HĐ bán {hdb.SoHDBan}.");
                    }

                    foreach (var newCt in hdb.ChiTiet)
                    {
                        KhoSach sachTrongKho = GetSachForBanHang(newCt.MaSach, con, transaction);
                        if (sachTrongKho == null) throw new Exception($"Sản phẩm '{newCt.TenSach}' (Mã: {newCt.MaSach}) không tồn tại.");
                        if (sachTrongKho.SoLuong < newCt.SLBan) throw new Exception($"Không đủ SL tồn cho '{newCt.TenSach}'. Tồn: {sachTrongKho.SoLuong}, Bán: {newCt.SLBan}");

                        newCt.SoHDBan = hdb.SoHDBan;
                        newCt.DonGiaBanKhiBan = sachTrongKho.DonGiaBan;
                        if (!AddChiTietHDBan_TrongTransaction(newCt, con, transaction, true))
                        {
                            throw new Exception($"Lỗi khi thêm chi tiết mới cho sản phẩm {newCt.MaSach} vào HĐ bán {hdb.SoHDBan}.");
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Lỗi khi cập nhật Hóa Đơn Bán ({hdb?.SoHDBan}): {ex.ToString()}");
                    throw;
                }
            }
        }

        public bool DeleteHoaDonBan(string soHDBan)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    List<ChiTietHDBan> chiTietToDelete = GetChiTietHDBanListTrongTransaction(soHDBan, con, transaction);
                    foreach (var ct in chiTietToDelete)
                    {
                        if (!UpdateKhoSachQuantity(ct.MaSach, ct.SLBan, con, transaction))
                        {
                            throw new Exception($"Lỗi khi hoàn tác kho cho sản phẩm {ct.MaSach} khi xóa HĐ bán {soHDBan}.");
                        }
                    }

                    if (!DeleteAllChiTietHDBanBySoHD(soHDBan, con, transaction))
                    {
                        throw new Exception($"Lỗi khi xóa chi tiết của HĐ bán {soHDBan}.");
                    }

                    string queryHD = "DELETE FROM dbo.HoaDonBan WHERE SoHDBan = @SoHDBan";
                    using (SqlCommand cmdHD = new SqlCommand(queryHD, con, transaction))
                    {
                        cmdHD.Parameters.AddWithValue("@SoHDBan", soHDBan);
                        cmdHD.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Lỗi khi xóa Hóa Đơn Bán ({soHDBan}): {ex.ToString()}");
                    throw;
                }
            }
        }

        // --- THAO TÁC VỚI CHI TIẾT HÓA ĐƠN BÁN ---
        public List<ChiTietHDBan> GetChiTietHDBanList(string soHDBan)
        {
            List<ChiTietHDBan> list = new List<ChiTietHDBan>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Sửa SQL: Không tham chiếu ctb.DonGiaBanKhiBan, chỉ dùng ks.DonGiaBan
                string query = @"SELECT ctb.SoHDBan, ctb.MaSach, ks.TenSach, ctb.SLBan, ctb.KhuyenMai, 
                                        ks.DonGiaBan AS DonGiaDaBan, ctb.ThanhTien 
                                 FROM dbo.ChiTietHDBan ctb
                                 JOIN dbo.KhoSach ks ON ctb.MaSach = ks.MaSach
                                 WHERE ctb.SoHDBan = @SoHDBan";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@SoHDBan", soHDBan);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ChiTietHDBan
                            {
                                SoHDBan = reader["SoHDBan"].ToString(),
                                MaSach = reader["MaSach"].ToString(),
                                TenSach = reader["TenSach"].ToString(),
                                SLBan = Convert.ToInt32(reader["SLBan"]),
                                KhuyenMai = Convert.ToSingle(reader["KhuyenMai"]),
                                DonGiaBanKhiBan = Convert.ToSingle(reader["DonGiaDaBan"]), // Sẽ lấy từ ks.DonGiaBan
                                ThanhTien = Convert.ToSingle(reader["ThanhTien"])
                            });
                        }
                    }
                }
            }
            return list;
        }

        private List<ChiTietHDBan> GetChiTietHDBanListTrongTransaction(string soHDBan, SqlConnection con, SqlTransaction transaction)
        {
            List<ChiTietHDBan> list = new List<ChiTietHDBan>();
            // Sửa SQL: Không tham chiếu ctb.DonGiaBanKhiBan, chỉ dùng ks.DonGiaBan
            string query = @"SELECT ctb.SoHDBan, ctb.MaSach, ks.TenSach, ctb.SLBan, ctb.KhuyenMai, 
                                    ks.DonGiaBan AS DonGiaDaBan, ctb.ThanhTien 
                             FROM dbo.ChiTietHDBan ctb
                             JOIN dbo.KhoSach ks ON ctb.MaSach = ks.MaSach
                             WHERE ctb.SoHDBan = @SoHDBan";
            using (SqlCommand cmd = new SqlCommand(query, con, transaction))
            {
                cmd.Parameters.AddWithValue("@SoHDBan", soHDBan);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ChiTietHDBan
                        {
                            SoHDBan = reader["SoHDBan"].ToString(),
                            MaSach = reader["MaSach"].ToString(),
                            TenSach = reader["TenSach"].ToString(),
                            SLBan = Convert.ToInt32(reader["SLBan"]),
                            KhuyenMai = Convert.ToSingle(reader["KhuyenMai"]),
                            DonGiaBanKhiBan = Convert.ToSingle(reader["DonGiaDaBan"]), // Sẽ lấy từ ks.DonGiaBan
                            ThanhTien = Convert.ToSingle(reader["ThanhTien"])
                        });
                    }
                }
            }
            return list;
        }

        private bool AddChiTietHDBan_TrongTransaction(ChiTietHDBan ctb, SqlConnection con, SqlTransaction transaction, bool updateKho)
        {
            // Sửa SQL: Loại bỏ cột DonGiaBanKhiBan khỏi INSERT
            string query = "INSERT INTO dbo.ChiTietHDBan (SoHDBan, MaSach, SLBan, KhuyenMai, ThanhTien) VALUES (@SoHDBan, @MaSach, @SLBan, @KhuyenMai, @ThanhTien)";
            using (SqlCommand cmd = new SqlCommand(query, con, transaction))
            {
                cmd.Parameters.AddWithValue("@SoHDBan", ctb.SoHDBan);
                cmd.Parameters.AddWithValue("@MaSach", ctb.MaSach);
                cmd.Parameters.AddWithValue("@SLBan", ctb.SLBan);
                // Không thêm parameter cho @DonGiaBanKhiBan nữa
                cmd.Parameters.AddWithValue("@KhuyenMai", ctb.KhuyenMai);
                cmd.Parameters.AddWithValue("@ThanhTien", ctb.ThanhTien); // ThanhTien đã được tính dựa trên ctb.DonGiaBanKhiBan (lấy từ KhoSach)
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0 && updateKho)
                {
                    return UpdateKhoSachQuantity(ctb.MaSach, -ctb.SLBan, con, transaction);
                }
                return rowsAffected > 0;
            }
        }

        private bool DeleteAllChiTietHDBanBySoHD(string soHDBan, SqlConnection con, SqlTransaction transaction)
        {
            string query = "DELETE FROM dbo.ChiTietHDBan WHERE SoHDBan = @SoHDBan";
            using (SqlCommand cmd = new SqlCommand(query, con, transaction))
            {
                cmd.Parameters.AddWithValue("@SoHDBan", soHDBan);
                return cmd.ExecuteNonQuery() >= 0;
            }
        }

        // --- CÁC HÀM TIỆN ÍCH CHO COMBOBOX ---
        public DataTable GetKhachHangForComboBox()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT MaKhach, TenKhach FROM dbo.KhachHang ORDER BY TenKhach";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public DataTable GetNhanVienForComboBox()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT MaNhanVien, TenNhanVien FROM dbo.NhanVien ORDER BY TenNhanVien";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public DataTable GetSachForBanComboBox()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT MaSach, TenSach, SoLuong, DonGiaBan FROM dbo.KhoSach WHERE SoLuong > 0 ORDER BY TenSach";
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