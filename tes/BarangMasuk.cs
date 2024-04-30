using MySql.Data.MySqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace tes
{
    public partial class BarangMasuk : Form
    {
        string server = DatabaseConfig.Instance.Server;
        string database = DatabaseConfig.Instance.DatabaseName;
        string uid = DatabaseConfig.Instance.UserId;
        string password = DatabaseConfig.Instance.Password;


        public BarangMasuk()
        {
            InitializeComponent();
        }

        private void LoadLaporan()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            string query = "SELECT * FROM transaction_in WHERE payment = 'tunai' and DATE(tgl) = @tgl";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    string tglv = STARTDATE.Value.ToString("yyyy-MM-dd");
                    cmd.Parameters.AddWithValue("@tgl", tglv);
                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            dgv.Rows.Clear();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string ID = reader["id"].ToString();
                                    string No_faktur = reader["no_faktur"].ToString();
                                    DateTime Tgls = Convert.ToDateTime(reader["tgl"]);
                                    string Kode = reader["kode"].ToString();
                                    string Nama = reader["nama"].ToString();
                                    int Qty = Convert.ToInt32(reader["qty"]);
                                    string Supplier = reader["suplier"].ToString();
                                    string Payment = reader["payment"].ToString();
                                    decimal harga = Convert.ToDecimal(reader["harga"]);
                                    string Tgl = Tgls.ToString("yyyy-MM-dd");
                                    string hargaStr = harga.ToString("N0");
                                    Image deleteIcon = Properties.Resources.icons8_delete_24px_1;

                                    dgv.Rows.Add(ID, No_faktur, Tgl, Kode, Nama, Qty, hargaStr, Supplier, Payment, deleteIcon);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                    }
                }
            }
        }

        private void updateStock(int qty, string kode)
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            string query = "UPDATE product set masuk = masuk - @qty where kode_brg = @kode";

            MySqlConnection conn = new MySqlConnection(connectionString);

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                try
                {
                    cmd.Parameters.AddWithValue("@qty", qty);
                    cmd.Parameters.AddWithValue("@kode", kode);
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan", ex.Message);
                }
                conn.Close();
            }
        }
        private void DeleteDataProduk(int id)
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            string query = "DELETE FROM transaction_in where id = @id";
            MySqlConnection conn = new MySqlConnection(connectionString);
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                try
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        dgv.Rows.RemoveAt(dgv.SelectedRows[0].Index);
                        MessageBox.Show("Data berhasil dihapus dan stok sudah di Pulihkan", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadLaporan();
                    }
                    else
                    {
                        MessageBox.Show("Data tidak ditemukan atau gagal dihapus");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi Kesalahan ketika menghapus data!", ex.Message);
                }
            }

        }
        private void BarangMasuk_Load(object sender, EventArgs e)
        {
            STARTDATE.Value = DateTime.Now;
        }

        private void STARTDATE_ValueChanged(object sender, EventArgs e)
        {
            LoadLaporan();
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (MessageBox.Show("Apakah Anda Yakin Ingin Menghapus?", "Hapus", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (e.ColumnIndex == dgv.Columns["DeleteIcon"].Index && e.RowIndex >= 0)
                {
                    int IDToDelete = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["ID"].Value);
                    int retur = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["Column5"].Value);
                    string kodeBarang = dgv.Rows[e.RowIndex].Cells["Column3"].Value.ToString();
                    updateStock(retur, kodeBarang);
                    DeleteDataProduk(IDToDelete);
                }
            }
        }

        private void ExportToExcel(DataGridView dgv, string filePath)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            FileInfo excelFile = new FileInfo(filePath);

            using (ExcelPackage package = new ExcelPackage(excelFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Data");

                // Kolom yang diinginkan
                string[] desiredColumns = { "No", "Tanggal", "Kode Barang", "Qty" };

                // Menambahkan header kolom yang diinginkan ke worksheet
                for (int i = 0; i < desiredColumns.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = desiredColumns[i];
                }

                // Menambahkan data ke worksheet
                for (int row = 0; row < dgv.Rows.Count; row++)
                {
                    for (int col = 0; col < dgv.Columns.Count; col++)
                    {
                        string columnName = dgv.Columns[col].HeaderText;

                        // Menambahkan data hanya jika kolom ada dalam kolom yang diinginkan
                        if (desiredColumns.Contains(columnName))
                        {
                            int colIndex = Array.IndexOf(desiredColumns, columnName) + 1;
                            worksheet.Cells[row + 2, colIndex].Value = dgv.Rows[row].Cells[col].Value;
                        }
                    }
                }

                // Mengatur lebar kolom
                for (int i = 1; i <= desiredColumns.Length; i++)
                {
                    worksheet.Column(i).AutoFit();
                }

                // Membuat tabel dan menerapkan gaya tabel
                var dataRange = worksheet.Cells["A1:" + worksheet.Cells[worksheet.Dimension.End.Row, desiredColumns.Length].Address];
                var tbl = worksheet.Tables.Add(dataRange, "MyTable");
                tbl.TableStyle = TableStyles.Medium2;

                package.Save();
            }
        }


        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save as Excel File";
            saveFileDialog.Filter = "Excel Files|*.xlsx";
            saveFileDialog.DefaultExt = "xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                ExportToExcel(dgv, filePath);
                MessageBox.Show("Data berhasil diekspor ke Excel.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SEARCH_TextChanged(object sender, EventArgs e)
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            string query = "SELECT * FROM transaction_in WHERE payment = 'tunai' and DATE(tgl) = @tgl and nama like @kode or payment = 'tunai' and DATE(tgl) = @tgl and kode like @kode";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    string tglv = STARTDATE.Value.ToString("yyyy-MM-dd");
                    cmd.Parameters.AddWithValue("@tgl", tglv);
                    cmd.Parameters.AddWithValue("@kode", "%" + SEARCH.Text + "%");
                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            dgv.Rows.Clear();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string ID = reader["id"].ToString();
                                    string No_faktur = reader["no_faktur"].ToString();
                                    DateTime Tgls = Convert.ToDateTime(reader["tgl"]);
                                    string Kode = reader["kode"].ToString();
                                    string Nama = reader["nama"].ToString();
                                    int Qty = Convert.ToInt32(reader["qty"]);
                                    string Supplier = reader["suplier"].ToString();
                                    string Payment = reader["payment"].ToString();
                                    decimal harga = Convert.ToDecimal(reader["harga"]);
                                    string Tgl = Tgls.ToString("yyyy-MM-dd");
                                    string hargaStr = harga.ToString("N0");
                                    Image deleteIcon = Properties.Resources.icons8_delete_24px_1;

                                    dgv.Rows.Add(ID, No_faktur, Tgl, Kode, Nama, Qty, hargaStr, Supplier, Payment, deleteIcon);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                    }
                }
            }
        }
    }
}
