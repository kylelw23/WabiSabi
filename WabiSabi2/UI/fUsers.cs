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
    public partial class fUsers : Form
    {
        public fUsers()
        {
            InitializeComponent();
        }

        userBLL userBLL = new userBLL();
        userDAL userDAL = new userDAL();

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBoxFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblFirstName_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            string keywords = txtBoxSearch.Text;

            //check if the keywords has value 
            if (keywords != null)
            {
                //Show user based on keywords
                DataTable dataTable = userDAL.Search(keywords);
                dgvUser.DataSource = dataTable;
            }
            else
            {
                //show all users from the database
                DataTable dataTable = userDAL.Select();
                dgvUser.DataSource = dataTable;
            }
        }

        private void fUsers_Load(object sender, EventArgs e)
        {
            DataTable dataTable = userDAL.Select();
            dgvUser.DataSource = dataTable;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(cmbUserType.Text == "")
            {
                MessageBox.Show("Vui lòng điền mục 'Loại' tài khoản");
            }
            else
            {
                //Getting Username of the logged in user
                string logged_in_user = fLogin.logged_in;

                //Getting data from UI
                userBLL.first_name = txtBoxFirstName.Text;
                userBLL.last_name = txtBoxLastName.Text;
                userBLL.email = txtBoxEmail.Text;
                userBLL.username = txtBoxUsername.Text;
                userBLL.password = txtBoxPassword.Text;
                userBLL.contact = txtBoxContact.Text;
                userBLL.address = txtBoxAddress.Text;
                userBLL.gender = cmbGender.Text;
                userBLL.user_type = cmbUserType.Text;

                userBLL.added_date = DateTime.Now;

                userBLL usr = userDAL.GetIDFromUsername(logged_in_user);
                userBLL.added_by = usr.username;

                //Inserting data into database
                bool success = userDAL.Insert(userBLL);
                //If data is succesfully insertted then the value of sucess will be true
                if (success == true)
                {
                    //Data successfully inserted
                    MessageBox.Show("User successfully created.");
                    clear();
                }
                else
                {
                    //Failed
                    MessageBox.Show("Failed to add new user.");
                }
                //refreshing data grid view
                DataTable dataTable = userDAL.Select();
                dgvUser.DataSource = dataTable;
            }
        }
        //Clearing data after execute functionality
        private void clear()
        {
            txtBoxUserID.Text = "";
            txtBoxFirstName.Text = "";
            txtBoxLastName.Text = "";
            txtBoxEmail.Text = "";
            txtBoxUsername.Text = "";
            txtBoxPassword.Text = "";
            txtBoxContact.Text = "";
            txtBoxAddress.Text = "";
            cmbGender.Text = "";
            cmbUserType.Text = "";
        }

        private void dgvUser_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Get the index of the particular row
            int rowIndex = e.RowIndex;
            txtBoxUserID.Text = dgvUser.Rows[rowIndex].Cells[0].Value.ToString();
            txtBoxFirstName.Text = dgvUser.Rows[rowIndex].Cells[1].Value.ToString();
            txtBoxLastName.Text = dgvUser.Rows[rowIndex].Cells[2].Value.ToString();
            txtBoxEmail.Text = dgvUser.Rows[rowIndex].Cells[3].Value.ToString();
            txtBoxUsername.Text = dgvUser.Rows[rowIndex].Cells[4].Value.ToString();
            txtBoxPassword.Text = dgvUser.Rows[rowIndex].Cells[5].Value.ToString();
            txtBoxContact.Text = dgvUser.Rows[rowIndex].Cells[6].Value.ToString();
            txtBoxAddress.Text = dgvUser.Rows[rowIndex].Cells[7].Value.ToString();
            cmbGender.Text = dgvUser.Rows[rowIndex].Cells[8].Value.ToString();
            cmbUserType.Text = dgvUser.Rows[rowIndex].Cells[9].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get the values from the UI
            userBLL.id = Convert.ToInt32(txtBoxUserID.Text);
            userBLL.first_name = txtBoxFirstName.Text;
            userBLL.last_name = txtBoxLastName.Text;
            userBLL.email = txtBoxEmail.Text;
            userBLL.username = txtBoxUsername.Text;
            userBLL.password = txtBoxPassword.Text;
            userBLL.contact = txtBoxContact.Text;
            userBLL.address = txtBoxAddress.Text;
            userBLL.gender = cmbGender.Text;
            userBLL.user_type = cmbUserType.Text;
            userBLL.added_date = DateTime.Now;

            String logged_in_user = fLogin.logged_in;
            userBLL user = userDAL.GetIDFromUsername(logged_in_user);

            userBLL.added_by = user.username;

            //Updating data into database
            bool success = userDAL.Update(userBLL);
            
            if(success == true)
            {
                MessageBox.Show("User " + txtBoxFirstName.Text + " is updated successfully");
                clear();
            }
            else
            {
                MessageBox.Show("Can not update user " + txtBoxFirstName.Text);
            }

            //refreshing data grid view
            DataTable dataTable = userDAL.Select();
            dgvUser.DataSource = dataTable;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(cmbUserType.Text.ToLower() == "admin")
            {
                MessageBox.Show("Bạn không thể xóa tài khoản admin, vui lòng liên hệ admin");
            }
            else
            {
                //Getting User ID from Form
                userBLL.id = Convert.ToInt32(txtBoxUserID.Text);

                bool success = userDAL.Delete(userBLL);

                if (success == true)
                {
                    MessageBox.Show("Người dùng " + txtBoxFirstName.Text + " được xóa khỏi hệ thống thành công");
                    clear();
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật người dùng này " + txtBoxFirstName.Text);
                }

                //refreshing data grid view
                DataTable dataTable = userDAL.Select();
                dgvUser.DataSource = dataTable;


            }
        }

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblTop_Click(object sender, EventArgs e)
        {

        }

        private void txtBoxPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblPassword_Click(object sender, EventArgs e)
        {

        }

        private void txtBoxAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblAddress_Click(object sender, EventArgs e)
        {

        }

        private void txtBoxContact_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblContact_Click(object sender, EventArgs e)
        {

        }

        private void lblGender_Click(object sender, EventArgs e)
        {

        }

        private void cmbGender_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbUserType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void txtBoxUserID_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblUserID_Click(object sender, EventArgs e)
        {

        }

        private void lblSearch_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtBoxLastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }
    }
}
