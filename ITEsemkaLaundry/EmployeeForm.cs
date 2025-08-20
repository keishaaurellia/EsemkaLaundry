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
    public partial class EmployeeForm : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        int selectedRow = -1;
        string status = null;

        public EmployeeForm()
        {
            InitializeComponent();
        }

        private void EmployeeForm_Load(object sender, EventArgs e)
        {
            loadDgv();
            enableButton(false);
            loadCmbJob();
            clearField();
        }

        private void loadDgv()
        {
            dgv.Rows.Clear();
 

            IQueryable<Employee> employees = db.Employees.Where(x => x.Name.Contains(txtSearch.Text) || x.PhoneNumber.Contains(txtSearch.Text) || x.Email.Contains(txtSearch.Text));
            foreach (var item in employees)
            {
                dgv.Rows.Add(item.Id, item.Password, item.Name, item.Email, item.PhoneNumber, item.Address, item.DateofBirth, item.Job.Name, item.Salary, item.IdJob);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadDgv();
        }

        private void enableButton (bool enable)
        {
            btnInsert.Enabled = !enable;
            btnUpdate.Enabled = !enable;
            btnDelete.Enabled = !enable;
            btnSave.Enabled = enable;
            btnCancel.Enabled = enable;
        }

        private void clearField()
        {
            txtEmployeeId.Text = "";
            txtPassowrd.Text = "";
            txtConfirmPassword.Text = "";
            txtName.Text = "";
            txtEmail.Text = "";
            txtPhoneNumber.Text = "";
            txtAddress.Text = "";
            dateTimePicker1.Text = "";
            cbJob.Text = "";
            nmSalary.Value = 0;
           
        }

        private void loadCmbJob()
        {
            cbJob.Items.Clear();

            cbJob.DataSource = db.Jobs;
            cbJob.ValueMember = "Id";
            cbJob.DisplayMember = "Name";
        }


        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

                if (selectedRow != -1)
                {
                    txtEmployeeId.Text = dgv.Rows[selectedRow].Cells[0].Value.ToString();
                    txtPassowrd.Text = dgv.Rows[selectedRow].Cells[1].Value.ToString();
                    txtConfirmPassword.Text = dgv.Rows[selectedRow].Cells[1].Value.ToString();
                    txtName.Text = dgv.Rows[selectedRow].Cells[2].Value.ToString();
                    txtEmail.Text = dgv.Rows[selectedRow].Cells[3].Value.ToString();
                    txtPhoneNumber.Text = dgv.Rows[selectedRow].Cells[4].Value.ToString();
                    txtAddress.Text = dgv.Rows[selectedRow].Cells[5].Value.ToString();
                    dateTimePicker1.Value = Convert.ToDateTime(dgv.Rows[selectedRow].Cells[6].Value);
                    cbJob.SelectedValue = Convert.ToInt32(dgv.Rows[selectedRow].Cells[9].Value.ToString());
                    nmSalary.Value = Convert.ToInt32(dgv.Rows[selectedRow].Cells[8].Value);
              }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            enableButton(true);
            status = "insert";
            txtSearch.Enabled = false;
            clearField();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedRow != 1)
            {
                enableButton(true);
                status = "update";
                txtSearch.Enabled = false;
                clearField();
            }
            else
            {
                MessageBox.Show("Slect Row!");
            }
        }

        private bool checkAll()
        {
            Regex regex = new Regex(@"^+.+@.+\..+$");
            Regex phone = new Regex("^[+][0-9]+$");

            if (txtConfirmPassword.Text != txtPassowrd.Text)
            {
                MessageBox.Show("Password tidak sama!");
                return false;
            }
            else if (!phone.IsMatch(txtPhoneNumber.Text))
            {
                MessageBox.Show("Phone Number not valid!");
                return false;
            }
            else if (!regex.IsMatch(txtEmail.Text))
            {
                MessageBox.Show("Format Email ga Valid!");
                return false;
            }
            else if (txtAddress.Text == "" || txtConfirmPassword.Text == "" || txtEmail.Text == "" || txtName.Text == "" || txtPassowrd.Text == "" || txtPhoneNumber.Text == "")
            {
                MessageBox.Show("All field must be filled!");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Employee employee = db.Employees.Where(x => x.Id.Equals(txtEmployeeId.Text)).FirstOrDefault();

            db.Employees.DeleteOnSubmit(employee);
            db.SubmitChanges();
            loadDgv();
            enableButton(false);
            clearField();


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (checkAll())
            {
                if (status == "update")
                {

                    Employee employee = db.Employees.Where(x => x.Id.Equals(txtEmployeeId.Text)).FirstOrDefault();
                    employee.Password = txtPassowrd.Text;
                    employee.Name = txtName.Text;
                    employee.Email = txtEmail.Text;
                    employee.PhoneNumber = txtPhoneNumber.Text;
                    employee.Address = txtAddress.Text;
                    employee.DateofBirth = dateTimePicker1.Value;
                    employee.IdJob = Convert.ToInt32(cbJob.SelectedValue);
                    employee.Salary = Convert.ToInt32(nmSalary.Value);

                    db.SubmitChanges();
                    MessageBox.Show("update succes!");
                    loadDgv();
                    enableButton(false);
                    clearField();
                }
                else if (status == "insert")
                {
                    Employee employee = new Employee();
                    employee.Password = txtPassowrd.Text;
                    employee.Name = txtName.Text;
                    employee.Email = txtEmail.Text;
                    employee.PhoneNumber = txtPhoneNumber.Text;
                    employee.Address = txtAddress.Text;
                    employee.DateofBirth = dateTimePicker1.Value;
                    employee.IdJob = Convert.ToInt32(cbJob.SelectedValue);
                    employee.Salary = Convert.ToInt32(nmSalary.Value);

                    db.Employees.InsertOnSubmit(employee);
                    db.SubmitChanges();
                    MessageBox.Show("insert succes!");
                    loadDgv();
                    enableButton(false);
                    clearField();
                }
            }
           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            enableButton(false);
            clearField();
            txtSearch.Enabled = true;
        }
    }
}
