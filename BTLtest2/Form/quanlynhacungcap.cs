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
    public partial class quanlynhacungcap : Form
    {
        private NhaCungCapBUS nhaCungCapBUS;
        private bool dangThemMoi = false; // Cờ để xác định đang thêm mới hay sửa
        public quanlynhacungcap()
        {
            InitializeComponent();
            nhaCungCapBUS = new NhaCungCapBUS();
        }

        private void quanlynhacungcap_Load(object sender, EventArgs e)
        {

            TaiDuLieuDataGridView();
            DatTrangThaiDieuKhien(true); // Trạng thái ban đầu
            XoaTrangCacTruong();
        }
        /// <summary>
        /// Tải dữ liệu nhà cung cấp lên DataGridView.
        /// </summary>
        private void TaiDuLieuDataGridView()
        {
            DataTable dt = nhaCungCapBUS.LayDanhSachNhaCungCap();
            dgvNhaCungCap.DataSource = dt;
            // Tùy chỉnh hiển thị cột nếu cần (ví dụ: độ rộng)
            if (dgvNhaCungCap.Columns.Count > 0)
            {
                dgvNhaCungCap.Columns["Mã NCC"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgvNhaCungCap.Columns["Tên Nhà Cung Cấp"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvNhaCungCap.Columns["Địa Chỉ"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvNhaCungCap.Columns["SĐT"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }
        /// <summary>
        /// Xóa trắng nội dung các TextBox nhập liệu.
        /// </summary>
        private void XoaTrangCacTruong()
        {
            txtMaNCC.Clear();
            txtTenNCC.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
        }
        /// <summary>
        /// Thiết lập trạng thái Enabled/Disabled cho các điều khiển trên form.
        /// </summary>
        /// <param name="dangXem">True: đang ở chế độ xem/chờ. False: đang ở chế độ thêm/sửa.</param>
        private void DatTrangThaiDieuKhien(bool dangXem)
        {
            txtMaNCC.ReadOnly = dangXem || !dangThemMoi; // Mã NCC chỉ được nhập khi thêm mới
            txtTenNCC.ReadOnly = dangXem;
            txtDiaChi.ReadOnly = dangXem;
            txtSDT.ReadOnly = dangXem;

            btnThem.Enabled = dangXem;
            btnSua.Enabled = dangXem && dgvNhaCungCap.SelectedRows.Count > 0;
            btnXoa.Enabled = dangXem && dgvNhaCungCap.SelectedRows.Count > 0;
            btnLuu.Enabled = !dangXem;
            btnBoQua.Enabled = !dangXem;
            btnDong.Enabled = true; // Nút Đóng luôn được phép

            dgvNhaCungCap.Enabled = dangXem;
        }

        private void btnBoqua_Click(object sender, EventArgs e)
        {
            XoaTrangCacTruong();
            DatTrangThaiDieuKhien(true); // Quay về trạng thái xem ban đầu
                                         // Nếu có dòng nào đang được chọn trong DataGridView, hiển thị lại thông tin của dòng đó
            if (dgvNhaCungCap.SelectedRows.Count > 0 && dgvNhaCungCap.CurrentRow != null)
            {
                DataGridViewRow row = dgvNhaCungCap.CurrentRow;
                if (row.Cells["Mã NCC"].Value != null)
                {
                    txtMaNCC.Text = row.Cells["Mã NCC"].Value.ToString();
                    txtTenNCC.Text = row.Cells["Tên Nhà Cung Cấp"].Value.ToString();
                    txtDiaChi.Text = row.Cells["Địa Chỉ"].Value.ToString();
                    txtSDT.Text = row.Cells["SĐT"].Value.ToString();
                }
            }
        }

        private void dgvNhaCungCap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Đảm bảo người dùng click vào một dòng hợp lệ (không phải header)
            if (e.RowIndex >= 0 && e.RowIndex < dgvNhaCungCap.Rows.Count - 1) // Tránh dòng mới ở cuối nếu có
            {
                DataGridViewRow row = dgvNhaCungCap.Rows[e.RowIndex];
                if (row.Cells["Mã NCC"].Value != null) // Kiểm tra cell có giá trị không
                {
                    txtMaNCC.Text = row.Cells["Mã NCC"].Value.ToString();
                    txtTenNCC.Text = row.Cells["Tên Nhà Cung Cấp"].Value.ToString();
                    txtDiaChi.Text = row.Cells["Địa Chỉ"].Value.ToString();
                    txtSDT.Text = row.Cells["SĐT"].Value.ToString();

                    DatTrangThaiDieuKhien(true); // Quay về trạng thái xem, cho phép Sửa/Xóa
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            dangThemMoi = true;
            XoaTrangCacTruong();
            DatTrangThaiDieuKhien(false); // Chuyển sang trạng thái nhập liệu
            txtMaNCC.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvNhaCungCap.SelectedRows.Count == 0 || string.IsNullOrEmpty(txtMaNCC.Text))
            {
                MessageBox.Show("Vui lòng chọn một nhà cung cấp để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            dangThemMoi = false; // Đang ở chế độ sửa
            DatTrangThaiDieuKhien(false); // Chuyển sang trạng thái nhập liệu (Mã NCC sẽ readonly)
            txtTenNCC.Focus();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvNhaCungCap.SelectedRows.Count == 0 || string.IsNullOrEmpty(txtMaNCC.Text))
            {
                MessageBox.Show("Vui lòng chọn một nhà cung cấp để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa nhà cung cấp '{txtTenNCC.Text}' (Mã: {txtMaNCC.Text}) không?",
                                                   "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                if (nhaCungCapBUS.XoaNhaCungCap(txtMaNCC.Text))
                {
                    MessageBox.Show("Xóa nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDuLieuDataGridView();
                    XoaTrangCacTruong();
                    DatTrangThaiDieuKhien(true);
                }
                // Thông báo lỗi đã được xử lý bên trong NhaCungCapBUS
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // --- Kiểm tra dữ liệu đầu vào ---
            if (string.IsNullOrWhiteSpace(txtMaNCC.Text))
            {
                MessageBox.Show("Mã nhà cung cấp không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNCC.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTenNCC.Text))
            {
                MessageBox.Show("Tên nhà cung cấp không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNCC.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDiaChi.Text)) // Giả sử địa chỉ cũng bắt buộc
            {
                MessageBox.Show("Địa chỉ nhà cung cấp không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaChi.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtSDT.Text)) // Giả sử SĐT cũng bắt buộc
            {
                MessageBox.Show("Số điện thoại nhà cung cấp không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }
            // --- Kết thúc kiểm tra dữ liệu ---


            NhaCungCap ncc = new NhaCungCap
            {
                MaNCC = txtMaNCC.Text.Trim(),
                TenNhaCungCap = txtTenNCC.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim(),
                DienThoai = txtSDT.Text.Trim()
            };

            bool ketQua = false;
            if (dangThemMoi) // Nếu đang ở chế độ thêm mới
            {
                ketQua = nhaCungCapBUS.ThemNhaCungCap(ncc);
                if (ketQua)
                {
                    MessageBox.Show("Thêm nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else // Nếu đang ở chế độ sửa
            {
                ketQua = nhaCungCapBUS.SuaNhaCungCap(ncc);
                if (ketQua)
                {
                    MessageBox.Show("Cập nhật nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (ketQua) // Nếu thao tác thành công
            {
                TaiDuLieuDataGridView();
                XoaTrangCacTruong();
                DatTrangThaiDieuKhien(true); // Quay về trạng thái xem
            }
            // Các thông báo lỗi cụ thể (ví dụ trùng mã) đã được xử lý trong NhaCungCapBUS
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form hiện tại
        }
    }
}
