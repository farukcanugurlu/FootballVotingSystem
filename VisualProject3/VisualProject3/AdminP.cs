using System;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Windows.Forms;

namespace FootballVotingSystem
{
    public partial class AdminP : Form
    {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\Logs.mdb");

        public AdminP()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            AddAdmin();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            AddUser();
        }

        private void AdminP_Load(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void AddAdmin()
        {
            string username = textBox7.Text;
            string password = textBox8.Text;

            try
            {
                conn.Open();
                string insertQuery2 = "INSERT INTO Admin (Username, [Password]) VALUES (?, ?)";
                OleDbCommand cmd2 = new OleDbCommand(insertQuery2, conn);
                cmd2.Parameters.AddWithValue("@p1", username);
                cmd2.Parameters.AddWithValue("@p2", password);

                cmd2.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            MessageBox.Show("Admin added successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            RefreshDataGridView();
        }

        private void AddUser()
        {
            try
            {
                conn.Open();
                string insertQuery = "INSERT INTO Logs (Username, [Password], Name, Surname, Birthdate, Email) VALUES (?, ?, ?, ?, ?, ?)";
                OleDbCommand cmd = new OleDbCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@p1", textBox1.Text);
                cmd.Parameters.AddWithValue("@p2", textBox2.Text);
                cmd.Parameters.AddWithValue("@p3", textBox3.Text);
                cmd.Parameters.AddWithValue("@p4", textBox4.Text);
                cmd.Parameters.AddWithValue("@p5", textBox5.Text);
                cmd.Parameters.AddWithValue("@p6", textBox6.Text);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            MessageBox.Show("User added successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshDataGridView();
        }

        private void RefreshDataGridView()
        {
            this.logsTableAdapter.Fill(this.logsDataSet.Logs);
          
            this.adminTableAdapter.Fill(this.logsDataSet.Admin);
        }
    }
}
