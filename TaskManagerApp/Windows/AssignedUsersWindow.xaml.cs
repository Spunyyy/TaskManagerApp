using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for AssignedUsersWindow.xaml
    /// </summary>
    public partial class AssignedUsersWindow : Window
    {
        private Project project;
        private List<User> unassignedUsers { get; set; } = new List<User>();
        private List<User> assignedUsers { get; set; } = new List<User>();

        public AssignedUsersWindow(Project project)
        {
            InitializeComponent();
            this.project = project;

            loadAssignedUsers();
            loadOtherUsers();

            assignedUsersDataGrid.ItemsSource = assignedUsers;
            unassignedUsersDataGrid.ItemsSource = unassignedUsers;
        }

        private void loadAssignedUsers()
        {
            foreach(User u in project.AssignedUsers)
            {
                assignedUsers.Add(u);
            }
        }

        private void loadOtherUsers()
        {
            bool addUser = true;
            foreach (User unassignedUser in App.Users)
            {
                foreach(User assignedUser in assignedUsers)
                {
                    if(unassignedUser.Id == assignedUser.Id)
                    {
                        addUser = false;
                    }
                }

                if (addUser)
                {
                    unassignedUsers.Add(unassignedUser);
                }
                addUser = true;
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

        private void unassignButton_Click(object sender, RoutedEventArgs e)
        {
            List<User> us = new List<User>();
            foreach(User u in assignedUsersDataGrid.SelectedItems)
            {
                us.Add(u);
                unassignedUsers.Add(u);
            }
            foreach (User u in us)
            {
                assignedUsers.Remove(u);
            }
            unassignedUsersDataGrid.Items.Refresh();
            assignedUsersDataGrid.Items.Refresh();
        }

        private void assignButton_Click(object sender, RoutedEventArgs e)
        {
            List<User> us = new List<User>();
            foreach (User u in unassignedUsersDataGrid.SelectedItems)
            {
                us.Add(u);
                assignedUsers.Add(u);
            }
            foreach (User u in us)
            {
                unassignedUsers.Remove(u);
            }
            unassignedUsersDataGrid.Items.Refresh();
            assignedUsersDataGrid.Items.Refresh();
        }

        private void stornoButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save changes?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                var currentUserIds = project.AssignedUsers.Select(u => u.Id).ToList();

                var usersToAdd = assignedUsers.Select(u => u.Id).Except(currentUserIds).ToList();

                var usersToRemove = currentUserIds.Except(assignedUsers.Select(u => u.Id)).ToList();

                using(TaskManagerContext context = new TaskManagerContext())
                {
                    var projectEdit = context.Projects
                                        .Where(p => p.Id == project.Id)
                                        .Include(p => p.AssignedUsers)
                                        .FirstOrDefault();

                    if (usersToAdd.Any())
                    {
                        var users = context.Users
                                    .Where(u => usersToAdd.Contains(u.Id))
                                    .Include(u => u.Role)
                                    .ToList();

                        foreach (var user in users)
                        {
                            project.AssignedUsers.Add(user);
                            projectEdit.AssignedUsers.Add(user);
                        }
                    }

                    if (usersToRemove.Any())
                    {
                        var users = project.AssignedUsers
                                        .Where(u => usersToRemove.Contains(u.Id))
                                        .ToList();

                        var usersEdit = projectEdit.AssignedUsers
                                        .Where(u => usersToRemove.Contains(u.Id))
                                        .ToList();

                        foreach (var user in users)
                        {
                            project.AssignedUsers.Remove(user);
                        }
                        foreach (var user in usersEdit)
                        {
                            projectEdit.AssignedUsers.Remove(user);
                        }
                    }



                    context.SaveChanges();
                }
            }

            Close();
        }
    }
}
