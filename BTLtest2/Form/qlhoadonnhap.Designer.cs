namespace BTLtest2
{
    partial class qlhoadonnhap
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbMaNhanVien = new System.Windows.Forms.ComboBox();
            this.btnThemSanPham = new System.Windows.Forms.Button();
            this.txtTongTien = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtThanhTien = new System.Windows.Forms.TextBox();
            this.dgvChiTietHoaDon = new System.Windows.Forms.DataGridView();
            this.txtDonGiaNhap = new System.Windows.Forms.TextBox();
            this.btnDong = new System.Windows.Forms.Button();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.btnIn = new System.Windows.Forms.Button();
            this.btnSuaHoaDon = new System.Windows.Forms.Button();
            this.btnLuuHoaDon = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.txtMaHoaDon = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnThemHoaDon = new System.Windows.Forms.Button();
            this.cmbMaNhaCungCap = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpNgayNhap = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.btnTimhoadon = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSuaSanPham = new System.Windows.Forms.Button();
            this.cmbMaHang = new System.Windows.Forms.ComboBox();
            this.btnXoaHoaDon = new System.Windows.Forms.Button();
            this.dgvHoaDonNhap = new System.Windows.Forms.DataGridView();
            this.btnXoaSanPham = new System.Windows.Forms.Button();
            this.txtMaHoaDonTimKiem = new System.Windows.Forms.TextBox();
            this.bnt_huy = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTietHoaDon)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoaDonNhap)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbMaNhanVien
            // 
            this.cmbMaNhanVien.FormattingEnabled = true;
            this.cmbMaNhanVien.Location = new System.Drawing.Point(380, 50);
            this.cmbMaNhanVien.Margin = new System.Windows.Forms.Padding(2);
            this.cmbMaNhanVien.Name = "cmbMaNhanVien";
            this.cmbMaNhanVien.Size = new System.Drawing.Size(130, 21);
            this.cmbMaNhanVien.TabIndex = 24;
            // 
            // btnThemSanPham
            // 
            this.btnThemSanPham.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnThemSanPham.FlatAppearance.BorderSize = 0;
            this.btnThemSanPham.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemSanPham.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnThemSanPham.Location = new System.Drawing.Point(4, 276);
            this.btnThemSanPham.Margin = new System.Windows.Forms.Padding(2);
            this.btnThemSanPham.Name = "btnThemSanPham";
            this.btnThemSanPham.Size = new System.Drawing.Size(107, 24);
            this.btnThemSanPham.TabIndex = 45;
            this.btnThemSanPham.Text = "Thêm sản phẩm";
            this.btnThemSanPham.UseVisualStyleBackColor = false;
            this.btnThemSanPham.Click += new System.EventHandler(this.btnThemSanPham_Click);
            // 
            // txtTongTien
            // 
            this.txtTongTien.Location = new System.Drawing.Point(429, 241);
            this.txtTongTien.Margin = new System.Windows.Forms.Padding(2);
            this.txtTongTien.Name = "txtTongTien";
            this.txtTongTien.Size = new System.Drawing.Size(156, 20);
            this.txtTongTien.TabIndex = 42;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(287, 20);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(91, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Mã nhà cung cấp";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(365, 244);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(55, 13);
            this.label18.TabIndex = 41;
            this.label18.Text = "Tổng tiền:";
            // 
            // txtThanhTien
            // 
            this.txtThanhTien.Location = new System.Drawing.Point(407, 51);
            this.txtThanhTien.Margin = new System.Windows.Forms.Padding(2);
            this.txtThanhTien.Name = "txtThanhTien";
            this.txtThanhTien.Size = new System.Drawing.Size(99, 20);
            this.txtThanhTien.TabIndex = 38;
            // 
            // dgvChiTietHoaDon
            // 
            this.dgvChiTietHoaDon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChiTietHoaDon.Location = new System.Drawing.Point(21, 78);
            this.dgvChiTietHoaDon.Margin = new System.Windows.Forms.Padding(2);
            this.dgvChiTietHoaDon.Name = "dgvChiTietHoaDon";
            this.dgvChiTietHoaDon.RowHeadersWidth = 51;
            this.dgvChiTietHoaDon.Size = new System.Drawing.Size(562, 148);
            this.dgvChiTietHoaDon.TabIndex = 27;
            this.dgvChiTietHoaDon.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChiTietHoaDon_CellClick);
            // 
            // txtDonGiaNhap
            // 
            this.txtDonGiaNhap.Location = new System.Drawing.Point(98, 51);
            this.txtDonGiaNhap.Margin = new System.Windows.Forms.Padding(2);
            this.txtDonGiaNhap.Name = "txtDonGiaNhap";
            this.txtDonGiaNhap.Size = new System.Drawing.Size(99, 20);
            this.txtDonGiaNhap.TabIndex = 37;
            // 
            // btnDong
            // 
            this.btnDong.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnDong.FlatAppearance.BorderSize = 0;
            this.btnDong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDong.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDong.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDong.Location = new System.Drawing.Point(543, 275);
            this.btnDong.Margin = new System.Windows.Forms.Padding(2);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(56, 24);
            this.btnDong.TabIndex = 26;
            this.btnDong.Text = "Đóng";
            this.btnDong.UseVisualStyleBackColor = false;
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.Location = new System.Drawing.Point(404, 25);
            this.txtSoLuong.Margin = new System.Windows.Forms.Padding(2);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(104, 20);
            this.txtSoLuong.TabIndex = 36;
            // 
            // btnIn
            // 
            this.btnIn.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnIn.FlatAppearance.BorderSize = 0;
            this.btnIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIn.Location = new System.Drawing.Point(463, 275);
            this.btnIn.Margin = new System.Windows.Forms.Padding(2);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(76, 24);
            this.btnIn.TabIndex = 25;
            this.btnIn.Text = "In hóa đơn";
            this.btnIn.UseVisualStyleBackColor = false;
            this.btnIn.Click += new System.EventHandler(this.btnIn_Click);
            // 
            // btnSuaHoaDon
            // 
            this.btnSuaHoaDon.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnSuaHoaDon.FlatAppearance.BorderSize = 0;
            this.btnSuaHoaDon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSuaHoaDon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSuaHoaDon.Location = new System.Drawing.Point(367, 275);
            this.btnSuaHoaDon.Margin = new System.Windows.Forms.Padding(2);
            this.btnSuaHoaDon.Name = "btnSuaHoaDon";
            this.btnSuaHoaDon.Size = new System.Drawing.Size(86, 24);
            this.btnSuaHoaDon.TabIndex = 24;
            this.btnSuaHoaDon.Text = "Sửa hóa đơn";
            this.btnSuaHoaDon.UseVisualStyleBackColor = false;
            this.btnSuaHoaDon.Click += new System.EventHandler(this.btnSuaHoaDon_Click);
            // 
            // btnLuuHoaDon
            // 
            this.btnLuuHoaDon.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnLuuHoaDon.FlatAppearance.BorderSize = 0;
            this.btnLuuHoaDon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuuHoaDon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLuuHoaDon.Location = new System.Drawing.Point(215, 276);
            this.btnLuuHoaDon.Margin = new System.Windows.Forms.Padding(2);
            this.btnLuuHoaDon.Name = "btnLuuHoaDon";
            this.btnLuuHoaDon.Size = new System.Drawing.Size(46, 24);
            this.btnLuuHoaDon.TabIndex = 23;
            this.btnLuuHoaDon.Text = "Lưu";
            this.btnLuuHoaDon.UseVisualStyleBackColor = false;
            this.btnLuuHoaDon.Click += new System.EventHandler(this.btnLuuHoaDon_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(337, 54);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(58, 13);
            this.label14.TabIndex = 32;
            this.label14.Text = "Thành tiền";
            // 
            // txtMaHoaDon
            // 
            this.txtMaHoaDon.Location = new System.Drawing.Point(128, 18);
            this.txtMaHoaDon.Margin = new System.Windows.Forms.Padding(2);
            this.txtMaHoaDon.Name = "txtMaHoaDon";
            this.txtMaHoaDon.Size = new System.Drawing.Size(130, 20);
            this.txtMaHoaDon.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(298, 52);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Mã nhân viên";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 45);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Ngày nhập";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(230, 7);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(234, 24);
            this.label2.TabIndex = 54;
            this.label2.Text = "HÓA ĐƠN NHẬP HÀNG";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnThemHoaDon
            // 
            this.btnThemHoaDon.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnThemHoaDon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnThemHoaDon.FlatAppearance.BorderSize = 0;
            this.btnThemHoaDon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemHoaDon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnThemHoaDon.Location = new System.Drawing.Point(115, 276);
            this.btnThemHoaDon.Margin = new System.Windows.Forms.Padding(2);
            this.btnThemHoaDon.Name = "btnThemHoaDon";
            this.btnThemHoaDon.Size = new System.Drawing.Size(95, 24);
            this.btnThemHoaDon.TabIndex = 22;
            this.btnThemHoaDon.Text = "Thêm hóa đơn";
            this.btnThemHoaDon.UseVisualStyleBackColor = false;
            this.btnThemHoaDon.Click += new System.EventHandler(this.btnThemHoaDon_Click);
            // 
            // cmbMaNhaCungCap
            // 
            this.cmbMaNhaCungCap.FormattingEnabled = true;
            this.cmbMaNhaCungCap.Location = new System.Drawing.Point(382, 18);
            this.cmbMaNhaCungCap.Margin = new System.Windows.Forms.Padding(2);
            this.cmbMaNhaCungCap.Name = "cmbMaNhaCungCap";
            this.cmbMaNhaCungCap.Size = new System.Drawing.Size(158, 21);
            this.cmbMaNhaCungCap.TabIndex = 34;
            this.cmbMaNhaCungCap.SelectedIndexChanged += new System.EventHandler(this.cmbMaNhaCungCap_SelectedIndexChanged_1);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpNgayNhap);
            this.groupBox1.Controls.Add(this.cmbMaNhaCungCap);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cmbMaNhanVien);
            this.groupBox1.Controls.Add(this.txtMaHoaDon);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(24, 28);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(569, 125);
            this.groupBox1.TabIndex = 55;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thông tin chung";
            // 
            // dtpNgayNhap
            // 
            this.dtpNgayNhap.Location = new System.Drawing.Point(128, 45);
            this.dtpNgayNhap.Name = "dtpNgayNhap";
            this.dtpNgayNhap.Size = new System.Drawing.Size(130, 20);
            this.dtpNgayNhap.TabIndex = 36;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Mã hóa đơn";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(28, 54);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(71, 13);
            this.label15.TabIndex = 31;
            this.label15.Text = "Đơn giá nhập";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(31, 28);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "Mã hàng";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(337, 32);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(49, 13);
            this.label11.TabIndex = 28;
            this.label11.Text = "Số lượng";
            // 
            // btnTimhoadon
            // 
            this.btnTimhoadon.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnTimhoadon.FlatAppearance.BorderSize = 0;
            this.btnTimhoadon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimhoadon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTimhoadon.Location = new System.Drawing.Point(258, 625);
            this.btnTimhoadon.Margin = new System.Windows.Forms.Padding(2);
            this.btnTimhoadon.Name = "btnTimhoadon";
            this.btnTimhoadon.Size = new System.Drawing.Size(57, 24);
            this.btnTimhoadon.TabIndex = 59;
            this.btnTimhoadon.Text = "Tìm";
            this.btnTimhoadon.UseVisualStyleBackColor = false;
            this.btnTimhoadon.Click += new System.EventHandler(this.btnTimhoadon_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(42, 633);
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(68, 13);
            this.label19.TabIndex = 58;
            this.label19.Text = "Mã hóa đơn:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSuaSanPham);
            this.groupBox2.Controls.Add(this.cmbMaHang);
            this.groupBox2.Controls.Add(this.btnThemSanPham);
            this.groupBox2.Controls.Add(this.txtTongTien);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.txtThanhTien);
            this.groupBox2.Controls.Add(this.dgvChiTietHoaDon);
            this.groupBox2.Controls.Add(this.txtDonGiaNhap);
            this.groupBox2.Controls.Add(this.btnDong);
            this.groupBox2.Controls.Add(this.txtSoLuong);
            this.groupBox2.Controls.Add(this.btnIn);
            this.groupBox2.Controls.Add(this.btnSuaHoaDon);
            this.groupBox2.Controls.Add(this.btnLuuHoaDon);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.btnThemHoaDon);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Location = new System.Drawing.Point(11, 318);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(603, 304);
            this.groupBox2.TabIndex = 56;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Thông tin các mặt hàng";
            // 
            // btnSuaSanPham
            // 
            this.btnSuaSanPham.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnSuaSanPham.FlatAppearance.BorderSize = 0;
            this.btnSuaSanPham.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSuaSanPham.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSuaSanPham.Location = new System.Drawing.Point(265, 275);
            this.btnSuaSanPham.Margin = new System.Windows.Forms.Padding(2);
            this.btnSuaSanPham.Name = "btnSuaSanPham";
            this.btnSuaSanPham.Size = new System.Drawing.Size(87, 24);
            this.btnSuaSanPham.TabIndex = 47;
            this.btnSuaSanPham.Text = "Sửa sản phẩm";
            this.btnSuaSanPham.UseVisualStyleBackColor = false;
            this.btnSuaSanPham.Click += new System.EventHandler(this.btnSuaSanPham_Click);
            // 
            // cmbMaHang
            // 
            this.cmbMaHang.FormattingEnabled = true;
            this.cmbMaHang.Location = new System.Drawing.Point(85, 24);
            this.cmbMaHang.Name = "cmbMaHang";
            this.cmbMaHang.Size = new System.Drawing.Size(121, 21);
            this.cmbMaHang.TabIndex = 46;
            // 
            // btnXoaHoaDon
            // 
            this.btnXoaHoaDon.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnXoaHoaDon.FlatAppearance.BorderSize = 0;
            this.btnXoaHoaDon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoaHoaDon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnXoaHoaDon.Location = new System.Drawing.Point(418, 626);
            this.btnXoaHoaDon.Margin = new System.Windows.Forms.Padding(2);
            this.btnXoaHoaDon.Name = "btnXoaHoaDon";
            this.btnXoaHoaDon.Size = new System.Drawing.Size(86, 24);
            this.btnXoaHoaDon.TabIndex = 60;
            this.btnXoaHoaDon.Text = "Xóa hóa đơn";
            this.btnXoaHoaDon.UseVisualStyleBackColor = false;
            this.btnXoaHoaDon.Click += new System.EventHandler(this.btnXoaHoaDon_Click);
            // 
            // dgvHoaDonNhap
            // 
            this.dgvHoaDonNhap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHoaDonNhap.Location = new System.Drawing.Point(31, 157);
            this.dgvHoaDonNhap.Margin = new System.Windows.Forms.Padding(2);
            this.dgvHoaDonNhap.Name = "dgvHoaDonNhap";
            this.dgvHoaDonNhap.RowHeadersWidth = 51;
            this.dgvHoaDonNhap.Size = new System.Drawing.Size(562, 148);
            this.dgvHoaDonNhap.TabIndex = 46;
            this.dgvHoaDonNhap.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHoaDonNhap_CellClick);
            // 
            // btnXoaSanPham
            // 
            this.btnXoaSanPham.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnXoaSanPham.FlatAppearance.BorderSize = 0;
            this.btnXoaSanPham.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoaSanPham.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnXoaSanPham.Location = new System.Drawing.Point(325, 626);
            this.btnXoaSanPham.Margin = new System.Windows.Forms.Padding(2);
            this.btnXoaSanPham.Name = "btnXoaSanPham";
            this.btnXoaSanPham.Size = new System.Drawing.Size(86, 24);
            this.btnXoaSanPham.TabIndex = 61;
            this.btnXoaSanPham.Text = "Xóa sản phẩm";
            this.btnXoaSanPham.UseVisualStyleBackColor = false;
            this.btnXoaSanPham.Click += new System.EventHandler(this.btnXoaSanPham_Click);
            // 
            // txtMaHoaDonTimKiem
            // 
            this.txtMaHoaDonTimKiem.Location = new System.Drawing.Point(114, 629);
            this.txtMaHoaDonTimKiem.Margin = new System.Windows.Forms.Padding(2);
            this.txtMaHoaDonTimKiem.Name = "txtMaHoaDonTimKiem";
            this.txtMaHoaDonTimKiem.Size = new System.Drawing.Size(130, 20);
            this.txtMaHoaDonTimKiem.TabIndex = 62;
            // 
            // bnt_huy
            // 
            this.bnt_huy.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bnt_huy.FlatAppearance.BorderSize = 0;
            this.bnt_huy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnt_huy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnt_huy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bnt_huy.Location = new System.Drawing.Point(508, 626);
            this.bnt_huy.Margin = new System.Windows.Forms.Padding(2);
            this.bnt_huy.Name = "bnt_huy";
            this.bnt_huy.Size = new System.Drawing.Size(66, 24);
            this.bnt_huy.TabIndex = 48;
            this.bnt_huy.Text = "Huỷ";
            this.bnt_huy.UseVisualStyleBackColor = false;
            this.bnt_huy.Click += new System.EventHandler(this.bnt_huy_Click);
            // 
            // qlhoadonnhap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Pink;
            this.ClientSize = new System.Drawing.Size(617, 695);
            this.Controls.Add(this.bnt_huy);
            this.Controls.Add(this.txtMaHoaDonTimKiem);
            this.Controls.Add(this.btnXoaSanPham);
            this.Controls.Add(this.dgvHoaDonNhap);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnTimhoadon);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnXoaHoaDon);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "qlhoadonnhap";
            this.Text = "qlhoadonnhap";
            this.Load += new System.EventHandler(this.qlhoadonnhap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTietHoaDon)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoaDonNhap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbMaNhanVien;
        private System.Windows.Forms.Button btnThemSanPham;
        private System.Windows.Forms.TextBox txtTongTien;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtThanhTien;
        private System.Windows.Forms.DataGridView dgvChiTietHoaDon;
        private System.Windows.Forms.TextBox txtDonGiaNhap;
        private System.Windows.Forms.Button btnDong;
        private System.Windows.Forms.TextBox txtSoLuong;
        private System.Windows.Forms.Button btnIn;
        private System.Windows.Forms.Button btnSuaHoaDon;
        private System.Windows.Forms.Button btnLuuHoaDon;
        private System.Windows.Forms.Label label14;
        public System.Windows.Forms.TextBox txtMaHoaDon;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnThemHoaDon;
        private System.Windows.Forms.ComboBox cmbMaNhaCungCap;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnTimhoadon;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnXoaHoaDon;
        private System.Windows.Forms.DataGridView dgvHoaDonNhap;
        private System.Windows.Forms.ComboBox cmbMaHang;
        private System.Windows.Forms.Button btnSuaSanPham;
        private System.Windows.Forms.Button btnXoaSanPham;
        private System.Windows.Forms.DateTimePicker dtpNgayNhap;
        private System.Windows.Forms.TextBox txtMaHoaDonTimKiem;
        private System.Windows.Forms.Button bnt_huy;
    }
}