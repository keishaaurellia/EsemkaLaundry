using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ITEsemkaLaundry
{
    public partial class ServiceForm : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        int selectedRow = -1;
        string status = null;

        public ServiceForm()
        {
            InitializeComponent();
            
        }
        private void ServiceForm_Load(object sender, EventArgs e)
        {
            loadDgv();
            loadCmbCategory();
            clearField();
            enableButton(false);
            loadCmbUnit();

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
            txtServiceId.Text = "";
            txtServiceName.Text = "";
            cbCategory.Text = "";
            cbUnit.Text = "";
            nmEstDur.Text = "";
            nmPrice.Text = "";
        }

        private void loadCmbCategory()
        {
            cbCategory.Items.Clear();

            cbCategory.DataSource = db.Categories;
            cbCategory.ValueMember = "Id";
            cbCategory.DisplayMember = "Name";
        }

        private void loadCmbUnit()
        {
            cbUnit.Items.Clear();

            cbUnit.DataSource = db.Units;
            cbUnit.ValueMember = "Id";
            cbUnit.DisplayMember = "Name";
        }

        private bool checkAll()
        {
            if  (txtServiceName.Text == "" || cbCategory.Text == "" || cbUnit.Text == "" || nmEstDur.Text == "" || nmPrice.Text == "")
            {
                MessageBox.Show("All field must be filled");
                return false;
            }


            return true;
        }


        private void loadDgv()
        {
            dgv.Rows.Clear();

            IQueryable<Service> service = db.Services.Where(x => x.Name.Contains(txtSearch.Text) || x.Category.Name.Contains(txtSearch.Text) || x.Unit.Name.Contains(txtSearch.Text));
            foreach (var item in service)
            {
                dgv.Rows.Add(item.Id, item.Name, item.Category.Name, item.Unit.Name, item.PriceUnit, item.EstimationDuration, item.IdCategory, item.IdUnit);
            }
        }
        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (selectedRow != -1)
            {
                txtServiceId.Text = dgv.Rows[selectedRow].Cells[0].Value.ToString();
                txtServiceName.Text = dgv.Rows[selectedRow].Cells[1].Value.ToString();
                cbCategory.SelectedValue = Convert.ToInt32(dgv.Rows[selectedRow].Cells[6].Value.ToString());
                cbUnit.SelectedValue = Convert.ToInt32(dgv.Rows[selectedRow].Cells[7].Value.ToString());
                nmPrice.Value = Convert.ToInt32(dgv.Rows[selectedRow].Cells[4].Value);
                nmEstDur.Value = Convert.ToInt32(dgv.Rows[selectedRow].Cells[5].Value);
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Service service = db.Services.Where(x => x.Id.Equals(txtServiceId.Text)).FirstOrDefault();

            db.Services.DeleteOnSubmit(service);
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
                    Service service = db.Services.Where(x => x.Id.Equals(txtServiceId.Text)).FirstOrDefault();
                    service.Name = txtServiceName.Text;
                    service.IdCategory = Convert.ToInt32(cbCategory.SelectedValue);
                    service.IdUnit = Convert.ToInt32(cbUnit.SelectedValue);
                    service.PriceUnit = Convert.ToInt32(nmPrice.Value);
                    service.EstimationDuration = Convert.ToInt32(nmEstDur.Value);

                    db.SubmitChanges();
                    MessageBox.Show("Update succes!");
                    loadDgv();
                    enableButton(false);
                    clearField();
                }
                else if (status == "insert")
                {
                    Service service = new Service();
                    service.Name = txtServiceName.Text;
                    service.IdCategory = Convert.ToInt32(cbCategory.SelectedValue);
                    service.IdUnit = Convert.ToInt32(cbUnit.SelectedValue);
                    service.PriceUnit = Convert.ToInt32(nmPrice.Value);
                    service.EstimationDuration = Convert.ToInt32(nmEstDur.Value);

                    db.Services.InsertOnSubmit(service);
                    db.SubmitChanges();
                    MessageBox.Show("insert succes!");
                    loadDgv();
                    enableButton(false);
                    clearField();

                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadDgv();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            enableButton(false);
            clearField();
            txtSearch.Enabled = true;
        }

    }
    }

