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
    class transactionDAL
    {
        //Creating a connection string variable
        static string myconnstring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;

        #region INSERT transactions Method
        public bool Insert_Transaction(transactionsBLL t,out int transactionID)
        {
            //create  a boolean value  and set its value to false
            bool isSuccess = false;
            //Set the out transactionID value to negative 1 i.e. -1
            transactionID = -1;
            //Create SqlConnection
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                //Query to insert transaction
                string sql = "INSERT INTO tbl_transactions (type,dea_cust_id,grandTotal,transaction_date,tax,discount,added_by) VALUES(@type,@dea_cust_id,@grandTotal,@transaction_date,@tax,@discount,@added_by); SELECT @@IDENTITY";

                //Sql command pass the value in sql query
                SqlCommand cmd = new SqlCommand(sql,conn);

                //Passing Values through parameterd
                cmd.Parameters.AddWithValue("@type", t.type);
                cmd.Parameters.AddWithValue("@dea_cust_id", t.dea_cust_id);
                cmd.Parameters.AddWithValue("@grandTotal", t.grandTotal);
                cmd.Parameters.AddWithValue("@transaction_date", t.transaction_date);
                cmd.Parameters.AddWithValue("@tax", t.tax);
                cmd.Parameters.AddWithValue("@discount", t.discount);
                cmd.Parameters.AddWithValue("@added_by", t.added_by);

                //Open Database Connection
                conn.Open();

                //Execute the query
                object o = cmd.ExecuteScalar();

                //If the query executed Successfully the value will not be null else it will be null
                if (o!=null)
                {
                    //executed succesfully
                    transactionID = int.Parse(o.ToString());
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
    }
}
