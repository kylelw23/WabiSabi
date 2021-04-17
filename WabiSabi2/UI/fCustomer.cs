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
    public partial class fCustomer : Form
    {
        public fCustomer()
        {
            InitializeComponent();
        }
        customerBLL customerBLL = new customerBLL();
        customerDAL customerDAL = new customerDAL();

        userDAL userDAL = new userDAL();

        private void fCustomer_Load(object sender, EventArgs e)
        {
            DataTable dataTable = customerDAL.Select();
            dgvCustomer.DataSource = dataTable;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            //Getting data from UI
            customerBLL.name = txtBoxCustomerName.Text;
            customerBLL.email = txtBoxCustomerEmail.Text;
            customerBLL.contact = txtBoxCustomerContact.Text;
            customerBLL.address = txtBoxCustomerAddress.Text;
            customerBLL.added_date = DateTime.Now;

            string logged_in_user = fLogin.logged_in;

            userBLL user = userDAL.GetIDFromUsername(logged_in_user);
            customerBLL.added_by = user.username;

            //Inserting data into database
            bool success = customerDAL.Insert(customerBLL);
            //If data is succesfully insertted then the value of sucess will be true
            if (success == true)
            {
                //Data successfully inserted
                MessageBox.Show("Khách hàng được thêm vào thành công.");
                clear();
            }
            else
            {
                //Failed
                MessageBox.Show("Không thể thêm khách hàng.");
            }
            //refreshing data grid view
            DataTable dataTable = customerDAL.Select();
            dgvCustomer.DataSource = dataTable;
        }
        private void clear()
        {
            txtBoxCustomerID.Text = "";
            txtBoxCustomerName.Text = "";
            txtBoxCustomerEmail.Text = "";
            txtBoxCustomerContact.Text = "";
            txtBoxCustomerAddress.Text = "";
            txtBoxSearch.Text = "";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get the values from the UI
            customerBLL.id = Convert.ToInt32(txtBoxCustomerID.Text);
            customerBLL.name = txtBoxCustomerName.Text;
            customerBLL.email = txtBoxCustomerEmail.Text;
            customerBLL.contact = txtBoxCustomerContact.Text;
            customerBLL.address = txtBoxCustomerAddress.Text;
            customerBLL.added_date = DateTime.Now;

            string logged_in_user = fLogin.logged_in;
            userBLL user = userDAL.GetIDFromUsername(logged_in_user);
            customerBLL.added_by = user.username;

            //Updating data into database
            bool success = customerDAL.Update(customerBLL);

            if (success == true)
            {
                MessageBox.Show("Khách hàng " + txtBoxCustomerName.Text + " được cập nhật thành công");
                clear();
            }
            else
            {
                MessageBox.Show("Không thể cập nhật khách hàng " + txtBoxCustomerName.Text);
            }

            //refreshing data grid view
            DataTable dataTable = customerDAL.Select();
            dgvCustomer.DataSource = dataTable;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Getting User ID from Form
            customerBLL.id = Convert.ToInt32(txtBoxCustomerID.Text);

            bool success = customerDAL.Delete(customerBLL);

            if (success == true)
            {
                MessageBox.Show("Khách hàng " + txtBoxCustomerName.Text + " được cập nhật thành công");
                clear();
            }
            else
            {
                MessageBox.Show("Không thể cập nhật khách hàng " + txtBoxCustomerName.Text);
            }

            //refreshing data grid view
            DataTable dataTable = customerDAL.Select();
            dgvCustomer.DataSource = dataTable;
        }

        private void dgvCustomer_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex;
            txtBoxCustomerID.Text = dgvCustomer.Rows[rowIndex].Cells[0].Value.ToString();
            txtBoxCustomerName.Text = dgvCustomer.Rows[rowIndex].Cells[1].Value.ToString();
            txtBoxCustomerEmail.Text = dgvCustomer.Rows[rowIndex].Cells[2].Value.ToString();
            txtBoxCustomerContact.Text = dgvCustomer.Rows[rowIndex].Cells[2].Value.ToString();
            txtBoxCustomerAddress.Text = dgvCustomer.Rows[rowIndex].Cells[2].Value.ToString();
        }

        private void txtBoxSearch_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtBoxSearch.Text;

            //check if the keywords has value 
            if (keywords != null)
            {
                //Show user based on keywords
                DataTable dataTable = customerDAL.Search(keywords);
                dgvCustomer.DataSource = dataTable;
            }
            else
            {
                //show all users from the database
                DataTable dataTable = customerDAL.Select();
                dgvCustomer.DataSource = dataTable;
            }
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
