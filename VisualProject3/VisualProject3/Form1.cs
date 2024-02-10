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
using System.Net.Mail;

namespace VisualProject3
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\faruk\\Desktop\\VisualProgramming\\VisualProject3\\VisualProject3\\bin\\Debug\\Logs.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter da = new OleDbDataAdapter();

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    if(textBox1.Text == "" && textBox2.Text == "")
        //    {
        //        MessageBox.Show("Login credentials cannot be empty", "Register failed",MessageBoxButtons.OK,MessageBoxIcon.Error);
        //    }
        //    else
        //    {
        //        conn.Open();
        //        OleDbCommand command = new OleDbCommand("INSERT into Logs(Username,Password) values ('" + textBox1.Text + "','" + textBox2.Text + "')", conn);
        //        command.ExecuteNonQuery();
        //        conn.Close();

        //        MessageBox.Show("Your account has ben succesfully created","Registration Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
        //        new Login().Show();
        //    }
        //}

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(textBox6.Text) || string.IsNullOrEmpty(textBox7.Text))
            {
                MessageBox.Show("Login credentials cannot be empty", "Register failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(textBox2.Text == textBox3.Text)
            {
                if (IsValidEmail(textBox7.Text))
                {

                    conn.Open();
                
                    OleDbCommand command = new OleDbCommand("INSERT INTO Logs (Username, [Password],Name,Surname,Birthdate,Email) VALUES (?, ?, ? , ? ,? ,?)", conn);
                    command.Parameters.AddWithValue("@p1", textBox1.Text);
                    command.Parameters.AddWithValue("@p2", textBox2.Text);
                    command.Parameters.AddWithValue("@p3", textBox4.Text);
                    command.Parameters.AddWithValue("@p4", textBox5.Text);
                    command.Parameters.AddWithValue("@p5", textBox6.Text);
                    command.Parameters.AddWithValue("@p6", textBox7.Text);

                    command.ExecuteNonQuery();
                    conn.Close();

                    MessageBox.Show("Your account has been successfully created", "Registration Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    new Login().Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Your email is not valid","Registration Failed",MessageBoxButtons.OKCancel,MessageBoxIcon.Error);

                }


            }
            else if(textBox2.Text != textBox3.Text)
            {
                MessageBox.Show("Passwords must be the same", "Registration Failed", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                return;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked==true)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox1.Focus();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            new Login().Show();
            this.Hide();
        }

        private void Register_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
        }
    }
}
