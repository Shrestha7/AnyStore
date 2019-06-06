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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        loginBLL l = new loginBLL();
        loginDAL dal = new loginDAL();
        //for username after login
        public static string loggedIn;

        private void pBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            l.username = txtUsername.Text.Trim();
            l.password = txtPassword.Text.Trim();
            l.user_type = cmbUserType.Text.Trim();

            //Checking login Credentials
            bool sucess = dal.loginCheck(l);
            if (sucess==true)
            {
                //Login Sucessful
                MessageBox.Show("Login Successful.");
                //for username
                loggedIn = l.username;
                //Open respective form based on user_type
                switch (l.user_type)
                {
                    case "Admin":
                        {
                            //Display admin Dashboard
                            FirmAdminDashboard admin = new FirmAdminDashboard();
                            admin.Show();
                            this.Hide();
                        }
                        break;
                    case "User":
                        {
                            //Display User Dashboard
                            firmUserDashboard user = new firmUserDashboard();
                            user.Show();
                            this.Hide();
                        }
                        break;
                    default:
                        {
                            //Display an error Message
                            MessageBox.Show("Invalid User Type.");
                        }
                        break;
                }
            }
            else
            {
                //Login Failed
                MessageBox.Show("Login Failed. Please Try again.");
            }
        }
    }
}
