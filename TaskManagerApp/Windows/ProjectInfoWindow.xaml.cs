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
    /// Interaction logic for ProjectInfoWindow.xaml
    /// </summary>
    public partial class ProjectInfoWindow : Window
    {
        private Project project;

        public ProjectInfoWindow(Project project)
        {
            InitializeComponent();
            this.project = project;
            loadTxtBlocks();
        }

        private void loadTxtBlocks()
        {
            titleTxtBlock.Text = project.Title;
            descTxtBlock.Text = project.Description;

            startTxtBlock.Text = project.StartDate.ToShortDateString(); 
            endTxtBlock.Text = project.EndDate.ToShortDateString(); 
            priorityTxtBlock.Text = project.Priority.ToString(); 
            statusTxtBlock.Text = project.Status.ToString();

            int createdTasks = 0;
            int progressTasks = 0;
            int finishedTasks = 0;
            foreach (ProjectTask task in project.Tasks)
            {
                if (task.Status == Status.Created)
                {
                    ++createdTasks;
                }
                else if (task.Status == Status.InProgress)
                {
                    ++progressTasks;
                }
                else if (task.Status == Status.Finished)
                {
                    ++finishedTasks;
                }
            }
            createdTxtBlock.Text = createdTasks.ToString();
            progressTxtBlock.Text = progressTasks.ToString();
            finishedTxtBlock.Text = finishedTasks.ToString();

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
    }
}
