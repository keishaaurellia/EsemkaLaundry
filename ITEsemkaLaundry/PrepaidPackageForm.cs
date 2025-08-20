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
    public partial class PrepaidPackageForm : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        private int customerId = -1;

        public PrepaidPackageForm()
        {
            InitializeComponent();
        }



        private void loadCmb()
        {
            var package = db.Packages.ToList();
            cbPackage.ValueMember = "Id";
            cbPackage.DisplayMember = "Name";

            cbPackage.DataSource = package;
        }

        private void loadCmbPackage()
        {
            cbPackage.Items.Clear();
            //CARA 1
            /*cbPackage.DataSource = db.PrepaidPackages.Select(x => x.Package.Service.Name);
            
            cbPackage.ValueMember = "Id";
            cbPackage.DisplayMember = "Name";*/

            //CARA 2
            var query = db.PrepaidPackages.Select(x => x.Package.Service.Name);

            cbPackage.DataSource = query;
        }

        private void PrepaidPackageForm_Load(object sender, EventArgs e)
        {
            loadDgv();
            //loadCmb();
            loadCmbPackage();
        }


        private void loadDgv()
        {
            dataGridView1.Rows.Clear();

            IQueryable<PrepaidPackage> prepaidPackages = db.PrepaidPackages
                .Where(x => x.Customer.Name.Contains(txtSearch.Text) ||
                             x.Package.Service.Name.Contains(txtSearch.Text) ||
                             x.Package.TotalUnit.ToString().Contains(txtSearch.Text));

            foreach (var pp in prepaidPackages)
            {
                dataGridView1.Rows.Add(pp.Id,
                             pp.Customer.Name,
                             pp.Package.Service.Name + " " + pp.Package.TotalUnit + " " + 
                             pp.Package.Service.Unit.Name,
                             pp.Price);
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadDgv();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            PrepaidPackage prepaidPackage = new PrepaidPackage();
            prepaidPackage.IdCustomer = customerId;
            prepaidPackage.IdPackage = Convert.ToInt32(cbPackage.SelectedIndex + 1);
            MessageBox.Show(cbPackage.SelectedIndex.ToString() + 1);
            prepaidPackage.Price = Convert.ToInt32(nmPrice.Value);
            prepaidPackage.StartDateTime = DateTime.Now;    
            prepaidPackage.CompletedDatetime = null;

            db.PrepaidPackages.InsertOnSubmit(prepaidPackage);
            db.SubmitChanges();
            MessageBox.Show("Submit Succes!");
            loadDgv();
        }

        private void txtPhoneNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Customer cus = db.Customers.Where(x => x.PhoneNumber.Equals(txtPhoneNumber.Text)).FirstOrDefault();
                if (cus != null)
                {
                    addressChange.Text = cus.Address;
                    nameChange.Text = cus.Name;
                    customerId = cus.Id;
                }
                else
                {
                    MessageBox.Show("There's no customer!");
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AddCustomerForm addCustomerForm = new AddCustomerForm();
            addCustomerForm.Show();
        }
    }
}
