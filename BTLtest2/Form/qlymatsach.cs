using BTLtest2.Class;
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
    public partial class qlymatsach : Form
    {
        private fmatsach fmatsach;
        private MatSachData originalSelectedMatSach;
        private bool isInAddMode = false; // Biến cờ để theo dõi trạng thái thêm mới
        public qlymatsach()
        {
            InitializeComponent();
            fmatsach = new fmatsach();
        }

        private void qlymatsach_Load(object sender, EventArgs e)
        {
            LoadSachComboBox(); // Tải danh sách sách vào ComboBox
            LoadMatSachGrid();  // Tải dữ liệu mất sách vào DataGridView
            ClearForm();        // Xóa trắng các trường nhập liệu
            SetFormState("initial"); // Đặt trạng thái ban đầu cho form ("initial", "adding", "editing")
        }
        private void LoadSachComboBox()
        {
            List<SachInfo> sachList = fmatsach.GetSachInfoList();
            cmbMaSach.DataSource = sachList;
            cmbMaSach.DisplayMember = "ToString"; // Sử dụng phương thức ToString() đã ghi đè trong SachInfo
            cmbMaSach.ValueMember = "MaSach";     // Giá trị thực sự của mỗi item là MaSach
            if (sachList.Any())
                cmbMaSach.SelectedIndex = 0; // Chọn item đầu tiên nếu có
        }
        private void LoadMatSachGrid()
        {
            dgvMatSach.DataSource = null; // Xóa dữ liệu cũ trước khi tải mới
            List<MatSachDisplay> entries = fmatsach.GetMatSachEntries();
            dgvMatSach.DataSource = entries;

            // Tùy chọn: Cấu hình các cột nếu cần (ví dụ: ẩn MaSach nếu TenSach đã đủ)
            if (dgvMatSach.Columns["MaLanMat"] != null) dgvMatSach.Columns["MaLanMat"].HeaderText = "Mã Lần Mất";
            if (dgvMatSach.Columns["MaSach"] != null) dgvMatSach.Columns["MaSach"].HeaderText = "Mã Sách"; // Có thể ẩn nếu không cần
            if (dgvMatSach.Columns["TenSach"] != null) dgvMatSach.Columns["TenSach"].HeaderText = "Tên Sách";
            if (dgvMatSach.Columns["SLMat"] != null) dgvMatSach.Columns["SLMat"].HeaderText = "Số Lượng Mất";
            if (dgvMatSach.Columns["NgayMat"] != null) dgvMatSach.Columns["NgayMat"].HeaderText = "Ngày Mất";

            dgvMatSach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Tự động điều chỉnh độ rộng cột
        }
        private void ClearForm()
        {
            txtMaLanMat.Clear();
            if (cmbMaSach.Items.Count > 0) cmbMaSach.SelectedIndex = 0;
            txtSLMat.Clear();
            dtpNgayMat.Value = DateTime.Now; // Đặt ngày về ngày hiện tại
            dgvMatSach.ClearSelection();    // Bỏ chọn tất cả các dòng trong DataGridView
            originalSelectedMatSach = null; // Đặt lại dữ liệu gốc đã chọn
        }
        // Thiết lập trạng thái của form (ví dụ: các nút nào được bật/tắt)
        private void SetFormState(string state)
        {
            isInAddMode = false;
            if (state == "initial")
            {
                ClearForm();
                txtMaLanMat.Enabled = false; // Mã Lần Mất luôn không cho sửa/nhập ở trạng thái ban đầu
                cmbMaSach.Enabled = false;
                txtSLMat.Enabled = false;
                dtpNgayMat.Enabled = false;

                btnThem.Text = "Thêm";
                btnThem.Enabled = true;

                btnSua.Text = "Lưu";
                btnSua.Enabled = false;

                btnXoa.Enabled = false;
            }
            else if (state == "selected")
            {
                txtMaLanMat.Enabled = false; // Khóa chính không sửa khi đã chọn
                cmbMaSach.Enabled = true;
                txtSLMat.Enabled = true;
                dtpNgayMat.Enabled = true;

                btnThem.Text = "Thêm";
                btnThem.Enabled = true;

                btnSua.Text = "Lưu";
                btnSua.Enabled = true;

                btnXoa.Enabled = true;
            }
            else if (state == "adding")
            {
                // txtMaLanMat.Clear(); // ClearForm đã làm việc này
                txtMaLanMat.Enabled = false; // KHÔNG CHO PHÉP NHẬP Mã Lần Mất nữa
                                             // txtMaLanMat.Text = "(Tự động)"; // Sẽ đặt ở btnThem_Click

                cmbMaSach.Enabled = true;
                txtSLMat.Enabled = true;
                dtpNgayMat.Enabled = true;

                btnThem.Text = "Lưu Mới";
                btnThem.Enabled = true;

                btnSua.Text = "Lưu";
                btnSua.Enabled = false;

                btnXoa.Enabled = false;
                isInAddMode = true;
            }
        }
        // Sự kiện khi lựa chọn thay đổi trong DataGridView
        private void dgvMatSach_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMatSach.SelectedRows.Count > 0) // Nếu có dòng được chọn
            {
                // Không cần gọi PopulateFormFromSelectedRow ở đây nữa nếu CellClick xử lý chính
                // PopulateFormFromSelectedRow(dgvMatSach.SelectedRows[0]);
                // SetFormState("selected"); // Trạng thái sẽ được đặt bởi CellClick hoặc khi cần thiết
            }
            else
            {
                // Nếu không có dòng nào được chọn, đặt lại form về trạng thái ban đầu
                // Điều này quan trọng khi người dùng bỏ chọn tất cả các dòng
                if (!isInAddMode)
                {
                    SetFormState("initial");
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (isInAddMode) // Nếu đang ở chế độ "Lưu Mới"
            {
                // --- BẮT ĐẦU LƯU MỚI ---
                // BỎ Kiểm tra Mã Lần Mất (do CSDL tự tạo)
                // if (string.IsNullOrWhiteSpace(txtMaLanMat.Text)) { /* ... */ return; }
                // if (!int.TryParse(txtMaLanMat.Text, out int maLanMatValue) || maLanMatValue <= 0) { /* ... */ return; }

                // Kiểm tra các trường khác
                if (string.IsNullOrWhiteSpace(txtSLMat.Text) || cmbMaSach.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng nhập đủ thông tin (Số lượng mất, Mã sách).", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // ... (các kiểm tra khác cho SLMat, MaSach giữ nguyên) ...
                if (!int.TryParse(txtSLMat.Text, out int slMat) || slMat <= 0)
                {
                    MessageBox.Show("Số lượng mất phải là một số nguyên dương.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                SachInfo selectedSach = cmbMaSach.SelectedItem as SachInfo;
                if (selectedSach == null)
                {
                    MessageBox.Show("Vui lòng chọn sách từ danh sách.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (slMat > selectedSach.SoLuongHienCo)
                {
                    MessageBox.Show($"Số lượng mất ({slMat}) không thể lớn hơn số lượng hiện có của sách '{selectedSach.TenSach}' ({selectedSach.SoLuongHienCo}).", "Số lượng không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo đối tượng MatSachData KHÔNG CẦN MaLanMat từ người dùng
                MatSachData data = new MatSachData
                {
                    // MaLanMat = maLanMatValue, // BỎ DÒNG NÀY
                    MaSach = selectedSach.MaSach,
                    SLMat = slMat,
                    NgayMat = dtpNgayMat.Value
                };
                // data.MaLanMat sẽ có giá trị mặc định (0 cho int), điều này không sao vì fmatsach sẽ không chèn nó.

                bool success = fmatsach.ThemMatSach(data);
                if (success)
                {
                    MessageBox.Show("Thêm thông tin mất sách thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadMatSachGrid();
                    LoadSachComboBox();
                    SetFormState("initial");
                }
                else
                {
                    // Lỗi này giờ đây sẽ phản ánh lỗi từ CSDL mà fmatsach bắt được (nếu có)
                    MessageBox.Show("Thêm thất bại. Vui lòng kiểm tra lại hoặc liên hệ quản trị viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                // --- KẾT THÚC LƯU MỚI ---
            }
            else // Nếu đang ở chế độ "Thêm"
            {
                ClearForm();
                txtMaLanMat.Text = "(Tự động)"; // QUAY LẠI HIỂN THỊ (Tự động)
                SetFormState("adding");
                cmbMaSach.Focus(); // Chuyển focus sang Mã Sách hoặc Số lượng mất
            }
        }

        
        
        

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Kiểm tra đầu vào cơ bản
            if (isInAddMode || originalSelectedMatSach == null)
            {
                MessageBox.Show("Không có mục nào được chọn để cập nhật hoặc đang ở chế độ thêm mới.", "Thao tác không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSLMat.Text) || cmbMaSach.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin (Số lượng mất, Mã sách).", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtSLMat.Text, out int slMat) || slMat <= 0)
            {
                MessageBox.Show("Số lượng mất phải là một số nguyên dương.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SachInfo selectedSach = cmbMaSach.SelectedItem as SachInfo;
            if (selectedSach == null)
            {
                MessageBox.Show("Vui lòng chọn sách từ danh sách.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int soLuongCoTheMatToiDaKhiSua = selectedSach.SoLuongHienCo + originalSelectedMatSach.SLMat;
            if (selectedSach.MaSach == originalSelectedMatSach.MaSach && slMat > soLuongCoTheMatToiDaKhiSua)
            {
                MessageBox.Show($"Số lượng mất mới ({slMat}) không hợp lệ. Số lượng hiện có của sách '{selectedSach.TenSach}' là {selectedSach.SoLuongHienCo}, số lượng mất ban đầu là {originalSelectedMatSach.SLMat}. Tổng cộng có thể khai báo mất tối đa {soLuongCoTheMatToiDaKhiSua}.", "Số lượng không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (selectedSach.MaSach != originalSelectedMatSach.MaSach && slMat > selectedSach.SoLuongHienCo)
            {
                MessageBox.Show($"Số lượng mất ({slMat}) không thể lớn hơn số lượng hiện có của sách mới '{selectedSach.TenSach}' ({selectedSach.SoLuongHienCo}).", "Số lượng không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MatSachData data = new MatSachData
            {
                MaLanMat = originalSelectedMatSach.MaLanMat,
                MaSach = selectedSach.MaSach,
                SLMat = slMat,
                NgayMat = dtpNgayMat.Value
            };

            bool success = fmatsach.SuaMatSach(data, originalSelectedMatSach.MaSach, originalSelectedMatSach.SLMat);
            if (success)
            {
                MessageBox.Show("Cập nhật thông tin mất sách thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadMatSachGrid();
                LoadSachComboBox();
                SetFormState("initial");
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại. Vui lòng kiểm tra lại hoặc liên hệ quản trị viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvMatSach.SelectedRows.Count == 0 || originalSelectedMatSach == null)
            {
                MessageBox.Show("Vui lòng chọn một mục để xóa.", "Chưa chọn mục", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (isInAddMode)
            {
                MessageBox.Show("Không thể xóa khi đang ở chế độ thêm mới.", "Thao tác không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            DialogResult confirmation = MessageBox.Show($"Bạn có chắc chắn muốn xóa mục mất sách có mã '{originalSelectedMatSach.MaLanMat}' không?",
                                                      "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmation == DialogResult.Yes)
            {
                bool success = fmatsach.XoaMatSach(originalSelectedMatSach.MaLanMat);
                if (success)
                {
                    MessageBox.Show("Xóa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadMatSachGrid();
                    LoadSachComboBox();
                    SetFormState("initial");
                }
                else
                {
                    MessageBox.Show("Xóa thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ClearForm();
            SetFormState("initial");
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form hiện tại
        }
        private void PopulateFormFromSelectedRow(DataGridViewRow selectedRow)
        {
            if (selectedRow == null) return;

            txtMaLanMat.Text = selectedRow.Cells["MaLanMat"].Value.ToString();
            cmbMaSach.SelectedValue = selectedRow.Cells["MaSach"].Value.ToString();
            txtSLMat.Text = selectedRow.Cells["SLMat"].Value.ToString();
            dtpNgayMat.Value = Convert.ToDateTime(selectedRow.Cells["NgayMat"].Value);

            // Lưu trữ giá trị gốc để so sánh khi chỉnh sửa
            originalSelectedMatSach = new MatSachData
            {
                MaLanMat = Convert.ToInt32(selectedRow.Cells["MaLanMat"].Value),
                MaSach = selectedRow.Cells["MaSach"].Value.ToString(),
                SLMat = Convert.ToInt32(selectedRow.Cells["SLMat"].Value),
                NgayMat = Convert.ToDateTime(selectedRow.Cells["NgayMat"].Value)
            };
        }
        private void dgvMatSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Đảm bảo rằng người dùng nhấp vào một dòng hợp lệ (không phải header)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow clickedRow = dgvMatSach.Rows[e.RowIndex];
                PopulateFormFromSelectedRow(clickedRow);
                SetFormState("selected"); // Chuyển form sang trạng thái "selected" để cho phép sửa/xóa
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
        }
    }
}
