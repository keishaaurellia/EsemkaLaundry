using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ITEsemkaLaundry
{
    public partial class LoginForm : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Employee employee = db.Employees.Where(x => x.Email.Equals(txtEmail.Text) && x.Password.Equals(txtPassword.Text)).FirstOrDefault();

            if (employee != null) { 

                ClassDataSource.EmployeeId = employee.Id;



                MainForm mainForm = new MainForm();
                mainForm.Show();

            }
            else
            {
                MessageBox.Show("Please Try Again, Your Data is not Valid!");
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
