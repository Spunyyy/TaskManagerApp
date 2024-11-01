using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
using TaskManagerApp.Pages;
using TaskManagerApp.Model.Enums;

namespace TaskManagerApp.Windows
{
    /// <summary>
    /// Interaction logic for RoleWindow.xaml
    /// </summary>
    public partial class RoleWindow : Window
    {
        private Role role;

        public RoleWindow(Role role)
        {
            InitializeComponent();
            this.role = role;
            DataContext = role;
            if (!string.IsNullOrEmpty(role.Permissions))
            {
                loadCheckBoxes();
            }
        }

        private void loadCheckBoxes()
        {
            string[] perms = role.Permissions.Split(',');
            foreach(string s in perms)
            {
                switch (s)
                {
                    case "Admin":
                        adminCkBx.IsChecked = true;
                        break;
                    case "CreateUser":
                        createUserCkBx.IsChecked = true;
                        break;
                    case "ManageUsers":
                        managerUserCkBx.IsChecked = true;
                        break;
                    case "CreateProject":
                        createProjectCkBx.IsChecked = true;
                        break;
                    case "DeleteProject":
                        deleteProjectCkBx.IsChecked = true;
                        break;
                    case "AssignProject":
                        assignProjectCkBx.IsChecked = true;
                        break;
                    case "AddTask":
                        addTaskCkBx.IsChecked = true;
                        break;
                    case "EditTask":
                        editTaskCkBx.IsChecked = true;
                        break;
                    case "AssignTask":
                        assignTaskCkBx.IsChecked = true;
                        break;
                    case "DeleteTask":
                        deleteTaskCkBx.IsChecked = true;
                        break;
                }
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

        private void roleNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "role name")
            {
                textBox.Text = "";
            }
        }

        private void roleNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "role name";
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
            if(roleNameTextBox.Text == "role name")
            {
                return;
            }

            role.Perms = new List<Permission>();

            foreach(Object o in checkBoxGrid.Children)
            {
                if(o is CheckBox)
                {
                    if(((CheckBox)o).IsChecked == true)
                    {
                        switch (((CheckBox)o).Tag.ToString())
                        {
                            case "Admin":
                                role.Perms.Add(Permission.Admin);
                                break;
                            case "CreateUser":
                                role.Perms.Add(Permission.CreateUser);
                                break;
                            case "ManageUsers":
                                role.Perms.Add(Permission.ManageUsers);
                                break;
                            case "CreateProject":
                                role.Perms.Add(Permission.CreateProject);
                                break;
                            case "DeleteProject":
                                role.Perms.Add(Permission.DeleteProject);
                                break;
                            case "AssignProject":
                                role.Perms.Add(Permission.AssignProject);
                                break;
                            case "AddTask":
                                role.Perms.Add(Permission.AddTask);
                                break;
                            case "EditTask":
                                role.Perms.Add(Permission.EditTask);
                                break;
                            case "AssignTask":
                                role.Perms.Add(Permission.AssignTask);
                                break;
                            case "DeleteTask":
                                role.Perms.Add(Permission.DeleteTask);
                                break;
                        }
                    }
                }
            }

            role.serializePerms();
            role.RoleName = roleNameTextBox.Text;

            DialogResult = true;
            Close();
        }
    }
}
