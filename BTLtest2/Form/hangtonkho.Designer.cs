namespace BTLtest2
{
    partial class hangtonkho
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.DoughnutSeriesView doughnutSeriesView1 = new DevExpress.XtraCharts.DoughnutSeriesView();
            this.txtLuongTon = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.bntdong = new System.Windows.Forms.Button();
            this.bnthienthi = new System.Windows.Forms.Button();
            this.bntbieudo = new System.Windows.Forms.Button();
            this.bnttaoexel = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txtLuongBan = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpTuNgay = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpDenNgay = new System.Windows.Forms.DateTimePicker();
            this.chartInventoryReport = new DevExpress.XtraCharts.ChartControl();
            this.sachBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartInventoryReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(doughnutSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sachBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLuongTon
            // 
            this.txtLuongTon.Location = new System.Drawing.Point(456, 113);
            this.txtLuongTon.Margin = new System.Windows.Forms.Padding(2);
            this.txtLuongTon.Name = "txtLuongTon";
            this.txtLuongTon.Size = new System.Drawing.Size(125, 20);
            this.txtLuongTon.TabIndex = 66;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(299, 113);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 24);
            this.label5.TabIndex = 65;
            this.label5.Text = "Lượng tồn";
            // 
            // bntdong
            // 
            this.bntdong.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bntdong.FlatAppearance.BorderSize = 0;
            this.bntdong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntdong.Location = new System.Drawing.Point(413, 309);
            this.bntdong.Margin = new System.Windows.Forms.Padding(2);
            this.bntdong.Name = "bntdong";
            this.bntdong.Size = new System.Drawing.Size(95, 33);
            this.bntdong.TabIndex = 64;
            this.bntdong.Text = "Đóng";
            this.bntdong.UseVisualStyleBackColor = false;
            this.bntdong.Click += new System.EventHandler(this.bntdong_Click);
            // 
            // bnthienthi
            // 
            this.bnthienthi.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bnthienthi.FlatAppearance.BorderSize = 0;
            this.bnthienthi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnthienthi.Location = new System.Drawing.Point(287, 309);
            this.bnthienthi.Margin = new System.Windows.Forms.Padding(2);
            this.bnthienthi.Name = "bnthienthi";
            this.bnthienthi.Size = new System.Drawing.Size(90, 33);
            this.bnthienthi.TabIndex = 63;
            this.bnthienthi.Text = "Hiển thị";
            this.bnthienthi.UseVisualStyleBackColor = false;
            this.bnthienthi.Click += new System.EventHandler(this.bnthienthi_Click);
            // 
            // bntbieudo
            // 
            this.bntbieudo.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bntbieudo.FlatAppearance.BorderSize = 0;
            this.bntbieudo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntbieudo.Location = new System.Drawing.Point(163, 309);
            this.bntbieudo.Margin = new System.Windows.Forms.Padding(2);
            this.bntbieudo.Name = "bntbieudo";
            this.bntbieudo.Size = new System.Drawing.Size(96, 33);
            this.bntbieudo.TabIndex = 62;
            this.bntbieudo.Text = "Biểu đồ";
            this.bntbieudo.UseVisualStyleBackColor = false;
            this.bntbieudo.Click += new System.EventHandler(this.bntbieudo_Click);
            // 
            // bnttaoexel
            // 
            this.bnttaoexel.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bnttaoexel.FlatAppearance.BorderSize = 0;
            this.bnttaoexel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnttaoexel.Location = new System.Drawing.Point(42, 309);
            this.bnttaoexel.Margin = new System.Windows.Forms.Padding(2);
            this.bnttaoexel.Name = "bnttaoexel";
            this.bnttaoexel.Size = new System.Drawing.Size(95, 33);
            this.bnttaoexel.TabIndex = 61;
            this.bnttaoexel.Text = "Xuất file";
            this.bnttaoexel.UseVisualStyleBackColor = false;
            this.bnttaoexel.Click += new System.EventHandler(this.bnttaoexel_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(21, 164);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(535, 98);
            this.dataGridView1.TabIndex = 60;
            // 
            // txtLuongBan
            // 
            this.txtLuongBan.Location = new System.Drawing.Point(159, 113);
            this.txtLuongBan.Margin = new System.Windows.Forms.Padding(2);
            this.txtLuongBan.Name = "txtLuongBan";
            this.txtLuongBan.Size = new System.Drawing.Size(125, 20);
            this.txtLuongBan.TabIndex = 59;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(2, 113);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 24);
            this.label4.TabIndex = 58;
            this.label4.Text = "Lượng bán";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(239, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 24);
            this.label1.TabIndex = 53;
            this.label1.Text = "Hàng tồn kho ";
            // 
            // dtpTuNgay
            // 
            this.dtpTuNgay.Location = new System.Drawing.Point(125, 46);
            this.dtpTuNgay.Margin = new System.Windows.Forms.Padding(2);
            this.dtpTuNgay.Name = "dtpTuNgay";
            this.dtpTuNgay.Size = new System.Drawing.Size(135, 20);
            this.dtpTuNgay.TabIndex = 67;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(24, 42);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 24);
            this.label2.TabIndex = 68;
            this.label2.Text = "Từ ngày";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(277, 46);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 24);
            this.label3.TabIndex = 69;
            this.label3.Text = "Đến ngày";
            // 
            // dtpDenNgay
            // 
            this.dtpDenNgay.Location = new System.Drawing.Point(375, 49);
            this.dtpDenNgay.Margin = new System.Windows.Forms.Padding(2);
            this.dtpDenNgay.Name = "dtpDenNgay";
            this.dtpDenNgay.Size = new System.Drawing.Size(135, 20);
            this.dtpDenNgay.TabIndex = 70;
            // 
            // chartInventoryReport
            // 
            this.chartInventoryReport.Location = new System.Drawing.Point(559, 136);
            this.chartInventoryReport.Name = "chartInventoryReport";
            series1.Name = "Series 1";
            series1.SeriesID = 0;
            doughnutSeriesView1.HoleRadiusPercent = 61;
            series1.View = doughnutSeriesView1;
            this.chartInventoryReport.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.chartInventoryReport.Size = new System.Drawing.Size(373, 287);
            this.chartInventoryReport.TabIndex = 71;
            // 
            // hangtonkho
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Pink;
            this.ClientSize = new System.Drawing.Size(932, 420);
            this.Controls.Add(this.chartInventoryReport);
            this.Controls.Add(this.dtpDenNgay);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtpTuNgay);
            this.Controls.Add(this.txtLuongTon);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bntdong);
            this.Controls.Add(this.bnthienthi);
            this.Controls.Add(this.bntbieudo);
            this.Controls.Add(this.bnttaoexel);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txtLuongBan);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "hangtonkho";
            this.Text = "hangtonkho";
            this.Load += new System.EventHandler(this.hangtonkho_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(doughnutSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartInventoryReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sachBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLuongTon;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button bntdong;
        private System.Windows.Forms.Button bnthienthi;
        private System.Windows.Forms.Button bntbieudo;
        private System.Windows.Forms.Button bnttaoexel;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtLuongBan;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpTuNgay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpDenNgay;
        private DevExpress.XtraCharts.ChartControl chartInventoryReport;
        private System.Windows.Forms.BindingSource sachBindingSource;
    }
}