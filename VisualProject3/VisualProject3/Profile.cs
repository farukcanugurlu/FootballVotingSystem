using System;
using System.Data;
using System.Data.OleDb;
using System.Net.Mail;
using System.Windows.Forms;
using VisualProject3;

namespace FootballVotingSystem
{
    public partial class Profile : Form
    {
        private int loggedInUserID;
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\faruk\\Desktop\\VisualProgramming\\VisualProject3\\VisualProject3\\bin\\Debug\\Logs.mdb");

        public Profile(int loggedInUserID)
        {
            InitializeComponent();
            this.loggedInUserID = loggedInUserID;
            this.Load += Profile_Load;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void Profile_Load(object sender, EventArgs e)
        {
            string query = "SELECT * FROM Logs WHERE UserID = @UserID";
            this.WindowState = FormWindowState.Maximized;

            // OleDbCommand nesnesini tanımlayın
            OleDbCommand cmd = new OleDbCommand(query, conn);

            using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\faruk\\Desktop\\VisualProgramming\\VisualProject3\\VisualProject3\\bin\\Debug\\Logs.mdb"))
            {
                conn.Open();

                string usernameQuery = "SELECT Username FROM Logs WHERE UserID = @UserID";

                using (OleDbCommand cmd2 = new OleDbCommand(usernameQuery, conn))
                {
                    cmd2.Parameters.AddWithValue("@UserID", loggedInUserID);

                    // OleDbDataReader kullanarak sorguyu çalıştırın ve sonucu okuyun
                    using (OleDbDataReader reader = cmd2.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string usern = reader["Username"].ToString();
                            usern.ToUpper();
                            label1.Text = "Dear " + usern+": You can change your account details down here";
                        }
                        else
                        {
                            MessageBox.Show("Kullanıcı adı bulunamadı.");
                        }
                    }
                }
            }


            // @UserID parametresini ekleyin
            cmd.Parameters.AddWithValue("@UserID", loggedInUserID);

            // OleDbDataAdapter nesnesini tanımlayın
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);

            // DataSet oluşturun
            DataSet ds = new DataSet();

            try
            {
                conn.Open();
                da.Fill(ds, "Logs");

                dataGridView3.DataSource = ds.Tables["Logs"];
                dataGridView3.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri yüklenirken bir hata oluştu: " + ex.Message);
            }
            finally
            {
                conn.Close(); 
            }
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (OleDbConnection connUpdate = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\faruk\\Desktop\\VisualProgramming\\VisualProject3\\VisualProject3\\bin\\Debug\\Logs.mdb"))
                {
                    connUpdate.Open();

                    OleDbCommand command1 = new OleDbCommand("SELECT * FROM Logs WHERE Password = @Password", connUpdate);
                    command1.Parameters.AddWithValue("@Password", textBox2.Text);

                    using (OleDbDataReader reader = command1.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox7.Text))
                            {
                                MessageBox.Show("Credentials cannot be empty", "Update failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else if (textBox3.Text != textBox4.Text)
                            {
                                MessageBox.Show("Passwords must be the same", "Registration Failed", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                            }
                            else if (IsValidEmail(textBox7.Text))
                            {
                                OleDbTransaction transaction = connUpdate.BeginTransaction();
           
                                try
                                {
                                    OleDbCommand command = new OleDbCommand("UPDATE Logs SET [Password] = ?, Email = ? WHERE UserId = ?", connUpdate);
                                    command.Parameters.AddWithValue("@p1", textBox3.Text);
                                    command.Parameters.AddWithValue("@p2", textBox7.Text);
                                    command.Parameters.AddWithValue("@UserId", loggedInUserID);

                                    command.Transaction = transaction;
                                    command.ExecuteNonQuery();

                                    transaction.Commit();

                                    MessageBox.Show("Your account has been successfully updated", "Update Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    connUpdate.Close();
                                    this.Dispose();
                                    new Login().Show();
                                    this.Hide();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("An error occurred during the update: " + ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                finally
                                {
                                    connUpdate.Close();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Your email is not valid", "Registration Failed", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Your current password is wrong", "Update Failed", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            conn.Close();
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

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            new Login().Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            new Main(loggedInUserID).Show();
        }
    }
}
