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
    public partial class frmCategories : Form
    {
        public frmCategories()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        categoriesBLL c = new categoriesBLL();
        categoriesDAL dal = new categoriesDAL();
        userDAL udal = new userDAL();

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //get the values form category form
            c.title = txtTitle.Text;
            c.description = txtDescription.Text;
            c.added_date = DateTime.Now;

            //Getting ID in Added by field
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            //passing the id of logged in user in added by field
            c.added_by = usr.id;

            //Creating Boolean Method To insert data into database
            bool success = dal.Insert(c);

            if (success==true)
            {
                //new category inserted successfull
                MessageBox.Show("New Category Inserted successful.");
                Clear();
                //Refresh Data grid view
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
            else
            {
                //failed to insert
                MessageBox.Show("New category Inserted Failed.");
            }
         }
        public void Clear()
        {
            txtCategoryID.Text = "";
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtSearch.Text = "";
        }

        private void frmCategories_Load(object sender, EventArgs e)
        {
            //Display all the categories in data grid view
            DataTable dt = dal.Select();
            dgvCategories.DataSource = dt;
        }

        private void dgvCategories_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Finding the row Index of the Row Clicked on Data Grid View
            int RowIndex = e.RowIndex;
            txtCategoryID.Text = dgvCategories.Rows[RowIndex].Cells[0].Value.ToString();
            txtTitle.Text = dgvCategories.Rows[RowIndex].Cells[1].Value.ToString();
            txtDescription.Text = dgvCategories.Rows[RowIndex].Cells[2].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get the value from category form
            c.id = int.Parse(txtCategoryID.Text);
            c.title = txtTitle.Text;
            c.description = txtDescription.Text;
            c.added_date = DateTime.Now;
            //Getting ID in Added by field
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            //passing the id of logged in user in added by field
            c.added_by = usr.id;

            //Creating Boolean variable to update categories and check
            bool success = dal.Update(c);
            //If the category is updated successfully then sucess will be true else false
            if (success==true)
            {
                //category updated successfully
                MessageBox.Show("Category Updated Sucessfully.");
                Clear();
                //Refres Data Grid view
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
            else
            {
                //failed to update category
                MessageBox.Show("Category Updated Failed.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Get to ID of the category which we want to delete
            c.id = int.Parse(txtCategoryID.Text);

            //Creating Boolean variable to Delete The category
            bool success = dal.Delete(c);

            //If the category id Deleted Successfully then the value of success will be true else it will be false
            if (success==true)
            {
                //category Deleted Successfully
                MessageBox.Show("Category Deleted Successful.");
                Clear();
                //Refreshing Data grid view
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
            else
            {
                //category deleted failed
                MessageBox.Show("Category Deleted failed.");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get keywords
            string keywords = txtSearch.Text;

            //Filter the category based on keywords
            if (keywords!=null)
            {
                // use search method to display the categories
                DataTable dt = dal.Search(keywords);
                dgvCategories.DataSource = dt;
            }
            else
            {
                //use select method to display the categories
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
        }
    }
}
