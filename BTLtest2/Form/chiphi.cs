using BTLtest2.Class;
using BTLtest2.function;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Globalization;
using System.Runtime.InteropServices;

namespace BTLtest2
{
    public partial class chiphi : Form
    {
        public chiphi()
        {
            InitializeComponent();
        }


        private void bntbieudo_Click(object sender, EventArgs e)
        {


            DateTime fromDate = datestart.Value;
            DateTime toDate = dateend.Value;
            float chiPhiMin = 0;

            float.TryParse(tongcp.Text, out chiPhiMin);

            bieudo bdForm = new bieudo(fromDate, toDate, chiPhiMin);
            bdForm.ShowDialog();
        }

        private void bnthienthi_Click(object sender, EventArgs e)
        {
            DateTime fromDate = datestart.Value;
            DateTime toDate = dateend.Value;
            float chiPhiMin = 0;

            if (!float.TryParse(tongcp.Text, out chiPhiMin))
            {
                chiPhiMin = 0; // Mặc định nếu không nhập
            }

            var data = baocaochiphi.GetChiPhi(fromDate, toDate, chiPhiMin);
            dataGridView1.DataSource = data;
            // Tính tổng chi phí
            float tong = data.Sum(cp => cp.TongTien);

            // Hiển thị lên label
            tongchiphi.Text = $"Tổng chi phí: {tong:N0} VNĐ";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string soHDNhap = dataGridView1.Rows[e.RowIndex].Cells[0].Value?.ToString();
                MessageBox.Show($"Số HĐ nhập: {soHDNhap}");

                if (!string.IsNullOrEmpty(soHDNhap))
                {
                    chitiethoadonnhap form = new chitiethoadonnhap(soHDNhap);
                    form.ShowDialog();
                }
            }
        }

        private void bnttaoexel_Click(object sender, EventArgs e)
        {
            try
            {
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook workbook = excelApp.Workbooks.Add(Type.Missing);
                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.ActiveSheet;
                worksheet.Name = "ChiTiet";

                // Ghi tiêu đề cột
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    Excel.Range cell = worksheet.Cells[1, i + 1];
                    cell.Value = dataGridView1.Columns[i].HeaderText;
                    cell.Font.Bold = true;
                    cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(Color.LightGray);
                }

                // Ghi dữ liệu từng dòng
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        var value = dataGridView1.Rows[i].Cells[j].Value;

                        if (value is float || value is double || value is decimal)
                        {
                            worksheet.Cells[i + 2, j + 1].NumberFormat = "#,##0.00";
                            worksheet.Cells[i + 2, j + 1].Value = value;
                        }
                        else if (DateTime.TryParse(value?.ToString(), out DateTime date))
                        {
                            worksheet.Cells[i + 2, j + 1].NumberFormat = "dd/MM/yyyy";
                            worksheet.Cells[i + 2, j + 1].Value = date;
                        }
                        else
                        {
                            worksheet.Cells[i + 2, j + 1].Value = value?.ToString();
                        }
                    }
                }

                // Autofit
                worksheet.Columns.AutoFit();

                // Chọn nơi lưu
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveFileDialog.Title = "Chọn nơi lưu file Excel";
                saveFileDialog.FileName = "ChiTietHoaDon.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    workbook.SaveAs(saveFileDialog.FileName);
                    workbook.Close();
                    excelApp.Quit();

                    // Giải phóng COM object
                    Marshal.ReleaseComObject(worksheet);
                    Marshal.ReleaseComObject(workbook);
                    Marshal.ReleaseComObject(excelApp);

                    MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Mở file
                    System.Diagnostics.Process.Start(saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất Excel: " + ex.Message);
            }
        }

      
    }
}