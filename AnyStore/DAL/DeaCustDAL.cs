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
    class DeaCustDAL
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
                String sql = "SELECT * FROM tbl_dea_cust";

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
        #region INSERT NEW Category
        public bool Insert(DeaCustBLL dc)
        {
            //Connecting  A Boolean Variabled and set its default value to false
            bool isSucess = false;

            //Connecting to Database
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                //Writing Query to Add New Category
                string sql = "INSERT INTO tbl_dea_cust(type,name,email,contact,address,added_date,added_by) VALUES(@type,@name,@email,@contact,@address,@added_date,@added_by)";

                //Creating SQL Command to pass values in our query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Passing Values through parameterd
                cmd.Parameters.AddWithValue("@type", dc.type);
                cmd.Parameters.AddWithValue("@name", dc.name);
                cmd.Parameters.AddWithValue("@email", dc.email);
                cmd.Parameters.AddWithValue("@contact", dc.contact);
                cmd.Parameters.AddWithValue("@address", dc.address);
                cmd.Parameters.AddWithValue("@added_date", dc.added_date);
                cmd.Parameters.AddWithValue("@added_by", dc.added_by);

                //Open Database Connection
                conn.Open();

                //Creating the int  Variable to execute query
                int rows = cmd.ExecuteNonQuery();

                //If the query  is executed successfully then its value will be greater than 0 else it will be less than 0
                if (rows > 0)
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
        public bool Update(DeaCustBLL dc)
        {
            //Creating Boolean variable and set its default value to false
            bool isSuccess = false;

            //Creating SQL Connection
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                //Query to update category
                string sql = "UPDATE tbl_dea_cust SET type=@type,name=@name,email=@email,contact=@contact,address=@address, added_date=@added_date, added_by=@added_by WHERE id =@id";

                //SQL command to pass the value on sql query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Passing Value using cmd
                cmd.Parameters.AddWithValue("type", dc.type);
                cmd.Parameters.AddWithValue("name", dc.name);
                cmd.Parameters.AddWithValue("email", dc.email);
                cmd.Parameters.AddWithValue("contact", dc.contact);
                cmd.Parameters.AddWithValue("address", dc.address);
                cmd.Parameters.AddWithValue("added_date", dc.added_date);
                cmd.Parameters.AddWithValue("added_by", dc.added_by);
                cmd.Parameters.AddWithValue("id", dc.id);

                //Open Database Connection
                conn.Open();

                //Create Int Variable to execute query
                int rows = cmd.ExecuteNonQuery();

                //if the query is successfully executed than the value  will be greater than 0
                if (rows > 0)
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
        #region DELETE Method
        public bool Delete(DeaCustBLL dc)
        {
            //Create Boolean variable and set its value to false
            bool isSuccess = false;

            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                //SQL query to delete from database
                string sql = "DELETE FROM tbl_dea_cust WHERE id=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);

                //passing the value using cmd
                cmd.Parameters.AddWithValue("id", dc.id);

                //Open sql connection
                conn.Open();

                int rows = cmd.ExecuteNonQuery();

                //if the query is executed successfully then the value is greater than 0 else its less than 0
                if (rows > 0)
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
                String sql = "SELECT * FROM tbl_dea_cust WHERE id LIKE '%" + keywords + "%' OR type LIKE '%" + keywords + "%' OR name LIKE '%" + keywords + "%'";

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
        #region SEARCH METHOD  to search dealer or customer for transaction Module
        public DeaCustBLL SearchDealerCustomerForTransaction(string keyword)
        {
            //Create an object DeaCustBLL class
            DeaCustBLL dc = new DeaCustBLL();

            //Create a database connection
            SqlConnection conn = new SqlConnection(myconnstring);

            //create a datatable to hold the data temporarily
            DataTable dt = new DataTable();

            try
            {
                //Query to search Dealer or Customer based on keywords
                string sql = "SELECT name,email,contact,address from tbl_dea_cust WHERE id LIKE '%" + keyword + "%' OR name LIKE '%" + keyword + "%'  ";

                //Sql data adapter to execute the query
                SqlDataAdapter adapter = new SqlDataAdapter(sql,conn);

                //open db connection
                conn.Open();

                //Transfer the data from sqldata adapter to data table
                adapter.Fill(dt);

                //IF we have value on dt  save it in dealerCustomer BLL
                if (dt.Rows.Count>0)
                {
                    dc.name = dt.Rows[0]["name"].ToString();
                    dc.email = dt.Rows[0]["email"].ToString();
                    dc.contact = dt.Rows[0]["contact"].ToString();
                    dc.address= dt.Rows[0]["address"].ToString();
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

            return dc;
        }
        #endregion
        #region Method to get id of the dealer or customer based on name
        public DeaCustBLL GetDeaCustIDFromName(string Name)
        {
            //Object of DeaCust BLL and return 
            DeaCustBLL dc = new DeaCustBLL();

            //Sql Connection here
            SqlConnection conn = new SqlConnection(myconnstring);
            //Data table to hold the data temporarliy
            DataTable dt = new DataTable();

            try
            {
                //SQL Query to get id based on Name
                string sql = "SELECT id FROM tbl_dea_cust WHERE name='" + Name + "'";
                //Create the sql DATA adapter to execute the query
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

                conn.Open();

                //passing the value from adapter to datatable
                adapter.Fill(dt);
                if (dt.Rows.Count>0)
                {
                    //pass the value from dt to DeaCustBLL dc
                    dc.id = int.Parse(dt.Rows[0]["id"].ToString());
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

            return dc;
        }
        #endregion


    }
}
