using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WabiSabi2.BLL;

namespace WabiSabi2.DAL
{
    class transactionDAL
    {
        //Create a connection string
        static string myconnectingstring = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        #region Insert transaction method
        public bool Insert_Transaction(transactionBLL transaction)
        {
            //Create a boolean value and set its default value to false
            bool isSuccess = false;
            //Set the out transactionID value to negative -1
            //Create a sqlConnection first
            SqlConnection connection = new SqlConnection(myconnectingstring);

            try
            {
                //SQL Query to insert transactions
                string sql = "INSERT INTO tbl_transactions (type, customer_name, customer_contact, grandTotal, transaction_date, tax, discount, added_by, billText)"+
                    "VALUES (@type, @customer_name, @customer_contact, @grandTotal, @transaction_date, @tax, @discount, @added_by, @billText);";

                //SQL command to pass the value in the sql query
                SqlCommand cmd = new SqlCommand(sql, connection);

                cmd.Parameters.AddWithValue("@type", transaction.type);
                cmd.Parameters.AddWithValue("@customer_name", transaction.customer_name);
                cmd.Parameters.AddWithValue("@customer_contact", transaction.customer_contact);
                cmd.Parameters.AddWithValue("@grandTotal", transaction.grandTotal);
                cmd.Parameters.AddWithValue("@transaction_date", transaction.transaction_date);
                cmd.Parameters.AddWithValue("@tax", transaction.tax);
                cmd.Parameters.AddWithValue("@discount", transaction.discount);
                cmd.Parameters.AddWithValue("@added_by", transaction.added_by);
                cmd.Parameters.AddWithValue("@billText", transaction.billText);

                //Open database connection
                connection.Open();

                int rows = cmd.ExecuteNonQuery();

                // if the query is executed successfully then the value of rows will be greater than 0 else it will be less than 0
                if (rows > 0)
                {
                    //query is successfull
                    isSuccess = true;
                }
                else
                {
                    //query is failed
                    isSuccess = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Close the connection
                connection.Close();
            }
            return isSuccess;
        }
        #endregion

        #region Method to display all the transaction
        public DataTable DisplayAllTransactions()
        {
            DataTable dataTable = new DataTable();
            //Create a sqlConnection first
            SqlConnection connection = new SqlConnection(myconnectingstring);

            try
            {
                //SQL Query to insert transactions
                string sql = "Select * from tbl_transactions";

                //SQL command to pass the value in the sql query
                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                //Open database connection
                connection.Open();
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Close the connection
                connection.Close();
            }
            return dataTable;
        }
        #endregion

        #region Method to display transactions based on transaction type
        public DataTable DisplayTransactionByType(string type)
        {
            DataTable dataTable = new DataTable();
            //Create a sqlConnection first
            SqlConnection connection = new SqlConnection(myconnectingstring);

            try
            {
                //SQL Query to insert transactions
                string sql = "Select * from tbl_transactions where type = '"+type+"'";

                //SQL command to pass the value in the sql query
                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                //Open database connection
                connection.Open();
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Close the connection
                connection.Close();
            }
            return dataTable;
        }
        #endregion


        #region method to show all history transactions with customer name, number, order type, total...
        public DataTable DisplayHistoryTransaction()
        {
            DataTable dataTable = new DataTable();
            //Create a sqlConnection first
            SqlConnection connection = new SqlConnection(myconnectingstring);

            try
            {
                //SQL Query to insert transactions
                string sql = "Select id, customer_name, customer_contact, type, grandTotal, transaction_date, added_by from tbl_transactions";

                //SQL command to pass the value in the sql query
                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                //Open database connection
                connection.Open();
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Close the connection
                connection.Close();
            }
            return dataTable;
        }
        #endregion

        #region retrieve the bill from the database
        public string GetBill(string id)
        {
            string billText = "";
            SqlConnection connection = new SqlConnection(myconnectingstring);

            DataTable dataTable = new DataTable();

            try
            {
                string sql = "SELECT billText FROM tbl_transactions Where id='" + id + "'";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                connection.Open();

                adapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    billText = dataTable.Rows[0]["billText"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return billText;
        }
        #endregion

        #region method to show all history transactions with customer name, number, order type, total...
        public DataTable SearchHistoryTransactions(string keywords)
        {
            DataTable dataTable = new DataTable();
            //Create a sqlConnection first
            SqlConnection connection = new SqlConnection(myconnectingstring);

            try
            {
                //SQL Query to insert transactions
                string sql = "Select id, customer_name, customer_contact, type, grandTotal, transaction_date, added_by from tbl_transactions where customer_name LIKE '%" + keywords + "%'";

                //SQL command to pass the value in the sql query
                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                //Open database connection
                connection.Open();
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Close the connection
                connection.Close();
            }
            return dataTable;
        }
        #endregion

        #region Method to display transactions based on transaction type
        public DataTable DisplayTransactionByDate(string date)
        {
            DataTable dataTable = new DataTable();
            //Create a sqlConnection first
            SqlConnection connection = new SqlConnection(myconnectingstring);

            try
            {
                //SQL Query to insert transactions
                string sql = "Select * from tbl_transactions where transaction_date = '" + date + "'";

                //SQL command to pass the value in the sql query
                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                //Open database connection
                connection.Open();
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Close the connection
                connection.Close();
            }
            return dataTable;
        }
        #endregion
    }
}
