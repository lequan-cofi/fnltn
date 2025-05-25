namespace BTLtest2
{
    partial class doanhthu
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
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.DoughnutSeriesView doughnutSeriesView1 = new DevExpress.XtraCharts.DoughnutSeriesView();
            DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.DoughnutSeriesView doughnutSeriesView2 = new DevExpress.XtraCharts.DoughnutSeriesView();
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series3 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.StackedBarSeriesView stackedBarSeriesView1 = new DevExpress.XtraCharts.StackedBarSeriesView();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtThongKeTren = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.bnttaoexel = new System.Windows.Forms.Button();
            this.bntdong = new System.Windows.Forms.Button();
            this.datestart = new System.Windows.Forms.DateTimePicker();
            this.dateend = new System.Windows.Forms.DateTimePicker();
            this.lbtongdoangthu = new System.Windows.Forms.Label();
            this.bnt_dt = new System.Windows.Forms.Button();
            this.bnt_cp = new System.Windows.Forms.Button();
            this.bnt_ln = new System.Windows.Forms.Button();
            this.chartControlDoanhThu = new DevExpress.XtraCharts.ChartControl();
            this.chartControlChiPhi = new DevExpress.XtraCharts.ChartControl();
            this.chartControlLoiNhuan = new DevExpress.XtraCharts.ChartControl();
            this.txtThongKeDuoi = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControlDoanhThu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(doughnutSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControlChiPhi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(doughnutSeriesView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControlLoiNhuan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(stackedBarSeriesView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(46, 93);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "Khoảng thời gian";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(136, 127);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 24);
            this.label3.TabIndex = 4;
            this.label3.Text = "đến";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(61, 168);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 24);
            this.label4.TabIndex = 6;
            this.label4.Text = "Thống kê trên";
            // 
            // txtThongKeTren
            // 
            this.txtThongKeTren.Location = new System.Drawing.Point(217, 173);
            this.txtThongKeTren.Margin = new System.Windows.Forms.Padding(2);
            this.txtThongKeTren.Name = "txtThongKeTren";
            this.txtThongKeTren.Size = new System.Drawing.Size(125, 20);
            this.txtThongKeTren.TabIndex = 7;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(13, 268);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(536, 150);
            this.dataGridView1.TabIndex = 8;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // bnttaoexel
            // 
            this.bnttaoexel.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bnttaoexel.FlatAppearance.BorderSize = 0;
            this.bnttaoexel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnttaoexel.Location = new System.Drawing.Point(78, 458);
            this.bnttaoexel.Margin = new System.Windows.Forms.Padding(2);
            this.bnttaoexel.Name = "bnttaoexel";
            this.bnttaoexel.Size = new System.Drawing.Size(95, 36);
            this.bnttaoexel.TabIndex = 9;
            this.bnttaoexel.Text = "Xuất file";
            this.bnttaoexel.UseVisualStyleBackColor = false;
            this.bnttaoexel.Click += new System.EventHandler(this.bnttaoexel_Click);
            // 
            // bntdong
            // 
            this.bntdong.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bntdong.FlatAppearance.BorderSize = 0;
            this.bntdong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntdong.Location = new System.Drawing.Point(315, 458);
            this.bntdong.Margin = new System.Windows.Forms.Padding(2);
            this.bntdong.Name = "bntdong";
            this.bntdong.Size = new System.Drawing.Size(95, 36);
            this.bntdong.TabIndex = 12;
            this.bntdong.Text = "Đóng";
            this.bntdong.UseVisualStyleBackColor = false;
            this.bntdong.Click += new System.EventHandler(this.bntdong_Click);
            // 
            // datestart
            // 
            this.datestart.Location = new System.Drawing.Point(217, 95);
            this.datestart.Name = "datestart";
            this.datestart.Size = new System.Drawing.Size(200, 20);
            this.datestart.TabIndex = 13;
            // 
            // dateend
            // 
            this.dateend.Location = new System.Drawing.Point(217, 131);
            this.dateend.Name = "dateend";
            this.dateend.Size = new System.Drawing.Size(200, 20);
            this.dateend.TabIndex = 14;
            // 
            // lbtongdoangthu
            // 
            this.lbtongdoangthu.AutoSize = true;
            this.lbtongdoangthu.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbtongdoangthu.Location = new System.Drawing.Point(16, 420);
            this.lbtongdoangthu.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbtongdoangthu.Name = "lbtongdoangthu";
            this.lbtongdoangthu.Size = new System.Drawing.Size(55, 24);
            this.lbtongdoangthu.TabIndex = 15;
            this.lbtongdoangthu.Text = "Tổng";
            // 
            // bnt_dt
            // 
            this.bnt_dt.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bnt_dt.FlatAppearance.BorderSize = 0;
            this.bnt_dt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnt_dt.Location = new System.Drawing.Point(50, 26);
            this.bnt_dt.Margin = new System.Windows.Forms.Padding(2);
            this.bnt_dt.Name = "bnt_dt";
            this.bnt_dt.Size = new System.Drawing.Size(95, 36);
            this.bnt_dt.TabIndex = 16;
            this.bnt_dt.Text = "Doanh thu";
            this.bnt_dt.UseVisualStyleBackColor = false;
            this.bnt_dt.Click += new System.EventHandler(this.bnt_dt_Click);
            // 
            // bnt_cp
            // 
            this.bnt_cp.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bnt_cp.FlatAppearance.BorderSize = 0;
            this.bnt_cp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnt_cp.Location = new System.Drawing.Point(217, 26);
            this.bnt_cp.Margin = new System.Windows.Forms.Padding(2);
            this.bnt_cp.Name = "bnt_cp";
            this.bnt_cp.Size = new System.Drawing.Size(95, 36);
            this.bnt_cp.TabIndex = 17;
            this.bnt_cp.Text = "Chi phi";
            this.bnt_cp.UseVisualStyleBackColor = false;
            this.bnt_cp.Click += new System.EventHandler(this.bnt_cp_Click);
            // 
            // bnt_ln
            // 
            this.bnt_ln.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bnt_ln.FlatAppearance.BorderSize = 0;
            this.bnt_ln.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnt_ln.Location = new System.Drawing.Point(414, 26);
            this.bnt_ln.Margin = new System.Windows.Forms.Padding(2);
            this.bnt_ln.Name = "bnt_ln";
            this.bnt_ln.Size = new System.Drawing.Size(95, 36);
            this.bnt_ln.TabIndex = 18;
            this.bnt_ln.Text = "Lợi nhuận";
            this.bnt_ln.UseVisualStyleBackColor = false;
            this.bnt_ln.Click += new System.EventHandler(this.bnt_ln_Click);
            // 
            // chartControlDoanhThu
            // 
            this.chartControlDoanhThu.Location = new System.Drawing.Point(554, 136);
            this.chartControlDoanhThu.Name = "chartControlDoanhThu";
            series1.Name = "Series 1";
            series1.SeriesID = 0;
            series1.View = doughnutSeriesView1;
            this.chartControlDoanhThu.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.chartControlDoanhThu.Size = new System.Drawing.Size(439, 237);
            this.chartControlDoanhThu.TabIndex = 19;
            this.chartControlDoanhThu.Click += new System.EventHandler(this.chartControlChiPhi_Click);
            // 
            // chartControlChiPhi
            // 
            this.chartControlChiPhi.Location = new System.Drawing.Point(553, 137);
            this.chartControlChiPhi.Name = "chartControlChiPhi";
            series2.Name = "Series 1";
            series2.SeriesID = 0;
            series2.View = doughnutSeriesView2;
            this.chartControlChiPhi.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series2};
            this.chartControlChiPhi.Size = new System.Drawing.Size(439, 237);
            this.chartControlChiPhi.TabIndex = 21;
            // 
            // chartControlLoiNhuan
            // 
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            this.chartControlLoiNhuan.Diagram = xyDiagram1;
            this.chartControlLoiNhuan.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            this.chartControlLoiNhuan.Location = new System.Drawing.Point(554, 135);
            this.chartControlLoiNhuan.Name = "chartControlLoiNhuan";
            series3.Name = "Series 2";
            series3.SeriesID = 1;
            stackedBarSeriesView1.BarWidth = 0.8D;
            stackedBarSeriesView1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(112)))), ((int)(((byte)(192)))));
            series3.View = stackedBarSeriesView1;
            this.chartControlLoiNhuan.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series3};
            this.chartControlLoiNhuan.Size = new System.Drawing.Size(438, 350);
            this.chartControlLoiNhuan.TabIndex = 22;
            // 
            // txtThongKeDuoi
            // 
            this.txtThongKeDuoi.Location = new System.Drawing.Point(217, 215);
            this.txtThongKeDuoi.Margin = new System.Windows.Forms.Padding(2);
            this.txtThongKeDuoi.Name = "txtThongKeDuoi";
            this.txtThongKeDuoi.Size = new System.Drawing.Size(125, 20);
            this.txtThongKeDuoi.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(61, 210);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 24);
            this.label1.TabIndex = 23;
            this.label1.Text = "Thống kê dưới";
            // 
            // doanhthu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Pink;
            this.ClientSize = new System.Drawing.Size(999, 539);
            this.Controls.Add(this.txtThongKeDuoi);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chartControlLoiNhuan);
            this.Controls.Add(this.chartControlChiPhi);
            this.Controls.Add(this.chartControlDoanhThu);
            this.Controls.Add(this.bnt_ln);
            this.Controls.Add(this.bnt_cp);
            this.Controls.Add(this.bnt_dt);
            this.Controls.Add(this.lbtongdoangthu);
            this.Controls.Add(this.dateend);
            this.Controls.Add(this.datestart);
            this.Controls.Add(this.bntdong);
            this.Controls.Add(this.bnttaoexel);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txtThongKeTren);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "doanhthu";
            this.Text = "doanhthu";
            this.Load += new System.EventHandler(this.doanhthu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(doughnutSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControlDoanhThu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(doughnutSeriesView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControlChiPhi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(stackedBarSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControlLoiNhuan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtThongKeTren;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button bnttaoexel;
        private System.Windows.Forms.Button bntdong;
        private System.Windows.Forms.DateTimePicker datestart;
        private System.Windows.Forms.DateTimePicker dateend;
        private System.Windows.Forms.Label lbtongdoangthu;
        private System.Windows.Forms.Button bnt_dt;
        private System.Windows.Forms.Button bnt_cp;
        private System.Windows.Forms.Button bnt_ln;
        private DevExpress.XtraCharts.ChartControl chartControlDoanhThu;
        private DevExpress.XtraCharts.ChartControl chartControlChiPhi;
        private DevExpress.XtraCharts.ChartControl chartControlLoiNhuan;
        private System.Windows.Forms.TextBox txtThongKeDuoi;
        private System.Windows.Forms.Label label1;
    }
}