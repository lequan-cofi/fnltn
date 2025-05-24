using BTLtest2.Class;
using BTLtest2.function;
using DevExpress.CodeParser;
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
    public partial class qlhoadonnhap : Form
    {

        private HoaDonNhapLogic hdnLogic; // Đối tượng xử lý logic nghiệp vụ cho hóa đơn nhập
        private BindingList<ChiTietHDNhap> currentChiTietList; // Danh sách các sản phẩm trong hóa đơn hiện tại (đang tạo mới hoặc sửa)
         // Lưu trữ hóa đơn đang được sửa (nếu có)
        private List<ChiTietHDNhap> originalChiTietListForEdit = null; // Lưu trữ danh sách chi tiết gốc khi sửa hóa đơn
        private HoaDonNhap currentEditingHoaDon = null;
        public qlhoadonnhap()
        {
            InitializeComponent();
            hdnLogic = new HoaDonNhapLogic();
            currentChiTietList = new BindingList<ChiTietHDNhap>(); // Khởi tạo danh sách cho các mục hóa đơn hiện tại
            dgvChiTietHoaDon.DataSource = currentChiTietList; // Gán danh sách này làm nguồn cho DataGridView chi tiết

        }


        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void qlhoadonnhap_Load(object sender, EventArgs e)
        {

            LoadHoaDonNhapList(); // Tải danh sách hóa đơn nhập lên DataGridView chính
            LoadComboBoxes();     // Tải dữ liệu cho các ComboBox (Nhân viên, NCC, Sách)
            ConfigureChiTietDataGridView(); // Cấu hình các cột cho DataGridView chi tiết hóa đơn
            SetInitialFormState(); // Thiết lập trạng thái ban đầu cho form (các nút, trường nhập liệu)
        }
        private void SetInitialFormState(bool clearHeader = true)
        {
            if (clearHeader)
            {
                ClearHeaderFields(); // Xóa trắng các trường thông tin chung của hóa đơn
            }
            currentChiTietList.Clear(); // Xóa tất cả sản phẩm trong danh sách chi tiết hiện tại
            ClearSanPhamFields(); // Xóa trắng các trường nhập liệu sản phẩm
            txtTongTien.Text = "0"; // Đặt tổng tiền về 0

            // Các trường thông tin chung
            txtMaHoaDon.Enabled = true; // Cho phép nhập mã hóa đơn (hoặc tự động tạo)
            dtpNgayNhap.Enabled = true;
            cmbMaNhanVien.Enabled = true;
            cmbMaNhaCungCap.Enabled = true;

            // Các trường chi tiết sản phẩm
            cmbMaHang.Enabled = true;
            txtSoLuong.Enabled = true;
            txtDonGiaNhap.Enabled = true;
            // txtThanhTien được tính tự động

            // Trạng thái các nút
            btnThemHoaDon.Enabled = true;  // Nút "Thêm hóa đơn"
            btnLuuHoaDon.Enabled = false;  // Nút "Lưu" (chỉ bật khi đang thêm mới hoặc sửa)
            btnSuaHoaDon.Enabled = false;  // Nút "Sửa hóa đơn" (chỉ bật khi đã chọn một hóa đơn)
            btnXoaHoaDon.Enabled = false;  // Nút "Xóa hóa đơn" (chỉ bật khi đã chọn một hóa đơn)

            btnThemSanPham.Enabled = true; // Nút "Thêm sản phẩm" vào chi tiết
            btnSuaSanPham.Enabled = false; // Nút "Sửa sản phẩm" (chỉ bật khi chọn sản phẩm trong chi tiết)
            btnXoaSanPham.Enabled = false; // Nút "Xóa sản phẩm" (chỉ bật khi chọn sản phẩm trong chi tiết)

            dgvHoaDonNhap.ClearSelection();    // Bỏ chọn trên DataGridView danh sách hóa đơn
            dgvChiTietHoaDon.ClearSelection(); // Bỏ chọn trên DataGridView chi tiết hóa đơn

            currentEditingHoaDon = null; // Không có hóa đơn nào đang được sửa
            originalChiTietListForEdit = null;
        }
        private void SetEditingState() // Khi nhấn nút "Thêm" hoặc "Sửa" hóa đơn
        {
            // txtMaHoaDon.Enabled = false; // Không cho sửa Mã HĐ khi đang sửa, hoặc khi thêm mới thì để true
            dtpNgayNhap.Enabled = true;
            cmbMaNhanVien.Enabled = true;
            cmbMaNhaCungCap.Enabled = true;

            cmbMaHang.Enabled = true;
            txtSoLuong.Enabled = true;
            txtDonGiaNhap.Enabled = true;

            btnThemHoaDon.Enabled = false; // Đang trong quá trình thêm/sửa rồi
            btnLuuHoaDon.Enabled = true;   // Bật nút Lưu
            btnSuaHoaDon.Enabled = false;  // Vô hiệu hóa nút Sửa (vì đang sửa rồi hoặc đang thêm mới)
            btnXoaHoaDon.Enabled = false;  // Vô hiệu hóa nút Xóa

            btnThemSanPham.Enabled = true;
            // btnSuaSanPham và btnXoaSanPham sẽ phụ thuộc vào việc có chọn dòng nào trong dgvChiTietHoaDon không
        }
        private void LoadComboBoxes()
        {
            // ComboBox Nhân Viên
            cmbMaNhanVien.DataSource = hdnLogic.GetNhanVienForComboBox();
            cmbMaNhanVien.DisplayMember = "TenNhanVien"; // Hiển thị tên nhân viên
            cmbMaNhanVien.ValueMember = "MaNhanVien";   // Giá trị là mã nhân viên
            cmbMaNhanVien.SelectedIndex = -1;          // Không chọn mục nào ban đầu

            // ComboBox Nhà Cung Cấp
            cmbMaNhaCungCap.DataSource = hdnLogic.GetNhaCungCapForComboBox();
            cmbMaNhaCungCap.DisplayMember = "TenNhaCungCap";
            cmbMaNhaCungCap.ValueMember = "MaNCC";
            cmbMaNhaCungCap.SelectedIndex = -1;

            // ComboBox Sách (Mã Hàng)
            cmbMaHang.DataSource = hdnLogic.GetSachForComboBox();
            cmbMaHang.DisplayMember = "TenSach"; // Hiển thị tên sách
            cmbMaHang.ValueMember = "MaSach";  // Giá trị là mã sách
            cmbMaHang.SelectedIndex = -1;
        }
        private void ConfigureChiTietDataGridView()
        {
            dgvChiTietHoaDon.AutoGenerateColumns = false; // Tắt tự động tạo cột
            // Bạn cần định nghĩa các cột trong Designer của Visual Studio hoặc bằng code ở đây
            // Đảm bảo DataPropertyName của mỗi cột khớp với tên thuộc tính trong class ChiTietHDNhap
            // Ví dụ:
            // dgvChiTietHoaDon.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMaSach", HeaderText = "Mã Sách", DataPropertyName = "MaSach", Width = 80 });
            // dgvChiTietHoaDon.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTenSach", HeaderText = "Tên Sách", DataPropertyName = "TenSach", Width = 200 });
            // dgvChiTietHoaDon.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSLNhap", HeaderText = "SL Nhập", DataPropertyName = "SLNhap", Width = 70 });
            // dgvChiTietHoaDon.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDonGiaNhap", HeaderText = "Đơn Giá Nhập", DataPropertyName = "DonGiaNhap", Width = 100, Format = "N0" });
            // dgvChiTietHoaDon.Columns.Add(new DataGridViewTextBoxColumn { Name = "colKhuyenMai", HeaderText = "Khuyến Mãi", DataPropertyName = "KhuyenMai", Width = 80, Format = "N0" });
            // dgvChiTietHoaDon.Columns.Add(new DataGridViewTextBoxColumn { Name = "colThanhTien", HeaderText = "Thành Tiền", DataPropertyName = "ThanhTien", Width = 120, Format = "N0" });
            // Cấu hình thêm các thuộc tính cột nếu cần (ví dụ: Alignment, ReadOnly...)
        }
        private void LoadHoaDonNhapList()
        {
            DataTable dt = hdnLogic.GetHoaDonNhapList();
            dgvHoaDonNhap.DataSource = dt;
            // Có thể ẩn các cột Mã nếu bạn đã hiển thị Tên tương ứng
            // Ví dụ: dgvHoaDonNhap.Columns["MaNhanVien"].Visible = false;
            //         dgvHoaDonNhap.Columns["MaNCC"].Visible = false;
        }
        private void cmbMaNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMaNhanVien.SelectedItem is DataRowView drv) // Nếu nguồn là DataTable
            {
                // Giả sử DataTable từ GetNhanVienForComboBox() có cột "TenNhanVien"
               
            }
            // Nếu bạn binding trực tiếp List<NhanVien> thì sẽ khác
            // else if (cmbMaNhanVien.SelectedItem is NhanVien nv)
            // {
            //     txtTenNhanVien.Text = nv.TenNhanVien;
            // }
            else
            {
              
            }
        }

        private void cmbMaNhaCungCap_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMaNhaCungCap.SelectedValue != null && cmbMaNhaCungCap.SelectedValue != DBNull.Value)
            {
                string maNCC = cmbMaNhaCungCap.SelectedValue.ToString();
                NhaCungCaphdn ncc = hdnLogic.GetNhaCungCapDetails(maNCC); // Hàm lấy chi tiết NCC
                if (ncc != null)
                {
                   
                }
            }
            else
            {
               
            }
        }

        private void cmbMaHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMaHang.SelectedItem is DataRowView drv) // Nếu nguồn là DataTable
            {
               
                // Tùy chọn: Tự động điền Đơn giá nhập từ KhoSach nếu có trong dữ liệu ComboBox
                if (drv.Row.Table.Columns.Contains("DonGiaNhap") && drv["DonGiaNhap"] != DBNull.Value)
                {
                    txtDonGiaNhap.Text = Convert.ToSingle(drv["DonGiaNhap"]).ToString("N0"); // "N0" để format số không có phần thập phân
                }
                else
                {
                    txtDonGiaNhap.Clear();
                }
                txtSoLuong.Focus(); // Chuyển focus sang ô số lượng
            }
            else
            {
               
                txtDonGiaNhap.Clear();
            }
            txtThanhTien.Clear(); // Xóa thành tiền khi chọn sách mới
        }

        private void TinhThanhTienSanPham()
        {
            if (int.TryParse(txtSoLuong.Text, out int sl) && float.TryParse(txtDonGiaNhap.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out float dg))
            {
                // Giả sử KhuyenMai là 0, hoặc bạn thêm một trường nhập KhuyenMai
                float khuyenMai = 0; // Thay đổi nếu có trường nhập khuyến mãi
                txtThanhTien.Text = ((sl * dg) - khuyenMai).ToString("N0");
            }
            else
            {
                txtThanhTien.Text = "0";
            }
        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            TinhThanhTienSanPham();
        }

        private void txtDonGiaNhap_TextChanged(object sender, EventArgs e)
        {
            TinhThanhTienSanPham();
        }
        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số và phím Backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtDonGiaNhap_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số, dấu chấm thập phân (nếu cần) và phím Backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // Chỉ cho phép một dấu chấm thập phân
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }


        private void TinhTongTienHoaDon()
        {
            float tong = 0;
            foreach (var item in currentChiTietList)
            {
                tong += item.ThanhTien;
            }
            txtTongTien.Text = tong.ToString("N0"); // "N0" để format số không có phần thập phân
        }


        // --- Các xử lý sự kiện cho nút bấm liên quan đến Chi Tiết Hóa Đơn (Sản Phẩm) ---

        private void btnThemSanPham_Click(object sender, EventArgs e)
        {
            if (cmbMaHang.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbMaHang.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtSoLuong.Text))
            {
                MessageBox.Show("Vui lòng nhập số lượng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDonGiaNhap.Text))
            {
                MessageBox.Show("Vui lòng nhập đơn giá.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGiaNhap.Focus();
                return;
            }


            if (!int.TryParse(txtSoLuong.Text, out int slNhap) || slNhap <= 0)
            {
                MessageBox.Show("Số lượng nhập không hợp lệ. Phải là số nguyên dương.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoLuong.Focus();
                return;
            }
            if (!float.TryParse(txtDonGiaNhap.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out float donGiaNhap) || donGiaNhap < 0)
            {
                MessageBox.Show("Đơn giá nhập không hợp lệ. Phải là số không âm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDonGiaNhap.Focus();
                return;
            }

            string maSachChon = cmbMaHang.SelectedValue.ToString();
            // Kiểm tra xem sản phẩm đã tồn tại trong danh sách chi tiết hiện tại chưa
            var existingItem = currentChiTietList.FirstOrDefault(item => item.MaSach == maSachChon);

            if (existingItem != null)
            {
                // Nếu đã tồn tại, cập nhật số lượng và đơn giá (hoặc chỉ số lượng tùy theo yêu cầu)
                DialogResult result = MessageBox.Show($"Sản phẩm '{existingItem.TenSach}' đã có trong hóa đơn.\nBạn có muốn cộng thêm số lượng và cập nhật đơn giá không?",
                                                     "Sản phẩm đã tồn tại", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    existingItem.SLNhap += slNhap;
                    existingItem.DonGiaNhap = donGiaNhap; // Cập nhật đơn giá theo lần nhập mới nhất
                    existingItem.ThanhTien = existingItem.SLNhap * existingItem.DonGiaNhap; // Tính lại thành tiền
                    currentChiTietList.ResetItem(currentChiTietList.IndexOf(existingItem)); // Cập nhật lại dòng trên DataGridView
                }
            }
            else
            {
                // Nếu chưa tồn tại, thêm mới vào danh sách
                ChiTietHDNhap newItem = new ChiTietHDNhap
                {
                    MaSach = maSachChon,
                   
                    SLNhap = slNhap,
                    DonGiaNhap = donGiaNhap,
                    KhuyenMai = 0, // Thêm trường nhập liệu cho KhuyenMai nếu cần
                    ThanhTien = slNhap * donGiaNhap // Tính thành tiền
                };
                currentChiTietList.Add(newItem);
            }

            TinhTongTienHoaDon(); // Tính lại tổng tiền của hóa đơn
            ClearSanPhamFields(); // Xóa trắng các trường nhập liệu sản phẩm để chuẩn bị cho lần nhập tiếp theo
            cmbMaHang.Focus();    // Đưa con trỏ về ComboBox chọn hàng
        }

        private ChiTietHDNhap selectedChiTiet = null; // Lưu trữ sản phẩm đang được chọn trong dgvChiTietHoaDon

        private void dgvChiTietHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Đảm bảo click vào một dòng có dữ liệu thực sự trong currentChiTietList
            if (e.RowIndex >= 0 && e.RowIndex < currentChiTietList.Count)
            {
                selectedChiTiet = currentChiTietList[e.RowIndex];

                // Thêm kiểm tra: đảm bảo selectedChiTiet không null trước khi sử dụng
                if (selectedChiTiet != null)
                {
                    // Hiển thị thông tin của sản phẩm được chọn lên các trường nhập liệu

                    // Kiểm tra MaSach trước khi gán cho ComboBox
                    if (!string.IsNullOrEmpty(selectedChiTiet.MaSach))
                    {
                        // Cố gắng chọn giá trị. Nếu giá trị không tồn tại trong ComboBox,
                        // SelectedIndex sẽ tự động được đặt thành -1 (không có lỗi).
                        // Tuy nhiên, để chắc chắn, bạn có thể kiểm tra xem giá trị có trong ComboBox không
                        // nếu bạn muốn xử lý cụ thể hơn.
                        cmbMaHang.SelectedValue = selectedChiTiet.MaSach;
                    }
                    else
                    {
                        cmbMaHang.SelectedIndex = -1; // Nếu MaSach rỗng, không chọn gì trong ComboBox
                    }

                    // txtTenHang.Text = selectedChiTiet.TenSach; // TenHang sẽ tự cập nhật theo cmbMaHang
                    txtSoLuong.Text = selectedChiTiet.SLNhap.ToString();
                    txtDonGiaNhap.Text = selectedChiTiet.DonGiaNhap.ToString("N0");
                    txtThanhTien.Text = selectedChiTiet.ThanhTien.ToString("N0");

                    // Kích hoạt nút Sửa và Xóa sản phẩm, vô hiệu hóa nút Thêm sản phẩm
                    btnSuaSanPham.Enabled = true;
                    btnXoaSanPham.Enabled = true;
                    btnThemSanPham.Enabled = false;
                }
                else
                {
                    // Trường hợp này ít xảy ra nếu currentChiTietList được quản lý tốt
                    // và không chứa các phần tử null. Tuy nhiên, để an toàn:
                    ClearSanPhamFields();
                    // selectedChiTiet đã là null
                    btnSuaSanPham.Enabled = false;
                    btnXoaSanPham.Enabled = false;
                    btnThemSanPham.Enabled = true;
                }
            }
            else
            {
                // Nếu click ra ngoài dòng có dữ liệu hợp lệ (ví dụ: header, hàng trống mới ở cuối, hoặc khi danh sách rỗng)
                ClearSanPhamFields(); // Xóa thông tin trên các trường nhập sản phẩm
                selectedChiTiet = null;
                btnSuaSanPham.Enabled = false;
                btnXoaSanPham.Enabled = false;
                btnThemSanPham.Enabled = true; // Cho phép thêm mới
            }
        }

        private void btnSuaSanPham_Click(object sender, EventArgs e)
        {
            if (selectedChiTiet == null) // Kiểm tra xem đã chọn sản phẩm nào để sửa chưa
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm từ danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate dữ liệu nhập vào
            if (!int.TryParse(txtSoLuong.Text, out int slNhap) || slNhap <= 0)
            {
                MessageBox.Show("Số lượng nhập không hợp lệ. Phải là số nguyên dương.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoLuong.Focus();
                return;
            }
            if (!float.TryParse(txtDonGiaNhap.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out float donGiaNhap) || donGiaNhap < 0)
            {
                MessageBox.Show("Đơn giá nhập không hợp lệ. Phải là số không âm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDonGiaNhap.Focus();
                return;
            }

            // Cập nhật thông tin cho sản phẩm đã chọn trong danh sách currentChiTietList
            selectedChiTiet.SLNhap = slNhap;
            selectedChiTiet.DonGiaNhap = donGiaNhap;
            selectedChiTiet.ThanhTien = slNhap * donGiaNhap; // Tính lại thành tiền
            // Cập nhật KhuyenMai nếu có

            // currentChiTietList.ResetBindings(); // Cách này refresh toàn bộ list, có thể dùng ResetItem
            int index = currentChiTietList.IndexOf(selectedChiTiet);
            if (index != -1) currentChiTietList.ResetItem(index); // Chỉ refresh dòng được sửa


            TinhTongTienHoaDon(); // Tính lại tổng tiền hóa đơn
            ClearSanPhamFields(); // Xóa trắng các trường nhập sản phẩm
            selectedChiTiet = null; // Bỏ chọn sản phẩm

            // Reset trạng thái các nút sản phẩm
            btnSuaSanPham.Enabled = false;
            btnXoaSanPham.Enabled = false;
            btnThemSanPham.Enabled = true; // Cho phép thêm sản phẩm mới
            MessageBox.Show("Sản phẩm đã được cập nhật trong danh sách chi tiết.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXoaSanPham_Click(object sender, EventArgs e)
        {
            if (selectedChiTiet != null) // Kiểm tra xem đã chọn sản phẩm nào để xóa chưa
            {
                // Xác nhận trước khi xóa
                if (MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm '{selectedChiTiet.TenSach}' khỏi hóa đơn này không?",
                                    "Xác nhận xóa sản phẩm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    currentChiTietList.Remove(selectedChiTiet); // Xóa sản phẩm khỏi danh sách
                    TinhTongTienHoaDon(); // Tính lại tổng tiền
                    ClearSanPhamFields(); // Xóa trắng các trường nhập sản phẩm
                    selectedChiTiet = null; // Bỏ chọn

                    // Reset trạng thái các nút sản phẩm
                    btnSuaSanPham.Enabled = false;
                    btnXoaSanPham.Enabled = false;
                    btnThemSanPham.Enabled = true;
                    MessageBox.Show("Sản phẩm đã được xóa khỏi danh sách chi tiết.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm từ danh sách để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void ClearSanPhamFields()
        {
            cmbMaHang.SelectedIndex = -1; // Bỏ chọn ComboBox Mã hàng
                     // Xóa Tên hàng
            txtSoLuong.Clear();          // Xóa Số lượng
            txtDonGiaNhap.Clear();       // Xóa Đơn giá nhập
            txtThanhTien.Clear();        // Xóa Thành tiền
            dgvChiTietHoaDon.ClearSelection(); // Bỏ chọn dòng trên DataGridView chi tiết

            // Reset trạng thái nút (sau khi thêm/sửa/xóa sản phẩm hoặc khi chọn dòng khác)
            btnSuaSanPham.Enabled = false;
            btnXoaSanPham.Enabled = false;
            btnThemSanPham.Enabled = true; // Luôn cho phép thêm mới nếu không có sản phẩm nào được chọn để sửa/xóa
            selectedChiTiet = null;
        }
        // --- Các xử lý sự kiện cho nút bấm liên quan đến Hóa Đơn chính ---

        private void ClearHeaderFields()
        {
            txtMaHoaDon.Clear();
            dtpNgayNhap.Value = DateTime.Now; // Đặt ngày nhập là ngày hiện tại
            cmbMaNhanVien.SelectedIndex = -1;
            // txtTenNhanVien.Clear(); // Sẽ tự xóa khi cmbMaNhanVien thay đổi
            cmbMaNhaCungCap.SelectedIndex = -1;
            // txtTenNhaCungCap.Clear(); // Sẽ tự xóa khi cmbMaNhaCungCap thay đổi
            // txtDiaChiNCC.Clear();
            // txtDienThoaiNCC.Clear();
            txtMaHoaDonTimKiem.Clear();
        }

        private void btnThemHoaDon_Click(object sender, EventArgs e)
        {
            SetInitialFormState(); // Xóa các trường, thiết lập cho việc nhập mới
            txtMaHoaDon.Enabled = true; // Cho phép nhập mã hóa đơn mới
            txtMaHoaDon.Focus();
            // Có thể bạn muốn tự động tạo SoHDNhap ở đây
            // txtMaHoaDon.Text = GenerateNewSoHDNhap(); // Implement this function if needed
            SetEditingState(); // Bật nút Lưu, tắt các nút khác
            currentEditingHoaDon = null; // Đảm bảo đang thêm mới, không phải sửa
            originalChiTietListForEdit = null;
        }

        private void btnLuuHoaDon_Click(object sender, EventArgs e)
        {
            // Kiểm tra các thông tin bắt buộc của hóa đơn
            if (string.IsNullOrWhiteSpace(txtMaHoaDon.Text))
            {
                MessageBox.Show("Vui lòng nhập Số hóa đơn.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaHoaDon.Focus();
                return;
            }
            if (cmbMaNhanVien.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Nhân viên.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbMaNhanVien.Focus();
                return;
            }
            if (cmbMaNhaCungCap.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Nhà cung cấp.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbMaNhaCungCap.Focus();
                return;
            }

            if (currentChiTietList.Count == 0) // Kiểm tra xem hóa đơn có sản phẩm nào không
            {
                MessageBox.Show("Hóa đơn phải có ít nhất một sản phẩm trong chi tiết.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbMaHang.Focus(); // Hướng người dùng đến việc thêm sản phẩm
                return;
            }

            // Tạo đối tượng HoaDonNhap từ thông tin trên form
            HoaDonNhap hdn = new HoaDonNhap
            {
                SoHDNhap = txtMaHoaDon.Text.Trim(),
                NgayNhap = dtpNgayNhap.Value,
                MaNhanVien = cmbMaNhanVien.SelectedValue.ToString(),
                MaNCC = cmbMaNhaCungCap.SelectedValue.ToString(),
                TongTien = float.Parse(txtTongTien.Text.Replace(",", "")), // Bỏ dấu phẩy nếu có format
                ChiTiet = new List<ChiTietHDNhap>(currentChiTietList) // Sao chép danh sách chi tiết
            };

            bool success;

            if (currentEditingHoaDon != null && currentEditingHoaDon.SoHDNhap == hdn.SoHDNhap) // Đang ở chế độ sửa
            {
                // Đây là thao tác CẬP NHẬT.
                // QUAN TRỌNG: Hàm UpdateHoaDonNhap trong BusinessLogic cần phải mạnh mẽ.
                // Nó phải xử lý các thay đổi trong ChiTiet (xóa cũ, thêm mới, cập nhật KhoSach tương ứng)
                // Hiện tại, BusinessLogic.UpdateHoaDonNhap được đơn giản hóa.
                // Bạn cần truyền chi tiết gốc hoặc quản lý trạng thái cẩn thận để điều chỉnh kho chính xác.
                // MessageBox.Show("Chức năng cập nhật Hóa Đơn và chi tiết phức tạp, cần logic cẩn thận để hoàn tác và áp dụng thay đổi vào kho.", "Lưu ý Phát triển", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Logic phức tạp cho cập nhật:
                // 1. Xác định các chi tiết bị xóa, thêm mới, hoặc thay đổi số lượng/giá.
                // 2. Với các chi tiết bị xóa khỏi hóa đơn: hoàn tác số lượng trong kho.
                // 3. Với các chi tiết bị thay đổi: hoàn tác số lượng cũ, áp dụng số lượng mới và giá mới vào kho.
                // 4. Với các chi tiết mới được thêm vào hóa đơn: áp dụng số lượng và giá mới vào kho.
                // Do sự phức tạp này, cách đơn giản hơn (nhưng có thể không tối ưu hiệu năng với nhiều chi tiết) là:
                // a. Xóa tất cả chi tiết cũ của hóa đơn này (đồng thời hoàn tác kho).
                // b. Thêm tất cả chi tiết mới từ `currentChiTietList` (đồng thời cập nhật kho).
                // Tuy nhiên, hàm UpdateHoaDonNhap hiện tại chưa hỗ trợ điều này. Nó chỉ cập nhật header.
                // --> CẦN NÂNG CẤP HÀM UpdateHoaDonNhap TRONG BusinessLogic.cs
                success = hdnLogic.UpdateHoaDonNhap(hdn); // Cần nâng cấp hàm này!
                // Sau khi nâng cấp UpdateHoaDonNhap, bạn có thể cần gọi hàm này để xóa các chi tiết cũ
                // và thêm các chi tiết mới, đồng thời cập nhật kho.
            }
            else // Đây là thao tác THÊM MỚI (SoHDNhap chưa có hoặc khác với currentEditingHoaDon)
            {
                // Kiểm tra xem SoHDNhap đã tồn tại chưa trước khi thêm
                if (hdnLogic.GetHoaDonNhapById(hdn.SoHDNhap) != null)
                {
                    MessageBox.Show("Số hóa đơn này đã tồn tại trong hệ thống! Vui lòng nhập số khác.", "Lỗi trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMaHoaDon.Focus();
                    txtMaHoaDon.SelectAll();
                    return; // Không thực hiện thêm
                }
                success = hdnLogic.AddHoaDonNhap(hdn); // Gọi hàm thêm mới từ BusinessLogic
            }


            if (success)
            {
                MessageBox.Show((currentEditingHoaDon != null && currentEditingHoaDon.SoHDNhap == hdn.SoHDNhap) ? "Cập nhật hóa đơn thành công!" : "Thêm mới hóa đơn thành công!",
                                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadHoaDonNhapList();    // Tải lại danh sách hóa đơn
                SetInitialFormState();   // Reset form về trạng thái ban đầu
            }
            else
            {
                MessageBox.Show((currentEditingHoaDon != null && currentEditingHoaDon.SoHDNhap == hdn.SoHDNhap) ? "Cập nhật hóa đơn thất bại." : "Thêm mới hóa đơn thất bại.",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvHoaDonNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Đảm bảo click vào một dòng hợp lệ
            {
                DataGridViewRow row = dgvHoaDonNhap.Rows[e.RowIndex];
                string soHDNhap = row.Cells["SoHDNhap"].Value.ToString(); // Đảm bảo tên cột này khớp với DataGridView của bạn

                // Lấy thông tin đầy đủ của hóa đơn (bao gồm cả chi tiết) từ BusinessLogic
                HoaDonNhap hdn = hdnLogic.GetHoaDonNhapById(soHDNhap);
                if (hdn != null)
                {
                    currentEditingHoaDon = hdn; // Lưu hóa đơn này lại để biết là đang xem/sửa nó
                    originalChiTietListForEdit = new List<ChiTietHDNhap>(hdn.ChiTiet); // Sao chép danh sách chi tiết gốc

                    // Hiển thị thông tin hóa đơn lên các trường nhập liệu
                    txtMaHoaDon.Text = hdn.SoHDNhap;
                    dtpNgayNhap.Value = hdn.NgayNhap;

                    cmbMaNhanVien.SelectedValue = hdn.MaNhanVien;
                    // txtTenNhanVien.Text = hdn.TenNhanVien; // Sẽ tự cập nhật qua sự kiện SelectedIndexChanged của ComboBox

                    cmbMaNhaCungCap.SelectedValue = hdn.MaNCC;
                    // txtTenNhaCungCap.Text = hdn.TenNhaCungCap; // Sẽ tự cập nhật
                    // txtDiaChiNCC.Text = hdn.DiaChiNCC;
                    // txtDienThoaiNCC.Text = hdn.DienThoaiNCC;

                    txtTongTien.Text = hdn.TongTien.ToString("N0");

                    // Hiển thị danh sách chi tiết hóa đơn
                    currentChiTietList.Clear(); // Xóa danh sách chi tiết hiện tại
                    foreach (var ct in hdn.ChiTiet)
                    {
                        currentChiTietList.Add(ct); // Thêm các chi tiết của hóa đơn được chọn
                    }
                    // dgvChiTietHoaDon.DataSource = currentChiTietList; // Đã được gán nguồn, chỉ cần ResetBindings nếu không tự cập nhật
                    // currentChiTietList.ResetBindings(); // Đảm bảo UI được cập nhật (thường BindingList tự làm)

                    // Cập nhật trạng thái các nút
                    btnSuaHoaDon.Enabled = true;  // Cho phép sửa hóa đơn này
                    btnXoaHoaDon.Enabled = true;  // Cho phép xóa hóa đơn này
                    btnLuuHoaDon.Enabled = false; // Chưa ở chế độ lưu cho đến khi nhấn "Sửa"
                    btnThemHoaDon.Enabled = true; // Vẫn cho phép bắt đầu thêm hóa đơn mới
                    txtMaHoaDon.Enabled = false;  // Không cho sửa Mã Hóa Đơn (khóa chính) khi đã chọn

                    ClearSanPhamFields(); // Xóa các trường nhập sản phẩm
                }
            }
        }

        private void btnSuaHoaDon_Click(object sender, EventArgs e)
        {
            if (currentEditingHoaDon == null || string.IsNullOrWhiteSpace(txtMaHoaDon.Text)) // Kiểm tra xem đã chọn hóa đơn nào chưa
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn từ danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Dữ liệu đã được tải lên form khi click vào dgvHoaDonNhap
            SetEditingState();       // Bật các trường cho phép sửa, bật nút Lưu
            txtMaHoaDon.Enabled = false; // Không cho phép sửa Số Hóa Đơn (PK)
            dtpNgayNhap.Focus();
        }

        private void btnXoaHoaDon_Click(object sender, EventArgs e)
        {
            if (currentEditingHoaDon == null || string.IsNullOrWhiteSpace(txtMaHoaDon.Text)) // Kiểm tra xem đã chọn hóa đơn nào chưa
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn từ danh sách để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Xác nhận lại trước khi xóa
            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa hóa đơn số '{txtMaHoaDon.Text}' không?\n" +
                                 "Hành động này sẽ cập nhật (giảm) lại số lượng sách tương ứng trong kho.",
                                 "Xác nhận xóa hóa đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bool success = hdnLogic.DeleteHoaDonNhap(txtMaHoaDon.Text); // Gọi hàm xóa từ BusinessLogic
                if (success)
                {
                    MessageBox.Show("Xóa hóa đơn thành công! Số lượng sách trong kho đã được cập nhật.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadHoaDonNhapList();    // Tải lại danh sách
                    SetInitialFormState();   // Reset form
                }
                else
                {
                    MessageBox.Show("Xóa hóa đơn thất bại. Có lỗi xảy ra trong quá trình xử lý.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnTimhoadon_Click(object sender, EventArgs e)
        {
            string soHDCanTim = txtMaHoaDonTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(soHDCanTim))
            {
                LoadHoaDonNhapList(); // Tải lại toàn bộ danh sách nếu ô tìm kiếm trống
                return;
            }

            // Tìm dòng trong DataGridView
            DataGridViewRow foundRow = null;
            foreach (DataGridViewRow row in dgvHoaDonNhap.Rows)
            {
                // Đảm bảo cột "SoHDNhap" tồn tại và giá trị không null
                if (row.Cells["SoHDNhap"] != null && row.Cells["SoHDNhap"].Value != null &&
                    row.Cells["SoHDNhap"].Value.ToString().Equals(soHDCanTim, StringComparison.OrdinalIgnoreCase))
                {
                    foundRow = row;
                    break;
                }
            }

            if (foundRow != null)
            {
                dgvHoaDonNhap.ClearSelection();       // Xóa lựa chọn hiện tại
                foundRow.Selected = true;             // Chọn dòng tìm thấy
                dgvHoaDonNhap.CurrentCell = foundRow.Cells[0]; // Di chuyển focus đến dòng đó (chọn ô đầu tiên của dòng)

                // Kích hoạt sự kiện CellClick để tải chi tiết hóa đơn lên form
                dgvHoaDonNhap_CellClick(dgvHoaDonNhap, new DataGridViewCellEventArgs(dgvHoaDonNhap.CurrentCell.ColumnIndex, foundRow.Index));
            }
            else
            {
                MessageBox.Show($"Không tìm thấy hóa đơn có số '{soHDCanTim}'.", "Không tìm thấy", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadHoaDonNhapList(); // Tải lại toàn bộ nếu không tìm thấy để người dùng không bị kẹt ở kết quả tìm kiếm rỗng
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bnt_huy_Click(object sender, EventArgs e)
        {
            // Tùy chọn: Hỏi người dùng xác nhận nếu có thay đổi chưa lưu.
            // Điều này hữu ích nếu người dùng đã nhập nhiều dữ liệu.
            if (currentChiTietList.Any() || !string.IsNullOrWhiteSpace(txtMaHoaDon.Text) /* Thêm các kiểm tra khác nếu cần */)
            {
                DialogResult confirmResult = MessageBox.Show(
                    "Bạn có chắc chắn muốn hủy bỏ các thay đổi hiện tại không?",
                    "Xác nhận hủy",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.No)
                {
                    return; // Người dùng chọn không hủy, không làm gì cả
                }
            }

            // Gọi hàm SetInitialFormState để đưa form về trạng thái ban đầu.
            // Hàm này sẽ xóa các trường nhập liệu, danh sách chi tiết tạm thời,
            // và đặt lại trạng thái các nút.
            SetInitialFormState(true); // true để xóa cả phần header của hóa đơn

            // Có thể bạn muốn vô hiệu hóa nút Hủy sau khi đã hủy xong,
            // và chỉ bật lại khi người dùng bắt đầu một thao tác mới.
            // btnHuyBo.Enabled = false; // Tùy theo logic quản lý trạng thái nút của bạn
        }

        private void cmbMaNhaCungCap_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có thông tin hóa đơn để in không
            if (string.IsNullOrWhiteSpace(txtMaHoaDon.Text) || currentChiTietList == null || !currentChiTietList.Any())
            {
                MessageBox.Show("Không có thông tin hóa đơn hoặc chi tiết hóa đơn để in.\nVui lòng chọn hoặc tạo một hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            string tempPdfPath = string.Empty;

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
                string safeSoHDNhap = txtMaHoaDon.Text.Trim().Replace("/", "-").Replace("\\", "-");
                worksheet.Name = $"PhieuNhap_{safeSoHDNhap}";

                int currentRow = 1;
                Excel.Range currentRange;

                // --- Định nghĩa các cột sẽ xuất cho chi tiết phiếu nhập với độ rộng gợi ý ---
                // !!! BẠN CẦN TINH CHỈNH CÁC GIÁ TRỊ 'Width' NÀY CHO PHÙ HỢP VỚI KHỔ A4 NGANG !!!
                var exportColumns = new[] {
            new { Header = "Mã Sách", PropertyName = "MaSach", Width = 18, Align = Excel.XlHAlign.xlHAlignLeft, Format = (string)null },
            new { Header = "Tên Sách", PropertyName = "TenSach", Width = 45, Align = Excel.XlHAlign.xlHAlignLeft, Format = (string)null },
            new { Header = "SL Nhập", PropertyName = "SLNhap", Width = 10, Align = Excel.XlHAlign.xlHAlignRight, Format = "#,##0" },
            new { Header = "Đơn Giá Nhập", PropertyName = "DonGiaNhap", Width = 18, Align = Excel.XlHAlign.xlHAlignRight, Format = "#,##0" },
            new { Header = "Khuyến Mãi", PropertyName = "KhuyenMai", Width = 15, Align = Excel.XlHAlign.xlHAlignRight, Format = "#,##0" }, // Giả sử KM là số tiền
            new { Header = "Thành Tiền", PropertyName = "ThanhTien", Width = 20, Align = Excel.XlHAlign.xlHAlignRight, Format = "#,##0" }
        };
                int numberOfExportColumns = exportColumns.Length;

                // --- 1. Thông tin Cửa Hàng ---
                string tenCuaHang = "Cửa Hàng Minh Châu"; // Lấy từ cấu hình hoặc hằng số
                string diaChiCH = "Phượng Lích 1, Diễn Hoá, Diễn Châu, Nghệ An";
                string dienThoaiCH = "0335549158";

                worksheet.Cells[currentRow, 1].Value = tenCuaHang;
                worksheet.Cells[currentRow, 1].Font.Bold = true;
                worksheet.Cells[currentRow, 1].Font.Size = 12;
                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfExportColumns]];
                currentRange.Merge();
                currentRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                currentRow++;

                worksheet.Cells[currentRow, 1].Value = "Địa chỉ: " + diaChiCH;
                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfExportColumns]]; currentRange.Merge(); currentRow++;
                worksheet.Cells[currentRow, 1].Value = "Điện thoại: " + dienThoaiCH;
                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfExportColumns]]; currentRange.Merge(); currentRow++;
                currentRow++;

                // --- 2. Tiêu đề Phiếu Nhập Kho ---
                string reportTitle = "PHIẾU NHẬP KHO";
                worksheet.Cells[currentRow, 1].Value = reportTitle;
                worksheet.Cells[currentRow, 1].Font.Bold = true;
                worksheet.Cells[currentRow, 1].Font.Size = 16;
                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfExportColumns]]; currentRange.Merge(); currentRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; currentRow++;
                currentRow++;

                // --- 3. Thông tin chung của Phiếu Nhập ---
                worksheet.Cells[currentRow, 1].Value = $"Số PN: {txtMaHoaDon.Text.Trim()}";
                worksheet.Cells[currentRow, 1].Font.Bold = true;
                // Ngày nhập ở cột khác hoặc căn lề phải nếu trên cùng dòng
                Excel.Range ngayNhapCell = worksheet.Cells[currentRow, numberOfExportColumns];
                ngayNhapCell.Value = $"Ngày nhập: {dtpNgayNhap.Value:dd/MM/yyyy}";
                ngayNhapCell.Font.Bold = true;
                ngayNhapCell.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                currentRow++;

                worksheet.Cells[currentRow, 1].Value = $"Nhân viên: {cmbMaNhanVien.Text}"; // Lấy tên NV từ cmb
                currentRow++;
                currentRow++; // Dòng trống

                // --- 4. Thông tin Nhà Cung Cấp ---
                worksheet.Cells[currentRow, 1].Value = "THÔNG TIN NHÀ CUNG CẤP";
                worksheet.Cells[currentRow, 1].Font.Bold = true;
                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfExportColumns]]; currentRange.Merge(); currentRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; currentRow++;

                string tenNCC = cmbMaNhaCungCap.Text;
                string maNCC = cmbMaNhaCungCap.SelectedValue?.ToString() ?? "N/A";
                worksheet.Cells[currentRow, 1].Value = $"Nhà cung cấp: {tenNCC} (Mã NCC: {maNCC})";
                // Nếu bạn lấy thêm được địa chỉ, SĐT của NCC thì thêm vào đây
                // Ví dụ:
                // NhaCungCaphdn nccDetails = hdnLogic.GetNhaCungCapDetails(maNCC);
                // if (nccDetails != null) {
                //     currentRow++; worksheet.Cells[currentRow, 1].Value = $"Địa chỉ NCC: {nccDetails.DiaChi}";
                //     currentRow++; worksheet.Cells[currentRow, 1].Value = $"SĐT NCC: {nccDetails.DienThoai}";
                // }
                currentRow++;
                currentRow++; // Dòng trống trước bảng chi tiết

                // --- 5. Bảng Chi Tiết Phiếu Nhập ---
                int excelColIndex = 1;
                foreach (var colInfo in exportColumns)
                {
                    Excel.Range cell = worksheet.Cells[currentRow, excelColIndex];
                    cell.Value = colInfo.Header;
                    cell.Font.Bold = true;
                    cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                    cell.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    excelColIndex++;
                }
                currentRow++;

                foreach (ChiTietHDNhap detailItem in currentChiTietList)
                {
                    excelColIndex = 1;
                    foreach (var colInfo in exportColumns)
                    {
                        object value = typeof(ChiTietHDNhap).GetProperty(colInfo.PropertyName)?.GetValue(detailItem, null);
                        Excel.Range currentCell = worksheet.Cells[currentRow, excelColIndex];
                        currentCell.Value = value;
                        currentCell.HorizontalAlignment = colInfo.Align;
                        if (!string.IsNullOrEmpty(colInfo.Format))
                        {
                            currentCell.NumberFormat = colInfo.Format;
                        }
                        currentCell.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        excelColIndex++;
                    }
                    currentRow++;
                }
                // currentRow++; // Dòng trống sau bảng - có thể bỏ nếu muốn tổng tiền sát bảng

                // --- 6. Tổng tiền Phiếu Nhập ---
                int tongCongLabelCol = Math.Max(1, numberOfExportColumns - 1);
                int tongCongValueCol = numberOfExportColumns;

                worksheet.Cells[currentRow, tongCongLabelCol].Value = "Tổng cộng tiền nhập:";
                worksheet.Cells[currentRow, tongCongLabelCol].Font.Bold = true;
                worksheet.Cells[currentRow, tongCongLabelCol].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                float tongTienNhap = 0;
                string tongTienText = txtTongTien.Text; // txtTongTien của Form qlhoadonnhap
                System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CurrentCulture;
                if (float.TryParse(tongTienText, System.Globalization.NumberStyles.Any, culture, out tongTienNhap))
                {
                    worksheet.Cells[currentRow, tongCongValueCol].Value = tongTienNhap;
                }
                else { worksheet.Cells[currentRow, tongCongValueCol].Value = tongTienText; }
                worksheet.Cells[currentRow, tongCongValueCol].NumberFormat = "#,##0";
                worksheet.Cells[currentRow, tongCongValueCol].Font.Bold = true;
                worksheet.Cells[currentRow, tongCongValueCol].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                currentRow++;
                currentRow++;

                // --- 7. Ngày xuất (ở cuối, căn trái) ---
                worksheet.Cells[currentRow, 1].Value = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                worksheet.Cells[currentRow, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                currentRow++;

                // --- Áp dụng độ rộng cột ---
                for (int i = 0; i < exportColumns.Length; i++)
                {
                    Excel.Range column = (Excel.Range)worksheet.Columns[i + 1];
                    column.ColumnWidth = exportColumns[i].Width;
                }

                // --- Cấu hình trang in cho PDF (A4 Ngang) ---
                worksheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape; // Khổ ngang
                worksheet.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4;       // Khổ A4
                worksheet.PageSetup.Zoom = false;
                worksheet.PageSetup.FitToPagesWide = 1; // Fit theo chiều rộng 1 trang
                worksheet.PageSetup.FitToPagesTall = 1; // Cố gắng fit 1 trang chiều cao, đặt false nếu cho phép nhiều trang
                                                        // Điều chỉnh lề (đơn vị là point, 1 inch = 72 points)
                worksheet.PageSetup.LeftMargin = excelApp.InchesToPoints(0.4);
                worksheet.PageSetup.RightMargin = excelApp.InchesToPoints(0.4);
                worksheet.PageSetup.TopMargin = excelApp.InchesToPoints(0.5);
                worksheet.PageSetup.BottomMargin = excelApp.InchesToPoints(0.5);
                worksheet.PageSetup.HeaderMargin = excelApp.InchesToPoints(0.3);
                worksheet.PageSetup.FooterMargin = excelApp.InchesToPoints(0.3);
                // worksheet.PageSetup.CenterHorizontally = true; // Căn giữa trang theo chiều ngang (tùy chọn)


                // --- Xuất PDF tạm và Xem trước ---
                tempPdfPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".pdf");
                workbook.ExportAsFixedFormat(
                    Excel.XlFixedFormatType.xlTypePDF, tempPdfPath,
                    Excel.XlFixedFormatQuality.xlQualityStandard, true, true,
                    Type.Missing, Type.Missing, false, Type.Missing);

                DialogResult userAction = DialogResult.Cancel;
                Process pdfProcess = null;
                try
                {
                    pdfProcess = Process.Start(tempPdfPath);
                    userAction = MessageBox.Show(
                        "Phiếu nhập kho đã được mở để xem trước.\n\nBạn có muốn LƯU phiếu này không?",
                        "Xác Nhận Lưu Phiếu Nhập", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                }
                catch (Exception exPreview)
                {
                    MessageBox.Show("Không thể mở bản xem trước PDF: " + exPreview.Message +
                                    "\n\nTuy nhiên, bạn vẫn có thể chọn để lưu file.", "Lỗi Xem Trước", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    userAction = MessageBox.Show(
                        "Không thể mở xem trước. Bạn có muốn tiếp tục lưu phiếu nhập này không?",
                        "Xác Nhận Lưu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }

                if (userAction == DialogResult.Yes)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "PDF files (*.pdf)|*.pdf",
                        Title = "Chọn nơi lưu file PDF Phiếu Nhập Kho",
                        FileName = $"PhieuNhapKho_{safeSoHDNhap}.pdf"
                    };

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            if (pdfProcess != null && !pdfProcess.HasExited)
                            {
                                try { pdfProcess.CloseMainWindow(); pdfProcess.WaitForExit(1000); } catch { /* Bỏ qua lỗi nếu không đóng được */ }
                            }
                            File.Copy(tempPdfPath, saveFileDialog.FileName, true);
                            MessageBox.Show("Xuất Phiếu Nhập Kho PDF thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            try { Process.Start(saveFileDialog.FileName); }
                            catch (Exception exOpenFinal) { MessageBox.Show("Không thể mở file PDF đã lưu tự động: " + exOpenFinal.Message, "Lỗi Mở File", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                        }
                        catch (IOException ioEx) { MessageBox.Show($"Lỗi khi lưu file PDF: {ioEx.Message}\nVui lòng đóng trình xem PDF (nếu đang mở file tạm) và thử lại.", "Lỗi Lưu File", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        catch (Exception exSaveFinal) { MessageBox.Show("Lỗi khi lưu file PDF cuối cùng: " + exSaveFinal.Message, "Lỗi Lưu File", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                    else { MessageBox.Show("Thao tác lưu phiếu nhập đã được hủy.", "Đã hủy", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
                else { MessageBox.Show("Thao tác xuất phiếu nhập đã được hủy.", "Đã hủy", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
            catch (Exception exMain)
            {
                MessageBox.Show("Lỗi nghiêm trọng khi tạo phiếu nhập PDF: " + exMain.ToString(), "Lỗi Xuất File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                try
                {
                    if (worksheet != null) Marshal.ReleaseComObject(worksheet);
                    if (workbook != null) { workbook.Close(false); Marshal.ReleaseComObject(workbook); }
                    if (excelApp != null) { excelApp.Quit(); Marshal.ReleaseComObject(excelApp); }
                }
                catch { /* Bỏ qua lỗi giải phóng COM */ }
                worksheet = null; workbook = null; excelApp = null;

                if (!string.IsNullOrEmpty(tempPdfPath) && File.Exists(tempPdfPath))
                {
                    try { File.Delete(tempPdfPath); } catch { /* Bỏ qua lỗi xóa file tạm */ }
                }
                GC.Collect(); GC.WaitForPendingFinalizers();
            }
        }
    }
}

