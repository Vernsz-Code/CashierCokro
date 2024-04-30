using MySql.Data.MySqlClient;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace tes
{
    public partial class FormDashboard : Form
    {
        string server = DatabaseConfig.Instance.Server;
        string database = DatabaseConfig.Instance.DatabaseName;
        string uid = DatabaseConfig.Instance.UserId;
        string password = DatabaseConfig.Instance.Password;


        public FormDashboard()
        {
            InitializeComponent();
        }

        void resetBtn()
        {
            btnHari.FillColor = System.Drawing.Color.White;
            btnHari.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            btnBulan.FillColor = System.Drawing.Color.White;
            btnBulan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            btnTahun.FillColor = System.Drawing.Color.White;
            btnTahun.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        private void btnHari_Click(object sender, EventArgs e)
        {
            resetBtn();
            btnHari.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(156)))), ((int)(((byte)(56)))));
            btnHari.ForeColor = System.Drawing.Color.White;
        }

        private void btnBulan_Click(object sender, EventArgs e)
        {
            resetBtn();
            btnBulan.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(156)))), ((int)(((byte)(56)))));
            btnBulan.ForeColor = System.Drawing.Color.White;
        }

        private void btnTahun_Click(object sender, EventArgs e)
        {
            resetBtn();
            btnTahun.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(156)))), ((int)(((byte)(56)))));
            btnTahun.ForeColor = System.Drawing.Color.White;
        }

        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            Penjualan();
            HPP();
        }

        private void Penjualan()
        {

            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            MySqlConnection connection = new MySqlConnection(connectionString);
            string query = "SELECT SUM(subtotal) as Penjualan, SUM(laba) as laba, SUM(retur) as retur from transaction WHERE DATE(tgl) = @tgl";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                string tgl = DatePicker.Value.ToString("yyyy-MM-dd");
                connection.Open();
                cmd.Parameters.AddWithValue("@tgl", tgl);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        decimal p = 0;
                        decimal d = 0;
                        int i = 0;
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(0) || !reader.IsDBNull(1) || !reader.IsDBNull(2))
                            {
                                p = reader.GetDecimal(0);
                                lbl_PENJUALAN.Text = p.ToString("C", new CultureInfo("ID-id"));
                                d = reader.GetDecimal(1);
                                lbl_LABA.Text = d.ToString("C", new CultureInfo("ID-id"));
                                i = reader.GetInt32(2);
                                lbl_RETUR.Text = i.ToString();
                            }
                            else
                            {
                                lbl_PENJUALAN.Text = "Rp0,00";
                                lbl_LABA.Text = "Rp0,00";
                                lbl_RETUR.Text = "0";
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("Data Tidak Ditemukan");
                    }
                }
            }
        }
        private void HPP()
        {

            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            MySqlConnection connection = new MySqlConnection(connectionString);
            string query = "SELECT SUM(product.beli * transaction.qty)\r\nFROM transaction \r\nINNER JOIN product ON transaction.kode = product.kode_brg WHERE DATE(transaction.tgl) = @tgl ;\r\n";
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                string tgl = DatePicker.Value.ToString("yyyy-MM-dd");
                connection.Open();
                cmd.Parameters.AddWithValue("@tgl", tgl);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        decimal p = 0;
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                p = reader.GetDecimal(0);
                                lbl_hpp.Text = p.ToString("C", new CultureInfo("ID-id"));
                            }
                            else
                            {

                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("Data Tidak Ditemukan");
                    }
                }
            }
        }
        private void FormDashboard_Load(object sender, EventArgs e)
        {
            DatePicker.Value = DateTime.Now;
        }
    }
}
