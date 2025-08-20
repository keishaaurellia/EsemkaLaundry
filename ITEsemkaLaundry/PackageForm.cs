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
    public partial class PackageForm : Form
    {

        DataClasses1DataContext db = new DataClasses1DataContext();
        int selectedRow = -1;
        string status = null;

        public PackageForm()
        {
            InitializeComponent();
        }

        private void PackageForm_Load(object sender, EventArgs e)
        {
            loadDgv();
            loadService();
        }

        private void loadDgv()
        {
            dgv.Rows.Clear();
            
            IQueryable<Package> packages = db.Packages.Where(x => x.Service.Name.Contains(txtSearch.Text) || x.TotalUnit.ToString().Contains(txtSearch.Text) || x.Price.ToString().Contains(txtSearch.Text));

            foreach (Package item in packages)
            {
                dgv.Rows.Add(item.Id, item.Service.Name, item.TotalUnit, item.Price, item.IdService);
            }
        }

        private void loadService()
        {
            cbService.Items.Clear();

            cbService.DataSource = db.Services;
            cbService.ValueMember = "Id";
            cbService.DisplayMember = "Name";
        }

        private void enableButton(bool enable)
        {
            btnInsert.Enabled = !enable;
            btnUpdate.Enabled = !enable;
            btnDelete.Enabled = !enable;
            btnSave.Enabled = enable;
            btnCancel.Enabled = enable;
        }


        private void clearField()
        {
            txtPackageId.Text = "";
            cbService.Text = "";
            nmPrice.Text = "";
            nmTotalUnit.Text = "";
        }

        private bool checkAll()
        {
            if (cbService.Text == "" || nmPrice.Text == "" || nmTotalUnit.Text == "")
            {
                MessageBox.Show("All field must be filled");
                return false;
            }


            return true;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadDgv();
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (selectedRow != -1) {
                txtPackageId.Text = dgv.Rows[selectedRow].Cells[0].Value.ToString();
                cbService.SelectedValue = Convert.ToInt32(dgv.Rows[selectedRow].Cells[4].Value.ToString());
                nmTotalUnit.Value = Convert.ToDecimal(dgv.Rows[selectedRow].Cells[2].Value.ToString());
                nmPrice.Value = Convert.ToInt32(dgv.Rows[selectedRow].Cells[3].Value);
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
                MessageBox.Show("Select Row!");
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Package package = db.Packages.Where(x => x.Id.Equals(txtPackageId)).FirstOrDefault();

            db.Packages.DeleteOnSubmit(package);
            db.SubmitChanges();
            loadDgv();
            enableButton(false);    
            clearField();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (checkAll())
            {
                if(status == "update")
                {
                    Package package = db.Packages.Where(x => x.Id.Equals(txtPackageId.Text)).FirstOrDefault();
                    package.IdService = Convert.ToInt32(cbService.SelectedValue);
                    package.TotalUnit = Convert.ToInt32(nmTotalUnit.Value);
                    package.Price = Convert.ToInt32(nmPrice.Value);

                    db.SubmitChanges();
                    MessageBox.Show("Update Succes!");
                    loadDgv();
                    enableButton(false);
                    clearField();
                } 
                else if (status == "insert")
                {
                    Package package = new Package();
                    package.IdService = Convert.ToInt32(cbService.SelectedValue);
                    package.TotalUnit = Convert.ToInt32(nmTotalUnit.Value);
                    package.Price = Convert.ToInt32(nmPrice.Value);

                    db.Packages.InsertOnSubmit(package);
                    db.SubmitChanges();
                    MessageBox.Show("Insert succes!");
                    loadDgv();
                    enableButton(false);
                    clearField();
                }
            }
        }

        private void cbService_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

