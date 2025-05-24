using BTLtest2.Class; // Giả sử InventoryDataPoint ở đây, hoặc namespace của nó

using BTLtest2.function; // Giả sử InventoryDataAccess ở đây

using DevExpress.XtraCharts;

using System;

using System.Collections.Generic;

using System.ComponentModel;

using System.Data;

using System.Data.SqlClient;

using System.Diagnostics;

using System.Drawing;

using System.IO;

using System.Linq;

using System.Runtime.InteropServices;

using System.Text;

using System.Threading.Tasks;

using System.Windows.Forms;

using static BTLtest2.Class.ThongKeItem;



// using System.Windows.Forms.DataVisualization.Charting; // Không dùng chart chuẩn

using Excel = Microsoft.Office.Interop.Excel;

using Series = DevExpress.XtraCharts.Series;





namespace BTLtest2

{

    public partial class hangtonkho : Form

    {

        private InventoryDataAccess inventoryDataAccess;



        // Assume you have DateTimePicker controls named dtpTuNgay and dtpDenNgay

        // And TextBox controls txtLuongTon, txtLuongBan

        // And a DataGridView named dgvHangTonKho



        public hangtonkho()

        {

            InitializeComponent();

            inventoryDataAccess = new InventoryDataAccess();

            // Initialize DateTimePickers to sensible defaults if needed

            // dtpTuNgay.Value = DateTime.Now.AddMonths(-1);

            // dtpDenNgay.Value = DateTime.Now;

            dtpTuNgay.Value = DateTime.Now.Date.AddMonths(-1);

            dtpDenNgay.Value = DateTime.Now.Date;

        }



        private void hangtonkho_Load(object sender, EventArgs e)

        {

            txtLuongTon.Text = "";

            txtLuongBan.Text = "";



            // Initialize ComboBox for filter types if you add them

            // Example:

            // cmbFilterTypeLuongTon.Items.AddRange(new object[] { "Nhỏ hơn hoặc bằng (<=)", "Lớn hơn hoặc bằng (>=)", "Bằng (==)" });

            // if (cmbFilterTypeLuongTon.Items.Count > 0) cmbFilterTypeLuongTon.SelectedIndex = 0;

            // cmbFilterTypeLuongBan.Items.AddRange(new object[] { "Nhỏ hơn hoặc bằng (<=)", "Lớn hơn hoặc bằng (>=)", "Bằng (==)" });

            // if (cmbFilterTypeLuongBan.Items.Count > 0) cmbFilterTypeLuongBan.SelectedIndex = 0;





            SetupDataGridView();

            // Optionally load initial data, or wait for "Hiển thị" button click

            // LoadInventoryData(); 

            chartInventoryReport.Visible = false; // Ẩn biểu đồ ban đầu



        }

        private void SetupDataGridView()

        {

            dataGridView1.Columns.Clear();

            dataGridView1.AutoGenerateColumns = false;

            dataGridView1.AllowUserToAddRows = false;

            dataGridView1.AllowUserToDeleteRows = false;

            dataGridView1.ReadOnly = true;

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dataGridView1.MultiSelect = false;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;



            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn

            { Name = "MaSachCol", HeaderText = "Mã Sách", DataPropertyName = "MaSach", FillWeight = 12 });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn

            { Name = "TenSachCol", HeaderText = "Tên Sách", DataPropertyName = "TenSach", FillWeight = 28 });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn

            {

                Name = "SoLuongCol",

                HeaderText = "Lượng Tồn",

                DataPropertyName = "SoLuong",

                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight },

                FillWeight = 10

            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn

            {

                Name = "LuongBanCol",

                HeaderText = "Lượng Bán",

                DataPropertyName = "LuongBanDaTinh",

                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight },

                FillWeight = 10

            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn

            {

                Name = "DonGiaNhapCol",

                HeaderText = "Giá Nhập",

                DataPropertyName = "DonGiaNhap",

                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight },

                FillWeight = 13

            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn

            {

                Name = "DonGiaBanCol",

                HeaderText = "Giá Bán",

                DataPropertyName = "DonGiaBan",

                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight },

                FillWeight = 13

            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn

            { Name = "MaLoaiSachCol", HeaderText = "Mã Loại", DataPropertyName = "MaLoaiSach", FillWeight = 14 });

        }

        private string GetFilterTypeFromComboBox(ComboBox comboBox) // You'll need to add ComboBoxes to your form for this

        {

            if (comboBox != null && comboBox.SelectedItem != null)

            {

                string selected = comboBox.SelectedItem.ToString();

                if (selected.Contains("<=")) return "lessequal";

                if (selected.Contains(">=")) return "greaterequal";

                if (selected.Contains("==")) return "equal"; // Ensure your GetOperator handles "==" or "equal"

            }

            return "greaterequal"; // Default if no combobox or not selected, adjust as needed

        }



        private void bnthienthi_Click(object sender, EventArgs e)

        {

            LoadInventoryData();

        }

        private void LoadInventoryData()

        {

            DateTime? tuNgay = dtpTuNgay.Value;     // Assuming your DateTimePicker is named dtpTuNgay

            DateTime? denNgay = dtpDenNgay.Value;   // Assuming your DateTimePicker is named dtpDenNgay



            // Basic date validation

            if (tuNgay.HasValue && denNgay.HasValue && tuNgay.Value.Date > denNgay.Value.Date)

            {

                MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc.", "Lỗi Ngày", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;

            }



            int? filterSoLuongTon = null;

            // Placeholder: you'll need ComboBoxes on your UI for these filter types (e.g., cmbLuongTonFilterType)

            string filterSoLuongTonType = "greaterequal"; // Default, e.g., from a ComboBox like cmbLuongTonFilterType



            int? filterLuongBan = null;

            string filterLuongBanType = "greaterequal";  // Default, e.g., from a ComboBox like cmbLuongBanFilterType





            if (!string.IsNullOrWhiteSpace(txtLuongTon.Text))

            {

                if (int.TryParse(txtLuongTon.Text, out int parsedSoLuong))

                {

                    if (parsedSoLuong >= 0) { filterSoLuongTon = parsedSoLuong; }

                    else { MessageBox.Show("Lượng tồn phải là số không âm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtLuongTon.Focus(); return; }

                }

                else { MessageBox.Show("Lượng tồn không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtLuongTon.Focus(); return; }

            }



            if (!string.IsNullOrWhiteSpace(txtLuongBan.Text))

            {

                if (int.TryParse(txtLuongBan.Text, out int parsedLuongBan))

                {

                    if (parsedLuongBan >= 0) { filterLuongBan = parsedLuongBan; }

                    else { MessageBox.Show("Lượng bán phải là số không âm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtLuongBan.Focus(); return; }

                }

                else { MessageBox.Show("Lượng bán không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtLuongBan.Focus(); return; }

            }



            // Example: If you add ComboBoxes for filter types (e.g., cmbLuongTonFilterType, cmbLuongBanFilterType)

            // filterSoLuongTonType = GetFilterTypeFromComboBox(cmbLuongTonFilterType);

            // filterLuongBanType = GetFilterTypeFromComboBox(cmbLuongBanFilterType);





            try

            {

                List<Sach> inventory = inventoryDataAccess.GetInventoryItems(

                            tuNgay, denNgay, // Pass date range

                                                filterSoLuongTon, filterSoLuongTonType,

                            filterLuongBan, filterLuongBanType);



                dataGridView1.DataSource = null;

                if (inventory != null && inventory.Any())

                {

                    dataGridView1.DataSource = inventory;

                }

                else

                {

                    MessageBox.Show("Không tìm thấy sách nào thỏa mãn điều kiện.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }

            catch (Exception ex)

            {

                MessageBox.Show($"Đã xảy ra lỗi khi tải dữ liệu tồn kho: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

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

                    MessageBox.Show("Không thể khởi tạo ứng dụng Excel. Vui lòng kiểm tra cài đặt Office của bạn.", "Lỗi Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;

                }

                workbook = excelApp.Workbooks.Add(Type.Missing);

                worksheet = (Excel.Worksheet)workbook.ActiveSheet;

                worksheet.Name = "HangTonKho";



                // --- BEGIN: Thêm thông tin cửa hàng và báo cáo ---

                int currentRow = 1;

                // Sử dụng số cột của dataGridView1 nếu có, nếu không thì mặc định một số cột cho merge

                int numberOfColumnsToMerge = dataGridView1.Columns.Count > 0 ? dataGridView1.Columns.Count : 6; // Giả sử báo cáo có khoảng 6 cột chính



                string tenCuaHang = "Cửa Hàng Minh Châu";

                string diaChi = "Phượng Lích 1, Diễn Hoá, Diễn Châu, Nghệ An";

                string dienThoai = "0335549158";

                Excel.Range currentRange;



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

                currentRow++;



                string reportTitle = "BÁO CÁO HÀNG TỒN KHO";

                worksheet.Cells[currentRow, 1].Value = reportTitle;

                worksheet.Cells[currentRow, 1].Font.Bold = true;

                worksheet.Cells[currentRow, 1].Font.Size = 16;

                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; currentRow++;



                // dtpTuNgay, dtpDenNgay là tên DateTimePicker của bạn cho form này

                string dateRangeInfo = $"Từ ngày: {dtpTuNgay.Value:dd/MM/yyyy} Đến ngày: {dtpDenNgay.Value:dd/MM/yyyy}";

                worksheet.Cells[currentRow, 1].Value = dateRangeInfo;

                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; currentRow++;



                // txtLuongTon, txtLuongBan là tên TextBox filter của bạn

                string luongTonFilterInfo = txtLuongTon.Text.Trim();

                if (!string.IsNullOrEmpty(luongTonFilterInfo))

                {

                    worksheet.Cells[currentRow, 1].Value = $"Điều kiện lượng tồn: {luongTonFilterInfo}";

                    currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRow++;

                }

                string luongBanFilterInfo = txtLuongBan.Text.Trim();

                if (!string.IsNullOrEmpty(luongBanFilterInfo))

                {

                    worksheet.Cells[currentRow, 1].Value = $"Điều kiện lượng bán: {luongBanFilterInfo}";

                    currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRow++;

                }



                string exportDateInfo = $"Ngày xuất báo cáo: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

                worksheet.Cells[currentRow, 1].Value = exportDateInfo;

                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight; currentRow++;

                currentRow++;

                // --- END: Thêm thông tin ---



                if (dataGridView1.Columns.Count == 0)

                {

                    MessageBox.Show("Không có dữ liệu để xuất hoặc DataGridView chưa được cấu hình cột.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (workbook != null) { workbook.Close(false); } // Đóng nếu đã tạo

                    // COM objects sẽ được giải phóng trong finally

                    return; // Thoát sớm

                }



                // Ghi tiêu đề cột từ DataGridView

                for (int i = 0; i < dataGridView1.Columns.Count; i++)

                {

                    if (dataGridView1.Columns[i].Visible)

                    {

                        Excel.Range cell = worksheet.Cells[currentRow, i + 1];

                        cell.Value = dataGridView1.Columns[i].HeaderText;

                        cell.Font.Bold = true;

                        cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                        cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                        cell.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                    }

                }

                // currentRow++; // Không tăng ở đây, dữ liệu sẽ ghi từ dòng tiếp theo



                // Ghi dữ liệu dòng từ DataGridView

                for (int i = 0; i < dataGridView1.Rows.Count; i++)

                {

                    if (dataGridView1.Rows[i].IsNewRow) continue; // Bỏ qua dòng mới nếu có

                    for (int j = 0; j < dataGridView1.Columns.Count; j++)

                    {

                        if (dataGridView1.Columns[j].Visible)

                        {

                            var value = dataGridView1.Rows[i].Cells[j].Value;

                            Excel.Range currentCell = worksheet.Cells[i + currentRow + 1, j + 1];

                            if (value is float || value is double || value is decimal || value is int) { currentCell.NumberFormat = "#,##0"; currentCell.Value = value; }

                            else if (value is DateTime dateValue) { currentCell.NumberFormat = "dd/MM/yyyy"; currentCell.Value = dateValue; }

                            else if (DateTime.TryParse(value?.ToString(), out DateTime parsedDate)) { currentCell.NumberFormat = "dd/MM/yyyy"; currentCell.Value = parsedDate; }

                            else { currentCell.Value = value?.ToString(); }

                            currentCell.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                        }

                    }

                }

                worksheet.Columns.AutoFit();



                // --- Bước 1: Lưu vào file tạm ---

                tempExcelFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + "_HangTonKhoPreview.xlsx");

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

                      "Báo cáo Hàng Tồn Kho đã được mở để xem trước.\n\nBạn có muốn LƯU file này không?",

                      "Xác Nhận Lưu File Excel", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                }

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

                    SaveFileDialog saveFileDialog = new SaveFileDialog // Tạo mới khi cần

                    {

                        Filter = "Excel files (*.xlsx)|*.xlsx|Excel 97-2003 Workbook (*.xls)|*.xls",

                        Title = "Chọn nơi lưu file Báo Cáo Hàng Tồn Kho",

                        FileName = "BaoCaoHangTonKho.xlsx"

                    };



                    if (saveFileDialog.ShowDialog() == DialogResult.OK)

                    {

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

                MessageBox.Show("Lỗi khi xuất Excel: " + ex.ToString(), "Lỗi Xuất File", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            finally

            {

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



        // Đặt hàm này ở cuối class Form hoặc trong một class tiện ích

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



        private void bntbieudo_Click(object sender, EventArgs e, thongkehanghoa thongkehanghoa)

        {



        }



        private void bntbieudo_Click(object sender, EventArgs e)

        {

            LoadInventoryChart();

            chartInventoryReport.Visible = true; // Hiển thị chart sau khi tải



        }

        private void LoadInventoryChart()

        {

            DateTime? tuNgay = dtpTuNgay.Value;

            DateTime? denNgay = dtpDenNgay.Value;



            int? filterSoLuongTon = null;

            string filterSoLuongTonType = "greaterequal"; // Hoặc lấy từ ComboBox

            int? filterLuongBan = null;

            string filterLuongBanType = "greaterequal"; // Hoặc lấy từ ComboBox



            if (!string.IsNullOrWhiteSpace(txtLuongTon.Text)) { if (int.TryParse(txtLuongTon.Text, out int val) && val >= 0) filterSoLuongTon = val; else { /* Xử lý lỗi nhập liệu */ } }

            if (!string.IsNullOrWhiteSpace(txtLuongBan.Text)) { if (int.TryParse(txtLuongBan.Text, out int val) && val >= 0) filterLuongBan = val; else { /* Xử lý lỗi nhập liệu */ } }



            try

            {

                List<Sach> sachItems = inventoryDataAccess.GetInventoryItems(

                               tuNgay, denNgay,

                               filterSoLuongTon, filterSoLuongTonType,

                               filterLuongBan, filterLuongBanType);



                List<InventoryDataPoint> chartData = new List<InventoryDataPoint>();

                if (sachItems != null)

                {

                    chartData = sachItems

                      .Where(s => s.SoLuong > 0)

                      .Select(s => new InventoryDataPoint { TenSach = s.TenSach, LuongTon = s.SoLuong })

                      .OrderByDescending(dp => dp.LuongTon)

                      .ToList();

                }



                // Gọi hàm vẽ biểu đồ doughnut với tiêu đề mặc định

                PopulateInventoryPieChart(this.chartInventoryReport, chartData, "Tồn Kho Theo Mặt Hàng");

            }

            catch (Exception ex)

            {

                PopulateInventoryPieChart(this.chartInventoryReport, new List<InventoryDataPoint>(), "Tồn Kho Theo Mặt Hàng (Lỗi)");

                MessageBox.Show($"Lỗi khi tải dữ liệu cho biểu đồ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }



        // Hàm chung để vẽ biểu đồ Pie (tương tự PopulateDoughnutChart nhưng là Pie)

        private void PopulateInventoryPieChart(ChartControl chart, List<InventoryDataPoint> dataSource, string chartTitleText)

        {

            if (chart == null) return;



            chart.Series.Clear();

            chart.Titles.Clear();



            Legend legend;

            if (chart.Legends.Count > 0) { legend = chart.Legends[0]; }

            else { legend = new Legend(); chart.Legends.Add(legend); }

            legend.Name = "InventoryDoughnutLegend";

            legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right; // Legend bên phải

            legend.AlignmentVertical = LegendAlignmentVertical.Center;

            legend.Font = new Font("Arial", 8f); // Cỡ chữ legend (điều chỉnh nếu cần)

            // legend.MarkerMode = LegendMarkerMode.Marker; // Đảm bảo marker hiển thị (thường là mặc định)



            ChartTitle title = new ChartTitle();

            // Đặt tiêu đề biểu đồ theo phong cách "Tồn Kho Theo Mặt Hàng"

            title.Text = string.IsNullOrEmpty(chartTitleText) ? "Tồn Kho Theo Mặt Hàng" : chartTitleText;

            title.Font = new Font("Arial", 14, FontStyle.Bold); // Cỡ chữ tiêu đề lớn hơn

            chart.Titles.Add(title);



            Series seriesDoughnut = new Series("InventorySeries", ViewType.Doughnut);



            // *** THAY ĐỔI CẤU HÌNH CHO LEGEND VÀ LABEL ***

            // 1. Legend sẽ hiển thị tên mặt hàng (Argument)

            seriesDoughnut.LegendTextPattern = "{A}"; // {A} = Argument của SeriesPoint



            // 2. Nhãn trên lát cắt sẽ hiển thị "Tên Mặt Hàng: XX%"

            seriesDoughnut.Label.TextPattern = "{A}: {VP:P0}%"; // {A}=Argument, {VP:P0}=ValueAsPercent (0 số lẻ) + ký tự %

            seriesDoughnut.Label.LineVisibility = DevExpress.Utils.DefaultBoolean.True;

            if (seriesDoughnut.Label is PieSeriesLabel pieLabel) // Sử dụng PieSeriesLabel cho Doughnut

            {

                pieLabel.Position = PieSeriesLabelPosition.Outside;

                // Tùy chỉnh thêm cho đường nối và hộp nhãn nếu cần (ví dụ màu, kiểu đường)

                // pieLabel.ResolveOverlappingMode = ResolveOverlappingMode.Default;

            }

            // *** KẾT THÚC THAY ĐỔI CẤU HÌNH ***



            if (dataSource != null && dataSource.Any(dp => dp.LuongTon > 0))

            {

                double totalStock = dataSource.Where(dp => dp.LuongTon > 0).Sum(dp => (double)dp.LuongTon);

                int maxItemsToShow = 7; // Số mục chính hiển thị, còn lại gộp vào "Khác"

                var sortedData = dataSource.Where(dp => dp.LuongTon > 0)

                     .OrderByDescending(d => d.LuongTon)

                     .ToList();

                float otherValueTotal = 0;

                int itemsAddedToSeries = 0;



                for (int i = 0; i < sortedData.Count; i++)

                {

                    if (!string.IsNullOrEmpty(sortedData[i].TenSach))

                    {

                        if (itemsAddedToSeries < maxItemsToShow)

                        {

                            // Argument của SeriesPoint là TenSach

                            SeriesPoint point = new SeriesPoint(sortedData[i].TenSach,

                                new double[] { sortedData[i].LuongTon });

                            seriesDoughnut.Points.Add(point);

                            itemsAddedToSeries++;

                        }

                        else

                        {

                            otherValueTotal += sortedData[i].LuongTon;

                        }

                    }

                }



                if (otherValueTotal > 0 && itemsAddedToSeries >= maxItemsToShow)

                {

                    // Argument cho "Mặt hàng khác"

                    SeriesPoint otherPoint = new SeriesPoint("Mặt hàng khác",

                              new double[] { otherValueTotal });

                    seriesDoughnut.Points.Add(otherPoint);

                }

            }



            if (seriesDoughnut.Points.Count == 0)

            {

                title.Text = (string.IsNullOrEmpty(chartTitleText) ? "Tồn Kho Theo Mặt Hàng" : chartTitleText) + " (Không có dữ liệu)";

                seriesDoughnut.Points.Add(new SeriesPoint("Không có dữ liệu", 100));

                if (seriesDoughnut.Points.Count > 0) seriesDoughnut.Points[0].Color = Color.Gainsboro;



                seriesDoughnut.Label.TextPattern = "{A}";

                seriesDoughnut.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.False;

                legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

            }

            else

            {

                // Tooltip vẫn hiển thị đầy đủ: Tên Mặt Hàng: Số Lượng (Phần trăm)

                seriesDoughnut.ToolTipPointPattern = "{A}: {V:N0} ({VP:P2})";

                seriesDoughnut.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.True;



                legend.Visibility = DevExpress.Utils.DefaultBoolean.True;



                if (seriesDoughnut.View is DoughnutSeriesView doughnutView)

                {

                    doughnutView.HoleRadiusPercent = 50; // Điều chỉnh độ lớn của lỗ doughnut

                    // Tùy chọn: Tách lát cắt lớn nhất nếu có nhiều hơn 1 lát

                    if (seriesDoughnut.Points.Count > 1)

                    {

                        SeriesPoint pointToExplode = seriesDoughnut.Points.Cast<SeriesPoint>()

                                        .OrderByDescending(p => Math.Abs(Convert.ToDouble(p.Values[0])))

                                        .FirstOrDefault();

                        if (pointToExplode != null && pointToExplode.Argument.ToString() != "Không có dữ liệu")

                        {

                            doughnutView.ExplodedPoints.Clear();

                            doughnutView.ExplodedPoints.Add(pointToExplode);

                            doughnutView.ExplodedDistancePercentage = 10; // Khoảng cách tách (ví dụ 10%)

                        }

                    }

                    else

                    {

                        doughnutView.ExplodedPoints.Clear(); // Không explode nếu chỉ có 1 điểm

                    }

                }

            }



            chart.Series.Add(seriesDoughnut);

            chart.PaletteName = "Office 2013"; // Bạn có thể chọn Palette màu khác

            chart.RefreshData();

        }





        // --- Các hàm còn lại của bạn (Excel export, bntdong_Click) giữ nguyên ---

        // (Copy lại hàm bnttaoexel_Click và bntdong_Click từ code bạn đã cung cấp)







    }

}