using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
using TaskManagerApp.Model;
using TaskManagerApp.Windows;

namespace TaskManagerApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            foreach(User u in App.Users)
            {
                if(u.Email == emailTextBox.Text && u.Password == Utils.HashString(passwordBox.Password))
                {
                    MainWindow window = new MainWindow(u);
                    window.Show();
                    Close();
                    return;
                }
            }

            MessageBox.Show("Incorrect email or password!");
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

        private void settingButton_Click(object sender, RoutedEventArgs e)
        {
            DBSettingsWindow window = new DBSettingsWindow();
            window.Show();
            Close();
        }
    }
}
