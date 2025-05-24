using BTLtest2.Class;
using BTLtest2.function;
using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BTLtest2
{
    public partial class bieudo : Form
    {

        private DateTime _fromDate;
        private DateTime _toDate;

        public bieudo(DateTime fromDate, DateTime toDate)
        {
            InitializeComponent();
            _fromDate = fromDate;
            _toDate = toDate;

            // Gán sự kiện cho nút Thoát (nếu bạn đã thêm nút btnThoat trong designer)
            // this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
        }



        private void LoadChart(ChartControl chart, List<ThongKeItem> data, string valueField, string title)
        {
            chart.Series.Clear();
            Series series = new Series(title, ViewType.Doughnut);

            foreach (var item in data)
            {
                double value = 0;
                if (valueField == "DoanhThu") value = item.DoanhThu;
                else if (valueField == "ChiPhi") value = item.ChiPhi;
                else if (valueField == "LoiNhuan") value = item.LoiNhuan;

                if (value > 0)
                    series.Points.Add(new SeriesPoint(item.MaSach, value));
            }

            chart.Series.Add(series);
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            (series.View as DoughnutSeriesView).HoleRadiusPercent = 60;
        }
        
        private void chartControl1_Click(object sender, EventArgs e)
        {

        }

        private void chartControl2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void bieudo_Load(object sender, EventArgs e)
        {
            LoadAndDisplayCharts();
            chartControlDoanhThu.Visible = false;
            chartControlChiPhi.Visible = false;
            chartControlLoiNhuan.Visible = false;
           
        }
        private void LoadAndDisplayCharts()
        {
            try
            {
                // 1. Lấy và hiển thị biểu đồ Doanh Thu
                List<MatHangThongKe> doanhThuData = thongkedoanhthu.GetDoanhThuTheoMatHang(_fromDate, _toDate);
                if (doanhThuData.Any())
                {
                    PopulateDoughnutChart(chartControlDoanhThu, doanhThuData, "Doanh Thu Theo Mặt Hàng");
                }
                else
                {
                    ShowNoDataMessage(chartControlDoanhThu, "Không có dữ liệu doanh thu theo mặt hàng.");
                }


                // 2. Lấy và hiển thị biểu đồ Chi Phí
                List<MatHangThongKe> chiPhiData = thongkechiphi.GetChiPhiTheoMatHang(_fromDate, _toDate);
                if (chiPhiData.Any())
                {
                    PopulateDoughnutChart(chartControlChiPhi, chiPhiData, "Chi Phí Nhập Hàng Theo Mặt Hàng");
                }
                else
                {
                    ShowNoDataMessage(chartControlChiPhi, "Không có dữ liệu chi phí theo mặt hàng.");
                }


                // 3. Tính toán và hiển thị biểu đồ Lợi Nhuận
                List<MatHangThongKe> loiNhuanData = CalculateLoiNhuanTheoMatHang(doanhThuData, chiPhiData);
                if (loiNhuanData.Any())
                {
                    PopulateDoughnutChart(chartControlLoiNhuan, loiNhuanData, "Lợi Nhuận Theo Mặt Hàng");
                }
                else
                {
                    ShowNoDataMessage(chartControlLoiNhuan, "Không có dữ liệu lợi nhuận để hiển thị.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu biểu đồ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void PopulateDoughnutChart(ChartControl chart, List<MatHangThongKe> dataSource, string chartTitleText)
        {
            if (chart == null) return;

            chart.Series.Clear();
            chart.Titles.Clear();
            chart.Legends.Clear(); // << Đã sửa thành .Legends.Clear() - TỐT

            ChartTitle title = new ChartTitle();
            title.Text = chartTitleText;
            chart.Titles.Add(title);

            Series series = new Series(chartTitleText + "_Series", ViewType.Doughnut);

                                            // trong logic thêm điểm vào series.
            if (dataSource != null && dataSource.Any()) // dataSource có phần tử
            {
                int maxItemsToShow = 7;
                var sortedData = dataSource.OrderByDescending(d => d.GiaTri).ToList();
                float otherValueTotal = 0;
                int itemsAddedToSeries = 0;

                for (int i = 0; i < sortedData.Count; i++)
                {
                    bool isProfitChart = chartTitleText.Contains("Lợi Nhuận");
                    bool shouldAddPoint = (isProfitChart && sortedData[i].GiaTri != 0) || (!isProfitChart && sortedData[i].GiaTri > 0);

                    if (shouldAddPoint) // Nếu điểm này nên được thêm
                    {
                        if (itemsAddedToSeries < maxItemsToShow)
                        {
                            // QUAN TRỌNG: Cần kiểm tra sortedData[i].TenMatHang không null/empty
                            if (!string.IsNullOrEmpty(sortedData[i].TenMatHang))
                            {
                                series.Points.Add(new SeriesPoint(sortedData[i].TenMatHang, sortedData[i].GiaTri));
                                itemsAddedToSeries++;
                            }
                        }
                        else
                        {
                            otherValueTotal += sortedData[i].GiaTri;
                        }
                    }
                }

                if (otherValueTotal != 0 && itemsAddedToSeries == maxItemsToShow)
                {
                    series.Points.Add(new SeriesPoint("Mặt hàng khác", otherValueTotal));
                }
                // KẾT THÚC VÒNG LẶP VÀ XỬ LÝ dataSource
                // Tại đây, series.Points có thể vẫn rỗng nếu không có điểm nào thỏa mãn shouldAddPoint
                // hoặc nếu dataSource ban đầu có item nhưng TenMatHang bị null/empty.
            }

            // VẤN ĐỀ TIỀM ẨN:
            if (series.Points.Count == 0) // Nếu series không có điểm nào sau tất cả các xử lý
            {
                // A. Code hiện tại của bạn:
                title.Text = chartTitleText + " (Không có dữ liệu)";
                chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
                // -> Series rỗng vẫn được thêm vào chart ở dòng chart.Series.Add(series) bên dưới khối else
                //    (vì hasMeaningfulData không được cập nhật đúng)
                // -> Hoặc nếu hasMeaningfulData được dùng để điều khiển chart.Series.Add(series),
                //    thì series rỗng có thể không được add, nhưng flow code không rõ ràng.
            }
            // else // Khối else này trong code bạn cung cấp có thể không được thực thi đúng ý đồ
            // {
            //     hasMeaningfulData = true; // Dòng này cần được đặt đúng chỗ
            //     chart.Series.Add(series);
            //     // ... cấu hình appearance ...
            // }

            // Dựa trên code bạn cung cấp, có vẻ như chart.Series.Add(series) và các cấu hình
            // appearance nằm ngoài điều kiện kiểm tra series.Points.Count == 0 một cách rõ ràng.
            // => Điều này có thể dẫn đến việc thêm một Series rỗng vào biểu đồ.

            // CẦN ĐẢM BẢO:
            // 1. Nếu series.Points.Count == 0 sau khi xử lý dataSource,
            //    HOẶC LÀ thêm một điểm dữ liệu giả (dummy point) VÀO series,
            //    HOẶC LÀ KHÔNG THÊM series này vào chart.Series.
            // 2. `hasMeaningfulData` cần được cập nhật chính xác.

            // Đề xuất sửa logic ở đây:
            if (series.Points.Count == 0)
            {
                // Trường hợp không có điểm dữ liệu nào (kể cả "Mặt hàng khác")
                title.Text = chartTitleText + " (Không có dữ liệu)";
                // Thêm một điểm giả để DevExpress có gì đó để vẽ (và không bị lỗi)
                series.Points.Add(new SeriesPoint("Không có dữ liệu", 100)); // Giá trị 100 để vẽ thành 1 vòng tròn
                series.Points[0].Color = Color.Gainsboro; // Màu nhạt
                series.Label.TextPattern = "{A}"; // Hiển thị "Không có dữ liệu" trên lát cắt

                chart.Series.Add(series); // Thêm series chứa điểm giả
                chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False; // Ẩn legend cho trường hợp này
            }
            else // series.Points.Count > 0, có dữ liệu để vẽ
            {
                // hasMeaningfulData = true; // Không cần biến này nữa nếu cấu trúc như vầy
                chart.Series.Add(series);

                // --- Cấu hình Appearance chỉ khi có dữ liệu thực sự ---
                series.Label.TextPattern = "{A}: {VP:P2}";
                series.Label.ResolveOverlappingMode = ResolveOverlappingMode.Default;
                series.LegendTextPattern = "{A}";

                Legend legend = new Legend(chartTitleText + "_Legend");
                legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
                legend.AlignmentVertical = LegendAlignmentVertical.Center;
                chart.Legends.Add(legend);
                series.LegendName = legend.Name;
                legend.Visibility = DevExpress.Utils.DefaultBoolean.True;

                series.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.True;
                series.ToolTipPointPattern = "{A}: {V:N0} VNĐ";

                if (series.View is DoughnutSeriesView doughnutView)
                {
                    doughnutView.HoleRadiusPercent = 40;
                    if (series.Points.Count > 1) // Chỉ explode nếu có nhiều hơn 1 điểm
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
                chart.PaletteName = "Office 2013";
            }
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
