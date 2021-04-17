using DGVPrinterHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using WabiSabi2.BLL;
using WabiSabi2.DAL;

namespace WabiSabi2.UI
{
    public partial class fSale : Form
    {
        public fSale()
        {
            InitializeComponent();
        }

        customerDAL customerDAL = new customerDAL();
        productDAL productDAl = new productDAL();
        categoryDAL categoryDAL = new categoryDAL();

        DataTable transactionDT = new DataTable();
        userDAL userDAL = new userDAL();
        
        string receipt = "-----------------------------------------------------------------------------" + Environment.NewLine;

        transactionDAL transactionDAL = new transactionDAL();
        transactionDetailDAL transactionDetailDAL = new transactionDetailDAL();

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtBoxSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the keywords from the text box
            string keywords = txtBoxCustomerSearch.Text;

            if(keywords == "")
            {
                //Clear all the textboxes
                txtBoxCustomerName.Text = "";
                txtBoxCustomerAddress.Text = "";
                txtBoxCustomerContact.Text = "";
            }
            else
            {
                customerBLL customerBLL = customerDAL.SearchCustomerForTransaction(keywords);

                txtBoxCustomerName.Text = customerBLL.name;
                txtBoxCustomerAddress.Text = customerBLL.address;
                txtBoxCustomerContact.Text = customerBLL.contact;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtBoxProductSearch.Text;
            dgvProducts.Rows.Clear();
            //check if the keywords has value 
            if (keywords != null)
            {
                //Show user based on keywords
                DataTable dataTable = productDAl.SearchForSale(keywords);
                foreach (DataRow row in dataTable.Rows)
                {
                    dgvProducts.Rows.Add(row["name"].ToString(), row["size"].ToString(), row["rate"].ToString());
                }
            }
            else
            {
                //show all users from the database
                DataTable dataTable = productDAl.GetProductNameSizePrice();
                foreach (DataRow row in dataTable.Rows)
                {
                    dgvProducts.Rows.Add(row["name"].ToString(), row["size"].ToString(), row["rate"].ToString());
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
        }

        private void fSale_Load(object sender, EventArgs e)
        {
            DataTable dataTableHistoryOrder = transactionDAL.DisplayHistoryTransaction();
            foreach (DataRow row in dataTableHistoryOrder.Rows)
            {
                if(row["type"].ToString() == "Dùng t?i quán")
                {
                    dgvHistoryOrder.Rows.Add(row["id"].ToString(), row["customer_name"].ToString(), row["customer_contact"].ToString(), "Dùng tại quán", row["grandTotal"].ToString(), row["transaction_date"].ToString(), row["added_by"].ToString());
                }else if(row["type"].ToString() == "Mang di")
                {
                    dgvHistoryOrder.Rows.Add(row["id"].ToString(), row["customer_name"].ToString(), row["customer_contact"].ToString(), "Mang đi", row["grandTotal"].ToString(), row["transaction_date"].ToString(), row["added_by"].ToString());
                }
                else
                {
                    dgvHistoryOrder.Rows.Add(row["id"].ToString(), row["customer_name"].ToString(), row["customer_contact"].ToString(), "Giao hàng", row["grandTotal"].ToString(), row["transaction_date"].ToString(), row["added_by"].ToString());
                }

            }

            //Specify Columns for our trnsaction data table
            transactionDT.Columns.Add("Tên món");
            transactionDT.Columns.Add("Ghi chú");
            transactionDT.Columns.Add("Giá");
            transactionDT.Columns.Add("Số lượng");
            transactionDT.Columns.Add("Tổng giá tiền");

            dgvTopping.Rows.Add("Chân trâu đường đen", "5,000");
            dgvTopping.Rows.Add("Chân trâu hoàng kim", "5,000");
            dgvTopping.Rows.Add("Thạch củ năng", "5,000");
            dgvTopping.Rows.Add("Nha đam", "5,000");
            dgvTopping.Rows.Add("Rau câu", "5,000");
            dgvTopping.Rows.Add("Sương sáo", "5,000");
            dgvTopping.Rows.Add("Viên phô mai", "5,000");
            dgvTopping.Rows.Add("Khoai dẻo", "7,000");
            dgvTopping.Rows.Add("Hạt sen", "7,000");
            dgvTopping.Rows.Add("Pudding phô mai jerry", "10,000");
            dgvTopping.Rows.Add("Kem muối Machiato", "7,000");

            DataTable dataTable = productDAl.GetProductNameSizePrice();
            foreach (DataRow row in dataTable.Rows)
            {
                dgvProducts.Rows.Add( row["name"].ToString(), row["size"].ToString(), row["rate"].ToString());
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            string value = txtBoxDiscount.Text;
            if (value == "")
            {
                decimal subTotal = decimal.Parse(txtBoxSubTotal.Text);
                txtBoxGrandTotal.Text = txtBoxSubTotal.Text;
            }
            else
            {
                decimal subTotal = decimal.Parse(txtBoxSubTotal.Text);


                decimal discount = decimal.Parse(txtBoxDiscount.Text);

                //Calculate the grand total based on discount
                decimal grandTotal = subTotal * ((100 - discount) / 100);

                //Display the grandTotal in text box
                txtBoxGrandTotal.Text = Convert.ToInt32(grandTotal).ToString("##,#", CultureInfo.CreateSpecificCulture("en-US"));
            }
        }

        private void txtBoxReceived_TextChanged(object sender, EventArgs e)
        {
            if(Convert.ToInt32(txtBoxGrandTotal.Text.Replace(",","")) != 0)
            {
                //Get the paid amount and grand total
                int grandTotal = Convert.ToInt32(txtBoxGrandTotal.Text.Replace(",", ""));
                int paidAmount = 0;
                if(txtBoxReceived.Text == "")
                {
                    paidAmount = 0;
                }
                else
                {
                    paidAmount = Convert.ToInt32(txtBoxReceived.Text);
                }

                int returnAmount = paidAmount - grandTotal;

                //Display the return amount
                txtBoxReturn.Text = returnAmount.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US"));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(cmbBoxTransactionType.Text == "Giao hàng" && txtBoxCustomerAddress.Text == "")
            {
                MessageBox.Show("Bạn chưa điền địa chỉ giao hàng của khách!");
            }
            else if(txtBoxCustomerName.Text == "")
            {
                MessageBox.Show("Bạn quên điền thông tin khách hàng");
            }
            else if(Convert.ToInt32(txtBoxSubTotal.Text.Replace(",","")) ==0)
            {
                MessageBox.Show("Bạn chưa thêm món ăn vào đơn ");
            }
            else
            {
                int totalRows = dgvAddedProducts.Rows.Count;
          
                int verticalLength = (totalRows*3 + 23)*13;
                printPreviewDialog1.Document = printDocument1;
                printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 300, verticalLength);
                printPreviewDialog1.ShowDialog();
                //Get the value from Purchase sales
                transactionBLL transactionBLL = new transactionBLL();

                //Get customer details

                transactionBLL.customer_name = txtBoxCustomerName.Text;
                transactionBLL.customer_contact = txtBoxCustomerContact.Text;
                transactionBLL.type = cmbBoxTransactionType.Text;
                transactionBLL.grandTotal = Convert.ToInt32(txtBoxGrandTotal.Text.Replace(",",""));
                transactionBLL.transaction_date = DateTime.Now;
                transactionBLL.tax = 0;
                transactionBLL.discount = Convert.ToInt32(txtBoxDiscount.Text);
                transactionBLL.billText = receipt;

                string username = fLogin.logged_in;

                transactionBLL.added_by = username;
                transactionBLL.transactionDetails = transactionDT;
                //Lets create a boolean var and set its value to false
                bool success = false;

            
                    int transactionID = -1;
                    //Create a boolean value and insert transaction
                    bool transactionInsert = transactionDAL.Insert_Transaction(transactionBLL);

                    //Use for loop to insert transaction details
                    for(int i = 0; i < transactionDT.Rows.Count; i++)
                    {
                        //Get all the details of the product
                        transactionDetailBLL transactionDetailBLL = new transactionDetailBLL();
                        //Get the product name and convert it to ID
                        string ProductName = transactionDT.Rows[i][0].ToString();
                        productBLL productBLL = productDAl.GetIDFromProductName(ProductName);

                        transactionDetailBLL.product_id = productBLL.id;
                        transactionDetailBLL.product_note = transactionDT.Rows[i][1].ToString();
                        transactionDetailBLL.rate = Convert.ToInt32(transactionDT.Rows[i][2].ToString().Replace(",",""));
                        transactionDetailBLL.qty = Convert.ToInt32(transactionDT.Rows[i][3].ToString());
                        transactionDetailBLL.total = Convert.ToInt32(transactionDT.Rows[i][4].ToString().Replace(",", ""));
                        transactionDetailBLL.customer_name = txtBoxCustomerName.Text;
                        transactionDetailBLL.customer_contact = txtBoxCustomerContact.Text;
                        transactionDetailBLL.added_date = DateTime.Now;
                        transactionDetailBLL.added_by = username;

                        //Insert transaction details inside the database
                        bool transactionDetailInsert = transactionDetailDAL.InsertTransactionDetail(transactionDetailBLL);
                        success = transactionInsert && transactionDetailInsert;
                    }

                        if(success = true)
                        {

                            MessageBox.Show("Hóa đơn hoàn tất");
                            //Print bill
                            receipt = "-----------------------------------------------------------------------------" + Environment.NewLine;
                            clear();
                        }
                        else
                        {
                            //transaction failed
                            MessageBox.Show("Hóa đơn thất bại");
                        }
                    
            }
        }
        private void clear()
        {

            dgvAddedProducts.DataSource = null;
            dgvAddedProducts.Rows.Clear();
            txtBoxCustomerContact.Text = "";
            txtBoxCustomerAddress.Text = "";
            txtBoxCustomerName.Text = "";
            txtBoxCustomerSearch.Text = "";
            txtBoxDiscount.Text = "0";
            txtBoxGrandTotal.Text = "0";
            txtBoxProductSearch.Text = "";
            txtBoxReceived.Text = "";
            txtBoxReturn.Text = "";
            txtBoxSubTotal.Text = "0";
            cmbBoxTransactionType.Text = "Dùng tại quán";

            dgvHistoryOrder.DataSource = null;
            dgvHistoryOrder.Rows.Clear();
            DataTable dataTableHistoryOrder = transactionDAL.DisplayHistoryTransaction();
            foreach (DataRow row in dataTableHistoryOrder.Rows)
            {
                if (row["type"].ToString() == "Dùng t?i quán")
                {
                    dgvHistoryOrder.Rows.Add(row["id"].ToString(), row["customer_name"].ToString(), row["customer_contact"].ToString(), "Dùng tại quán", row["grandTotal"].ToString(), row["transaction_date"].ToString(), row["added_by"].ToString());
                }
                else if (row["type"].ToString() == "Mang di")
                {
                    dgvHistoryOrder.Rows.Add(row["id"].ToString(), row["customer_name"].ToString(), row["customer_contact"].ToString(), "Mang đi", row["grandTotal"].ToString(), row["transaction_date"].ToString(), row["added_by"].ToString());
                }
                else
                {
                    dgvHistoryOrder.Rows.Add(row["id"].ToString(), row["customer_name"].ToString(), row["customer_contact"].ToString(), "Giao hàng", row["grandTotal"].ToString(), row["transaction_date"].ToString(), row["added_by"].ToString());
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(dgvAddedProducts.CurrentCell == null)
            {
                MessageBox.Show("Hãy chọn món bạn cần xóa!");
            }
            else
            {

                int selectedRow = dgvAddedProducts.CurrentCell.RowIndex;
                if (dgvAddedProducts.Rows[selectedRow].Cells[4].Value == null)
                {
                    MessageBox.Show("Hãy chọn món bạn cần xóa!");
                }
                else
                {
                    int deletedItemMoney = Convert.ToInt32(dgvAddedProducts.Rows[selectedRow].Cells[4].Value.ToString().Replace(",", ""));

                    int subTotal = Convert.ToInt32(txtBoxSubTotal.Text.Replace(",", ""));
                    subTotal = subTotal - deletedItemMoney;

                    //Display the subtotal
                    if (txtBoxSubTotal.Text == "")
                    {
                        txtBoxSubTotal.Text = "0";
                    }
                    else
                    {
                        txtBoxSubTotal.Text = subTotal.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US"));
                    }

                    //Display the temporary total
                    if (txtBoxGrandTotal.Text == "")
                    {
                        txtBoxGrandTotal.Text = "0";
                    }
                    else
                    {
                        txtBoxGrandTotal.Text = subTotal.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US"));

                    }
                        int paidAmount = 0;
                        if (txtBoxReceived.Text == "")
                        {
                            paidAmount = 0;
                        }
                        else
                        {
                            paidAmount = Convert.ToInt32(txtBoxReceived.Text);
                        }

                        int returnAmount = paidAmount - subTotal;

                        //Display the return amount
                        txtBoxReturn.Text = returnAmount.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US"));


                    dgvAddedProducts.Rows.RemoveAt(selectedRow);
                }
            }
        }

        private void dgvAddedProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
        }

        private void dgvTopping_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            //Get product name, rate and qty customer wants to buy
            if (dgvTopping.Rows[rowIndex].Cells[0].Value == null)
            {
                MessageBox.Show("Hãy chọn topping có tên!");
            }
            else
            {
                string toppingName = dgvTopping.Rows[rowIndex].Cells[0].Value.ToString();

                string toppingRate = dgvTopping.Rows[rowIndex].Cells[1].Value.ToString(); ;
                toppingRate = toppingRate.Replace(",", "");
                int rate = Convert.ToInt32(toppingRate);


                int total = rate * 1; //Total = RatexQty

                //Display the subtotal in text box
                //Get the subtotal value from text box
                if (txtBoxSubTotal.Text == "")
                {
                    txtBoxSubTotal.Text = "0";
                }
                int subTotal = Convert.ToInt32(txtBoxSubTotal.Text.Replace(",", ""));
                subTotal = subTotal + total;

                dgvAddedProducts.Rows.Add("(Topping) " + toppingName, "", rate.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US")), 1, total.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US")));



                //Display the subtotal
                txtBoxSubTotal.Text = subTotal.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US"));

                //Display the temporary total
                txtBoxGrandTotal.Text = subTotal.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US"));

                //Clear the text boxes
                txtBoxProductSearch.Text = "";

            }
        }

        private void dataGridView1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex;
            //Get product name, rate and qty customer wants to buy
            if (dgvProducts.Rows[rowIndex].Cells[0].Value == null)
            {
                MessageBox.Show("Hãy chọn món có tên!");
            }
            else
            {
                string productName = dgvProducts.Rows[rowIndex].Cells[0].Value.ToString();
                string size = dgvProducts.Rows[rowIndex].Cells[1].Value.ToString();

                string productRate = dgvProducts.Rows[rowIndex].Cells[2].Value.ToString(); ;
                productRate = productRate.Replace(",", "");
                int rate = Convert.ToInt32(productRate);


                int total = rate * 1; //Total = RatexQty

                //Display the subtotal in text box
                //Get the subtotal value from text box
                if (txtBoxSubTotal.Text == "")
                {
                    txtBoxSubTotal.Text = "0";
                }
                int subTotal = Convert.ToInt32(txtBoxSubTotal.Text.Replace(",", ""));
                subTotal = subTotal + total;

                //Check whether the product is selected or not
                if (productName == "")
                {
                    //Display error message
                    MessageBox.Show("Hãy chọn món. Vui lòng thử lại");
                }
                else
                {
                    dgvAddedProducts.Rows.Add(productName + " (" + size + ")", cmbSugar.Text, rate.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US")), 1, total.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US")));
                    transactionDT.Rows.Add(productName + "(" + size + ")", cmbSugar.Text, rate.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US")), 1, total.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US")));

               

                    //Display the subtotal
                    txtBoxSubTotal.Text = subTotal.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US"));

                    //Display the temporary total
                    txtBoxGrandTotal.Text = subTotal.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US"));
                    if (Convert.ToInt32(txtBoxReceived.Text.Replace(",", "")) != 0)
                    {
                        //Get the paid amount and grand total
                        int grandTotal1 = Convert.ToInt32(txtBoxGrandTotal.Text.Replace(",", ""));
                        int paidAmount = 0;
                        if (txtBoxReceived.Text == "")
                        {
                            paidAmount = 0;
                        }
                        else
                        {
                            paidAmount = Convert.ToInt32(txtBoxReceived.Text);
                        }

                        int returnAmount = paidAmount - grandTotal1;

                        //Display the return amount
                        txtBoxReturn.Text = returnAmount.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US"));
                    }
                    //Clear the text boxes
                    txtBoxProductSearch.Text = "";
                }

            }
        }

        private void dgvTopping_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex;
            //Get product name, rate and qty customer wants to buy
            if (dgvTopping.Rows[rowIndex].Cells[0].Value == null)
            {
                MessageBox.Show("Hãy chọn topping có tên!");
            }
            else
            {
                string toppingName = dgvTopping.Rows[rowIndex].Cells[0].Value.ToString();

                string toppingRate = dgvTopping.Rows[rowIndex].Cells[1].Value.ToString(); ;
                toppingRate = toppingRate.Replace(",", "");
                int rate = Convert.ToInt32(toppingRate);


                int total = rate * 1; //Total = RatexQty

                //Display the subtotal in text box
                //Get the subtotal value from text box
                if(txtBoxSubTotal.Text == "")
                {
                    txtBoxSubTotal.Text = "0";
                }
                int subTotal = Convert.ToInt32(txtBoxSubTotal.Text.Replace(",", ""));
                subTotal = subTotal + total;

                dgvAddedProducts.Rows.Add("(Topping) "+toppingName, "", rate.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US")), 1, total.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US")));
                


                //Display the subtotal
                txtBoxSubTotal.Text = subTotal.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US"));

                //Display the temporary total
                txtBoxGrandTotal.Text = subTotal.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US"));

                //Clear the text boxes
                txtBoxProductSearch.Text = "";

                if (Convert.ToInt32(txtBoxReceived.Text.Replace(",", "")) != 0)
                {
                    //Get the paid amount and grand total
                    int grandTotal1 = Convert.ToInt32(txtBoxGrandTotal.Text.Replace(",", ""));
                    int paidAmount = 0;
                    if (txtBoxReceived.Text == "")
                    {
                        paidAmount = 0;
                    }
                    else
                    {
                        paidAmount = Convert.ToInt32(txtBoxReceived.Text);
                    }

                    int returnAmount = paidAmount - grandTotal1;

                    //Display the return amount
                    txtBoxReturn.Text = returnAmount.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US"));
                }
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string username = fLogin.logged_in;
            receipt += ("\t" + "          Wabi Sabi Coffee & Milk Tea" + Environment.NewLine);
            receipt += ("                     345 Ung Văn Khiêm, P Mỹ Xuyên" + Environment.NewLine+ "\t          TP Long Xuyên, An Giang" + Environment.NewLine);
            receipt += ("\t                 " + "Tel: 0903.393434" + Environment.NewLine);
            receipt += ("-----------------------------------------------------------------------------" + Environment.NewLine);
            receipt += ("\t" + "                PHIẾU THANH TOÁN" + Environment.NewLine);
            receipt += Environment.NewLine;
            receipt += ("Ngày: " + DateTime.Now + Environment.NewLine);
            receipt += ("Thực hiện bởi: " + username + Environment.NewLine);
            receipt += ("Loại hóa đơn: " + cmbBoxTransactionType.Text + Environment.NewLine);
            receipt += ("Khách hàng: " + txtBoxCustomerName.Text + Environment.NewLine);
            if (cmbBoxTransactionType.Text == "Giao hàng")
            {
                receipt += "Số điện thoại:" + txtBoxCustomerContact.Text + "%" + Environment.NewLine;
                receipt += "Địa chỉ:" + txtBoxCustomerAddress.Text + "%" + Environment.NewLine;
            }
            receipt += ("-----------------------------------------------------------------------------" + Environment.NewLine);
            receipt += PrintRow("Tên món", "Đ.Giá", "SL", "T.Tiền") + Environment.NewLine;
            receipt += ("-----------------------------------------------------------------------------" + Environment.NewLine);
            foreach (DataGridViewRow row in dgvAddedProducts.Rows)
            {
                if(row.Cells[0].Value == null)
                {
                    break;
                }
                receipt += PrintRow(row.Cells[0].Value.ToString(), row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString(), row.Cells[4].Value.ToString()) + Environment.NewLine;
            }
            receipt += ("-----------------------------------------------------------------------------" + Environment.NewLine);
            receipt += "Thành tiền:\t\t\t              " + txtBoxSubTotal.Text + Environment.NewLine;
            if (Convert.ToInt32(txtBoxDiscount.Text) > 0)
            {
                receipt += "Giảm giá(%):\t\t\t              " + txtBoxDiscount.Text+"%" + Environment.NewLine;
            }
            receipt += ("-----------------------------------------------------------------------------" + Environment.NewLine);
            receipt += "Thành tiền:\t\t\t              " + txtBoxGrandTotal.Text + Environment.NewLine;
            receipt += "Tiền khách đưa:\t\t\t              " + txtBoxReceived.Text + Environment.NewLine;
            receipt += "Tiền thừa:\t\t\t              " + txtBoxReturn.Text + Environment.NewLine;
            if (cmbBoxTransactionType.Text == "Dùng tại quán")
            {
                receipt += ("-----------------------------------------------------------------------------" + Environment.NewLine);

                receipt += ("                        Password wifi: wabisabi94") + Environment.NewLine;
            }

            receipt += ("-----------------------------------------------------------------------------" + Environment.NewLine);
            receipt += ("                Chúc quý khách vui vẻ và hẹn gặp lại!" + Environment.NewLine);
            receipt += ("" + Environment.NewLine);
            receipt += ("" + Environment.NewLine);
            receipt += ("" + Environment.NewLine);
            receipt += ("" + Environment.NewLine);
            receipt += ("" + Environment.NewLine);
            receipt += ("" + Environment.NewLine);
            receipt += ("" + Environment.NewLine);
            receipt += ("" + Environment.NewLine);
            receipt += ("" + Environment.NewLine);
            e.Graphics.DrawString(receipt, new Font("Arial", 8, FontStyle.Regular), Brushes.Black, 5, 0);
        }

        static string PrintRow(params string[] columns)
        {
            int width = (75 - columns.Length) / columns.Length;
            string row = " ";

            foreach (string column in columns)
            {
                if (column.Length < 10)
                {
                    row += AlignCentre2(column, width) + " ";
                }
                else
                {
                    row += AlignCentre(column, width) + " ";

                }
            }

            return row;
        }

        static string AlignCentre(string text, int width)
        {
            string preText = Regex.Replace(text, "(.{" + width + "})", "$1" + Environment.NewLine);
            int lastIndex = preText.LastIndexOf('\n');
            string lastWord = preText.Substring(preText.LastIndexOf('\n') + 1);
            if (lastIndex > 0)
            {
                return preText += new string(' ', 22 - lastWord.Length);
            }else
            {
                return preText;
            }
            /* text = text.Length > width ? text.Substring(0, index) + "\n" + text.Substring(text.Length - index, text.Length-1) : text;*//*
             string preText = Regex.Replace(text, "(.{" + width + "})", "$1" + Environment.NewLine);
             int lastIndex = preText.LastIndexOf('\n');
             if (lastIndex > 0)
             {
               *//*  text = preText.PadRight(preText.Length + width - (preText.Substring(preText.Length - lastIndex).Length));*//*
                 text += new string(' ', width - (preText.Substring(preText.Length - lastIndex - 1).Length));
             }
             else
             {
                 text = text.Length > width ? text.Substring(0, width - 3) : text;
             }

             if (string.IsNullOrEmpty(text))
             {
                 return new string(' ', width);
             }
             else
             {
                 return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
             }*/
        }
        static string AlignCentre2(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
        string billText = "";
        private void button3_Click(object sender, EventArgs e)
        {
            int selectedRow = dgvHistoryOrder.CurrentCell.RowIndex;
            if (dgvHistoryOrder.Rows[selectedRow].Cells[0].Value == null)
            {
                MessageBox.Show("Hãy chọn hóa đơn bạn cần in!");
            }
            else
            {
                string id = dgvHistoryOrder.Rows[selectedRow].Cells[0].Value.ToString();
                billText = transactionDAL.GetBill(id);
                printPreviewDialog1.Document = printDocument2;
                printDocument2.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 300, 800);
                printPreviewDialog1.ShowDialog();

            }
        }
        private int DrawWrapped(string text, Font f, Point location, Size maxSize, Graphics g)
        {
            // how much space is needed
            var neededRect = g.MeasureString(text, f, maxSize.Width - location.X);
            var rect = new Rectangle(location, neededRect.ToSize());
            g.DrawString(text, f, Brushes.Black, rect, StringFormat.GenericDefault);
            return (int)Math.Ceiling(neededRect.Height);
        }
        private void printDocument2_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(billText, new Font("Arial", 8, FontStyle.Regular), Brushes.Black, 0, 0);
        }

        private void cmbProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvProducts.DataSource = null;
            dgvProducts.Rows.Clear();
            string type = cmbProductType.Text;

            DataTable dataTable = productDAl.DisplayProductsByType(type);
            foreach (DataRow row in dataTable.Rows)
            {
                dgvProducts.Rows.Add(row["name"].ToString(), row["size"].ToString(), row["rate"].ToString());
            }
        }

        private void dgvProducts_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            //Get product name, rate and qty customer wants to buy
            if (dgvProducts.Rows[rowIndex].Cells[0].Value == null)
            {
                MessageBox.Show("Hãy chọn món có tên!");
            }
            else
            {
                string productName = dgvProducts.Rows[rowIndex].Cells[0].Value.ToString();
                string size = dgvProducts.Rows[rowIndex].Cells[1].Value.ToString();

                string productRate = dgvProducts.Rows[rowIndex].Cells[2].Value.ToString(); ;
                productRate = productRate.Replace(",", "");
                int rate = Convert.ToInt32(productRate);


                int total = rate * 1; //Total = RatexQty

                //Display the subtotal in text box
                //Get the subtotal value from text box
                if (txtBoxSubTotal.Text == "")
                {
                    txtBoxSubTotal.Text = "0";
                }
                int subTotal = Convert.ToInt32(txtBoxSubTotal.Text.Replace(",", ""));
                subTotal = subTotal + total;

                //Check whether the product is selected or not
                if (productName == "")
                {
                    //Display error message
                    MessageBox.Show("Hãy chọn món. Vui lòng thử lại");
                }
                else
                {
                    dgvAddedProducts.Rows.Add(productName + " (" + size + ")", cmbSugar.Text, rate.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US")), 1, total.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US")));
                    transactionDT.Rows.Add(productName + "(" + size + ")", cmbSugar.Text, rate.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US")), 1, total.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US")));



                    //Display the subtotal
                    txtBoxSubTotal.Text = subTotal.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US"));

                    //Display the temporary total
                    txtBoxGrandTotal.Text = subTotal.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US"));

                    //Clear the text boxes
                    txtBoxProductSearch.Text = "";
                }
                if (Convert.ToInt32(txtBoxReceived.Text.Replace(",", "")) != 0)
                {
                    //Get the paid amount and grand total
                    int grandTotal1 = Convert.ToInt32(txtBoxGrandTotal.Text.Replace(",", ""));
                    int paidAmount = 0;
                    if (txtBoxReceived.Text == "")
                    {
                        paidAmount = 0;
                    }
                    else
                    {
                        paidAmount = Convert.ToInt32(txtBoxReceived.Text);
                    }

                    int returnAmount = paidAmount - grandTotal1;

                    //Display the return amount
                    txtBoxReturn.Text = returnAmount.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US"));
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = cmbBoxTransactionType.Text;

            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtBoxHistoryOrderSearch.Text;
            dgvHistoryOrder.DataSource = null;
            dgvHistoryOrder.Rows.Clear();
            //check if the keywords has value 
            if (keywords != null)
            {
                //Show user based on keywords
                DataTable dataTableHistoryOrder = transactionDAL.SearchHistoryTransactions(keywords);
                foreach (DataRow row in dataTableHistoryOrder.Rows)
                {
                    if (row["type"].ToString() == "Dùng t?i quán")
                    {
                        dgvHistoryOrder.Rows.Add(row["id"].ToString(), row["customer_name"].ToString(), row["customer_contact"].ToString(), "Dùng tại quán", row["grandTotal"].ToString(), row["transaction_date"].ToString(), row["added_by"].ToString());
                    }
                    else if (row["type"].ToString() == "Mang di")
                    {
                        dgvHistoryOrder.Rows.Add(row["id"].ToString(), row["customer_name"].ToString(), row["customer_contact"].ToString(), "Mang đi", row["grandTotal"].ToString(), row["transaction_date"].ToString(), row["added_by"].ToString());
                    }
                    else
                    {
                        dgvHistoryOrder.Rows.Add(row["id"].ToString(), row["customer_name"].ToString(), row["customer_contact"].ToString(), "Giao hàng", row["grandTotal"].ToString(), row["transaction_date"].ToString(), row["added_by"].ToString());
                    }

                }
            }
            else
            {
                //show all users from the database
                DataTable dataTableHistoryOrder = transactionDAL.DisplayHistoryTransaction();
                foreach (DataRow row in dataTableHistoryOrder.Rows)
                {
                    if (row["type"].ToString() == "Dùng t?i quán")
                    {
                        dgvHistoryOrder.Rows.Add(row["id"].ToString(), row["customer_name"].ToString(), row["customer_contact"].ToString(), "Dùng tại quán", row["grandTotal"].ToString(), row["transaction_date"].ToString(), row["added_by"].ToString());
                    }
                    else if (row["type"].ToString() == "Mang di")
                    {
                        dgvHistoryOrder.Rows.Add(row["id"].ToString(), row["customer_name"].ToString(), row["customer_contact"].ToString(), "Mang đi", row["grandTotal"].ToString(), row["transaction_date"].ToString(), row["added_by"].ToString());
                    }
                    else
                    {
                        dgvHistoryOrder.Rows.Add(row["id"].ToString(), row["customer_name"].ToString(), row["customer_contact"].ToString(), "Giao hàng", row["grandTotal"].ToString(), row["transaction_date"].ToString(), row["added_by"].ToString());
                    }

                }
            }
        }

        private void dgvAddedProducts_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if(rowIndex != -1 )
            {
                int productRate = Convert.ToInt32(dgvAddedProducts.Rows[rowIndex].Cells[2].Value.ToString().Replace(",", ""));
                int productNumber = Convert.ToInt32(dgvAddedProducts.Rows[rowIndex].Cells[3].Value.ToString().Replace(",", ""));
                int productTotal = productRate * productNumber;
                dgvAddedProducts.Rows[rowIndex].Cells[4].Value = productTotal.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US"));
                int subTotal = 0;
                //Recalculate the whole bill
                foreach (DataGridViewRow row in dgvAddedProducts.Rows)
                {
                    if (row.Cells[0].Value == null)
                    {
                        break;
                    }
                    subTotal += Convert.ToInt32(row.Cells[4].Value.ToString().Replace(",", ""));
                }


                //Display the subtotal
                txtBoxSubTotal.Text = subTotal.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US"));

                decimal discount = decimal.Parse(txtBoxDiscount.Text);

                //Calculate the grand total based on discount
                decimal grandTotal = subTotal * ((100 - discount) / 100);

                //Display the grandTotal in text box
                txtBoxGrandTotal.Text = Convert.ToInt32(grandTotal).ToString("##,#", CultureInfo.CreateSpecificCulture("en-US"));


                if (Convert.ToInt32(txtBoxReceived.Text.Replace(",", "")) != 0)
                {
                    //Get the paid amount and grand total
                    int grandTotal1 = Convert.ToInt32(txtBoxGrandTotal.Text.Replace(",", ""));
                    int paidAmount = 0;
                    if (txtBoxReceived.Text == "")
                    {
                        paidAmount = 0;
                    }
                    else
                    {
                        paidAmount = Convert.ToInt32(txtBoxReceived.Text);
                    }

                    int returnAmount = paidAmount - grandTotal1;

                    //Display the return amount
                    txtBoxReturn.Text = returnAmount.ToString("##,#", CultureInfo.CreateSpecificCulture("en-US"));
                }
            }
        }
    }
}
