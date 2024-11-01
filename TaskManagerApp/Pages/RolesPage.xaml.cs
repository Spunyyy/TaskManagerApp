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
    /// Interaction logic for RolesPage.xaml
    /// </summary>
    public partial class RolesPage : Page
    {
        private List<Role> roles { get; set; } = new List<Role>();

        public RolesPage()
        {
            InitializeComponent();
            loadRoles();
            DataContext = roles;
        }

        private void loadRoles()
        {
            roles.Clear();

            using(TaskManagerContext context = new TaskManagerContext())
            {
                var rolesContext = context.Roles
                                        .OrderBy(r => r.Id);

                foreach(Role r in rolesContext)
                {
                    roles.Add(r);
                }
            }

            rolesDataGrid.Items.Refresh();
        }


        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            Role role = (Role)rolesDataGrid.SelectedItem;
            RoleWindow window = new RoleWindow(role);
            window.ShowDialog();
            if (window.DialogResult == true)
            {
                using (TaskManagerContext context = new TaskManagerContext())
                {
                    var oldRole = context.Roles
                                        .Where(r => r.Id == role.Id)
                                        .FirstOrDefault();
                    if(oldRole is Role)
                    {
                        oldRole.CopyFrom(role);
                    }

                    context.SaveChanges();
                }
                loadRoles();
            }
        }
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            Role role = (Role)rolesDataGrid.SelectedItem;

            MessageBoxResult msgbxResult = MessageBox.Show("Do you really want \ndelete role " + role.RoleName + "?", "Confirmation", MessageBoxButton.YesNo);
            if(msgbxResult == MessageBoxResult.Yes)
            {
                using(TaskManagerContext context = new TaskManagerContext())
                {
                    var roleDelete = context.Roles
                                        .Where(r => r.Id == role.Id)
                                        .FirstOrDefault();
                    if(roleDelete is Role)
                    {
                        context.Remove(roleDelete);
                    }

                    context.SaveChanges();
                }

                loadRoles();
            }

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.getInstance().frame.Content = new MainPage();
        }

        private void createRoleButton_Click(object sender, RoutedEventArgs e)
        {
            Role role = new Role();
            RoleWindow window = new RoleWindow(role);
            window.ShowDialog();
            if(window.DialogResult == true)
            {
                using(TaskManagerContext context = new TaskManagerContext())
                {
                    context.Roles.Add(role);

                    context.SaveChanges();
                }

                loadRoles();
            }
        }

        
    }
}
