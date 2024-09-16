using LinqLabs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHomeWork
{
    public partial class Frm作業_1 : Form
    {
        private int _currentPage = 1;
        private int _pageSize = 10;
        public Frm作業_1()
        {
            InitializeComponent();

            this.productsTableAdapter1.Fill(this.nwDataSet1.Products);
            this.ordersTableAdapter1.Fill(this.nwDataSet1.Orders);
            this.order_DetailsTableAdapter1.Fill(this.nwDataSet1.Order_Details);




            var q1 = from o in this.nwDataSet1.Orders
                     select o.OrderDate.Year;


            this.comboBox1.DataSource = q1.Distinct().ToList();

            textBoxPageSize.Text = _pageSize.ToString();

        }

        private void Frm作業_1_Load(object sender, EventArgs e)
        {

            textBoxPageSize.Text = _pageSize.ToString();
            LoadCurrentPage();
        }





        private void LoadCurrentPage()
        {

            int.TryParse(textBoxPageSize.Text, out _pageSize);


            if (_pageSize <= 0)
            {
                _pageSize = 10;
                textBoxPageSize.Text = _pageSize.ToString();
            }

            int skip = (_currentPage - 1) * _pageSize;
            int take = _pageSize;

            var q = from o in nwDataSet1.Orders

                    select o;



            this.dataGridView1.DataSource = q.Skip(skip).Take(take).ToList();

        }
        private void textBoxPageSize_TextChanged(object sender, EventArgs e)
        {

            if (!int.TryParse(textBoxPageSize.Text, out int newSize) || newSize <= 0)
            {

                textBoxPageSize.Text = _pageSize.ToString();
                return;
            }

            _pageSize = newSize;
            LoadCurrentPage();
        }


        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            this.lblDetails.Text = "Order details";


            var OrderID = ((NWDataSet.OrdersRow)this.bindingSource1.Current).OrderID;

            var q = from o in this.nwDataSet1.Order_Details
                    where o.OrderID == OrderID
                    select o;

            this.dataGridView2.DataSource = q.CopyToDataTable();
        }



        private void button14_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files = dir.GetFiles();

            //files[0].CreationTime
            this.dataGridView1.DataSource = files;

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.lblMaster.Text = "Orders";



            var q = from i in this.nwDataSet1.Orders
                    select i;

            this.bindingSource1.DataSource = q;

            this.dataGridView1.DataSource = nwDataSet1.Orders.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");

            System.IO.FileInfo[] files = dir.GetFiles();

            this.lblMaster.Text = "2017 Files";
            IEnumerable<System.IO.FileInfo> q = files
                .Where(finfo => finfo.CreationTime.Year == 2024)
                .OrderBy(finfo => finfo.CreationTime);
            var newinfo = q.ToList();
            this.dataGridView1.DataSource = newinfo;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.lblMaster.Text = "Big Files";
            System.IO.DirectoryInfo dirs = new System.IO.DirectoryInfo(@"c:\windows");
            FileInfo[] files = dirs.GetFiles();

            var q = from f in files
                    where f.Length > 2000
                    select f;
            this.dataGridView1.DataSource = q.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int year;
            int.TryParse(this.comboBox1.Text, out year);

            var q = from o in this.nwDataSet1.Orders
                    where o.OrderDate.Year == year
                    select o;



            this.bindingSource1.DataSource = q.ToList();
            this.dataGridView1.DataSource = this.bindingSource1;
        }



        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {


        }




        private void button13_Click(object sender, EventArgs e)
        {
            _currentPage++;
            LoadCurrentPage();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadCurrentPage();
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.lblDetails.Text = "Order details";


            var OrderID = (int)dataGridView1.Rows[e.RowIndex].Cells["orderid"].Value;

            var q = from o in this.nwDataSet1.Order_Details
                    where o.OrderID == OrderID
                    select o;

            this.dataGridView2.DataSource = q.CopyToDataTable();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int year;
            int.TryParse(this.comboBox1.Text, out year);

            var q = from o in this.nwDataSet1.Orders 
                    where o.OrderDate.Year == year
                    select o;



            this.bindingSource1.DataSource = q.ToList();
            this.dataGridView1.DataSource = this.bindingSource1;
        }
    }
}
