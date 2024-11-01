using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApp.Model
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public User User { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Read { get; set; }
    }
}
