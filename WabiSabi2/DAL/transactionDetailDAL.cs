using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WabiSabi2.BLL;

namespace WabiSabi2.DAL
{
    class transactionDetailDAL
    {
        //Create Connection string
        static string myconnectingstring = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        #region Insert transaction method
        public bool InsertTransactionDetail(transactionDetailBLL transactionDetail)
        {
            //Create a boolean value and set its default value to false
            bool isSuccess = false;
            //Create a sqlConnection first
            SqlConnection connection = new SqlConnection(myconnectingstring);

            try
            {
                //SQL Query to insert transactions
                string sql = "INSERT INTO tbl_transaction_detail (product_id, rate, qty, total, customer_name, added_date, added_by)" +
                    "VALUES (@product_id, @rate, @qty, @total, @customer_name, @added_date, @added_by)";

                //SQL command to pass the value in the sql query
                SqlCommand cmd = new SqlCommand(sql, connection);

                cmd.Parameters.AddWithValue("@product_id", transactionDetail.product_id);
                cmd.Parameters.AddWithValue("@rate", transactionDetail.rate);
                cmd.Parameters.AddWithValue("@qty", transactionDetail.qty);
                cmd.Parameters.AddWithValue("@total", transactionDetail.total);
                cmd.Parameters.AddWithValue("@customer_name", transactionDetail.customer_name);
                cmd.Parameters.AddWithValue("@added_date", transactionDetail.added_date);
                cmd.Parameters.AddWithValue("@added_by", transactionDetail.added_by);

                //Open database connection
                connection.Open();

                //Execute the query
                int rows = cmd.ExecuteNonQuery();


                //If the query is executed successfully then the value will not be null else it will be null
                if (rows > 0)
                {
                    // Query executed successfully;
                    isSuccess = true;

                }
                else
                {
                    // Failed to execute query
                    isSuccess = false;
                }
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
            return isSuccess;
        }
        #endregion
    }
}
