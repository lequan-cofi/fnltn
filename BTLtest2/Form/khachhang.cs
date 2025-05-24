using BTLtest2.Class;
using BTLtest2.function;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace BTLtest2
{
    public partial class khachhang : Form
    {
        private DataAccess dataAccess;
        public khachhang()
        {
            InitializeComponent();
            dataAccess = new DataAccess();
        }

        private void khachhang_Load(object sender, EventArgs e)
        {
            // Thiết lập giá trị mặc định cho các control khi form được tải
            dtpStartDate.Value = DateTime.Now.AddMonths(-1); // Ví dụ: 1 tháng trước
            dtpEndDate.Value = DateTime.Now;
            txtSoLanMua.Text = "1"; // Mặc định ít nhất 1 lần mua
            txtTongHoaDon.Text = "0"; // Mặc định tổng hóa đơn ít nhất là 0

            // Cấu hình DataGridView
            SetupDataGridView();
        }
        private void SetupDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false; // Tắt tự động tạo cột
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            // Định nghĩa các cột sẽ hiển thị
            // Thứ tự và DataPropertyName phải khớp với thuộc tính của class KhachHang
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaKhachCol",
                HeaderText = "Mã Khách",
                DataPropertyName = "MaKhach",
                FillWeight = 15 // Tỷ lệ chiều rộng cột
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenKhachCol",
                HeaderText = "Tên Khách Hàng",
                DataPropertyName = "TenKhach",
                FillWeight = 35
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DienThoaiCol",
                HeaderText = "Điện Thoại",
                DataPropertyName = "DienThoai",
                FillWeight = 20
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SoLanMuaCol",
                HeaderText = "Số Lần Mua",
                DataPropertyName = "SoLanMua",
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight },
                FillWeight = 15
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TongTienHoaDonCol",
                HeaderText = "Tổng Hóa Đơn",
                DataPropertyName = "TongTienHoaDon",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }, // Format số, không có phần thập phân
                FillWeight = 25
            });
        }

        private void bnthienthi_Click(object sender, EventArgs e)
        {
            // 1. Lấy giá trị từ các input controls
            DateTime startDate = dtpStartDate.Value;
            DateTime endDate = dtpEndDate.Value;
            int minSoLanMua;
            double minTongHoaDon;

            // 2. Validate đầu vào
            if (endDate < startDate)
            {
                MessageBox.Show("Ngày kết thúc không được nhỏ hơn ngày bắt đầu.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpEndDate.Focus();
                return;
            }

            if (!int.TryParse(txtSoLanMua.Text, out minSoLanMua) || minSoLanMua < 0)
            {
                MessageBox.Show("Số lần mua phải là một số nguyên không âm.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLanMua.Focus();
                return;
            }

            if (!double.TryParse(txtTongHoaDon.Text, out minTongHoaDon) || minTongHoaDon < 0)
            {
                MessageBox.Show("Tổng hóa đơn phải là một số không âm.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTongHoaDon.Focus();
                return;
            }

            // 3. Gọi hàm từ DataAccess để lấy dữ liệu
            try
            {
                List<KhachHang> danhSachKhachHang = dataAccess.GetLoyalCustomers(startDate, endDate, minSoLanMua, minTongHoaDon);

                // 4. Hiển thị dữ liệu lên DataGridView
                dataGridView1.DataSource = null; // Xóa dữ liệu cũ
                if (danhSachKhachHang != null && danhSachKhachHang.Any())
                {
                    dataGridView1.DataSource = danhSachKhachHang;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy khách hàng nào thỏa mãn điều kiện.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Lỗi này có thể đã được xử lý ở DataAccess, nhưng thêm ở đây để bắt các lỗi khác nếu có
                MessageBox.Show($"Đã xảy ra lỗi khi lấy dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bntdong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bnttaoexel_Click(object sender, EventArgs e)
        {
            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            string tempExcelFilePath = string.Empty;

            try
            {
                excelApp = new Excel.Application();
                if (excelApp == null)
                {
                    MessageBox.Show("Không thể khởi tạo ứng dụng Excel...", "Lỗi Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                workbook = excelApp.Workbooks.Add(Type.Missing);
                worksheet = (Excel.Worksheet)workbook.ActiveSheet;
                worksheet.Name = "KhachHangThanThiet";

                // --- BEGIN: Thêm thông tin cửa hàng và báo cáo ---
                int currentRow = 1;
                int numberOfColumnsToMerge = dataGridView1.Columns.Count > 0 ? dataGridView1.Columns.Count : 5; // Giả sử báo cáo KH có 5 cột

                string tenCuaHang = "Cửa Hàng Minh Châu";
                string diaChi = "Phượng Lích 1, Diễn Hoá, Diễn Châu, Nghệ An";
                string dienThoai = "0335549158";
                Excel.Range currentRange;

                worksheet.Cells[currentRow, 1].Value = tenCuaHang;
                worksheet.Cells[currentRow, 1].Font.Bold = true;
                worksheet.Cells[currentRow, 1].Font.Size = 14;
                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; currentRow++;
                worksheet.Cells[currentRow, 1].Value = "Địa chỉ: " + diaChi;
                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRow++;
                worksheet.Cells[currentRow, 1].Value = "Điện thoại: " + dienThoai;
                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRow++;
                currentRow++;

                string reportTitle = "BÁO CÁO KHÁCH HÀNG THÂN THIẾT";
                worksheet.Cells[currentRow, 1].Value = reportTitle;
                worksheet.Cells[currentRow, 1].Font.Bold = true;
                worksheet.Cells[currentRow, 1].Font.Size = 16;
                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; currentRow++;

                // dtpStartDate, dtpEndDate là tên DateTimePicker của bạn
                string dateRangeInfo = $"Từ ngày: {dtpStartDate.Value:dd/MM/yyyy} Đến ngày: {dtpEndDate.Value:dd/MM/yyyy}";
                worksheet.Cells[currentRow, 1].Value = dateRangeInfo;
                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; currentRow++;

                // txtSoLanMua, txtTongHoaDon là tên TextBox filter
                string soLanMuaFilterInfo = txtSoLanMua.Text.Trim();
                if (!string.IsNullOrEmpty(soLanMuaFilterInfo))
                {
                    worksheet.Cells[currentRow, 1].Value = $"Điều kiện số lần mua: {soLanMuaFilterInfo}";
                    currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRow++;
                }
                string tongHoaDonFilterInfo = txtTongHoaDon.Text.Trim();
                if (!string.IsNullOrEmpty(tongHoaDonFilterInfo))
                {
                    worksheet.Cells[currentRow, 1].Value = $"Điều kiện tổng hóa đơn: {tongHoaDonFilterInfo}";
                    currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRow++;
                }

                string exportDateInfo = $"Ngày xuất báo cáo: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                worksheet.Cells[currentRow, 1].Value = exportDateInfo;
                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight; currentRow++;
                currentRow++;
                // --- END: Thêm thông tin ---

                if (dataGridView1.Columns.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất...", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (workbook != null) { workbook.Close(false); }
                    return;
                }

                // (Ghi tiêu đề và dữ liệu từ dataGridView1 tương tự như hàm trên)
                for (int i = 0; i < dataGridView1.Columns.Count; i++) { /* ... */ Excel.Range cell = worksheet.Cells[currentRow, i + 1]; cell.Value = dataGridView1.Columns[i].HeaderText; cell.Font.Bold = true; cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray); cell.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; }
                for (int i = 0; i < dataGridView1.Rows.Count; i++) { if (dataGridView1.Rows[i].IsNewRow) continue; for (int j = 0; j < dataGridView1.Columns.Count; j++) { /* ... */ var value = dataGridView1.Rows[i].Cells[j].Value; Excel.Range currentCell = worksheet.Cells[i + currentRow + 1, j + 1]; if (value is float || value is double || value is decimal || value is int) { currentCell.NumberFormat = "#,##0"; currentCell.Value = value; } else if (value is DateTime dateValue) { currentCell.NumberFormat = "dd/MM/yyyy"; currentCell.Value = dateValue; } else if (DateTime.TryParse(value?.ToString(), out DateTime parsedDate)) { currentCell.NumberFormat = "dd/MM/yyyy"; currentCell.Value = parsedDate; } else { currentCell.Value = value?.ToString(); } currentCell.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; } }
                worksheet.Columns.AutoFit();


                // --- Bước 1: Lưu vào file tạm ---
                tempExcelFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + "_KhachHangPreview.xlsx");
                workbook.SaveAs(tempExcelFilePath);

                // --- Quan trọng: Đóng và giải phóng COM object TRƯỚC KHI mở preview ---
                if (worksheet != null) { Marshal.ReleaseComObject(worksheet); worksheet = null; }
                if (workbook != null) { workbook.Close(false); Marshal.ReleaseComObject(workbook); workbook = null; }
                if (excelApp != null) { excelApp.Quit(); Marshal.ReleaseComObject(excelApp); excelApp = null; }
                GC.Collect();
                GC.WaitForPendingFinalizers();

                // --- Bước 2: Mở file tạm để xem trước ---
                DialogResult userAction = DialogResult.Cancel;
                Process excelProcess = null;
                try
                {
                    excelProcess = Process.Start(tempExcelFilePath);
                    userAction = MessageBox.Show(
                        "Báo cáo Khách Hàng Thân Thiết đã được mở để xem trước.\n\nBạn có muốn LƯU file này không?",
                        "Xác Nhận Lưu File Excel", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                }
                // ... (Phần catch và xử lý userAction, SaveFileDialog, File.Copy như hàm trên) ...
                catch (Exception exPreview)
                {
                    MessageBox.Show("Không thể mở bản xem trước Excel: " + exPreview.Message +
                                    "\n\nBạn vẫn có thể chọn để lưu file.", "Lỗi Xem Trước", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    userAction = MessageBox.Show(
                        "Không thể mở xem trước. Bạn có muốn tiếp tục lưu file Excel này không?",
                        "Xác Nhận Lưu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }

                if (userAction == DialogResult.Yes)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel files (*.xlsx)|*.xlsx|Excel 97-2003 Workbook (*.xls)|*.xls",
                        Title = "Chọn nơi lưu file Báo Cáo Khách Hàng",
                        FileName = "BaoCaoKhachHangThanThiet.xlsx" // Tên file mặc định
                    };

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // (Copy logic File.Copy, MessageBox, Process.Start như hàm trên)
                        try
                        {
                            if (excelProcess != null && !excelProcess.HasExited)
                            {
                                try { excelProcess.CloseMainWindow(); excelProcess.WaitForExit(1000); } catch { /* ignore */ }
                            }
                            File.Copy(tempExcelFilePath, saveFileDialog.FileName, true);
                            MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            try { Process.Start(saveFileDialog.FileName); }
                            catch (Exception exOpenFinal) { MessageBox.Show("Không thể mở file Excel đã lưu: " + exOpenFinal.Message, "Lỗi Mở File", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                        }
                        catch (IOException ioEx) { MessageBox.Show($"Lỗi khi lưu file Excel: {ioEx.Message}\nVui lòng đóng file Excel xem trước (nếu đang mở) và thử lại.", "Lỗi Lưu File", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        catch (Exception exSaveFinal) { MessageBox.Show("Lỗi khi lưu file Excel cuối cùng: " + exSaveFinal.Message, "Lỗi Lưu File", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                    else { MessageBox.Show("Thao tác lưu file Excel đã được hủy.", "Đã hủy", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
                else { MessageBox.Show("Thao tác xuất file Excel đã được hủy.", "Đã hủy", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất Excel (Khách hàng): " + ex.ToString(), "Lỗi Xuất File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // ... (Giải phóng COM và xóa file tạm như hàm trên) ...
                ReleaseComObject(worksheet);
                ReleaseComObject(workbook);
                ReleaseComObject(excelApp);

                if (!string.IsNullOrEmpty(tempExcelFilePath) && File.Exists(tempExcelFilePath))
                {
                    try { File.Delete(tempExcelFilePath); }
                    catch (IOException ioExDel) { Console.WriteLine($"Lỗi khi xóa file Excel tạm ({tempExcelFilePath}): {ioExDel.Message}."); }
                    catch (Exception exDeleteTemp) { Console.WriteLine("Lỗi không xác định khi xóa file Excel tạm: " + exDeleteTemp.Message); }
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        private void ReleaseComObject(object obj)
        {
            try
            {
                if (obj != null && Marshal.IsComObject(obj))
                {
                    Marshal.ReleaseComObject(obj);
                }
            }
            catch (Exception ex)
            {
                // Ghi log lỗi thay vì hiển thị MessageBox để tránh làm phiền người dùng trong finally
                Console.WriteLine("Lỗi khi giải phóng đối tượng COM: " + ex.Message);
            }
            finally
            {
                obj = null;
            }
        }
    }
}
