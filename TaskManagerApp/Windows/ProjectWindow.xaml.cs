using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for ProjectWindow.xaml
    /// </summary>
    public partial class ProjectWindow : Window
    {
        private Project project;
        private Project copyProject = new Project();
        public ProjectWindow(Project project)
        {
            InitializeComponent();
            this.project = project;
            copyProject.CopyFrom(project);
            DataContext = copyProject;
            priorityComboBox.ItemsSource = new Priority[] { Priority.Low, Priority.Mid, Priority.High };
            if(copyProject.Title != null)
            {
                statusComboBox.ItemsSource = new Status[] { Status.Created, Status.InProgress, Status.Finished };
                priorityComboBox.SelectedItem = copyProject.Priority;
                statusComboBox.SelectedItem = copyProject.Status;
            }
            else
            {
                copyProject.StartDate = DateTime.Today;
                copyProject.EndDate = DateTime.Today;
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
            DialogResult = false;
            Close();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (titleTextBox.Text == "Title" || descTextBox.Text == "Description" || startDatePicker.SelectedDate == DateTime.Today || endDatePicker.SelectedDate == DateTime.Today || priorityComboBox.SelectedItem == null)
            {
                return;
            }

            if(((DateTime)startDatePicker.SelectedDate) >= ((DateTime)endDatePicker.SelectedDate))
            {
                return;
            }

            copyProject.Priority = (Priority)priorityComboBox.SelectedItem;
            if (statusComboBox.SelectedItem == null)
            {
                copyProject.Status = Status.Created;
            }
            else
            {
                copyProject.Status = (Status)statusComboBox.SelectedItem;
            }

            if (copyProject.DateCreated.Date.Year == 1)
            {
                copyProject.DateCreated = DateTime.Today;
            }

            copyProject.LastUpdated = DateTime.Today;

            if (copyProject.ArePropertiesChanged(project))
            {
                MessageBoxResult msgbxResult = MessageBox.Show("Project has been changed. Do you want to save?", "Confirmation", MessageBoxButton.YesNo);
                if (msgbxResult == MessageBoxResult.Yes)
                {
                    project.CopyFrom(copyProject);
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
    }
}
