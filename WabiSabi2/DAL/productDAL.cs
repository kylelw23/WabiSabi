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
    class productDAL
    {
        static string myconnectingstring = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        #region Get product from database
        public DataTable Select()
        {
            SqlConnection connection = new SqlConnection(myconnectingstring);

            DataTable dataTable = new DataTable();

            try
            {
                string sql = "SELECT * FROM tbl_products";

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

        #region Get product name, size, price from database
        public DataTable GetProductNameSizePrice()
        {
            SqlConnection connection = new SqlConnection(myconnectingstring);

            DataTable dataTable = new DataTable();

            try
            {
                string sql = "SELECT name, size, rate FROM tbl_products";

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


        #region insert data into table product
        public bool Insert(productBLL product)
        {
            bool isSuccess = false;
            SqlConnection connection = new SqlConnection(myconnectingstring);

            try
            {
                string sql = "INSERT INTO tbl_products (name, category, description, size, rate, qty, added_date, added_by) VALUES " +
                    "(@name, @category, @description, @size, @rate, @qty, @added_date, @added_by)";

                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@name", product.name);
                command.Parameters.AddWithValue("@category", product.category);
                command.Parameters.AddWithValue("@description", product.description);
                command.Parameters.AddWithValue("@size", product.size);
                command.Parameters.AddWithValue("@rate", product.rate);
                command.Parameters.AddWithValue("@qty", product.qty);
                command.Parameters.AddWithValue("@added_date", product.added_date);
                command.Parameters.AddWithValue("@added_by", product.added_by);

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

        #region Update data from table product
        public bool Update(productBLL product)
        {
            bool isSuccess = false;
            SqlConnection connection = new SqlConnection(myconnectingstring);

            try
            {
                string sql = "UPDATE tbl_products SET name = @name, category = @category, description = @description,size=@size, rate = @rate, qty= @qty, added_date = @added_date, added_by = @added_by where id = @id";

                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@name", product.name);
                command.Parameters.AddWithValue("@category", product.category);
                command.Parameters.AddWithValue("@description", product.description);
                command.Parameters.AddWithValue("@size", product.size);
                command.Parameters.AddWithValue("@rate", product.rate);
                command.Parameters.AddWithValue("@qty", product.qty);
                command.Parameters.AddWithValue("@added_date", product.added_date);
                command.Parameters.AddWithValue("@added_by", product.added_by);
                command.Parameters.AddWithValue("@id", product.id);

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

        #region Delete data from table product
        public bool Delete(productBLL product)
        {
            bool isSuccess = false;
            SqlConnection connection = new SqlConnection(myconnectingstring);
            try
            {
                string sql = "DELETE from tbl_products where id = @id";

                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@id", product.id);

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

        #region Search product from database
        public DataTable Search(string keywords)
        {
            SqlConnection connection = new SqlConnection(myconnectingstring);

            DataTable dataTable = new DataTable();

            try
            {
                string sql = "SELECT * FROM tbl_products WHERE id LIKE '%" + keywords + "%' OR name LIKE '%" + keywords + "%' OR category LIKE '%" + keywords + "%'";

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
        #region Method to search product in transaction module
        public productBLL GetProductsForTransaction(string keywords)
        {
            //Create an object for customerBLL class
            productBLL productBLL = new productBLL();

            // Create a database connection
            SqlConnection connection = new SqlConnection(myconnectingstring);

            //Create a data table to hold the value temporarily
            DataTable dataTable = new DataTable();

            try
            {
                //Write a SQL query to search dealer or customer based on keywords
                string sql = "SELECT name, rate, qty from tbl_products where name LIKE '%" + keywords + "%'";

                //Create a SQL data adapter to execute the query
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);

                //Open the database connection
                connection.Open();

                //Transfer the data from SqlData Adapter to data table
                adapter.Fill(dataTable);

                // if we have values on dataTable we need to save it in dealer Customer BLL
                if (dataTable.Rows.Count > 0)
                {
                    productBLL.name = dataTable.Rows[0]["name"].ToString();
                    productBLL.rate = Convert.ToInt32(dataTable.Rows[0]["rate"].ToString());
                    productBLL.qty = Convert.ToInt32(dataTable.Rows[0]["qty"].ToString());
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

            return productBLL;
        }
        #endregion
        #region Method to get product id based on product name
        public productBLL GetIDFromProductName(string productName)
        {
            productBLL productBLL = new productBLL();
            SqlConnection connection = new SqlConnection(myconnectingstring);

            DataTable dataTable = new DataTable();

            try
            {
                string sql = "SELECT id FROM tbl_products Where name='" + productName + "'";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                connection.Open();

                adapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    productBLL.id = Convert.ToInt32(dataTable.Rows[0]["id"].ToString());
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
            return productBLL;
        }
        #endregion

        #region method to get curent quantity from the database based on product ID
        public int GetProductQty(int productID)
        {
            SqlConnection connection = new SqlConnection(myconnectingstring);

            int qty = 0;

            DataTable dataTable = new DataTable();

            try
            {
                string sql = "SELECT qty from tbl_products WHERE id =" + productID;

                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                connection.Open();

                adapter.Fill(dataTable);

                if(dataTable.Rows.Count > 0)
                {
                    qty = Convert.ToInt32(dataTable.Rows[0]["qty"].ToString());
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
            return qty;
        }
        #endregion

        #region search for sale
        public DataTable SearchForSale(string keywords)
        {
            SqlConnection connection = new SqlConnection(myconnectingstring);

            DataTable dataTable = new DataTable();

            try
            {
                string sql = "SELECT name, size, rate FROM tbl_products WHERE id LIKE '%" + keywords + "%' OR name LIKE '%" + keywords + "%' OR category LIKE '%" + keywords + "%'";

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

        #region Method to display products based on products type
        public DataTable DisplayProductsByType(string category)
        {
            DataTable dataTable = new DataTable();
            //Create a sqlConnection first
            SqlConnection connection = new SqlConnection(myconnectingstring);

            try
            {
                //SQL Query to insert transactions
                string sql = "Select name,size,rate from tbl_products where category = '" + category + "'";

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
