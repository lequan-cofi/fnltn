namespace BTLtest2
{
    partial class bieudo
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
            DevExpress.XtraCharts.Series series3 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.DoughnutSeriesView doughnutSeriesView3 = new DevExpress.XtraCharts.DoughnutSeriesView();
            this.chartControlDoanhThu = new DevExpress.XtraCharts.ChartControl();
            this.chartControlChiPhi = new DevExpress.XtraCharts.ChartControl();
            this.chartControlLoiNhuan = new DevExpress.XtraCharts.ChartControl();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chartControlDoanhThu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(doughnutSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControlChiPhi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(doughnutSeriesView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControlLoiNhuan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(doughnutSeriesView3)).BeginInit();
            this.SuspendLayout();
            // 
            // chartControlDoanhThu
            // 
            this.chartControlDoanhThu.Location = new System.Drawing.Point(755, 440);
            this.chartControlDoanhThu.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chartControlDoanhThu.Name = "chartControlDoanhThu";
            series1.Name = "Series 1";
            series1.SeriesID = 0;
            series1.View = doughnutSeriesView1;
            this.chartControlDoanhThu.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.chartControlDoanhThu.Size = new System.Drawing.Size(752, 435);
            this.chartControlDoanhThu.TabIndex = 0;
            // 
            // chartControlChiPhi
            // 
            this.chartControlChiPhi.Location = new System.Drawing.Point(762, 0);
            this.chartControlChiPhi.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chartControlChiPhi.Name = "chartControlChiPhi";
            series2.Name = "Series 1";
            series2.SeriesID = 0;
            series2.View = doughnutSeriesView2;
            this.chartControlChiPhi.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series2};
            this.chartControlChiPhi.Size = new System.Drawing.Size(744, 437);
            this.chartControlChiPhi.TabIndex = 1;
            // 
            // chartControlLoiNhuan
            // 
            this.chartControlLoiNhuan.Location = new System.Drawing.Point(0, 438);
            this.chartControlLoiNhuan.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chartControlLoiNhuan.Name = "chartControlLoiNhuan";
            series3.Name = "Series 1";
            series3.SeriesID = 0;
            series3.View = doughnutSeriesView3;
            this.chartControlLoiNhuan.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series3};
            this.chartControlLoiNhuan.Size = new System.Drawing.Size(752, 435);
            this.chartControlLoiNhuan.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(521, 366);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(216, 62);
            this.button1.TabIndex = 3;
            this.button1.Text = "Thoát";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // bieudo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1503, 914);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chartControlLoiNhuan);
            this.Controls.Add(this.chartControlChiPhi);
            this.Controls.Add(this.chartControlDoanhThu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "bieudo";
            this.Text = "bieudo";
            this.Load += new System.EventHandler(this.bieudo_Load);
            ((System.ComponentModel.ISupportInitialize)(doughnutSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControlDoanhThu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(doughnutSeriesView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControlChiPhi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(doughnutSeriesView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControlLoiNhuan)).EndInit();
            this.ResumeLayout(false);

        }

       

        private DevExpress.XtraCharts.ChartControl chartControlDoanhThu;
        private DevExpress.XtraCharts.ChartControl chartControlChiPhi;
        private DevExpress.XtraCharts.ChartControl chartControlLoiNhuan;
        private System.Windows.Forms.Button button1;
    }
}