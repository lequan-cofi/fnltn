using BTLtest2.Class;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTLtest2.function
{
    internal class fmatsach
    {
        private string connectionString = "Data Source=DESKTOP-VT5RUI9;Initial Catalog=laptrinh.net;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        // Phương thức trợ giúp để tạo và mở kết nối
        private SqlConnection GetOpenConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        // Lấy danh sách tất cả sách cho ComboBox MaSach
        public List<SachInfo> GetSachInfoList()
        {
            List<SachInfo> sachList = new List<SachInfo>();
            using (SqlConnection conn = GetOpenConnection())
            {
                string query = "SELECT MaSach, TenSach, SoLuong FROM dbo.KhoSach ORDER BY TenSach;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sachList.Add(new SachInfo
                            {
                                MaSach = reader["MaSach"].ToString(),
                                TenSach = reader["TenSach"].ToString(),
                                SoLuongHienCo = Convert.ToInt32(reader["SoLuong"])
                            });
                        }
                    }
                }
            }
            return sachList;
        }

        // Lấy tất cả các bản ghi sách bị mất bao gồm TenSach để hiển thị
        public List<MatSachDisplay> GetMatSachEntries()
        {
            List<MatSachDisplay> entries = new List<MatSachDisplay>();
            using (SqlConnection conn = GetOpenConnection())
            {
                string query = @"
                    SELECT ms.MaLanMat, ms.MaSach, ks.TenSach, ms.SLMat, ms.NgayMat
                    FROM dbo.MatSach ms
                    INNER JOIN dbo.KhoSach ks ON ms.MaSach = ks.MaSach
                    ORDER BY ms.NgayMat DESC, ms.MaLanMat DESC;"; // Sắp xếp theo ngày mất giảm dần, sau đó là mã lần mất giảm dần
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            entries.Add(new MatSachDisplay
                            {
                                MaLanMat = Convert.ToInt32(reader["MaLanMat"]),
                                MaSach = reader["MaSach"].ToString(),
                                TenSach = reader["TenSach"].ToString(),
                                SLMat = Convert.ToInt32(reader["SLMat"]),
                                NgayMat = Convert.ToDateTime(reader["NgayMat"])
                            });
                        }
                    }
                }
            }
            return entries;
        }

        // Lấy một bản ghi MatSach (ví dụ: trước khi xóa để biết SLMat và MaSach)
        public MatSachData GetMatSachById(int maLanMat)
        {
            MatSachData entry = null;
            using (SqlConnection conn = GetOpenConnection())
            {
                string query = "SELECT MaLanMat, MaSach, SLMat, NgayMat FROM dbo.MatSach WHERE MaLanMat = @MaLanMat;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaLanMat", maLanMat);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            entry = new MatSachData
                            {
                                MaLanMat = Convert.ToInt32(reader["MaLanMat"]),
                                MaSach = reader["MaSach"].ToString(),
                                SLMat = Convert.ToInt32(reader["SLMat"]),
                                NgayMat = Convert.ToDateTime(reader["NgayMat"])
                            };
                        }
                    }
                }
            }
            return entry;
        }


        // Thêm một bản ghi sách bị mất mới và cập nhật KhoSach
        public bool ThemMatSach(MatSachData matSach)
        {
            using (SqlConnection conn = GetOpenConnection())
            {
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // THAY ĐỔI CÂU LỆNH INSERT: Bỏ MaLanMat khỏi danh sách cột và VALUES
                        string queryMatSach = @"
                        INSERT INTO dbo.MatSach (MaSach, SLMat, NgayMat) 
                        VALUES (@MaSach, @SLMat, @NgayMat);";

                        // Nếu bạn muốn lấy lại MaLanMat vừa được tạo tự động, bạn có thể thêm:
                        // queryMatSach += " SELECT SCOPE_IDENTITY();";

                        using (SqlCommand cmdMatSach = new SqlCommand(queryMatSach, conn, transaction))
                        {
                            // BỎ THAM SỐ MaLanMat
                            // cmdMatSach.Parameters.AddWithValue("@MaLanMat", matSach.MaLanMat); 
                            cmdMatSach.Parameters.AddWithValue("@MaSach", matSach.MaSach);
                            cmdMatSach.Parameters.AddWithValue("@SLMat", matSach.SLMat);
                            cmdMatSach.Parameters.AddWithValue("@NgayMat", matSach.NgayMat);

                            // Nếu bạn dùng "SELECT SCOPE_IDENTITY();" để lấy ID mới:
                            // object newId = cmdMatSach.ExecuteScalar();
                            // if (newId != null && newId != DBNull.Value)
                            // {
                            //    matSach.MaLanMat = Convert.ToInt32(newId); // Cập nhật lại đối tượng matSach nếu cần
                            // }
                            // else
                            // {
                            //    cmdMatSach.ExecuteNonQuery(); // Nếu không cần lấy ID mới hoặc SCOPE_IDENTITY không trả về gì
                            // }
                            cmdMatSach.ExecuteNonQuery(); // Cách đơn giản nhất nếu không cần lấy ID trả về ngay
                        }

                        string queryKhoSach = @"
                        UPDATE dbo.KhoSach 
                        SET SoLuong = SoLuong - @SLMat 
                        WHERE MaSach = @MaSach;";
                        using (SqlCommand cmdKhoSach = new SqlCommand(queryKhoSach, conn, transaction))
                        {
                            cmdKhoSach.Parameters.AddWithValue("@SLMat", matSach.SLMat);
                            cmdKhoSach.Parameters.AddWithValue("@MaSach", matSach.MaSach);
                            cmdKhoSach.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        // Bạn có thể giữ lại MessageBox này trong quá trình debug
                        System.Windows.Forms.MessageBox.Show("Lỗi SQL chi tiết khi thêm: \n" + ex.ToString(), "Lỗi Database", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        Console.WriteLine("Lỗi khi thêm Mất Sách: " + ex.ToString());
                        return false;
                    }
                }
            }
        }

        // Cập nhật một bản ghi sách bị mất hiện có và điều chỉnh số lượng KhoSach
        public bool SuaMatSach(MatSachData updatedMatSach, string originalMaSach, int originalSLMat)
        {
            using (SqlConnection conn = GetOpenConnection())
            {
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Cập nhật mục trong MatSach
                        string queryUpdateMatSach = @"
                            UPDATE dbo.MatSach 
                            SET MaSach = @NewMaSach, SLMat = @NewSLMat, NgayMat = @NewNgayMat 
                            WHERE MaLanMat = @MaLanMat;";
                        using (SqlCommand cmdUpdate = new SqlCommand(queryUpdateMatSach, conn, transaction))
                        {
                            cmdUpdate.Parameters.AddWithValue("@NewMaSach", updatedMatSach.MaSach);
                            cmdUpdate.Parameters.AddWithValue("@NewSLMat", updatedMatSach.SLMat);
                            cmdUpdate.Parameters.AddWithValue("@NewNgayMat", updatedMatSach.NgayMat);
                            cmdUpdate.Parameters.AddWithValue("@MaLanMat", updatedMatSach.MaLanMat);
                            cmdUpdate.ExecuteNonQuery();
                        }

                        // 2. Điều chỉnh KhoSach:
                        // 2a. Hoàn lại số lượng mất cũ cho sách ban đầu
                        string queryRevertKhoSach = @"
                            UPDATE dbo.KhoSach 
                            SET SoLuong = SoLuong + @OriginalSLMat 
                            WHERE MaSach = @OriginalMaSach;";
                        using (SqlCommand cmdRevert = new SqlCommand(queryRevertKhoSach, conn, transaction))
                        {
                            cmdRevert.Parameters.AddWithValue("@OriginalSLMat", originalSLMat);
                            cmdRevert.Parameters.AddWithValue("@OriginalMaSach", originalMaSach);
                            cmdRevert.ExecuteNonQuery();
                        }

                        // 2b. Áp dụng số lượng mất mới cho sách (có thể là sách mới)
                        string queryApplyNewKhoSach = @"
                            UPDATE dbo.KhoSach 
                            SET SoLuong = SoLuong - @NewSLMat 
                            WHERE MaSach = @NewMaSach;";
                        using (SqlCommand cmdApplyNew = new SqlCommand(queryApplyNewKhoSach, conn, transaction))
                        {
                            cmdApplyNew.Parameters.AddWithValue("@NewSLMat", updatedMatSach.SLMat);
                            cmdApplyNew.Parameters.AddWithValue("@NewMaSach", updatedMatSach.MaSach);
                            cmdApplyNew.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Lỗi khi cập nhật Mất Sách: " + ex.Message);
                        return false;
                    }
                }
            }
        }


        // Xóa một bản ghi sách bị mất và cập nhật KhoSach
        public bool XoaMatSach(int maLanMat)
        {
            MatSachData recordToDelete = GetMatSachById(maLanMat); // Lấy thông tin bản ghi cần xóa
            if (recordToDelete == null) return false; // Không tìm thấy bản ghi

            using (SqlConnection conn = GetOpenConnection())
            {
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Xóa khỏi MatSach
                        string queryDeleteMatSach = "DELETE FROM dbo.MatSach WHERE MaLanMat = @MaLanMat;";
                        using (SqlCommand cmdDelete = new SqlCommand(queryDeleteMatSach, conn, transaction))
                        {
                            cmdDelete.Parameters.AddWithValue("@MaLanMat", maLanMat);
                            cmdDelete.ExecuteNonQuery();
                        }

                        // 2. Cập nhật KhoSach (tăng số lượng - sách không còn được coi là mất từ bản ghi này nữa)
                        string queryKhoSach = @"
                            UPDATE dbo.KhoSach 
                            SET SoLuong = SoLuong + @SLMat 
                            WHERE MaSach = @MaSach;";
                        using (SqlCommand cmdKhoSach = new SqlCommand(queryKhoSach, conn, transaction))
                        {
                            cmdKhoSach.Parameters.AddWithValue("@SLMat", recordToDelete.SLMat);
                            cmdKhoSach.Parameters.AddWithValue("@MaSach", recordToDelete.MaSach);
                            cmdKhoSach.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Lỗi khi xóa Mất Sách: " + ex.Message);
                        return false;
                    }
                }
            }
        }
    }
}
