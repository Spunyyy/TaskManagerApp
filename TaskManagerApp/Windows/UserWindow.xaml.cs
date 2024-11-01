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
using TaskManagerApp.Model.Enums;
using TaskManagerApp.Model;
using TaskManagerApp.Data;
using System.Windows.Interop;

namespace TaskManagerApp.Windows
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        private User user;
        private User copyUser = new User();

        private List<Role> roles = new List<Role>();

        private bool userEdit;

        public UserWindow(User user, bool userEdit)
        {
            InitializeComponent();
            loadRoles();
            this.user = user;
            copyUser.CopyFrom(user);
            this.userEdit = userEdit;
            DataContext = copyUser;

            roleComboBox.ItemsSource = roles;
            roleComboBox.SelectedValuePath = "Id";

            if (copyUser.Role != null)
            {
                roleComboBox.SelectedValue = copyUser.Role.Id;
            }
        }

        private void loadRoles()
        {
            using (TaskManagerContext context = new TaskManagerContext())
            {
                roles = context.Roles.ToList();
            }
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

        private void nameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "Name")
            {
                textBox.Text = "";
            }
        }

        private void nameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Name";
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void surnameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "Surname")
            {
                textBox.Text = "";
            }
        }

        private void surnameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Surname";
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void emailTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "example@example.com")
            {
                textBox.Text = "";
            }
        }

        private void emailTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "example@example.com";
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void stornoButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (nameTextBox.Text == "Name" || surnameTextBox.Text == "Surname" || emailTextBox.Text == "example@example.com" || string.IsNullOrEmpty(pwdBox.Password) || roleComboBox.SelectedItem == null)
            {
                return;
            }

            copyUser.Password = Utils.HashString(pwdBox.Password);

            copyUser.RoleId = ((Role)roleComboBox.SelectedItem).Id;

            if (userEdit)
            {
                copyUser.Role = ((Role)roleComboBox.SelectedItem);
            }

            copyUser.AccountCreated = DateTime.Today;
            copyUser.LastLogin = new DateTime(2000, 1, 1);

            if (copyUser.ArePropertiesChanged(user))
            {
                MessageBoxResult msgbxResult = MessageBox.Show("User has been changed. Do you want to save?", "Confirmation", MessageBoxButton.YesNo);
                if (msgbxResult == MessageBoxResult.Yes)
                {
                    user.CopyFrom(copyUser);
                }
                else
                {
                    DialogResult = false;
                    Close();
                }
            }

            DialogResult = true;
            Close();
        }
    }
}
