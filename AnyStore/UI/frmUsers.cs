using AnyStore.BLL;
using AnyStore.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.UI
{
    public partial class frmUsers : Form
    {
        public frmUsers()
        {
            InitializeComponent();
        }

        userBLL u = new userBLL();
        userDAL dal = new userDAL();

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {   
           


            //Getting Data from UI
            u.first_name = txtFirstName.Text;
            u.last_name = txtLastName.Text;
            u.email = txtEmail.Text;
            u.username = txtUsername.Text;
            u.password = txtPassword.Text;
            u.contact = txtContact.Text;
            u.address = txtAddress.Text;
            u.gender = cmbGender.Text;
            u.user_type = cmbUsertype.Text;
            u.added_date = DateTime.Now;

            //Getting username of logged in username
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = dal.GetIDFromUsername(loggedUser);

            u.added_by = usr.id;

            //Inserting Data into Datebase
            bool success = dal.Insert(u);
            //If data is successfully inserted the value of success will be true else it will be false
            if (success == true)
            {
                //Data successfully Inserted
                MessageBox.Show("Successfully user created.");
                clear();
            }
            else
            {
                //Data failed to insert
                MessageBox.Show("Failed to create user.");
            }
            //Refreshing data in Grid view
            DataTable dt = dal.Select();
            dgvUsers.DataSource = dt;
        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            DataTable dt = dal.Select();
            dgvUsers.DataSource = dt;
        }

        private void clear()
        {
            txtUserID.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtContact.Text = "";
            txtAddress.Text = "";
            cmbGender.Text = "";
            cmbUsertype.Text = "";
        }

        private void dgvUsers_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Get the index of particular row
            int rowIndex = e.RowIndex;
            txtUserID.Text = dgvUsers.Rows[rowIndex].Cells[0].Value.ToString();
            txtFirstName.Text = dgvUsers.Rows[rowIndex].Cells[1].Value.ToString();
            txtLastName.Text = dgvUsers.Rows[rowIndex].Cells[2].Value.ToString();
            txtEmail.Text = dgvUsers.Rows[rowIndex].Cells[3].Value.ToString();
            txtUsername.Text = dgvUsers.Rows[rowIndex].Cells[4].Value.ToString();
            txtPassword.Text = dgvUsers.Rows[rowIndex].Cells[5].Value.ToString();
            txtContact.Text = dgvUsers.Rows[rowIndex].Cells[6].Value.ToString();
            txtAddress.Text = dgvUsers.Rows[rowIndex].Cells[7].Value.ToString();
            cmbGender.Text = dgvUsers.Rows[rowIndex].Cells[8].Value.ToString();
            cmbUsertype.Text = dgvUsers.Rows[rowIndex].Cells[9].Value.ToString();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get values from user UI
            u.id = Convert.ToInt32(txtUserID.Text);
            u.first_name = txtFirstName.Text;
            u.last_name = txtLastName.Text;
            u.email = txtEmail.Text;
            u.username = txtUsername.Text;
            u.password = txtPassword.Text;
            u.contact = txtContact.Text;
            u.address = txtAddress.Text;
            u.gender = cmbGender.Text;
            u.user_type = cmbUsertype.Text;
            u.added_date = DateTime.Now;
            u.added_by = 1;

            //Updating Data into Database
            bool success = dal.Update(u);
            //If data is successfully updated the values is true else it will be false
            if (success == true)
            {
                //Data update successfull
                MessageBox.Show("User Data update Successfully.");
                clear();
            }
            else
            {
                //Data update failed
                MessageBox.Show("Fail to update user Data.");
            }
            //Refreshing data grid view
            DataTable dt = dal.Select();
            dgvUsers.DataSource = dt;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //getting user id from Form
            u.id = Convert.ToInt32(txtUserID.Text);

            bool success = dal.Delete(u);
            //if data is delete the value of success will be true else false

            if (success == true)
            {
                //Data deleted successfull
                MessageBox.Show("User Successfully Deleted.");
                clear();
            }
            else
            {
                //Data Deleted failed
                MessageBox.Show("User Failed to Delete.");
            }
            //Refreshing data grid view
            DataTable dt = dal.Select();
            dgvUsers.DataSource = dt;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get user from textbox
            string keywords = txtSearch.Text;

            //Check if the keywords has a value or not
            if (keywords !=null)
            {
                //show users based on keywords
                DataTable dt = dal.Search(keywords);
                dgvUsers.DataSource = dt;
            }
            else
            {
                //show all the users from the database
                DataTable dt = dal.Select();
                dgvUsers.DataSource = dt;
            }
        }

        
    }
}
