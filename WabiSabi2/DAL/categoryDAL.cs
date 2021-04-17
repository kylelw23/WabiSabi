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
    class categoryDAL
    {
        //static string method for database connection string
        static string myconnectionstring = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        #region Select method
        public DataTable Select()
        {
            SqlConnection connection = new SqlConnection(myconnectionstring);

            DataTable dataTable = new DataTable();

            try
            {
                string sql = "SELECT * FROM tbl_category";

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

        #region Insert new category
        public bool Insert(categoryBLL category)
        {
            bool isSuccess = false;
            SqlConnection connection = new SqlConnection(myconnectionstring);

            try
            {
                string sql = "INSERT INTO tbl_category (title, description, added_date, added_by) VALUES " +
                    "(@title, @description, @added_date, @added_by)";

                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@title", category.title);
                command.Parameters.AddWithValue("@description", category.description);
                command.Parameters.AddWithValue("@added_date", category.added_date);
                command.Parameters.AddWithValue("@added_by", category.added_by);

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

        #region Update method
        public bool Update(categoryBLL category)
        {
            bool isSuccess = false;
            SqlConnection connection = new SqlConnection(myconnectionstring);

            try
            {
                string sql = "UPDATE tbl_category SET title = @title, description = @description, added_date = @added_date, added_by = @added_by where id = @id";

                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@title", category.title);
                command.Parameters.AddWithValue("@description", category.description);
                command.Parameters.AddWithValue("@added_date", category.added_date);
                command.Parameters.AddWithValue("@added_by", category.added_by);
                command.Parameters.AddWithValue("@id", category.id);

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

        #region Delete category method
        public bool Delete(categoryBLL  category)
        {
            bool isSuccess = false;
            SqlConnection connection = new SqlConnection(myconnectionstring);
            try
            {
                string sql = "DELETE from tbl_category where id = @id";

                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@id", category.id);

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

        #region Search category
        public DataTable Search(string keywords)
        {
            SqlConnection connection = new SqlConnection(myconnectionstring);

            DataTable dataTable = new DataTable();

            try
            {
                string sql = "SELECT * FROM tbl_category WHERE id LIKE '%" + keywords + "%' OR title LIKE '%" + keywords + "%' OR description LIKE '%" + keywords + "%'";

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
    }
}
