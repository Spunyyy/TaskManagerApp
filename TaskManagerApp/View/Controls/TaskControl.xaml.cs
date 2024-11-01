using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for TaskControl.xaml
    /// </summary>
    public partial class TaskControl : UserControl
    {
        private ProjectTask task;

        public TaskControl(ProjectTask task)
        {
            InitializeComponent();
            this.task = task;
            DataContext = task;
            setUser();
            setTxtBlocks();
        }

        private void setTxtBlocks()
        {
            dueDateTxtBlock.Text = "Due date: " + task.DueDate.ToShortDateString();
            priorityTxtBlock.Text = "Priority: " + task.Priority.ToString();
            statusTxtBlock.Text = "Status: " + task.Status.ToString();
            userTxtBlock.Text = "User: " + task.AssignedUser.Name + " " + task.AssignedUser.Surname;
        }

        private void setUser()
        {
            using(TaskManagerContext context = new TaskManagerContext())
            {
                var user = context.Users.Where(u => u.Id == task.AssignedUserId).FirstOrDefault();
                task.AssignedUser = user;
            }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            TaskWindow window = new TaskWindow(task, TaskPage.getProject().AssignedUsers.ToList());
            window.ShowDialog();
            if (window.DialogResult == true)
            {
                using (TaskManagerContext context = new TaskManagerContext())
                {
                    context.Entry(task).State = EntityState.Modified;
                    
                    context.SaveChanges();
                }
            }

            TaskPage.getInstance().taskPanel.Children.Clear();
            TaskPage.getProject().Tasks.Add(task);
            loadControls();
        }

        private void loadControls()
        {
            foreach (ProjectTask task in TaskPage.getProject().Tasks)
            {
                TaskControl control = new TaskControl(task);
                TaskPage.getInstance().taskPanel.Children.Add(control);
            }
        }
    }
}
