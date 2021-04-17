using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WabiSabi2.UI;

namespace WabiSabi2
{
    public partial class fAdminDashboard : Form
    {
        public fAdminDashboard()
        {
            InitializeComponent();
        }

        private void fAdminDashboard_Load(object sender, EventArgs e)
        {
            lblLoggedInUser.Text = fLogin.logged_in;
        }

        private void lblFooter_Click(object sender, EventArgs e)
        {

        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fUsers formUser = new fUsers();
            formUser.ShowDialog();
        }

        private void lblAppFName_Click(object sender, EventArgs e)
        {

        }

        private void fAdminDashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fCategory category = new fCategory();
            category.ShowDialog();
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fProduct product = new fProduct();
            product.ShowDialog();
        }

        private void trransactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fSale sale = new fSale();
            sale.ShowDialog();
        }

        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fCustomer customer = new fCustomer();
            customer.ShowDialog();
        }

        private void quảnLýHóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fTransactions transaction = new fTransactions();
            transaction.ShowDialog();
        }
    }
}
