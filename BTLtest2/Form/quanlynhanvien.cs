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
    public partial class quanlynhanvien : Form
    {
        private NhanVienFunctions nvFunctions;
        private DataTable dtNhanVien; // To hold employee data for the DataGridView

        public quanlynhanvien()
        {
            InitializeComponent();
            nvFunctions = new NhanVienFunctions();
        }

        private void quanlynhanvien_Load(object sender, EventArgs e)
        {
            LoadCongViec();
            LoadNhanVienData();
            SetInitialButtonStates();
            ClearForm();
            DisableFormControls();
        }
        private void LoadCongViec()
        {
            List<CongViec> congViecList = nvFunctions.GetCongViecList();
            cmbChucVu.DataSource = congViecList;
            cmbChucVu.DisplayMember = "TenCongViec";
            cmbChucVu.ValueMember = "MaCv";
            cmbChucVu.SelectedIndex = -1; // No initial selection
        }

        private void LoadNhanVienData()
        {
            dtNhanVien = nvFunctions.GetAllNhanVien();
            dgvNhanVien.DataSource = dtNhanVien;
            dgvNhanVien.ClearSelection(); // Clear any default selection
                                          // Optional: Configure DataGridView columns here if needed
                                          // e.g., dgvNhanVien.Columns["MaNhanVien"].HeaderText = "Mã Nhân Viên";
                                          // dgvNhanVien.Columns["TenNhanVien"].HeaderText = "Tên Nhân Viên";
                                          // ... and so on for other columns
                                          // You might want to set AutoSizeColumnsMode, ReadOnly, AllowUserToAddRows = false, etc.
            dgvNhanVien.ReadOnly = true;
            dgvNhanVien.AllowUserToAddRows = false;
            dgvNhanVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Recommended
        }

        private void ClearForm()
        {
            txtMaNhanVien.Text = "";
            txtTenNhanVien.Text = "";
            txtDiaChi.Text = "";
            txtDienThoai.Text = "";
            cmbChucVu.SelectedIndex = -1;
            rdoNam.Checked = false;
            rdoNu.Checked = false;
            dtpNgaySinh.Value = DateTime.Now;
            dtpNgaySinh.Checked = false; // If ShowCheckBox is true on DateTimePicker for nullable dates
            txtMaNhanVien.Focus();
        }

        private void EnableFormControls(bool isAdding = false) // Added parameter to control MaNhanVien field
        {
            txtMaNhanVien.Enabled = isAdding; // Only enable for new entries
            txtTenNhanVien.Enabled = true;
            txtDiaChi.Enabled = true;
            txtDienThoai.Enabled = true;
            cmbChucVu.Enabled = true;
            rdoNam.Enabled = true;
            rdoNu.Enabled = true;
            dtpNgaySinh.Enabled = true;
        }

        private void DisableFormControls()
        {
            txtMaNhanVien.Enabled = false;
            txtTenNhanVien.Enabled = false;
            txtDiaChi.Enabled = false;
            txtDienThoai.Enabled = false;
            cmbChucVu.Enabled = false;
            rdoNam.Enabled = false;
            rdoNu.Enabled = false;
            dtpNgaySinh.Enabled = false;
        }

        private void SetInitialButtonStates()
        {
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            btnDong.Enabled = true;
        }

        private void dgvNhanVien_CellBorderStyleChanged(object sender, EventArgs e)
        {

        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a valid row is clicked (not the header and row index is within bounds)
            if (e.RowIndex >= 0 && e.RowIndex < dgvNhanVien.Rows.Count)
            {
                // Ensure the full row is selected visually upon click
                dgvNhanVien.Rows[e.RowIndex].Selected = true;

                DataGridViewRow selectedRow = dgvNhanVien.Rows[e.RowIndex]; // Use e.RowIndex to get the clicked row

                txtMaNhanVien.Text = selectedRow.Cells["MaNhanVien"].Value?.ToString();
                txtTenNhanVien.Text = selectedRow.Cells["TenNhanVien"].Value?.ToString();
                txtDiaChi.Text = selectedRow.Cells["DiaChi"].Value?.ToString();
                txtDienThoai.Text = selectedRow.Cells["DienThoai"].Value?.ToString();

                // Handle ComboBox selection safely
                object maCvValue = selectedRow.Cells["MaCv"].Value;
                if (maCvValue != null && maCvValue != DBNull.Value)
                {
                    cmbChucVu.SelectedValue = maCvValue;
                }
                else
                {
                    cmbChucVu.SelectedIndex = -1; // Or handle as appropriate
                }


                string gioiTinh = selectedRow.Cells["GioiTinh"].Value?.ToString();
                if (!string.IsNullOrEmpty(gioiTinh))
                {
                    if (gioiTinh.Equals("Nam", StringComparison.OrdinalIgnoreCase)) rdoNam.Checked = true;
                    else if (gioiTinh.Equals("Nữ", StringComparison.OrdinalIgnoreCase)) rdoNu.Checked = true;
                    else { rdoNam.Checked = false; rdoNu.Checked = false; }
                }
                else
                {
                    rdoNam.Checked = false;
                    rdoNu.Checked = false;
                }


                if (selectedRow.Cells["NgaySinh"].Value != DBNull.Value && selectedRow.Cells["NgaySinh"].Value != null)
                {
                    dtpNgaySinh.Value = Convert.ToDateTime(selectedRow.Cells["NgaySinh"].Value);
                    dtpNgaySinh.Checked = true; // If ShowCheckBox is true
                }
                else
                {
                    dtpNgaySinh.Value = DateTime.Now; // Or your default
                    dtpNgaySinh.Checked = false; // If ShowCheckBox is true
                }

                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnThem.Enabled = true; // Keep Add enabled, or disable if preferred when a row is selected
                btnLuu.Enabled = false;  // Should only be enabled after "Sửa" or "Thêm"
                btnBoQua.Enabled = false; // Should only be enabled after "Sửa" or "Thêm"
                DisableFormControls(); // Keep controls disabled until "Sửa" is clicked
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ClearForm();
            EnableFormControls(isAdding: true); // Pass true to enable MaNhanVien
            txtMaNhanVien.Focus();

            btnLuu.Enabled = true;
            btnBoQua.Enabled = true;
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            dgvNhanVien.ClearSelection(); // Deselect rows in DataGridView
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.SelectedRows.Count > 0)
            {
                EnableFormControls(isAdding: false); // Pass false, MaNhanVien remains disabled
                txtTenNhanVien.Focus();

                btnLuu.Enabled = true;
                btnBoQua.Enabled = true;
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên từ danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.SelectedRows.Count > 0)
            {
                string maNV = dgvNhanVien.SelectedRows[0].Cells["MaNhanVien"].Value.ToString();
                string tenNV = dgvNhanVien.SelectedRows[0].Cells["TenNhanVien"].Value.ToString();

                DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhân viên '{tenNV}' (Mã: {maNV}) không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    if (nvFunctions.DeleteNhanVien(maNV))
                    {
                        MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadNhanVienData(); // Reload data
                        ClearForm();
                        DisableFormControls();
                        SetInitialButtonStates(); // Reset buttons
                    }
                    else
                    {
                        MessageBox.Show("Xóa nhân viên thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhân viên từ danh sách để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Basic Validation
            if (txtMaNhanVien.Enabled && string.IsNullOrWhiteSpace(txtMaNhanVien.Text)) // Check MaNV only if it's enabled (Add mode)
            {
                MessageBox.Show("Mã nhân viên không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNhanVien.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTenNhanVien.Text))
            {
                MessageBox.Show("Tên nhân viên không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNhanVien.Focus();
                return;
            }
            if (cmbChucVu.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn chức vụ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbChucVu.Focus();
                return;
            }
            if (!rdoNam.Checked && !rdoNu.Checked)
            {
                MessageBox.Show("Vui lòng chọn giới tính.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Add more validation as needed (DienThoai, DiaChi format, etc.)

            NhanVien nv = new NhanVien
            {
                MaNhanVien = txtMaNhanVien.Text.Trim(),
                TenNhanVien = txtTenNhanVien.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim(),
                DienThoai = txtDienThoai.Text.Trim(),
                MaCv = cmbChucVu.SelectedValue.ToString(),
                GioiTinh = rdoNam.Checked ? "Nam" : "Nữ", // Assuming one must be checked due to validation
                NgaySinh = dtpNgaySinh.Checked ? (DateTime?)dtpNgaySinh.Value : null
            };

            bool success;
            string currentAction = txtMaNhanVien.Enabled ? "Thêm" : "Sửa"; // Determine action based on MaNhanVien field's state

            if (currentAction == "Thêm")
            {
                // Check if MaNhanVien already exists only when adding
                DataRow[] existingRows = dtNhanVien.Select($"MaNhanVien = '{nv.MaNhanVien}'");
                if (existingRows.Length > 0)
                {
                    MessageBox.Show("Mã nhân viên đã tồn tại. Vui lòng nhập mã khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMaNhanVien.Focus();
                    return;
                }
                success = nvFunctions.AddNhanVien(nv);
            }
            else // Sửa mode
            {
                success = nvFunctions.UpdateNhanVien(nv);
            }

            if (success)
            {
                MessageBox.Show($"{currentAction} thông tin nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadNhanVienData();
                ClearForm();
                DisableFormControls();
                SetInitialButtonStates();
            }
            else
            {
                MessageBox.Show($"{currentAction} thông tin nhân viên thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            ClearForm();
            DisableFormControls();
            SetInitialButtonStates();
            dgvNhanVien.ClearSelection();
            // If a row was previously selected, you might want to re-populate the form
            // with that row's data, or simply leave it clear.
            // For simplicity, we are just clearing and resetting.
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
