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
    public partial class TransactionDepositForm : Form
    {
        private int customerId = -1;
        private DateTime? estimationDateTime = null;
       
        DataClasses1DataContext db = new DataClasses1DataContext();


        public TransactionDepositForm()
        {
            InitializeComponent();
        }

        private void TransactionDepositForm_Load(object sender, EventArgs e)
        {
            loadCB();
            timer1.Start();
        }

        private void loadCB()
        {
            cbService.Items.Clear();

            cbService.DataSource = db.Services;
            cbService.ValueMember = "Id";
            cbService.DisplayMember = "Name";
        }

        private bool checkAll()
        {
            if (cbService.Text == "" || txtPricePerUnit.Text == "")
            {
                MessageBox.Show("All Field must filled!");
                return false;
            }

            return true;
        }
        private void estimationTime()
        {
            int a = 0;

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                a += Convert.ToInt32(dgv.Rows[i].Cells[5].Value) * Convert.ToInt32(dgv.Rows[i].Cells[8].Value);
            }

            TimeSpan result = TimeSpan.FromHours(a);
            string fromTimeString = result.ToString();
            estimationDateTime = DateTime.Now + result;

            estimationPay.Text = $"Estimation Time: {fromTimeString}";
        }

        private void generateTotal()
        {
            int a = 0;

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                a += Convert.ToInt32(dgv.Rows[i].Cells[4].Value);
            }

            totalPay.Text = $"Total Pay: {a.ToString("N")}";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            

            if (!checkAll()) return;

            string selectedService = cbService.SelectedItem?.ToString() ?? "";
            int pricePerUnit = int.Parse(txtPricePerUnit.Text);
            double totalUnit = (double)nmTotalUnit.Value;
            int subtotal = pricePerUnit * (int)totalUnit;

            string prepaidPackage = "";
            int? prepaidpackageid = null;


            if (checkBox1.Checked  && checkBox1.Tag is PrepaidPackage prepaidPackage1)
            {
                prepaidpackageid = prepaidPackage1.Id;

               

                var prepaidService = db.Packages.FirstOrDefault(x => x.Id == prepaidPackage1.IdPackage);

                if (prepaidService != null)
                    prepaidPackage = prepaidService.Service.Name;
                    
            }

            
            dgv.Rows.Add(cbService.SelectedValue, prepaidpackageid, cbService.Text, prepaidPackage, pricePerUnit, totalUnit, subtotal, null);
            generateTotal();
            estimationTime();
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7)
            {
                dgv.Rows.RemoveAt(e.RowIndex);
                generateTotal();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AddCustomerForm addCustomerForm = new AddCustomerForm();
            addCustomerForm.Show();
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

                    var customerPrepaidPackage = db.PrepaidPackages.Where(x => x.IdCustomer == cus.Id && x.CompletedDatetime == null).FirstOrDefault();
                    if (customerPrepaidPackage != null)
                    {
                        checkBox1.Enabled = true;
                        checkBox1.Tag = customerPrepaidPackage;
                    }
                    else
                    {
                        checkBox1.Enabled = false;
                    }
                }
                else
                {
                    MessageBox.Show("There's no customer!");
                }
            }


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            txtCurrentTime.Text = $"Current time : {DateTime.Now.ToString("dd-MMM-yy HH:mm:ss")}";
        }

        private void txtPhoneNumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (customerId == -1 || ClassDataSource.EmployeeId == -1)
                {
                    MessageBox.Show("Invalid Customer or Employee ID.");
                    return;
                }

                else
                {
                    DataClasses1DataContext db = new DataClasses1DataContext();

                    HeaderDeposit headerDeposit = new HeaderDeposit();
                    headerDeposit.IdCustomer = customerId;
                    headerDeposit.TransactionDatetime = DateTime.Now;
                    headerDeposit.IdEmployee = 1;
                    headerDeposit.CompleteEstimationDatetime = estimationDateTime;

                    db.HeaderDeposits.InsertOnSubmit(headerDeposit);
                    db.SubmitChanges();


                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        DetailDeposit detailDeposit = new DetailDeposit();


                        detailDeposit.IdDeposit = headerDeposit.Id;
                        detailDeposit.IdService = Convert.ToInt32(dgv.Rows[i].Cells[0].Value);
                        detailDeposit.PriceUnit = Convert.ToInt32(dgv.Rows[i].Cells[4].Value);
                        detailDeposit.TotalUnit = Convert.ToInt32(dgv.Rows[i].Cells[5].Value);
                        detailDeposit.CompletedDatetime = null;

                        if (dgv.Rows[i].Cells[1].Value != null)
                        {
                            detailDeposit.IdPrepaidPackage = Convert.ToInt32(dgv.Rows[i].Cells[1].Value);
                        }

                        db.DetailDeposits.InsertOnSubmit(detailDeposit);
                        db.SubmitChanges();

                    }
                    MessageBox.Show("Insert Success!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
         
        }
    }
}
