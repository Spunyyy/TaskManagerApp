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
using TaskManagerApp.Data;
using TaskManagerApp.Model;

namespace TaskManagerApp.Windows
{
    /// <summary>
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private User user;

        public ChangePasswordWindow(User user)
        {
            InitializeComponent();
            this.user = user;
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

        private void stornoButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pwdBox.Password))
            {
                return;
            }
            string hashPassword = Utils.HashString(pwdBox.Password);
            using(TaskManagerContext context = new TaskManagerContext())
            {
                var userPwd = context.Users
                                .Where(u => u.Id == user.Id)
                                .FirstOrDefault();

                if(userPwd is User)
                {
                    userPwd.Password = hashPassword;
                }

                context.SaveChanges();
            }

            user.Password = hashPassword;

            Close();
        }
    }
}
