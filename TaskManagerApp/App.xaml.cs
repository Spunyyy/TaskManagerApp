using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data;
using System.Windows;
using TaskManagerApp.Data;
using TaskManagerApp.Model;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using Microsoft.Data.SqlClient;

namespace TaskManagerApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static List<User> Users = new List<User>();
        public static string connectionString = "";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (!loadDBSettings())
            {
                MessageBox.Show("Database settings are not loaded!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            loadUsers();
        }

        public static void loadUsers()
        {
            Users.Clear();
            using (TaskManagerContext context = new TaskManagerContext())
            {
                foreach (User u in context.Users.Include(u => u.Role).ToList())
                {
                    Users.Add(u);
                }
            }
        }

        private bool loadDBSettings()
        {
            string filePath = "data/database.json";

            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory("data");
                File.Create(filePath).Close();

                JObject dataSettings = new JObject
                {
                    {"DatabaseServer", "ipAdress" },
                    {"DatabaseName", "TestDB" },
                    {"Username", "admin" },
                    {"Password", "password" },
                };

                File.WriteAllText(filePath, dataSettings.ToString());

                return false;
            }
            string settings = File.ReadAllText(filePath);

            JObject jsonSettings = JObject.Parse(settings);
            if (jsonSettings["DatabaseServer"].ToString() == "ipAdress")
            {
                return false;
            }

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = jsonSettings["DatabaseServer"].ToString();
            builder.InitialCatalog = jsonSettings["DatabaseName"].ToString();
            builder.UserID = jsonSettings["Username"].ToString();
            builder.Password = jsonSettings["Password"].ToString();
            connectionString = builder.ConnectionString;

            return true;
        }
    }
}
