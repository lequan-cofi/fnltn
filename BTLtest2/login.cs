using BTLtest2.Class;
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
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }
        public taikhoan Laythongtin()
        {
            taikhoan tk = new taikhoan();
            tk.usename = txtusename.Text;
            tk.password = txtpassword.Text;
            return tk;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            string username = txtusename.Text;
            // kiem tra dang nhap
            if (txtusename.Text == "" || txtpassword.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
            }
            else
            {
                if (function.ktradangnhap.Kiemtra(txtusename.Text, txtpassword.Text))
                {
                   

                    this.Hide();
                    main mainForm = new main(username);
                    mainForm.FormClosed += (s, args) => this.Show(); // Khi main đóng, hiện lại login
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Đăng nhập thất bại");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void login_Load(object sender, EventArgs e)
        {
            txtpassword.UseSystemPasswordChar = true;
            btn_mk.Text = "👁️"; // Hoặc dùng icon tùy bạn
        }

        private void btn_mk_Click(object sender, EventArgs e)
        {
            if (txtpassword.UseSystemPasswordChar)
            {
                txtpassword.UseSystemPasswordChar = false;
                btn_mk.Text = "🙈"; // Biểu tượng "ẩn"
            }
            else
            {
                txtpassword.UseSystemPasswordChar = true;
                btn_mk.Text = "👁️"; // Biểu tượng "hiện"
            }
        }
    }
}
