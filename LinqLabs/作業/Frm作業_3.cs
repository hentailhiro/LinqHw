﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinqLabs.作業
{
    public partial class Frm作業_3 : Form
    {

        private List<Student> students_scores;
        private List<StudentAverage> _AvgScore;
        NorthwindEntities _dbContext = new NorthwindEntities();

        public Frm作業_3()
        {
            InitializeComponent();


            students_scores = new List<Student>()
                                         {
                                            new Student{ Name = "aaa", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Male" },
                                            new Student{ Name = "bbb", Class = "CS_102", Chi = 80, Eng = 80, Math = 100, Gender = "Male" },
                                            new Student{ Name = "ccc", Class = "CS_101", Chi = 60, Eng = 50, Math = 75, Gender = "Female" },
                                            new Student{ Name = "ddd", Class = "CS_102", Chi = 80, Eng = 70, Math = 85, Gender = "Female" },
                                            new Student{ Name = "eee", Class = "CS_101", Chi = 80, Eng = 80, Math = 50, Gender = "Female" },
                                            new Student{ Name = "fff", Class = "CS_102", Chi = 80, Eng = 80, Math = 80, Gender = "Female" },

                                          };


        }

        private void button36_Click(object sender, EventArgs e)
        {
            #region 搜尋 班級學生成績

            // 
            // 共幾個 學員成績 ?						

            // 找出 前面三個 的學員所有科目成績					
            // 找出 後面兩個 的學員所有科目成績					

            // 找出 Name 'aaa','bbb','ccc' 的學員國文英文科目成績						

            // 找出學員 'bbb' 的成績	                          

            // 找出除了 'bbb' 學員的學員的所有成績 ('bbb' 退學)	

            // 找出 'aaa', 'bbb' 'ccc' 學員 國文數學兩科 科目成績  |				
            // 數學不及格 ... 是誰 
            #endregion

            #region 學生搜尋

            var Count = students_scores.Select(student => new
            {
                student.Name,
                student.Chi,
                student.Eng,
                student.Math
            }).Count();



            //前三個
            var topThree = students_scores.Take(3).Select(student => new
            {
                student.Name,
                student.Chi,
                student.Eng,
                student.Math
            });
            //後兩個
            var lastTwo = students_scores.Skip(Math.Max(0, students_scores.Count() - 2))
                .Select(student => new
                {
                    student.Name,
                    student.Chi,
                    student.Eng,
                    student.Math
                });
            //找出 Name 'aaa','bbb','ccc' 的學員國文英文科目成績

            var names = new[] { "aaa", "bbb", "ccc" };

            var selectedStudents = from student in students_scores
                                   where names.Contains(student.Name)
                                   select new
                                   {
                                       student.Name,
                                       student.Chi,
                                       student.Eng
                                   };
            //3.0 lambda

            var selectedStudentslamda = students_scores
            .Where(student => names.Contains(student.Name))
            .Select(student => new
            {
                student.Name,
                student.Chi,
                student.Eng
            });


            //找出找出學員 'bbb' 的成績

            var bbbScore = students_scores.Where(student => student.Name == "bbb").
                Select(student => new
                {
                    student.Name,
                    student.Chi,
                    student.Eng,
                    student.Math
                });


            // 找出除了 'SelectListEmp' 學員的學員的所有成績 ('SelectListEmp' 退學)	
            var existbbb = from student in students_scores
                           where student.Name != "bbb"
                           select new
                           {
                               student.Name,
                               student.Chi,
                               student.Eng,
                               student.Math
                           };

            // 找出 'aaa', 'bbb' 'ccc' 學員 國文數學兩科 科目成績
            var selectedabcsocre = from student in students_scores
                                   where names.Contains(student.Name)
                                   select new
                                   {
                                       student.Name,
                                       student.Chi,
                                       student.Math
                                   };


            //數學不及格... 是誰
            var Mathlessthan60 = from student in students_scores
                                 where student.Math < 60
                                 select new
                                 {
                                     student.Name,
                                     student.Chi,
                                     student.Eng,
                                     student.Math
                                 };

            //3.0 lambda
            var Mathlessthan60lam = students_scores
            .Where(student => student.Math < 60)
            .Select(student => new
            {
                student.Name,
                student.Chi,
                student.Eng,
                student.Math
            });
            #endregion

            var classscores = students_scores
            .GroupBy(f => f.Class)
            .OrderBy(g => g.Key)
            .Select(g => new
            {
                Class = g.Key,
                Files = g.ToList()
            })
            .ToList();
            treeView1.Nodes.Clear();
            foreach (var group in classscores)
            {
                TreeNode node = treeView1.Nodes.Add(group.Class);


                foreach (var item in group.Files)
                {

                    node.Nodes.Add($"{item.Name.ToString()}/{item.Gender.ToString()}/國{item.Chi.ToString()}/英{item.Eng.ToString()}/數{item.Math.ToString()}");

                }
            }

        }

        private void button37_Click(object sender, EventArgs e)
        {
            #region 學生搜尋
            //個人 sum, min, max, avg
            var studentScores = students_scores
            .Select(student => new
            {
                student.Name,
                Sum = student.Chi + student.Eng + student.Math,
                Min = new[] { student.Chi, student.Eng, student.Math }.Min(),
                Max = new[] { student.Chi, student.Eng, student.Math }.Max(),
                Avg = new[] { student.Chi, student.Eng, student.Math }.Average()
            });

            //3.0 lambda
            var studentScoreslambda = students_scores
            .Select(student => new
            {
                student.Name,
                Sum = student.Chi + student.Eng + student.Math,
                Min = new[] { student.Chi, student.Eng, student.Math }.Min(),
                Max = new[] { student.Chi, student.Eng, student.Math }.Max(),
                Avg = new[] { student.Chi, student.Eng, student.Math }.Average()
            });
            // 統計 每個學生個人成績 並排序

            var sortedStudentScores = students_scores
            .Select(student => new
            {
                student.Name,
                Sum = student.Chi + student.Eng + student.Math
            })
            .OrderByDescending(s => s.Sum);
            #endregion
            var studentscores = students_scores
               .Select(student => new
               {
                   student.Name,
                   student.Class,
                   student.Gender,
                   student.Chi,
                   student.Eng,
                   student.Math,
                   Avg = new[] { student.Chi, student.Eng, student.Math }.Average()
               })
               .GroupBy(f => new { f.Name, f.Avg })
               .OrderByDescending(s => s.Key.Avg)
               .Select(g => new
               {
                   g.Key.Name,
                   g.Key.Avg,
                   Files = g.ToList()
               })
               .ToList();
            treeView1.Nodes.Clear();
            foreach (var group in studentscores)
            {
                TreeNode node = treeView1.Nodes.Add($"{group.Name} Avg:{group.Avg:n}");

                foreach (var item in group.Files)
                {

                    node.Nodes.Add($"{item.Gender.ToString()}");
                    node.Nodes.Add($"國{item.Chi.ToString()}");
                    node.Nodes.Add($"英{item.Eng.ToString()}");
                    node.Nodes.Add($"數{item.Math.ToString()}");
                }
            }

        }

        public class Student
        {
            public string Name { get; set; }
            public string Class { get; set; }

            public int Chi { get; set; }

            public int Eng { get; set; }
            public int Math { get; set; }

            public string Gender { get; set; }
        }

        public class StudentAverage
        {
            public string Name { get; set; }
            public double AverageScore { get; set; }
        }

        private void button33_Click(object sender, EventArgs e)
        {
            // split=> 分成 三群 '待加強'(60~69) '佳'(70~89) '優良'(90~100) 
            // print 每一群是哪幾個 ? (每一群 sort by 分數 descending)

            _AvgScore = students_scores.Select(s => new StudentAverage
            {
                Name = s.Name,
                AverageScore = new[] { s.Chi, s.Eng, s.Math }.Average()
            }).ToList();





            var GroupScores1 = _AvgScore
            .GroupBy(student => EvaluateMath(student.AverageScore))
            .Select(group => new
            {

                評價 = group.Key,
                Students = group.OrderByDescending(x => x.AverageScore)


            });

            foreach (var group in GroupScores1)
            {
                TreeNode node = treeView1.Nodes.Add(group.評價.ToString());


                foreach (var item in group.Students)
                {

                    node.Nodes.Add(item.Name.ToString() + " 平均分數   " + item.AverageScore.ToString());

                }



            }
        }

        private static object EvaluateMath(double score)
        {
            if (score >= 90 && score <= 100)
            {
                return "優良";
            }
            else if (score >= 70 && score <= 89)
            {
                return "佳";
            }
            else if (score >= 60 && score <= 69)
            {
                return "待加強";
            }
            else
            {
                return "不合格";
            }
        }



        private object BigFile(FileInfo f)
        {
            if (f.Length >= 2000)
            {
                return "大";
            }
            else if (f.Length >= 1000)
            {
                return "中";
            }
            else
            {
                return "小";
            }

        }
        private void button8_Click(object sender, EventArgs e)
        {

            var q = this._dbContext.Products.AsEnumerable().GroupBy(p => PriceCategory(p.UnitPrice.Value)).Select(
                g => new
                {
                    Price = g.Key,
                    Count = g.Count()
                });
            this.dataGridView1.DataSource = q.ToList();
        }

        private string PriceCategory(decimal? n)
        {


            if (!n.HasValue)
            { return "unknown"; }

            switch (n)
            {
                case decimal n1 when n1 < 10:
                    return "Low";
                case decimal n2 when n2 >= 10 && n2 < 30:
                    return "Medium";
                default:
                    return "High";
            }

        }

        private void button15_Click(object sender, EventArgs e)
        {
            var q = this._dbContext.Orders.AsEnumerable().GroupBy(p => p.OrderDate.Value.Year).Select(
               g => new
               {
                   Year = g.Key,
                   Count = g.Count()
               });
            this.dataGridView1.DataSource = q.ToList();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var groupedOrders = this._dbContext.Orders
                .AsEnumerable()
                .GroupBy(p => p.OrderDate.Value.Year)
                .Select(yearGroup => new
                {

                    Year = yearGroup.Key,
                    Months = yearGroup
                .GroupBy(p => p.OrderDate.Value.Month)
                .Select(monthGroup => new
                {
                    Month = monthGroup.Key,
                    Count = monthGroup.Count()

                })
                .OrderBy(m => m.Month)
                .ToList()
                }).ToList();
            this.treeView1.Nodes.Clear();
            foreach (var group in groupedOrders)
            {
                TreeNode node = treeView1.Nodes.Add(group.Year.ToString());
                foreach (var item in group.Months)
                {
                    node.Nodes.Add("第" + item.Month.ToString() + "月" + "   " + "有" + item.Count.ToString() + "筆");
                }
            }


        }

        //AsEnumerable()將資料加載到記憶體中進行計算。這意味著在進行複雜計算之前先執行查詢，然後在記憶體中進行計算
        private void button2_Click(object sender, EventArgs e)
        {
            //使用join
            //var y = (from b in this._dbContext.Products
            //         join c in this._dbContext.Order_Details on b.ProductID equals c.ProductID
            //         group new { b.ProductName, TotalPrice = c.UnitPrice * c.Quantity } by b.ProductName into g
            //         select new
            //         {
            //             Product = g.Key,
            //             TotalPrice = g.Sum(y => y.TotalPrice) // 累計每個產品的總價格
            //         })
            //.OrderBy(y => y.TotalPrice); // 按 TotalPrice 升序排序


            //使用導覽entiyframework
            var p = this._dbContext.Products.AsEnumerable()
            .Select(x => new
            {
                Product = x.ProductName,
                TotalPrice = Math.Round(x.Order_Details.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)))
            })
            .OrderBy(x => x.TotalPrice)
            .ToList();


            this.dataGridView1.DataSource = p.ToList();

        }

        private void button1_Click(object sender, EventArgs e)
        {


            var topSalespeople = this._dbContext.Employees.AsEnumerable()
            .Join(this._dbContext.Orders,

            employee => employee.EmployeeID,// 連接鍵：Employee 表格的 EmployeeID

            order => order.EmployeeID,// 連接鍵：Order 表格的 EmployeeID

            (employee, order) => new { employee, order })// 連接結果的匿名類型，包含 employee 和 order 兩個部分
            .Join(this._dbContext.Order_Details,
            eo => eo.order.OrderID,// 連接鍵：Order 表格的 OrderID
            detail => detail.OrderID,// 連接鍵：OrderDetail 表格的 OrderID
            (eo, detail) => new { eo.employee, detail })// 連接結果的匿名類型，包含 employee 和 detail 兩個部分
            .GroupBy(ed => new { ed.employee.EmployeeID, ed.employee.FirstName, ed.employee.LastName })
            .Select(g => new
            {
                g.Key.EmployeeID,
                Name = g.Key.FirstName + "  " + g.Key.LastName,
                TotalSales = g.Sum(ed => ed.detail.UnitPrice * ed.detail.Quantity * (decimal)(1 - ed.detail.Discount))
            })
            .OrderByDescending(r => r.TotalSales)
            .Take(5)
            .ToList();


            //嘗試用selectmany修復中

            //var topSalespeople = this._dbContext.Employees.AsEnumerable()
            //.SelectMany(employee => this._dbContext.Orders
            //  .Where(order => order.EmployeeID == employee.EmployeeID)
            //   .SelectMany(order => this._dbContext.Order_Details
            //    .Where(detail => detail.OrderID == order.OrderID)
            //     .Select(detail => new
            //     {
            //         employee.EmployeeID,
            //         employee.FirstName,
            //         employee.LastName,
            //         TotalSales = (detail.UnitPrice * detail.Quantity * (decimal)(1 - detail.Discount)) 
            //     })))
            //      .GroupBy(w => new { w.EmployeeID, w.FirstName, w.LastName })
            //       .Select(g => new
            //       {
            //           g.Key.EmployeeID,
            //           Name = g.Key.FirstName + " " + g.Key.LastName,
            //           TotalSales = g.Sum(y => y.TotalSales) 
            //       })
            //        .OrderByDescending(r => r.TotalSales)
            //         .Take(5)
            //           .ToList();





            this.dataGridView1.DataSource = topSalespeople.ToList();





        }

        private void SelectListEmp(IEnumerable<dynamic> list)
        {
            foreach (dynamic item in list)
            {
                int Employee = item.EmployeeID;
                string Name = item.Name;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var topFive = this._dbContext.Order_Details.AsEnumerable()
            .Select(y => new
            {
                Name = y.Order.Customer.ContactName.ToString(),
                ProductName = y.Product.ProductName,
                TotalPrice = y.UnitPrice * y.Quantity * (1 - (decimal)y.Discount)
            })
            .OrderByDescending(d => d.TotalPrice)
            .Take(5)
            .ToList();

            this.dataGridView1.DataSource = topFive;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            var x = this._dbContext.Order_Details.AsEnumerable().Where(p => p.UnitPrice * p.Quantity > 300).Select(y => new
            {
                Name = y.Order.Customer.ContactName.ToString(),

                TotalPrice = y.UnitPrice * y.Quantity * (1 - (decimal)y.Discount)
            }).OrderByDescending(d => d.TotalPrice).ToList();


            this.dataGridView1.DataSource = x.ToList();
        }

        private void Frm作業_3_Load(object sender, EventArgs e)
        {

        }

        private void button38_Click_1(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo dirs = new System.IO.DirectoryInfo(@"c:\windows");
            FileInfo[] files = dirs.GetFiles();

            var groupedFiles = files
            .GroupBy(f => BigFile(f))
            .Select(g => new
            {
                大小 = g.Key,
                Files = g.OrderByDescending(f => f.Length)
            })
            .ToList();
            this.treeView1.Nodes.Clear();
            foreach (var group in groupedFiles)
            {
                TreeNode node = treeView1.Nodes.Add(group.大小.ToString());


                foreach (var item in group.Files)
                {


                    node.Nodes.Add(item.Name.ToString() + "  容量   " + item.Length.ToString() + "   Byte");
                }



            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {

            System.IO.DirectoryInfo dirs = new System.IO.DirectoryInfo(@"c:\windows");
            FileInfo[] files = dirs.GetFiles();

            var groupedFiles = files
            .GroupBy(f => BigFile(f))
            .OrderBy(g => g.Key)
            .Select(g => new
            {
                大小 = g.Key,
                Files = g.OrderByDescending(f => f.Length)
            })
            .ToList();
            treeView1.Nodes.Clear();
            foreach (var group in groupedFiles)
            {
                TreeNode node = treeView1.Nodes.Add(group.大小.ToString());

                foreach (var item in group.Files)
                {
                    node.Nodes.Add(item.Name.ToString() + "  容量   " + item.Length.ToString() + "   Byte");
                }
            }
        }
    }


}


