using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApp.Model.Enums
{
    public enum Permission
    {
        Admin,
        CreateUser,
        ManageUsers,
        CreateProject,
        DeleteProject,
        AssignProject,
        AddTask,
        EditTask,
        AssignTask,
        DeleteTask
    }
}
