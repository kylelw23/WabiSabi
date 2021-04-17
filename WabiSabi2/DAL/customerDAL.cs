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
    class customerDAL
    {
        static string myconnectingstring = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        #region Retrieve customer data from database
        public DataTable Select()
        {
            SqlConnection connection = new SqlConnection(myconnectingstring);

            DataTable dataTable = new DataTable();

            try
            {
                string sql = "SELECT * FROM tbl_customers";

                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                connection.Open();
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return dataTable;
        }

        #endregion

        #region Add customer data
        public bool Insert(customerBLL customer)
        {
            bool isSuccess = false;
            SqlConnection connection = new SqlConnection(myconnectingstring);

            try
            {
                string sql = "INSERT INTO tbl_customers (name, email, contact, address, added_Date, added_by) VALUES " +
                    "(@name, @email, @contact, @address, @added_date, @added_by)";

                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@name", customer.name);
                command.Parameters.AddWithValue("@email", customer.email);
                command.Parameters.AddWithValue("@contact", customer.contact);
                command.Parameters.AddWithValue("@address", customer.address);
                command.Parameters.AddWithValue("@added_date", customer.added_date);
                command.Parameters.AddWithValue("@added_by", customer.added_by);

                connection.Open();

                int rows = command.ExecuteNonQuery();

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return isSuccess;
        }
        #endregion

        #region Update customer data
        public bool Update(customerBLL customer)
        {
            bool isSuccess = false;
            SqlConnection connection = new SqlConnection(myconnectingstring);

            try
            {
                string sql = "UPDATE tbl_customers SET name = @name, email = @email, contact = @contact,address = @address,  added_date = @added_date, added_by = @added_by where id = @id";

                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@name", customer.name);
                command.Parameters.AddWithValue("@email", customer.email);
                command.Parameters.AddWithValue("@contact", customer.contact);
                command.Parameters.AddWithValue("@address", customer.address);
                command.Parameters.AddWithValue("@added_date", customer.added_date);
                command.Parameters.AddWithValue("@added_by", customer.added_by);
                command.Parameters.AddWithValue("@id", customer.id);

                connection.Open();

                int rows = command.ExecuteNonQuery();
                if (rows > 0)
                {
                    //Query is successfull
                    isSuccess = true;
                }
                else
                {
                    //Query is failed
                    isSuccess = false;
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
            return isSuccess;
        }
        #endregion

        #region Delete customer data
        public bool Delete(customerBLL customer)
        {
            bool isSuccess = false;
            SqlConnection connection = new SqlConnection(myconnectingstring);
            try
            {
                string sql = "DELETE from tbl_customers where id = @id";

                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@id", customer.id);

                connection.Open();

                int rows = command.ExecuteNonQuery();
                if (rows > 0)
                {
                    //Query is successfull
                    isSuccess = true;
                }
                else
                {
                    //Query is failed
                    isSuccess = false;
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
            return isSuccess;
        }
        #endregion

        #region Search for customer
        public DataTable Search(string keywords)
        {
            SqlConnection connection = new SqlConnection(myconnectingstring);

            DataTable dataTable = new DataTable();

            try
            {
                string sql = "SELECT * FROM tbl_customers WHERE id LIKE '%" + keywords + "%' OR name LIKE '%" + keywords + "%' OR email LIKE '%" + keywords + "%' OR contact LIKE '%" + keywords + "%'";

                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                connection.Open();
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return dataTable;
        }
        #endregion

        #region Method to search customer for transaction module
        public customerBLL SearchCustomerForTransaction(string keywords)
        {
            //Create an object for customerBLL class
            customerBLL customerBLL = new customerBLL();

            // Create a database connection
            SqlConnection connection = new SqlConnection(myconnectingstring);

            //Create a data table to hold the value temporarily
            DataTable dataTable = new DataTable();

            try
            {
                //Write a SQL query to search dealer or customer based on keywords
                string sql = "SELECT name, address, contact from tbl_customers where name LIKE '%"+keywords+"%' OR contact like '%"+keywords+"%'";

                //Create a SQL data adapter to execute the query
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);

                //Open the database connection
                connection.Open();

                //Transfer the data from SqlData Adapter to data table
                adapter.Fill(dataTable);

                // if we have values on dataTable we need to save it in dealer Customer BLL
                if(dataTable.Rows.Count > 0)
                {
                    customerBLL.name = dataTable.Rows[0]["name"].ToString();
                    customerBLL.email = dataTable.Rows[0]["addresss"].ToString();
                    customerBLL.contact = dataTable.Rows[0]["contact"].ToString();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return customerBLL;
        }
        #endregion
    }
}
