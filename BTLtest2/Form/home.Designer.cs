using System;
using System.Windows.Forms;

namespace BTLtest2
{
    partial class home
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(home));
            this.panelchillform = new System.Windows.Forms.Panel();
            this.submenuhoadon = new System.Windows.Forms.Panel();
            this.bnthdmua = new System.Windows.Forms.Button();
            this.bnthdban = new System.Windows.Forms.Button();
            this.submenubaocao = new System.Windows.Forms.Panel();
            this.bnthanghoa = new System.Windows.Forms.Button();
            this.bnthangtonkho = new System.Windows.Forms.Button();
            this.bntkhachhang = new System.Windows.Forms.Button();
            this.bntchiphi = new System.Windows.Forms.Button();
            this.bnt_loinhuan = new System.Windows.Forms.Button();
            this.bntdtcpln = new System.Windows.Forms.Button();
            this.Logo = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bntdangxuat = new System.Windows.Forms.Button();
            this.bntthanhtoan = new System.Windows.Forms.Button();
            this.bnt_quanlykhachhang = new System.Windows.Forms.Button();
            this.bnttimkiem = new System.Windows.Forms.Button();
            this.bntbaocao = new System.Windows.Forms.Button();
            this.bnthoadon = new System.Windows.Forms.Button();
            this.bntqlynhanvien = new System.Windows.Forms.Button();
            this.bntqlysach = new System.Windows.Forms.Button();
            this.bnt_trangchu = new System.Windows.Forms.Button();
            this.panelchillform.SuspendLayout();
            this.submenuhoadon.SuspendLayout();
            this.submenubaocao.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelchillform
            // 
            this.panelchillform.AutoSize = true;
            this.panelchillform.BackColor = System.Drawing.Color.Pink;
            this.panelchillform.Controls.Add(this.submenuhoadon);
            this.panelchillform.Controls.Add(this.submenubaocao);
            this.panelchillform.Location = new System.Drawing.Point(189, 95);
            this.panelchillform.Name = "panelchillform";
            this.panelchillform.Size = new System.Drawing.Size(533, 391);
            this.panelchillform.TabIndex = 0;
            this.panelchillform.Paint += new System.Windows.Forms.PaintEventHandler(this.penicilliform_Paint);
            // 
            // submenuhoadon
            // 
            this.submenuhoadon.Controls.Add(this.bnthdmua);
            this.submenuhoadon.Controls.Add(this.bnthdban);
            this.submenuhoadon.Location = new System.Drawing.Point(8, 171);
            this.submenuhoadon.Name = "submenuhoadon";
            this.submenuhoadon.Size = new System.Drawing.Size(389, 91);
            this.submenuhoadon.TabIndex = 1;
            // 
            // bnthdmua
            // 
            this.bnthdmua.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bnthdmua.FlatAppearance.BorderSize = 0;
            this.bnthdmua.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnthdmua.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnthdmua.Location = new System.Drawing.Point(191, 16);
            this.bnthdmua.Name = "bnthdmua";
            this.bnthdmua.Size = new System.Drawing.Size(134, 58);
            this.bnthdmua.TabIndex = 13;
            this.bnthdmua.Text = "Hoá đơn mua";
            this.bnthdmua.UseVisualStyleBackColor = false;
            this.bnthdmua.Click += new System.EventHandler(this.bnthdmua_Click);
            // 
            // bnthdban
            // 
            this.bnthdban.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bnthdban.FlatAppearance.BorderSize = 0;
            this.bnthdban.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnthdban.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnthdban.Location = new System.Drawing.Point(9, 13);
            this.bnthdban.Name = "bnthdban";
            this.bnthdban.Size = new System.Drawing.Size(134, 58);
            this.bnthdban.TabIndex = 12;
            this.bnthdban.Text = "Hoá đơn bán";
            this.bnthdban.UseVisualStyleBackColor = false;
            this.bnthdban.Click += new System.EventHandler(this.bnthdban_Click);
            // 
            // submenubaocao
            // 
            this.submenubaocao.Controls.Add(this.bnthanghoa);
            this.submenubaocao.Controls.Add(this.bnthangtonkho);
            this.submenubaocao.Controls.Add(this.bntkhachhang);
            this.submenubaocao.Controls.Add(this.bntchiphi);
            this.submenubaocao.Controls.Add(this.bnt_loinhuan);
            this.submenubaocao.Controls.Add(this.bntdtcpln);
            this.submenubaocao.Location = new System.Drawing.Point(7, 208);
            this.submenubaocao.Margin = new System.Windows.Forms.Padding(2);
            this.submenubaocao.Name = "submenubaocao";
            this.submenubaocao.Size = new System.Drawing.Size(524, 181);
            this.submenubaocao.TabIndex = 0;
            // 
            // bnthanghoa
            // 
            this.bnthanghoa.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bnthanghoa.FlatAppearance.BorderSize = 0;
            this.bnthanghoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnthanghoa.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnthanghoa.Location = new System.Drawing.Point(196, 96);
            this.bnthanghoa.Name = "bnthanghoa";
            this.bnthanghoa.Size = new System.Drawing.Size(139, 57);
            this.bnthanghoa.TabIndex = 11;
            this.bnthanghoa.Text = "Hàng hoá";
            this.bnthanghoa.UseVisualStyleBackColor = false;
            this.bnthanghoa.Click += new System.EventHandler(this.bnthanghoa_Click);
            // 
            // bnthangtonkho
            // 
            this.bnthangtonkho.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bnthangtonkho.FlatAppearance.BorderSize = 0;
            this.bnthangtonkho.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnthangtonkho.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnthangtonkho.Location = new System.Drawing.Point(367, 96);
            this.bnthangtonkho.Name = "bnthangtonkho";
            this.bnthangtonkho.Size = new System.Drawing.Size(139, 57);
            this.bnthangtonkho.TabIndex = 10;
            this.bnthangtonkho.Text = "Hàng tồn kho ";
            this.bnthangtonkho.UseVisualStyleBackColor = false;
            this.bnthangtonkho.Click += new System.EventHandler(this.bnthangtonkho_Click);
            // 
            // bntkhachhang
            // 
            this.bntkhachhang.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bntkhachhang.FlatAppearance.BorderSize = 0;
            this.bntkhachhang.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntkhachhang.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntkhachhang.Location = new System.Drawing.Point(10, 96);
            this.bntkhachhang.Name = "bntkhachhang";
            this.bntkhachhang.Size = new System.Drawing.Size(139, 57);
            this.bntkhachhang.TabIndex = 9;
            this.bntkhachhang.Text = "Khách hàng";
            this.bntkhachhang.UseVisualStyleBackColor = false;
            this.bntkhachhang.Click += new System.EventHandler(this.bntkhachhang_Click);
            // 
            // bntchiphi
            // 
            this.bntchiphi.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bntchiphi.FlatAppearance.BorderSize = 0;
            this.bntchiphi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntchiphi.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntchiphi.Location = new System.Drawing.Point(372, 16);
            this.bntchiphi.Name = "bntchiphi";
            this.bntchiphi.Size = new System.Drawing.Size(134, 57);
            this.bntchiphi.TabIndex = 8;
            this.bntchiphi.Text = "Chi phí";
            this.bntchiphi.UseVisualStyleBackColor = false;
            this.bntchiphi.Click += new System.EventHandler(this.bntchiphi_Click);
            // 
            // bnt_loinhuan
            // 
            this.bnt_loinhuan.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bnt_loinhuan.FlatAppearance.BorderSize = 0;
            this.bnt_loinhuan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnt_loinhuan.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnt_loinhuan.Location = new System.Drawing.Point(196, 16);
            this.bnt_loinhuan.Name = "bnt_loinhuan";
            this.bnt_loinhuan.Size = new System.Drawing.Size(139, 58);
            this.bnt_loinhuan.TabIndex = 7;
            this.bnt_loinhuan.Text = "Lợi nhuận";
            this.bnt_loinhuan.UseVisualStyleBackColor = false;
            this.bnt_loinhuan.Click += new System.EventHandler(this.bnt_loinhuan_Click);
            // 
            // bntdtcpln
            // 
            this.bntdtcpln.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bntdtcpln.FlatAppearance.BorderSize = 0;
            this.bntdtcpln.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntdtcpln.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntdtcpln.Location = new System.Drawing.Point(15, 16);
            this.bntdtcpln.Name = "bntdtcpln";
            this.bntdtcpln.Size = new System.Drawing.Size(134, 58);
            this.bntdtcpln.TabIndex = 6;
            this.bntdtcpln.Text = "Danh thu";
            this.bntdtcpln.UseVisualStyleBackColor = false;
            this.bntdtcpln.Click += new System.EventHandler(this.bntdtcpln_Click);
            // 
            // Logo
            // 
            this.Logo.AutoSize = true;
            this.Logo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Logo.BackgroundImage")));
            this.Logo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Logo.Location = new System.Drawing.Point(28, 6);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(136, 107);
            this.Logo.TabIndex = 27;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(189, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(650, 79);
            this.panel1.TabIndex = 26;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Pink;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(348, 34);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(213, 24);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(3, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Quản lý";
            // 
            // bntdangxuat
            // 
            this.bntdangxuat.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bntdangxuat.FlatAppearance.BorderSize = 0;
            this.bntdangxuat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntdangxuat.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntdangxuat.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.bntdangxuat.Location = new System.Drawing.Point(21, 547);
            this.bntdangxuat.Name = "bntdangxuat";
            this.bntdangxuat.Size = new System.Drawing.Size(163, 34);
            this.bntdangxuat.TabIndex = 24;
            this.bntdangxuat.Text = "Đăng xuất";
            this.bntdangxuat.UseVisualStyleBackColor = false;
            this.bntdangxuat.Click += new System.EventHandler(this.bntdangxuat_Click);
            // 
            // bntthanhtoan
            // 
            this.bntthanhtoan.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bntthanhtoan.FlatAppearance.BorderSize = 0;
            this.bntthanhtoan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntthanhtoan.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntthanhtoan.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.bntthanhtoan.Location = new System.Drawing.Point(21, 364);
            this.bntthanhtoan.Name = "bntthanhtoan";
            this.bntthanhtoan.Size = new System.Drawing.Size(163, 34);
            this.bntthanhtoan.TabIndex = 23;
            this.bntthanhtoan.Text = "Thanh Toán";
            this.bntthanhtoan.UseVisualStyleBackColor = false;
            this.bntthanhtoan.Click += new System.EventHandler(this.bntthanhtoan_Click);
            // 
            // bnt_quanlykhachhang
            // 
            this.bnt_quanlykhachhang.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bnt_quanlykhachhang.FlatAppearance.BorderSize = 0;
            this.bnt_quanlykhachhang.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnt_quanlykhachhang.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnt_quanlykhachhang.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.bnt_quanlykhachhang.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bnt_quanlykhachhang.Location = new System.Drawing.Point(21, 244);
            this.bnt_quanlykhachhang.Name = "bnt_quanlykhachhang";
            this.bnt_quanlykhachhang.Size = new System.Drawing.Size(163, 34);
            this.bnt_quanlykhachhang.TabIndex = 22;
            this.bnt_quanlykhachhang.Text = "Quản lý khách hàng";
            this.bnt_quanlykhachhang.UseVisualStyleBackColor = false;
            this.bnt_quanlykhachhang.Click += new System.EventHandler(this.bnt_quanlykhachhang_Click);
            // 
            // bnttimkiem
            // 
            this.bnttimkiem.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bnttimkiem.FlatAppearance.BorderSize = 0;
            this.bnttimkiem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnttimkiem.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnttimkiem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.bnttimkiem.Location = new System.Drawing.Point(21, 403);
            this.bnttimkiem.Name = "bnttimkiem";
            this.bnttimkiem.Size = new System.Drawing.Size(163, 34);
            this.bnttimkiem.TabIndex = 21;
            this.bnttimkiem.Text = "Tìm kiếm";
            this.bnttimkiem.UseVisualStyleBackColor = false;
            // 
            // bntbaocao
            // 
            this.bntbaocao.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bntbaocao.FlatAppearance.BorderSize = 0;
            this.bntbaocao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntbaocao.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntbaocao.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.bntbaocao.Location = new System.Drawing.Point(21, 323);
            this.bntbaocao.Name = "bntbaocao";
            this.bntbaocao.Size = new System.Drawing.Size(163, 34);
            this.bntbaocao.TabIndex = 20;
            this.bntbaocao.Text = "Báo cáo";
            this.bntbaocao.UseVisualStyleBackColor = false;
            this.bntbaocao.Click += new System.EventHandler(this.bntbaocao_Click);
            // 
            // bnthoadon
            // 
            this.bnthoadon.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bnthoadon.FlatAppearance.BorderSize = 0;
            this.bnthoadon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnthoadon.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnthoadon.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.bnthoadon.Location = new System.Drawing.Point(21, 284);
            this.bnthoadon.Name = "bnthoadon";
            this.bnthoadon.Size = new System.Drawing.Size(163, 34);
            this.bnthoadon.TabIndex = 19;
            this.bnthoadon.Text = "Hoá đơn";
            this.bnthoadon.UseVisualStyleBackColor = false;
            this.bnthoadon.Click += new System.EventHandler(this.bnthoadon_Click);
            // 
            // bntqlynhanvien
            // 
            this.bntqlynhanvien.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bntqlynhanvien.FlatAppearance.BorderSize = 0;
            this.bntqlynhanvien.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntqlynhanvien.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntqlynhanvien.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.bntqlynhanvien.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bntqlynhanvien.Location = new System.Drawing.Point(21, 203);
            this.bntqlynhanvien.Name = "bntqlynhanvien";
            this.bntqlynhanvien.Size = new System.Drawing.Size(163, 34);
            this.bntqlynhanvien.TabIndex = 18;
            this.bntqlynhanvien.Text = "Quản lý nhân viên";
            this.bntqlynhanvien.UseVisualStyleBackColor = false;
            this.bntqlynhanvien.Click += new System.EventHandler(this.bntqlynhanvien_Click);
            // 
            // bntqlysach
            // 
            this.bntqlysach.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bntqlysach.FlatAppearance.BorderSize = 0;
            this.bntqlysach.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntqlysach.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bntqlysach.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.bntqlysach.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bntqlysach.Location = new System.Drawing.Point(21, 164);
            this.bntqlysach.Name = "bntqlysach";
            this.bntqlysach.Size = new System.Drawing.Size(163, 34);
            this.bntqlysach.TabIndex = 17;
            this.bntqlysach.Text = "Quản lý sách";
            this.bntqlysach.UseVisualStyleBackColor = false;
            this.bntqlysach.Click += new System.EventHandler(this.bntqlysach_Click);
            // 
            // bnt_trangchu
            // 
            this.bnt_trangchu.BackColor = System.Drawing.Color.LightSteelBlue;
            this.bnt_trangchu.FlatAppearance.BorderSize = 0;
            this.bnt_trangchu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bnt_trangchu.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bnt_trangchu.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.bnt_trangchu.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bnt_trangchu.Location = new System.Drawing.Point(21, 123);
            this.bnt_trangchu.Name = "bnt_trangchu";
            this.bnt_trangchu.Size = new System.Drawing.Size(163, 34);
            this.bnt_trangchu.TabIndex = 16;
            this.bnt_trangchu.Text = "Trang Chủ";
            this.bnt_trangchu.UseVisualStyleBackColor = false;
            // 
            // home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Pink;
            this.ClientSize = new System.Drawing.Size(963, 656);
            this.Controls.Add(this.panelchillform);
            this.Controls.Add(this.Logo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bntdangxuat);
            this.Controls.Add(this.bntthanhtoan);
            this.Controls.Add(this.bnt_quanlykhachhang);
            this.Controls.Add(this.bnttimkiem);
            this.Controls.Add(this.bntbaocao);
            this.Controls.Add(this.bnthoadon);
            this.Controls.Add(this.bntqlynhanvien);
            this.Controls.Add(this.bntqlysach);
            this.Controls.Add(this.bnt_trangchu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "home";
            this.Text = "home";
            this.Load += new System.EventHandler(this.home_Load);
            this.panelchillform.ResumeLayout(false);
            this.submenuhoadon.ResumeLayout(false);
            this.submenubaocao.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void penicilliform_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void Home_Load(object sender, EventArgs e) => throw new NotImplementedException();

        

        #endregion

        private System.Windows.Forms.Panel panelchillform;
        private System.Windows.Forms.Panel Logo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bntdangxuat;
        private System.Windows.Forms.Button bntthanhtoan;
        private System.Windows.Forms.Button bnt_quanlykhachhang;
        private System.Windows.Forms.Button bnttimkiem;
        private System.Windows.Forms.Button bntbaocao;
        private System.Windows.Forms.Button bnthoadon;
        private System.Windows.Forms.Button bntqlynhanvien;
        private System.Windows.Forms.Button bntqlysach;
        private System.Windows.Forms.Button bnt_trangchu;
        private System.Windows.Forms.Panel submenubaocao;
        private System.Windows.Forms.Button bnthanghoa;
        private System.Windows.Forms.Button bnthangtonkho;
        private System.Windows.Forms.Button bntkhachhang;
        private System.Windows.Forms.Button bntchiphi;
        private System.Windows.Forms.Button bnt_loinhuan;
        private System.Windows.Forms.Button bntdtcpln;
        private System.Windows.Forms.Panel submenuhoadon;
        private System.Windows.Forms.Button bnthdban;
        private System.Windows.Forms.Button bnthdmua;
    }
}