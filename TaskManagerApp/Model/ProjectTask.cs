using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Model.Enums;

namespace TaskManagerApp.Model
{
    public class ProjectTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public int AssignedUserId { get; set; }
        public User AssignedUser { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime? CompletionDate { get; set; }

        public void CopyFrom(object source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            PropertyInfo[] sourceProperties = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var sourceProperty in sourceProperties)
            {
                PropertyInfo targetProperty = this.GetType().GetProperty(sourceProperty.Name);

                if (targetProperty != null && targetProperty.CanWrite)
                {
                    object value = sourceProperty.GetValue(source);
                    targetProperty.SetValue(this, value);
                }
            }
        }

        public bool ArePropertiesChanged(ProjectTask other)
        {
            Type type = typeof(ProjectTask);
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {

                object thisValue = property.GetValue(this);
                object otherValue = property.GetValue(other);


                if (thisValue != otherValue)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
