using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;
using System;
using System.IO;
using System.Windows.Forms;
using static tes.formConfigurateDataBase;

namespace tes
{
    class userInformation
    {
        public string user;
        public string pass;
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            UserInformation userInform = new UserInformation();
            string jsonFilePath = $"{Application.StartupPath}/dataBaseInformation.json";
            string jsonFilePaths = $"{Application.StartupPath}/dataInformation.json";
            if (File.Exists(jsonFilePaths) ) 
            {
                using (StreamReader sr = new StreamReader(jsonFilePaths))
                {
                    string jsonData = sr.ReadToEnd();
                    UserInformation data2 = JsonConvert.DeserializeObject<UserInformation>(jsonData);
                    userInform.User = data2.User;
                    userInform.Pass = data2.Pass;
                }
            }
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

                    formConfigurateDataBase form = new formConfigurateDataBase();
                    form.ReadDataInformJson();
                    // Lakukan sesuatu dengan data...
                }
            }
            else
            {
                Console.WriteLine("File not found: dataBaseInformation.json");
            }
            if(userInform.User != ""|| userInform.Pass != "")
            {
                Application.Run(new FormMain());
            }
            else
            {
                Application.Run(new FormLogin());
            }
        }
    }
}
