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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskManagerApp.Data;
using TaskManagerApp.Model;
using TaskManagerApp.Windows;

namespace TaskManagerApp.Pages
{
    /// <summary>
    /// Interaction logic for UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
            DataContext = App.Users;
            dateCreatedTxtBlock.Visibility = Visibility.Hidden;
            lastLoginTxtBlock.Visibility = Visibility.Hidden;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.getInstance().frame.Content = new MainPage();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            User user = (User)usersDataGrid.SelectedItem;
            UserWindow window = new UserWindow(user, true);
            window.ShowDialog();
            if (window.DialogResult == true)
            {
                using (TaskManagerContext context = new TaskManagerContext())
                {
                    var oldUser = context.Users
                                        .Where(u => u.Id == user.Id)
                                        .FirstOrDefault();
                    if (oldUser is User)
                    {
                        oldUser.CopyFrom(user);
                    }

                    context.SaveChanges();
                }
                App.loadUsers();
                usersDataGrid.Items.Refresh();
            }
        }
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            User user = (User)usersDataGrid.SelectedItem;

            MessageBoxResult msgbxResult = MessageBox.Show("Do you really want \ndelete user " + user.Name + " " + user.Surname + "?", "Confirmation", MessageBoxButton.YesNo);
            if (msgbxResult == MessageBoxResult.Yes)
            {
                using (TaskManagerContext context = new TaskManagerContext())
                {
                    var userDelete = context.Users
                                        .Where(u => u.Id == user.Id)
                                        .FirstOrDefault();
                    if (userDelete is User)
                    {
                        context.Remove(userDelete);
                    }

                    context.SaveChanges();
                }
                App.loadUsers();
                usersDataGrid.Items.Refresh();
            }

        }

        private void usersDataGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (usersDataGrid.SelectedItem != null)
            {
                dateCreatedTxtBlock.Visibility = Visibility.Visible;
                lastLoginTxtBlock.Visibility = Visibility.Visible;
                dateCreatedTxtBlock.Text = "Date created: " + ((User)usersDataGrid.SelectedItem).AccountCreated.ToShortDateString();
                lastLoginTxtBlock.Text = "Last login: " + ((User)usersDataGrid.SelectedItem).LastLogin.ToShortDateString();
            }
        }
    }
}
