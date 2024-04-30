using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace tes
{
    public partial class FormDataBarang : Form
    {
        string server = DatabaseConfig.Instance.Server;
        string database = DatabaseConfig.Instance.DatabaseName;
        string uid = DatabaseConfig.Instance.UserId;
        string password = DatabaseConfig.Instance.Password;

        public FormDataBarang()
        {
            InitializeComponent();
        }

        private void loadData()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};AllowUserVariables=true;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            string query = @"select
                kode_brg,
                nama_brg
                from product;";
            connection.Open();
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                try
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dgv.Rows.Clear();
                            int no = 0;
                            while (reader.Read())
                            {
                                no++;
                                string kode_brg = reader.GetString(0);
                                string nama_brg = reader.GetString(1);

                                dgv.Rows.Add(no, kode_brg, nama_brg);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan (Master - 1): " + ex.Message);
                }
            }
        }

        private void FormDataBarang_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void SearchData()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};AllowUserVariables=true;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            string query = @"select
                kode_brg,
                nama_brg
                from product where nama_brg like @kode or kode_brg like @kode;";
            connection.Open();
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@kode", "%" + SEARCH.Text + "%");
                try
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dgv.Rows.Clear();
                            int no = 0;
                            while (reader.Read())
                            {
                                no++;
                                string kode_brg = reader.GetString(0);
                                string nama_brg = reader.GetString(1);

                                dgv.Rows.Add(no, kode_brg, nama_brg);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan (Master - 1): " + ex.Message);
                }
            }
        }

        private void SEARCH_TextChanged(object sender, EventArgs e)
        {
            SearchData();
        }
    }
}
