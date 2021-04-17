using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WabiSabi2.DAL;

namespace WabiSabi2.UI
{
    public partial class fTransactions : Form
    {
        public fTransactions()
        {
            InitializeComponent();
        }
        transactionDAL transactionDAL = new transactionDAL();

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fTransactions_Load(object sender, EventArgs e)
        {
            //Display all the transactions
            DataTable dataTable = transactionDAL.DisplayAllTransactions();
            dgvTransactions.DataSource = dataTable;
        }

        private void cmbBoxTransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = cmbBoxTransactionType.Text;

            DataTable dataTable = transactionDAL.DisplayTransactionByType(type);
            dgvTransactions.DataSource = dataTable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dataTable = transactionDAL.DisplayAllTransactions();
            dgvTransactions.DataSource = dataTable;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            string date = dateTimePicker1.Text;

            DataTable dataTable = transactionDAL.DisplayTransactionByDate(date);
            dgvTransactions.DataSource = dataTable;
        }
    }
}
