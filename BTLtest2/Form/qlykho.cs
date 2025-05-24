using BTLtest2.Class;
using BTLtest2.function;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BTLtest2.Class.Sach;

namespace BTLtest2
{
    public partial class qlykho : Form
    {
        private functionqlykho qlkho;
        public qlykho()
        {
            InitializeComponent();
            qlkho = new functionqlykho();
        }
        
        private void ClearAllControls(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is System.Windows.Forms.TextBox)
                    ((System.Windows.Forms.TextBox)ctrl).Clear();
                else if (ctrl is System.Windows.Forms.ComboBox)
                    ((System.Windows.Forms.ComboBox)ctrl).SelectedIndex = -1;
                else if (ctrl is System.Windows.Forms.PictureBox)
                    ((System.Windows.Forms.PictureBox)ctrl).Image = null;
                else if (ctrl.HasChildren)
                    ClearAllControls(ctrl); // Đệ quy nếu có control con
            }
        }
        private khosach LayThongTinSachTuForm()
        {
            khosach sachMoi = new khosach();

            // Lấy các giá trị chuỗi, sử dụng Trim() để loại bỏ khoảng trắng thừa
            sachMoi.MaSach = txtMasach.Text.Trim();
            sachMoi.TenSach = txtTensach.Text.Trim();
            sachMoi.Anh = txtAnh.Text.Trim();

            // Chuyển đổi Số lượng với TryParse
            if (int.TryParse(txtSoluong.Text.Trim(), out int soLuongValue))
            {
                sachMoi.SoLuong = soLuongValue;
            }
            else
            {
                MessageBox.Show("Số lượng nhập không hợp lệ. Vui lòng nhập một số nguyên.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoluong.Focus();
                return null; // Trả về null để báo hiệu có lỗi
            }

            // Chuyển đổi Đơn giá nhập với TryParse
            if (decimal.TryParse(txtDongianhap.Text.Trim(), out decimal donGiaNhapValue))
            {
                sachMoi.DonGiaNhap = donGiaNhapValue; // DonGiaBan sẽ tự động được tính trong class Sach
            }
            else
            {
                MessageBox.Show("Đơn giá nhập không hợp lệ. Vui lòng nhập một số thập phân.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDongianhap.Focus();
                return null; // Trả về null để báo hiệu có lỗi
            }

            // Chuyển đổi Số trang với TryParse
            if (int.TryParse(txtSotrang.Text.Trim(), out int soTrangValue))
            {
                sachMoi.SoTrang = soTrangValue;
            }
            else
            {
                MessageBox.Show("Số trang nhập không hợp lệ. Vui lòng nhập một số nguyên.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSotrang.Focus();
                return null; // Trả về null để báo hiệu có lỗi
            }

            // Lấy giá trị từ ComboBox một cách an toàn
            // Nếu SelectedValue là null (chưa chọn), sẽ gán string.Empty
            // Quan trọng: Đảm bảo các thuộc tính MaLoaiSach, MaTacGia,... trong class Sach có thể chấp nhận string.Empty
            // hoặc bạn cần kiểm tra và yêu cầu người dùng chọn nếu chúng là bắt buộc.
            sachMoi.MaLoaiSach = cmbLoaiSach.SelectedValue?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(sachMoi.MaLoaiSach) && cmbLoaiSach.Items.Count > 0) // Ví dụ kiểm tra nếu Mã loại sách là bắt buộc
            {
                MessageBox.Show("Vui lòng chọn Loại sách.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbLoaiSach.Focus();
                return null;
            }

            sachMoi.MaTacGia = cmbTacGia.SelectedValue?.ToString() ?? string.Empty;
            // Thêm kiểm tra tương tự cho MaTacGia nếu bắt buộc

            sachMoi.MaNXB = cmbNXB.SelectedValue?.ToString() ?? string.Empty;
            // Thêm kiểm tra tương tự cho MaNXB nếu bắt buộc

            sachMoi.MaLinhVuc = cmbLinhVuc.SelectedValue?.ToString() ?? string.Empty;
            // Thêm kiểm tra tương tự cho MaLinhVuc nếu bắt buộc

            sachMoi.MaNgonNgu = cmbNgonNgu.SelectedValue?.ToString() ?? string.Empty;
            // Thêm kiểm tra tương tự cho MaNgonNgu nếu bắt buộc

            return sachMoi; // Trả về đối tượng Sach đã được điền thông tin
        }

        private void LoadData()
        {
            List<khosach> sachlist = functionqlykho.GetSach();
            dgvKhosach.AutoGenerateColumns = true;
            dgvKhosach.DataSource = null;
            dgvKhosach.DataSource = sachlist;
        }
        private void Load_DataGridView()
        {
            
        }

        private void ResetValues()
        {
           
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
           btnBoqua.Enabled = true;
            btnLuu.Enabled = true;
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnTim.Enabled = false;
            btnSua.Enabled = false;
            txtMasach.Enabled = true;
            txtMasach.Focus();
            ClearAllControls(this);
        }

       
        private void btnBoqua_Click(object sender, EventArgs e)
        {
          
        }



        private void btnTim_Click(object sender, EventArgs e)
        {
            
        }




        private void btnSua_Click(object sender, EventArgs e)
        {
            string MaSach = txtMasach.Text;
            if (string.IsNullOrEmpty(MaSach))
            {
                MessageBox.Show("Vui lòng chọn hàng hóa để sửa");
            }
            else
            {
                khosach ks = LayThongTinSachTuForm();
                functionqlykho.UpdateSach(ks);
                LoadData();
                MessageBox.Show("Cập nhật hàng hóa thành công");
                ClearAllControls(this); // 'this' là Form1
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Bước 1: Lấy thông tin từ Form
            // Đảm bảo LayThongTinSachTuForm() đã được sửa như gợi ý trước để trả về null nếu có lỗi nhập liệu
            khosach ks = LayThongTinSachTuForm();

            // Kiểm tra nếu LayThongTinSachTuForm() trả về null (do lỗi định dạng số, thiếu ComboBox bắt buộc,...)
            if (ks == null)
            {
                return; // Dừng lại, MessageBox lỗi đã được hiển thị từ LayThongTinSachTuForm()
            }

            // Bước 2: Kiểm tra các trường bắt buộc khác (nếu LayThongTinSachTuForm chưa kiểm tra hết)
            if (string.IsNullOrWhiteSpace(ks.MaSach))
            {
                MessageBox.Show("Mã sách không được để trống.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMasach.Focus(); // Focus vào ô Mã sách
                return;
            }
            if (string.IsNullOrWhiteSpace(ks.TenSach))
            {
                MessageBox.Show("Tên sách không được để trống.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTensach.Focus();
                return;
            }
            // Bạn có thể thêm các kiểm tra khác ở đây, ví dụ: ks.SoLuong > 0

            // Bước 3: Gọi phương thức AddSach và xử lý kết quả
            bool themThanhCong = false;
            try
            {
                themThanhCong = functionqlykho.AddSach(ks);
            }
            catch (Exception ex) // Bắt các lỗi không lường trước khác có thể xảy ra từ tầng dưới
            {
                MessageBox.Show("Đã xảy ra lỗi ngoài ý muốn khi thêm sách: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Ghi log lỗi chi tiết ex.ToString() nếu cần thiết cho việc gỡ lỗi sau này
            }

            // Bước 4: Hiển thị kết quả và cập nhật UI dựa trên việc thêm có thành công hay không
            if (themThanhCong)
            {
                MessageBox.Show("Thêm hàng hóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData(); // Tải lại dữ liệu lên DataGridView

                // Thiết lập lại trạng thái nút và form cho thao tác tiếp theo
                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnTim.Enabled = true;
                btnLuu.Enabled = false;
                btnBoqua.Enabled = false;
                txtMasach.Enabled = false; // Sau khi thêm, có thể không cho sửa Mã sách ngay
                ClearAllControls(this);    // Xóa các ô nhập liệu
            }
            else
            {
                // Thông báo lỗi chung. Nguyên nhân cụ thể đã được functionqlykho.AddSach ghi ra Console
                // (do Mã sách trùng, hoặc Mã Loại Sách/Tác Giả không tồn tại,...)
                MessageBox.Show("Thêm hàng hóa thất bại. \nNguyên nhân có thể do: \n - Mã sách đã tồn tại. \n - Các mã tham chiếu (Loại sách, Tác giả, NXB,...) không hợp lệ. \n\nVui lòng kiểm tra lại thông tin hoặc xem cửa sổ Output (khi chạy Debug) để biết chi tiết.", "Lỗi Thêm Sách", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Không LoadData() lại, giữ lại thông tin người dùng đã nhập để họ sửa.
            }
            // Không cần gọi LoadData() một lần nữa ở ngoài cùng.

        }
           
        private void btnXoa_Click(object sender, EventArgs e)
        {
            string MaSach = txtMasach.Text;
            if (string.IsNullOrEmpty(MaSach))
            {
                MessageBox.Show("Vui lòng chọn hàng hóa để xóa");
            }
            else
            {
                functionqlykho.DeleteSach(MaSach);
                LoadData();
                MessageBox.Show("Xóa hàng hóa thành công");
                ClearAllControls(this); // 'this' là Form1
            }
            LoadData();
        }


        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn ảnh";
                ofd.Filter = "Ảnh (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Hiển thị ảnh vào PictureBox
                    picAnh.Image = Image.FromFile(ofd.FileName);
                    picAnh.SizeMode = PictureBoxSizeMode.StretchImage;

                    // Nếu cần lưu đường dẫn ảnh
                    txtAnh.Text = ofd.FileName;
                }
            }
        }

        private void txtDongianhap_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void txtDongianhap_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void txtSoluong_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void dgvKhosach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnBoqua_Click_1(object sender, EventArgs e)
        {
            btnBoqua.Enabled = false;
            btnLuu.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnTim.Enabled = true;
            btnSua.Enabled = true;
            txtMasach.Enabled = false;
            ClearAllControls(this);
            LoadData();
        }

        private void qlykho_Load_1(object sender, EventArgs e)
        {
            LoadComboBoxes(); // Giữ lại nếu bạn vẫn dùng
            
            LoadData();
            
        }
        private void LoadComboBoxes()
        {
            try
            {
                // ComboBox Loại Sách
                cmbLoaiSach.DataSource = qlkho.GetLoaiSachForComboBox(); 
                cmbLoaiSach.DisplayMember = "TenLoai"; 
                cmbLoaiSach.ValueMember = "MaLoaiSach";   // Giả sử tên cột là MaLoaiSach
                cmbLoaiSach.SelectedIndex = -1;           // Không chọn mục nào ban đầu

                // ComboBox Tác Giả
                cmbTacGia.DataSource = qlkho.GetTacGiaForComboBox(); // Sẽ tạo phương thức này
                cmbTacGia.DisplayMember = "TenTacGia";  // Giả sử tên cột là TenTacGia
                cmbTacGia.ValueMember = "MaTacGia";    // Giả sử tên cột là MaTacGia
                cmbTacGia.SelectedIndex = -1;

                // ComboBox Nhà Xuất Bản
                cmbNXB.DataSource = qlkho.GetNXBForComboBox(); // Sẽ tạo phương thức này
                cmbNXB.DisplayMember = "TenNXB";      // Giả sử tên cột là TenNXB
                cmbNXB.ValueMember = "MaNXB";        // Giả sử tên cột là MaNXB
                cmbNXB.SelectedIndex = -1;

                // ComboBox Lĩnh Vực
                cmbLinhVuc.DataSource = qlkho.GetLinhVucForComboBox(); // Sẽ tạo phương thức này
                cmbLinhVuc.DisplayMember = "TenLinhVuc"; // Giả sử tên cột là TenLinhVuc
                cmbLinhVuc.ValueMember = "MaLinhVuc";   // Giả sử tên cột là MaLinhVuc
                cmbLinhVuc.SelectedIndex = -1;

                // ComboBox Ngôn Ngữ (đã có phương thức GetNgonNguForComboBox)
                cmbNgonNgu.DataSource = qlkho.GetNgonNguForComboBox();
                cmbNgonNgu.DisplayMember = "TenNgonNgu";
                cmbNgonNgu.ValueMember = "MaNgonNgu";
                cmbNgonNgu.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu cho ComboBox: \n" + ex.Message, "Lỗi Tải Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvKhosach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvKhosach.Rows[e.RowIndex];
                // Lấy giá trị từ các ô trong hàng đã chọn
                txtMasach.Text = row.Cells["MaSach"].Value.ToString();
                txtTensach.Text = row.Cells["TenSach"].Value.ToString();
                txtSoluong.Text = row.Cells["SoLuong"].Value.ToString();
                txtDongianhap.Text = row.Cells["DonGiaNhap"].Value.ToString();
                txtSotrang.Text = row.Cells["SoTrang"].Value.ToString();
                txtAnh.Text = row.Cells["Anh"].Value.ToString();
                string duongdan = row.Cells["Anh"].Value.ToString();
                txtAnh.Text = duongdan;
                // Chọn giá trị cho các ComboBox
                cmbLoaiSach.SelectedValue = row.Cells["MaLoaiSach"].Value;
                cmbTacGia.SelectedValue = row.Cells["MaTacGia"].Value;
                cmbNXB.SelectedValue = row.Cells["MaNXB"].Value;
                cmbLinhVuc.SelectedValue = row.Cells["MaLinhVuc"].Value;
                cmbNgonNgu.SelectedValue = row.Cells["MaNgonNgu"].Value;
                // Kiểm tra tệp tồn tại và hiển thị ảnh
                if (File.Exists(duongdan))
                {
                    picAnh.Image = Image.FromFile(duongdan);
                    picAnh.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    picAnh.Image = null; // hoặc gán ảnh mặc định
                    MessageBox.Show("Không tìm thấy ảnh: " + duongdan);
                }
                // Kích hoạt nút sửa và xóa
                btnXoa.Enabled = true;
                btnSua.Enabled = true;
            }
        }

        private void btnTim_Click_1(object sender, EventArgs e)
        {
            // Lấy các giá trị từ các control tìm kiếm trên form
            string tuKhoaMa = txtTimMaSach.Text.Trim(); // Giả sử bạn có TextBox txtTimMaSach
            string tuKhoaTen = txtTimTenSach.Text.Trim(); // Giả sử bạn có TextBox txtTimTenSach

            // Lấy giá trị từ ComboBox, nếu không chọn thì có thể là null hoặc string.Empty
            string maLoaiFilter = cmbLoaiSach.SelectedValue?.ToString() ?? string.Empty;
            string maTacGiaFilter = cmbTacGia.SelectedValue?.ToString() ?? string.Empty;
            string maNXBFilter = cmbNXB.SelectedValue?.ToString() ?? string.Empty;

            // Gọi phương thức tìm kiếm
            List<khosach> ketQuaTimKiem = functionqlykho.TimKiemSach(tuKhoaMa, tuKhoaTen, maLoaiFilter, maTacGiaFilter, maNXBFilter);

            // Hiển thị kết quả lên DataGridView
            dgvKhosach.DataSource = null; // Xóa dữ liệu cũ
            dgvKhosach.DataSource = ketQuaTimKiem;

            if (ketQuaTimKiem.Count == 0)
            {
                MessageBox.Show("Không tìm thấy sách nào phù hợp với tiêu chí của bạn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_matsach_Click(object sender, EventArgs e)
        {
            qlymatsach qlymatsach = new qlymatsach();
            qlymatsach.ShowDialog();
        }
    }
}
