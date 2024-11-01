using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskManagerApp.Windows;
using TaskManagerApp.Model;
using TaskManagerApp.Pages;
using System.Data;
using TaskManagerApp.Data;

namespace TaskManagerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static User loggedUser;

        private static MainWindow instance;
        public MainWindow(User user)
        {
            InitializeComponent();
            instance = this;
            loggedUser = user;
        }
        public static MainWindow getInstance()
        {
            return instance;
        }

        public static User getLoggedUser()
        {
            return loggedUser;
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

        private void addUserItem_Click(object sender, RoutedEventArgs e)
        {
            User user = new User();
            UserWindow window = new UserWindow(user, false);
            window.ShowDialog();
            if (window.DialogResult == true)
            {
                using (TaskManagerContext context = new TaskManagerContext())
                {
                    context.Users.Add(user);

                    context.SaveChanges();
                }

                App.loadUsers();
            }
        }

        private void manageUserItem_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new UsersPage();
        }

        private void rolesItem_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new RolesPage();
        }

        private void chngPwdItem_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow window = new ChangePasswordWindow(loggedUser);
            window.ShowDialog();
        }

        private void addProjectsItem_Click(object sender, RoutedEventArgs e)
        {
            Project project = new Project();
            ProjectWindow window = new ProjectWindow(project);
            window.ShowDialog();
            if (window.DialogResult == true)
            {
                using (TaskManagerContext context = new TaskManagerContext())
                {
                    context.Projects.Add(project);

                    context.SaveChanges();
                }
            }
        }

        private void viewProjectsItem_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new ProjectsPage();
        }

        private void viewNotiItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}