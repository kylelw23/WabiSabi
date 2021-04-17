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
    class loginDAL
    {
        //Static string to connect database
        static string myconnectionstring = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public int loginCheck(loginBLL login)
        {
            //Create a boolean variable and set its value to false and return it
            int isSuccess = 0;

            SqlConnection connection = new SqlConnection(myconnectionstring);

            try
            {
                //SQL Query to check login
                string sql = "SELECT user_type FROM tbl_users Where username=@username AND password=@password";
                
                //Creating SQL command to pass vale
                SqlCommand cmd = new SqlCommand(sql, connection);

                cmd.Parameters.AddWithValue("@username", login.username);
                cmd.Parameters.AddWithValue("@password", login.password);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                string user_type = dataTable.Rows[0].Field<string>(0);

                //Checking the roews in datatable 
                if (dataTable.Rows.Count > 0)
                {
                    if(user_type.ToLower() == "admin")
                    {
                        isSuccess = 1;
                    }
                    if(user_type.ToLower() == "staff")
                    {
                        isSuccess = 2;
                    }
                }
                else
                {
                    // Login failed
                    isSuccess = 0;
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
    }
}
