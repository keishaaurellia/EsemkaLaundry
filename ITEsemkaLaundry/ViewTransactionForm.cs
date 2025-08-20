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
    public partial class ViewTransactionForm : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        int selectedRow = -1;
        int selectedRow2 = -1;

        List<object> list = null;

        public ViewTransactionForm()
        {
            InitializeComponent();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ViewTransactionForm_Load(object sender, EventArgs e)
        {
                loadDgv();
        }


        private void loadDgv()
        {
            dataGridView1.Rows.Clear();
           

            IQueryable<HeaderDeposit> headerDeposits = db.HeaderDeposits.Where(x => x.Customer.Name.Contains(txtSearch.Text) || x.Employee.Name.Contains(txtSearch.Text) || x.TransactionDatetime.ToString().Contains(txtSearch.Text));

            foreach (var item in headerDeposits)
            {
                list = new List<object>
                {
                    item.Id,
                    item.IdCustomer,
                    item.Customer.Name,
                    item.Employee.Name,
                    item.TransactionDatetime,
                    item.CompleteEstimationDatetime
                };
                dataGridView1.Rows.Add(list.ToArray());
            }

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadDgv();
        }

        private void loadDgv2()
        {

            int headerId = (int)dataGridView1.Rows[selectedRow].Cells[0].Value;

            var detailDepositData = db.DetailDeposits.Where(x => x.IdDeposit == headerId).Select(x =>
            new {

                x.Id,
                x.IdService,
                x.Service.Name,
                x.IdPrepaidPackage,
                x.TotalUnit,
                x.CompletedDatetime
            });

            dataGridView2.DataSource = detailDepositData;
            DataGridViewButtonColumn dataGridViewButtonColumn = new DataGridViewButtonColumn();
            dataGridViewButtonColumn.HeaderText = "Action";
            dataGridViewButtonColumn.Text = "Complete";
            dataGridViewButtonColumn.UseColumnTextForButtonValue = true;
            dataGridView2.Columns.Add(dataGridViewButtonColumn);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (selectedRow != -1)
            {

                dataGridView2.Rows.Clear();
                dataGridView2.Columns.Clear();
                loadDgv2();
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow2 = e.RowIndex;

            if (selectedRow2 != -1) 
            { 
                if (e.ColumnIndex == 6)
                {
                   DetailDeposit detailDeposit = db.DetailDeposits.Where(x => x.Id.Equals(dataGridView2.Rows[selectedRow2].
                       Cells[0].Value.ToString())).FirstOrDefault();
                    if (detailDeposit != null) { 
                    detailDeposit.CompletedDatetime = DateTime.Now; 

                    db.SubmitChanges();

                        dataGridView2.Rows.Clear();
                        dataGridView2.Columns.Clear();
                        loadDgv2();
                    }
                }
            }
        }
    }
}
