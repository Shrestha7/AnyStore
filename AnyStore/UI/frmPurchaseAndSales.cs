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
using System.Transactions;
using System.Windows.Forms;

namespace AnyStore.UI
{
    public partial class frmPurchaseAndSales : Form
    {
        public frmPurchaseAndSales()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        DeaCustDAL dcDAL = new DeaCustDAL();
        productsDAL pDAL = new productsDAL();
        userDAL uDAL = new userDAL();
        transactionDAL tDAL = new transactionDAL();
        transactionDetailDAL tdDAL = new transactionDetailDAL();

        DataTable transactionDT = new DataTable();

        private void frmPurchaseAndSales_Load(object sender, EventArgs e)
        {
            //get the transactiontype value from the userdashboard
            string type = firmUserDashboard.transactionType;
            //Set the value on lblTop
            lblTop.Text= type;

            //specify cols for our transactionDataTable
            transactionDT.Columns.Add("Product Name");
            transactionDT.Columns.Add("Rate");
            transactionDT.Columns.Add("Quantity");
            transactionDT.Columns.Add("Total");
            
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //get the keyword from the text box
            string keyword = txtSearch.Text;

            if (keyword=="")
            {
                //clear all textbox
                txtName.Text = "";
                txtEmail.Text = "";
                txtContact.Text = "";
                txtAddress.Text = "";
                return;
            }

            //get the detail the set the values on txtboxs
            DeaCustBLL dc = dcDAL.SearchDealerCustomerForTransaction(keyword);

            //transfer the value form deacustBLL to txtbox
            txtName.Text = dc.name;
            txtEmail.Text = dc.email;
            txtContact.Text = dc.contact;
            txtAddress.Text = dc.address;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Get the product name , rate and qty customer wants to buy
            string productName = txtProductName.Text;
            decimal Rate = decimal.Parse(txtRate.Text);
            decimal Qty = decimal.Parse(txtQty.Text);

            decimal Total = Rate * Qty;

            //Display subtotal in textboxes
            //Get the subtotal value from textboxes
            decimal subTotal = decimal.Parse(txtSubTotal.Text);
            subTotal = subTotal + Total;

            //Check whether the product is selected or not
            if (productName=="")
            {
                //Display error message
                MessageBox.Show("Select the product first. Try Again.");
            }
            else
            {
                //add product to the data grid view
                transactionDT.Rows.Add(productName, Rate, Qty, Total);

                //Show in data grid view
                dgvAddedProducts.DataSource = transactionDT;

                //Display the subtotal in textbox
                txtSubTotal.Text = subTotal.ToString();

                //Clear the TextBoxes
                txtSearchProduct.Text = "";
                txtProductName.Text = "";
                txtInventory.Text = "0.00";
                txtRate.Text = "0.00";
                txtQty.Text = "0.00";

            }
            
        }

        private void txtSearchProduct_TextChanged(object sender, EventArgs e)
        {
            //get the keywords from productsearch textbox
            string keyword = txtSearchProduct.Text;

            //Check if we have value to txtsearchProduct or not
            if (keyword=="")
            {
                txtProductName.Text = "";
                txtInventory.Text = "";
                txtRate.Text = "";
                txtQty.Text = "";
                return;
            }

            //search and display product
            productsBLL p = pDAL.GetProductsForTransaction(keyword);

            //Set the value on textboxes based on p objects
            txtProductName.Text = p.name;
            txtInventory.Text = p.qty.ToString();
            txtRate.Text = p.rate.ToString();
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            //get the value form discount textbox
            string value = txtDiscount.Text;

            if (value=="")
            {
                MessageBox.Show("Please add Discount First");

            }
            else
            {
                //get the discount in decimal value
                decimal subTotal = decimal.Parse(txtSubTotal.Text);
                decimal discount = decimal.Parse(txtDiscount.Text);

                //Calcualte teh grandtotal  based on discount
                decimal grandTotal = ((100 - discount) / 100) * subTotal;

                //Display the grandTotal in textBox
                txtGrandTotal.Text = grandTotal.ToString();
            }
        }

        private void txtVat_TextChanged(object sender, EventArgs e)
        {
            //check if the grandTotal has value or not if it has not value then calculate the discount first
            string check = txtGrandTotal.Text;
            if (check=="")
            {
                //Display the error message to calculate discount
                MessageBox.Show("Calculate the discount and set the grandTotal First.");
            }
            else
            {
                //calculate Vat
                //Getting the VAT percent first
                decimal previousGT = decimal.Parse(txtGrandTotal.Text);
                decimal vat = decimal.Parse(txtVat.Text);
                decimal grandTotalWithVAT =((100+vat)/100)*previousGT;

                //Display new grand total with vat
                txtGrandTotal.Text = grandTotalWithVAT.ToString();
            }
        }

        private void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            //get the paid amount and grand total
            decimal grandTotal = decimal.Parse(txtGrandTotal.Text);
            decimal paidAmount = decimal.Parse(txtPaidAmount.Text);

            decimal returnAmount = paidAmount - grandTotal;

            //Display the return amount as well
            txtReturnAmount.Text = returnAmount.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //get the details from the Purchasesale form
            transactionsBLL transaction = new transactionsBLL();

            transaction.type = lblTop.Text;

            //get the id of dealer or customer 
            //get the name of dealer or customer
            string deaCustName = txtName.Text;
            DeaCustBLL dc = dcDAL.GetDeaCustIDFromName(deaCustName);

            transaction.dea_cust_id = dc.id;
            transaction.grandTotal=Math.Round(decimal.Parse(txtGrandTotal.Text),2);
            transaction.transaction_date = DateTime.Now;
            transaction.tax = decimal.Parse(txtVat.Text);
            transaction.discount = decimal.Parse(txtDiscount.Text);

            //Get the username of logged in user
            string username = frmLogin.loggedIn;
            userBLL u = uDAL.GetIDFromUsername(username);

            transaction.added_by = u.id;
            transaction.transactionDetails = transactionDT;

            //create a Boolean variable and set its value to false
            bool success = false;

            //Actual code to insert Transaction and transaction details
            using (TransactionScope scope = new TransactionScope())
            {
                int transactionID = -1;
                //boolean value and insert transaction
                bool w = tDAL.Insert_Transaction(transaction, out transactionID);

                //use for loop to insert Transaction Details
                for (int i = 0; i < transactionDT.Rows.Count; i++)
                {
                    //Get all the details of the product
                    transactionDetailBLL transactionDetail = new transactionDetailBLL();
                    //get the product name and convert it to id
                    string ProductName = transactionDT.Rows[i][0].ToString();
                    productsBLL p = pDAL.GetProductIDFromName(ProductName);

                    transactionDetail.product_id = p.id;
                    transactionDetail.rate = decimal.Parse(transactionDT.Rows[i][1].ToString());
                    transactionDetail.qty = decimal.Parse(transactionDT.Rows[i][2].ToString());
                    transactionDetail.total = Math.Round(decimal.Parse(transactionDT.Rows[i][3].ToString()),2);
                    transactionDetail.dea_cust_id = dc.id;
                    transactionDetail.added_date = DateTime.Now;
                    transactionDetail.added_by = u.id;

                    //insert transaction details inside the database
                    bool y = tdDAL.InsertTransactionDetail(transactionDetail);
                    success = w && y;
                }
                
                if (success == true)
                {
                    //transaction complete
                    scope.Complete();
                    MessageBox.Show("Transaction Complete Sucessfull.");
                    //clear data gridview and textbox
                    dgvAddedProducts.DataSource = null;
                    dgvAddedProducts.Rows.Clear();

                    txtSearch.Text = "";
                    txtName.Text = "";
                    txtEmail.Text = "";
                    txtContact.Text = "";
                    txtAddress.Text = "";
                    txtSearchProduct.Text = "";
                    txtProductName.Text = "";
                    txtInventory.Text = "0";
                    txtRate.Text = "0";
                    txtQty.Text = "0";
                    txtSubTotal.Text = "0";
                    txtDiscount.Text = "0";
                    txtVat.Text = "0";
                    txtGrandTotal.Text = "0";
                    txtPaidAmount.Text = "0";
                    txtReturnAmount.Text = "0";
                }
                else
                {
                    //Transaction failed
                    MessageBox.Show("Transaction failed.");
                }
            }

        }
    }
}
