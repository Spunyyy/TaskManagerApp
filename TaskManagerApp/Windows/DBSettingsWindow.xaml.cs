using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TaskManagerApp.Windows
{
    /// <summary>
    /// Interaction logic for DBSettingsWindow.xaml
    /// </summary>
    public partial class DBSettingsWindow : Window
    {
        public DBSettingsWindow()
        {
            InitializeComponent();

            string settings = File.ReadAllText("data/database.json");

            JObject settingsJson = JObject.Parse(settings);

            databaseServerTxtBox.Text = settingsJson["DatabaseServer"].ToString();
            databaseNameTxtBox.Text = settingsJson["DatabaseName"].ToString();
            usernameTxtBox.Text = settingsJson["Username"].ToString();
            passwordTxtBox.Text = settingsJson["Password"].ToString();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void topBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            JObject jsonSettings = new JObject
            {
                {"DatabaseServer", databaseServerTxtBox.Text },
                {"DatabaseName", databaseNameTxtBox.Text },
                {"Username", usernameTxtBox.Text },
                {"Password", passwordTxtBox.Text }
            };

            File.WriteAllText("data/database.json", jsonSettings.ToString());

            MessageBox.Show("Settings saved. Restart application!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

        private void stornoButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
