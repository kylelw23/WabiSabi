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
    public partial class fUserDashBoard : Form
    {
        public fUserDashBoard()
        {
            InitializeComponent();
        }

        private void salesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void fUserDashBoard_FormClosed(object sender, FormClosedEventArgs e)
        {
            fLogin login = new fLogin();
            login.Show();
            this.Hide();
        }

        private void fUserDashBoard_Load(object sender, EventArgs e)
        {
            lblLoggedInUser.Text = fLogin.logged_in;
        }

        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fCustomer customer = new fCustomer();
            customer.ShowDialog();
        }

        private void purchaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fSale sale = new fSale();
            sale.ShowDialog();
        }

        private void quảnLýHóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fTransactions transactions = new fTransactions();
            transactions.ShowDialog();
        }
    }
}
