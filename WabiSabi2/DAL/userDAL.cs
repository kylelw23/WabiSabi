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
    class userDAL
    {
        static string myconnectingstring = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        #region Get Data from Database
        public DataTable Select()
        {
            SqlConnection connection = new SqlConnection(myconnectingstring);

            DataTable dataTable = new DataTable();

            try
            {
                string sql = "SELECT * FROM tbl_users";

                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                connection.Open();
                adapter.Fill(dataTable);
            }
            catch(Exception ex)
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

        #region Insert Data into database
        public bool Insert(userBLL user)
        {
            bool isSuccess = false;
            SqlConnection connection = new SqlConnection(myconnectingstring);

            try
            {
                string sql = "INSERT INTO tbl_users (first_name, last_name, email, username, password, contact, address, gender, user_type, added_date, added_by) VALUES " +
                    "(@first_name, @last_name, @email, @username, @password, @contact, @address, @gender, @user_type, @added_date, @added_by)";

                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@first_name", user.first_name);
                command.Parameters.AddWithValue("@last_name", user.last_name);
                command.Parameters.AddWithValue("@email", user.email);
                command.Parameters.AddWithValue("@username", user.username);
                command.Parameters.AddWithValue("@password", user.password);
                command.Parameters.AddWithValue("@contact", user.contact);
                command.Parameters.AddWithValue("@address", user.address);
                command.Parameters.AddWithValue("@gender", user.gender);
                command.Parameters.AddWithValue("@user_type", user.user_type);
                command.Parameters.AddWithValue("@added_date", user.added_date);
                command.Parameters.AddWithValue("@added_by", user.added_by);

                connection.Open();

                int rows = command.ExecuteNonQuery();

                // if the query is executed successfully then the value of rows will be greater than 0 else it will be less than 0
                if(rows > 0)
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
                connection.Close();
            }
            return isSuccess;
        }
        #endregion

        #region Update data in database
        public bool Update(userBLL user)
        {
            bool isSuccess = false;
            SqlConnection connection = new SqlConnection(myconnectingstring);

            try
            {
                string sql = "UPDATE tbl_users SET first_name = @first_name, last_name=@last_name, email=@email, username= @username, password = @password, contact = @contact, address=@address, gender=@gender, user_type=@user_type, added_date=@added_date, added_by=@added_by where id = @id";

                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@first_name", user.first_name);
                command.Parameters.AddWithValue("@last_name", user.last_name);
                command.Parameters.AddWithValue("@email", user.email);
                command.Parameters.AddWithValue("@username", user.username);
                command.Parameters.AddWithValue("@password", user.password);
                command.Parameters.AddWithValue("@contact", user.contact);
                command.Parameters.AddWithValue("@address", user.address);
                command.Parameters.AddWithValue("@gender", user.gender);
                command.Parameters.AddWithValue("@user_type", user.user_type);
                command.Parameters.AddWithValue("@added_date", user.added_date);
                command.Parameters.AddWithValue("@added_by", user.added_by);
                command.Parameters.AddWithValue("@id", user.id);

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

        #region Delete data from database
        public bool Delete(userBLL user)
        {
            bool isSuccess = false;
            SqlConnection connection = new SqlConnection(myconnectingstring);
            try
            {
                string sql = "DELETE from tbl_users where id = @id";

                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@id", user.id);

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

        #region Search user from the database using keywords
        public DataTable Search(string keywords)
        {
            SqlConnection connection = new SqlConnection(myconnectingstring);

            DataTable dataTable = new DataTable();

            try
            {
                string sql = "SELECT * FROM tbl_users WHERE id LIKE '%"+keywords+ "%' OR first_name LIKE '%"+keywords+"%' OR last_name LIKE '%"+keywords+"%' OR username LIKE '%"+keywords+"%'";

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

        #region Getting User ID from Username
        public  userBLL GetIDFromUsername(string username)
        {
            userBLL user = new userBLL();
            SqlConnection connection = new SqlConnection(myconnectingstring);

            DataTable dataTable = new DataTable();

            try
            {
                string sql = "SELECT username FROM tbl_users Where username='" + username + "'";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                connection.Open();

                adapter.Fill(dataTable);
                if (dataTable.Rows.Count > 0)
                {
                    user.username = dataTable.Rows[0]["username"].ToString();
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
            return user;
        }
        #endregion
    }
}
