using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace FootballVotingSystem
{
    public partial class AdminLogin : Form
    {
        public AdminLogin()
        {
            InitializeComponent();
        }

        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\faruk\\Desktop\\VisualProgramming\\VisualProject3\\VisualProject3\\bin\\Debug\\Logs.mdb");

        private void button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            OleDbCommand command = new OleDbCommand("SELECT * FROM Admin WHERE Username = '" + textBox1.Text + "' AND Password = '" + textBox2.Text + "'", conn);
            command.Connection = conn;
            OleDbDataReader reader = command.ExecuteReader();

            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text)){
                MessageBox.Show("Login credentials cannot be empty", "Register failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            else if (reader.Read() == true)
            {
                conn.Close();
                new AdminP().Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid Username or Password", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text = "";
                textBox2.Text = "";
                textBox1.Focus();
                conn.Close();
            }
            conn.Close();
        }
    }
}
