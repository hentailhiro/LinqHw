using LinqLabs.EFAadventure2019;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHomeWork
{
    public partial class Frm作業_2 : Form
    {

        AdventureWorks2019Entities _dbContext = new AdventureWorks2019Entities();
        public Frm作業_2()
        {
            InitializeComponent();
            var query =
            from pp in _dbContext.ProductPhotoes
            join p in _dbContext.ProductProductPhotoes on pp.ProductPhotoID equals p.ProductPhotoID
            join pm in _dbContext.Products on p.ProductID equals pm.ProductID
            join ps in _dbContext.ProductSubcategories on pm.ProductSubcategoryID equals ps.ProductSubcategoryID
            join pc in _dbContext.ProductCategories on ps.ProductCategoryID equals pc.ProductCategoryID
            where pc.ProductCategoryID == 1
            select pp.ModifiedDate.Year;

            comboBox3.DataSource = query.Distinct().ToList();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void button11_Click(object sender, EventArgs e)
        {
            var query =
            from pp in _dbContext.ProductPhotoes
            join p in _dbContext.ProductProductPhotoes on pp.ProductPhotoID equals p.ProductPhotoID
            join pm in _dbContext.Products on p.ProductID equals pm.ProductID
            join ps in _dbContext.ProductSubcategories on pm.ProductSubcategoryID equals ps.ProductSubcategoryID
            join pc in _dbContext.ProductCategories on ps.ProductCategoryID equals pc.ProductCategoryID
            where pc.ProductCategoryID == 1
            select new
            {
                pp.ProductPhotoID,
                pm.Name,
                pp.ThumbNailPhoto,
                pp.LargePhoto,
                pp.ModifiedDate


            };
            this.dataGridView1.DataSource = query.OrderBy(x => x.ProductPhotoID).ToList();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.dateTimePicker1.Value != null && dateTimePicker2.Value != null)
            {
                DateTime startDate = dateTimePicker1.Value.Date;
                DateTime endDate = dateTimePicker2.Value.Date;


                if (endDate < startDate)
                {
                    MessageBox.Show("結束日期不可大於開始日期.");
                    return;
                }

                var qDatePick =
                 from pp in _dbContext.ProductPhotoes
                 join p in _dbContext.ProductProductPhotoes on pp.ProductPhotoID equals p.ProductPhotoID
                 join pm in _dbContext.Products on p.ProductID equals pm.ProductID
                 join ps in _dbContext.ProductSubcategories on pm.ProductSubcategoryID equals ps.ProductSubcategoryID
                 join pc in _dbContext.ProductCategories on ps.ProductCategoryID equals pc.ProductCategoryID
                 where pc.ProductCategoryID == 1 &&
                       pp.ModifiedDate >= startDate &&
                       pp.ModifiedDate <= endDate
                 select new
                 {
                     pp.ProductPhotoID,
                     pm.Name,
                     pp.ThumbNailPhoto,
                     pp.LargePhoto,
                     pp.ModifiedDate


                 };

                if (qDatePick.Any())
                {

                    this.dataGridView1.DataSource = qDatePick.ToList();

                }
                else
                {

                    MessageBox.Show("没有符合。");
                    this.dataGridView1.DataSource = null;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int selectedYear = Convert.ToInt32(comboBox3.Text);
            var qDatePick =
               from pp in _dbContext.ProductPhotoes
               join p in _dbContext.ProductProductPhotoes on pp.ProductPhotoID equals p.ProductPhotoID
               join pm in _dbContext.Products on p.ProductID equals pm.ProductID
               join ps in _dbContext.ProductSubcategories on pm.ProductSubcategoryID equals ps.ProductSubcategoryID
               join pc in _dbContext.ProductCategories on ps.ProductCategoryID equals pc.ProductCategoryID
               where pc.ProductCategoryID == 1 && (
                     pp.ModifiedDate.Year == selectedYear)


               select new
               {
                   pp.ProductPhotoID,
                   pm.Name,
                   pp.ThumbNailPhoto,
                   pp.LargePhoto,
                   pp.ModifiedDate


               };

            this.dataGridView1.DataSource = qDatePick.ToList();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string seasonText = comboBox2.Text.ToString();


            int startMonth, endMonth;


            switch (seasonText)
            {
                case "第一季":
                    startMonth = 1;
                    endMonth = 3;
                    break;
                case "第二季":
                    startMonth = 4;
                    endMonth = 6;
                    break;
                case "第三季":
                    startMonth = 7;
                    endMonth = 9;
                    break;
                case "第四季":
                    startMonth = 10;
                    endMonth = 12;
                    break;
                default:
                    startMonth = 1;
                    endMonth = 12;
                    break;
            }



            var qDatePick =
               from pp in _dbContext.ProductPhotoes
               join p in _dbContext.ProductProductPhotoes on pp.ProductPhotoID equals p.ProductPhotoID
               join pm in _dbContext.Products on p.ProductID equals pm.ProductID
               join ps in _dbContext.ProductSubcategories on pm.ProductSubcategoryID equals ps.ProductSubcategoryID
               join pc in _dbContext.ProductCategories on ps.ProductCategoryID equals pc.ProductCategoryID
               where pc.ProductCategoryID == 1 && (pp.ModifiedDate.Month >= startMonth && pp.ModifiedDate.Month <= endMonth)


               select new
               {
                   pp.ProductPhotoID,
                   pm.Name,
                   pp.ThumbNailPhoto,
                   pp.LargePhoto,
                   pp.ModifiedDate


               };

            this.dataGridView1.DataSource = qDatePick.ToList();
        }



        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            byte[] photo = (byte[])dataGridView1.Rows[e.RowIndex].Cells["LargePhoto"].Value;
            MemoryStream ms = new MemoryStream(photo);
            Image img = Image.FromStream(ms);
            this.pictureBox1.Image = img;
        }



    }
}
