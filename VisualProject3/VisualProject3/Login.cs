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
using FootballVotingSystem;

namespace VisualProject3
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\faruk\\Desktop\\VisualProgramming\\VisualProject3\\VisualProject3\\bin\\Debug\\Logs.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter da = new OleDbDataAdapter();

        private void label4_Click(object sender, EventArgs e)
        {
            new Register().Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            OleDbCommand command = new OleDbCommand("SELECT * FROM Logs WHERE Username = '" + textBox1.Text + "' AND Password = '" + textBox2.Text + "'", conn);
            OleDbCommand command2 = new OleDbCommand("SELECT UserID FROM Logs WHERE Username = '" + textBox1.Text + "' AND Password = '" + textBox2.Text + "'", conn);
            command.Connection = conn;
            OleDbDataReader reader = command.ExecuteReader();
            if (reader.Read() == true)
            {
                Program.LoggedInUserID = Convert.ToInt32(reader["UserID"]);
                conn.Close();
                new Main(Program.LoggedInUserID).Show();
                this.Hide();
                
            }

            else
            {
                MessageBox.Show("Invalid Username or Password","Login failed",MessageBoxButtons.OK,MessageBoxIcon.Error);
                conn.Close();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox1.Focus();
            }
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox1.Focus();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            conn.Close();
            new AdminLogin().Show();
            this.Hide();
        }
    }
}
