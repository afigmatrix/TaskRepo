using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lesson14042023
{
    public partial class Form1 : Form
    {
        string connectionString = "Server=localhost;Database=Matrix132;Integrated Security=true;";
        int deleteUserId = 0;
        public Form1()
        {
            InitializeComponent();
            this.gv_Customer.SelectionChanged -= new System.EventHandler(this.gv_Customer_SelectionChanged);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand selectQuery = new SqlCommand("select * from Customer", conn);
                DataTable customerList = new DataTable();
                conn.Open();
                var sqlResult = selectQuery.ExecuteReader();
                customerList.Load(sqlResult);
                conn.Close();
                gv_Customer.DataSource = customerList;
            }
            this.gv_Customer.SelectionChanged += new System.EventHandler(this.gv_Customer_SelectionChanged);



            #region FileOperations


            //FileStream fs = File.Create(@"D:\DevGroup\Matrix132\Projects\test.txt");
            //fs.Close();
            //if (File.Exists(@"D:\DevGroup\Matrix132\Projects\test.txt"))
            //{
            //    //StreamWriter writer = new StreamWriter(@"D:\DevGroup\Matrix132\Projects\test.txt");
            //    //writer.WriteLine("Hi");
            //    //writer.Close();
            //    //StreamReader sr = new(@"D:\DevGroup\Matrix132\Projects\test.txt");
            //    //var result = sr.ReadToEnd();
            //}
            //else
            //{
            //    FileStream fs = File.Create(@"D:\DevGroup\Matrix132\Projects\test.txt");
            //    fs.Close();
            //}
            #endregion
            #region DirectoryOperations
            //try
            //{
            //    throw new ArgumentNullException("Argument is not valid");
            //}
            //catch (Exception ex)
            //{
            //    string exePath = Directory.GetCurrentDirectory();
            //    string fullPath = Path.Combine(exePath, "Log");
            //    string filePath = Path.Combine(fullPath, "Log14042023.txt");
            //    //OccuredExcep
            //    Directory.CreateDirectory(fullPath);
            //    DirectoryInfo di = new DirectoryInfo(filePath);
            //    FileStream fs = File.Create(filePath);
            //    StreamWriter sr = new StreamWriter(fs);
            //    sr.WriteLine(ex.Message);
            //    sr.Close();
            //    fs.Close();
            //}
            #endregion
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string Name = tbxCustomerName.Text;
            string Age = tbxCustomerAge.Text;
            string Citizenship = tbxCitizenship.Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand insertQuery = new SqlCommand($"insert into Customer values(@pName,@pAge,@pCS) ", conn);

                insertQuery.Parameters.AddWithValue("@pName", Name);
                insertQuery.Parameters.AddWithValue("@pAge", Age);
                insertQuery.Parameters.AddWithValue("@pCS", Citizenship);
                conn.Open();
                _ = insertQuery.ExecuteNonQuery();
                SqlCommand selectQuery = new SqlCommand("select * from Customer", conn);
                DataTable customerList = new DataTable();
                var sqlResult = selectQuery.ExecuteReader();
                customerList.Load(sqlResult);
                conn.Close();
                gv_Customer.DataSource = null;
                gv_Customer.DataSource = customerList;
            }


        }

        private void gv_Customer_SelectionChanged(object sender, EventArgs e)
        {
            int rowIndex = gv_Customer.CurrentCell.RowIndex;
            var custmerInfos = gv_Customer.Rows[rowIndex];
            string customerName = custmerInfos.Cells["CustomerName"].Value.ToString();
            deleteUserId = (int)custmerInfos.Cells["ID"].Value;
            tbxDeleteName.Text = customerName;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (deleteUserId > 0)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand deleteQuery = new SqlCommand($"delete  from Customer where ID = {deleteUserId}", conn);
                    conn.Open();
                   _=deleteQuery.ExecuteNonQuery();

                    SqlCommand selectQuery = new SqlCommand("select * from Customer", conn);
                    DataTable customerList = new DataTable();
                    var sqlResult = selectQuery.ExecuteReader();
                    customerList.Load(sqlResult);
                    conn.Close();
                    gv_Customer.DataSource = customerList;
                }
            }
        }
    }
}
