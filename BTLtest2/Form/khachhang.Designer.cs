namespace BTLtest2
{
    partial class khachhang
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
            this.bntdong = new System.Windows.Forms.Button();
            this.bnthienthi = new System.Windows.Forms.Button();
            this.bnttaoexel = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txtTongHoaDon = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSoLanMua = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // bntdong
            // 
            this.bntdong.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bntdong.FlatAppearance.BorderSize = 0;
            this.bntdong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntdong.Location = new System.Drawing.Point(421, 344);
            this.bntdong.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bntdong.Name = "bntdong";
            this.bntdong.Size = new System.Drawing.Size(95, 33);
            this.bntdong.TabIndex = 36;
            this.bntdong.Text = "Đóng";
            this.bntdong.UseVisualStyleBackColor = false;
            this.bntdong.Click += new System.EventHandler(this.bntdong_Click);
            // 
            // bnthienthi
            // 
            this.bnthienthi.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bnthienthi.FlatAppearance.BorderSize = 0;
            this.bnthienthi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnthienthi.Location = new System.Drawing.Point(251, 344);
            this.bnthienthi.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bnthienthi.Name = "bnthienthi";
            this.bnthienthi.Size = new System.Drawing.Size(90, 33);
            this.bnthienthi.TabIndex = 35;
            this.bnthienthi.Text = "Hiển thị";
            this.bnthienthi.UseVisualStyleBackColor = false;
            this.bnthienthi.Click += new System.EventHandler(this.bnthienthi_Click);
            // 
            // bnttaoexel
            // 
            this.bnttaoexel.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bnttaoexel.FlatAppearance.BorderSize = 0;
            this.bnttaoexel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnttaoexel.Location = new System.Drawing.Point(71, 344);
            this.bnttaoexel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bnttaoexel.Name = "bnttaoexel";
            this.bnttaoexel.Size = new System.Drawing.Size(95, 33);
            this.bnttaoexel.TabIndex = 33;
            this.bnttaoexel.Text = "Xuất file";
            this.bnttaoexel.UseVisualStyleBackColor = false;
            this.bnttaoexel.Click += new System.EventHandler(this.bnttaoexel_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(50, 216);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(535, 98);
            this.dataGridView1.TabIndex = 32;
            // 
            // txtTongHoaDon
            // 
            this.txtTongHoaDon.Location = new System.Drawing.Point(216, 182);
            this.txtTongHoaDon.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtTongHoaDon.Name = "txtTongHoaDon";
            this.txtTongHoaDon.Size = new System.Drawing.Size(125, 20);
            this.txtTongHoaDon.TabIndex = 31;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(46, 177);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 24);
            this.label4.TabIndex = 30;
            this.label4.Text = "Tổng hoá đơn";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(357, 99);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 24);
            this.label3.TabIndex = 28;
            this.label3.Text = "đến";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(46, 99);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 24);
            this.label2.TabIndex = 27;
            this.label2.Text = "Khoảng thời gian";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(212, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 24);
            this.label1.TabIndex = 25;
            this.label1.Text = "Khách hàng thân thiết";
            // 
            // txtSoLanMua
            // 
            this.txtSoLanMua.Location = new System.Drawing.Point(216, 142);
            this.txtSoLanMua.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtSoLanMua.Name = "txtSoLanMua";
            this.txtSoLanMua.Size = new System.Drawing.Size(125, 20);
            this.txtSoLanMua.TabIndex = 38;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(46, 138);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 24);
            this.label5.TabIndex = 37;
            this.label5.Text = "Số lần mua";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Location = new System.Drawing.Point(201, 101);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(140, 20);
            this.dtpStartDate.TabIndex = 39;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Location = new System.Drawing.Point(406, 103);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(140, 20);
            this.dtpEndDate.TabIndex = 40;
            // 
            // khachhang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Pink;
            this.ClientSize = new System.Drawing.Size(637, 422);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.txtSoLanMua);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bntdong);
            this.Controls.Add(this.bnthienthi);
            this.Controls.Add(this.bnttaoexel);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txtTongHoaDon);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "khachhang";
            this.Text = "khachhang";
            this.Load += new System.EventHandler(this.khachhang_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bntdong;
        private System.Windows.Forms.Button bnthienthi;
        private System.Windows.Forms.Button bnttaoexel;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtTongHoaDon;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSoLanMua;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
    }
}