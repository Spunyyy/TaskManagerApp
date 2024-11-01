using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApp.Model
{
    public class History
    {
        public int Id { get; set; }
        public string EntityType { get; set; } = null!;
        public int EntityId { get; set; }
        public string ChangeType { get; set; } = null!;
        public string Description { get; set; } = null!;
        public User User { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
