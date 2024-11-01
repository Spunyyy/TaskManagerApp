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
using TaskManagerApp.Windows;

namespace TaskManagerApp.Pages
{
    /// <summary>
    /// Interaction logic for TaskPage.xaml
    /// </summary>
    public partial class TaskPage : Page
    {
        private static Project Project;

        private static TaskPage instance;

        public TaskPage(Project project)
        {
            InitializeComponent();
            Project = project;
            loadControls();
            instance = this;
        }

        public static TaskPage getInstance()
        {
            return instance;
        }

        public static Project getProject()
        {
            return Project;
        }

        private void loadControls()
        {
            foreach (ProjectTask task in Project.Tasks)
            {
                TaskControl control = new TaskControl(task);
                taskPanel.Children.Add(control);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.getInstance().frame.Content = new ProjectsPage();
        }

        private void addTaskButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectTask task = new ProjectTask();
            TaskWindow window = new TaskWindow(task, Project.AssignedUsers.ToList());
            window.ShowDialog();
            if(window.DialogResult == true)
            {
                task.ProjectId = Project.Id;
                using (TaskManagerContext context = new TaskManagerContext())
                {
                    context.Tasks.Add(task);

                    context.SaveChanges();
                }

                TaskControl control = new TaskControl(task);
                taskPanel.Children.Add(control);
            }
        }
    }
}
