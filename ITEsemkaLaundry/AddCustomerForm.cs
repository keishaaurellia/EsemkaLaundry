using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ITEsemkaLaundry
{
    public partial class AddCustomerForm : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        public AddCustomerForm()
        {
            InitializeComponent();
        }

        private bool checkAll()
        {
            Regex phone = new Regex("^[+][0-9]+$");

            if (txtCustomerName.Text == "" || txtPhoneNumber.Text == "" || txtAddress.Text == "")
            {
                MessageBox.Show("all field must filled");
                return false;
            }
            else if (!phone.IsMatch(txtPhoneNumber.Text))
            {
                MessageBox.Show("Phone Number not valid!");
                return false;
            }
            return true;
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkAll()) {
                Customer customer = new Customer();
                customer.Name = txtCustomerName.Text;
                customer.PhoneNumber = txtPhoneNumber.Text;
                customer.Address = txtAddress.Text;

                db.Customers.InsertOnSubmit(customer);
                MessageBox.Show("Add Succes!");
                db.SubmitChanges();

                this.Close();
            }
        }
    }
}
