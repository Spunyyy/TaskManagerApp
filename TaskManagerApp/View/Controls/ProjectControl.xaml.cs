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
using TaskManagerApp.Model.Enums;
using TaskManagerApp.Pages;
using TaskManagerApp.Windows;

namespace TaskManagerApp.View.Controls
{
    /// <summary>
    /// Interaction logic for ProjectControl.xaml
    /// </summary>
    public partial class ProjectControl : UserControl
    {
        private Project project;
        public ProjectControl(Project project)
        {
            InitializeComponent();
            this.project = project;
            DataContext = project;
            setLabel();
            progress();
            deadLineTxtBlock.Text = "Deadline: " + project.EndDate.ToShortDateString();
        }

        private void progress()
        {
            if(project.Tasks.Count == 0)
            {
                return;
            }
            taskProgessBar.Maximum = taskProgessBar.Width;
            taskProgessBar.Value = taskProgessBar.Width / project.Tasks.Count * getDoneTasks();
        }

        private void setLabel()
        {
            tasksTxtBlock.Text = "Tasks done: " + getDoneTasks() + "/" + project.Tasks.Count;
        }

        private int getDoneTasks()
        {
            int counter = 0;
            foreach (ProjectTask t in project.Tasks)
            {
                if(t.Status == Status.Finished)
                {
                    ++counter;
                }
            }
            return counter;
        }

        private void tasksButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.getInstance().frame.Content = new TaskPage(project);
        }

        private void usersButton_Click(object sender, RoutedEventArgs e)
        {
            AssignedUsersWindow window = new AssignedUsersWindow(project);
            window.ShowDialog();
        }

        private void infoButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectInfoWindow window = new ProjectInfoWindow(project);
            window.ShowDialog();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectWindow window = new ProjectWindow(project);
            window.ShowDialog();
            if(window.DialogResult == true)
            {
                using (TaskManagerContext context = new TaskManagerContext())
                {
                    context.Entry(project).State = EntityState.Modified;

                    context.SaveChanges();
                }
            }

            ProjectsPage.getInstance().projectsPanel.Children.Clear();

            foreach (Project p in ProjectsPage.getProjects())
            {
                ProjectControl control = new ProjectControl(p);
                ProjectsPage.getInstance().projectsPanel.Children.Add(control);
            }
        }
    }
}
