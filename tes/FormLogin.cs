using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace tes
{
    public partial class FormLogin : Form
    {

        string server = DatabaseConfig.Instance.Server;
        string database = DatabaseConfig.Instance.DatabaseName;
        string uid = DatabaseConfig.Instance.UserId;
        string password = DatabaseConfig.Instance.Password;

        public FormLogin()
        {
            InitializeComponent();
        }


            private void FormLogin_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
    {
            if (user.Text == "database" && pass.Text == "config")
            {
                formConfigurateDataBase form = new formConfigurateDataBase();
                form.ShowDialog();
            }
            else
            {
                string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
                MySqlConnection connection = new MySqlConnection(connectionString);
                string query = "select id from staff where User = @user and Password = @pass";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    string users = user.Text;
                    string passz = pass.Text;

                    cmd.Parameters.AddWithValue("@user", users);
                    cmd.Parameters.AddWithValue("@pass", passz);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                {
                                    FormMain forms = new FormMain();
                                    MessageBox.Show("Berhasil Login");
                                    formConfigurateDataBase forms2 = new formConfigurateDataBase();
                                    forms2.writeDataInformJson(user.Text, pass.Text);
                                    forms.ShowDialog();
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Gagal Login");
                                }
                            }
                        }
                    }
                    connection.Close();
                }
            }
        }
    }
}
