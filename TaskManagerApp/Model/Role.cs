using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApp.Model.Enums;

namespace TaskManagerApp.Model
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = null!;
        public string Permissions { get; set; } = null!;

        [NotMapped]
        public ICollection<Permission> Perms { get; set; } = null!;

        public void serializePerms()
        {
            Permissions = string.Join(",", Perms);
            Perms.Clear();
        }

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
    }
}
