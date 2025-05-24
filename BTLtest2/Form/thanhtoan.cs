using BTLtest2.function;
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
    public partial class thanhtoan : Form
    {
        private fcthanhtoan dataService;
        private string selectedSoHDBan = null;
        private string selectedPaymentMethodForPendingConfirmation = ""; // To store method if waiting for "Thanh toán thành công"

        public thanhtoan()
        {
            InitializeComponent();
            dataService = new fcthanhtoan();
        }

        private void thanhtoan_Load(object sender, EventArgs e)
        {
            if (picChuyenKhoan != null) picChuyenKhoan.Visible = false;
            if (btnThanhToanThanhCong != null) btnThanhToanThanhCong.Visible = false; // Hide new button
            if (radTienMat != null) radTienMat.Checked = true;
            if (dgvChiTietHoaDon != null) dgvChiTietHoaDon.Visible = false;

            LoadHoaDonData();
        }
        private void LoadHoaDonData()
        {
            try
            {
                DataTable dtHoaDon = dataService.GetHoaDonChuaThanhToan();
                if (dgvHoaDon != null)
                {
                    dgvHoaDon.DataSource = dtHoaDon;
                    dgvHoaDon.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách hoá đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ClearInvoiceSelectionAndDetails();
            }
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a valid row is clicked (not the header row and within bounds)
            if (e.RowIndex >= 0 && e.RowIndex < dgvHoaDon.Rows.Count && dgvHoaDon.Rows[e.RowIndex].Cells["SoHDBan"].Value != null)
            {
                ResetPaymentConfirmationState(); // Reset if user clicks a new invoice while one transfer is pending
                DataGridViewRow row = dgvHoaDon.Rows[e.RowIndex];
                selectedSoHDBan = row.Cells["SoHDBan"].Value.ToString();

                if (cmbMaHoaDon != null) cmbMaHoaDon.Text = selectedSoHDBan;
                if (txtTongTien != null)
                {
                    txtTongTien.Text = (row.Cells["TongTien"].Value != DBNull.Value) ? Convert.ToDecimal(row.Cells["TongTien"].Value).ToString("N0") : "0";
                }

                LoadChiTietData(selectedSoHDBan);
                if (dgvChiTietHoaDon != null) dgvChiTietHoaDon.Visible = true;
            }
            else
            {
                ClearInvoiceSelectionAndDetails(); // Clears selection and hides details grid
            }
        }
        private void LoadChiTietData(string soHDBan)
        {
            try
            {
                DataTable dtChiTiet = dataService.GetChiTietHoaDon(soHDBan);
                if (dgvChiTietHoaDon != null)
                {
                    dgvChiTietHoaDon.DataSource = dtChiTiet;
                    dgvChiTietHoaDon.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết hoá đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (dgvChiTietHoaDon != null) dgvChiTietHoaDon.DataSource = null;
            }
        }

        // A single event handler for both radio buttons to manage QR code visibility
        private void radPayment_CheckedChanged(object sender, EventArgs e)
        {
            if (picChuyenKhoan == null || btnThanhToanThanhCong == null) return;

            RadioButton rb = sender as RadioButton;
            if (rb != null && rb.Checked)
            {
                // If user switches payment method while a "Chuyen Khoan" confirmation is pending, reset.
                if (btnThanhToanThanhCong.Visible)
                {
                    ResetPaymentConfirmationState();
                    // Keep picChuyenKhoan visible only if "Chuyen Khoan" is still selected
                    picChuyenKhoan.Visible = (radChuyenKhoan != null && radChuyenKhoan.Checked);
                }
                else // Normal behavior when not in pending confirmation
                {
                    picChuyenKhoan.Visible = (radChuyenKhoan != null && radChuyenKhoan.Checked);
                }
            }
        }

        private void bntxacnhan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedSoHDBan))
            {
                MessageBox.Show("Vui lòng chọn một hoá đơn để thanh toán.", "Chưa chọn hoá đơn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (radTienMat != null && radTienMat.Checked)
            {
                ProcessPayment("Tiền mặt");
            }
            else if (radChuyenKhoan != null && radChuyenKhoan.Checked)
            {
                // Show QR and "Thanh toán thành công" button
                if (picChuyenKhoan != null) picChuyenKhoan.Visible = true;
                if (btnThanhToanThanhCong != null) btnThanhToanThanhCong.Visible = true;
                if (btnXacNhanThanhToan != null) btnXacNhanThanhToan.Enabled = false; // Disable "Xác nhận" until resolved
                selectedPaymentMethodForPendingConfirmation = "Chuyển khoản";
                MessageBox.Show("Vui lòng quét mã QR để chuyển khoản và sau đó nhấn 'Thanh toán thành công'.", "Chuyển khoản", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn phương thức thanh toán.", "Chưa chọn PTTT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void ClearInvoiceSelectionAndDetails()
        {
            selectedSoHDBan = null;
            if (cmbMaHoaDon != null) { cmbMaHoaDon.SelectedIndex = -1; cmbMaHoaDon.Text = string.Empty; }
            if (txtTongTien != null) txtTongTien.Clear();

            if (dgvChiTietHoaDon != null)
            {
                dgvChiTietHoaDon.DataSource = null;
                dgvChiTietHoaDon.Visible = false;
            }

            if (dgvHoaDon != null && dgvHoaDon.CurrentRow != null) dgvHoaDon.ClearSelection();
            if (radTienMat != null) radTienMat.Checked = true; // Resets payment method, which hides QR via event

            ResetPaymentConfirmationState(); // Ensure pending confirmation UI is reset

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearInvoiceSelectionAndDetails(); // This now also calls ResetPaymentConfirmationState

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThanhToanThanhCong_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedSoHDBan) || string.IsNullOrEmpty(selectedPaymentMethodForPendingConfirmation))
            {
                MessageBox.Show("Không có thông tin thanh toán chờ xử lý hoặc hoá đơn không được chọn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetPaymentConfirmationState();
                return;
            }

            // Process payment for the stored method (should be "Chuyển khoản")
            ProcessPayment(selectedPaymentMethodForPendingConfirmation);
            ResetPaymentConfirmationState();
        }
        private void ProcessPayment(string trangThaiMoi)
        {
            if (string.IsNullOrEmpty(selectedSoHDBan)) return; // Should have been checked before

            try
            {
                bool success = dataService.UpdateTrangThaiHoaDon(selectedSoHDBan, trangThaiMoi);
                if (success)
                {
                    MessageBox.Show($"Thanh toán bằng '{trangThaiMoi}' thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadHoaDonData(); // Refreshes list and calls ClearInvoiceSelectionAndDetails
                }
                else
                {
                    MessageBox.Show("Không tìm thấy hoá đơn để cập nhật hoặc trạng thái không thay đổi.", "Không thể cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật trạng thái ({trangThaiMoi}): " + ex.Message, "Lỗi cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure the main confirm button is re-enabled if it was disabled
                // ClearInvoiceSelectionAndDetails will handle hiding QR/Thanh toán thành công button
                // if LoadHoaDonData successfully calls it.
                // Additional explicit reset might be needed if ProcessPayment is called directly elsewhere.
                if (btnXacNhanThanhToan != null) btnXacNhanThanhToan.Enabled = true;
                if (btnThanhToanThanhCong != null) btnThanhToanThanhCong.Visible = false;
                if (picChuyenKhoan != null && trangThaiMoi != "Chuyển khoản") picChuyenKhoan.Visible = false;
                selectedPaymentMethodForPendingConfirmation = "";
            }
        }

        private void ResetPaymentConfirmationState()
        {
            if (btnXacNhanThanhToan != null) btnXacNhanThanhToan.Enabled = true;
            if (btnThanhToanThanhCong != null) btnThanhToanThanhCong.Visible = false;
            // picChuyenKhoan visibility will be handled by radPayment_CheckedChanged or if Tiền mặt is processed
            if (radChuyenKhoan != null && !radChuyenKhoan.Checked && picChuyenKhoan != null)
            {
                picChuyenKhoan.Visible = false;
            }
            selectedPaymentMethodForPendingConfirmation = "";
        }

    }
}
