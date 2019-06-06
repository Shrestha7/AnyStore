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
    class productsDAL
    {
        //Creating static  string method  for Database connection
        static string myconnstring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;
        #region SELECT Method for product
        public DataTable Select()
        {
            SqlConnection conn = new SqlConnection(myconnstring);

            DataTable dt = new DataTable();

            try
            {
                //Writing SQL Query to get all the data form database
                String sql = "SELECT * FROM tbl_products";

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
        #region Insert Method for product
        public bool Insert(productsBLL c)
        {
            //Connecting  A Boolean Variabled and set its default value to false
            bool isSucess = false;

            //Connecting to Database
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                //Writing Query to Add New Category
                string sql = "INSERT INTO tbl_products(name,category,description,rate,qty,added_date,added_by) VALUES(@name,@category,@description,@rate,@qty,@added_date,@added_by)";

                //Creating SQL Command to pass values in our query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Passing Values through parameterd
                cmd.Parameters.AddWithValue("@name", c.name);
                cmd.Parameters.AddWithValue("@category", c.category);
                cmd.Parameters.AddWithValue("@description", c.description);
                cmd.Parameters.AddWithValue("@rate", c.rate);
                cmd.Parameters.AddWithValue("@qty", c.qty);
                cmd.Parameters.AddWithValue("@added_date", c.added_date);
                cmd.Parameters.AddWithValue("@added_by", c.added_by);

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
        #region UPDATE METHOD for product
        public bool Update(productsBLL c)
        {
            //Creating Boolean variable and set its default value to false
            bool isSuccess = false;

            //Creating SQL Connection
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                //Query to update category
                string sql = "UPDATE tbl_products SET name=@name, category=@category, description=@description, rate=@rate,added_date=@added_date, added_by=@added_by WHERE id =@id";

                //SQL command to pass the value on sql query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Passing Value using cmd
                cmd.Parameters.AddWithValue("name", c.name);
                cmd.Parameters.AddWithValue("category", c.category);
                cmd.Parameters.AddWithValue("description", c.description);
                cmd.Parameters.AddWithValue("rate", c.rate);
                cmd.Parameters.AddWithValue("qty", c.qty);
                cmd.Parameters.AddWithValue("added_date", c.added_date);
                cmd.Parameters.AddWithValue("added_by", c.added_by);
                cmd.Parameters.AddWithValue("id", c.id);

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
        #region DELETE Method for product
        public bool Delete(productsBLL c)
        {
            //Create Boolean variable and set its value to false
            bool isSuccess = false;

            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                //SQL query to delete from database
                string sql = "DELETE FROM tbl_products WHERE id=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);

                //passing the value using cmd
                cmd.Parameters.AddWithValue("id", c.id);

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
        #region SEARCH Method for product
        public DataTable Search(string keywords)
        {
            //SQL Connection for Database Connection
            SqlConnection conn = new SqlConnection(myconnstring);

            //Creating Data Table to hold the data from database temporarily
            DataTable dt = new DataTable();

            try
            {
                //SQL Query to search category form database
                String sql = "SELECT * FROM tbl_products WHERE id LIKE '%" + keywords + "%' OR name LIKE '%" + keywords + "%' OR category LIKE '%"+keywords+"%' ";

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
        #region method to search transacation module
        public productsBLL GetProductsForTransaction(String keyword)
        {
            productsBLL p = new productsBLL();
            //SqlConnections
            SqlConnection conn = new SqlConnection(myconnstring);
            //DataTable to store the data temporarily
            DataTable dt = new DataTable();

            try
            {
                //Query to get the detail
                string sql = "SELECT name,rate, qty FROM tbl_products WHERE id LIKE'%" + keyword + "%' OR name LIKE '%" + keyword + "%'";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

                //Open Database Connection
                conn.Open();

                //Adding the value form adapter to Data Table dt
                adapter.Fill(dt);

                if (dt.Rows.Count>0)
                {
                    p.name = dt.Rows[0]["name"].ToString();
                    p.rate = decimal.Parse(dt.Rows[0]["rate"].ToString());
                    p.qty = decimal.Parse(dt.Rows[0]["qty"].ToString());
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

            return p;
        }
        #endregion
       #region method to get product ID based on product name 
        public productsBLL GetProductIDFromName(string ProductName)
        {
            //Object of DeaCust BLL and return 
            productsBLL p = new productsBLL();

            //Sql Connection here
            SqlConnection conn = new SqlConnection(myconnstring);
            //Data table to hold the data temporarliy
            DataTable dt = new DataTable();

            try
            {
                //SQL Query to get id based on Name
                string sql = "SELECT id FROM tbl_products WHERE name='" + ProductName + "'";
                //Create the sql DATA adapter to execute the query
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);

                conn.Open();

                //passing the value from adapter to datatable
                adapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    //pass the value from dt to DeaCustBLL dc
                    p.id = int.Parse(dt.Rows[0]["id"].ToString());
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

            return p;
        }
        #endregion
        #region Method to get current quantity form the db based on Product ID
        public decimal GetProductQty(int ProductID)
        {
            //SQL connection first
            SqlConnection conn = new SqlConnection(myconnstring);
            //Create a Decimal Variable and set its default value to 0
            decimal qty = 0;
            //Data table to save the data for temperarily
            DataTable dt = new DataTable();
            try
            {
                string sql = "SELECT qty FROM tbl_products WHERE id =" + ProductID;

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                conn.Open();
                adapter.Fill(dt);
                if (dt.Rows.Count>0)
                {
                    qty = decimal.Parse(dt.Rows[0]["qty"].ToString());
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
            return qty;
        }
        #endregion
        #region Method  to update Quantity
        public bool UpdateQuantity(int ProductID, decimal Qty)
        {
            bool success = false;
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                string sql = "UPDATE tbl_products SET qty=@qty WHERE id=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@qty", Qty);
                cmd.Parameters.AddWithValue("@id", ProductID);
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows>0)
                {
                    success = true;
                }
                else
                {
                    success = false;
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
            return success;
        }
        #endregion

    }

}
