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
using static BTLtest2.Class.clqlhoadonban;
using Excel = Microsoft.Office.Interop.Excel;

namespace BTLtest2
{
    public partial class quanlyhoadonban : Form
    {
        private HoaDonBanLogic hdbLogic;
        private BindingList<ChiTietHDBan> currentChiTietBanList;
        private HoaDonBan currentEditingHDBan = null;
        private List<ChiTietHDBan> originalChiTietListForEdit = null;


        public quanlyhoadonban()
        {
            InitializeComponent();
            hdbLogic = new HoaDonBanLogic();
            currentChiTietBanList = new BindingList<ChiTietHDBan>();
            dgvChiTietHDBan.DataSource = currentChiTietBanList;
        }
        private void quanlyhoadonban_Load(object sender, EventArgs e)
        {
            LoadHoaDonBanListToGrid();
            LoadComboBoxesHDB();
            ConfigureChiTietHDBDataGridView();
            SetInitialFormStateHDB(true);
        }

        private void LoadHoaDonBanListToGrid()
        {
            try
            {
                dgvHoaDonBan.DataSource = hdbLogic.GetHoaDonBanList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách hóa đơn bán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadComboBoxesHDB()
        {
            try
            {
                cmbMaKhachHang.DataSource = hdbLogic.GetKhachHangForComboBox();
                cmbMaKhachHang.DisplayMember = "TenKhach";
                cmbMaKhachHang.ValueMember = "MaKhach";
                cmbMaKhachHang.SelectedIndex = -1;

                cmbMaNhanVien.DataSource = hdbLogic.GetNhanVienForComboBox();
                cmbMaNhanVien.DisplayMember = "TenNhanVien";
                cmbMaNhanVien.ValueMember = "MaNhanVien";
                cmbMaNhanVien.SelectedIndex = -1;

                cmbMaHang.DataSource = hdbLogic.GetSachForBanComboBox();
                cmbMaHang.DisplayMember = "TenSach";
                cmbMaHang.ValueMember = "MaSach";
                cmbMaHang.SelectedIndex = -1;

                // Status ComboBox (Trạng Thái) - THÊM MỚI
                cboTrangThai.Items.Clear(); // Xóa các mục cũ nếu có
                cboTrangThai.Items.Add("Chưa thanh toán");
                cboTrangThai.Items.Add("Tiền mặt");
                cboTrangThai.Items.Add("Chuyển khoản");
                cboTrangThai.SelectedIndex = 0; // Chọn "Chưa thanh toán" làm mặc định
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu cho ComboBox: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ConfigureChiTietHDBDataGridView()
        {
            dgvChiTietHDBan.AutoGenerateColumns = false;
            // Thêm cột thủ công nếu bạn chưa làm trong Designer
            // Ví dụ:
            if (dgvChiTietHDBan.Columns["MaSachCol"] == null)
            {
                DataGridViewTextBoxColumn colMaSach = new DataGridViewTextBoxColumn { Name = "MaSachCol", HeaderText = "Mã Sách", DataPropertyName = "MaSach", Width = 80 };
                dgvChiTietHDBan.Columns.Add(colMaSach);
            }
            if (dgvChiTietHDBan.Columns["TenSachCol"] == null)
            {
                DataGridViewTextBoxColumn colTenSach = new DataGridViewTextBoxColumn { Name = "TenSachCol", HeaderText = "Tên Sách", DataPropertyName = "TenSach", Width = 200, ReadOnly = true };
                dgvChiTietHDBan.Columns.Add(colTenSach);
            }
            if (dgvChiTietHDBan.Columns["SLBanCol"] == null)
            {
                DataGridViewTextBoxColumn colSLBan = new DataGridViewTextBoxColumn { Name = "SLBanCol", HeaderText = "SL Bán", DataPropertyName = "SLBan", Width = 70 };
                dgvChiTietHDBan.Columns.Add(colSLBan);
            }
            if (dgvChiTietHDBan.Columns["DonGiaBanKhiBanCol"] == null)
            {
                DataGridViewTextBoxColumn colDonGia = new DataGridViewTextBoxColumn { Name = "DonGiaBanKhiBanCol", HeaderText = "Đơn Giá", DataPropertyName = "DonGiaBanKhiBan", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }, ReadOnly = true };
                dgvChiTietHDBan.Columns.Add(colDonGia);
            }
            if (dgvChiTietHDBan.Columns["KhuyenMaiCol"] == null)
            {
                DataGridViewTextBoxColumn colKhuyenMai = new DataGridViewTextBoxColumn { Name = "KhuyenMaiCol", HeaderText = "KM (%)", DataPropertyName = "KhuyenMai", Width = 70, DefaultCellStyle = new DataGridViewCellStyle { Format = "P0" } };
                dgvChiTietHDBan.Columns.Add(colKhuyenMai);
            }
            if (dgvChiTietHDBan.Columns["ThanhTienCol"] == null)
            {
                DataGridViewTextBoxColumn colThanhTien = new DataGridViewTextBoxColumn { Name = "ThanhTienCol", HeaderText = "Thành Tiền", DataPropertyName = "ThanhTien", Width = 120, DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }, ReadOnly = true };
                dgvChiTietHDBan.Columns.Add(colThanhTien);
            }
        }
        private void SetInitialFormStateHDB(bool clearHeader)
        {
            if (clearHeader) ClearHeaderFieldsHDB();
            currentChiTietBanList.Clear();
            ClearSanPhamFieldsHDB();
            txtTongTienHDB.Text = "0";

            txtMaHoaDon.Enabled = true;
            dtpNgayBan.Enabled = true;
            cmbMaKhachHang.Enabled = true;
            cmbMaNhanVien.Enabled = true;

            cmbMaHang.Enabled = true;
            txtSoLuongBan.Enabled = true;
            txtGiamGia.Enabled = true;

            btnThemHoaDonHDB.Enabled = true;
            btnLuuHDB.Enabled = false;
            btnSuaHoaDonHDB.Enabled = false;
            btnXoaHoaDonHDB.Enabled = false;
            btnHuyHoaDonHDB.Enabled = false;

            btnThemSanPhamHDB.Enabled = true;
            btnSuaSanPhamHDB.Enabled = false;
            btnXoaSanPhamHDB.Enabled = false;

            dgvHoaDonBan.ClearSelection();
            dgvChiTietHDBan.ClearSelection();
            currentEditingHDBan = null;
            originalChiTietListForEdit = null;
            cmbMaNhanVien.Enabled = false;
            cboTrangThai.Enabled = false; // SỬA ĐỔI: Mặc định disable khi xem

        }

        private void SetEditingStateHDB()
        {
            dtpNgayBan.Enabled = true;
            cmbMaKhachHang.Enabled = true;
            cmbMaNhanVien.Enabled = true;

            cmbMaHang.Enabled = true;
            txtSoLuongBan.Enabled = true;
            txtGiamGia.Enabled = true;

            btnThemHoaDonHDB.Enabled = false;
            btnLuuHDB.Enabled = true;
            btnSuaHoaDonHDB.Enabled = false;
            btnXoaHoaDonHDB.Enabled = false;
            btnHuyHoaDonHDB.Enabled = true;

            btnThemSanPhamHDB.Enabled = true;
            cmbMaNhanVien.Enabled = true;
            cboTrangThai.Enabled = true; // SỬA ĐỔI: CHO PHÉP CHỈNH SỬA TRẠNG THÁI

        }


        private void ClearHeaderFieldsHDB()
        {
            txtMaHoaDon.Clear();
            dtpNgayBan.Value = DateTime.Now;
            cmbMaKhachHang.SelectedIndex = -1;
            cmbMaNhanVien.SelectedIndex = -1;
            txtMaHoaDon.Clear();
            dtpNgayBan.Value = DateTime.Now;
            cmbMaKhachHang.SelectedIndex = -1;
            cmbMaNhanVien.SelectedIndex = -1;
            // THÊM MỚI HOẶC DI CHUYỂN VÀO ĐÂY: Đặt lại ComboBox trạng thái
            if (cboTrangThai.Items.Count > 0)
            {
                cboTrangThai.SelectedIndex = 0; // Ví dụ: "Chưa thanh toán"
            }
            else
            {
                cboTrangThai.SelectedItem = null;
            }
        }

        private void ClearSanPhamFieldsHDB()
        {
            cmbMaHang.SelectedIndex = -1;
            txtSoLuongBan.Clear();
            txtGiamGia.Text = "0";
            txtThanhTienHDB.Clear();
            // lblDonGiaHienThi.Text = "0"; 
            // lblSoLuongTonHienThi.Text = "0";
           

            dgvChiTietHDBan.ClearSelection();
            selectedChiTietBan = null;

            btnSuaSanPhamHDB.Enabled = false;
            btnXoaSanPhamHDB.Enabled = false;
            btnThemSanPhamHDB.Enabled = true;
        }

        private float currentDonGiaBanFromKho = 0;
        private int currentSoLuongTonTrongKho = 0;

        private void cmbMaHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMaHang.SelectedItem is DataRowView drv)
            {
                try
                {
                    object soLuongObj = drv["SoLuong"];
                    Console.WriteLine($"DEBUG: cmbMaHang_SelectedIndexChanged - drv['SoLuong'] raw value: {soLuongObj}, type: {soLuongObj?.GetType().ToString() ?? "null"}");

                    if (soLuongObj != DBNull.Value && soLuongObj != null)
                    {
                        currentSoLuongTonTrongKho = Convert.ToInt32(soLuongObj);
                    }
                    else
                    {
                        currentSoLuongTonTrongKho = 0;
                        Console.WriteLine($"DEBUG: cmbMaHang_SelectedIndexChanged - drv['SoLuong'] was DBNull or null, setting currentSoLuongTonTrongKho to 0.");
                    }
                    Console.WriteLine($"DEBUG: cmbMaHang_SelectedIndexChanged - currentSoLuongTonTrongKho after conversion: {currentSoLuongTonTrongKho}");

                    object donGiaBanObj = drv["DonGiaBan"];
                    Console.WriteLine($"DEBUG: cmbMaHang_SelectedIndexChanged - drv['DonGiaBan'] raw value: {donGiaBanObj}, type: {donGiaBanObj?.GetType().ToString() ?? "null"}");
                    if (donGiaBanObj != DBNull.Value && donGiaBanObj != null)
                    {
                        currentDonGiaBanFromKho = Convert.ToSingle(donGiaBanObj);
                    }
                    else
                    {
                        currentDonGiaBanFromKho = 0;
                        Console.WriteLine($"DEBUG: cmbMaHang_SelectedIndexChanged - drv['DonGiaBan'] was DBNull or null, setting currentDonGiaBanFromKho to 0.");
                    }
                    Console.WriteLine($"DEBUG: cmbMaHang_SelectedIndexChanged - currentDonGiaBanFromKho after conversion: {currentDonGiaBanFromKho}");

                    txtSoLuongBan.Text = "1";
                    txtGiamGia.Text = "0";
                    txtSoLuongBan.Focus();
                    TinhThanhTienSanPhamHDB();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR in cmbMaHang_SelectedIndexChanged: {ex.ToString()}");
                    currentDonGiaBanFromKho = 0;
                    currentSoLuongTonTrongKho = 0;
                    MessageBox.Show($"Lỗi khi xử lý lựa chọn sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                currentDonGiaBanFromKho = 0;
                currentSoLuongTonTrongKho = 0;
                txtThanhTienHDB.Clear();
                Console.WriteLine($"DEBUG: cmbMaHang_SelectedIndexChanged - SelectedItem is not DataRowView or null.");
            }
        }

        private void TinhThanhTienSanPhamHDB()
        {
            if (int.TryParse(txtSoLuongBan.Text, out int slBan) && slBan > 0)
            {
                float giamGiaPercent = 0;
                if (float.TryParse(txtGiamGia.Text, out float gg) && gg >= 0 && gg <= 100)
                {
                    giamGiaPercent = gg / 100.0f;
                }
                float thanhTien = (slBan * currentDonGiaBanFromKho) * (1 - giamGiaPercent);
                txtThanhTienHDB.Text = thanhTien.ToString("N0");
            }
            else
            {
                txtThanhTienHDB.Text = "0";
            }
        }

        private void txtSoLuongBan_TextChanged(object sender, EventArgs e) { TinhThanhTienSanPhamHDB(); }
        private void txtGiamGia_TextChanged(object sender, EventArgs e) { TinhThanhTienSanPhamHDB(); }
        private void txtSoLuongBan_KeyPress(object sender, KeyPressEventArgs e) { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true; }
        private void txtGiamGia_KeyPress(object sender, KeyPressEventArgs e) { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.') e.Handled = true; if (e.KeyChar == '.' && (sender as TextBox).Text.Contains('.')) e.Handled = true; }


        private void TinhTongTienHoaDonHDB()
        {
            float tong = 0;
            foreach (var item in currentChiTietBanList)
            {
                tong += item.ThanhTien;
            }
            txtTongTienHDB.Text = tong.ToString("N0");
        }


        private void btnThemHoaDon_Click(object sender, EventArgs e)
        {

        }

        private void btnThemSanPhamHDB_Click(object sender, EventArgs e)
        {
            if (cmbMaHang.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbMaHang.Focus(); return;
            }
            if (!int.TryParse(txtSoLuongBan.Text, out int slBan) || slBan <= 0)
            {
                MessageBox.Show("Số lượng bán phải là số nguyên dương.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoLuongBan.Focus(); return;
            }
            if (slBan > currentSoLuongTonTrongKho)
            {
                MessageBox.Show($"Số lượng tồn kho của '{cmbMaHang.Text}' không đủ (Tồn: {currentSoLuongTonTrongKho}, Bán: {slBan}).", "Hết hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuongBan.Focus(); return;
            }

            float giamGia = 0;
            if (float.TryParse(txtGiamGia.Text, out float gg) && gg >= 0 && gg <= 100) giamGia = gg / 100.0f;

            string maSachChon = cmbMaHang.SelectedValue.ToString();
            var existingItem = currentChiTietBanList.FirstOrDefault(item => item.MaSach == maSachChon);

            if (existingItem != null)
            {
                int tongSLBanMoi = existingItem.SLBan + slBan;
                if (tongSLBanMoi > currentSoLuongTonTrongKho)
                {
                    MessageBox.Show($"Tổng số lượng bán ({tongSLBanMoi}) vượt quá số lượng tồn kho ({currentSoLuongTonTrongKho}) cho '{cmbMaHang.Text}'.", "Hết hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                existingItem.SLBan = tongSLBanMoi;
                existingItem.KhuyenMai = giamGia;
                existingItem.DonGiaBanKhiBan = currentDonGiaBanFromKho;
                existingItem.ThanhTien = (existingItem.SLBan * existingItem.DonGiaBanKhiBan) * (1 - existingItem.KhuyenMai);
                currentChiTietBanList.ResetItem(currentChiTietBanList.IndexOf(existingItem));
            }
            else
            {
                currentChiTietBanList.Add(new ChiTietHDBan
                {
                    MaSach = maSachChon,
                    TenSach = cmbMaHang.Text,
                    SLBan = slBan,
                    DonGiaBanKhiBan = currentDonGiaBanFromKho,
                    KhuyenMai = giamGia,
                    ThanhTien = (slBan * currentDonGiaBanFromKho) * (1 - giamGia)
                });
            }
            TinhTongTienHoaDonHDB();
            ClearSanPhamFieldsHDB();
            cmbMaHang.Focus();
            btnHuyHoaDonHDB.Enabled = true;
        }
        private ChiTietHDBan selectedChiTietBan = null;
        private void dgvChiTietHDBan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < currentChiTietBanList.Count)
            {
                selectedChiTietBan = currentChiTietBanList[e.RowIndex];
                if (selectedChiTietBan != null)
                {
                    cmbMaHang.SelectedValue = selectedChiTietBan.MaSach;
                    txtSoLuongBan.Text = selectedChiTietBan.SLBan.ToString();
                    txtGiamGia.Text = (selectedChiTietBan.KhuyenMai * 100).ToString();
                    txtThanhTienHDB.Text = selectedChiTietBan.ThanhTien.ToString("N0");

                    btnSuaSanPhamHDB.Enabled = true;
                    btnXoaSanPhamHDB.Enabled = true;
                    btnThemSanPhamHDB.Enabled = false;
                    btnHuyHoaDonHDB.Enabled = true;
                }
            }
            else
            {
                ClearSanPhamFieldsHDB();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (selectedChiTietBan == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm từ danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(txtSoLuongBan.Text, out int slBan) || slBan <= 0)
            {
                MessageBox.Show("Số lượng bán phải là số nguyên dương.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoLuongBan.Focus(); return;
            }

            if (slBan > (currentSoLuongTonTrongKho + selectedChiTietBan.SLBan))
            {
                MessageBox.Show($"Số lượng bán mới ({slBan}) vượt quá số lượng có thể bán cho '{cmbMaHang.Text}'. Tối đa: {currentSoLuongTonTrongKho + selectedChiTietBan.SLBan}", "Hết hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            float giamGia = 0;
            if (float.TryParse(txtGiamGia.Text, out float gg) && gg >= 0 && gg <= 100) giamGia = gg / 100.0f;

            selectedChiTietBan.SLBan = slBan;
            selectedChiTietBan.KhuyenMai = giamGia;
            selectedChiTietBan.DonGiaBanKhiBan = currentDonGiaBanFromKho;
            selectedChiTietBan.ThanhTien = (slBan * selectedChiTietBan.DonGiaBanKhiBan) * (1 - giamGia);

            currentChiTietBanList.ResetItem(currentChiTietBanList.IndexOf(selectedChiTietBan));
            TinhTongTienHoaDonHDB();
            ClearSanPhamFieldsHDB();
            MessageBox.Show("Sản phẩm đã được cập nhật trong danh sách.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btnXoaSanPhamHDB_Click(object sender, EventArgs e)
        {
            if (selectedChiTietBan != null)
            {
                if (MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm '{selectedChiTietBan.TenSach}' khỏi hóa đơn này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    currentChiTietBanList.Remove(selectedChiTietBan);
                    TinhTongTienHoaDonHDB();
                    ClearSanPhamFieldsHDB();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnThemHoaDonHDB_Click(object sender, EventArgs e)
        {
            SetInitialFormStateHDB(true);
            txtMaHoaDon.Enabled = true;
            txtMaHoaDon.Focus();
            SetEditingStateHDB();
            currentEditingHDBan = null;
            originalChiTietListForEdit = null;
        }

        private void btnLuuHDB_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaHoaDon.Text)) { MessageBox.Show("Vui lòng nhập Mã hóa đơn.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtMaHoaDon.Focus(); return; }
            if (cmbMaKhachHang.SelectedValue == null) { MessageBox.Show("Vui lòng chọn Khách hàng.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); cmbMaKhachHang.Focus(); return; }
            if (cmbMaNhanVien.SelectedValue == null) { MessageBox.Show("Vui lòng chọn Nhân viên.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); cmbMaNhanVien.Focus(); return; }
            if (currentChiTietBanList.Count == 0) { MessageBox.Show("Hóa đơn phải có ít nhất một sản phẩm.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); cmbMaHang.Focus(); return; }
            if (cboTrangThai.SelectedItem == null) { MessageBox.Show("Vui lòng chọn Trạng thái hóa đơn.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); cboTrangThai.Focus(); return; } // SỬA ĐỔI: THÊM KIỂM TRA


            HoaDonBan hdb = new HoaDonBan
            {
                SoHDBan = txtMaHoaDon.Text.Trim(),
                NgayBan = dtpNgayBan.Value,
                MaKhach = cmbMaKhachHang.SelectedValue.ToString(),
                MaNhanVien = cmbMaNhanVien.SelectedValue.ToString(),
                TongTien = float.Parse(txtTongTienHDB.Text.Replace(",", "")),
                TrangThai = cboTrangThai.SelectedItem.ToString(), // SỬA ĐỔI: LẤY TỪ COMBOBOX

                ChiTiet = new List<ChiTietHDBan>(currentChiTietBanList.Select(ct => new ChiTietHDBan
                {
                    MaSach = ct.MaSach,
                    SLBan = ct.SLBan,
                    DonGiaBanKhiBan = ct.DonGiaBanKhiBan,
                    KhuyenMai = ct.KhuyenMai,
                    ThanhTien = ct.ThanhTien,
                    TenSach = ct.TenSach,
                    SoHDBan = txtMaHoaDon.Text.Trim() // Gán SoHDBan cho chi tiết
                }))
            };

            bool success = false;
            try
            {
                if (currentEditingHDBan != null && currentEditingHDBan.SoHDBan == hdb.SoHDBan)
                {
                    success = hdbLogic.UpdateHoaDonBan(hdb, originalChiTietListForEdit);
                }
                else
                {
                    if (hdbLogic.GetHoaDonBanById(hdb.SoHDBan) != null)
                    {
                        MessageBox.Show("Số hóa đơn bán này đã tồn tại!", "Lỗi trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtMaHoaDon.Focus(); return;
                    }
                    success = hdbLogic.AddHoaDonBan(hdb);
                }

                if (success)
                {
                    MessageBox.Show((currentEditingHDBan != null && currentEditingHDBan.SoHDBan == hdb.SoHDBan) ? "Cập nhật hóa đơn bán thành công!" : "Thêm mới hóa đơn bán thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadHoaDonBanListToGrid();
                    SetInitialFormStateHDB(true);
                    LoadComboBoxesHDB();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu hóa đơn bán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuaHoaDonHDB_Click(object sender, EventArgs e)
        {
            if (currentEditingHDBan == null)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn từ danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            txtMaHoaDon.Enabled = false;
            SetEditingStateHDB();
            dtpNgayBan.Focus();
        }

        private void btnXoaHoaDonHDB_Click(object sender, EventArgs e)
        {
            if (currentEditingHDBan == null)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa hóa đơn '{currentEditingHDBan.SoHDBan}' không?\nHành động này sẽ hoàn trả số lượng sách vào kho.", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (hdbLogic.DeleteHoaDonBan(currentEditingHDBan.SoHDBan))
                    {
                        MessageBox.Show("Xóa hóa đơn bán thành công! Số lượng sách đã được hoàn trả vào kho.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadHoaDonBanListToGrid();
                        SetInitialFormStateHDB(true);
                        LoadComboBoxesHDB();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa hóa đơn bán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHuyHoaDonHDB_Click(object sender, EventArgs e)
        {
            if (currentEditingHDBan != null)
            {
                bool changed = currentChiTietBanList.Count != (originalChiTietListForEdit?.Count ?? 0);
                if (!changed && originalChiTietListForEdit != null)
                {
                    // So sánh chi tiết hơn nếu số lượng item bằng nhau
                    for (int i = 0; i < currentChiTietBanList.Count; i++)
                    {
                        if (currentChiTietBanList[i].MaSach != originalChiTietListForEdit[i].MaSach ||
                           currentChiTietBanList[i].SLBan != originalChiTietListForEdit[i].SLBan ||
                           currentChiTietBanList[i].KhuyenMai != originalChiTietListForEdit[i].KhuyenMai)
                        {
                            changed = true;
                            break;
                        }
                    }
                }

                if (changed ||
                    (currentEditingHDBan.NgayBan.Date != dtpNgayBan.Value.Date) || // So sánh ngày, bỏ qua giờ
                    (currentEditingHDBan.MaKhach != (cmbMaKhachHang.SelectedValue?.ToString() ?? "")) ||
                    (currentEditingHDBan.MaNhanVien != (cmbMaNhanVien.SelectedValue?.ToString() ?? ""))
                   )
                {
                    DialogResult confirm = MessageBox.Show("Bạn có thay đổi chưa lưu. Bạn có chắc muốn hủy và tải lại hóa đơn gốc?", "Xác nhận hủy sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirm == DialogResult.Yes)
                    {
                        if (dgvHoaDonBan.CurrentRow != null)
                            dgvHoaDonBan_CellClick(dgvHoaDonBan, new DataGridViewCellEventArgs(0, dgvHoaDonBan.CurrentRow.Index));
                        else
                            SetInitialFormStateHDB(true); // Nếu không có dòng nào đang chọn, reset
                    }
                    return; // Dừng lại dù có hủy hay không nếu đang sửa
                }
            }

            if (currentEditingHDBan == null && (currentChiTietBanList.Any() || !string.IsNullOrWhiteSpace(txtMaHoaDon.Text)))
            {
                DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn hủy bỏ việc thêm hóa đơn mới này không?", "Xác nhận hủy thêm mới", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    SetInitialFormStateHDB(true);
                }
            }
            else
            {
                SetInitialFormStateHDB(true);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTimhoadon_Click(object sender, EventArgs e)
        {
            string soHDCanTim = txtTimKiemHDB.Text.Trim();
            if (string.IsNullOrEmpty(soHDCanTim))
            {
                LoadHoaDonBanListToGrid(); return;
            }

            if (dgvHoaDonBan.DataSource is DataTable dt)
            {
                DataRow[] foundRows = dt.Select($"SoHDBan = '{soHDCanTim.Replace("'", "''")}'");
                if (foundRows.Length > 0)
                {
                    foreach (DataGridViewRow row in dgvHoaDonBan.Rows)
                    {
                        if (row.Cells["SoHDBan"] != null && row.Cells["SoHDBan"].Value.ToString().Equals(soHDCanTim, StringComparison.OrdinalIgnoreCase))
                        {
                            dgvHoaDonBan.ClearSelection();
                            row.Selected = true;
                            dgvHoaDonBan.CurrentCell = row.Cells[0];
                            dgvHoaDonBan_CellClick(dgvHoaDonBan, new DataGridViewCellEventArgs(0, row.Index));
                            return;
                        }
                    }
                }
            }
            MessageBox.Show($"Không tìm thấy hóa đơn bán có số '{soHDCanTim}'.", "Không tìm thấy", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        private void dgvChiTietHDBan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSuaSanPhamHDB_Click(object sender, EventArgs e)
        {
            if (selectedChiTietBan == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm từ danh sách để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(txtSoLuongBan.Text, out int slBan) || slBan <= 0)
            {
                MessageBox.Show("Số lượng bán phải là số nguyên dương.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoLuongBan.Focus(); return;
            }

            if (slBan > (currentSoLuongTonTrongKho + selectedChiTietBan.SLBan))
            {
                MessageBox.Show($"Số lượng bán mới ({slBan}) vượt quá số lượng có thể bán cho '{cmbMaHang.Text}'. Tối đa: {currentSoLuongTonTrongKho + selectedChiTietBan.SLBan}", "Hết hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            float giamGia = 0;
            if (float.TryParse(txtGiamGia.Text, out float gg) && gg >= 0 && gg <= 100) giamGia = gg / 100.0f;

            selectedChiTietBan.SLBan = slBan;
            selectedChiTietBan.KhuyenMai = giamGia;
            selectedChiTietBan.DonGiaBanKhiBan = currentDonGiaBanFromKho;
            selectedChiTietBan.ThanhTien = (slBan * selectedChiTietBan.DonGiaBanKhiBan) * (1 - giamGia);

            currentChiTietBanList.ResetItem(currentChiTietBanList.IndexOf(selectedChiTietBan));
            TinhTongTienHoaDonHDB();
            ClearSanPhamFieldsHDB();
            MessageBox.Show("Sản phẩm đã được cập nhật trong danh sách.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvHoaDonBan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvHoaDonBan.Rows.Count - (dgvHoaDonBan.AllowUserToAddRows ? 1 : 0))
            {
                DataGridViewRow row = dgvHoaDonBan.Rows[e.RowIndex];
                if (row.Cells["SoHDBan"].Value == null) return; // Bỏ qua nếu ô SoHDBan rỗng
                string soHDBan = row.Cells["SoHDBan"].Value.ToString();

                HoaDonBan hdb = hdbLogic.GetHoaDonBanById(soHDBan);
                if (hdb != null)
                {
                    currentEditingHDBan = hdb;
                    originalChiTietListForEdit = new List<ChiTietHDBan>(hdb.ChiTiet.Select(ct => new ChiTietHDBan
                    {
                        SoHDBan = ct.SoHDBan,
                        MaSach = ct.MaSach,
                        SLBan = ct.SLBan,
                        DonGiaBanKhiBan = ct.DonGiaBanKhiBan,
                        KhuyenMai = ct.KhuyenMai,
                        ThanhTien = ct.ThanhTien,
                        TenSach = ct.TenSach
                    }));


                    txtMaHoaDon.Text = hdb.SoHDBan;
                    dtpNgayBan.Value = hdb.NgayBan;
                    cmbMaKhachHang.SelectedValue = hdb.MaKhach;
                    cmbMaNhanVien.SelectedValue = hdb.MaNhanVien;
                    cboTrangThai.SelectedItem = hdb.TrangThai; // SỬA ĐỔI: HIỂN THỊ TRẠNG THÁI
                    txtTongTienHDB.Text = hdb.TongTien.ToString("N0");

                    currentChiTietBanList.Clear();
                    foreach (var ct in hdb.ChiTiet)
                    {
                        currentChiTietBanList.Add(ct);
                    }

                    txtMaHoaDon.Enabled = false;
                    btnSuaHoaDonHDB.Enabled = true;
                    btnXoaHoaDonHDB.Enabled = true;
                    btnLuuHDB.Enabled = false;
                    btnThemHoaDonHDB.Enabled = true;
                    btnHuyHoaDonHDB.Enabled = true;
                    ClearSanPhamFieldsHDB();
                }
            }
        }

        private void dgvHoaDonBan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaHoaDon.Text) || currentChiTietBanList == null || !currentChiTietBanList.Any())
            {
                MessageBox.Show("Không có thông tin hóa đơn hoặc chi tiết hóa đơn để in. Vui lòng chọn hoặc tạo một hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            string tempPdfPath = string.Empty; // Đường dẫn đến file PDF tạm

            try
            {
                excelApp = new Excel.Application();
                if (excelApp == null)
                {
                    MessageBox.Show("Không thể khởi tạo ứng dụng Excel...", "Lỗi Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                workbook = excelApp.Workbooks.Add(Type.Missing);
                worksheet = (Excel.Worksheet)workbook.ActiveSheet;
                string safeSoHDBan = txtMaHoaDon.Text.Trim().Replace("/", "-").Replace("\\", "-");
                worksheet.Name = $"HoaDon_{safeSoHDBan}";

                // --- BEGIN: Dựng nội dung Excel cho hóa đơn ---
                // (Toàn bộ code dựng nội dung Excel của bạn từ bước trước: thông tin cửa hàng,
                // tiêu đề hóa đơn, thông tin chung, thông tin khách hàng, bảng chi tiết, tổng tiền,
                // ngày bán, ngày xuất, cài đặt trang in... sẽ nằm ở đây)
                // Ví dụ rút gọn:
                int currentRow = 1;
                // ... (Thêm các dòng thông tin vào worksheet) ...
                // (Giả sử bạn đã có code hoàn chỉnh từ lần trả lời trước để điền dữ liệu vào worksheet)

                // --- (Bắt đầu phần code điền dữ liệu ví dụ từ phản hồi trước) ---
                Excel.Range currentRange;
                var exportColumns = new[] {
            new { Header = "Tên Sách", PropertyName = "TenSach", Width = 45, Align = Excel.XlHAlign.xlHAlignLeft, Format = (string)null },
            new { Header = "Số Lượng", PropertyName = "SLBan", Width = 8, Align = Excel.XlHAlign.xlHAlignRight, Format = "#,##0" },
            new { Header = "Đơn Giá", PropertyName = "DonGiaBanKhiBan", Width = 15, Align = Excel.XlHAlign.xlHAlignRight, Format = "#,##0" },
            new { Header = "KM (%)", PropertyName = "KhuyenMai", Width = 8, Align = Excel.XlHAlign.xlHAlignRight, Format = "0%" },
            new { Header = "Thành Tiền", PropertyName = "ThanhTien", Width = 18, Align = Excel.XlHAlign.xlHAlignRight, Format = "#,##0" }
        };
                int numberOfExportColumns = exportColumns.Length;

                // 1. Thông tin Cửa Hàng
                worksheet.Cells[currentRow, 1].Value = "Cửa Hàng Minh Châu";
                // ... (Thêm các dòng thông tin cửa hàng, tiêu đề, số HĐ, khách hàng như code trước) ...
                // Ví dụ vắn tắt:
                currentRow = 1; // Reset lại cho dễ hình dung
                string tenCuaHang = "Cửa Hàng Minh Châu";
                string diaChiCH = "Phượng Lích 1, Diễn Hoá, Diễn Châu, Nghệ An";
                string dienThoaiCH = "0335549158";

                worksheet.Cells[currentRow, 1].Value = tenCuaHang;
                worksheet.Cells[currentRow, 1].Font.Bold = true;
                worksheet.Cells[currentRow, 1].Font.Size = 12;
                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfExportColumns]];
                currentRange.Merge();
                currentRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                currentRow++;
                worksheet.Cells[currentRow, 1].Value = "Địa chỉ: " + diaChiCH; currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfExportColumns]]; currentRange.Merge(); currentRow++;
                worksheet.Cells[currentRow, 1].Value = "Điện thoại: " + dienThoaiCH; currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfExportColumns]]; currentRange.Merge(); currentRow++;
                currentRow++;

                string reportTitle = "HÓA ĐƠN BÁN HÀNG";
                worksheet.Cells[currentRow, 1].Value = reportTitle;
                worksheet.Cells[currentRow, 1].Font.Bold = true;
                worksheet.Cells[currentRow, 1].Font.Size = 16;
                currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfExportColumns]]; currentRange.Merge(); currentRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; currentRow++;
                currentRow++;

                worksheet.Cells[currentRow, 1].Value = $"Số HĐ: {txtMaHoaDon.Text.Trim()}"; worksheet.Cells[currentRow, 1].Font.Bold = true; currentRow++;
                worksheet.Cells[currentRow, 1].Value = $"Khách hàng: {cmbMaKhachHang.Text} (Mã KH: {cmbMaKhachHang.SelectedValue?.ToString() ?? "N/A"})"; currentRange = worksheet.Range[worksheet.Cells[currentRow, 1], worksheet.Cells[currentRow, numberOfExportColumns]]; currentRange.Merge(); currentRow++;
                currentRow++;

                int excelColIndex = 1;
                foreach (var colInfo in exportColumns) { Excel.Range cell = worksheet.Cells[currentRow, excelColIndex]; cell.Value = colInfo.Header; cell.Font.Bold = true; cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter; cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray); cell.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; excelColIndex++; }
                currentRow++;

                foreach (ChiTietHDBan detailItem in currentChiTietBanList) { excelColIndex = 1; foreach (var colInfo in exportColumns) { object value = typeof(ChiTietHDBan).GetProperty(colInfo.PropertyName)?.GetValue(detailItem, null); Excel.Range currentCell = worksheet.Cells[currentRow, excelColIndex]; currentCell.Value = value; currentCell.HorizontalAlignment = colInfo.Align; if (!string.IsNullOrEmpty(colInfo.Format)) { currentCell.NumberFormat = colInfo.Format; } currentCell.Borders.LineStyle = Excel.XlLineStyle.xlContinuous; excelColIndex++; } currentRow++; }
                // currentRow++; // Bỏ dòng trống này nếu muốn tổng tiền ngay dưới

                int tongCongLabelCol = Math.Max(1, numberOfExportColumns - 1); int tongCongValueCol = numberOfExportColumns;
                worksheet.Cells[currentRow, tongCongLabelCol].Value = "Tổng cộng tiền hàng:"; worksheet.Cells[currentRow, tongCongLabelCol].Font.Bold = true; worksheet.Cells[currentRow, tongCongLabelCol].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                float tongTienHoaDonVal = 0; string tongTienTextVal = txtTongTienHDB.Text; System.Globalization.CultureInfo cultureVal = System.Globalization.CultureInfo.CurrentCulture; if (float.TryParse(tongTienTextVal, System.Globalization.NumberStyles.Any, cultureVal, out tongTienHoaDonVal)) { worksheet.Cells[currentRow, tongCongValueCol].Value = tongTienHoaDonVal; } else { worksheet.Cells[currentRow, tongCongValueCol].Value = tongTienTextVal; }
                worksheet.Cells[currentRow, tongCongValueCol].NumberFormat = "#,##0"; worksheet.Cells[currentRow, tongCongValueCol].Font.Bold = true; worksheet.Cells[currentRow, tongCongValueCol].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight; currentRow++;
                currentRow++;

                worksheet.Cells[currentRow, 1].Value = $"Ngày bán: {dtpNgayBan.Value:dd/MM/yyyy}"; worksheet.Cells[currentRow, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft; currentRow++;
                worksheet.Cells[currentRow, 1].Value = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}"; worksheet.Cells[currentRow, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft; currentRow++;

                for (int i = 0; i < exportColumns.Length; i++) { Excel.Range column = (Excel.Range)worksheet.Columns[i + 1]; column.ColumnWidth = exportColumns[i].Width; }
                worksheet.PageSetup.Orientation = Excel.XlPageOrientation.xlPortrait; worksheet.PageSetup.Zoom = false; worksheet.PageSetup.FitToPagesWide = 1; worksheet.PageSetup.FitToPagesTall = 1;
                worksheet.PageSetup.LeftMargin = excelApp.InchesToPoints(0.5); worksheet.PageSetup.RightMargin = excelApp.InchesToPoints(0.5); worksheet.PageSetup.TopMargin = excelApp.InchesToPoints(0.5); worksheet.PageSetup.BottomMargin = excelApp.InchesToPoints(0.5);
                // --- (Kết thúc phần code điền dữ liệu ví dụ) ---

                // --- END: Dựng nội dung Excel ---

                // Tạo đường dẫn file PDF tạm
                tempPdfPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".pdf");

                // Xuất ra file PDF tạm thời, không tự động mở
                workbook.ExportAsFixedFormat(
                    Excel.XlFixedFormatType.xlTypePDF,
                    tempPdfPath,
                    Excel.XlFixedFormatQuality.xlQualityStandard,
                    true, // IncludeDocProperties
                    true, // IgnorePrintAreas (quan trọng nếu bạn có đặt vùng in)
                    Type.Missing, // From
                    Type.Missing, // To
                    false,        // OpenAfterPublish -> Đặt là false
                    Type.Missing  // FixedFormatExtClassPtr
                );

                // Đóng workbook và Excel sớm hơn để giải phóng file cho việc xem trước (nếu có thể)
                // Việc này sẽ được thực hiện trong finally, nhưng nếu muốn thử đóng sớm:
                // if (workbook != null) { workbook.Close(false); Marshal.ReleaseComObject(workbook); workbook = null; }
                // if (excelApp != null) { excelApp.Quit(); Marshal.ReleaseComObject(excelApp); excelApp = null; }
                // Tuy nhiên, an toàn nhất là để finally xử lý.

                // --- Bước Xem Trước ---
                DialogResult userAction = DialogResult.Cancel; // Hành động mặc định
                Process pdfProcess = null;
                try
                {
                    pdfProcess = Process.Start(tempPdfPath); // Mở file PDF tạm để xem

                    // Đợi một chút để người dùng có thời gian xem, hoặc hiển thị MessageBox trong khi PDF đang mở
                    // MessageBox này sẽ chặn luồng, người dùng xem PDF rồi quay lại đây để quyết định
                    userAction = MessageBox.Show(
                        "Hóa đơn đã được mở để xem trước.\n\nBạn có muốn LƯU hóa đơn này không?",
                        "Xác Nhận Lưu Hóa Đơn",
                        MessageBoxButtons.YesNoCancel, // Thêm nút Cancel
                        MessageBoxIcon.Question
                    );
                }
                catch (Exception exPreview)
                {
                    MessageBox.Show("Không thể mở bản xem trước PDF: " + exPreview.Message +
                                    "\n\nTuy nhiên, bạn vẫn có thể chọn để lưu file.",
                                    "Lỗi Xem Trước", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // Cho phép người dùng vẫn lưu dù không xem trước được
                    userAction = MessageBox.Show(
                        "Không thể mở xem trước. Bạn có muốn tiếp tục lưu hóa đơn này không?",
                        "Xác Nhận Lưu",
                        MessageBoxButtons.YesNo, // Không có Cancel ở đây nếu preview lỗi
                        MessageBoxIcon.Question);
                }

                if (userAction == DialogResult.Yes)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "PDF files (*.pdf)|*.pdf",
                        Title = "Chọn nơi lưu file PDF Hóa Đơn",
                        FileName = $"HoaDon_{safeSoHDBan}.pdf"
                    };

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            // Cố gắng đóng trình xem PDF trước khi sao chép, nếu không có thể bị lỗi file locked
                            if (pdfProcess != null && !pdfProcess.HasExited)
                            {
                                // Thử đóng nhẹ nhàng trước
                                pdfProcess.CloseMainWindow();
                                pdfProcess.WaitForExit(1000); // Chờ 1 giây
                                if (!pdfProcess.HasExited)
                                {
                                    // Nếu vẫn chưa đóng, thử kill (cẩn thận)
                                    // pdfProcess.Kill();
                                    // pdfProcess.WaitForExit(500);
                                    // Tốt hơn là thông báo người dùng tự đóng nếu cần
                                    MessageBox.Show("Vui lòng đóng cửa sổ xem trước PDF để hoàn tất việc lưu file.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            File.Copy(tempPdfPath, saveFileDialog.FileName, true); // Sao chép file tạm đến vị trí người dùng chọn, true để ghi đè nếu tồn tại
                            MessageBox.Show("Xuất Hóa Đơn PDF thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            try
                            {
                                Process.Start(saveFileDialog.FileName); // Mở file đã lưu cuối cùng
                            }
                            catch (Exception exOpenFinal)
                            {
                                MessageBox.Show("Không thể mở file PDF đã lưu tự động: " + exOpenFinal.Message, "Lỗi Mở File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        catch (IOException ioEx) // Bắt lỗi cụ thể khi copy file (thường do file lock)
                        {
                            MessageBox.Show($"Lỗi khi lưu file PDF: {ioEx.Message}\nCó thể file xem trước đang được mở và khóa file. Vui lòng đóng trình xem PDF và thử lại.", "Lỗi Lưu File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (Exception exSaveFinal)
                        {
                            MessageBox.Show("Lỗi khi lưu file PDF cuối cùng: " + exSaveFinal.Message, "Lỗi Lưu File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else // Người dùng nhấn Cancel trên SaveFileDialog
                    {
                        MessageBox.Show("Thao tác lưu hóa đơn đã được hủy.", "Đã hủy", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else // Người dùng nhấn No hoặc Cancel trên MessageBox xác nhận sau khi xem trước
                {
                    MessageBox.Show("Thao tác xuất hóa đơn đã được hủy.", "Đã hủy", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception exMain)
            {
                MessageBox.Show("Lỗi nghiêm trọng khi tạo hóa đơn PDF: " + exMain.ToString(), "Lỗi Xuất File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Luôn giải phóng đối tượng COM và xóa file tạm
                try
                {
                    if (worksheet != null) Marshal.ReleaseComObject(worksheet);
                    if (workbook != null) { workbook.Close(false); Marshal.ReleaseComObject(workbook); }
                    if (excelApp != null) { excelApp.Quit(); Marshal.ReleaseComObject(excelApp); }
                }
                catch (Exception exRelease) { Console.WriteLine("Lỗi khi giải phóng đối tượng COM: " + exRelease.Message); }
                worksheet = null; workbook = null; excelApp = null;

                if (!string.IsNullOrEmpty(tempPdfPath) && File.Exists(tempPdfPath))
                {
                    try
                    {
                        File.Delete(tempPdfPath);
                    }
                    catch (IOException ioExDel)
                    {
                        Console.WriteLine($"Lỗi khi xóa file PDF tạm ({tempPdfPath}): {ioExDel.Message}. File có thể vẫn đang bị khóa.");
                        // Có thể thông báo cho người dùng hoặc ghi log ở đây nếu cần
                    }
                    catch (Exception exDeleteTemp)
                    {
                        Console.WriteLine("Lỗi không xác định khi xóa file PDF tạm: " + exDeleteTemp.Message);
                    }
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();

            }
        }
        private void ReleaseComObject(object obj)
        {
            try
            {
                if (obj != null && Marshal.IsComObject(obj))
                {
                    Marshal.ReleaseComObject(obj);
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
    }
 }

    


