using AnyStore.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore
{
    public partial class firmUserDashboard : Form
    {
        public firmUserDashboard()
        {
            InitializeComponent();
        }

        //Set a public static method to specify whether the form is purchase or sales
        public static string transactionType;

        private void firmUserDashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
            this.Hide();
        }

        private void firmUserDashboard_Load(object sender, EventArgs e)
        {
            lblLoggedInUser.Text = frmLogin.loggedIn;
        }

        private void dealerAndCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDeaCust DeaCust = new frmDeaCust();
            DeaCust.Show();
        }

        private void purchaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //set the value of transactiontype static method to purchase
            transactionType = "Purchase";
            frmPurchaseAndSales purchase = new frmPurchaseAndSales();
            purchase.Show();
            
        }

        private void salesFormsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //set the value of transactiontype static method to sales
            transactionType = "Sales";
            frmPurchaseAndSales purchase = new frmPurchaseAndSales();
            purchase.Show();
           
        }
    }
}
