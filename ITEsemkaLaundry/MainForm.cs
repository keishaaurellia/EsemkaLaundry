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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void emloyeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmployeeForm employeeForm = new EmployeeForm();
            employeeForm.MdiParent = this;
            employeeForm.Show();
           
        }

        private void serviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ServiceForm service = new ServiceForm();
            service.MdiParent = this;
            service.Show();
        }

        private void packageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PackageForm package = new PackageForm();
            package.MdiParent = this;
            package.Show();

        }

        private void transactionDepositToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TransactionDepositForm transactionDepositForm = new TransactionDepositForm();
            transactionDepositForm.MdiParent = this;
            transactionDepositForm.Show();
        }

        private void viewTransactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewTransactionForm viewTransactionForm = new ViewTransactionForm();
            viewTransactionForm.MdiParent = this;
            viewTransactionForm.Show();
        }

        private void prepaidPackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrepaidPackageForm prepaidPackageForm = new PrepaidPackageForm();
            prepaidPackageForm.MdiParent = this;
            prepaidPackageForm.Show();
        }
    }
}
