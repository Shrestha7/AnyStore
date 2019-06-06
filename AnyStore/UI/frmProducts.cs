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
    public partial class frmProducts : Form
    {
        public frmProducts()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        categoriesDAL cdal = new categoriesDAL();
        productsBLL p = new productsBLL();
        productsDAL pdal = new productsDAL();
        userDAL udal = new userDAL();

        private void frmProducts_Load(object sender, EventArgs e)
        {
            //creating the datatable to hold the category from database
            DataTable categoriesDT = cdal.Select();
            //specify Datasource for category ComboBox
            cmbCategory.DataSource = categoriesDT;
            //specify Display Member and value member for comboBox
            cmbCategory.DisplayMember = "title";
            cmbCategory.ValueMember = "title";

            //Load all the products in data grid view
            DataTable dt = pdal.Select();
            dgvProducts.DataSource = dt;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //get all the value form product form
            p.name = txtName.Text;
            p.category = cmbCategory.Text;
            p.description = txtDescription.Text;
            p.rate = decimal.Parse(txtRate.Text);
            p.qty = 0;
            p.added_date = DateTime.Now;
            //Getting username form the logedin user
            String loggedUsr = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUsr);

            p.added_by = usr.id;

            //Boolean variable to check the product is added succesfully or not
            bool success = pdal.Insert(p);
            //product added succesfully the value is true else its false
            if (success==true)
            {
                //product inserted succesfully
                MessageBox.Show("Product Added Successfully.");
                //calling the clear method
                Clear();
                //Refreshing Data Grid view
                DataTable dt = pdal.Select();
                dgvProducts.DataSource = dt;
            }
            else
            {
                //Product failed to  inserted
                MessageBox.Show("Failed to Add Product.");
            }
        }
        public void Clear()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtDescription.Text = "";
            txtRate.Text = "";
            txtSearch.Text = "";

        }

        private void dgvProducts_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Create a integer variable to know which products was click
            int rowIndex = e.RowIndex;
            //Display the  value on Respective textBoxes
            txtID.Text = dgvProducts.Rows[rowIndex].Cells[0].Value.ToString();
            txtName.Text = dgvProducts.Rows[rowIndex].Cells[1].Value.ToString();
            cmbCategory.Text = dgvProducts.Rows[rowIndex].Cells[2].Value.ToString();
            txtDescription.Text = dgvProducts.Rows[rowIndex].Cells[3].Value.ToString();
            txtRate.Text = dgvProducts.Rows[rowIndex].Cells[4].Value.ToString();
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get the values from ui or products form
            p.id = int.Parse(txtID.Text);
            p.name = txtName.Text;
            p.category = cmbCategory.Text;
            p.description = txtDescription.Text;
            p.rate = decimal.Parse(txtRate.Text);
            p.added_date = DateTime.Now;

            //Getting username form the logedin user
            String loggedUsr = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUsr);

            p.added_by = usr.id;

            //Boolean variable to check the product is update succesfully or not
            bool success = pdal.Update(p);
            //product added succesfully the value is true else its false
            if (success == true)
            {
                //product inserted succesfully
                MessageBox.Show("Product Updated Successfully.");
                //calling the clear method
                Clear();
                //Refreshing Data Grid view
                DataTable dt = pdal.Select();
                dgvProducts.DataSource = dt;
            }
            else
            {
                //Product failed to  inserted
                MessageBox.Show("Failed to Updated Product.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Get to ID of the products which we want to delete
            p.id = int.Parse(txtID.Text);

            //Creating Boolean variable to Delete The category
            bool success = pdal.Delete(p);

            //If the Product id Deleted Successfully then the value of success will be true else it will be false
            if (success == true)
            {
                //product Deleted Successfully
                MessageBox.Show("Product Deleted Successfull.");
                Clear();
                //Refreshing Data grid view
                DataTable dt = pdal.Select();
                dgvProducts.DataSource = dt;
            }
            else
            {
                //category deleted failed
                MessageBox.Show("Product Deleted failed.");
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
                DataTable dt = pdal.Search(keywords);
                dgvProducts.DataSource = dt;
            }
            else
            {
                //use select method to display the categories
                DataTable dt = pdal.Select();
                dgvProducts.DataSource = dt;
            }
        }
    }
}
