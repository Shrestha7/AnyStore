using AnyStore.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.DAL
{
    class categoriesDAL
    {

        //Static String Method for Database connection String
        static string myconnstring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;

        #region Select Method
        public DataTable Select()
        {
            SqlConnection conn = new SqlConnection(myconnstring);

            DataTable dt = new DataTable();

            try
            {
                //Writing SQL Query to get all the data form database
                String sql = "SELECT * FROM tbl_categories";

                SqlCommand cmd = new SqlCommand(sql,conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //Open Database Connection
                conn.Open();

                //Adding the value form adapter to Data Table dt
                adapter.Fill(dt);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dt;

        }
        #endregion
        #region INSERT NEW Category
        public bool Insert(categoriesBLL c)
        {
            //Connecting  A Boolean Variabled and set its default value to false
            bool isSucess = false;

            //Connecting to Database
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                //Writing Query to Add New Category
                string sql = "INSERT INTO tbl_categories(title,description,added_date,added_by) VALUES(@title,@description,@added_date,@added_by)";

                //Creating SQL Command to pass values in our query
                SqlCommand cmd = new SqlCommand(sql,conn);

                //Passing Values through parameterd
                cmd.Parameters.AddWithValue("@title", c.title);
                cmd.Parameters.AddWithValue("@description", c.description);
                cmd.Parameters.AddWithValue("@added_date", c.added_date);
                cmd.Parameters.AddWithValue("@added_by", c.added_by);

                //Open Database Connection
                conn.Open();

                //Creating the int  Variable to execute query
                int rows = cmd.ExecuteNonQuery();

                //If the query  is executed successfully then its value will be greater than 0 else it will be less than 0
                if (rows>0)
                {
                    //Query Executed Succesfully
                    isSucess = true;
                }
                else
                {
                    //Failed to execute
                    isSucess = false;
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Closing Database Connection
                conn.Close();
            }

            return isSucess;
        }
        #endregion
        #region UPDATE METHOD
        public bool Update(categoriesBLL c)
        {
            //Creating Boolean variable and set its default value to false
            bool isSuccess = false;

            //Creating SQL Connection
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                //Query to update category
                string sql = "UPDATE tbl_categories SET title=@title, description=@description, added_date=@added_date, added_by=@added_by WHERE id =@id";

                //SQL command to pass the value on sql query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Passing Value using cmd
                cmd.Parameters.AddWithValue("title", c.title);
                cmd.Parameters.AddWithValue("description", c.description);
                cmd.Parameters.AddWithValue("added_date", c.added_date);
                cmd.Parameters.AddWithValue("added_by", c.added_by);
                cmd.Parameters.AddWithValue("id", c.id);

                //Open Database Connection
                conn.Open();

                //Create Int Variable to execute query
                int rows = cmd.ExecuteNonQuery();

                //if the query is successfully executed than the value  will be greater than 0
                if (rows>0)
                {
                    //Query Executed succesfull
                    isSuccess = true;
                }
                else
                {
                    //failed executed 
                    isSuccess = false;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return isSuccess;
        }
        #endregion
        #region DELETE Category Method
        public bool Delete(categoriesBLL c)
        {
            //Create Boolean variable and set its value to false
            bool isSuccess = false;

            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                //SQL query to delete from database
                string sql = "DELETE FROM tbl_categories WHERE id=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);

                //passing the value using cmd
                cmd.Parameters.AddWithValue("id", c.id);

                //Open sql connection
                conn.Open();

                int rows = cmd.ExecuteNonQuery();

                //if the query is executed successfully then the value is greater than 0 else its less than 0
                if (rows>0)
                {
                    //executed successfully
                    isSuccess = true;
                }
                else
                {
                    //executed failed
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return isSuccess;
        }
        #endregion
        #region Search category
        public DataTable Search(string keywords)
        {
            //SQL Connection for Database Connection
            SqlConnection conn = new SqlConnection(myconnstring);

            //Creating Data Table to hold the data from database temporarily
            DataTable dt = new DataTable();

            try
            {
                //SQL Query to search category form database
                String sql = "SELECT * FROM tbl_categories WHERE id LIKE '%"+keywords+"%' OR title LIKE '%"+keywords+"%' OR description LIKE '%"+keywords+"%'";

                //Creating Sql command to pass value
                SqlCommand cmd = new SqlCommand(sql, conn);


                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                //Open Database Connection
                conn.Open();

                //Adding the value form adapter to Data Table dt
                adapter.Fill(dt);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
        #endregion
    }
}
