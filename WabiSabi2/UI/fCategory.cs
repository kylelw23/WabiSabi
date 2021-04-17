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
    public partial class fCategory : Form
    {
        public fCategory()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        categoryBLL categoryBLL = new categoryBLL();
        categoryDAL categoryDAL = new categoryDAL();

        userDAL userDAl = new userDAL();


        private void btnCategoryAdd_Click(object sender, EventArgs e)
        {

            //Getting data from UI
            categoryBLL.title = txtBoxCategoryTitle.Text;
            categoryBLL.description = txtBoxCategoryDescription.Text;
            categoryBLL.added_date = DateTime.Now;

            string logged_in_user = fLogin.logged_in;

            userBLL user = userDAl.GetIDFromUsername(logged_in_user);
            categoryBLL.added_by = user.username;

            //Inserting data into database
            bool success = categoryDAL.Insert(categoryBLL);
            //If data is succesfully insertted then the value of sucess will be true
            if (success == true)
            {
                //Data successfully inserted
                MessageBox.Show("Danh mục được thêm vào thành công");
                clear();
            }
            else
            {
                //Failed
                MessageBox.Show("Thêm danh mục thất bại");
            }
            //refreshing data grid view
            DataTable dataTable = categoryDAL.Select();
            dgvCategory.DataSource = dataTable;
        }

        private void clear()
        {
            txtBoxCategoryID.Text = "";
            txtBoxCategoryDescription.Text = "";
            txtBoxCategoryTitle.Text = "";
            txtBoxCategorySearch.Text = "";
        }

        private void fCategory_Load(object sender, EventArgs e)
        {
            DataTable dataTable = categoryDAL.Select();
            dgvCategory.DataSource = dataTable;
        }

        private void dgvCategory_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex;
            txtBoxCategoryID.Text = dgvCategory.Rows[rowIndex].Cells[0].Value.ToString();
            txtBoxCategoryTitle.Text = dgvCategory.Rows[rowIndex].Cells[1].Value.ToString();
            txtBoxCategoryDescription.Text = dgvCategory.Rows[rowIndex].Cells[2].Value.ToString();
        }

        private void dgvCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnCategoryUpdate_Click(object sender, EventArgs e)
        {
            //Get the values from the UI
            categoryBLL.id = Convert.ToInt32(txtBoxCategoryID.Text);
            categoryBLL.title = txtBoxCategoryTitle.Text;
            categoryBLL.description = txtBoxCategoryDescription.Text;
            categoryBLL.added_date = DateTime.Now;

            string logged_in_user = fLogin.logged_in;
            userBLL user = userDAl.GetIDFromUsername(logged_in_user);
            categoryBLL.added_by = user.username;

            //Updating data into database
            bool success = categoryDAL.Update(categoryBLL);

            if (success == true)
            {
                MessageBox.Show("Danh mục " + txtBoxCategoryTitle.Text + " được cập nhật thành công");
                clear();
            }
            else
            {
                MessageBox.Show("Không thể cập nhật danh mục " + txtBoxCategoryTitle.Text);
            }

            //refreshing data grid view
            DataTable dataTable = categoryDAL.Select();
            dgvCategory.DataSource = dataTable;
        }

        private void btnCategoryDelete_Click(object sender, EventArgs e)
        {
            //Getting User ID from Form
            categoryBLL.id = Convert.ToInt32(txtBoxCategoryID.Text);

            bool success = categoryDAL.Delete(categoryBLL);

            if (success == true)
            {
                MessageBox.Show("Danh mục " + txtBoxCategoryTitle.Text + " được xóa thành công khỏi hệ thống");
                clear();
            }
            else
            {
                MessageBox.Show("Không thể xóa danh mục " + txtBoxCategoryTitle.Text);
            }

            //refreshing data grid view
            DataTable dataTable = categoryDAL.Select();
            dgvCategory.DataSource = dataTable;
        }

        private void txtBoxCategorySearch_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtBoxCategorySearch.Text;

            //check if the keywords has value 
            if (keywords != null)
            {
                //Show user based on keywords
                DataTable dataTable = categoryDAL.Search(keywords);
                dgvCategory.DataSource = dataTable;
            }
            else
            {
                //show all users from the database
                DataTable dataTable = categoryDAL.Select();
                dgvCategory.DataSource = dataTable;
            }
        }
    }
}
