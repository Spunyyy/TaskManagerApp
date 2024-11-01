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
using TaskManagerApp.Model.Enums;

namespace TaskManagerApp.Windows
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        private ProjectTask task;

        private ProjectTask copyTask = new ProjectTask();

        private List<User> users;

        public TaskWindow(ProjectTask task, List<User> users)
        {
            InitializeComponent();
            this.task = task;
            this.users = users;
            copyTask.CopyFrom(task);
            DataContext = copyTask;
            usersDataGrid.ItemsSource = users;
            priorityComboBox.ItemsSource = new Priority[] { Priority.Low, Priority.Mid, Priority.High };
            if(copyTask.Title != null)
            {
                statusComboBox.ItemsSource = new Status[] { Status.Created, Status.InProgress, Status.Finished };
                priorityComboBox.SelectedItem = copyTask.Priority;
                statusComboBox.SelectedItem = copyTask.Status;
                userTextBlock.Text = copyTask.AssignedUser.Name + " " + copyTask.AssignedUser.Surname;
            }
            else
            {
                copyTask.DueDate = DateTime.Today;
                statusComboBox.Visibility = Visibility.Hidden;
                statusLabel.Visibility = Visibility.Hidden;
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

        private void stornoButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if(titleTextBox.Text == "Title" || descTextBox.Text == "Description" || dueDatePicker.SelectedDate == DateTime.Today || priorityComboBox.SelectedItem == null || string.IsNullOrEmpty(userTextBlock.Text))
            {
                return;
            }

            string[] str = userTextBlock.Text.Split(" ");
            foreach (User u in users)
            {
                if (u.Name == str[0] && u.Surname == str[1])
                {
                    copyTask.AssignedUserId = u.Id;
                }
            }

            if (copyTask.AssignedUserId == 0)
            {
                return;
            }

            copyTask.Priority = (Priority)priorityComboBox.SelectedItem;

            if(statusComboBox.SelectedItem == null)
            {
                copyTask.Status = Status.Created;
            }
            else
            {
                copyTask.Status = (Status)statusComboBox.SelectedItem;
            }

            if (task.DateCreated.Date.Year == 1)
            {
                task.DateCreated = DateTime.Today;
            }

            task.LastUpdated = DateTime.Today;

            if (copyTask.ArePropertiesChanged(task))
            {
                MessageBoxResult msgbxResult = MessageBox.Show("Task has been changed. Do you want to save?", "Confirmation", MessageBoxButton.YesNo);
                if (msgbxResult == MessageBoxResult.Yes)
                {
                    task.CopyFrom(copyTask);
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

        private void titleTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Title";
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void titleTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "Title")
            {
                textBox.Text = "";
            }
        }

        private void descTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Description";
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void descTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "Description")
            {
                textBox.Text = "";
            }
        }

        private void usersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(usersDataGrid.SelectedItem != null)
            {
                userTextBlock.Text = ((User)usersDataGrid.SelectedItem).Name + " " + ((User)usersDataGrid.SelectedItem).Surname;
            }
        }

        private void filterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<User> filteredUsers = new List<User>();
            foreach(User u in users)
            {
                if (u.Name.StartsWith(filterTextBox.Text))
                {
                    filteredUsers.Add(u);
                }
                else if (u.Surname.StartsWith(filterTextBox.Text))
                {
                    filteredUsers.Add(u);
                }
            }
            usersDataGrid.ItemsSource = filteredUsers;
        }
    }
}
