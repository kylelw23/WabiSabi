using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WabiSabi2.BLL;
using WabiSabi2.DAL;

namespace WabiSabi2.UI
{
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }

        loginBLL loginBLL = new loginBLL();
        loginDAL loginDAL = new loginDAL();

        public static string logged_in;

        private void fLogin_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            loginBLL.username = txbUsername.Text.Trim();
            loginBLL.password = txbPassword.Text.Trim();

            //Checking the login credentials
            int success = loginDAL.loginCheck(loginBLL);
            if (success == 1)
            {
                //login success
                MessageBox.Show("Đăng nhập thành công. Loại tài khoản quản trị");
                logged_in = loginBLL.username;
                //Need to open Respecive Forms based on user type
                fAdminDashboard admin = new fAdminDashboard();
                this.Hide();
                admin.ShowDialog();
                this.Show();
            }
            else if(success == 2)
            {
                MessageBox.Show("Đăng nhập thành công. Loại tài khoản nhân viên");
                //Need to open Respecive Forms based on user type
                logged_in = loginBLL.username;
                fUserDashBoard admin = new fUserDashBoard();

                this.Hide();
                admin.ShowDialog();
                this.Show();
            }
            else
            {
                // login fail
                MessageBox.Show("Đăng nhập thất bại. Xin vui lòng thử lại!");
            }
        }

        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự muốn thoát chương trình?", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }
    }
}
