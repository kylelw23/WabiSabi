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
    public partial class fProduct : Form
    {
        public fProduct()
        {
            InitializeComponent();
        }
        categoryDAL categoryDAl = new categoryDAL();
        productBLL productBLL = new productBLL();
        productDAL productDAL = new productDAL();

        userDAL userDAL = new userDAL();

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fProduct_Load(object sender, EventArgs e)
        {
            DataTable dataTableProduct = productDAL.Select();
            dgvProduct.DataSource = dataTableProduct;

            DataTable dataTableCategory = categoryDAl.Select();
            cmbProductCategory.DataSource = dataTableCategory;
            cmbProductCategory.DisplayMember = "title";
            cmbProductCategory.ValueMember = "title";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            productBLL.name = txtBoxProductName.Text;
            productBLL.category = cmbProductCategory.Text;
            productBLL.description = txtBoxDescription.Text;
            productBLL.size = cmbProductSize.Text;
            productBLL.rate = Convert.ToInt32(txtBoxProductRate.Text);
            productBLL.qty = 0;
            productBLL.added_date = DateTime.Now;

            String logged_in_user = fLogin.logged_in;
            userBLL user = userDAL.GetIDFromUsername(logged_in_user);

            productBLL.added_by = user.username;

            bool success = productDAL.Insert(productBLL);

            if(success == true)
            {
                //Data successfully inserted
                MessageBox.Show("Món được thêm vào thành công");
                clear();
            }
            else
            {
                //Failed
                MessageBox.Show("Không thể thêm vào món này");
            }
            //refreshing data grid view
            DataTable dataTable = productDAL.Select();
            dgvProduct.DataSource = dataTable;
        }

        private void clear()
        {
            txtBoxProductID.Text = "";
            txtBoxDescription.Text = "";
            txtBoxProductName.Text = "";
            txtBoxProductRate.Text = "";
            txtBoxProductSearch.Text = "";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get the values from the UI
            productBLL.id = Convert.ToInt32(txtBoxProductID.Text);
            productBLL.name = txtBoxProductName.Text;
            productBLL.category = cmbProductCategory.Text;
            productBLL.description = txtBoxDescription.Text;
            productBLL.size = cmbProductSize.Text;
            productBLL.rate = Convert.ToInt32(txtBoxProductRate.Text);
            productBLL.added_date = DateTime.Now;

            String logged_in_user = fLogin.logged_in;
            userBLL user = userDAL.GetIDFromUsername(logged_in_user);

            productBLL.added_by = user.username;

            //Updating data into database
            bool success = productDAL.Update(productBLL);

            if (success == true)
            {
                MessageBox.Show("Món " + txtBoxProductName.Text + " được cập nhật thành công");
                clear();
            }
            else
            {
                MessageBox.Show("Không thể cập nhật món " + txtBoxProductName.Text);
            }

            //refreshing data grid view
            DataTable dataTable = productDAL.Select();
            dgvProduct.DataSource = dataTable;
        }

        private void dgvProduct_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex;
            txtBoxProductID.Text = dgvProduct.Rows[rowIndex].Cells[0].Value.ToString();
            txtBoxProductName.Text = dgvProduct.Rows[rowIndex].Cells[1].Value.ToString();
            cmbProductCategory.Text = dgvProduct.Rows[rowIndex].Cells[2].Value.ToString();
            txtBoxDescription.Text = dgvProduct.Rows[rowIndex].Cells[3].Value.ToString();
            cmbProductSize.Text = dgvProduct.Rows[rowIndex].Cells[4].Value.ToString();
            txtBoxProductRate.Text = dgvProduct.Rows[rowIndex].Cells[5].Value.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Getting User ID from Form
            productBLL.id = Convert.ToInt32(txtBoxProductID.Text);

            bool success = productDAL.Delete(productBLL);

            if (success == true)
            {
                MessageBox.Show("Món " + txtBoxProductName.Text + " được xóa thành công khỏi hệ thống");
                clear();
            }
            else
            {
                MessageBox.Show("Không thể cập nhật món " + txtBoxProductName.Text);
            }

            //refreshing data grid view
            DataTable dataTable = productDAL.Select();
            dgvProduct.DataSource = dataTable;
        }

        private void txtBoxProductSearch_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtBoxProductSearch.Text;

            //check if the keywords has value 
            if (keywords != null)
            {
                //Show user based on keywords
                DataTable dataTable = productDAL.Search(keywords);
                dgvProduct.DataSource = dataTable;
            }
            else
            {
                //show all users from the database
                DataTable dataTable = productDAL.Select();
                dgvProduct.DataSource = dataTable;
            }
        }

        private void cmbProductCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
