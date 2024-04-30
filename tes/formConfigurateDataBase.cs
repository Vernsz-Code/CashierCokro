using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;

namespace tes
{
    public partial class formConfigurateDataBase : Form
    {

        public formConfigurateDataBase()
        {
            InitializeComponent();
        }

        public class DataInformation
        {
            public string Server { get; set; }
            public string Database { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
        }

        public class UserInformation
        {
            public string User { get; set; }
            public string Pass { get; set; }
        }

        private void writeDataJson()
        {
            DataInformation data1 = new DataInformation
            {
                Server = ServerBox.Text,
                Database = dataBaseBox.Text,
                Name = usernameBox.Text,
                Password = passwordBox.Text
            };

            string jsonFilePath = $"{Application.StartupPath}/dataBaseInformation.json";

            // Menggunakan using statement untuk memastikan bahwa stream file ditutup
            using (StreamWriter writer = new StreamWriter(jsonFilePath))
            {
                try
                {
                    var jsonWriter = JsonConvert.SerializeObject(data1, Formatting.Indented);
                    writer.Write(jsonWriter);
                    MessageBox.Show("Data Berhasil Disimpan!!!");
                }
                catch
                {
                    MessageBox.Show("Data Gagal Disimpan!!!");
                }
            }
        }
        public void writeDataInformJson(string user, string pass)
        {
            UserInformation data1 = new UserInformation
            {
                User = user,
                Pass = pass
            };

            string jsonFilePath = $"{Application.StartupPath}/dataInformation.json";

            // Menggunakan using statement untuk memastikan bahwa stream file ditutup
            using (StreamWriter writer = new StreamWriter(jsonFilePath))
            {
                try
                {
                    var jsonWriter = JsonConvert.SerializeObject(data1, Formatting.Indented);
                    writer.Write(jsonWriter);

                    DatabaseConfig.Instance.User = user;
                    DatabaseConfig.Instance.Pass = pass;
                }
                catch
                {
                    MessageBox.Show("Data Gagal Disimpan!!!");
                }
            }
        }


        private void ReadDataJson()
        {
            string jsonFilePath = $"{Application.StartupPath}/dataBaseInformation.json";

            if (File.Exists(jsonFilePath))
            {
                // Menggunakan using statement untuk memastikan bahwa stream file ditutup
                using (StreamReader reader = new StreamReader(jsonFilePath))
                {
                    string jsonData = reader.ReadToEnd();
                    DataInformation data = JsonConvert.DeserializeObject<DataInformation>(jsonData);

                    // Gunakan data sesuai kebutuhan Anda
                    string server = data.Server;
                    string database = data.Database;
                    string name = data.Name;
                    string password = data.Password;


                    DatabaseConfig.Instance.Server = server;
                    DatabaseConfig.Instance.DatabaseName = database;
                    DatabaseConfig.Instance.UserId = name;
                    DatabaseConfig.Instance.Password = password;
                }
            }
            else
            {
                Console.WriteLine("File not found: dataBaseInformation.json");
            }
        }
        public void ReadDataInformJson()
        {
            string jsonFilePath = $"{Application.StartupPath}/dataInformation.json";

            if (File.Exists(jsonFilePath))
            {
                // Menggunakan using statement untuk memastikan bahwa stream file ditutup
                using (StreamReader reader = new StreamReader(jsonFilePath))
                {
                    string jsonData = reader.ReadToEnd();
                    UserInformation data = JsonConvert.DeserializeObject<UserInformation>(jsonData);

                    DatabaseConfig.Instance.User = data.User;
                    DatabaseConfig.Instance.Pass = data.Pass;
                }
            }
            else
            {
                Console.WriteLine("File not found: dataBaseInformation.json");
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            writeDataJson();
            ReadDataJson();
        }
    }
}
