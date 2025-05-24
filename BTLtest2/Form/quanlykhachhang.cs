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
    public partial class quanlykhachhang : Form
    {
        private KhachHang_Function khachHangFunctions; // Đối tượng để gọi các hàm xử lý
        private bool dangThemMoi = false; // Cờ kiểm tra trạng thái Thêm mới
        private bool dangChinhSua = false; // Cờ kiểm tra trạng thái Chỉnh sửa
        public quanlykhachhang()
        {
           
            InitializeComponent();
            khachHangFunctions = new KhachHang_Function();
        }

        private void quanlykhachhang_Load(object sender, EventArgs e)
        {
            TaiDuLieuLenDataGridView();
            DatTrangThaiControlsBanDau();
            // Thiết lập tiêu đề cột cho DataGridView (nếu bạn không thiết lập trong Designer)
            ThietLapTieuDeCotDataGridView();
        }
        private void ThietLapTieuDeCotDataGridView()
        {
            // Đảm bảo rằng tên cột ("MaKhach", "TenKhach",...) khớp với tên cột trong DataTable trả về
            if (dgvKhachHang.Columns["MaKhach"] != null)
                dgvKhachHang.Columns["MaKhach"].HeaderText = "Mã Khách";
            if (dgvKhachHang.Columns["TenKhach"] != null)
                dgvKhachHang.Columns["TenKhach"].HeaderText = "Tên Khách Hàng";
            if (dgvKhachHang.Columns["GioiTinh"] != null)
                dgvKhachHang.Columns["GioiTinh"].HeaderText = "Giới Tính";
            if (dgvKhachHang.Columns["DiaChi"] != null)
                dgvKhachHang.Columns["DiaChi"].HeaderText = "Địa Chỉ";
            if (dgvKhachHang.Columns["DienThoai"] != null)
                dgvKhachHang.Columns["DienThoai"].HeaderText = "Số Điện Thoại";

            // Tùy chỉnh độ rộng cột nếu cần
            // dgvKhachHang.Columns["MaKhach"].Width = 80;
            // dgvKhachHang.Columns["TenKhach"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        /// <summary>
        /// Tải dữ liệu khách hàng lên DataGridView.
        /// </summary>
        private void TaiDuLieuLenDataGridView()
        {
            DataTable dt = khachHangFunctions.LayDanhSachKhachHang();
            dgvKhachHang.DataSource = dt; // Gán nguồn dữ liệu
            dgvKhachHang.ClearSelection(); // Bỏ chọn dòng hiện tại (nếu có)
        }
        /// <summary>
        /// Thiết lập trạng thái ban đầu (hoặc sau khi Lưu/Bỏ qua) cho các controls.
        /// </summary>
        private void DatTrangThaiControlsBanDau()
        {
            // Vô hiệu hóa các ô nhập liệu
            txtMaKhachHang.Enabled = false;
            txtTenKhachHang.Enabled = false;
            radNam.Enabled = false;
            radNu.Enabled = false;
            txtDiaChi.Enabled = false;
            txtSoDienThoai.Enabled = false;

            // Kích hoạt các nút chức năng chính
            btnThem.Enabled = true;
            btnSua.Enabled = dgvKhachHang.SelectedRows.Count > 0; // Chỉ bật khi có dòng được chọn
            btnXoa.Enabled = dgvKhachHang.SelectedRows.Count > 0;  // Chỉ bật khi có dòng được chọn
            btnDong.Enabled = true;

            // Vô hiệu hóa các nút thao tác phụ
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;

            dgvKhachHang.Enabled = true; // Cho phép tương tác với Grid

            dangThemMoi = false;
            dangChinhSua = false;
        }
        /// <summary>
        /// Thiết lập trạng thái cho các controls khi đang Thêm hoặc Sửa.
        /// </summary>
        private void DatTrangThaiKhiThemSua()
        {
            txtMaKhachHang.Enabled = dangThemMoi; // Mã KH chỉ cho nhập khi thêm mới
            txtTenKhachHang.Enabled = true;
            radNam.Enabled = true;
            radNu.Enabled = true;
            txtDiaChi.Enabled = true;
            txtSoDienThoai.Enabled = true;

            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnDong.Enabled = false; // Hoặc true tùy theo logic bạn muốn

            btnLuu.Enabled = true;
            btnBoQua.Enabled = true;

            dgvKhachHang.Enabled = false; // Không cho tương tác với Grid khi đang sửa/thêm
        }

        /// <summary>
        /// Xóa trắng nội dung các ô nhập liệu.
        /// </summary>
        private void XoaTrangControls()
        {
            txtMaKhachHang.Clear();
            txtTenKhachHang.Clear();
            radNam.Checked = true; // Mặc định chọn Nam (hoặc theo logic của bạn)
            txtDiaChi.Clear();
            txtSoDienThoai.Clear();
        }

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Bỏ qua nếu click vào header row hoặc không có dòng nào được chọn thực sự
            if (e.RowIndex < 0 || e.RowIndex >= dgvKhachHang.Rows.Count - (dgvKhachHang.AllowUserToAddRows ? 1 : 0)) // Trừ dòng mới nếu có
            {
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                //XoaTrangControls(); // Tùy chọn: Xóa trắng form nếu không có dòng nào hợp lệ được chọn
                return;
            }

            // Nếu đang trong chế độ thêm/sửa thì không làm gì cả khi click vào grid
            if (dangThemMoi || dangChinhSua) return;


            DataGridViewRow row = dgvKhachHang.Rows[e.RowIndex];

            // Kiểm tra xem ô có giá trị không trước khi truy cập
            txtMaKhachHang.Text = row.Cells["MaKhach"].Value?.ToString() ?? "";
            txtTenKhachHang.Text = row.Cells["TenKhach"].Value?.ToString() ?? "";
            txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString() ?? "";
            txtSoDienThoai.Text = row.Cells["DienThoai"].Value?.ToString() ?? "";

            string gioiTinh = row.Cells["GioiTinh"].Value?.ToString() ?? "";
            if (gioiTinh.Equals("Nam", StringComparison.OrdinalIgnoreCase))
            {
                radNam.Checked = true;
            }
            else if (gioiTinh.Equals("Nữ", StringComparison.OrdinalIgnoreCase) || gioiTinh.Equals("Nu", StringComparison.OrdinalIgnoreCase))
            {
                radNu.Checked = true;
            }
            else
            {
                // Xử lý trường hợp giới tính không xác định (nếu có) hoặc để mặc định
                radNam.Checked = false; // Hoặc true tùy bạn
                radNu.Checked = false;
            }

            // Kích hoạt nút Sửa và Xóa
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            dangThemMoi = true;
            dangChinhSua = false;
            XoaTrangControls();
            DatTrangThaiKhiThemSua();
            txtMaKhachHang.Focus(); // Đưa con trỏ vào ô Mã khách hàng
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.SelectedRows.Count == 0 && dgvKhachHang.CurrentRow == null) // Kiểm tra cả CurrentRow
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Nếu không có dòng nào được chọn, lấy dòng hiện tại (CurrentRow)
            if (dgvKhachHang.SelectedRows.Count == 0 && dgvKhachHang.CurrentRow != null)
            {
                // Kích hoạt sự kiện CellClick cho dòng hiện tại để load dữ liệu lên form
                // Điều này hữu ích nếu người dùng chỉ di chuyển bằng bàn phím mà không click chuột
                dgvKhachHang_CellClick(dgvKhachHang, new DataGridViewCellEventArgs(0, dgvKhachHang.CurrentRow.Index));
            }


            dangChinhSua = true;
            dangThemMoi = false;
            DatTrangThaiKhiThemSua();
            txtMaKhachHang.Enabled = false; // Không cho sửa Mã Khách
            txtTenKhachHang.Focus(); // Đưa con trỏ vào ô Tên khách hàng
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.SelectedRows.Count == 0 && dgvKhachHang.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maKhach = "";
            string tenKhach = "";

            // Ưu tiên dòng đang được chọn (SelectedRows)
            if (dgvKhachHang.SelectedRows.Count > 0)
            {
                maKhach = dgvKhachHang.SelectedRows[0].Cells["MaKhach"].Value?.ToString() ?? "";
                tenKhach = dgvKhachHang.SelectedRows[0].Cells["TenKhach"].Value?.ToString() ?? "";
            }
            // Nếu không có SelectedRows, thử lấy từ CurrentRow
            else if (dgvKhachHang.CurrentRow != null)
            {
                maKhach = dgvKhachHang.CurrentRow.Cells["MaKhach"].Value?.ToString() ?? "";
                tenKhach = dgvKhachHang.CurrentRow.Cells["TenKhach"].Value?.ToString() ?? "";
            }


            if (string.IsNullOrEmpty(maKhach))
            {
                MessageBox.Show("Không thể xác định khách hàng cần xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            DialogResult dialogResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng '{tenKhach}' (Mã: {maKhach}) không?",
                                                     "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                bool ketQua = khachHangFunctions.XoaKhachHang(maKhach);
                if (ketQua)
                {
                    MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDuLieuLenDataGridView();
                    XoaTrangControls();
                    DatTrangThaiControlsBanDau();
                }
                // Các thông báo lỗi đã được xử lý bên trong hàm XoaKhachHang
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // --- Kiểm tra dữ liệu đầu vào ---
            if (dangThemMoi && string.IsNullOrWhiteSpace(txtMaKhachHang.Text))
            {
                MessageBox.Show("Mã khách hàng không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaKhachHang.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtTenKhachHang.Text))
            {
                MessageBox.Show("Tên khách hàng không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKhachHang.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
            {
                MessageBox.Show("Số điện thoại không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return;
            }
            // Thêm các kiểm tra khác nếu cần (ví dụ: định dạng SĐT, độ dài,...)

            // Tạo đối tượng KhachHang từ dữ liệu trên form
            KhachHang kh = new KhachHang
            {
                MaKhach = txtMaKhachHang.Text.Trim(),
                TenKhach = txtTenKhachHang.Text.Trim(),
                GioiTinh = radNam.Checked ? "Nam" : "Nữ",
                DiaChi = txtDiaChi.Text.Trim(),
                DienThoai = txtSoDienThoai.Text.Trim()
            };

            bool ketQua = false;
            if (dangThemMoi)
            {
                ketQua = khachHangFunctions.ThemKhachHang(kh);
                if (ketQua)
                {
                    MessageBox.Show("Thêm khách hàng mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (dangChinhSua)
            {
                // Mã khách không được sửa, nên lấy từ lúc chọn dòng để sửa (đã có trong txtMaKhachHang.Text)
                ketQua = khachHangFunctions.SuaKhachHang(kh);
                if (ketQua)
                {
                    MessageBox.Show("Cập nhật thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (ketQua)
            {
                TaiDuLieuLenDataGridView(); // Tải lại dữ liệu
                DatTrangThaiControlsBanDau(); // Đặt lại trạng thái controls
                XoaTrangControls();           // Xóa trắng ô nhập
            }
            // Thông báo lỗi (nếu có) đã được xử lý trong các hàm ThemKhachHang/SuaKhachHang
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            XoaTrangControls();
            DatTrangThaiControlsBanDau();
            // Nếu có dòng đang được chọn trong grid, hiển thị lại thông tin của nó (tùy chọn)
            if (dgvKhachHang.CurrentRow != null && dgvKhachHang.CurrentRow.Index >= 0)
            {
                dgvKhachHang_CellClick(dgvKhachHang, new DataGridViewCellEventArgs(0, dgvKhachHang.CurrentRow.Index));
            }
            else
            {
                dgvKhachHang.ClearSelection();
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form hiện tại
        }

        /// <summary>
        /// Sự kiện khi click vào một ô (hoặc dòng) trong DataGridView.
        /// Dùng để hiển thị thông tin của dòng được chọn lên các controls.
        /// </summary>
        // Sự kiện khi lựa chọn trong DataGridView thay đổi (hữu ích khi dùng phím mũi tên)
        private void dgvKhachHang_SelectionChanged(object sender, EventArgs e)
        {
            if (!dangThemMoi && !dangChinhSua) // Chỉ cập nhật nếu không ở chế độ thêm/sửa
            {
                if (dgvKhachHang.CurrentRow != null && dgvKhachHang.CurrentRow.Index >= 0 && dgvKhachHang.CurrentRow.Cells["MaKhach"].Value != null)
                {
                    // Gọi lại CellClick để load dữ liệu lên form
                    dgvKhachHang_CellClick(sender, new DataGridViewCellEventArgs(0, dgvKhachHang.CurrentRow.Index));
                    btnSua.Enabled = true;
                    btnXoa.Enabled = true;
                }
                else
                {
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                }
            }
        }
    }
}
