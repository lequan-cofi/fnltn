namespace BTLtest2
{
    partial class thanhtoan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(thanhtoan));
            this.picChuyenKhoan = new System.Windows.Forms.PictureBox();
            this.radChuyenKhoan = new System.Windows.Forms.RadioButton();
            this.radTienMat = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTongTien = new System.Windows.Forms.TextBox();
            this.cmbMaHoaDon = new System.Windows.Forms.ComboBox();
            this.dgvHoaDon = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnXacNhanThanhToan = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.dgvChiTietHoaDon = new System.Windows.Forms.DataGridView();
            this.btnThanhToanThanhCong = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picChuyenKhoan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoaDon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTietHoaDon)).BeginInit();
            this.SuspendLayout();
            // 
            // picChuyenKhoan
            // 
            this.picChuyenKhoan.Image = ((System.Drawing.Image)(resources.GetObject("picChuyenKhoan.Image")));
            this.picChuyenKhoan.InitialImage = null;
            this.picChuyenKhoan.Location = new System.Drawing.Point(574, 100);
            this.picChuyenKhoan.Name = "picChuyenKhoan";
            this.picChuyenKhoan.Size = new System.Drawing.Size(261, 349);
            this.picChuyenKhoan.TabIndex = 24;
            this.picChuyenKhoan.TabStop = false;
            // 
            // radChuyenKhoan
            // 
            this.radChuyenKhoan.AutoSize = true;
            this.radChuyenKhoan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radChuyenKhoan.Location = new System.Drawing.Point(26, 418);
            this.radChuyenKhoan.Name = "radChuyenKhoan";
            this.radChuyenKhoan.Size = new System.Drawing.Size(115, 19);
            this.radChuyenKhoan.TabIndex = 23;
            this.radChuyenKhoan.TabStop = true;
            this.radChuyenKhoan.Text = "Chuyển khoản";
            this.radChuyenKhoan.UseVisualStyleBackColor = true;
            // 
            // radTienMat
            // 
            this.radTienMat.AutoSize = true;
            this.radTienMat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radTienMat.Location = new System.Drawing.Point(26, 388);
            this.radTienMat.Name = "radTienMat";
            this.radTienMat.Size = new System.Drawing.Size(81, 19);
            this.radTienMat.TabIndex = 22;
            this.radTienMat.TabStop = true;
            this.radTienMat.Text = "Tiền mặt";
            this.radTienMat.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(677, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "VNĐ";
            // 
            // txtTongTien
            // 
            this.txtTongTien.Location = new System.Drawing.Point(478, 50);
            this.txtTongTien.Name = "txtTongTien";
            this.txtTongTien.Size = new System.Drawing.Size(184, 20);
            this.txtTongTien.TabIndex = 20;
            // 
            // cmbMaHoaDon
            // 
            this.cmbMaHoaDon.FormattingEnabled = true;
            this.cmbMaHoaDon.Location = new System.Drawing.Point(130, 50);
            this.cmbMaHoaDon.Name = "cmbMaHoaDon";
            this.cmbMaHoaDon.Size = new System.Drawing.Size(184, 21);
            this.cmbMaHoaDon.TabIndex = 19;
            // 
            // dgvHoaDon
            // 
            this.dgvHoaDon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHoaDon.Location = new System.Drawing.Point(6, 100);
            this.dgvHoaDon.Name = "dgvHoaDon";
            this.dgvHoaDon.RowHeadersWidth = 51;
            this.dgvHoaDon.RowTemplate.Height = 24;
            this.dgvHoaDon.Size = new System.Drawing.Size(744, 246);
            this.dgvHoaDon.TabIndex = 18;
            this.dgvHoaDon.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(379, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Tổng tiền";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 360);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 16);
            this.label4.TabIndex = 16;
            this.label4.Text = "Phương thức thanh toán";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Mã hóa đơn";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(250, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 31);
            this.label1.TabIndex = 14;
            this.label1.Text = "THANH TOÁN";
            // 
            // btnXacNhanThanhToan
            // 
            this.btnXacNhanThanhToan.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnXacNhanThanhToan.FlatAppearance.BorderSize = 0;
            this.btnXacNhanThanhToan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXacNhanThanhToan.Location = new System.Drawing.Point(17, 455);
            this.btnXacNhanThanhToan.Name = "btnXacNhanThanhToan";
            this.btnXacNhanThanhToan.Size = new System.Drawing.Size(135, 35);
            this.btnXacNhanThanhToan.TabIndex = 25;
            this.btnXacNhanThanhToan.Text = "Xác nhận thanh toán";
            this.btnXacNhanThanhToan.UseVisualStyleBackColor = false;
            this.btnXacNhanThanhToan.Click += new System.EventHandler(this.bntxacnhan_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(296, 455);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(135, 35);
            this.button1.TabIndex = 26;
            this.button1.Text = "Huý thanh toán";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(437, 455);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(135, 35);
            this.button2.TabIndex = 27;
            this.button2.Text = "Đóng";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dgvChiTietHoaDon
            // 
            this.dgvChiTietHoaDon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChiTietHoaDon.Location = new System.Drawing.Point(6, 100);
            this.dgvChiTietHoaDon.Name = "dgvChiTietHoaDon";
            this.dgvChiTietHoaDon.RowHeadersWidth = 51;
            this.dgvChiTietHoaDon.RowTemplate.Height = 24;
            this.dgvChiTietHoaDon.Size = new System.Drawing.Size(744, 246);
            this.dgvChiTietHoaDon.TabIndex = 28;
            // 
            // btnThanhToanThanhCong
            // 
            this.btnThanhToanThanhCong.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnThanhToanThanhCong.FlatAppearance.BorderSize = 0;
            this.btnThanhToanThanhCong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThanhToanThanhCong.Location = new System.Drawing.Point(158, 455);
            this.btnThanhToanThanhCong.Name = "btnThanhToanThanhCong";
            this.btnThanhToanThanhCong.Size = new System.Drawing.Size(135, 35);
            this.btnThanhToanThanhCong.TabIndex = 29;
            this.btnThanhToanThanhCong.Text = "Thanh toán thành công";
            this.btnThanhToanThanhCong.UseVisualStyleBackColor = false;
            this.btnThanhToanThanhCong.Click += new System.EventHandler(this.btnThanhToanThanhCong_Click);
            // 
            // thanhtoan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Pink;
            this.ClientSize = new System.Drawing.Size(838, 562);
            this.Controls.Add(this.btnThanhToanThanhCong);
            this.Controls.Add(this.picChuyenKhoan);
            this.Controls.Add(this.dgvChiTietHoaDon);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnXacNhanThanhToan);
            this.Controls.Add(this.radChuyenKhoan);
            this.Controls.Add(this.radTienMat);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTongTien);
            this.Controls.Add(this.cmbMaHoaDon);
            this.Controls.Add(this.dgvHoaDon);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "thanhtoan";
            this.Text = "thanhtoan";
            this.Load += new System.EventHandler(this.thanhtoan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picChuyenKhoan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHoaDon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTietHoaDon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picChuyenKhoan;
        private System.Windows.Forms.RadioButton radChuyenKhoan;
        private System.Windows.Forms.RadioButton radTienMat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTongTien;
        private System.Windows.Forms.ComboBox cmbMaHoaDon;
        private System.Windows.Forms.DataGridView dgvHoaDon;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnXacNhanThanhToan;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dgvChiTietHoaDon;
        private System.Windows.Forms.Button btnThanhToanThanhCong;
    }
}