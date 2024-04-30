using Microsoft.Reporting.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace tes
{
    public partial class FormCetakLabel : Form
    {
        string server = DatabaseConfig.Instance.Server;
        string database = DatabaseConfig.Instance.DatabaseName;
        string uid = DatabaseConfig.Instance.UserId;
        string password = DatabaseConfig.Instance.Password;

        public FormCetakLabel()
        {
            InitializeComponent();
        }
        DataSet dataSet2 = new DataSet();
        private void FormCetakLabel_Load(object sender, EventArgs e)
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            string query = "select nama_brg, jual, kode_brg from product";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                {
                    adapter.Fill(dataSet2, "dataSet2");
                }
            }

            ReportDataSource reportDataSource = new ReportDataSource("DataSet2", dataSet2.Tables["dataSet2"]);
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);

            reportViewer1.LocalReport.ReportPath = $"{Application.StartupPath}/Report3.rdlc";

            reportViewer1.SetPageSettings(new System.Drawing.Printing.PageSettings
            {
                Landscape = false,
                Margins = new System.Drawing.Printing.Margins(30, 0, 0, 0)
            });
            reportViewer1.ZoomMode = ZoomMode.PageWidth;
            reportViewer1.ZoomPercent = 75;

            this.reportViewer1.RefreshReport();
        }
    }
}
