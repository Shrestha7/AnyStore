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
    public partial class frmDeaCust : Form
    {
        public frmDeaCust()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        DeaCustBLL dc = new DeaCustBLL();
        DeaCustDAL dcDal = new DeaCustDAL();

        userDAL uDal = new userDAL();
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //get the values from form
            dc.type = cmbType.Text;
            dc.name = txtName.Text;
            dc.email = txtEmail.Text;
            dc.contact = txtContact.Text;
            dc.address = txtAddress.Text;
            dc.added_date = DateTime.Now;

            //Getting ID in Added by field
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = uDal.GetIDFromUsername(loggedUser);
            //passing the id of logged in user in added by field
            dc.added_by = usr.id;

            //Creating Boolean Method To insert data into database
            bool success = dcDal.Insert(dc);
             
            if (success == true)
            {
                //new category inserted successfull
                MessageBox.Show("Dealer or Customer Added Sucessfully.");
                Clear();
                //Refresh Data grid view
                DataTable dt = dcDal.Select();
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                //failed to insert
                MessageBox.Show("Dealer or Customer Failed .");
            }

        }
        public void Clear()
        {
            txtDeaCustID.Text = "";
            txtName.Text = "";
            txtEmail.Text = "";
            txtContact.Text = "";
            txtAddress.Text = "";
            txtSearch.Text = "";
        }

        private void frmDeaCust_Load(object sender, EventArgs e)
        {
            //Refresh Data grid view
            DataTable dt = dcDal.Select();
            dgvDeaCust.DataSource = dt;
        }

        private void dgvDeaCust_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Finding the row Index of the Row Clicked on Data Grid View
            int RowIndex = e.RowIndex;
            txtDeaCustID.Text = dgvDeaCust.Rows[RowIndex].Cells[0].Value.ToString();
            cmbType.Text = dgvDeaCust.Rows[RowIndex].Cells[1].Value.ToString();
            txtName.Text = dgvDeaCust.Rows[RowIndex].Cells[2].Value.ToString();
            txtEmail.Text = dgvDeaCust.Rows[RowIndex].Cells[3].Value.ToString();
            txtContact.Text = dgvDeaCust.Rows[RowIndex].Cells[4].Value.ToString();
            txtAddress.Text = dgvDeaCust.Rows[RowIndex].Cells[5].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get the value from category form
            dc.id = int.Parse(txtDeaCustID.Text);
            dc.type = cmbType.Text;
            dc.name = txtName.Text;
            dc.email = txtEmail.Text;
            dc.contact = txtContact.Text;
            dc.address = txtAddress.Text;
            dc.added_date = DateTime.Now;
            //Getting ID in Added by field
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = uDal.GetIDFromUsername(loggedUser);
            //passing the id of logged in user in added by field
            dc.added_by = usr.id;

            //Creating Boolean variable to update categories and check
            bool success = dcDal.Update(dc);
            //If the category is updated successfully then sucess will be true else false
            if (success == true)
            {
                //category updated successfully
                MessageBox.Show("Dealer or Customer Updated Sucessfully.");
                Clear();
                //Refres Data Grid view
                DataTable dt = dcDal.Select();
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                //failed to update category
                MessageBox.Show("Dealer or Customer Updated Failed.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Get to ID of the category which we want to delete
            dc.id = int.Parse(txtDeaCustID.Text);

            //Creating Boolean variable to Delete The category
            bool success = dcDal.Delete(dc);

            //If the category id Deleted Successfully then the value of success will be true else it will be false
            if (success == true)
            {
                //category Deleted Successfully
                MessageBox.Show(" Deleted Successful.");
                Clear();
                //Refreshing Data grid view
                DataTable dt = dcDal.Select();
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                //category deleted failed
                MessageBox.Show("Failed to Delete.");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get keywords
            string keywords = txtSearch.Text;

            //Filter the category based on keywords
            if (keywords != null)
            {
                // use search method to display the categories
                DataTable dt = dcDal.Search(keywords);
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                //use select method to display the categories
                DataTable dt = dcDal.Select();
                dgvDeaCust.DataSource = dt;
            }
        }
    }
}
