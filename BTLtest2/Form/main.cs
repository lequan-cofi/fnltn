using BTLtest2.function;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTLtest2
{
    public partial class main : Form
    {
        private Form activeForm = null;
        public main()
        {
            InitializeComponent();
        }
        private string currentUsername;
        private string currentPhanQuyen;
        public main(string username)
        {
            InitializeComponent();
            currentUsername = username;
            currentPhanQuyen = ktradangnhap.KiemtraPhanquyen(username);
        }
        private void hideSubmenu()
        {
            if (sub_menubaocao.Visible)
                sub_menubaocao.Visible = false;
            if (sub_menuhd.Visible == true)
                sub_menuhd.Visible = false;
            
        }
        private void showMenu(Panel subMenu)
        {
            if (activeForm != null)
                activeForm.Close();
            if (!subMenu.Visible)
            {
                hideSubmenu();
                subMenu.Visible = true;
            }
            else
                subMenu.Visible = false;
        }
        private void openChildForm(Form childForm)
        {
            hideSubmenu();
            if (activeForm != null)
                activeForm.Close();

            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            chllform.Controls.Add(childForm);
            chllform.Tag = childForm;

            childForm.BringToFront();
            childForm.Show();
        }

        private void main_Load(object sender, EventArgs e)
        {
            hideSubmenu();
           
            // Kiểm tra null tránh lỗi
            if (!string.IsNullOrEmpty(currentUsername))
            {
                lbusername.Text = "Xin chào: " + currentUsername;

                if (currentPhanQuyen == "1")
                {
                    lbRole.Text = "Quản lý";
                    bnt_qlnhanvien.Visible = true;
                }
                else
                {
                    lbRole.Text = "Nhân viên";
                    bnt_qlnhanvien.Visible = false;
                }
            }
            else
            {
                lbusername.Text = "Không xác định";
                lbRole.Text = "Không rõ quyền";
            }

        }

        private void bnt_qlysach_Click(object sender, EventArgs e)
        {
            openChildForm(new qlykho());
        }

        private void bnt_qlnhanvien_Click(object sender, EventArgs e)
        {
            openChildForm(new quanlynhanvien());
        }

        private void bnt_qlkhach_Click(object sender, EventArgs e)
        {
            openChildForm(new quanlykhachhang());
        }

        private void bnt_hoadon_Click(object sender, EventArgs e)
        {
            showMenu(sub_menuhd);   
        }

        private void bnt_hdnhap_Click(object sender, EventArgs e)
        {
            openChildForm(new qlhoadonnhap());
        }

        private void bnt_hdban_Click(object sender, EventArgs e)
        {
            openChildForm(new quanlyhoadonban());
        }

        private void bnt_thanhtoan_Click(object sender, EventArgs e)
        {
            openChildForm(new thanhtoan());
        }

        private void bnt_baocao_Click(object sender, EventArgs e)
        {
            showMenu(sub_menubaocao);
        }

        private void bnt_dt_Click(object sender, EventArgs e)
        {
            openChildForm(new doanhthu());
        }

        private void bnt_cp_Click(object sender, EventArgs e)
        {
        }

        private void bnt_ln_Click(object sender, EventArgs e)
        {
            openChildForm(new loinhuan());
        }

        private void bnt_kh_Click(object sender, EventArgs e)
        {
            openChildForm(new khachhang());
        }

        private void bnt_hh_Click(object sender, EventArgs e)
        {
            openChildForm(new hanghoa());
        }

        private void bnt_htk_Click(object sender, EventArgs e)
        {
            openChildForm(new hangtonkho());
        }

        private void bnt_tksach_Click(object sender, EventArgs e)
        {

        }

        private void bnt_timkiem_Click(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void bnt_quanlynhacc_Click(object sender, EventArgs e)
        {
            openChildForm(new quanlynhacungcap());
        }

        private void bnt_thoat_Click(object sender, EventArgs e)
        {
            DialogResult confirmLogout = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmLogout == DialogResult.Yes)
            {
                this.Close(); // Lệnh này sẽ đóng form 'main'
            }
        }

        private void bnt_trangchu_Click(object sender, EventArgs e)
        {
            if (activeForm != null)
                activeForm.Close();
        }
    }
}
