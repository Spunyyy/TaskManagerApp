using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Model.Enums;

namespace TaskManagerApp.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime AccountCreated { get; set; }
        public DateTime LastLogin { get; set; }
        public ICollection<Project> Projects { get; set; } = null!;

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

        public bool ArePropertiesChanged(User other)  
        {
            Type type = typeof(User);
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
