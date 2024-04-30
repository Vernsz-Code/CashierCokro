namespace tes
{
    public class DatabaseConfig
    {
        // Properti-properti konfigurasi database
        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string User { get; set; }
        public string Pass { get; set; }

        // Properti statis untuk mengakses instansi tunggal
        private static DatabaseConfig _instance;
        public static DatabaseConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DatabaseConfig();
                }
                return _instance;
            }
        }
    }
}
