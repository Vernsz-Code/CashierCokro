using Microsoft.Reporting.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace tes
{
    public partial class FormLabelHarga : Form
    {

        private static List<Stream> m_streams;
        private static int m_currentPageIndex = 0;

        string server = DatabaseConfig.Instance.Server;
        string database = DatabaseConfig.Instance.DatabaseName;
        string uid = DatabaseConfig.Instance.UserId;
        string password = DatabaseConfig.Instance.Password;
        public FormLabelHarga()
        {
            InitializeComponent();
        }
        private void SearchData()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};AllowUserVariables=true;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            string query = @"select
                kode_brg,
                nama_brg,
                jual,
                cl
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
                                decimal hargaJual = Convert.ToDecimal(reader["jual"]);
                                string jual = hargaJual.ToString("N0", new CultureInfo("id-ID"));
                                bool active = Convert.ToBoolean(reader["cl"]);
                                string str = "";
                                if (active)
                                {
                                    str = "Hapus";
                                }
                                else
                                {
                                    str = "Tambah";
                                }
                                dgv.Rows.Add(no, kode_brg, nama_brg, jual, active, str);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan (Master - 2): " + ex.Message);
                }
            }
        }
        private void checkDataCount()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};AllowUserVariables=true;";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Ganti "your_table" dengan nama tabel yang ingin Anda hitung jumlah datanya
                    string tableName = "product";

                    // Query untuk menghitung jumlah data dalam tabel
                    string countQuery = $"SELECT COUNT(*) FROM {tableName} where cl = true";

                    using (MySqlCommand cmd = new MySqlCommand(countQuery, connection))
                    {
                        int rowCount = Convert.ToInt32(cmd.ExecuteScalar());
                        Console.WriteLine($"Jumlah data dalam tabel {tableName}: {rowCount}");
                        label2.Text = rowCount.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void loadData()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};AllowUserVariables=true;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            string query = @"select
                kode_brg,
                nama_brg,
                jual,
                cl
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
                                decimal hargaJual = Convert.ToDecimal(reader["jual"]);
                                string jual = hargaJual.ToString("N0", new CultureInfo("id-ID"));
                                bool active = Convert.ToBoolean(reader["cl"]);
                                string str = "";
                                if (active)
                                {
                                    str = "Hapus";
                                }
                                else
                                {
                                    str = "Tambah";
                                }
                                dgv.Rows.Add(no, kode_brg, nama_brg, jual, active, str);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan (Master - 1): " + ex.Message);
                }
            }
            connection.Close();
        }
        private void FormLabelHarga_Load(object sender, EventArgs e)
        {
            loadData();
            checkDataCount();
        }

        private void SEARCH_TextChanged(object sender, EventArgs e)
        {
            SearchData();
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};AllowUserVariables=true;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            int coutCheck = int.Parse(label2.Text);
            // Use named parameters in the insert query
            string insertQuery = @"update product set cl = true where kode_brg = @kode";
            string deleteQuery = @"update product set cl = false where kode_brg = @kode";

            // Check if the clicked cell is in the checkbox column and is not a header
            if (e.ColumnIndex == dgv.Columns["column3"].Index && e.RowIndex >= 0)
            {
                // Toggle the value of the checkbox
                dgv.Rows[e.RowIndex].Cells["checkbox"].Value = !(bool)dgv.Rows[e.RowIndex].Cells["checkbox"].Value;
                connection.Open();

                // Optionally, you can perform additional actions based on the checkbox state
                if ((bool)dgv.Rows[e.RowIndex].Cells["checkbox"].Value)
                {
                    dgv.Rows[e.RowIndex].Cells["column3"].Value = "Hapus";

                    coutCheck += 1;
                    label2.Text = coutCheck.ToString();
                    string kode = dgv.Rows[e.RowIndex].Cells["column1"].Value.ToString();
                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, connection))
                    {
                        // Corrected the query parameters and added missing comma in the query
                        cmd.Parameters.AddWithValue("@kode", kode);
                        cmd.Parameters.AddWithValue("@active", true);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    dgv.Rows[e.RowIndex].Cells["column3"].Value = "Tambah";

                    coutCheck -= 1;
                    label2.Text = coutCheck.ToString();
                    string kode = dgv.Rows[e.RowIndex].Cells["column1"].Value.ToString();
                    using (MySqlCommand cmd = new MySqlCommand(deleteQuery, connection))
                    {
                        // Corrected the query parameters and added missing comma in the query
                        cmd.Parameters.AddWithValue("@kode", kode);
                        cmd.Parameters.AddWithValue("@active", true);
                        cmd.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }
        }


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            cetakLabelHarga();
        }


        private void cetakLabelHarga()
        {
            DataSet dataSet3 = new DataSet();
            ReportViewer reportViewer1 = new ReportViewer();
            string connectionStrings = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            string query1 = @"SELECT kode_brg, nama_brg, jual FROM product WHERE cl = true LIMIT @limit OFFSET 0;";
            string query2 = @"SELECT kode_brg, nama_brg, jual FROM product WHERE cl = true LIMIT @limit OFFSET @offset1;";
            string query3 = @"SELECT kode_brg, nama_brg, jual FROM product WHERE cl = true LIMIT @limit OFFSET @offset2;";
            string countQuery = $"SELECT COUNT(*) FROM product WHERE cl = true";
            decimal totalLength = 0;
            int result1 = 0;
            int result2 = 0;
            int result3 = 0;
            decimal lengthq1, lengthq2, lengthq3;

            using (MySqlConnection connections = new MySqlConnection(connectionStrings))
            {
                connections.Open();

                // Get the total length
                using (MySqlCommand cmd = new MySqlCommand(countQuery, connections))
                {
                    totalLength = Convert.ToDecimal(cmd.ExecuteScalar());
                }

                // Calculate the percentages
                lengthq1 = totalLength * 0.33m;
                result1 = Convert.ToInt32(Math.Ceiling(lengthq1));
                lengthq2 = totalLength * 0.66m;
                result2 = Convert.ToInt32(Math.Ceiling(lengthq2));
                lengthq3 = totalLength * 1.0m;
                result3 = Convert.ToInt32(Math.Ceiling(lengthq3));

                // Fetch the main data
                using (MySqlCommand command = new MySqlCommand(query1, connections))
                {
                    command.Parameters.AddWithValue("@limit", result1);
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(dataSet3, "DataTable1");
                    }
                }

                using (MySqlCommand command = new MySqlCommand(query2, connections))
                {
                    command.Parameters.AddWithValue("@limit", result2 - result1);
                    command.Parameters.AddWithValue("@offset1", result1);
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(dataSet3, "DataTable2");
                    }
                }

                using (MySqlCommand command = new MySqlCommand(query3, connections))
                {
                    command.Parameters.AddWithValue("@limit", result3 - result2);
                    command.Parameters.AddWithValue("@offset2", result2);
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(dataSet3, "DataTable3");
                    }
                }
            }

            ReportDataSource reportDataSource = new ReportDataSource("DataSet31", dataSet3.Tables["DataTable1"]);
            ReportDataSource reportDataSource1 = new ReportDataSource("DataSet32", dataSet3.Tables["DataTable2"]);
            ReportDataSource reportDataSource2 = new ReportDataSource("DataSet33", dataSet3.Tables["DataTable3"]);

            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            reportViewer1.LocalReport.DataSources.Add(reportDataSource2);

            reportViewer1.LocalReport.ReportPath = $"{Application.StartupPath}/Report5.rdlc";

            reportViewer1.SetPageSettings(new System.Drawing.Printing.PageSettings
            {
                Landscape = false
            });
            reportViewer1.ZoomMode = ZoomMode.PageWidth;
            reportViewer1.ZoomPercent = 75;

            rdlcPrint.Print(reportViewer1.LocalReport);
        }




        private void deleteAll()
        {
            string connection = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};AllowUserVariables=true;";

            MySqlConnection conn = new MySqlConnection(connection);

            string query = @"update product set cl = false";

            conn.Open();

            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.ExecuteNonQuery();

            conn.Close();

            MessageBox.Show("Semua Data Telah Berhasil diHapus!!!");
        }

        private void insertALL()
        {
            string connection = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};AllowUserVariables=true;";

            MySqlConnection conn = new MySqlConnection(connection);

            string query = @"update product set cl = true";

            conn.Open();

            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.ExecuteNonQuery();

            conn.Close();

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            insertALL();
            checkDataCount();
            loadData();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            deleteAll();
            checkDataCount();
            loadData();
        }

    }
}
