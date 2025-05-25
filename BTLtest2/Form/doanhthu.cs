using BTLtest2.Class;
using BTLtest2.function; // Đảm bảo các class baocaodoanhthu, baocaochiphi, thongkedoanhthu, thongkechiphi nằm trong namespace này
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

        private void doanhthu_Load(object sender, EventArgs e)
        {
            // Khởi tạo DateTimePickers (ví dụ)
            datestart.Value = DateTime.Now.Date.AddMonths(-1); // Lấy phần ngày, bỏ qua giờ
            dateend.Value = DateTime.Now.Date; // Lấy phần ngày

            // Khởi tạo các TextBox lọc
            txtThongKeTren.Text = "";
            txtThongKeDuoi.Text = "";

            // Ẩn các biểu đồ ban đầu
            chartControlDoanhThu.Visible = false;
            chartControlChiPhi.Visible = false;
            chartControlLoiNhuan.Visible = false;
            dataGridView1.DataSource = null; // Xóa dữ liệu cũ trên grid
            lbtongdoangthu.Text = "Tổng:"; // Reset label tổng
        }


        // Hàm hỗ trợ lấy giá trị lọc từ TextBox "Thống kê trên" và "Thống kê dưới"
        private bool TryGetFilterValues(out float? filterTren, out float? filterDuoi)
        {
            filterTren = null;
            filterDuoi = null;
            bool validTren = true;
            bool validDuoi = true;

            if (!string.IsNullOrWhiteSpace(txtThongKeTren.Text))
            {
                if (float.TryParse(txtThongKeTren.Text, out float trenValue))
                {
                    filterTren = trenValue;
                }
                else
                {
                    MessageBox.Show("Giá trị 'Thống kê trên' không hợp lệ.", "Lỗi Nhập Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtThongKeTren.Focus();
                    validTren = false;
                }
            }

            if (!string.IsNullOrWhiteSpace(txtThongKeDuoi.Text))
            {
                if (float.TryParse(txtThongKeDuoi.Text, out float duoiValue))
                {
                    filterDuoi = duoiValue;
                }
                else
                {
                    MessageBox.Show("Giá trị 'Thống kê dưới' không hợp lệ.", "Lỗi Nhập Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtThongKeDuoi.Focus();
                    validDuoi = false;
                }
            }

            if (filterTren.HasValue && filterDuoi.HasValue && filterTren.Value > filterDuoi.Value)
            {
                MessageBox.Show("'Thống kê trên' không thể lớn hơn 'Thống kê dưới'.", "Lỗi Lọc", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtThongKeTren.Focus();
                return false; // Trả về false nếu có lỗi logic giữa hai giá trị
            }

            return validTren && validDuoi; // Trả về true nếu cả hai (hoặc một trong hai nếu cái kia trống) hợp lệ hoặc trống
        }


        private void bnt_dt_Click(object sender, EventArgs e)
        {
            DateTime fromDate = datestart.Value.Date; // Chỉ lấy phần ngày
            DateTime toDate = dateend.Value.Date.AddDays(1).AddTicks(-1); // Đến cuối ngày đã chọn

            if (fromDate > toDate)
            {
                MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc.", "Lỗi Ngày", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!TryGetFilterValues(out float? filterTren, out float? filterDuoi))
            {
                return; // Dừng lại nếu giá trị lọc không hợp lệ
            }

            // Lấy dữ liệu doanh thu từ CSDL, truyền thêm các bộ lọc mới
            var data = baocaodoanhthu.GetDoanhThu(fromDate, toDate, filterTren, filterDuoi);
            dataGridView1.DataSource = data;

            // Tính tổng doanh thu từ dữ liệu đã lọc
            float tong = data?.Sum(dt => dt.TongTien) ?? 0; //Sử dụng null-conditional và null-coalescing
            lbtongdoangthu.Text = $"Tổng doanh thu: {tong:N0} VNĐ";

            // Tải và hiển thị chỉ biểu đồ doanh thu
            // Dữ liệu cho biểu đồ nên là dữ liệu chi tiết theo mặt hàng, không phải tổng hợp
            List<MatHangThongKe> doanhThuDataChart = thongkedoanhthu.GetDoanhThuTheoMatHang(fromDate, toDate, filterTren, filterDuoi);
            LoadDoanhThuChart(doanhThuDataChart); // Truyền dữ liệu đã lọc cho biểu đồ

            chartControlDoanhThu.Visible = true;
            chartControlChiPhi.Visible = false;
            chartControlLoiNhuan.Visible = false;
        }

        private void bnt_cp_Click(object sender, EventArgs e)
        {
            DateTime fromDate = datestart.Value.Date;
            DateTime toDate = dateend.Value.Date.AddDays(1).AddTicks(-1);

            if (fromDate > toDate)
            {
                MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc.", "Lỗi Ngày", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!TryGetFilterValues(out float? filterTren, out float? filterDuoi))
            {
                return;
            }

            // Lấy dữ liệu chi phí từ CSDL, truyền thêm các bộ lọc mới
            var data = baocaochiphi.GetChiPhi(fromDate, toDate, filterTren, filterDuoi);
            dataGridView1.DataSource = data;

            float tong = data?.Sum(dt => dt.TongTien) ?? 0;
            lbtongdoangthu.Text = $"Tổng chi phí: {tong:N0} VNĐ";

            // Dữ liệu cho biểu đồ chi phí theo mặt hàng
            List<MatHangThongKe> chiPhiDataChart = thongkechiphi.GetChiPhiTheoMatHang(fromDate, toDate, filterTren, filterDuoi);
            LoadChiPhiChart(chiPhiDataChart); // Truyền dữ liệu đã lọc cho biểu đồ

            chartControlDoanhThu.Visible = false;
            chartControlChiPhi.Visible = true;
            chartControlLoiNhuan.Visible = false;
        }

        private void bnt_ln_Click(object sender, EventArgs e)
        {
            dataloinhuan(); // Hàm này sẽ xử lý logic hiển thị lợi nhuận
        }

        private void dataloinhuan()
        {
            DateTime fromDate = datestart.Value.Date;
            DateTime toDate = dateend.Value.Date.AddDays(1).AddTicks(-1);

            if (fromDate > toDate)
            {
                MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc.", "Lỗi Ngày", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Lấy giá trị lọc "trên" và "dưới" từ TextBoxes
            if (!TryGetFilterValues(out float? filterTren, out float? filterDuoi))
            {
                return; // Dừng nếu giá trị lọc không hợp lệ
            }

            // 1. Lấy tổng doanh thu và tổng chi phí (ĐÃ LỌC theo fromDate, toDate, filterTren, filterDuoi cho từng hóa đơn)
            // GetDoanhThu và GetChiPhi phải được cập nhật để nhận filterTren, filterDuoi
            var overallRevenueData = baocaodoanhthu.GetDoanhThu(fromDate, toDate, filterTren, filterDuoi);
            var overallChiPhiData = baocaochiphi.GetChiPhi(fromDate, toDate, filterTren, filterDuoi);

            float tongDoanhThu = overallRevenueData?.Sum(dt => dt.TongTien) ?? 0;
            float tongChiPhi = overallChiPhiData?.Sum(cp => cp.TongTien) ?? 0;
            float loiNhuanTong = tongDoanhThu - tongChiPhi;
            lbtongdoangthu.Text = $"Lợi nhuận Tổng: {loiNhuanTong:N0} VNĐ";

            // 2. Lấy dữ liệu chi tiết theo mặt hàng để tính lợi nhuận theo mặt hàng
            // Các hàm GetDoanhThuTheoMatHang và GetChiPhiTheoMatHang cũng cần được cập nhật
            // để có thể nhận filterTren, filterDuoi (nếu muốn lọc ở mức mặt hàng)
            // Hoặc, tính toán lợi nhuận trước rồi lọc sau. Hiện tại, ta sẽ lọc trên lợi nhuận của từng mặt hàng.

            List<MatHangThongKe> doanhThuTheoMatHang = thongkedoanhthu.GetDoanhThuTheoMatHang(fromDate, toDate, null, null) ?? new List<MatHangThongKe>();
            List<MatHangThongKe> chiPhiTheoMatHang = thongkechiphi.GetChiPhiTheoMatHang(fromDate, toDate, null, null) ?? new List<MatHangThongKe>();
            List<MatHangThongKe> loiNhuanReportDataFull = CalculateLoiNhuanTheoMatHang(doanhThuTheoMatHang, chiPhiTheoMatHang);

            // Áp dụng bộ lọc "Thống kê trên" và "Thống kê dưới" cho lợi nhuận chi tiết của từng mặt hàng
            List<MatHangThongKe> loiNhuanReportDataFiltered = loiNhuanReportDataFull;
            if (filterTren.HasValue)
            {
                loiNhuanReportDataFiltered = loiNhuanReportDataFiltered.Where(item => item.GiaTri >= filterTren.Value).ToList();
            }
            if (filterDuoi.HasValue)
            {
                loiNhuanReportDataFiltered = loiNhuanReportDataFiltered.Where(item => item.GiaTri <= filterDuoi.Value).ToList();
            }

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
                // Không nên hiển thị MessageBox ở đây để tránh làm gián đoạn nếu lỗi nhỏ
            }

            LoadLoiNhuanChart(loiNhuanReportDataFiltered); // Truyền dữ liệu đã lọc cho biểu đồ

            chartControlDoanhThu.Visible = false;
            chartControlChiPhi.Visible = false;
            chartControlLoiNhuan.Visible = true;
        }


        // --- CÁC HÀM BIỂU ĐỒ (Cập nhật để nhận List<MatHangThongKe> đã được lọc nếu cần) ---
        private void LoadDoanhThuChart(List<MatHangThongKe> doanhThuData) // Nhận dữ liệu đã được xử lý
        {
            try
            {
                if (doanhThuData != null && doanhThuData.Any(item => item.GiaTri > 0)) // Chỉ hiển thị nếu có doanh thu > 0
                {
                    PopulateDoughnutChart(chartControlDoanhThu, doanhThuData, "Doanh Thu Theo Mặt Hàng");
                }
                else
                {
                    ShowNoDataMessage(chartControlDoanhThu, "Không có dữ liệu doanh thu theo mặt hàng (hoặc không có mục nào thỏa mãn điều kiện lọc).");
                }
            }
            catch (Exception ex)
            {
                ShowNoDataMessage(chartControlDoanhThu, "Lỗi tải dữ liệu doanh thu.");
                MessageBox.Show($"Lỗi khi tải dữ liệu biểu đồ doanh thu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChiPhiChart(List<MatHangThongKe> chiPhiData) // Nhận dữ liệu đã được xử lý
        {
            try
            {
                if (chiPhiData != null && chiPhiData.Any(item => item.GiaTri > 0)) // Chỉ hiển thị nếu có chi phí > 0
                {
                    PopulateDoughnutChart(chartControlChiPhi, chiPhiData, "Chi Phí Nhập Hàng Theo Mặt Hàng");
                }
                else
                {
                    ShowNoDataMessage(chartControlChiPhi, "Không có dữ liệu chi phí theo mặt hàng (hoặc không có mục nào thỏa mãn điều kiện lọc).");
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
                if (loiNhuanDataForChart != null && loiNhuanDataForChart.Any(item => item.GiaTri != 0)) // Chỉ hiển thị nếu có lợi nhuận khác 0
                {
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


        // --- Các hàm tiện ích (PopulateDoughnutChart, PopulateDevExpressBarChart, CalculateLoiNhuanTheoMatHang, ShowNoDataMessage, ReleaseComObject) giữ nguyên như bạn đã cung cấp ---
        // ... (Copy các hàm này từ code gốc của bạn vào đây) ...
        // Ví dụ:
        private void PopulateDoughnutChart(ChartControl chart, List<MatHangThongKe> dataSource, string chartTitleText)
        {
            // ... (Nội dung hàm PopulateDoughnutChart của bạn) ...
            // Đảm bảo hàm này lọc các dataSource có GiaTri <= 0 cho biểu đồ tròn Doanh thu/Chi phí
            if (chart == null) return;

            chart.Series.Clear();
            chart.Titles.Clear();
            chart.Legends.Clear(); // Đảm bảo xóa legend cũ

            ChartTitle title = new ChartTitle();
            title.Text = chartTitleText;
            chart.Titles.Add(title);

            Series series = new Series(chartTitleText + "_Series", ViewType.Doughnut);

            // Lọc bỏ các giá trị âm hoặc bằng 0 cho biểu đồ tròn (trừ khi là lợi nhuận)
            var displayData = dataSource;
            if (!chartTitleText.ToLower().Contains("lợi nhuận"))
            {
                displayData = dataSource?.Where(d => d.GiaTri > 0).ToList();
            }


            if (displayData != null && displayData.Any())
            {
                int maxItemsToShow = 7;
                var sortedData = displayData.OrderByDescending(d => Math.Abs(d.GiaTri))
                                           .ToList();
                float otherValueTotal = 0;
                int itemsAddedToSeries = 0;

                for (int i = 0; i < sortedData.Count; i++)
                {
                    if (!string.IsNullOrEmpty(sortedData[i].TenMatHang))
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

                if (otherValueTotal != 0 && itemsAddedToSeries >= maxItemsToShow)
                {
                    series.Points.Add(new SeriesPoint("Mặt hàng khác", otherValueTotal));
                }
            }

            if (series.Points.Count == 0)
            {
                title.Text = chartTitleText + " (Không có dữ liệu)";
                series.Points.Add(new SeriesPoint("Không có dữ liệu", 100));
                if (series.Points.Count > 0)
                {
                    series.Points[0].Color = Color.Gainsboro;
                }
                series.Label.TextPattern = "{A}";
                series.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.False;
                chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            }
            else
            {
                series.Label.TextPattern = "{A}: {VP:P0}";
                series.LegendTextPattern = "{A}";
                series.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.True;
                series.ToolTipPointPattern = "{A}: {V:N0} VNĐ";

                if (series.View is DoughnutSeriesView doughnutView)
                {
                    doughnutView.HoleRadiusPercent = 40;
                    if (series.Points.Count > 1)
                    {
                        SeriesPoint pointToExplode = series.Points.Cast<SeriesPoint>()
                                                        .OrderByDescending(p => Math.Abs(Convert.ToDouble(p.Values[0])))
                                                        .FirstOrDefault();
                        if (pointToExplode != null && pointToExplode.Argument.ToString() != "Không có dữ liệu")
                        {
                            doughnutView.ExplodedPoints.Clear();
                            doughnutView.ExplodedPoints.Add(pointToExplode);
                            doughnutView.ExplodedDistancePercentage = 10; // Thêm dòng này nếu thiếu
                        }
                    }
                    else if (series.View is DoughnutSeriesView dView) // Chắc chắn rằng không explode khi chỉ có 1 điểm
                    {
                        dView.ExplodedPoints.Clear();
                    }
                }
                chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
            }

            chart.Series.Add(series);
            chart.PaletteName = "Office 2013";
            chart.RefreshData(); // Thêm dòng này để đảm bảo biểu đồ được làm mới
        }
        private void PopulateDevExpressBarChart(DevExpress.XtraCharts.ChartControl chartControl, List<MatHangThongKe> data, string chartTitleText)
        {
            // ... (Nội dung hàm PopulateDevExpressBarChart của bạn) ...
            chartControl.Series.Clear();
            chartControl.Titles.Clear();
            chartControl.AnnotationRepository.Clear();
            chartControl.CustomDrawSeriesPoint -= ChartControl_LoiNhuan_CustomDrawSeriesPoint;

            if (data == null || !data.Any(item => item.GiaTri != 0)) // Kiểm tra xem có dữ liệu nào khác 0 không
            {
                ShowNoDataMessage(chartControl, chartTitleText + " (Không có dữ liệu thỏa mãn).");
                return;
            }

            Series seriesLoiNhuan = new Series("Lợi Nhuận", ViewType.Bar);
            // Chỉ thêm các điểm có giá trị khác 0 vào series
            foreach (var item in data.Where(d => d.GiaTri != 0))
            {
                seriesLoiNhuan.Points.Add(new SeriesPoint(item.TenMatHang, item.GiaTri));
            }

            // Nếu sau khi lọc mà không còn điểm nào thì cũng hiển thị "Không có dữ liệu"
            if (!seriesLoiNhuan.Points.Any())
            {
                ShowNoDataMessage(chartControl, chartTitleText + " (Không có dữ liệu thỏa mãn).");
                return;
            }


            chartControl.Series.Add(seriesLoiNhuan);
            ChartTitle chartTitle = new ChartTitle { Text = chartTitleText };
            chartControl.Titles.Add(chartTitle);

            XYDiagram diagram = chartControl.Diagram as XYDiagram;
            if (diagram != null)
            {
                diagram.AxisX.Title.Text = "Mặt Hàng";
                diagram.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                diagram.AxisX.Label.TextPattern = "{A}";
                diagram.AxisX.Label.ResolveOverlappingOptions.AllowHide = false;
                diagram.AxisX.Label.ResolveOverlappingOptions.AllowRotate = true;
                diagram.AxisX.Label.ResolveOverlappingOptions.AllowStagger = true;
                diagram.AxisX.Label.Angle = 30;
                diagram.AxisX.Label.MaxWidth = 100; // Giới hạn chiều rộng của nhãn
                diagram.AxisX.Label.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True; // Cho chữ mượt hơn


                diagram.AxisY.Title.Text = "Lợi Nhuận (VNĐ)";
                diagram.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
                diagram.AxisY.Label.TextPattern = "{V:N0}";
            }

            chartControl.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;
            chartControl.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
            chartControl.CustomDrawSeriesPoint += ChartControl_LoiNhuan_CustomDrawSeriesPoint;
            chartControl.RefreshData();
        }
        private void ChartControl_LoiNhuan_CustomDrawSeriesPoint(object sender, CustomDrawSeriesPointEventArgs e)
        {
            // ... (Nội dung hàm ChartControl_LoiNhuan_CustomDrawSeriesPoint của bạn) ...
            double value = e.SeriesPoint.Values[0];
            BarDrawOptions drawOptions = e.SeriesDrawOptions as BarDrawOptions;
            if (drawOptions == null) return;

            if (value < 0)
            {
                drawOptions.Color = Color.FromArgb(255, 102, 102); // Màu đỏ cho lỗ
            }
            else
            {
                drawOptions.Color = Color.FromArgb(96, 181, 201); // Màu xanh cho lãi
            }
        }
        private List<MatHangThongKe> CalculateLoiNhuanTheoMatHang(List<MatHangThongKe> doanhThuList, List<MatHangThongKe> chiPhiList)
        {
            // ... (Nội dung hàm CalculateLoiNhuanTheoMatHang của bạn) ...
            var loiNhuanDictionary = new Dictionary<string, float>();

            if (doanhThuList != null)
            {
                foreach (var dtItem in doanhThuList)
                {
                    if (!loiNhuanDictionary.ContainsKey(dtItem.TenMatHang))
                    {
                        loiNhuanDictionary[dtItem.TenMatHang] = 0;
                    }
                    loiNhuanDictionary[dtItem.TenMatHang] += dtItem.GiaTri;
                }
            }

            if (chiPhiList != null)
            {
                foreach (var cpItem in chiPhiList)
                {
                    if (!loiNhuanDictionary.ContainsKey(cpItem.TenMatHang))
                    {
                        loiNhuanDictionary[cpItem.TenMatHang] = 0;
                    }
                    loiNhuanDictionary[cpItem.TenMatHang] -= cpItem.GiaTri;
                }
            }
            return loiNhuanDictionary
                .Select(kvp => new MatHangThongKe { TenMatHang = kvp.Key, GiaTri = kvp.Value })
                .OrderByDescending(item => item.GiaTri)
                .ToList();
        }
        private void ShowNoDataMessage(ChartControl chartControl, string message)
        {
            // ... (Nội dung hàm ShowNoDataMessage của bạn) ...
            if (chartControl == null) return;
            chartControl.Titles.Clear();
            chartControl.Series.Clear();
            chartControl.Legends.Clear();

            ChartTitle title = new ChartTitle();
            title.Text = message;
            chartControl.Titles.Add(title);
            chartControl.RefreshData(); // Thêm dòng này
        }
        private void ReleaseComObject(object obj)
        {
            // ... (Nội dung hàm ReleaseComObject của bạn) ...
            try
            {
                if (obj != null && Marshal.IsComObject(obj))
                {
                    Marshal.FinalReleaseComObject(obj); //Sử dụng FinalReleaseComObject
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

        private void bnttaoexel_Click(object sender, EventArgs e)
        {
            // ... (Nội dung hàm bnttaoexel_Click của bạn, đã được sửa đổi ở trên) ...
            // Cần cập nhật phần lấy điều kiện lọc từ txtThongKeTren và txtThongKeDuoi
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

                string excelSheetName = "BaoCao";
                string excelReportTitle = "BÁO CÁO TỔNG HỢP";
                string excelDefaultFileName = "BaoCao.xlsx";

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
                worksheet.Name = excelSheetName;

                int currentRow = 1;
                // Số cột để merge, điều chỉnh dựa trên số cột DataGridView của bạn, ví dụ: 5 cột
                DataGridView dgvToExport = this.Controls.Find("dataGridView1", true).FirstOrDefault() as DataGridView;
                int numberOfColumnsToMerge = (dgvToExport != null && dgvToExport.Columns.GetColumnCount(DataGridViewElementStates.Visible) > 0)
                                           ? dgvToExport.Columns.GetColumnCount(DataGridViewElementStates.Visible)
                                           : 5; // Mặc định là 5 nếu không có DGV hoặc không có cột nào hiển thị


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

                worksheet.Cells[currentRow, 1].Value = excelReportTitle;
                worksheet.Cells[currentRow, 1].Font.Bold = true;
                worksheet.Cells[currentRow, 1].Font.Size = 16;
                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; currentRow++;

                Control dateStartControl = this.Controls.Find("datestart", true).FirstOrDefault();
                Control dateEndControl = this.Controls.Find("dateend", true).FirstOrDefault();
                if (dateStartControl is DateTimePicker dtpStart && dateEndControl is DateTimePicker dtpEnd)
                {
                    string dateRangeInfo = $"Từ ngày: {dtpStart.Value:dd/MM/yyyy} Đến ngày: {dtpEnd.Value:dd/MM/yyyy}";
                    worksheet.Cells[currentRow, 1].Value = dateRangeInfo;
                    currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; currentRow++;
                }

                // Thêm điều kiện lọc từ txtThongKeTren và txtThongKeDuoi
                string thongKeTrenInfo = txtThongKeTren.Text.Trim();
                if (!string.IsNullOrEmpty(thongKeTrenInfo))
                {
                    worksheet.Cells[currentRow, 1].Value = $"Điều kiện thống kê trên: >= {thongKeTrenInfo}";
                    currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRow++;
                }
                string thongKeDuoiInfo = txtThongKeDuoi.Text.Trim();
                if (!string.IsNullOrEmpty(thongKeDuoiInfo))
                {
                    worksheet.Cells[currentRow, 1].Value = $"Điều kiện thống kê dưới: <= {thongKeDuoiInfo}";
                    currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRow++;
                }
                // Bỏ dòng này nếu không còn textbox 'tongdt' cho mục đích lọc chung
                // Control tongDtControl = this.Controls.Find("tongdt", true).FirstOrDefault(); 
                // ...

                string exportDateInfo = $"Ngày xuất báo cáo: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                worksheet.Cells[currentRow, 1].Value = exportDateInfo;
                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfColumnsToMerge]]; currentRange.Merge(); currentRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                currentRow++;
                if (lbtongdoangthu != null && !string.IsNullOrWhiteSpace(lbtongdoangthu.Text) && lbtongdoangthu.Text.Contains(":"))
                {
                    // ... (logic ghi dòng tổng, giữ nguyên) ...
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
                    string parsableValueStr = displayTotalValueStr.Replace(",", ""); // Bỏ dấu phẩy nếu có
                    if (decimal.TryParse(parsableValueStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), out decimal numericTotalValue) || // Thử với culture vi-VN
                        decimal.TryParse(parsableValueStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out numericTotalValue)) // Rồi thử InvariantCulture
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

                    // Merge các ô cho dòng tổng nếu numberOfColumnsToMerge > 2
                    if (numberOfColumnsToMerge > 2)
                    {
                        Excel.Range labelCell = worksheet.Cells[currentRow, 1];
                        Excel.Range valueCell = worksheet.Cells[currentRow, 2];
                        // Merge label (nếu cần, nhưng thường label chỉ 1 cột)
                        // Merge value cells
                        Excel.Range valueMergeRange = worksheet.Range[valueCell, worksheet.Cells[currentRow, numberOfColumnsToMerge]];
                        // valueMergeRange.Merge(); // Chỉ merge nếu bạn muốn giá trị chiếm nhiều cột
                    }
                }
                currentRow++;

                if (dgvToExport == null || dgvToExport.Columns.GetColumnCount(DataGridViewElementStates.Visible) == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất hoặc DataGridView không tồn tại/chưa được cấu hình.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (workbook != null) workbook.Close(false); if (excelApp != null) excelApp.Quit();
                    ReleaseComObject(worksheet); ReleaseComObject(workbook); ReleaseComObject(excelApp);
                    return;
                }

                int headerStartRow = currentRow;
                int visibleColIndex = 1; // Để theo dõi chỉ số cột trong Excel cho các cột hiển thị
                for (int i = 0; i < dgvToExport.Columns.Count; i++)
                {
                    if (dgvToExport.Columns[i].Visible)
                    {
                        Excel.Range cell = worksheet.Cells[headerStartRow, visibleColIndex];
                        cell.Value = dgvToExport.Columns[i].HeaderText;
                        cell.Font.Bold = true; cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                        cell.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        visibleColIndex++;
                    }
                }
                for (int i = 0; i < dgvToExport.Rows.Count; i++)
                {
                    if (dgvToExport.Rows[i].IsNewRow) continue; //Bỏ qua dòng mới (nếu có)
                    visibleColIndex = 1;
                    for (int j = 0; j < dgvToExport.Columns.Count; j++)
                    {
                        if (dgvToExport.Columns[j].Visible)
                        {
                            var value = dgvToExport.Rows[i].Cells[j].Value;
                            Excel.Range currentCell = worksheet.Cells[i + headerStartRow + 1, visibleColIndex];
                            if (value is float || value is double || value is decimal || value is int || value is long || value is short || value is byte)
                            {
                                currentCell.NumberFormat = "#,##0"; currentCell.Value = value;
                            }
                            else if (value is DateTime dateValue) { currentCell.NumberFormat = "dd/MM/yyyy"; currentCell.Value = dateValue; }
                            else if (value != null && DateTime.TryParse(value.ToString(), out DateTime parsedDate)) { currentCell.NumberFormat = "dd/MM/yyyy"; currentCell.Value = parsedDate; }
                            else { currentCell.Value = value?.ToString(); }
                            currentCell.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                            visibleColIndex++;
                        }
                    }
                }
                worksheet.Columns.AutoFit();
                // ... (Phần còn lại của logic lưu file, xem trước, vv. giữ nguyên) ...
                tempExcelFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + "_" + excelDefaultFileName);
                workbook.SaveAs(tempExcelFilePath);

                if (worksheet != null) { Marshal.FinalReleaseComObject(worksheet); worksheet = null; }
                if (workbook != null) { workbook.Close(false); Marshal.FinalReleaseComObject(workbook); workbook = null; }
                if (excelApp != null) { excelApp.Quit(); Marshal.FinalReleaseComObject(excelApp); excelApp = null; }

                GC.Collect();
                GC.WaitForPendingFinalizers();


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
                        FileName = excelDefaultFileName
                    };

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            if (excelProcess != null && !excelProcess.HasExited)
                            {
                                try { excelProcess.CloseMainWindow(); excelProcess.WaitForExit(2000); } catch { /* ignore */ }
                                if (!excelProcess.HasExited) { try { excelProcess.Kill(); } catch {/* ignore */} }
                            }
                            File.Copy(tempExcelFilePath, saveFileDialog.FileName, true);
                            MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            try { Process.Start(new ProcessStartInfo(saveFileDialog.FileName) { UseShellExecute = true }); } //Sử dụng UseShellExecute
                            catch (Exception exOpenFinal) { MessageBox.Show("Không thể mở file Excel đã lưu: " + exOpenFinal.Message, "Lỗi Mở File", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                        }
                        catch (IOException ioEx) { MessageBox.Show($"Lỗi khi lưu file Excel: {ioEx.Message}\nVui lòng đóng file Excel xem trước (nếu đang mở) và thử lại.", "Lỗi Lưu File", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        catch (Exception exSaveFinal) { MessageBox.Show("Lỗi khi lưu file Excel cuối cùng: " + exSaveFinal.Message, "Lỗi Lưu File", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                    else { MessageBox.Show("Thao tác lưu file Excel đã được hủy.", "Đã hủy", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
                else { MessageBox.Show("Thao tác xuất file Excel đã được hủy hoặc không lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất Excel: " + ex.ToString(), "Lỗi Xuất File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ReleaseComObject(worksheet); // worksheet đã được gán null ở trên nhưng gọi lại không sao
                ReleaseComObject(workbook);  // workbook đã được gán null ở trên
                ReleaseComObject(excelApp);  // excelApp đã được gán null ở trên

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

        private void bntdong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Bỏ trống các hàm không dùng đến hoặc đã tích hợp
        private void bnthienthi_Click(object sender, EventArgs e) { /* Có thể dùng để refresh chung nếu cần */ }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) { /* Logic chi tiết hóa đơn có thể ở đây */ }
        private void bntbieudo_Click(object sender, EventArgs e)
        {
            // Nút này có thể không cần thiết nữa nếu biểu đồ tự động hiển thị
            // Hoặc dùng để chuyển đổi giữa các loại biểu đồ nếu muốn
            // Hiện tại, các nút Doanh thu, Chi phí, Lợi nhuận đã xử lý việc hiển thị biểu đồ tương ứng.
        }
        private void chartControlChiPhi_Click(object sender, EventArgs e) { /* Xử lý sự kiện click trên biểu đồ nếu cần */ }

    }
}