using BTLtest2.Class;
using BTLtest2.function;
using DevExpress.XtraCharts;
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
    public partial class doanhthu : Form
    {

        public doanhthu()
        {
            InitializeComponent();
        }

        private void bnthienthi_Click(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void bntbieudo_Click(object sender, EventArgs e)
        {
            chartControlDoanhThu.Visible = false;
            chartControlChiPhi.Visible = false;
            chartControlLoiNhuan.Visible = false;
        }

        private void bnt_cp_Click(object sender, EventArgs e)
        {
            DateTime fromDate = datestart.Value;
            DateTime toDate = dateend.Value;
            float chiphiMin = 0;

            if (!float.TryParse(tongdt.Text, out chiphiMin))
            {
                chiphiMin = 0; // Mặc định nếu không nhập
            }

            var data = baocaochiphi.GetChiPhi(fromDate, toDate, chiphiMin);
            dataGridView1.DataSource = data;
            // Tính tổng chi phí
            float tong = data.Sum(dt => dt.TongTien);

            // Hiển thị lên label
            lbtongdoangthu.Text = $"Tổng chi phí: {tong:N0} VNĐ";
            LoadChiPhiChart(fromDate, toDate);
            chartControlDoanhThu.Visible = false;
            chartControlChiPhi.Visible = true;
            chartControlLoiNhuan.Visible = false;
        }

        private void bnt_dt_Click(object sender, EventArgs e)
        {
            DateTime fromDate = datestart.Value;
            DateTime toDate = dateend.Value;
            float doanhThuMin = 0;

            if (!float.TryParse(tongdt.Text, out doanhThuMin))
            {
                doanhThuMin = 0; // Mặc định nếu không nhập
            }

            var data = baocaodoanhthu.GetDoanhThu(fromDate, toDate, doanhThuMin);
            dataGridView1.DataSource = data;
            // Tính tổng chi phí
            float tong = data.Sum(dt => dt.TongTien);

            // Hiển thị lên label
            lbtongdoangthu.Text = $"Tổng doanh thu: {tong:N0} VNĐ";
            // Tải và hiển thị chỉ biểu đồ doanh thu
            LoadDoanhThuChart(fromDate, toDate);
            chartControlDoanhThu.Visible = true;
            chartControlChiPhi.Visible = false;
            chartControlLoiNhuan.Visible = false;
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
                    MessageBox.Show("Không thể khởi tạo ứng dụng Excel. Vui lòng kiểm tra cài đặt Office của bạn.", "Lỗi Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                workbook = excelApp.Workbooks.Add(Type.Missing);
                worksheet = (Excel.Worksheet)workbook.ActiveSheet;

                // --- BEGIN: Xác định tên Sheet, Tiêu đề báo cáo và Tên file mặc định động ---
                string excelSheetName = "BaoCaoTongHop"; // Tên Sheet mặc định cho form doanhthu
                string excelReportTitle = "BÁO CÁO TỔNG HỢP DOANH THU - CHI PHÍ - LỢI NHUẬN"; // Tiêu đề báo cáo mặc định
                string excelDefaultFileName = "BaoCaoTongHopDTCP.xlsx"; // Tên file mặc định

                if (lbtongdoangthu != null && !string.IsNullOrWhiteSpace(lbtongdoangthu.Text) && lbtongdoangthu.Text.Contains(":"))
                {
                    string reportLabelPrefix = lbtongdoangthu.Text.Split(':')[0].Trim();
                    if (reportLabelPrefix.Equals("Tổng doanh thu", StringComparison.OrdinalIgnoreCase))
                    {
                        excelSheetName = "DoanhThu";
                        excelReportTitle = "BÁO CÁO DOANH THU";
                        excelDefaultFileName = "BaoCaoDoanhThu.xlsx";
                    }
                    else if (reportLabelPrefix.Equals("Tổng chi phí", StringComparison.OrdinalIgnoreCase))
                    {
                        excelSheetName = "ChiPhi";
                        excelReportTitle = "BÁO CÁO CHI PHÍ";
                        excelDefaultFileName = "BaoCaoChiPhi.xlsx";
                    }
                    else if (reportLabelPrefix.Equals("Lợi nhuận Tổng", StringComparison.OrdinalIgnoreCase))
                    {
                        excelSheetName = "LoiNhuan";
                        excelReportTitle = "BÁO CÁO LỢI NHUẬN";
                        excelDefaultFileName = "BaoCaoLoiNhuan.xlsx";
                    }
                }
                // Ghi chú: Nếu form này có thể là 'hangtonkho' thì bạn cần giữ lại logic `if (this is hangtonkho)`
                // Nhưng vì file này là `doanhthu.cs`, logic trên tập trung cho các trạng thái của form doanhthu.

                worksheet.Name = excelSheetName;
                // --- END: Xác định động ---

                int currentRow = 1;
                int numberOfColumnsToMerge = 5; // Giữ nguyên hoặc điều chỉnh
                // ... (Phần code thông tin cửa hàng giữ nguyên) ...
                string tenCuaHang = "Cửa Hàng Minh Châu";
                string diaChi = "Phượng Lích 1, Diễn Hoá, Diễn Châu, Nghệ An";
                string dienThoai = "0335549158";
                Excel.Range currentRange;

                // --- Thông tin cửa hàng ---
                worksheet.Cells[currentRow, 1].Value = tenCuaHang;
                worksheet.Cells[currentRow, 1].Font.Bold = true;
                worksheet.Cells[currentRow, 1].Font.Size = 14;
                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]];
                currentRange.Merge();
                currentRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                currentRow++;

                worksheet.Cells[currentRow, 1].Value = "Địa chỉ: " + diaChi;
                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRow++;
                worksheet.Cells[currentRow, 1].Value = "Điện thoại: " + dienThoai;
                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRow++;
                currentRow++; // Dòng trống

                // --- Tiêu đề báo cáo ---
                // Sử dụng excelReportTitle đã được xác định ở trên
                worksheet.Cells[currentRow, 1].Value = excelReportTitle;
                worksheet.Cells[currentRow, 1].Font.Bold = true;
                worksheet.Cells[currentRow, 1].Font.Size = 16;
                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; currentRow++;

                // --- Khoảng ngày lọc (giữ nguyên) ---
                Control dateStartControl = this.Controls.Find("datestart", true).FirstOrDefault() ?? this.Controls.Find("dtpTuNgay", true).FirstOrDefault();
                Control dateEndControl = this.Controls.Find("dateend", true).FirstOrDefault() ?? this.Controls.Find("dtpDenNgay", true).FirstOrDefault();
                if (dateStartControl is DateTimePicker dtpStart && dateEndControl is DateTimePicker dtpEnd)
                {
                    string dateRangeInfo = $"Từ ngày: {dtpStart.Value:dd/MM/yyyy} Đến ngày: {dtpEnd.Value:dd/MM/yyyy}";
                    worksheet.Cells[currentRow, 1].Value = dateRangeInfo;
                    currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; currentRow++;
                }

                // --- Điều kiện lọc TextBox (tongdt) (giữ nguyên) ---
                Control tongDtControl = this.Controls.Find("tongdt", true).FirstOrDefault();
                if (tongDtControl is TextBox txtTongDt)
                {
                    string TongFilterInfo = txtTongDt.Text.Trim();
                    if (!string.IsNullOrEmpty(TongFilterInfo))
                    {
                        worksheet.Cells[currentRow, 1].Value = $"Điều kiện lọc Tổng: {TongFilterInfo}";
                        currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRow++;
                    }
                }

                // --- Ngày xuất báo cáo (giữ nguyên) ---
                string exportDateInfo = $"Ngày xuất báo cáo: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                worksheet.Cells[currentRow, 1].Value = exportDateInfo;
                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                // --- Dòng tổng giá trị (đã thêm ở lần trước, giữ nguyên) ---
                currentRow++;
                if (lbtongdoangthu != null && !string.IsNullOrWhiteSpace(lbtongdoangthu.Text) && lbtongdoangthu.Text.Contains(":"))
                {
                    string rawTotalText = lbtongdoangthu.Text;
                    string displayTotalLabel = "Tổng cộng";
                    string displayTotalValueStr = "";
                    int colonIndex = rawTotalText.IndexOf(':');
                    if (colonIndex > 0)
                    {
                        displayTotalLabel = rawTotalText.Substring(0, colonIndex).Trim();
                        displayTotalValueStr = rawTotalText.Substring(colonIndex + 1).Replace("VNĐ", "").Trim();
                    }
                    else
                    {
                        displayTotalValueStr = rawTotalText.Replace("VNĐ", "").Trim();
                    }
                    worksheet.Cells[currentRow, 1].Value = displayTotalLabel + ":";
                    worksheet.Cells[currentRow, 1].Font.Bold = true;
                    worksheet.Cells[currentRow, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    string parsableValueStr = displayTotalValueStr.Replace(",", "");
                    if (decimal.TryParse(parsableValueStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal numericTotalValue))
                    {
                        worksheet.Cells[currentRow, 2].Value = numericTotalValue;
                        worksheet.Cells[currentRow, 2].NumberFormat = "#,##0";
                    }
                    else
                    {
                        worksheet.Cells[currentRow, 2].Value = displayTotalValueStr;
                    }
                    worksheet.Cells[currentRow, 2].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    worksheet.Cells[currentRow, 2].Font.Bold = true;
                }
                currentRow++; // Dòng trống sau dòng tổng

                // --- DataGridView (giữ nguyên logic xuất DGV) ---
                DataGridView dgvToExport = this.Controls.Find("dataGridView1", true).FirstOrDefault() as DataGridView;
                if (dgvToExport == null || dgvToExport.Columns.Count == 0)
                {
                    // ... (xử lý không có dữ liệu, giữ nguyên)
                    MessageBox.Show("Không có dữ liệu để xuất hoặc DataGridView không tồn tại/chưa được cấu hình.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (workbook != null) workbook.Close(false); if (excelApp != null) excelApp.Quit();
                    ReleaseComObject(worksheet); ReleaseComObject(workbook); ReleaseComObject(excelApp);
                    return;
                }

                int headerStartRow = currentRow;
                for (int i = 0; i < dgvToExport.Columns.Count; i++)
                {
                    // ... (ghi header, giữ nguyên)
                    if (dgvToExport.Columns[i].Visible)
                    {
                        Excel.Range cell = worksheet.Cells[headerStartRow, i + 1];
                        cell.Value = dgvToExport.Columns[i].HeaderText;
                        cell.Font.Bold = true; cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                        cell.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    }
                }
                for (int i = 0; i < dgvToExport.Rows.Count; i++)
                {
                    // ... (ghi data rows, giữ nguyên)
                    if (dgvToExport.AllowUserToAddRows && i == dgvToExport.Rows.Count - 1) continue;
                    for (int j = 0; j < dgvToExport.Columns.Count; j++)
                    {
                        if (dgvToExport.Columns[j].Visible)
                        {
                            var value = dgvToExport.Rows[i].Cells[j].Value;
                            Excel.Range currentCell = worksheet.Cells[i + headerStartRow + 1, j + 1];
                            if (value is float || value is double || value is decimal || value is int || value is long || value is short || value is byte)
                            {
                                currentCell.NumberFormat = "#,##0"; currentCell.Value = value;
                            }
                            else if (value is DateTime dateValue) { currentCell.NumberFormat = "dd/MM/yyyy"; currentCell.Value = dateValue; }
                            else if (value != null && DateTime.TryParse(value.ToString(), out DateTime parsedDate)) { currentCell.NumberFormat = "dd/MM/yyyy"; currentCell.Value = parsedDate; }
                            else { currentCell.Value = value?.ToString(); }
                            currentCell.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        }
                    }
                }
                worksheet.Columns.AutoFit();

                // --- Lưu file tạm, Mở xem trước, Lưu file cuối cùng ---
                tempExcelFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xlsx");
                workbook.SaveAs(tempExcelFilePath);

                if (workbook != null) { workbook.Close(false); Marshal.ReleaseComObject(workbook); workbook = null; }
                if (excelApp != null) { excelApp.Quit(); Marshal.ReleaseComObject(excelApp); excelApp = null; }

                DialogResult userAction = DialogResult.Cancel;
                Process excelProcess = null;
                try
                {
                    excelProcess = Process.Start(tempExcelFilePath);
                    userAction = MessageBox.Show(
                        "File Excel đã được mở để xem trước.\n\nBạn có muốn LƯU file này không?",
                        "Xác Nhận Lưu File Excel",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question
                    );
                }
                catch (Exception exPreview)
                {
                    // ... (xử lý lỗi xem trước, giữ nguyên) ...
                    MessageBox.Show("Không thể mở bản xem trước Excel: " + exPreview.Message +
                                   "\n\nTuy nhiên, bạn vẫn có thể chọn để lưu file.",
                                   "Lỗi Xem Trước", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    userAction = MessageBox.Show(
                        "Không thể mở xem trước. Bạn có muốn tiếp tục lưu file Excel này không?",
                        "Xác Nhận Lưu",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                }

                if (userAction == DialogResult.Yes)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Excel files (*.xlsx)|*.xlsx|Excel 97-2003 Workbook (*.xls)|*.xls",
                        Title = "Chọn nơi lưu file Excel",
                        // Sử dụng tên file mặc định đã được xác định ở trên
                        FileName = excelDefaultFileName
                    };

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // ... (logic copy file, mở file đã lưu, xử lý lỗi, giữ nguyên) ...
                        try
                        {
                            if (excelProcess != null && !excelProcess.HasExited)
                            {
                                try { excelProcess.CloseMainWindow(); excelProcess.WaitForExit(2000); } catch { /* ignore */ }
                            }
                            if (excelProcess != null && !excelProcess.HasExited)
                            {
                                try { excelProcess.Kill(); excelProcess.WaitForExit(1000); } catch { /* ignore */ }
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
                // ... (xử lý lỗi chung, giữ nguyên) ...
                MessageBox.Show("Lỗi khi xuất Excel: " + ex.ToString(), "Lỗi Xuất File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // ... (giải phóng COM, xóa file tạm, GC, giữ nguyên) ...
                ReleaseComObject(worksheet);
                ReleaseComObject(workbook);
                ReleaseComObject(excelApp);
                if (!string.IsNullOrEmpty(tempExcelFilePath) && File.Exists(tempExcelFilePath))
                {
                    try { File.Delete(tempExcelFilePath); }
                    catch (IOException ioExDel) { Console.WriteLine($"Lỗi khi xóa file Excel tạm ({tempExcelFilePath}): {ioExDel.Message}. File có thể vẫn đang được sử dụng."); }
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
                Console.WriteLine("Lỗi khi giải phóng đối tượng COM: " + ex.Message);
            }
            finally
            {
                obj = null;
            }
        }

        private void bnt_ln_Click(object sender, EventArgs e)
        {
            dataloinhuan();
        }

        private void bntdong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //private void LoadChart(ChartControl chart, List<ThongKeItem> data, string valueField, string title)
        //{
        //    chart.Series.Clear();
        //    Series series = new Series(title, ViewType.Doughnut);

        //    foreach (var item in data)
        //    {
        //        double value = 0;
        //        if (valueField == "DoanhThu") value = item.DoanhThu;
        //        else if (valueField == "ChiPhi") value = item.ChiPhi;
        //        else if (valueField == "LoiNhuan") value = item.LoiNhuan;

        //        if (value > 0)
        //            series.Points.Add(new SeriesPoint(item.MaSach, value));
        //    }

        //    chart.Series.Add(series);
        //    series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
        //    (series.View as DoughnutSeriesView).HoleRadiusPercent = 60;
        //}
        //private void LoadAndDisplayCharts()
        //{
        //    try
        //    {
        //        // 1. Lấy và hiển thị biểu đồ Doanh Thu
        //        List<MatHangThongKe> doanhThuData = thongkedoanhthu.GetDoanhThuTheoMatHang(fromdate, todate);
        //        if (doanhThuData.Any())
        //        {
        //            PopulateDoughnutChart(chartControlDoanhThu, doanhThuData, "Doanh Thu Theo Mặt Hàng");
        //        }
        //        else
        //        {
        //            ShowNoDataMessage(chartControlDoanhThu, "Không có dữ liệu doanh thu theo mặt hàng.");
        //        }


        //        // 2. Lấy và hiển thị biểu đồ Chi Phí
        //        List<MatHangThongKe> chiPhiData = thongkechiphi.GetChiPhiTheoMatHang(fromdate,todate);
        //        if (chiPhiData.Any())
        //        {
        //            PopulateDoughnutChart(chartControlChiPhi, chiPhiData, "Chi Phí Nhập Hàng Theo Mặt Hàng");
        //        }
        //        else
        //        {
        //            ShowNoDataMessage(chartControlChiPhi, "Không có dữ liệu chi phí theo mặt hàng.");
        //        }


        //        // 3. Tính toán và hiển thị biểu đồ Lợi Nhuận
        //        List<MatHangThongKe> loiNhuanData = CalculateLoiNhuanTheoMatHang(doanhThuData, chiPhiData);
        //        if (loiNhuanData.Any())
        //        {
        //            PopulateDoughnutChart(chartControlLoiNhuan, loiNhuanData, "Lợi Nhuận Theo Mặt Hàng");
        //        }
        //        else
        //        {
        //            ShowNoDataMessage(chartControlLoiNhuan, "Không có dữ liệu lợi nhuận để hiển thị.");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Lỗi khi tải dữ liệu biểu đồ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        // Bỏ phương thức LoadAndDisplayCharts() cũ nếu có.
        // Thay vào đó là các phương thức riêng lẻ:

        private void LoadDoanhThuChart(DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<MatHangThongKe> doanhThuData = thongkedoanhthu.GetDoanhThuTheoMatHang(fromDate, toDate);
                // Kiểm tra null cho doanhThuData trước khi dùng .Any()
                if (doanhThuData != null && doanhThuData.Any())
                {
                    PopulateDoughnutChart(chartControlDoanhThu, doanhThuData, "Doanh Thu Theo Mặt Hàng");
                }
                else
                {
                    ShowNoDataMessage(chartControlDoanhThu, "Không có dữ liệu doanh thu theo mặt hàng.");
                }
            }
            catch (Exception ex)
            {
                ShowNoDataMessage(chartControlDoanhThu, "Lỗi tải dữ liệu doanh thu.");
                MessageBox.Show($"Lỗi khi tải dữ liệu biểu đồ doanh thu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChiPhiChart(DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<MatHangThongKe> chiPhiData = thongkechiphi.GetChiPhiTheoMatHang(fromDate, toDate);
                if (chiPhiData != null && chiPhiData.Any())
                {
                    PopulateDoughnutChart(chartControlChiPhi, chiPhiData, "Chi Phí Nhập Hàng Theo Mặt Hàng");
                }
                else
                {
                    ShowNoDataMessage(chartControlChiPhi, "Không có dữ liệu chi phí theo mặt hàng.");
                }
            }
            catch (Exception ex)
            {
                ShowNoDataMessage(chartControlChiPhi, "Lỗi tải dữ liệu chi phí.");
                MessageBox.Show($"Lỗi khi tải dữ liệu biểu đồ chi phí: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLoiNhuanChart(List<MatHangThongKe> loiNhuanDataForChart)
        {
            try
            {
                // Dữ liệu lợi nhuận (loiNhuanDataForChart) đã được tính toán và lọc sẵn từ dataloinhuan()
                if (loiNhuanDataForChart != null && loiNhuanDataForChart.Any()) // Kiểm tra xem có dữ liệu không (sau khi lọc)
                {
                    // Chỉ kiểm tra Any() vì PopulateDevExpressBarChart đã xử lý việc có item.GiaTri != 0 hay không.
                    // Hoặc chặt chẽ hơn: if (loiNhuanDataForChart != null && loiNhuanDataForChart.Any(item => item.GiaTri != 0))
                    PopulateDevExpressBarChart(chartControlLoiNhuan, loiNhuanDataForChart, "Lợi Nhuận Theo Mặt Hàng");
                }
                else
                {
                    ShowNoDataMessage(chartControlLoiNhuan, "Không có dữ liệu lợi nhuận để hiển thị (hoặc không có mục nào thỏa mãn điều kiện lọc).");
                }
            }
            catch (Exception ex)
            {
                ShowNoDataMessage(chartControlLoiNhuan, "Lỗi tải dữ liệu lợi nhuận.");
                MessageBox.Show($"Lỗi khi tải dữ liệu biểu đồ lợi nhuận: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ShowNoDataMessage(ChartControl chartControl, string message)
        {
            if (chartControl == null) return;
            chartControl.Titles.Clear();
            chartControl.Series.Clear();
            chartControl.Legends.Clear(); // << THAY ĐỔI Ở ĐÂY: Xóa collection legends

            ChartTitle title = new ChartTitle();
            title.Text = message;
            chartControl.Titles.Add(title);
        }
        private List<MatHangThongKe> CalculateLoiNhuanTheoMatHang(List<MatHangThongKe> doanhThuList, List<MatHangThongKe> chiPhiList)
        {
            var loiNhuanDictionary = new Dictionary<string, float>();

            // Cộng doanh thu
            foreach (var dtItem in doanhThuList)
            {
                if (!loiNhuanDictionary.ContainsKey(dtItem.TenMatHang))
                {
                    loiNhuanDictionary[dtItem.TenMatHang] = 0;
                }
                loiNhuanDictionary[dtItem.TenMatHang] += dtItem.GiaTri;
            }

            // Trừ chi phí
            foreach (var cpItem in chiPhiList)
            {
                if (!loiNhuanDictionary.ContainsKey(cpItem.TenMatHang))
                {
                    // Nếu mặt hàng có chi phí nhưng không có doanh thu trong kỳ
                    loiNhuanDictionary[cpItem.TenMatHang] = 0;
                }
                loiNhuanDictionary[cpItem.TenMatHang] -= cpItem.GiaTri;
            }

            // Chuyển Dictionary thành List<MatHangThongKe>
            return loiNhuanDictionary
                .Select(kvp => new MatHangThongKe { TenMatHang = kvp.Key, GiaTri = kvp.Value })
                // .Where(item => item.GiaTri != 0) // Tùy chọn: Chỉ hiển thị mặt hàng có lợi nhuận khác 0
                .OrderByDescending(item => item.GiaTri) // Sắp xếp theo lợi nhuận giảm dần
                .ToList();
        }
        private void PopulateDevExpressBarChart(DevExpress.XtraCharts.ChartControl chartControl, List<MatHangThongKe> data, string chartTitleText)
        {
            chartControl.Series.Clear();
            chartControl.Titles.Clear();
            chartControl.AnnotationRepository.Clear(); // Xóa các chú thích cũ (nếu có)

            // Gỡ bỏ handler cũ nếu có để tránh gọi nhiều lần (quan trọng nếu hàm này được gọi lại)
            chartControl.CustomDrawSeriesPoint -= ChartControl_LoiNhuan_CustomDrawSeriesPoint;


            if (data == null || !data.Any())
            {
                ShowNoDataMessage(chartControl, "Không có dữ liệu để hiển thị.");
                return;
            }

            Series seriesLoiNhuan = new Series("Lợi Nhuận", ViewType.Bar);

            foreach (var item in data)
            {
                seriesLoiNhuan.Points.Add(new SeriesPoint(item.TenMatHang, item.GiaTri));
            }
            // Hoặc bạn có thể dùng DataSource nếu muốn:
            // seriesLoiNhuan.ArgumentDataMember = "TenMatHang";
            // seriesLoiNhuan.ValueDataMembers.AddRange("GiaTri");
            // seriesLoiNhuan.DataSource = data;

            chartControl.Series.Add(seriesLoiNhuan);

            // Thêm tiêu đề cho biểu đồ
            ChartTitle chartTitle = new ChartTitle();
            chartTitle.Text = chartTitleText;
            chartControl.Titles.Add(chartTitle);

            // Tùy chỉnh Diagram (XYDiagram cho biểu đồ cột)
            XYDiagram diagram = chartControl.Diagram as XYDiagram;
            if (diagram != null)
            {
                diagram.AxisX.Title.Text = "Mặt Hàng";
                diagram.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                diagram.AxisX.Label.TextPattern = "{A}"; // {A} là Argument (Tên mặt hàng)
                diagram.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
                diagram.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
                diagram.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;
                diagram.AxisX.Label.Angle = 30; // Nghiêng nhãn trục X nếu tên mặt hàng dài

                diagram.AxisY.Title.Text = "Lợi Nhuận (VNĐ)";
                diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                diagram.AxisY.Label.TextPattern = "{V:N0}"; // {V} là Value, N0 là định dạng số không có phần thập phân

                // Tùy chọn: Cho phép màu khác nhau cho cột âm và dương
                chartControl.CustomDrawSeriesPoint += ChartControl_LoiNhuan_CustomDrawSeriesPoint;
            }

            // Tùy chỉnh Chú giải (Legend)
            chartControl.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True; // Hiển thị chú giải
            chartControl.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;

            // Tùy chỉnh giao diện của Series cột (BarSeriesView)
            BarSeriesView barView = seriesLoiNhuan.View as BarSeriesView;
            if (barView != null)
            {
                // barView.BarWidth = 0.8; // Điều chỉnh độ rộng của cột
                // Không nên dùng ColorEach = true cho loại biểu đồ này,
                // vì màu sắc nên thể hiện lãi/lỗ thay vì mỗi cột một màu ngẫu nhiên.
            }
        }

        // Hàm tùy chỉnh màu sắc cho cột lợi nhuận âm/dương
        private void ChartControl_LoiNhuan_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            // Chỉ áp dụng cho series "Lợi Nhuận" (nếu có nhiều series khác)
            // if (e.Series.Name != "Lợi Nhuận") return;

            double value = e.SeriesPoint.Values[0]; // Giá trị lợi nhuận
            BarDrawOptions drawOptions = e.SeriesDrawOptions as BarDrawOptions;
            if (drawOptions == null) return;

            if (value < 0)
            {
                // Màu cho lợi nhuận âm (lỗ) - ví dụ: màu đỏ nhạt
                drawOptions.Color = Color.FromArgb(255, 102, 102); // Hoặc Color.IndianRed
            }
            else
            {
                // Màu cho lợi nhuận dương (lãi) - ví dụ: màu xanh lá cây nhạt
                drawOptions.Color = Color.FromArgb(96, 181, 201); // Hoặc Color.LightGreen
            }
        }
        private void PopulateDoughnutChart(ChartControl chart, List<MatHangThongKe> dataSource, string chartTitleText)
        {
            if (chart == null) return;

            chart.Series.Clear();
            chart.Titles.Clear();
            chart.Legends.Clear();

            ChartTitle title = new ChartTitle();
            title.Text = chartTitleText; // Sẽ cập nhật nếu không có dữ liệu
            chart.Titles.Add(title);

            Series series = new Series(chartTitleText + "_Series", ViewType.Doughnut);

            if (dataSource != null && dataSource.Any())
            {
                int maxItemsToShow = 7; // Số lượng mục hiển thị chính
                var sortedData = dataSource.OrderByDescending(d => Math.Abs(d.GiaTri)) // Sắp xếp theo giá trị tuyệt đối để lấy các mục lớn nhất
                                           .ToList();
                float otherValueTotal = 0;
                int itemsAddedToSeries = 0;

                for (int i = 0; i < sortedData.Count; i++)
                {
                    // Chỉ thêm điểm có giá trị khác 0 (hoặc > 0 tùy loại biểu đồ)
                    // và có TenMatHang hợp lệ
                    bool isProfitChart = chartTitleText.Contains("Lợi Nhuận");
                    bool shouldAddPoint = (isProfitChart && sortedData[i].GiaTri != 0) || (!isProfitChart && sortedData[i].GiaTri > 0);

                    if (shouldAddPoint && !string.IsNullOrEmpty(sortedData[i].TenMatHang))
                    {
                        if (itemsAddedToSeries < maxItemsToShow)
                        {
                            series.Points.Add(new SeriesPoint(sortedData[i].TenMatHang, sortedData[i].GiaTri));
                            itemsAddedToSeries++;
                        }
                        else
                        {
                            otherValueTotal += sortedData[i].GiaTri;
                        }
                    }
                }

                if (otherValueTotal != 0 && itemsAddedToSeries >= maxItemsToShow) // Thêm "Mặt hàng khác" nếu có
                {
                    series.Points.Add(new SeriesPoint("Mặt hàng khác", otherValueTotal));
                }
            }

            if (series.Points.Count == 0) // QUAN TRỌNG: Nếu không có điểm nào được thêm (kể cả "Mặt hàng khác")
            {
                title.Text = chartTitleText + " (Không có dữ liệu)"; // Cập nhật lại tiêu đề
                series.Points.Add(new SeriesPoint("Không có dữ liệu", 100)); // Thêm điểm giả
                if (series.Points.Count > 0) // Kiểm tra lại sau khi thêm điểm giả
                {
                    series.Points[0].Color = Color.Gainsboro; // Màu nhạt cho điểm giả
                }
                series.Label.TextPattern = "{A}"; // Hiển thị Argument ("Không có dữ liệu")
                series.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.False; // Tắt tooltip cho điểm giả
                chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False; // Ẩn legend
            }
            else // Có dữ liệu thực tế hoặc "Mặt hàng khác"
            {
                series.Label.TextPattern = "{A}: {VP:P0}"; // Ví dụ: Tên: 25% (P0 là phần trăm không có số lẻ)
                series.LegendTextPattern = "{A}";
                series.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.True;
                series.ToolTipPointPattern = "{A}: {V:N0} VNĐ"; // Định dạng giá trị số nguyên

                if (series.View is DoughnutSeriesView doughnutView)
                {
                    doughnutView.HoleRadiusPercent = 40;
                    // Tùy chọn: Explode lát cắt lớn nhất nếu có nhiều hơn 1 lát
                    if (series.Points.Count > 1)
                    {
                        SeriesPoint pointToExplode = series.Points.Cast<SeriesPoint>()
                                                       .OrderByDescending(p => Math.Abs(Convert.ToDouble(p.Values[0])))
                                                       .FirstOrDefault();
                        if (pointToExplode != null && pointToExplode.Argument.ToString() != "Không có dữ liệu")
                        {
                            doughnutView.ExplodedPoints.Clear();
                            doughnutView.ExplodedPoints.Add(pointToExplode);
                        }
                    }
                }
                chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True; // Hiển thị legend
            }

            chart.Series.Add(series); // Luôn thêm series (giờ đã có ít nhất 1 điểm giả nếu không có dữ liệu thật)
            chart.PaletteName = "Office 2013"; // Chọn màu
        }
        private void chartControlChiPhi_Click(object sender, EventArgs e)
        {

        }

        private void doanhthu_Load(object sender, EventArgs e)
        {
            // Khởi tạo DateTimePickers (ví dụ)
            datestart.Value = DateTime.Now.AddMonths(-1);
            dateend.Value = DateTime.Now;

            // Ẩn các biểu đồ ban đầu
            chartControlDoanhThu.Visible = false;
            chartControlChiPhi.Visible = false;
            chartControlLoiNhuan.Visible = false;

        }
        private void dataloinhuan()
        {
            DateTime fromDate = datestart.Value;
            DateTime toDate = dateend.Value;

            // --- Phần tính toán LỢI NHUẬN TỔNG cho Label (giữ nguyên logic lọc tổng của bạn) ---
            float tongFilterValue = 0; // Giá trị lọc cho tổng doanh thu/chi phí từ tongdt.Text
            if (!float.TryParse(tongdt.Text, out tongFilterValue))
            {
                tongFilterValue = 0; // Mặc định nếu không nhập hoặc không hợp lệ
            }

            // Lấy dữ liệu tổng doanh thu và tổng chi phí (đã áp dụng bộ lọc tongFilterValue)
            var overallRevenueData = baocaodoanhthu.GetDoanhThu(fromDate, toDate, tongFilterValue);
            var overallChiPhiData = baocaochiphi.GetChiPhi(fromDate, toDate, tongFilterValue);

            float tongDoanhThu = overallRevenueData?.Sum(dt => dt.TongTien) ?? 0;
            float tongChiPhi = overallChiPhiData?.Sum(cp => cp.TongTien) ?? 0;
            float loiNhuanTong = tongDoanhThu - tongChiPhi;
            lbtongdoangthu.Text = $"Lợi nhuận Tổng: {loiNhuanTong:N0} VNĐ";

            // --- Phần tạo và hiển thị BÁO CÁO LỢI NHUẬN CHI TIẾT cho DataGridView và Chart ---
            List<MatHangThongKe> doanhThuTheoMatHang = thongkedoanhthu.GetDoanhThuTheoMatHang(fromDate, toDate) ?? new List<MatHangThongKe>();
            List<MatHangThongKe> chiPhiTheoMatHang = thongkechiphi.GetChiPhiTheoMatHang(fromDate, toDate) ?? new List<MatHangThongKe>();
            List<MatHangThongKe> loiNhuanReportDataFull = CalculateLoiNhuanTheoMatHang(doanhThuTheoMatHang, chiPhiTheoMatHang);

            // << THAY ĐỔI: Áp dụng bộ lọc từ tongdt.Text cho danh sách lợi nhuận chi tiết >>
            List<MatHangThongKe> loiNhuanReportDataFiltered = loiNhuanReportDataFull; // Mặc định là danh sách đầy đủ
            float itemProfitFilterValue = 0;
            bool applyItemProfitFilter = false;

            if (!string.IsNullOrWhiteSpace(tongdt.Text))
            {
                if (float.TryParse(tongdt.Text, out itemProfitFilterValue))
                {
                    applyItemProfitFilter = true;
                }
                else
                {
                    // Tùy chọn: Thông báo cho người dùng nếu giá trị lọc không hợp lệ
                    // MessageBox.Show("Giá trị lọc trong ô 'Tổng' không hợp lệ. Sẽ hiển thị tất cả lợi nhuận mặt hàng.", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (applyItemProfitFilter)
            {
                // Lọc những mặt hàng có lợi nhuận (GiaTri) >= giá trị lọc
                // Bạn có thể thay đổi điều kiện lọc (ví dụ: <=, ==, hoặc phức tạp hơn) nếu cần
                loiNhuanReportDataFiltered = loiNhuanReportDataFull.Where(item => item.GiaTri >= itemProfitFilterValue).ToList();
            }
            // << KẾT THÚC THAY ĐỔI >>

            dataGridView1.DataSource = loiNhuanReportDataFiltered;

            try
            {
                if (dataGridView1.Columns["TenMatHang"] != null)
                {
                    dataGridView1.Columns["TenMatHang"].HeaderText = "Tên Mặt Hàng";
                }
                if (dataGridView1.Columns["GiaTri"] != null)
                {
                    dataGridView1.Columns["GiaTri"].HeaderText = "Lợi Nhuận (VNĐ)";
                    dataGridView1.Columns["GiaTri"].DefaultCellStyle.Format = "N0";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi đặt HeaderText cho cột DataGridView: " + ex.Message);
            }

            // Truyền danh sách lợi nhuận ĐÃ LỌC vào LoadLoiNhuanChart
            LoadLoiNhuanChart(loiNhuanReportDataFiltered);

            chartControlDoanhThu.Visible = false;
            chartControlChiPhi.Visible = false;
            chartControlLoiNhuan.Visible = true;
        }
    }
    
}
