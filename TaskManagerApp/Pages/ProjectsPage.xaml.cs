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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskManagerApp.Data;
using TaskManagerApp.Model;
using TaskManagerApp.View.Controls;

namespace TaskManagerApp.Pages
{
    /// <summary>
    /// Interaction logic for ProjectsPage.xaml
    /// </summary>
    public partial class ProjectsPage : Page
    {
        private static List<Project> projects = new List<Project>();

        private static ProjectsPage instance;

        public ProjectsPage()
        {
            InitializeComponent();
            projects.Clear();
            loadProject();
            loadControls();
            instance = this;
        }

        public static List<Project> getProjects()
        {
            return projects;
        }

        public static ProjectsPage getInstance()
        {
            return instance;
        }

        private void loadProject()
        {
            using(TaskManagerContext context = new TaskManagerContext())
            {
                var projectsContext = context.Projects.Include(p => p.Tasks).ThenInclude(pt => pt.AssignedUser)
                                                        .Include(p => p.AssignedUsers).ThenInclude(u => u.Role).ToList();

                foreach(Project p in projectsContext)
                {
                    projects.Add(p);
                }
            }
        }

        private void loadControls()
        {
            foreach(Project p in projects)
            {
                ProjectControl control = new ProjectControl(p);
                projectsPanel.Children.Add(control);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.getInstance().frame.Content = new MainPage();
        }
    }
}
