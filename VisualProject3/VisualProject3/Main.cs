using FootballVotingSystem;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace VisualProject3
{
    public partial class Main : Form
    {
        private int loggedInUserID;
        private Chart chart1;

        public Main(int userID)
        {
            InitializeComponent();
            this.loggedInUserID = userID;
            InitializeChart();
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void InitializeChart()
        {
            chart1 = new Chart();
            chart1.BackColor = Color.DarkCyan;
            chart1.Size = new System.Drawing.Size(515, 315);
            chart1.Location = new System.Drawing.Point(1200, 167);

            ChartArea chartArea1 = new ChartArea();
            chart1.ChartAreas.Add(chartArea1);

            Series series1 = new Series();
            series1.ChartType = SeriesChartType.Column;
            series1.Name = "Oy Dağılımı";
            chart1.Series.Add(series1);
            series1.SetCustomProperty("PixelPointWidth", "1");

            Legend legend = new Legend();
            chart1.Legends.Add(legend);

            Controls.Add(chart1);
            PopulateChartFromDatabase();
        }


        private void PopulateChartFromDatabase()
        {
            chart1.Series.Clear();

            using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\faruk\\Desktop\\VisualProgramming\\VisualProject3\\VisualProject3\\bin\\Debug\\Logs.mdb"))
            {
                conn.Open();

                string query = "SELECT NomineeID, Name, Votes FROM Nominees";
                using (OleDbCommand command = new OleDbCommand(query, conn))
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        List<(int nomineeID, string nomineeName, int votes)> nomineeList = new List<(int, string, int)>();

                        while (reader.Read())
                        {
                            int nomineeID = reader.GetInt32(0);
                            string nomineeName = reader.GetString(1);
                            int votes = reader.GetInt32(2);

                            while (chart1.Series.Count < nomineeID)
                            {
                                chart1.Series.Add("Aday " + (chart1.Series.Count + 1));
                            }

                            chart1.Series[nomineeID - 1].Name = nomineeName;
                            chart1.Series[nomineeID - 1].Points.AddXY("", votes);
                          
                        }
                    }
                }
            }
        }
    

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\faruk\\Desktop\\VisualProgramming\\VisualProject3\\VisualProject3\\bin\\Debug\\Logs.mdb"))
                {
                    conn.Open();

                    int selectedCandidateID = GetSelectedCandidateID(); 
                    bool hasVoted = CheckHasVoted(loggedInUserID);

                    if (selectedCandidateID != -1)
                    {
                        if (!hasVoted)
                        {
                            using (OleDbCommand updateVoteCountCommand = new OleDbCommand("UPDATE Nominees SET Votes = Votes + 1 WHERE NomineeID = ?", conn))
                            {
                                updateVoteCountCommand.Parameters.AddWithValue("@p1", selectedCandidateID);
                                updateVoteCountCommand.ExecuteNonQuery();
                            }
                            using (OleDbCommand updateHasVotedCommand = new OleDbCommand("UPDATE Logs SET HasVoted = True WHERE UserID = ?", conn))
                            {
                                updateHasVotedCommand.Parameters.AddWithValue("@p1", loggedInUserID);
                                updateHasVotedCommand.ExecuteNonQuery();
                            }

                            MessageBox.Show("Oy verme işlemi başarıyla tamamlandı.", "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                           
                            UpdateChart();
                        }
                        else
                        {
                            MessageBox.Show("Daha önce oy verdiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            UpdateChart();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Lütfen bir aday seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        UpdateChart();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateChart()
        {
            
            PopulateChartFromDatabase();
        }

        private int GetSelectedCandidateID()
        {
            for (int i = 0; i < 6; i++)
            {
                if (Controls.Find("radioButton" + (i + 1), true)[0] is RadioButton radioButton && radioButton.Checked)
                {
                    return i + 1;
                }

            }

            return -1;
        }

        private bool CheckHasVoted(int userID)
        {
            bool hasVoted = false;

            try
            {
                using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\faruk\\Desktop\\VisualProgramming\\VisualProject3\\VisualProject3\\bin\\Debug\\Logs.mdb"))
                {
                    conn.Open();

                    string checkHasVotedQuery = "SELECT HasVoted FROM Logs WHERE UserID = ?";
                    using (OleDbCommand checkHasVotedCommand = new OleDbCommand(checkHasVotedQuery, conn))
                    {
                        checkHasVotedCommand.Parameters.AddWithValue("@p1", userID);

                        object result = checkHasVotedCommand.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            hasVoted = (bool)result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return hasVoted;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            new Login().Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            new Profile(loggedInUserID).Show();
        }

        private async void Main_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            await LoadDataAsync();

        }
        private async Task LoadDataAsync()
        {
            await Task.Delay(3000); 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
