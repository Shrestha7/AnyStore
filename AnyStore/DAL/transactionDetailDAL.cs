using AnyStore.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.DAL
{
    class transactionDetailDAL
    {
        //Creating a connection string variable
        static string myconnstring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;

        #region INSERT method for transactions Detail
        public bool InsertTransactionDetail(transactionDetailBLL td)
        {
            //Creating a bool value and setting its default value to false
            bool isSuccess = false;
            //Create SqlConnection
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                //Query to insert transaction
                string sql = "INSERT INTO tbl_transaction_detail (product_id,rate,qty,total,dea_cust_id,added_date,added_by) VALUES(@product_id,@rate,@qty,@total,@dea_cust_id,@added_date,@added_by,@added_by)";

                //Sql command pass the value in sql query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Passing Values through parameterd
                cmd.Parameters.AddWithValue("@product_id", td.product_id);
                cmd.Parameters.AddWithValue("@rate", td.rate);
                cmd.Parameters.AddWithValue("@qty", td.qty);
                cmd.Parameters.AddWithValue("@total", td.total);
                cmd.Parameters.AddWithValue("@dea_cust_id", td.dea_cust_id);
                cmd.Parameters.AddWithValue("@added_date", td.added_date);
                cmd.Parameters.AddWithValue("@added_by", td.added_by);
               

                //Open Database Connection
                conn.Open();

                //Creating the int  Variable to execute query
                int rows = cmd.ExecuteNonQuery();

                //If the query  is executed successfully then its value will be greater than 0 else it will be less than 0
                if (rows > 0)
                {
                    //Query Executed Succesfully
                    isSuccess = true;
                }
                else
                {
                    //Failed to execute
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
    }
}
